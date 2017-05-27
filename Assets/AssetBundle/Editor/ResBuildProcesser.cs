using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using ArkCrossEngine;
using UnityEngine;
using UnityEditor;
using System.Security.Cryptography;

public class ResBuildProcesser
{
  public static bool BuildAllResources(int buildTarget = -1)
  {
    bool ret = true;
    try {
      LogBuildStatus("ResBuildConfig.Load() Start");
      if (!ResBuildConfig.Load()) {
        ResBuildLog.Warn("BuildAllResources.LoadConfig failed!");
        return false;
      }

      LogBuildStatus("ResBuildConfig.SetBuildTargetPlatform Start");
      if ((buildTarget >= 0 && !ResBuildConfig.SetBuildTargetPlatform((BuildTarget)buildTarget))) {
        ResBuildLog.Warn("BuildAllResources.SetBuildTargetPlatform failed!");
        return false;
      }
    
      LogBuildStatus("ResBuildLog.SetFileLog Start");
      if (!ResBuildLog.SetFileLog(true, ResBuildHelper.FormatResBuildLogFilePath())) {
        ResBuildLog.Warn("BuildAllResources.RedirectLog failed!");
        return false;
      }

      LogBuildStatus("ResBuildConfig.Check Start");
      if (!ResBuildConfig.Check()) {
        ResBuildLog.Warn("BuildAllResources.CheckConfig failed!");
        return false;
      }

      LogBuildStatus("ResDeployer.CleanCache Start");
      if (!ResDeployer.CleanCache()) {
        ResBuildLog.Warn("BuildAllResources.CleanCache failed!");
        return false;
      }

      LogBuildStatus("ResDeployer.CleanOutputResDir Start");
      if (ResBuildConfig.ResVersionIncrementalEnable) {
        ResBuildLog.Info("BuildAllResources.ResVersionIncrementalEnable On Skip CleanOutputResDir!");
      } else {
        if (!ResDeployer.CleanOutputResDir()) {
          ResBuildLog.Warn("BuildAllResources.CleanOutputRes failed!");
          return false;
        }
      }

      LogBuildStatus("ResBuilder.BuildAllResources Start");
      if (!ResExporter.ExportAllResBuildData()) {
        ResBuildLog.Warn("BuildAllResources.ExportAllResBuildData failed!");
        return false;
      }

      LogBuildStatus("ResCacheGenerator.BuildResCacheFile Start");
      if (!ResCacheGenerator.BuildResCacheFile()) {
        ResBuildLog.Warn("BuildAllResources.BuildResCacheFile failed!");
        return false;
      }

      LogBuildStatus("ResVersionGenerator.BuildResVersionFiles Start");
      if (!ResVersionGenerator.BuildResVersionFiles()) {
        ResBuildLog.Warn("BuildAllResources.BuildResVersionFiles failed!");
        return false;
      }

      LogBuildStatus("ResSheetGenerator.BuildResSheetFile Start");
      if (!ResSheetGenerator.BuildResSheetFile()) {
        ResBuildLog.Warn("BuildAllResources.BuildResSheetFile failed!");
        return false;
      }

      LogBuildStatus("VersionGenerator.GenVersionFile Start");
      if (!VersionGenerator.GenVersionFile()) {
        ResBuildLog.Warn("BuildAllResources.GenVersionFile failed!");
        return false;
      }

      LogBuildStatus("ResDeployer.CommitBuildInResources Start");
      if (!ResDeployer.CommitBuildInResources()) {
        ResBuildLog.Warn("BuildAllResources.CommitBuildInResources failed!");
        return false;
      }

      LogBuildStatus("ResDeployer.ApplyPlayerConfig Start");
      if (!ResDeployer.ApplyPlayerConfig()) {
        ResBuildLog.Warn("BuildAllResources.ApplyPlayerConfig failed!");
        return false;
      }
      ResBuildLog.Info("ResBuildProcesser.BuildAllResources Success");
    } catch (System.Exception ex) {
      ResBuildLog.Warn("ResBuildProcesser.BuildAllResources failed! ex:" + ex);
      ret = false;
    } finally {
      ResBuildLog.ResetLog();
    }
    return ret;
  }
  public static bool BuildSelectedResources(UnityEngine.Object selObj, int buildTarget = -1)
  {
    bool ret = true;
    try {
      LogBuildStatus("ResBuildConfig.Load() Start");
      if (!ResBuildConfig.Load()) {
        ResBuildLog.Warn("BuildSelectedResources.LoadConfig failed!");
        return false;
      }

      LogBuildStatus("ResBuildConfig.SetBuildTargetPlatform Start");
      if ((buildTarget >= 0 && !ResBuildConfig.SetBuildTargetPlatform((BuildTarget)buildTarget))) {
        ResBuildLog.Warn("BuildSelectedResources.SetBuildTargetPlatform failed!");
        return false;
      }

      LogBuildStatus("ResBuildLog.SetFileLog Start");
      if (!ResBuildLog.SetFileLog(true, ResBuildHelper.FormatResBuildLogFilePath())) {
        ResBuildLog.Warn("BuildSelectedResources.RedirectLog failed!");
        return false;
      }

      LogBuildStatus("ResBuilder.BuildSelectedResources Start");
      if (!ResExporter.ExportSelectedResBuildData(selObj)) {
        ResBuildLog.Warn("BuildSelectedResources.ExportSelectedResBuildData failed!");
        return false;
      }

      LogBuildStatus("ResVersionGenerator.BuildResVersionFiles Start");
      if (!ResVersionGenerator.BuildResVersionFiles(true)) {
        ResBuildLog.Warn("BuildAllResources.BuildResVersionFiles failed!");
        return false;
      }

      ResBuildLog.Info("ResBuildProcesser.BuildSelectedResources Success");
    } catch (System.Exception ex) {
      ResBuildLog.Warn("ResBuildProcesser.BuildAllResources failed! ex:" + ex);
      ret = false;
    } finally {
      ResBuildLog.ResetLog();
    }
    return ret;
  }
  private static void LogBuildStatus(string step)
  {
    ResBuildLog.Info("*********************************************");
    ResBuildLog.Info(step);
  }
}