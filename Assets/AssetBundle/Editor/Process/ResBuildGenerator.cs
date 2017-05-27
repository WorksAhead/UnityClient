using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using ArkCrossEngine;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

namespace ArkCrossEngine
{
  public class ResBuildData
  {
    public int m_Id;
    public string m_ResourcesName;
    public string m_TargetName;
    public string m_ResourcesShortName;
    public string m_MD5;
    public long m_Size;
    public override string ToString()
    {
      return string.Format("Id:{0} Resources:{1} Resources:{2} TargetName:{3} MD5:{4} Size:{5}",
        m_Id,
        m_ResourcesName,
        m_TargetName,
        m_MD5,
        m_Size);
    }
  }
  public class ResBuildGenerator
  {
    private static Regex s_NameRegex = new Regex(@"^[A-Za-z0-9\/._]+$");
    private static Dictionary<string, ResBuildData> s_CurContainer = new Dictionary<string, ResBuildData>();
    private static int s_IdGen = 0;
    private static bool s_IsContainerNew = false;

    public static Dictionary<string, ResBuildData> GetContainer(bool isReGen = false)
    {
      if (isReGen || !s_IsContainerNew) {
        GenAllResBuildData();
      }
      return s_CurContainer;
    }
    public static bool GenAllResBuildData()
    {
      s_IdGen = 0;
      s_CurContainer.Clear();
      for (int index = 0; index < ResBuildConfig.ResBuildDirConfigsCount; index++) {
        ResBuildDirConfig config = ResBuildConfig.ResBuildDirConfigs[index];
        if (config != null) {
          GenResBuildDataByConfig(config);
        }
      }
      s_IsContainerNew = true;
      ResBuildLog.Info("ResBuildGenerator GenAllResBuildData Success");
      return true;
    }
    public static bool GenResBuildData(UnityEngine.Object selObj)
    {
      s_IdGen = 0;
      s_CurContainer.Clear();
      if (selObj == null) {
        ResBuildLog.Warn("GenBuildConfig GenResBuildData null");
        return false;
      }
      string selAssetPath = AssetDatabase.GetAssetPath(selObj);
      if (string.IsNullOrEmpty(selAssetPath)) {
        ResBuildLog.Warn("GenBuildConfig GenBuildConfig GetAssetPath null");
        return false;
      }

      if (ResBuildHelper.IsDirectoryRes(selAssetPath)) {
        GenResBuildDataByDir(selAssetPath, new string[] { "*.*" });
      } else {
        GenResBuildDataByFile(selAssetPath);
      }
      s_IsContainerNew = true;
      ResBuildLog.Info("ResBuildGenerator GenResBuildData Success");
      return true;
    }
    private static bool GenResBuildDataByConfig(ResBuildDirConfig config)
    {
      string[] tResBuildDirs = config.ResBuildDirs.Split(ResBuildConfig.ConfigSplit, StringSplitOptions.RemoveEmptyEntries);
      if (tResBuildDirs == null) {
        ResBuildLog.Warn("GenBuildConfig ResBuildDirs error:" + config.ResBuildDirs);
        return false;
      }
      string[] tResBuildPattern = config.ResBuildPattern.Split(ResBuildConfig.ConfigSplit, StringSplitOptions.RemoveEmptyEntries);
      if (tResBuildPattern == null) {
        ResBuildLog.Warn("SearchDependencyByDir ResBuildPattern error:" + config.ResBuildPattern);
        return false;
      }
      foreach (string dir in tResBuildDirs) {
        if (ResBuildHelper.IsDirectoryRes(dir)) {
          GenResBuildDataByDir(dir, tResBuildPattern);
        } else if (ResBuildHelper.CheckFilePatternRegex(dir, tResBuildPattern)) {
          GenResBuildDataByFile(dir);
        } else {
          ResBuildLog.Warn("GenBuildConfig GenResBuildDataByConfig skip:" + dir);
        }
      }
      return true;
    }
    private static bool GenResBuildDataByDir(string dir, string[] tResBuildPattern)
    {
      DirectoryInfo source = new DirectoryInfo(dir);
      if (!source.Exists) {
        ResBuildLog.Warn("GenBuildConfig GenResBuildDataByDir not exist:" + dir);
        return false;
      }
      foreach (string pattern in tResBuildPattern) {
        FileInfo[] files = source.GetFiles(pattern, SearchOption.AllDirectories);
        foreach (FileInfo fInfo in files) {
          string assetPath = FormatResourceName(fInfo.FullName);
          if (string.IsNullOrEmpty(assetPath)) {
            continue;
          }
          GenResBuildDataByFile(assetPath);
        }
      }
      return true;
    }
    private static bool GenResBuildDataByFile(string assetPath)
    {
      string assetPathLower = assetPath.ToLower();
      if (s_CurContainer.ContainsKey(assetPathLower)) {
        ResBuildLog.Warn("GenBuildConfig GenResBuildDataByFile exist:" + assetPath);
        return false;
      }
      ResBuildData data = new ResBuildData();
      data.m_Id = s_IdGen++;
      data.m_ResourcesName = assetPathLower;
      if (!s_NameRegex.IsMatch(assetPathLower)) {
        ResBuildLog.Warn("PathError:" + assetPath);
      }
      data.m_TargetName = FormatTargetName(data.m_ResourcesName);
      data.m_ResourcesShortName = FormatResourceShortName(assetPath);
      data.m_Size = 0;
      data.m_MD5 = string.Empty;
      s_CurContainer.Add(assetPathLower, data);
      return true;
    }
    private static string FormatTargetName(string assetPath)
    {
      string guid = AssetDatabase.AssetPathToGUID(assetPath);
      string dirPath = guid.Substring(0, 2);
      return string.Format("{0}/{1}{2}", dirPath, guid, ResBuildConfig.BuildOptionExtend);
    }
    private static string FormatResourceName(string assetPath)
    {
      assetPath = assetPath.Replace("\\", "/");
      if (string.IsNullOrEmpty(assetPath)) {
        ResBuildLog.Warn("TranslateFilePathToAssetPath filePath invalid filePath:" + assetPath);
        return string.Empty;
      }
      assetPath = assetPath.Trim();
      int assetsIndex = assetPath.ToLower().IndexOf("assets");
      if (assetsIndex < 0) {
        ResBuildLog.Warn("TranslateFilePathToAssetPath filePath invalid filePath:" + assetPath);
        return string.Empty;
      }
      return assetPath.Substring(assetsIndex);
    }
    private static string FormatResourceShortName(string assetPath)
    {
      if (assetPath.EndsWith(".unity")) {
        return Path.GetFileNameWithoutExtension(assetPath);
      } else {
        int startIndex = assetPath.IndexOf("Resources/");
        startIndex += 10;
        int endIndex = assetPath.LastIndexOf(".");
        if (startIndex >= 0 && endIndex >= 0) {
          return assetPath.Substring(startIndex, (endIndex - startIndex));
        } else if (endIndex >= 0) {
          return assetPath.Substring(0, (endIndex - 0));
        } else {
          return assetPath;
        }
      }
    }
    public static List<ResBuildData> SearchResByNamePrefix(Dictionary<string, ResBuildData> container, string namePrefix)
    {
      List<ResBuildData> dataList = new List<ResBuildData>();
      foreach (ResBuildData data in container.Values) {
        if (data.m_ResourcesName.StartsWith(namePrefix)) {
          dataList.Add(data);
        }
      }
      return dataList;
    }
    public static List<ResBuildData> SearchResByNamePostfix(Dictionary<string, ResBuildData> container, string namePostfix)
    {
      List<ResBuildData> dataList = new List<ResBuildData>();
      foreach (ResBuildData data in container.Values) {
        if (data.m_ResourcesName.EndsWith(namePostfix)) {
          dataList.Add(data);
        }
      }
      return dataList;
    }
  }
}
