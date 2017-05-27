using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace ArkCrossEngine
{
    class NoticeConfigLoader
    {
        // FixMe: replace this url to config file
        public static string s_NoticeConfigUrl = "http://10.1.9.84:8080/ArkCross/Notice.txt";
        public static string s_NoticeContent = string.Empty;

        public static ResAsyncInfo RequestNoticeConfig()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(RequestNoticeConfigAsync(info));
            return info;
        }

        private static IEnumerator RequestNoticeConfigAsync(ResAsyncInfo info)
        {
            // if network not reachable, just skip
            if (UnityEngine.Application.internetReachability == NetworkReachability.NotReachable)
            {
                ResLoadHelper.Log("RequestNoticeConfig network not reachable:");
                info.IsError = true;
                ResUpdateControler.s_UpdateError = ResUpdateError.Network_Error;
                yield break;
            }

            // fire callback
            ResUpdateCallback.OnStartRequestNoticeConfig();
            s_NoticeContent = string.Empty;

            // try get notify message from server
            string requestNoticeConfigUrl = s_NoticeConfigUrl;
            requestNoticeConfigUrl = ResLoadHelper.GetDynamicUrl(requestNoticeConfigUrl);
            ResLoadHelper.Log("RequestNoticeConfig URL;" + requestNoticeConfigUrl);
            using (WWW tWWW = new WWW(requestNoticeConfigUrl))
            {
                yield return tWWW;
                try
                {
                    if (tWWW.error != null)
                    {
                        ResLoadHelper.Log("RequestNoticeConfig error");
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.RequestNoticeConfig_WWW_Error;
                        tWWW.Dispose();
                        yield break;
                    }

                    s_NoticeContent = tWWW.text;
                }
                catch (System.Exception ex)
                {
                    ResLoadHelper.Log("RequestNoticeConfig ex:" + ex);
                    info.IsError = true;
                    ResUpdateControler.s_UpdateError = ResUpdateError.RequestNoticeConfig_Save_Error;
                }
                finally
                {
                    tWWW.Dispose();
                }
            }

            // finished
            ResUpdateCallback.OnEndRequestNoticeConfig();

            info.IsDone = true;
            info.Progress = 1.0f;
        }
    }
}
