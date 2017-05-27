using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

public class ResWinMgr
{
  private static UnityEngine.Vector2 m_WinMinSize = new UnityEngine.Vector2(600.0f, 600.0f);
  private static Rect m_WinPosition = new Rect(300.0f, 200.0f, m_WinMinSize.x, m_WinMinSize.y);

  private static ResMainWin m_MainWin = null;
  private static ResPlayerWin m_PlayerWin = null;

  #region Config
  [MenuItem("Build/Config/Resources")]
  public static void CreateResMainWin()
  {
    m_MainWin = EditorWindow.GetWindow<ResMainWin>("Config", false, typeof(ResMainWin));
    m_MainWin.position = m_WinPosition;
    m_MainWin.minSize = m_WinMinSize;
    m_MainWin.ShowTab();
    m_MainWin.Focus();
  }
  [MenuItem("Build/Config/Player")]
  public static void CreateResPlayerWin()
  {
    m_PlayerWin = EditorWindow.GetWindow<ResPlayerWin>("Player", false, typeof(ResMainWin));
    m_PlayerWin.position = m_WinPosition;
    m_PlayerWin.minSize = m_WinMinSize;
    m_PlayerWin.ShowTab();
    m_PlayerWin.Focus();
  }
  #endregion

  #region BuildResources
  [MenuItem("Build/Build Res/Build All Resources")]
  public static void BuildAllResources()
  {
    if (ResBuildProcesser.BuildAllResources()) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildAllResources Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildAllResources Failed!",
        "OK");
    }
  }
  [MenuItem("Build/Build Res/BuildConfigFiles")]
  public static void BuildConfigFiles()
  {
    if (ResBuildConfig.Load()
           && ResCacheGenerator.BuildResCacheFile(true)
           && ResSheetGenerator.BuildResSheetFile()
           && ResVersionGenerator.BuildResVersionFiles()
           && VersionGenerator.GenVersionFile()) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildConfigFiles Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildConfigFiles Failed!",
        "OK");
    }
  }
  [MenuItem("Build/Build Res/Build Selected Resources")]
  public static void BuildSelectedResourceAsConfig()
  {
    UnityEngine.Object selObj = Selection.activeObject;
    if (selObj == null) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildSelectedResourceAsConfig Selected nothing!",
        "OK");
    }
    if (ResBuildProcesser.BuildSelectedResources(selObj)) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildSelectedResourceAsConfig Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildSelectedResourceAsConfig Failed!",
        "OK");
    }
  }
  [MenuItem("Build/Build Res/Build Sperate Steps/BuildResCacheFile")]
  public static void BuildResCacheFile()
  {
    if (ResBuildConfig.Load()
           && ResCacheGenerator.BuildResCacheFile(true)) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildResCacheFile Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildResCacheFile Failed!",
        "OK");
    }
  }
  [MenuItem("Build/Build Res/Build Sperate Steps/BuildResSheetFile")]
  public static void BuildResSheetFile()
  {
    if (ResBuildConfig.Load()
           && ResSheetGenerator.BuildResSheetFile()) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildResSheetFile Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildResSheetFile Failed!",
        "OK");
    }
  }
  [MenuItem("Build/Build Res/Build Sperate Steps/BuildResVersionFiles()")]
  public static void BuildResVersionFiles()
  {
    if (ResBuildConfig.Load()
           && ResVersionGenerator.BuildResVersionFiles(true)) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildResVersionFiles Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildResVersionFiles Failed!",
        "OK");
    }
  }
  [MenuItem("Build/Build Res/Build Sperate Steps/GenVersionFile()")]
  public static void GenVersionFile()
  {
    if (ResBuildConfig.Load()
          && VersionGenerator.GenVersionFile()) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "GenVersionFile Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "GenVersionFile Failed!",
        "OK");
    }
  }
  [MenuItem("Build/Build Res/Build Sperate Steps/Commit BuildIn Res")]
  public static void CommitBuildInRes()
  {
    if (ResBuildConfig.Load()
      && ResDeployer.CommitBuildInResources()) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "CommitBuildInResources Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "CommitBuildInResources Failed!",
        "OK");
    }
  }
  //[MenuItem("Build/BuildAllResources")]
  public static void BuildAllResourceAsConfig()
  {
    if (ResBuildProcesser.BuildAllResources()) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildAllResource Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildAllResource Failed!",
        "OK");
    }
  }
  //[MenuItem("Build/BuildAllResources/BuildAllResources For Windows")]
  public static void BuildAllResourcesForWindows()
  {
    if (ResBuildProcesser.BuildAllResources((int)BuildTarget.StandaloneWindows)) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildAllResourcesForWindows Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildAllResourcesForWindows Failed!",
        "OK");
    }
  }
  //[MenuItem("Build/BuildAllResources/BuildAllResources For Mac")]
  public static void BuildAllResourcesForMac()
  {
    if (ResBuildProcesser.BuildAllResources((int)BuildTarget.StandaloneOSXIntel)) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildAllResourcesForMac Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildAllResourcesForMac Failed!",
        "OK");
    }
  }
  //[MenuItem("Build/BuildAllResources/BuildAllResources For Android")]
  public static void BuildAllResourcesForAndroid()
  {
    if (ResBuildProcesser.BuildAllResources((int)BuildTarget.Android)) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildAllResourcesForAndroid Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildAllResourcesForAndroid Failed!",
        "OK");
    }
  }
  //[MenuItem("Build/BuildAllResources/BuildAllResources For iPhone")]
  public static void BuildAllResourcesForiPhone()
  {
    if (ResBuildProcesser.BuildAllResources((int)BuildTarget.iOS)) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildAllResourcesForiPhone Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "BuildAllResourcesForiPhone Failed!",
        "OK");
    }
  }
  #endregion

  #region Clean
  [MenuItem("Build/Clean Res/Clean Cache")]
  public static void CleanCache()
  {
    if (ResBuildConfig.Load()
     && ResDeployer.CleanCache()) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "CleanCache Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "CleanCache Failed!",
        "OK");
    }
  }
  [MenuItem("Build/Clean Res/Clean Buildin Res")]
  public static void CleanBuildInRes()
  {
    if (ResBuildConfig.Load()
      && ResDeployer.CleanBuildInRes()) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "CleanBuildInRes Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "CleanBuildInRes Failed!",
        "OK");
    }
  }
  [MenuItem("Build/Clean Res/Clean StreamingAssets")]
  public static void CleanStreamingAssets()
  {
    if (ResBuildConfig.Load()
      && ResDeployer.CleanStreamingAssets()) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "CleanStreamingAssets Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "CleanStreamingAssets Failed!",
        "OK");
    }
  }
  [MenuItem("Build/Clean Res/Clean Output Res")]
  public static void CleanOutputRes()
  {
    if (ResBuildConfig.Load()
      && ResDeployer.CleanOutputDir()) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "CleanOutputRes Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "CleanOutputRes Failed!",
        "OK");
    }
  }
  [MenuItem("Build/Clean Res/Clean All")]
  public static void CleanAll()
  {
    if (EditorUtility.DisplayDialog("Clean All", "Clean All Resources and Cache?", "Confirm", "Cancel")) {
      //TODO:防止资源删除不干净，重复执行3次，不要喷，我知道挫爆了~~~~
      if (ResBuildConfig.Load()
        && ResDeployer.CleanAll()
        && ResDeployer.CleanAll()
        && ResDeployer.CleanAll()) {
        EditorUtility.DisplayDialog(
          "Confirm",
          "CleanAll Success!",
          "OK");
      } else {
        EditorUtility.DisplayDialog(
          "Confirm",
          "CleanAll Failed!",
          "OK");
      }
    }
  }
  #endregion

  #region Tools
  [MenuItem("Build/Tools/ShaderFinder")]
  public static void ShaderFinder()
  {
    if (ResShaderFinder.FinderShaderData()) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "ShaderFinder Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "ShaderFinder Failed!",
        "OK");
    }
  }
  [MenuItem("Build/Tools/TypeFinder")]
  public static void TypeFinder()
  {
    if (ResTypeFinder.FinderTypeData()) {
      EditorUtility.DisplayDialog(
        "Confirm",
        "TypeFinder Success!",
        "OK");
    } else {
      EditorUtility.DisplayDialog(
        "Confirm",
        "TypeFinder Failed!",
        "OK");
    }
  }
  #endregion

  private static void CreateWins()
  {
    m_MainWin = EditorWindow.GetWindow<ResMainWin>("Config", false, typeof(ResMainWin));
    m_MainWin.position = m_WinPosition;
    m_MainWin.minSize = m_WinMinSize;

    m_PlayerWin = EditorWindow.GetWindow<ResPlayerWin>("Player", false, typeof(ResMainWin));
  }
}

