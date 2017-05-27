using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace ArkCrossEngine
{
    class ResVersionLoader
    {
        public static ResAsyncInfo RequestResVersion()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(RequestResVersionAsync(info));
            return info;
        }
        public static ResAsyncInfo LoadResVersion()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(LoadResVersionAsync(info));
            return info;
        }
        public static ResAsyncInfo LoadClientResVersion()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(LoadClientResVersionAsync(info));
            return info;
        }
        private static IEnumerator RequestResVersionAsync(ResAsyncInfo info)
        {
            if (UnityEngine.Application.internetReachability == NetworkReachability.NotReachable)
            {
                ResLoadHelper.Log("RequestResVersion network not reachable:");
                info.IsError = true;
                ResUpdateControler.s_UpdateError = ResUpdateError.Network_Error;
                yield break;
            }

            string requestResVersionUrl = ResLoadHelper.GetResVersionFileURL();
            requestResVersionUrl = ResLoadHelper.GetDynamicUrl(requestResVersionUrl);
            ResLoadHelper.Log("RequestResVersion URL;" + requestResVersionUrl);
            using (WWW tWWW = new WWW(requestResVersionUrl))
            {
                yield return tWWW;
                try
                {
                    if (tWWW.error != null)
                    {
                        ResLoadHelper.Log("RequestResVersionList error");
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.RequestResVersion_WWW_Error;
                        tWWW.Dispose();
                        yield break;
                    }
                    byte[] buffer = tWWW.bytes;
                    if (buffer == null || buffer.Length < 0)
                    {
                        ResLoadHelper.Log("RequestResVersionList tWWW.byte null");
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.RequestResVersion_Byte_Error;
                        tWWW.Dispose();
                        yield break;
                    }
                    ResUpdateControler.IsResVersionConfigCached = true;
                    ResUpdateTool.SaveCacheAB(buffer, ResUpdateControler.s_ResVersionZip, "");
                }
                catch (System.Exception ex)
                {
                    ResLoadHelper.Log("RequestResVersion ex:" + ex);
                    info.IsError = true;
                    ResUpdateControler.s_UpdateError = ResUpdateError.RequestResVersion_Save_Error;
                }
                finally
                {
                    tWWW.Dispose();
                }
            }
            if (!LoadResVersionImpl())
            {
                ResLoadHelper.Log("RequestResVersion LoadResVersionImpl failed");
                info.IsError = true;
                yield break;
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
        private static IEnumerator LoadResVersionAsync(ResAsyncInfo info)
        {
            string url = ResLoadHelper.GetBuildInResVersionFileURL();
            ResLoadHelper.Log("LoadResVersion url:" + url);
            AssetBundle assetBundle = null;
            if (!ResLoadHelper.IsConfigABCached(ResUpdateControler.s_ResVersionZip))
            {
                using (WWW tWWW = new WWW(url))
                {
                    yield return tWWW;
                    try
                    {
                        if (tWWW.error != null)
                        {
                            ResLoadHelper.Log("LoadResVersion www error url:" + url);
                            info.IsError = true;
                            ResUpdateControler.s_UpdateError = ResUpdateError.LoadResVersion_WWW_Error;
                            tWWW.Dispose();
                            yield break;
                        }
                        byte[] buffer = tWWW.bytes;
                        if (buffer == null || buffer.Length < 0)
                        {
                            ResLoadHelper.Log("LoadResVersion tWWW.byte null");
                            info.IsError = true;
                            ResUpdateControler.s_UpdateError = ResUpdateError.LoadResVersion_Byte_Error;
                            tWWW.Dispose();
                            yield break;
                        }
                        ResUpdateControler.IsResVersionConfigCached = true;
                        ResUpdateTool.SaveCacheAB(buffer, ResUpdateControler.s_ResVersionZip, "");
                    }
                    catch (System.Exception ex)
                    {
                        ResLoadHelper.Log("LoadResVersion ab failed url:" + url + "ex:" + ex);
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.LoadResVersion_Save_Error;
                    }
                    finally
                    {
                        if (assetBundle != null)
                        {
                            assetBundle.Unload(true);
                        }
                        tWWW.Dispose();
                    }
                }
            }
            if (!LoadResVersionImpl())
            {
                ResLoadHelper.Log("RequestResVersion LoadResVersionImpl failed");
                info.IsError = true;
                yield break;
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
        private static bool LoadResVersionImpl()
        {
            string url = ResLoadHelper.GetCachedResVersionFileURL();
            AssetBundle assetBundle = null;
            try
            {
                assetBundle = ResUpdateTool.LoadAssetBundle(url);
                if (assetBundle == null)
                {
                    ResLoadHelper.Log("LoadResVersionImpl assetBundleObj null url:" + url);
                    ResUpdateControler.s_UpdateError = ResUpdateError.LoadResVersion_Assetbundle_Error;
                    return false;
                }
                //TextAsset txt = null;
                throw new CodeNotImplException();
                //                 TextAsset txt = assetBundle.Load(
                //                       ResUpdateControler.s_ResVersionFile, typeof(UnityEngine.TextAsset)) as TextAsset;
                //                 if (txt == null || txt.bytes == null || txt.bytes.Length <= 0)
                //                 {
                //                     ResLoadHelper.Log("LoadResVersionImpl bytes null");
                //                     ResUpdateControler.s_UpdateError = ResUpdateError.LoadResVersion_Assetbundle_Load_Error;
                //                     return false;
                //                 }
//                 byte[] bytes = txt.bytes;
//                 ResVersionProvider.Instance.Clear();
//                 ResVersionProvider.Instance.CollectDataFromDBC(bytes);
//                 AssetExManager.Instance.InitAllAssetEx();
            }
            catch (System.Exception ex)
            {
                ResLoadHelper.Log("LoadResVersionImpl res failed url:" + url + " ex:" + ex);
                ResUpdateControler.s_UpdateError = ResUpdateError.LoadResVersion_AsssetEx_Mgr_Error;
                return false;
            }
            finally
            {
                if (assetBundle != null)
                {
                    assetBundle.Unload(true);
                }
            }
        }
        private static IEnumerator LoadClientResVersionAsync(ResAsyncInfo info)
        {
            //从本地加载版本号
            string resVersionFilePersistPath = Path.Combine(UnityEngine.Application.persistentDataPath,
              ResUpdateControler.s_ResCachePath + ResUpdateControler.s_ResVersionClientFile);
            string buildinVersionFilePath = Path.Combine(ResLoadHelper.GetStreamingAssetPath(),
                ResUpdateControler.s_ResBuildInPath + ResUpdateControler.s_ResVersionClientFile);

            if (File.Exists(resVersionFilePersistPath))
            {
                if (ResUpdateControler.IsNeedSyncPackage)
                {
                    //Note:无需合并新旧资源，改为删除就资源
                    //ClientResVersionProvider.Instance.Clear();
                    //ClientResVersionProvider.Instance.CollectDataFromDBC(resVersionFilePersistPath);
                    ResUpdateTool.CleanUpdatedResVersion();
                    ClientResVersionProvider.Instance.Clear();

                    using (WWW tWWW = new WWW(buildinVersionFilePath))
                    {
                        yield return tWWW;
                        try
                        {
                            if (tWWW.error != null)
                            {
                                ResLoadHelper.Log("LoadClientResVersion error");
                                info.IsError = true;
                                ResUpdateControler.s_UpdateError = ResUpdateError.LoadClientResVersion_WWW_Error;
                                tWWW.Dispose();
                                yield break;
                            }
                            byte[] bytes = tWWW.bytes;
                            if (bytes == null || bytes.Length == 0)
                            {
                                ResLoadHelper.Log("LoadClientResVersion bytes null or empty data;" + ResUpdateControler.s_ResVersionClientFile);
                                info.IsError = true;
                                ResUpdateControler.s_UpdateError = ResUpdateError.LoadClientResVersion_Byte_Error;
                                tWWW.Dispose();
                                yield break;
                            }
                            ClientResVersionProvider.Instance.CollectDataFromDBC(bytes);
                            ResVersionLoader.SaveClientResVersion();
                        }
                        catch (System.Exception ex)
                        {
                            ResLoadHelper.Log("LoadClientResVersion ex:" + ex);
                            info.IsError = false;
                            ResUpdateControler.s_UpdateError = ResUpdateError.LoadClientResVersion_Save_Error;
                            yield break;
                        }
                        finally
                        {
                            tWWW.Dispose();
                        }
                    }
                }
                else
                {
                    ClientResVersionProvider.Instance.Clear();
                    ClientResVersionProvider.Instance.CollectDataFromDBC(resVersionFilePersistPath);
                }
            }
            else
            {
                using (WWW tWWW = new WWW(buildinVersionFilePath))
                {
                    yield return tWWW;
                    try
                    {
                        if (tWWW.error != null)
                        {
                            ResLoadHelper.Log("LoadClientResVersion error");
                            info.IsError = true;
                            ResUpdateControler.s_UpdateError = ResUpdateError.LoadClientResVersion_WWW_Error;
                            tWWW.Dispose();
                            yield break;
                        }
                        byte[] bytes = tWWW.bytes;
                        if (bytes == null || bytes.Length == 0)
                        {
                            ResLoadHelper.Log("LoadClientResVersion bytes null or empty data;" + ResUpdateControler.s_ResVersionClientFile);
                            info.IsError = true;
                            ResUpdateControler.s_UpdateError = ResUpdateError.LoadClientResVersion_Byte_Error;
                            tWWW.Dispose();
                            yield break;
                        }
                        if (!string.IsNullOrEmpty(resVersionFilePersistPath))
                        {
                            string dir = Path.GetDirectoryName(resVersionFilePersistPath);
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }
                            if (null != bytes)
                            {
                                File.WriteAllBytes(resVersionFilePersistPath, bytes);
                            }
                            else
                            {
                                ResUpdateControler.s_UpdateError = ResUpdateError.LoadClientResVersion_Byte_Error;
                                ResLoadHelper.Log("LoadClientResVersion bytes null or empty data;" + ResUpdateControler.s_ResVersionClientFile);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        ResLoadHelper.Log("LoadClientResVersion ex:" + ex);
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.LoadClientResVersion_Save_Error;
                        yield break;
                    }
                    finally
                    {
                        tWWW.Dispose();
                    }
                }

                ClientResVersionProvider.Instance.Clear();
                ClientResVersionProvider.Instance.CollectDataFromDBC(resVersionFilePersistPath);
            }

            info.IsDone = true;
            info.Progress = 1.0f;
        }
        public static void SaveClientResVersion()
        {
            if (ClientResVersionProvider.Instance != null)
            {
                string fileContent = ResUpdateControler.s_ResVersionClientHeader + "\n";
                foreach (ClientResVersionData data in ClientResVersionProvider.Instance.GetArray().Values)
                {
                    if (data != null)
                    {
                        string dataStr = string.Format(ResUpdateControler.s_ResVersionClientFormat + "\n",
                          data.m_Name, data.m_MD5, data.m_IsBuildIn);
                        fileContent += dataStr;
                    }
                }
                try
                {
                    string resVersionFilePersistPath = Path.Combine(UnityEngine.Application.persistentDataPath,
                        ResUpdateControler.s_ResCachePath + ResUpdateControler.s_ResVersionClientFile);
                    File.WriteAllText(resVersionFilePersistPath, fileContent, Encoding.UTF8);
                }
                catch (System.Exception ex)
                {
                    ResUpdateControler.s_UpdateError = ResUpdateError.ClientResVersion_Save_Error;
                    ResLoadHelper.Log("SaveClientResVersion file failed!" + ex);
                }
            }
        }
    }
}
