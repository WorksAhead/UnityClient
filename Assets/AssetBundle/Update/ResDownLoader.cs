using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace ArkCrossEngine
{
    class ResDownLoader
    {
        public static ResAsyncInfo StartDownload()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(StartDownloadAsync(info));
            return info;
        }
        public static ResAsyncInfo DownLoadResById(int abId)
        {
            ResAsyncInfo info = new ResAsyncInfo();
            ResVersionData data = ResVersionProvider.Instance.GetDataById(abId);
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(DownLoadResAsync(data, info));
            return info;
        }
        public static ResAsyncInfo DownLoadRes(ResVersionData data)
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(DownLoadResAsync(data, info));
            return info;
        }
        private static IEnumerator StartDownloadAsync(ResAsyncInfo info)
        {
            List<int> toUpgradeRes = new List<int>();
            ResUpdateTool.SearchUpgradeRes(toUpgradeRes);
            if (toUpgradeRes != null && toUpgradeRes.Count > 0)
            {
                DateTime tStartDownload = DateTime.Now;
                long totalSize = ResLoadHelper.CaculateABSize(toUpgradeRes);
                long accSize = 0;
                List<ResAsyncInfo> asyncList = new List<ResAsyncInfo>();
                List<ResAsyncInfo> asyncDoneList = new List<ResAsyncInfo>();
                int index = 0;
                while (true)
                {
                    while (asyncList.Count < ResUpdateControler.s_AsyncCoroutineMax && index < toUpgradeRes.Count)
                    {
                        int abId = toUpgradeRes[index++];
                        ResAsyncInfo loadAssetInfo = DownLoadResById(abId);
                        loadAssetInfo.Target = (System.Object)(abId);
                        asyncList.Add(loadAssetInfo);
                    }
                    asyncDoneList.Clear();
                    foreach (ResAsyncInfo loadAssetInfo in asyncList)
                    {
                        if (loadAssetInfo.IsError)
                        {
                            ResLoadHelper.Log("更新资源错误 ab:" + loadAssetInfo.Tip);
                            info.IsError = true;
                            ResUpdateControler.s_UpdateError = ResUpdateError.ResDownloader_Asset_Error;
                            yield break;
                        }
                        else if (loadAssetInfo.IsDone)
                        {
                            int abId = (int)loadAssetInfo.Target;
                            ResVersionData data = ResVersionProvider.Instance.GetDataById(abId);
                            if (data != null)
                            {
                                accSize += data.m_Size;
                            }
                            double elapsed = (DateTime.Now - tStartDownload).TotalSeconds;
                            if (elapsed < double.Epsilon)
                            {
                                elapsed = double.Epsilon;
                            }
                            double speed = accSize / elapsed;
                            info.Progress = 0.5f + 0.5f * (float)(accSize / ((totalSize > 0) ? totalSize : double.Epsilon));
                            string tipFormat = Dict.Get(22);
                            if (string.IsNullOrEmpty(tipFormat))
                            {
                                tipFormat = "正在下载资源，已下载{0}M/{1}M，下载速度{2}";
                            }
                            info.Tip = string.Format(tipFormat,
                              ResUpdateTool.FormatNumber((float)accSize / (1024 * 1024)),
                              ResUpdateTool.FormatNumber((float)totalSize / (1024 * 1024)),
                              ResUpdateTool.FormatSpeed(speed));
                            ResUpdateControler.OnUpdateProgress(info.Progress);
                            ResUpdateControler.OnUpdateTip(info.Tip);

                            asyncDoneList.Add(loadAssetInfo);
                        }
                    }
                    foreach (ResAsyncInfo loadAssetInfo in asyncDoneList)
                    {
                        asyncList.Remove(loadAssetInfo);
                    }
                    if (ResUpdateControler.DownLoadNum >= 20)
                    {
                        ResVersionLoader.SaveClientResVersion();
                        ResUpdateControler.DownLoadNum = 0;
                    }
                    asyncDoneList.Clear();
                    if (asyncList.Count == 0 && index >= toUpgradeRes.Count)
                    {
                        break;
                    }
                    yield return 1;
                }
                ResUpdateControler.OnUpdateProgress(info.Progress);
                ResUpdateControler.OnUpdateRandomTip();
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
        private static IEnumerator DownLoadResAsync(ResVersionData data, ResAsyncInfo info)
        {
            if (UnityEngine.Application.internetReachability == NetworkReachability.NotReachable)
            {
                ResLoadHelper.Log("DownLoadResAsync network not reachable:");
                info.IsError = true;
                ResUpdateControler.s_UpdateError = ResUpdateError.Network_Error;
                yield break;
            }

            string url = ResLoadHelper.GetResABURL(data);
            info.Tip = data.m_AssetBundleName;
            url = ResLoadHelper.GetDynamicUrl(url);
            using (WWW tWWW = new WWW(url))
            {
                //tWWW.threadPriority = ThreadPriority.High;
                yield return tWWW;
                try
                {
                    if (tWWW.error != null)
                    {
                        ResLoadHelper.Log("DownLoadRes error data:" + data.m_AssetBundleName + " Url:" + url);
                        info.IsError = true;
                        ResUpdateControler.s_UpdateError = ResUpdateError.ResDownloader_WWW_Error;
                        tWWW.Dispose();
                        yield break;
                    }
                    //NOTE:累加下载数，用于保存本地资源列表,by lixiaojiang
                    bool isUnzip = !data.m_AssetName.EndsWith(".unity");
                    if (!ResUpdateTool.SaveCacheAB(tWWW.bytes, data.m_AssetBundleName, data.m_MD5, isUnzip))
                    {
                        throw new Exception(string.Format("DownLoadRes save ab failed. url:{0} abName:{1}", url, data.m_AssetBundleName));
                    }
                }
                catch (System.Exception ex)
                {
                    ResLoadHelper.Log("DownLoadRes ex:" + ex);
                    info.IsError = true;
                    ResUpdateControler.s_UpdateError = ResUpdateError.ResDownloader_Save_Error;
                }
                finally
                {
                    tWWW.Dispose();
                }
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
    }
}
