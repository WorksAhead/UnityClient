using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using ArkCrossEngine;
using UnityEngine;
using UnityEditor;

public class ResVersionGenerator
{
  public static bool BuildResVersionFiles(bool isReGen = false)
  {
    Dictionary<string, ResBuildData> container = ResBuildGenerator.GetContainer(isReGen);
    if (container == null || container.Count == 0) {
      ResBuildLog.Warn("ResVersionGenerator.BuildResVersionFiles ResBuildProvider is null or empty.");
      return false;
    }
    foreach (ResBuildData data in container.Values) {
      GenerateProperty(data);
    }
    if (!BuildResVersion(container)) {
      ResBuildLog.Warn("ResVersionGenerator.BuildResVersion failed!");
      return false;
    }
    if (!BuildClientResVersion(container)) {
      ResBuildLog.Warn("ResVersionGenerator.BuildClientResVersion failed!");
      return false;
    }
    ResBuildLog.Info("ResVersionGenerator.BuildResVersionFiles Success!");
    return true;
  }
  #region ResVersion
  private static bool BuildResVersion(Dictionary<string, ResBuildData> container)
  {
    if (!OutputResVersionFile(container)) {
      ResBuildLog.Warn("ResVersionGenerator.BuildResVersion OutputResVersionFile failed.");
      return false;
    }
    if (!BuildResVersionFile()) {
      ResBuildLog.Warn("ResVersionGenerator.BuildResVersion OutputResVersionFile failed.");
      return false;
    }
    ResBuildLog.Info("ResVersionGenerator.BuildResVersion Success");
    return true;
  }
  private static bool OutputResVersionFile(Dictionary<string, ResBuildData> container)
  {
    string filePath = ResBuildHelper.FormatResListFilePath();
    string fileContent = ResBuildConfig.ResVersionHeader + "\n";
    foreach (ResBuildData config in container.Values) {
      if (config != null) {
        string abInfo = string.Format(ResBuildConfig.ResVersionFormat + "\n",
          config.m_Id,
          config.m_TargetName,
          config.m_ResourcesName,
          config.m_ResourcesShortName,
          config.m_MD5,
          config.m_Size);
        fileContent += abInfo;
      }
    }
    try {
      if (!ResBuildHelper.CheckFilePath(filePath)) {
        ResBuildLog.Warn("ResVersionGenerator.OutputResVersionFile file not exist.");
        return false;
      }
      File.WriteAllText(filePath, fileContent, Encoding.UTF8);
    } catch (System.Exception ex) {
      ResBuildLog.Warn("ResVersionGenerator.OutputResVersionFile failed!" + ex);
      return false;
    }
    AssetDatabase.Refresh();
    ResBuildLog.Info("ResVersionGenerator.OutputResVersionFile Success");
    return true;
  }
  private static bool BuildResVersionFile()
  {
    string outputPath = ResBuildHelper.GetFilePathAbs(ResBuildHelper.GetPlatformPath(ResBuildConfig.BuildOptionTarget));
    try {
      if (!System.IO.Directory.Exists(outputPath)) {
        System.IO.Directory.CreateDirectory(outputPath);
      }
      if (!System.IO.Directory.Exists(outputPath)) {
        ResBuildLog.Warn("ResVersionGenerator.BuildResVersionFile directory create failed Path:" + outputPath);
        return false;
      }
    } catch (System.Exception ex) {
      ResBuildLog.Warn("ResVersionGenerator.RecordAssetList directory check failed! ex:" + ex);
      return false;
    }
    UnityEngine.TextAsset resVersionObj = AssetDatabase.LoadAssetAtPath(
      ResBuildHelper.FormatResListFilePath(), typeof(UnityEngine.TextAsset)) as TextAsset;
    if (resVersionObj != null) {
      UnityEngine.Object[] assets = { resVersionObj };
      string[] assetNames = { ResBuildConfig.ResVersionFilePath };
      string resVersionZipPath = ResBuildHelper.FormatResVersionZipPath();
      BuildPipeline.BuildAssetBundleExplicitAssetNames(assets, assetNames,
        resVersionZipPath,
        ResBuildConfig.BuildOptionRes,
        ResBuildConfig.BuildOptionTarget);
      if (ResBuildConfig.BuildOptionZip) {
        ZipHelper.ZipFile(resVersionZipPath, resVersionZipPath);
      }
    } else {
      ResBuildLog.Warn("ResVersionGenerator.BuildResVersionFile failed:");
      return false;
    }
    AssetDatabase.Refresh();
    ResBuildLog.Info("ResVersionGenerator.BuildResVersionFile Success");
    return true;
  }
  #endregion
  #region ClientResVersion
  private static bool BuildClientResVersion(Dictionary<string, ResBuildData> container)
  {
    if (!OutputClientResVersionFile(container)) {
      ResBuildLog.Warn("ResVersionGenerator.BuildResVersion OutputResVersionFile failed.");
      return false;
    }
    ResBuildLog.Info("ResVersionGenerator.BuildClientResVersion Success");
    return true;
  }
  private static bool OutputClientResVersionFile(Dictionary<string, ResBuildData> container)
  {
    string filePath = ResBuildHelper.FormatClientResListFilePath();
    string fileContent = ResBuildConfig.ResVersionClientHeader + "\n";
    try {
      if (!ResBuildHelper.CheckFilePath(filePath)) {
        ResBuildLog.Warn("ResVersionGenerator.OutputClientResVersionFile file not exist.");
        return false;
      }
      foreach (ResBuildData config in container.Values) {
        if (config != null) {
          string abInfo = string.Format(ResBuildConfig.ResVersionClientFormat + "\n",
            ResBuildHelper.FormatResNameFromConfig(config),
            config.m_MD5,
            true);
          fileContent += abInfo;
        }
      }
      File.WriteAllText(filePath, fileContent, Encoding.UTF8);
    } catch (System.Exception ex) {
      ResBuildLog.Warn("ResVersionGenerator.OutputClientResVersionFile failed!" + ex);
      return false;
    }

    AssetDatabase.Refresh();
    ResBuildLog.Info("ResVersionGenerator.OutputClientResVersionFile Success");
    return true;
  }
  #endregion
  private static bool GenerateProperty(ResBuildData config)
  {
    string pathName = ResBuildHelper.FormatResPathFromConfig(config);
    string filePath = ResBuildHelper.GetFilePathAbs(pathName);
    if (File.Exists(filePath)) {
      config.m_MD5 = ResBuildHelper.GetFileMd5(filePath);
      config.m_Size = ResBuildHelper.GetFileSize(filePath);
    } else {
      ResBuildLog.Warn("ResVersionGenerator.GenerateProperty file not exist.filePath:" + filePath);
      config.m_MD5 = string.Empty;
      config.m_Size = 0;
    }
    return true;
  }
  public static bool LoadResVersion()
  {
    string filePath = ResBuildHelper.GetFilePathAbs(ResBuildHelper.FormatResListFilePath());
    if (string.IsNullOrEmpty(filePath)) {
      ResBuildLog.Warn("ResVersionGenerator.LoadResVersion failed:");
      return false;
    }
    try {
      ResVersionProvider.Instance.Clear();
      byte[] buffer = File.ReadAllBytes(filePath);
      bool ret = ResVersionProvider.Instance.CollectDataFromDBC(buffer);
      if (!ret) {
        ResBuildLog.Warn("ResVersionGenerator.LoadResVersion CollectDataFromDBC failed! filePath:" + filePath);
        return false;
      }
    } catch (System.Exception ex) {
      ResBuildLog.Warn("ResVersionGenerator.LoadResVersion failed! ex:" + ex);
      return false;
    }
    ResBuildLog.Info("ResVersionGenerator.LoadResVersion Success");
    return true;
  }
}
