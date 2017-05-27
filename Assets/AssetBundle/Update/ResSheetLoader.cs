using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace ArkCrossEngine
{
    class ResSheetLoader
    {
        public static ResAsyncInfo RequestResSheet()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(RequestResSheetAsync(info));
            return info;
        }
        public static ResAsyncInfo LoadResSheet()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(LoadResSheetAsync(info));
            return info;
        }
        private static IEnumerator RequestResSheetAsync(ResAsyncInfo info)
        {
            if (UnityEngine.Application.internetReachability == NetworkReachability.NotReachable)
            {
                ResLoadHelper.Log("RequestResSheetList network not reachable:");
                info.IsError = true;
                ResUpdateControler.s_UpdateError = ResUpdateError.Network_Error;
                yield break;
            }

            string requestResSheetUrl = ResLoadHelper.GetResSheetFileURL();
            requestResSheetUrl = ResLoadHelper.GetDynamicUrl(requestResSheetUrl);
            ResLoadHelper.Log("RequestResSheet URL;" + requestResSheetUrl);
            using (WWW tWWW = new WWW(requestResSheetUrl))
            {
                yield return tWWW;
                try
                {
                    if (tWWW.error != null)
                    {
                        ResLoadHelper.Log("RequestResSheetList error");
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.RequestResSheet_WWW_Error;
                        tWWW.Dispose();
                        yield break;
                    }
                    byte[] buffer = tWWW.bytes;
                    if (buffer == null || buffer.Length < 0)
                    {
                        ResLoadHelper.Log("RequestResSheetList tWWW.byte null");
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.RequestResSheet_Byte_Error;
                        tWWW.Dispose();
                        yield break;
                    }
                    ResUpdateControler.IsResSheetConfigCached = true;
                    ResUpdateTool.SaveCacheAB(buffer, ResUpdateControler.s_ResSheetZip, "");
                }
                catch (System.Exception ex)
                {
                    ResLoadHelper.Log("RequestResSheet ex:" + ex);
                    info.IsError = true;
                    ResUpdateControler.s_UpdateError = ResUpdateError.RequestResSheet_Save_Error;
                }
                finally
                {
                    tWWW.Dispose();
                }
            }

            ResAsyncInfo loadResInfo = LoadResSheetImpl();
            if (loadResInfo.CurCoroutine != null)
            {
                yield return loadResInfo.CurCoroutine;
            }
            if (loadResInfo.IsError)
            {
                ResLoadHelper.Log("RequestResSheet LoadResSheetImpl failed");
                info.IsError = true;
                yield break;
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
        private static IEnumerator LoadResSheetAsync(ResAsyncInfo info)
        {
            if (ResUpdateControler.IsResSheetConfigCached)
            {
                info.IsDone = true;
                info.Progress = 1.0f;
                yield break;
            }
            string url = ResLoadHelper.GetBuildInResSheetFileURL();
            ResLoadHelper.Log("LoadResSheet url:" + url);
            AssetBundle assetBundle = null;
            if (!ResLoadHelper.IsConfigABCached(ResUpdateControler.s_ResSheetZip))
            {
                using (WWW tWWW = new WWW(url))
                {
                    yield return tWWW;
                    try
                    {
                        if (tWWW.error != null)
                        {
                            ResLoadHelper.Log("LoadResSheet www error url:" + url);
                            tWWW.Dispose();
                            ResUpdateControler.s_UpdateError = ResUpdateError.LoadResSheet_WWW_Error;
                            yield break;
                        }
                        byte[] buffer = tWWW.bytes;
                        if (buffer == null || buffer.Length < 0)
                        {
                            ResLoadHelper.Log("LoadResSheet tWWW.byte null");
                            info.IsError = true;
                            ResUpdateControler.s_UpdateError = ResUpdateError.LoadResSheet_Byte_Error;
                            tWWW.Dispose();
                            yield break;
                        }
                        ResUpdateTool.SaveCacheAB(buffer, ResUpdateControler.s_ResSheetZip, "");
                        ResUpdateControler.IsResSheetConfigCached = true;
                    }
                    catch (System.Exception ex)
                    {
                        ResLoadHelper.Log("LoadResSheet ab failed url:" + url + "ex:" + ex);
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.LoadResSheet_Save_Error;
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
            ResAsyncInfo loadResInfo = LoadResSheetImpl();
            if (loadResInfo.CurCoroutine != null)
            {
                yield return loadResInfo.CurCoroutine;
            }
            if (loadResInfo.IsError)
            {
                ResLoadHelper.Log("RequestResSheet LoadResSheetImpl failed");
                info.IsError = true;
                yield break;
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
        private static ResAsyncInfo LoadResSheetImpl()
        {
            ResAsyncInfo info = new ResAsyncInfo();

            string url = ResLoadHelper.GetCachedResSheetFileURL();
            AssetBundle assetBundle = null;
            try
            {
                assetBundle = ResUpdateTool.LoadAssetBundle(url);
                if (assetBundle == null)
                {
                    ResLoadHelper.Log("LoadResSheet assetBundleObj null url:" + url);
                    ResUpdateControler.s_UpdateError = ResUpdateError.LoadResSheet_Assetbundle_Error;
                    info.IsError = true;
                    return info;
                }
                info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResUpdateTool.ExtractResSheet(info, assetBundle));
            }
            catch (System.Exception ex)
            {
                ResLoadHelper.Log("LoadResSheet res failed url:" + url + " ex:" + ex);
                ResUpdateControler.IsResSheetConfigCached = false;
                ResUpdateControler.s_UpdateError = ResUpdateError.LoadResSheet_Extract_Error;
                info.IsError = true;
            }
            return info;
        }
    }
}
