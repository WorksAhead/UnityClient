using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using UnityEngine;
using System.IO;
using System.Security.Cryptography;

namespace ArkCrossEngine
{
    internal class ResUpdateTool
    {
        public static bool SearchUpgradeRes(List<int> container)
        {
            Dictionary<int, ResVersionData> resVersinList = ResVersionProvider.Instance.GetData();
            if (resVersinList != null && resVersinList.Count > 0)
            {
                foreach (ResVersionData rv in resVersinList.Values)
                {
                    if (ResLoadHelper.IsResNeedUpdate(rv.m_AssetBundleName, rv.m_MD5))
                    {
                        container.Add(rv.GetId());
                    }
                }
            }
            return true;
        }
        public static bool SaveCacheAB(byte[] bytes, string abName, string md5, bool isUnzip = true)
        {
            try
            {
                if (bytes == null)
                {
                    ResLoadHelper.Log("SaveCacheAB bytes null or empty data;" + abName);
                    return false;
                }
                string persistPath = ResLoadHelper.GetCachedURLAbs() + abName;
                if (!string.IsNullOrEmpty(persistPath))
                {
                    string dir = Path.GetDirectoryName(persistPath);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    if (ResUpdateControler.s_EnableZip && isUnzip)
                    {
                        ZipHelper.UnzipFile(bytes, dir);
                    }
                    else
                    {
                        File.WriteAllBytes(persistPath, bytes);
                    }
                    ClientResVersionData lrv = ClientResVersionProvider.Instance.GetDataByName(abName);
                    if (lrv != null)
                    {
                        lrv.m_MD5 = md5;
                        lrv.m_IsBuildIn = false;
                    }
                    else
                    {
                        lrv = new ClientResVersionData();
                        lrv.m_Name = abName;
                        lrv.m_MD5 = md5;
                        lrv.m_IsBuildIn = false;
                        ClientResVersionProvider.Instance.AddData(lrv);
                    }
                    ResUpdateControler.DownLoadNum++;
                }
            }
            catch (System.Exception ex)
            {
                ResLoadHelper.Log("SaveCacheAB abName:" + abName + "ex:" + ex);
                return false;
            }
            return true;
        }
        public static IEnumerator ExtractResSheet(ResAsyncInfo info, AssetBundle assetBundle)
        {
            if (assetBundle == null)
            {
                yield break;
            }

            string outPath = Path.Combine(UnityEngine.Application.persistentDataPath, ResUpdateControler.s_ResSheetCachePath);
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }
            string[] tResSheetPattern =
                ResUpdateControler.s_ResSheetPattern.Split(ResUpdateControler.s_ConfigSplit, StringSplitOptions.RemoveEmptyEntries);
            if (tResSheetPattern == null)
            {
                ResLoadHelper.Log("ExtractResSheet ResSheetPattern invalid:" + ResUpdateControler.s_ResSheetPattern);
                yield break;
            }

