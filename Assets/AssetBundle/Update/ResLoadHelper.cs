using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using System.IO;
using System.Security.Cryptography;

namespace ArkCrossEngine
{
    public class ResLoadHelper
    {
        private static string s_StreamingAssetsPath = string.Empty;

        private static string s_ResServerURLAbs = string.Empty;
        private static string s_BuildInURLAbs = string.Empty;
        private static string s_CachedURLAbs = string.Empty;

        public static string GetResServerURLAbs()
        {
            if (!string.IsNullOrEmpty(s_ResServerURLAbs))
            {
                return s_ResServerURLAbs;
            }
            else
            {
                s_ResServerURLAbs = string.Format("{0}/{1}/{2}_{3}/",
                  ResUpdateControler.ResServerURL, ResUpdateControler.AppName,
                  ResUpdateControler.Platform, ResUpdateControler.Channel);
                return s_ResServerURLAbs;
            }
        }
        public static string GetResVersionFileURL()
        {
            return GetResServerURLAbs() + ResUpdateControler.s_ResVersionZip;
        }
        public static string GetAssetDBFileURL()
        {
            return GetResServerURLAbs() + ResUpdateControler.s_AssetDBZip;
        }
        public static string GetResCacheFileURL()
        {
            return GetResServerURLAbs() + ResUpdateControler.s_ResCacheZip;
        }
        public static string GetServerListFileURL()
        {
            return GetResServerURLAbs() + ResUpdateControler.s_ServerListFile;
        }
        public static string GetNoticeConfigFileURL()
        {
            return GetResServerURLAbs() + ResUpdateControler.s_NoticeConfigFile;
        }
        public static string GetResSheetFileURL()
        {
            return GetResServerURLAbs() + ResUpdateControler.s_ResSheetZip;
        }
        public static string GetResABURL(ResVersionData resInfo)
        {
            if (resInfo == null)
            {
                ResLoadHelper.Log("GetResABURL ResVersionData null");
                return string.Empty;
            }
            return GetResServerURLAbs() + resInfo.m_AssetBundleName;
        }

        public static string GetBuildInURLAbs()
        {
            if (!string.IsNullOrEmpty(s_BuildInURLAbs))
            {
                return s_BuildInURLAbs;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(GetStreamingAssetPath());
                sb.Append(ResUpdateControler.s_ResBuildInPath);
                s_BuildInURLAbs = sb.ToString();
                return s_BuildInURLAbs;
            }
        }
        public static string GetBuildInResVersionFileURL()
        {
            return GetBuildInURLAbs() + ResUpdateControler.s_ResVersionZip;
        }
        public static string GetBuildInAssetDBFileURL()
        {
            return GetBuildInURLAbs() + ResUpdateControler.s_AssetDBZip;
        }
        public static string GetBuildInResCacheFileURL()
        {
            return GetBuildInURLAbs() + ResUpdateControler.s_ResCacheZip;
        }
        public static string GetBuildInResSheetFileURL()
        {
            return GetBuildInURLAbs() + ResUpdateControler.s_ResSheetZip;
        }

        public static string GetCachedURLAbs()
        {
            if (!string.IsNullOrEmpty(s_CachedURLAbs))
            {
                return s_CachedURLAbs;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(UnityEngine.Application.persistentDataPath);
                sb.Append("/");
                sb.Append(ResUpdateControler.s_ResCachePath);
                s_CachedURLAbs = sb.ToString();
                return s_CachedURLAbs;
            }
        }
        public static string GetCachedResVersionFileURL()
        {
            return GetCachedURLAbs() + ResUpdateControler.s_ResVersionZip;
        }
        public static string GetCachedAssetDBFileURL()
        {
            return GetCachedURLAbs() + ResUpdateControler.s_AssetDBZip;
        }
        public static string GetCachedResCacheFileURL()
        {
            return GetCachedURLAbs() + ResUpdateControler.s_ResCacheZip;
        }
        public static string GetCachedResSheetFileURL()
        {
            return GetCachedURLAbs() + ResUpdateControler.s_ResSheetZip;
        }

