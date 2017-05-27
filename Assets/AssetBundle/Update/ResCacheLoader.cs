using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace ArkCrossEngine
{
    class ResCacheLoader
    {
        public static ResAsyncInfo RequestResCache()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(RequestResCacheAsync(info));
            return info;
        }
        public static ResAsyncInfo LoadResCache()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(LoadResCacheAsync(info));
            return info;
        }
        private static IEnumerator RequestResCacheAsync(ResAsyncInfo info)
        {
            if (UnityEngine.Application.internetReachability == NetworkReachability.NotReachable)
            {
                ResLoadHelper.Log("RequestResCacheList network not reachable:");
                info.IsError = true;
                ResUpdateControler.s_UpdateError = ResUpdateError.Network_Error;
                yield break;
            }

            string requestResCacheUrl = ResLoadHelper.GetResCacheFileURL();
            requestResCacheUrl = ResLoadHelper.GetDynamicUrl(requestResCacheUrl);
            ResLoadHelper.Log("RequestResCache URL;" + requestResCacheUrl);
            using (WWW tWWW = new WWW(requestResCacheUrl))
            {
                yield return tWWW;
                try
                {
                    if (tWWW.error != null)
                    {
                        ResLoadHelper.Log("RequestResCacheList error");
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.RequestResCache_WWW_Error;
                        tWWW.Dispose();
                        yield break;
                    }
                    byte[] buffer = tWWW.bytes;
                    if (buffer == null || buffer.Length < 0)
                    {
                        ResLoadHelper.Log("RequestResCacheList tWWW.byte null");
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.RequestResCache_Byte_Error;
                        tWWW.Dispose();
                        yield break;
                    }
                    ResUpdateControler.IsResCacheConfigCached = true;
                    ResUpdateTool.SaveCacheAB(buffer, ResUpdateControler.s_ResCacheZip, "");
                }
                catch (System.Exception ex)
                {
                    ResLoadHelper.Log("RequestResCache ex:" + ex);
                    info.IsError = true;
                    ResUpdateControler.s_UpdateError = ResUpdateError.RequestResCache_Save_Error;
                }
                finally
                {
                    tWWW.Dispose();
                }
            }
            if (!LoadResCacheImpl())
            {
                ResLoadHelper.Log("RequestResCache LoadResCacheImpl failed");
                info.IsError = true;
                yield break;
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
        private static IEnumerator LoadResCacheAsync(ResAsyncInfo info)
        {
            string url = ResLoadHelper.GetBuildInResCacheFileURL();
            ResLoadHelper.Log("LoadResCache url:" + url);
            AssetBundle assetBundle = null;
            if (!ResLoadHelper.IsConfigABCached(ResUpdateControler.s_ResCacheZip))
            {
                using (WWW tWWW = new WWW(url))
                {
                    yield return tWWW;
                    try
                    {
                        if (tWWW.error != null)
                        {
                            ResLoadHelper.Log("LoadResCache www error url:" + url);
                            tWWW.Dispose();
                            ResUpdateControler.s_UpdateError = ResUpdateError.LoadResCache_WWW_Error;
                            yield break;
                        }
                        byte[] buffer = tWWW.bytes;
                        if (buffer == null || buffer.Length < 0)
                        {
                            ResLoadHelper.Log("LoadResCache tWWW.byte null");
                            info.IsError = true;
                            ResUpdateControler.s_UpdateError = ResUpdateError.LoadResCache_Byte_Error;
                            tWWW.Dispose();
                            yield break;
                        }
                        ResUpdateControler.IsResCacheConfigCached = true;
                        ResUpdateTool.SaveCacheAB(buffer, ResUpdateControler.s_ResCacheZip, "");
                    }
                    catch (System.Exception ex)
                    {
                        ResLoadHelper.Log("LoadResCache ab failed url:" + url + "ex:" + ex);
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.LoadResCache_Save_Exception;
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
            if (!LoadResCacheImpl())
            {
                ResLoadHelper.Log("RequestResCache LoadResCacheImpl failed");
                info.IsError = true;
                yield break;
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
        private static bool LoadResCacheImpl()
        {
            string url = ResLoadHelper.GetCachedResCacheFileURL();
            AssetBundle assetBundle = null;
            try
            {
                assetBundle = ResUpdateTool.LoadAssetBundle(url);
                if (assetBundle == null)
                {
                    ResLoadHelper.Log("LoadResCacheImpl assetBundleObj null url:" + url);
                    ResUpdateControler.s_UpdateError = ResUpdateError.LoadResCache_Assetbundle_Error;
                    return false;
                }
                throw new CodeNotImplException();
                //                 TextAsset txt = assetBundle.Load(
                //                       ResUpdateControler.s_ResCacheFile, typeof(UnityEngine.TextAsset)) as TextAsset;
                //                 if (txt == null || txt.bytes == null || txt.bytes.Length <= 0)
                //                 {
                //                     ResLoadHelper.Log("LoadResCacheImpl bytes null");
                //                     ResUpdateControler.s_UpdateError = ResUpdateError.LoadResCache_Assetbundle_Load_Error;
                //                     return false;
                //                 }
                //                 byte[] bytes = txt.bytes;
                //                 ResCacheProvider.Instance.Clear();
                //                 ResCacheProvider.Instance.CollectDataFromDBC(bytes);
            }
            catch (System.Exception ex)
            {
                ResLoadHelper.Log("LoadResCacheImpl res failed url:" + url + " ex:" + ex);
                ResUpdateControler.s_UpdateError = ResUpdateError.LoadResCache_Collect_Exception;
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
    }
}
