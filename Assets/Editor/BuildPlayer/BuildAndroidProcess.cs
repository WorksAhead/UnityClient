using System;
using System.IO;
using UnityEngine;
using UnityEditor;

class BuildAndroidProcess
{
    static string[] s_ChannelList = new string[] { "cyou", "uc" };

    static string defultIconPath = "Assets/UI/AtlasesBag2/baseUI/114.png";
    //static string defaultSplashPath = "Assets/UI/Atlases/CYLogo/Default.png";
    //static string androidSplashPath = "Assets/Android/Splash/Normal/Default@2x.png";
    static string channelIconPath = "Assets/Android/icons/";
    static string defaultPackageName = "com.cyou.mrd.df";
    static string channelDirectory = "AndroidPlugins/";
    static string targetDirectory = "Assets/Plugins/Android";

    static string keyStoreFile = "KeyStore/cyou.keystore";
    static string keyStorePass = "q1w2e3r4t5";
    static string keyAliasName = "cyou";
    static string keyAliasPass = "q1w2e3r4t5";

    [MenuItem("Build/Build Player/Build Android Without AB", false, 20)]
    static void BuildAndroidWithoutAB()
    {
        try
        {
            Debug.Log("BuildAndroidWithoutAB start:" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
            ResBuildConfig.Load();
            string buildPath = BuildPlayerHelper.GetBuildPathForAndroid();
            if (Directory.Exists(buildPath))
            {
                Directory.Delete(buildPath, true);
            }
            Directory.CreateDirectory(buildPath);

            foreach (string channelName in s_ChannelList)
            {
                BuildPlayerInfo info = new BuildPlayerInfo();
                info.m_SceneList = BuildPlayerHelper.GetAllScenes();
                info.m_BuildTarget = BuildTarget.Android;
                info.m_ChannelName = channelName;
                info.m_BuildPath = BuildPlayerHelper.GetBuildPathForAndroid();
                info.m_PlayerPath = BuildPlayerHelper.GetPlayerPathForAndroid(channelName, false);
                if (!BuildAndroidForChannel(info))
                {
                    Debug.LogError("BuildAndroidWithoutAB.BuildAndroidForChannel failed. channelName:" + channelName);
                }
            }
            Debug.Log("BuildAndroidWithoutAB end:" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
        }
        catch (System.Exception ex)
        {
            Debug.LogError("BuildAndroidWithoutAB exception ex:" + ex.Message);
        }
    }
    [MenuItem("Build/Build Player/Build Android With AB", false, 20)]
    static void BuildAndroidWithAB()
    {
        try
        {
            Debug.Log("BuildAndroidWithAB start:" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
            ResBuildConfig.Load();
            string buildPath = BuildPlayerHelper.GetBuildPathForAndroid();
            if (Directory.Exists(buildPath))
            {
                Directory.Delete(buildPath, true);
            }
            Directory.CreateDirectory(buildPath);

            foreach (string channelName in s_ChannelList)
            {
                BuildPlayerInfo info = new BuildPlayerInfo();
                info.m_SceneList = BuildPlayerHelper.GetAllScenesWithAB();
                info.m_BuildTarget = BuildTarget.Android;
                info.m_ChannelName = channelName;
                info.m_BuildPath = BuildPlayerHelper.GetBuildPathForAndroid();
                info.m_PlayerPath = BuildPlayerHelper.GetPlayerPathForAndroid(channelName, true);
                if (!BuildAndroidForChannel(info))
                {
                    Debug.LogError("BuildAndroidWithAB.BuildAndroidForChannel failed. channelName:" + channelName);
                }
            }
            Debug.Log("BuildAndroidWithAB end:" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
        }
        catch (System.Exception ex)
        {
            Debug.LogError("BuildAndroidWithAB exception ex:" + ex.Message);
        }
    }
    private static bool BuildAndroidForChannel(BuildPlayerInfo info)
    {
        try
        {
            Debug.Log("BuildAndroidForChannel info:" + info.ToString());
            Debug.Log("BuildAndroidForChannel start:" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
            //1.Check info variable.
            if (info.m_SceneList == null || info.m_SceneList.Length <= 0)
            {
                Debug.LogError("BuildAndroid sceneList null");
                return false;
            }
            if (string.IsNullOrEmpty(info.m_ChannelName))
            {
                Debug.LogError("BuildAndroid channel name null");
                return false;
            }
            if (string.IsNullOrEmpty(info.m_BuildPath))
            {
                Debug.LogError("BuildAndroid projPath null");
                return false;
            }

            //2.Delete old package
            if (File.Exists(info.m_PlayerPath))
            {
                File.Delete(info.m_PlayerPath);
            }

            //3.Set key store
            Debug.Log("My-Android: keyStoreFile = " + keyStoreFile);
            if (!File.Exists(keyStoreFile))
            {
                Debug.LogError("My-Android: keyStoreFile is empty!");
                return false;
            }
            PlayerSettings.Android.keystoreName = keyStoreFile;
            PlayerSettings.Android.keystorePass = keyStorePass;
            PlayerSettings.Android.keyaliasName = keyAliasName;
            PlayerSettings.Android.keyaliasPass = keyAliasPass;

            //4.Setup channel plugin
            if (!SetupChannelPlugin(info.m_ChannelName))
            {
                Debug.LogError("BuildAndroid SetupChannelPlugin failed");
                return false;
            }

            //5.Build player
            BuildPipeline.BuildPlayer(info.m_SceneList, info.m_PlayerPath, info.m_BuildTarget, BuildOptions.None);
            Debug.Log("BuildAndroidForChannel end:" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
        }
        catch (System.Exception ex)
        {
            Debug.LogError("BuildAndroid exception ex:" + ex.Message);
            return false;
        }
        return true;
    }
    private static bool SetupChannelPlugin(string channelName)
    {
        try
        {
            Debug.Log("SetupChannelPlugin start channelName:" + channelName);
            string channelPackageName = "";
            //string channelsplash = "";
            string channelIcon = "";
            switch (channelName)
            {
                case "uc":
                    channelIcon = channelIconPath + "114x114_uc.png";
                    channelPackageName = defaultPackageName + ".uc";
                    //channelsplash = androidSplashPath;
                    /*
                    channelsplash = "Assets/Android/Splash/UC/default.png";
                    */
                    break;
                default:
                    channelIcon = defultIconPath;
                    channelPackageName = defaultPackageName;
                    //channelsplash = androidSplashPath;
                    break;
            }

            AssetDatabase.Refresh();
            if (!channelIcon.Equals(defultIconPath))
            {
                Texture2D[] icons = new Texture2D[] { AssetDatabase.LoadMainAssetAtPath(channelIcon) as Texture2D, };
                PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, icons);
            }

            //splash 替换 自适应屏幕//
            //FileUtil.ReplaceFile(channelsplash, defaultSplashPath);
            PlayerSettings.Android.splashScreenScale = AndroidSplashScreenScale.ScaleToFill;

            //packagename
            PlayerSettings.applicationIdentifier = channelPackageName;

            string channelDir = Path.Combine(channelDirectory, channelName);
            string channelDirAbs = Path.Combine(UnityEngine.Application.dataPath + "/../", channelDir);
            AssetDatabase.Refresh();
            if (!Directory.Exists(channelDirAbs))
            {
                Debug.Log("channelBaseDirectory not exist:" + channelDirAbs);
                return false;
            }

            string targetDirectoryAbs = Path.Combine(UnityEngine.Application.dataPath + "/../", targetDirectory);
            FileUtil.DeleteFileOrDirectory(targetDirectory);
            //AssetDatabase.Refresh();

            //FileUtil.CopyFileOrDirectory(channelBaseDirectory, targetDirectory);
            BuildPlayerHelper.CopyDir(channelDirAbs, targetDirectoryAbs);
            AssetDatabase.Refresh();
            Debug.Log("SetupChannelPlugin end channelName:" + channelName);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("BuildAndroid exception ex:" + ex.Message);
            return false;
        }
        return true;
    }
}