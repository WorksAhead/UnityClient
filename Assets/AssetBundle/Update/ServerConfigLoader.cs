using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace ArkCrossEngine
{
    class ServerConfigLoader
    {
        public static ResAsyncInfo RequestServerList()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(RequestServerListAsync(info));
            return info;
        }
        private static IEnumerator RequestServerListAsync(ResAsyncInfo info)
        {
            if (UnityEngine.Application.internetReachability == NetworkReachability.NotReachable)
            {
                ResLoadHelper.Log("RequestServerList network not reachable:");
                info.IsError = true;
                ResUpdateControler.s_UpdateError = ResUpdateError.Network_Error;
                yield break;
            }

            ResUpdateCallback.OnStartRequestServerList();
            string requestServerListUrl = ResLoadHelper.GetServerListFileURL();
            requestServerListUrl = ResLoadHelper.GetDynamicUrl(requestServerListUrl);
            string persistServerListPath = Path.Combine(UnityEngine.Application.persistentDataPath,
              ResUpdateControler.s_ResSheetCachePath + FilePathDefine_Client.C_ServerConfig);
            ResLoadHelper.Log("RequestServerList URL;" + requestServerListUrl + " persistServerListPath:" + persistServerListPath);
            using (WWW tWWW = new WWW(requestServerListUrl))
            {
                yield return tWWW;
                try
                {
                    if (tWWW.error != null)
                    {
                        ResLoadHelper.Log("RequestServerList error");
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.RequestServerList_WWW_Error;
                        tWWW.Dispose();
                        yield break;
                    }
                    byte[] bytes = tWWW.bytes;
                    if (bytes == null || bytes.Length == 0)
                    {
                        ResLoadHelper.Log("RequestServerVersion bytes null or empty data;" + ResUpdateControler.s_ServerListFile);
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.RequestServerList_Byte_Error;
                        tWWW.Dispose();
                        yield break;
                    }
                    string dir = Path.GetDirectoryName(persistServerListPath);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    File.WriteAllBytes(persistServerListPath, bytes);
                }
                catch (System.Exception ex)
                {
                    ResLoadHelper.Log("RequestServerList ex:" + ex);
                    info.IsError = true;
                    ResUpdateControler.s_UpdateError = ResUpdateError.RequestServerList_Save_Error;
                }
                finally
                {
                    tWWW.Dispose();
                }
            }
            ResUpdateCallback.OnEndRequestServerList();

            info.IsDone = true;
            info.Progress = 1.0f;
        }
    }
}
