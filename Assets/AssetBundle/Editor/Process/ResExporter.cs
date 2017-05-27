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
  public class ResExporter
  {
    public static bool ExportAllResBuildData()
    {
      if (!CheckEnvironment()) {
        ResBuildLog.Warn("ResExporter.CheckEnvironment failed.");
        return false;
      }
      if (!ResBuildGenerator.GenAllResBuildData()) {
        ResBuildLog.Warn("ResExporter.ExportAllResBuildData failed.");
        return false;
      }
      Dictionary<string, ResBuildData> container = ResBuildGenerator.GetContainer(false);

      foreach (ResBuildData data in container.Values) {
        ExportResBuildData(data);
      }
      ResBuildLog.Info("ResExporter.ExportAllResBuildData Success");
      return true;
    }
    public static bool ExportSelectedResBuildData(UnityEngine.Object selObj)
    {
      if (!ResBuildGenerator.GenResBuildData(selObj)) {
        ResBuildLog.Warn("ResExporter.ExportSelectedResBuildData failed.");
        return false;
      }
      Dictionary<string, ResBuildData> container = ResBuildGenerator.GetContainer(false);

      foreach (ResBuildData data in container.Values) {
        ExportResBuildData(data);
      }
      ResBuildLog.Info("ResExporter.ExportSelectedResBuildData Success");
      return true;
    }

    public static bool ExportResBuildData(ResBuildData data)
    {
      try {
        string pathName = ResBuildHelper.FormatResPathFromConfig(data);
        string pDir = Path.GetDirectoryName(pathName);
        if (!Directory.Exists(pDir)) {
          Directory.CreateDirectory(pDir);
        }

        if (ResBuildHelper.IsSceneRes(data.m_ResourcesName)) {
          if (!string.IsNullOrEmpty(data.m_ResourcesName)) {
            string[] levels = { data.m_ResourcesName };
            BuildPipeline.BuildStreamedSceneAssetBundle(levels, pathName, ResBuildConfig.BuildOptionTarget);
          } else {
            throw new Exception("ResExporter ExportResBuildData failed!");
          }
        } else if (ResBuildHelper.IsFileRes(data.m_ResourcesName)) {
          UnityEngine.Object targetAssets = AssetDatabase.LoadMainAssetAtPath(data.m_ResourcesName);
          UnityEngine.Object[] assets = { targetAssets };
          string[] assetNames = { data.m_ResourcesName };
          if (targetAssets != null) {
            BuildPipeline.BuildAssetBundleExplicitAssetNames(
              assets,
              assetNames,
              pathName,
              ResBuildConfig.BuildOptionRes,
              ResBuildConfig.BuildOptionTarget);
            if (ResBuildConfig.BuildOptionZip) {
              ZipHelper.ZipFile(pathName, pathName);
            }
          } else {
            throw new Exception("ResExporter ExportResBuildData failed!");
          }
        } else {
          throw new Exception("ResExporter ExportResBuildData failed!");
        }
      } catch (System.Exception ex) {
        ResBuildLog.Warn("ResExporter ExportResBuildData failed! Config:" + data.ToString() + " ex" + ex);
        return false;
      }
      return true;
    }
    private static bool CheckEnvironment()
    {
      try {
        string outputPath = ResBuildHelper.GetFilePathAbs(ResBuildHelper.GetPlatformPath(ResBuildConfig.BuildOptionTarget));
        if (!System.IO.Directory.Exists(outputPath)) {
          System.IO.Directory.CreateDirectory(outputPath);
          ResBuildLog.Info("ResBuilder.CheckEnvironment Create Directory " + outputPath);
        }
        if (!System.IO.Directory.Exists(outputPath)) {
          ResBuildLog.Warn("ResBuilder.CheckEnvironment directory create failed Path:" + outputPath);
          return false;
        }
      } catch (System.Exception ex) {
        ResBuildLog.Warn("ResBuilder.CheckEnvironment directory check failed!ex:" + ex);
        return false;
      }
      ResBuildLog.Info("ResBuilder.CheckEnvironment Success");
      return true;
    }
  }
}
