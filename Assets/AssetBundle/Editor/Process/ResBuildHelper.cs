using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using ArkCrossEngine;
using UnityEngine;
using UnityEditor;
using System.Security.Cryptography;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

public class ResBuildHelper
{
  private static MD5CryptoServiceProvider s_MD5Generator = new MD5CryptoServiceProvider();
  public static string FormatListContent(List<int> list)
  {
    string ret = string.Empty;
    if (list != null && list.Count > 0) {
      for (int index = 0; index < list.Count; index++) {
        if (index != 0) {
          ret += " ";
        }
        ret += list[index].ToString().Trim();
      }
    }
    return ret;
  }
  public static bool IsSceneRes(string path)
  {
    if (string.IsNullOrEmpty(path)) {
      return false;
    }
    return path.EndsWith(".unity");
  }
  public static bool IsFileRes(string path)
  {
    if (string.IsNullOrEmpty(path)) {
      return false;
    }
    return !IsDirectoryRes(path);
  }
  public static bool IsDirectoryRes(string path)
  {
    if (string.IsNullOrEmpty(path)) {
      return false;
    }
    FileAttributes fileAttr = File.GetAttributes(path);
    return (fileAttr & FileAttributes.Directory) != 0;
  }

  /************************************************************************/
  /* FilePath                                                             */
  /************************************************************************/
  public static string GetFilePathAbs(string filePath)
  {
    filePath = ConvertPathSlash(filePath);
    if (Path.IsPathRooted(filePath)) {
      return filePath;
    } else {
      return Path.Combine(UnityEngine.Application.dataPath + "/..", filePath);
    }
  }
  public static string GetFilePathInAssets(string filePath)
  {
    filePath = ConvertPathSlash(filePath);
    if (Path.IsPathRooted(filePath)) {
      int assetIndex = filePath.ToLower().IndexOf("assets/");
      if (assetIndex >= 0) {
        return filePath.Substring(assetIndex);
      }
    }
    return filePath;
  }
  public static string ConvertPathSlash(string filePath)
  {
    return filePath.Replace("\\", "/");
  }
  public static string FormatResPathFromConfig(ResBuildData config)
  {
    return string.Format("{0}/{1}",
      GetPlatformPath(ResBuildConfig.BuildOptionTarget),
      config.m_TargetName);
  }
  public static string FormatResNameFromConfig(ResBuildData config)
  {
    return string.Format("{0}",
      config.m_TargetName);
  }
  public static string FormatResVersionZipPath()
  {
    return string.Format("{0}/{1}",
      GetPlatformPath(ResBuildConfig.BuildOptionTarget),
      ResBuildConfig.ResVersionZipPath);
  }
  public static string FormatAssetDBZipPath()
  {
    return string.Format("{0}/{1}",
      GetPlatformPath(ResBuildConfig.BuildOptionTarget),
      ResBuildConfig.AssetDBZipPath);
  }
  public static string FormatResCacheZipPath()
  {
    return string.Format("{0}/{1}",
      GetPlatformPath(ResBuildConfig.BuildOptionTarget),
      ResBuildConfig.ResCacheZipPath);
  }
  public static string FormatResSheetZipPath()
  {
    return string.Format("{0}/{1}",
      GetPlatformPath(ResBuildConfig.BuildOptionTarget),
      ResBuildConfig.ResSheetZipPath);
  }
  public static string FormatResListFilePath()
  {
    return string.Format("{0}/{1}",
      GetPlatformPath(ResBuildConfig.BuildOptionTarget),
      ResBuildConfig.ResVersionFilePath);
  }
  public static string FormatClientResListFilePath()
  {
    return string.Format("{0}/{1}",
      GetPlatformPath(ResBuildConfig.BuildOptionTarget),
      ResBuildConfig.ResVersionClientFilePath);
  }
  public static string FormatIncrementalResListFilePath()
  {
    return string.Format("{0}/{1}",
      GetPlatformPath(ResBuildConfig.BuildOptionTarget),
      ResBuildConfig.ResVersionIncrementalFilePath);
  }
  public static string FormatVersionServerFilePath()
  {
    return string.Format("{0}/{1}",
      GetPlatformPath(ResBuildConfig.BuildOptionTarget),
      ResBuildConfig.VersionServerFile);
  }
  public static string FormatVersionClientFilePath()
  {
    return string.Format("{0}/{1}",
      GetPlatformPath(ResBuildConfig.BuildOptionTarget),
      ResBuildConfig.VersionClientFile);
  }
  public static string FormatAssetListFilePath()
  {
    return string.Format("{0}/{1}",
      GetPlatformPath(ResBuildConfig.BuildOptionTarget),
      ResBuildConfig.AssetDBFilePath);
  }
  public static string FormatResCacheFilePath()
  {
    return string.Format("{0}/{1}",
      GetPlatformPath(ResBuildConfig.BuildOptionTarget),
      ResBuildConfig.ResCacheFilePath);
  }
  public static string FormatResBuildFilePath()
  {
    return string.Format("{0}/{1}",
      GetPlatformPath(ResBuildConfig.BuildOptionTarget),
      ResBuildConfig.ResBuildConfigFilePath);
  }
  public static string FormatResFinderShaderFilePath()
  {
    return string.Format("{0}/{1}",
      GetPlatformPath(ResBuildConfig.BuildOptionTarget),
      ResBuildConfig.ResFinderShaderFilePath);
  }
  public static string FormatResFinderTypeFilePath()
  {
    return string.Format("{0}/{1}",
      GetPlatformPath(ResBuildConfig.BuildOptionTarget),
      ResBuildConfig.ResFinderTypeFilePath);
  }
  public static string FormatResBuildLogFilePath()
  {
    return string.Format(ResBuildConfig.ResLogFilePath,
      ResBuildConfig.ResBuildConfigOutputPath,
      "Resources",
      GetChannelPlatformName(ResBuildConfig.BuildOptionTarget),
      DateTime.Now.ToString(ResBuildConfig.LogTimeFormat));
  }
  public static string FormatResPlayerLogFilePath()
  {
    return string.Format(ResBuildConfig.ResLogFilePath,
      ResBuildConfig.ResBuildPlayerPath,
      "Player",
      GetChannelPlatformName(ResBuildConfig.BuildOptionTarget),
      DateTime.Now.ToString(ResBuildConfig.LogTimeFormat));
  }
  public static string GetPlatformPath(UnityEditor.BuildTarget target)
  {
    string SavePath = string.Format("{0}/{1}/{2}",
          ResBuildConfig.ResBuildConfigOutputPath,
          ResBuildConfig.AppName,
          ResBuildHelper.GetChannelPlatformName(ResBuildConfig.BuildOptionTarget));
    return SavePath;
  }
  public static string GetPlatformPlayerPath(UnityEditor.BuildTarget target)
  {
    string SavePath = string.Format("{0}/{1}/",
          ResBuildConfig.ResBuildPlayerPath,
          ResBuildHelper.GetChannelPlatformName(ResBuildConfig.BuildOptionTarget));
    return SavePath;
  }
  public static bool CheckFilePath(string filePath)
  {
    if (!string.IsNullOrEmpty(filePath)) {
      try {
        string dir = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dir)) {
          Directory.CreateDirectory(dir);
        }
        if (!File.Exists(filePath)) {
          FileStream fs = File.Create(filePath);
          fs.Close();
        }
      } catch (System.Exception ex) {
        ResBuildLog.Warn("CheckFilePath file create failed!" + ex);
        return false;
      }
    }
    return true;
  }
  public static string GetAssetURL(UnityEngine.Object target)
  {
    string targetPath = AssetDatabase.GetAssetPath(target);
    string deviceHomeURL = ResLoadHelper.GetStreamingAssetPath();
    return Path.Combine(deviceHomeURL + "../../", targetPath);
  }
  public static FileInfo[] FilterFiles(DirectoryInfo sourceFolder, string pattern)
  {
    List<FileInfo> alFiles = new List<FileInfo>();
    string[] MultipleFilters = pattern.Split(ResBuildConfig.ConfigSplit, StringSplitOptions.RemoveEmptyEntries);
    foreach (string filter in MultipleFilters) {
      FileInfo[] files = sourceFolder.GetFiles(filter);
      alFiles.AddRange(files);
    }
    return alFiles.ToArray();
  }
  public static void CopyDirectory(string srcDir, string tgtDir, string searchPattern)
  {
    DirectoryInfo source = new DirectoryInfo(srcDir);
    DirectoryInfo target = new DirectoryInfo(tgtDir);
    if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase)) {
      throw new Exception(string.Format("为防止出现循环拷贝，要求目标目录不以源目录为起始！source:{0} target:{1}",
        source.FullName, target.FullName));
    }
    if (!source.Exists) {
      return;
    }
    if (!target.Exists) {
      target.Create();
    }
    FileInfo[] files = FilterFiles(source, searchPattern);
    for (int i = 0; i < files.Length; i++) {
      File.Copy(files[i].FullName, target.FullName + "/" + files[i].Name, true);
    }
    DirectoryInfo[] dirs = source.GetDirectories();
    for (int j = 0; j < dirs.Length; j++) {
      CopyDirectory(dirs[j].FullName, target.FullName + "/" + dirs[j].Name, searchPattern);
    }
  }
  public static bool MoveDirectory(string srcDir, string tgtDir, string searchPattern)
  {
    try {
      DirectoryInfo source = new DirectoryInfo(srcDir);
      DirectoryInfo target = new DirectoryInfo(tgtDir);
      if (!source.Exists) {
        ResBuildLog.Warn("MoveDirectory failed source not exist.srcDir:" + srcDir);
        return false;
      }
      if (!target.Exists) {
        target.Create();
      }
      FileInfo[] files = FilterFiles(source, searchPattern);
      for (int i = 0; i < files.Length; i++) {
        File.Move(files[i].FullName, target.FullName + "/" + files[i].Name);
      }
      DirectoryInfo[] dirs = source.GetDirectories();
      for (int j = 0; j < dirs.Length; j++) {
        if (!MoveDirectory(dirs[j].FullName, target.FullName + "/" + dirs[j].Name, searchPattern)) {
          ResBuildLog.Warn("MoveDirectory.MoveDirectory failed.dir:" + dirs[j].FullName);
          //return false;
        }
      }
      source.Delete(true);
    } catch (System.Exception ex) {
      ResBuildLog.Warn("MoveDirectory failed.ex:" + ex);
      return false;
    }
    return true;
  }
  public static bool DeleteDirectory(string target_dir)
  {
    try {
      DirectoryInfo dirInfo = new DirectoryInfo(target_dir);
      FileInfo[] files = dirInfo.GetFiles();
      DirectoryInfo[] dirs = dirInfo.GetDirectories();
      foreach (FileInfo file in files) {
        file.Attributes = FileAttributes.Normal;
        file.Delete();
      }
      foreach (DirectoryInfo dir in dirs) {
        if (!DeleteDirectory(dir.FullName)) {
          ResBuildLog.Warn("DeleteDirectory.DeleteDirectory failed.dir:" + dir.FullName);
          //return false;
        }
      }
      dirInfo.Refresh();
      dirInfo.Delete(false);
    } catch (System.Exception ex) {
      ResBuildLog.Warn("DeleteDirectory failed.ex:" + ex);
      //return false;
    }
    return true;
  }
  public static bool CleanDirectory(string target_dir)
  {
    try {
      DirectoryInfo dirInfo = new DirectoryInfo(target_dir);
      FileInfo[] files = dirInfo.GetFiles();
      DirectoryInfo[] dirs = dirInfo.GetDirectories();
      foreach (FileInfo file in files) {
        file.Attributes = FileAttributes.Normal;
        file.Delete();
      }
      foreach (DirectoryInfo dir in dirs) {
        if (!DeleteDirectory(dir.FullName)) {
          ResBuildLog.Warn("CleanDirectory.DeleteDirectory failed.dir:" + dir.FullName);
          //return false;
        }
      }
    } catch (System.Exception ex) {
      ResBuildLog.Warn("CleanDirectory failed.ex:" + ex);
      //return false;
    }
    return true;
  }
  public static void FindFilesByDirectoryTree(System.IO.DirectoryInfo root, string dirName,
    List<UnityEngine.Object> targetAssets, List<string> targetAssetsNames)
  {
    System.IO.FileInfo[] files = null;
    System.IO.DirectoryInfo[] subDirs = null;
    try {
      files = root.GetFiles("*.*");
    } catch (Exception e) {
      ResBuildLog.Warn("FindFilesByDirectoryTree ex:" + e);
    }
    if (files != null) {
      foreach (System.IO.FileInfo fi in files) {
        if (fi.Extension == ".meta") {
          continue;
        }
        string targetPath = string.Format("{0}/{1}", dirName, fi.Name);
        UnityEngine.Object target = AssetDatabase.LoadMainAssetAtPath(targetPath);
        targetAssets.Add(target);
        targetAssetsNames.Add(targetPath);
      }
      subDirs = root.GetDirectories();
      foreach (System.IO.DirectoryInfo dirInfo in subDirs) {
        string tDirName = string.Format("{0}/{1}", dirName, dirInfo.Name);
        FindFilesByDirectoryTree(dirInfo, tDirName, targetAssets, targetAssetsNames);
      }
    }
  }
  public static long GetFileSize(string filePath)
  {
    long lSize = 0;
    if (File.Exists(filePath))
      lSize = new FileInfo(filePath).Length;
    return lSize;
  }
  public static string GetFileMd5(string filePath)
  {
    FileStream fs = null;
    try {
      if (!File.Exists(filePath)) {
        return string.Empty;
      }
      fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
      byte[] hash = ResBuildHelper.GetMD5Generator().ComputeHash(fs);
      return System.BitConverter.ToString(hash);
    } catch (System.Exception ex) {
      ResBuildLog.Warn("GetFileMd5 failed filePath:" + filePath + "ex:" + ex);
    } finally {
      if (fs != null) { fs.Close(); }
    }
    return string.Empty;
  }
  public static bool CheckFilePatternEndWith(string filePath, string[] pattern)
  {
    if (pattern == null || pattern.Length == 0) {
      return false;
    }
    foreach (string tPattern in pattern) {
      if (filePath.EndsWith(tPattern)) {
        return true;
      }
    }
    return false;
  }
  public static bool CheckFilePatternStartsWith(string filePath, string[] pattern)
  {
    if (pattern == null || pattern.Length == 0) {
      return false;
    }
    foreach (string tPattern in pattern) {
      if (filePath.StartsWith(tPattern)) {
        return true;
      }
    }
    return false;
  }
  public static bool CheckFilePatternRegex(string filePath, string[] pattern)
  {
    if (pattern == null || pattern.Length == 0) {
      return false;
    }
    foreach (string tPattern in pattern) {
      Regex regex = new Regex(tPattern);
      if (regex.IsMatch(filePath)) {
        return true;
      }
    }
    return false;
  }
  public static string GetPlatformName(BuildTarget target)
  {
    switch (target) {
      case BuildTarget.StandaloneWindows:
      case BuildTarget.StandaloneWindows64: {
          return "win32";
        };
      case BuildTarget.StandaloneOSXIntel:
      case BuildTarget.StandaloneOSXIntel64:
      case BuildTarget.StandaloneOSXUniversal: {
          return "mac";
        };
      case BuildTarget.Android: {
          return "android";
        };
      case BuildTarget.iOS: {
          return "ios";
        };
      default: {
          return "invalid";
        }
    }
  }
  public static string GetChannelPlatformName(BuildTarget target)
  {
    return string.Format("{0}_{1}", GetPlatformName(target), ResBuildConfig.Channel);
  }
  public static string GetPlatformExtend(BuildTarget target)
  {
    switch (target) {
      case BuildTarget.StandaloneWindows:
      case BuildTarget.StandaloneWindows64: {
          return ".exe";
        };
      case BuildTarget.StandaloneOSXIntel:
      case BuildTarget.StandaloneOSXIntel64:
      case BuildTarget.StandaloneOSXUniversal: {
          return "";
        };
      case BuildTarget.Android: {
          return ".apk";
        };
      case BuildTarget.iOS: {
          return "";
        };
      default: {
          return "";
        }
    }
  }
  public static bool IsPathValid(string path)
  {
    if (!Path.IsPathRooted(path)) {
      path = ResBuildHelper.GetFilePathAbs(path);
    }

    try {
      FileAttributes fileAttr = File.GetAttributes(path);
      if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory) {
        return Directory.Exists(path);
      } else {
        return File.Exists(path);
      }
    } catch (System.Exception ex) {
      ResBuildLog.Warn("Path invalid, path:" + path + "ex:" + ex);
      return false;
    }
  }
  public static MD5CryptoServiceProvider GetMD5Generator()
  {
    return s_MD5Generator;
  }
}
