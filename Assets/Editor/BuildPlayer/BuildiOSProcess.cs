using System;
using System.IO;
using UnityEngine;
using UnityEditor;

class BuildiOSProcess
{
    static string[] s_ChannelList = new string[] { "cyou" };

    [MenuItem("Build/Build Player/Build iOS Without AB", false, 20)]
    static void BuildiOSWithoutAB()
    {
        try
        {
            Debug.Log("BuildiOSWithoutAB start:" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
            ResBuildConfig.Load();
            string buildPath = BuildPlayerHelper.GetBuildPathForiOS();
            if (Directory.Exists(buildPath))
            {
                Directory.Delete(buildPath, true);
            }
            Directory.CreateDirectory(buildPath);

            foreach (string channelName in s_ChannelList)
            {
                BuildPlayerInfo info = new BuildPlayerInfo();
                info.m_SceneList = BuildPlayerHelper.GetAllScenes();
                info.m_BuildTarget = BuildTarget.iOS;
                info.m_ChannelName = channelName;
                info.m_BuildPath = BuildPlayerHelper.GetBuildPathForiOS(channelName);
                info.m_PlayerPath = BuildPlayerHelper.GetPlayerPathForiOS(channelName, false);
                if (!BuildiOSForChannel(info))
                {
                    Debug.LogError("BuildiOSWithoutAB.BuildiOSForChannel failed. channelName:" + channelName);
                }
            }
            Debug.Log("BuildiOSWithoutAB end:" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
        }
        catch (System.Exception ex)
        {
            Debug.LogError("BuildiOSWithoutAB exception ex:" + ex.Message);
        }
    }
    [MenuItem("Build/Build Player/Build iOS With AB", false, 20)]
    static void BuildiOSWithAB()
    {
        try
        {
            Debug.Log("BuildiOSWithAB start:" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
            ResBuildConfig.Load();
            string buildPath = BuildPlayerHelper.GetBuildPathForiOS();
            if (Directory.Exists(buildPath))
            {
                Directory.Delete(buildPath, true);
            }
            Directory.CreateDirectory(buildPath);

            foreach (string channelName in s_ChannelList)
            {
                BuildPlayerInfo info = new BuildPlayerInfo();
                info.m_SceneList = BuildPlayerHelper.GetAllScenesWithAB();
                info.m_BuildTarget = BuildTarget.iOS;
                info.m_ChannelName = channelName;
                info.m_BuildPath = BuildPlayerHelper.GetBuildPathForiOS(channelName);
                info.m_PlayerPath = BuildPlayerHelper.GetPlayerPathForiOS(channelName, true);
                if (!BuildiOSForChannel(info))
                {
                    Debug.LogError("BuildiOSWithAB.BuildiOSForChannel failed. channelName:" + channelName);
                }
            }
            Debug.Log("BuildiOSWithAB end:" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
        }
        catch (System.Exception ex)
        {
            Debug.LogError("BuildiOSWithAB exception ex:" + ex.Message);
        }
    }
    private static bool BuildiOSForChannel(BuildPlayerInfo info)
    {
        try
        {
            Debug.Log("BuildiOSForChannel info:" + info.ToString());
            Debug.Log("BuildiOSForChannel start:" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
            //1.Check info variable.
            if (info.m_SceneList == null || info.m_SceneList.Length <= 0)
            {
                Debug.LogError("BuildiOS sceneList null");
                return false;
            }
            if (string.IsNullOrEmpty(info.m_ChannelName))
            {
                Debug.LogError("BuildiOS channel name null");
                return false;
            }
            if (string.IsNullOrEmpty(info.m_BuildPath))
            {
                Debug.LogError("BuildiOS projPath null");
                return false;
            }

            //2.Delete old package
            if (File.Exists(info.m_PlayerPath))
            {
                File.Delete(info.m_PlayerPath);
            }

            //3.Clean dir for xcode project 
            if (Directory.Exists(info.m_BuildPath))
            {
                Directory.Delete(info.m_BuildPath, true);
            }
            Directory.CreateDirectory(info.m_BuildPath);

            //4.Register channel name for XUPorter, used for add plist key-value
            XCodePostProcess.RegisterChannelName(info.m_ChannelName);

            //5.Build player
            BuildPipeline.BuildPlayer(info.m_SceneList, info.m_BuildPath, info.m_BuildTarget, BuildOptions.None);
            Debug.Log("BuildiOSForChannel end:" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
        }
        catch (System.Exception ex)
        {
            Debug.LogError("BuildiOS exception ex:" + ex.Message);
            return false;
        }
        return true;
    }
}