        public static bool IsConfigABCached(string abName)
        {
            if (abName.Equals(ResUpdateControler.s_ResVersionZip))
            {
                return ResUpdateControler.IsResVersionConfigCached;
            }
            else if (abName.Equals(ResUpdateControler.s_AssetDBZip))
            {
                return ResUpdateControler.IsAssetDBConfigCached;
            }
            else if (abName.Equals(ResUpdateControler.s_ResCacheZip))
            {
                return ResUpdateControler.IsResCacheConfigCached;
            }
            else if (abName.Equals(ResUpdateControler.s_ResSheetZip))
            {
                return ResUpdateControler.IsResSheetConfigCached;
            }
            return false;
        }
        public static bool IsResABCached(string abName, string md5)
        {
            ClientResVersionData lrv = ClientResVersionProvider.Instance.GetDataByName(abName);
            if (lrv == null || lrv.m_IsBuildIn)
            {
                return false;
            }
            string resCachedUrl = GetCachedURLAbs() + abName;
            if (!File.Exists(resCachedUrl))
            {
                return false;
            }
            return true;
        }
        public static bool IsResNeedUpdate(string abName, string md5)
        {
            ClientResVersionData lrv = ClientResVersionProvider.Instance.GetDataByName(abName);
            if (lrv == null)
            {
                return true;
            }
            if (string.Compare(md5, lrv.m_MD5, true) != 0)
            {
                return true;
            }
            string resCachedUrl = GetCachedURLAbs() + abName;
            if (!lrv.m_IsBuildIn && !File.Exists(resCachedUrl))
            {
                return true;
            }
            return false;
        }
        #region ResVersionData Helper
        public static ResVersionData GetResVersionDataByAssetId(int resId)
        {
            ResVersionData resVersionData = ResVersionProvider.Instance.GetDataById(resId);
            if (resVersionData == null)
            {
                ResLoadHelper.Log("GetResVersionDataByAssetId GetResVersionData failed resId:" + resId);
                return null;
            }
            return resVersionData;
        }
        public static ResVersionData GetResVersionDataByAssetName(string resName)
        {
            ResVersionData resVersionData = ResVersionProvider.Instance.GetDataByAssetName(resName);
            if (resVersionData == null)
            {
                ResLoadHelper.Log("GetResVersionDataByAssetId GetResVersionData failed resName:" + resName);
                return null;
            }
            return resVersionData;
        }
        #endregion

        #region Path Converter
        public static string GetStreamingAssetPath()
        {
            if (!string.IsNullOrEmpty(s_StreamingAssetsPath))
            {
                return s_StreamingAssetsPath;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
#if UNITY_ANDROID
      sb.Append("jar:file://");
      sb.Append(UnityEngine.Application.dataPath);
      sb.Append("!/assets/");
#else
                sb.Append("file://");
                sb.Append(UnityEngine.Application.streamingAssetsPath);
                sb.Append("/");
#endif
                s_StreamingAssetsPath = sb.ToString();
                return s_StreamingAssetsPath;
            }
        }
        public static string ConvertResourceAssetPathToAbs(string resPathInResDir)
        {
            if (resPathInResDir.StartsWith("assets/"))
            {
                return resPathInResDir;
            }
            else
            {
                //NOTE:Unity 4.2.2 支持多个Resources目录遍历查找，但DF项目强制只能创建一个Resources目录。
                //NOTE:且不能带有后缀
                StringBuilder sb = new StringBuilder();
                sb.Append("assets/resources/");
                sb.Append(resPathInResDir);
                //sb.Append(".prefab");
                return sb.ToString();
            }
        }
        #endregion

        #region Tools
        public static long CaculateABSize(List<int> toUpgradeRes)
        {
            long size = 0;
            for (int i = 0; i < toUpgradeRes.Count; i++)
            {
                ResVersionData data = ResVersionProvider.Instance.GetDataById(toUpgradeRes[i]);
                if (data != null)
                {
                    size += data.m_Size;
                }
            }
            /*
      foreach (int abId in toUpgradeRes) {
        ResVersionData data = ResVersionProvider.Instance.GetDataById(abId);
        if (data != null) {
          size += data.m_Size;
        }
      }*/
            return size;
        }
        public static bool CheckFilePatternEndWith(string filePath, string[] pattern)
        {
            if (pattern == null || pattern.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < pattern.Length; i++)
            {
                if (filePath.EndsWith(pattern[i]))
                {
                    return true;
                }
            }
            /*
      foreach (string tPattern in pattern) {
        if (filePath.EndsWith(tPattern)) {
          return true;
        }
      }*/
            return false;
        }
        public static bool CheckFilePatternStartsWith(string filePath, string[] pattern)
        {
            if (pattern == null || pattern.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < pattern.Length; i++)
            {
                if (filePath.StartsWith(pattern[i]))
                {
                    return true;
                }
            }
            /*
      foreach (string tPattern in pattern) {
        if (filePath.StartsWith(tPattern)) {
          return true;
        }
      }*/
            return false;
        }
        public static string GetFullNameWithoutExtention(string asset)
        {
            if (asset.Contains("."))
            {
                int endIndex = asset.LastIndexOf(".");
                return asset.Substring(0, endIndex);
            }
            else
            {
                return asset;
            }
        }
        public static string ConvertSceneName(string scenePath)
        {
            return Path.GetFileNameWithoutExtension(scenePath);
        }
        #endregion
        public static void Log(string msg)
        {
            LogicSystem.LogicLog(msg);
            //UnityEngine.Debug.Log(msg);
        }
        public static void ErrLog(string msg)
        {
            LogicSystem.LogicErrorLog(msg);
            //UnityEngine.Debug.LogError(msg);
        }
        public static string GetDynamicUrl(string url)
        {
            if (ResUpdateControler.s_UseDynmaicUrl)
            {
                url += "?time=" + DateTime.Now.ToString("yyyyMMddhhmmss");
            }
            return url;
        }
    }
}
