using UnityEditor;
using System.IO;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

class BuildPlayerInfo
{
    public BuildTarget m_BuildTarget = BuildTarget.iOS;
    public string m_ChannelName = "cyou";
    public string[] m_SceneList = null;
    public string m_BuildPath = string.Empty;
    public string m_PlayerPath = string.Empty;
    public override string ToString()
    {
        return string.Format("BuildTarget:{0} ChannelName:{1} ProjPath:{2} PlayerPath:{3}",
          m_BuildTarget, m_ChannelName, m_BuildPath, m_PlayerPath);
    }
}

class BuildPlayerHelper
{
    public static string GetAppName()
    {
        return "dfm";
    }
    public static string GetBranceName()
    {
        //Note:分包版本中，将读取Assets/AssetBundle/Config.txt中的Channel值。
        //Note:非分包版本中，使用默认值。
        return ResBuildConfig.Channel;
    }
    public static string[] GetAllScenes()
    {
        List<string> names = new List<string>();
        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null) continue;
            if (e.enabled) names.Add(e.path);
        }
        return names.ToArray();
    }
    public static string[] GetAllScenesWithAB()
    {
        return GetAllScenes();
    }
    #region iOS
    public static string GetBuildPathForiOS()
    {
        return Path.Combine(UnityEngine.Application.dataPath, "../../build/ios");
    }
    public static string GetBuildPathForiOS(string channelName)
    {
        return Path.Combine(GetBuildPathForiOS(), channelName);
    }
    public static string GetPlayerPathForiOS(string channelName, bool isAB)
    {
        if (isAB)
        {
            return Path.Combine(GetBuildPathForiOS(), string.Format("{0}_{1}_{2}_ab.ipa",
              GetAppName(), GetBranceName(), channelName));
        }
        else
        {
            return Path.Combine(GetBuildPathForiOS(), string.Format("{0}_{1}_{2}.ipa",
              GetAppName(), GetBranceName(), channelName));
        }
    }
    #endregion
    #region Android
    public static string GetBuildPathForAndroid()
    {
        return Path.Combine(UnityEngine.Application.dataPath, "../../build/android");
    }
    public static string GetPlayerPathForAndroid(string channelName, bool isAB)
    {
        if (isAB)
        {
            return Path.Combine(GetBuildPathForAndroid(), string.Format("{0}_{1}_{2}_ab.apk",
              GetAppName(), GetBranceName(), channelName));
        }
        else
        {
            return Path.Combine(GetBuildPathForAndroid(), string.Format("{0}_{1}_{2}.apk",
              GetAppName(), GetBranceName(), channelName));
        }
    }
    #endregion
    public static void CopyDir(string srcDir, string tgtDir)
    {
        DirectoryInfo source = new DirectoryInfo(srcDir);
        DirectoryInfo target = new DirectoryInfo(tgtDir);
        if (!source.Exists)
        {
            return;
        }
        if (!target.Exists)
        {
            target.Create();
        }
        FileInfo[] files = source.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            File.Copy(files[i].FullName, target.FullName + "/" + files[i].Name, true);
        }
        DirectoryInfo[] dirs = source.GetDirectories();
        for (int j = 0; j < dirs.Length; j++)
        {
            CopyDir(dirs[j].FullName, target.FullName + "/" + dirs[j].Name);
        }
    }
}