            //TextAsset txt = assetBundle.Load(ResUpdateControler.s_ResSheetFile, typeof(UnityEngine.TextAsset)) as TextAsset;
            //TextAsset txt = null;
            throw new CodeNotImplException();
            /*
            if (txt != null && txt.bytes != null && txt.bytes.Length > 0)
            {
                string sheetListFilePath = Path.Combine(outPath, ResUpdateControler.s_ResSheetFile);
                File.WriteAllBytes(sheetListFilePath, txt.bytes);

                using (MemoryStream ms = new MemoryStream(txt.bytes))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                    using (StreamReader sr = new StreamReader(ms, encoding))
                    {
                        int totalNum = int.Parse(sr.ReadLine());
                        for (int sheetIndex = 1; sheetIndex <= totalNum; sheetIndex++)
                        {
                            string sheetFile = sr.ReadLine();
                            if (!string.IsNullOrEmpty(sheetFile.Trim()) && ResLoadHelper.CheckFilePatternEndWith(sheetFile, tResSheetPattern))
                            {
                                //TextAsset asset = assetBundle.Load(sheetFile, typeof(UnityEngine.TextAsset)) as TextAsset;
                                TextAsset asset = null;
                                throw new CodeNotImplException();
                                if (asset != null && asset.bytes != null && asset.bytes.Length > 0)
                                {
                                    try
                                    {
                                        string sheetFilePath = Path.Combine(outPath, sheetFile);
                                        string dir = Path.GetDirectoryName(sheetFilePath);
                                        if (!Directory.Exists(dir))
                                        {
                                            Directory.CreateDirectory(dir);
                                        }
                                        File.WriteAllBytes(sheetFilePath, asset.bytes);
                                    }
                                    catch (System.Exception ex)
                                    {
                                        ResLoadHelper.Log(string.Format("ExtractResSheet skip file :{0} ex:{1} st:{2}", sheetFile, ex.Message, ex.StackTrace));
                                        info.IsError = true;
                                        ResUpdateControler.s_UpdateError = ResUpdateError.LoadResSheet_Extract_Error;
                                        if (assetBundle != null)
                                        {
                                            assetBundle.Unload(false);
                                        }
                                        yield break;
                                    }
                                }
                            }
                            else
                            {
                                ResLoadHelper.Log("ExtractResSheet skip file:" + sheetFile);
                            }
                            if (sheetIndex % 10 == 0)
                            {
                                ResUpdateControler.OnUpdateProgress(0.3f + 0.2f * sheetIndex / totalNum);
                                yield return 1;
                            }
                        }
                    }
                }
            }
            if (assetBundle != null)
            {
                assetBundle.Unload(false);
            }
            info.IsDone = true;
            info.Progress = 1.0f;
            */
        }
        public static void CleanUpdatedResVersion()
        {

        }
        public static AssetBundle LoadAssetBundle(string abUrl)
        {
            AssetBundle assetBundle = null;
            try
            {
                //UnityEngine.Debug.Log("LoadAssetBundle ab abUrl:" + abUrl);
                assetBundle = AssetBundle.LoadFromFile(abUrl);
                if (assetBundle == null)
                {
                    UnityEngine.Debug.Log("LoadAssetBundle null url:" + abUrl);
                    ResUpdateControler.s_UpdateError = ResUpdateError.Create_Assetbundle_Error;
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.Log("LoadAssetBundle failed abUrl:" + abUrl + " ex:" + ex);
            }
            return assetBundle;
        }
        public static string FormatSpeed(double speed)
        {
            string retSpeed = string.Empty;
            if (speed < 1024)
            {
                retSpeed = string.Format("{0:N2}B/S", speed);
            }
            else if (speed < 1024 * 1024)
            {
                retSpeed = string.Format("{0:N2}KB/S", speed / 1024);
            }
            else
            {
                retSpeed = string.Format("{0:N2}M/S", speed / (1024 * 1024));
            }
            return retSpeed;
        }
        public static string FormatNumber(double num)
        {
            return string.Format("{0:N2}", num);
        }
        public static string GetChannelName()
        {
            string channelName = string.Empty;
#if UNITY_IPHONE
      //Note:暂时使用固定值
      channelName = "cyou";
#elif UNITY_ANDROID
      channelName = ResUpdateControler.s_ChannelName;
#else
            channelName = "cyou";
#endif
            if (string.IsNullOrEmpty(channelName))
            {
                channelName = "cyou";
            }
            return channelName;
        }
        public static void SetChannelNameByChannelId(string channelId)
        {
            if (channelId == "2010041002")
            {
                ResUpdateControler.s_ChannelName = "cyou";
            }
            else if (channelId == "2010071003")
            {
                ResUpdateControler.s_ChannelName = "uc";
            }
            else
            {
                ResUpdateControler.s_ChannelName = "cyou";
            }
        }
    }
}