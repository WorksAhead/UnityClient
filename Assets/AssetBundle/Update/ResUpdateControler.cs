using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using System.Collections;

namespace ArkCrossEngine
{
    public class ResUpdateControler
    {
        public static bool s_EnableResServer = true;
        public static bool s_EnableResServerSkip = true;
        public static bool s_UseNewWWW = false;
        public static bool s_UseDynmaicUrl = true;
        public static bool s_EnableZip = true;
        public static bool s_EnableWeakCheck = true;

        public static long s_CachingSpaceMin = 3 * 1024 * 1024;
        public static float s_ProgressUpgrade = 0.2f;
        public static float s_ProgressCacheGlobal = 0.2f;
        public static float s_ProgressCacheMainMenu = 0.2f;
        public static string[] s_ConfigSplit = new string[] { ";", "|", };
        public static int s_AsyncCoroutineMax = 2; //HardWareQuality中配置
                                                   //public static int s_AsyncExtractCoroutineMax = 10; //HardWareQuality中配置
        public static int s_LoadSceneId = 0; //配置在SceneConfig.list中
        public static string s_ChannelName = "cyou";
#if UNITY_ANDROID
    public static string s_DefaultForceDownloadUrl = "http://mrd.changyou.com/df/dfm_{0}_{1}_ab.apk";
#elif UNITY_IPHONE
    public static string s_DefaultForceDownloadUrl = "itms-services://?action=download-manifest&amp;url=https://topic.changyou.com/mjzr/dfm_{0}_{1}_ab.plist";
#else
        public static string s_DefaultForceDownloadUrl = "http://mrd.changyou.com/df/download.html";
#endif
        public static ResUpdateError s_UpdateError = ResUpdateError.None;

        public static string s_VersionInfoFormat = "版本号:{0}.{1}.{2} ({3}.{4})";
        public static int s_ReconnectSkipNumMax = 3;

        #region  Define
        public static string s_ClientVersionFile = "ClientVersion.txt";
        public static string s_ServerVersionFile = "ServerVersion.txt";
        public static string s_ResVersionFile = "ResVersion_ab.txt";
        public static string s_ResVersionZip = "ResVersion.ab";
        public static string s_AssetDBFile = "AssetDB_ab.txt";
        public static string s_AssetDBZip = "AssetDB.ab";
        public static string s_ResCacheFile = "ResCache_ab.txt";
        public static string s_ResCacheZip = "ResCache.ab";
        public static string s_ServerListFile = "ServerConfig.txt";
        public static string s_NoticeConfigFile = "Notice.txt";
        public static string s_DynamicLevel = "Empty";
        public static string s_ResBuildInPath = "ResFile/";
        public static string s_ResCachePath = "ResFile/";

        public static string s_ResVersionClientFile = "ClientResVersion_ab.txt";
        public static string s_ResVersionClientFormat = "{0}	{1}	{2}";
        public static string s_ResVersionClientHeader = "Name	MD5	IsBuildIn";

        public static string s_ResSheetFile = "list.txt";
        public static string s_ResSheetZip = "ResSheet.ab";
        public static string s_ResSheetPattern = ".txt;.map;.dsl;.tmx";
        public static string s_ResSheetCachePath = "DataFile/";

        #endregion

        #region Config
        public static string ClientVersion
        {
            get { return ClientVersionInfo.Version.GetVersionStr(); }
            set
            {
                ClientVersionInfo.Version = new VersionNum(value);
                ClientVersionInfo.Save();
            }
        }
        public static string Platform
        {
            get { return ClientVersionInfo.Platform; }
            set { ClientVersionInfo.Platform = value; ClientVersionInfo.Save(); }
        }
        public static string AppName
        {
            get { return ClientVersionInfo.AppName; }
            set { ClientVersionInfo.AppName = value; ClientVersionInfo.Save(); }
        }
        public static string Channel
        {
            get { return ClientVersionInfo.Channel; }
            set { ClientVersionInfo.Channel = value; ClientVersionInfo.Save(); }
        }
        public static string ResServerURL
        {
            get { return ClientVersionInfo.ResServerURL; }
            set { ClientVersionInfo.ResServerURL = value; ClientVersionInfo.Save(); }
        }
        public static int CurChapter
        {
            get { return ClientVersionInfo.CurChapter; }
            set { ClientVersionInfo.CurChapter = value; ClientVersionInfo.Save(); }
        }
        public static bool IsResVersionConfigCached
        {
            get { return ClientVersionInfo.IsResVersionConfigCached; }
            set { ClientVersionInfo.IsResVersionConfigCached = value; ClientVersionInfo.Save(); }
        }
        public static bool IsAssetDBConfigCached
        {
            get { return ClientVersionInfo.IsAssetDBConfigCached; }
            set { ClientVersionInfo.IsAssetDBConfigCached = value; ClientVersionInfo.Save(); }
        }
        public static bool IsResCacheConfigCached
        {
            get { return ClientVersionInfo.IsResCacheConfigCached; }
            set { ClientVersionInfo.IsResCacheConfigCached = value; ClientVersionInfo.Save(); }
        }
        public static bool IsResSheetConfigCached
        {
            get { return ClientVersionInfo.IsResSheetConfigCached; }
            set { ClientVersionInfo.IsResSheetConfigCached = value; ClientVersionInfo.Save(); }
        }
        #endregion

        public static VersionInfo BuildinClientVersionInfo;
        public static VersionInfo PersistClientVersionInfo;
        public static VersionInfo ClientVersionInfo;
        public static VersionInfo ServerVersionInfo;
        public static bool IsNeedUpdate;
        public static bool IsNeedPauseUpdate;
        public static bool IsNeedLoadConfig;
        public static int DownLoadNum;
        public static float ProgressStart;
        public static float ProgressMax;
        public static int ProgressBarType;
        public static int TargetChapter;
        public static bool IsNeedSyncPackage;
        public static int ReconnectNum;

        public delegate void EventUpdateFailed();
        public static EventUpdateFailed HandleUpdateFailed;

        public static bool InitContext()
        {
            ResUpdateHandler.HandleStartUpdate = StartUpdate;
            ResUpdateHandler.HandleStartUpdateChapter = StartUpdateChapter;
            ResUpdateHandler.HandleExitUpdate = ExitUpdate;
            ResUpdateHandler.HandleSetUpdateProgressRange = SetUpdateProgressRange;
            ResUpdateHandler.HandleCacheResByConfig = ResLevelLoader.LoadLevelAsync;

            ResUpdateHandler.HandleGetEnableResServerSkip = GetEnableResServerSkip;
            ResUpdateHandler.HandleGetReconnectSkipNumMax = GetReconnectSkipNumMax;
            ResUpdateHandler.HandleGetReconnectNum = GetReconnectNum;
            ResUpdateHandler.HandleIncReconnectNum = IncReconnectNum;
            ResUpdateHandler.HandleResetReconnectNum = ResetReconnectNum;

            ResUpdateHandler.HandleLoadAssetFromABWithoutExtention = AssetExManager.Instance.GetAssetByNameWithoutExtention;
            ResUpdateHandler.HandleReleaseAllAssetBundle = AssetExManager.Instance.ClearAllAssetBundle;
            ResUpdateHandler.HandleCleanup = AssetExManager.Instance.Cleanup;

            ResUpdateHandler.HandleGetUpdateError = GetUpdateError;

            IsNeedLoadConfig = true;

            ClientResVersionProvider.Instance.Clear();
            return true;
        }
        public static ResAsyncInfo StartUpdate(int targetChapter)
        {
            TargetChapter = targetChapter;
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResUpdateControlerHandler.StartUpdate(info));
            return info;
        }
        public static ResAsyncInfo StartUpdateChapter(int targetChapter)
        {
            ResAsyncInfo info = new ResAsyncInfo();
            if (targetChapter > ResUpdateControler.CurChapter)
            {
                TargetChapter = targetChapter;
                info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResUpdateControlerHandler.StartUpdate(info));
            }
            else
            {
                info.IsDone = true;
                info.Progress = 1.0f;
                info.CurCoroutine = null;
            }
            return info;
        }
        public static void InitUpdate()
        {
            IsNeedUpdate = false;
            IsNeedPauseUpdate = false;
            DownLoadNum = 0;
            ProgressStart = 0;
            ProgressMax = 0;
            ProgressBarType = 0;
            HandleUpdateFailed = null;
            s_UpdateError = ResUpdateError.None;
            SetUpdateProgressRange(0.0f, 1.0f);
        }
        public static void ExitUpdate()
        {
            IsNeedUpdate = false;
            DownLoadNum = 0;
            ProgressStart = 0;
            ProgressMax = 0;
            ProgressBarType = 0;
            HandleUpdateFailed = null;
            TargetChapter = 0;
            s_UpdateError = ResUpdateError.None;
            SetUpdateProgressRange(0.0f, 1.0f);
        }
        public static void SetUpdateProgressRange(float start, float max, int progressBarType = 0)
        {
            ProgressStart = start;
            ProgressMax = max;
            ProgressBarType = progressBarType;
        }
        public static bool GetEnableResServerSkip()
        {
            return s_EnableResServerSkip;
        }
        public static int GetReconnectSkipNumMax()
        {
            return s_ReconnectSkipNumMax;
        }
        public static int GetReconnectNum()
        {
            return ReconnectNum;
        }
        public static void IncReconnectNum()
        {
            ReconnectNum++;
        }
        public static void ResetReconnectNum()
        {
            ReconnectNum = 0;
        }
        internal static ResAsyncInfo DetectVersion()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResUpdateControlerHandler.DetectVersion(info));
            return info;
        }
        internal static ResAsyncInfo RequestConfig()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResUpdateControlerHandler.RequestConfig(info));
            return info;
        }
        internal static ResAsyncInfo LoadConfigAsync()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResUpdateControlerHandler.LoadConfigAsync(info));
            return info;
        }
        internal static void OnUpdateFailed()
        {
            if (HandleUpdateFailed != null)
            {
                HandleUpdateFailed();
            }
        }
        internal static void OnUpdateTip(string tip)
        {
            LogicSystem.UpdateLoadingTip(tip);
        }
        internal static void OnUpdateRandomTip()
        {
            LogicSystem.UpdateRandomLoadingTip();
        }
        internal static void OnUpdateProgress(float progress)
        {
            LogicSystem.UpdateLoadingProgress(ProgressStart + ProgressMax * progress);
        }
        internal static int GetUpdateError()
        {
            return (int)s_UpdateError;
        }
        internal static void OnUpdateVersionInfo()
        {
            if (ClientVersionInfo == null || ClientVersionInfo.Version == null)
            {
                return;
            }
            VersionNum cVersionNum = ClientVersionInfo.Version;
            string versionInfo = string.Format(s_VersionInfoFormat,
                  cVersionNum.AppChannel, cVersionNum.AppMaster, cVersionNum.ResBase, CurChapter, (double)(CodeVersion.Value));
            LogicSystem.UpdateVersinoInfo(versionInfo);
            ResLoadHelper.Log("OnUpdateVersionInfo:" + versionInfo);
            ResUpdateCallback.OnUpdateVersionNum(versionInfo);
        }
        internal static void OnUpdateCompleted(bool isdownload)
        {
            if (isdownload)
            {
                ResUpdateControler.ClientVersion = ResUpdateControler.ServerVersionInfo.Version.GetVersionStr();
                ResUpdateControler.CurChapter = ResUpdateControler.TargetChapter;
                if (ResUpdateControler.DownLoadNum > 0)
                {
                    ResVersionLoader.SaveClientResVersion();
                    ResUpdateControler.DownLoadNum = 0;
                }
                ResUpdateControler.OnUpdateVersionInfo();
            }
            ReconnectNum = 0;
            IsNeedSyncPackage = false;
        }
        internal static void OnForceDownload(bool selected)
        {
            if (selected)
            {
                string downloadUrlFormat = s_DefaultForceDownloadUrl;
                if (ServerVersionInfo != null && !string.IsNullOrEmpty(ServerVersionInfo.ForceDownloadURL))
                {
                    downloadUrlFormat = ServerVersionInfo.ForceDownloadURL;
                }
#if UNITY_IPHONE
        string channelName = ResUpdateTool.GetChannelName();
        string downloadUrl = string.Format(downloadUrlFormat, Channel, channelName) + "&amp;time=" + DateTime.Now.ToString("yyyyMMddhhmmss");
#elif UNITY_ANDROID
        string channelName = ResUpdateTool.GetChannelName();
        string downloadUrl = string.Format(downloadUrlFormat, Channel, channelName);
#else
                string downloadUrl = downloadUrlFormat;
#endif
                ResLoadHelper.Log("OnForceDownload:" + downloadUrl);
                UnityEngine.Application.OpenURL(downloadUrl);
                UnityEngine.Application.Quit();
            }
        }
    }
    /// <summary>
    /// Unity 4.2.2不支持在dll的类中混合IEnumerator和普通函数, 
    /// 因此，将包含yield的函数分离开来，by lixiaojiang
    /// Bug:http://lists.ximian.com/pipermail/mono-bugs/2011-July/112237.html
    /// </summary>
    internal class ResUpdateControlerHandler
    {
        internal static IEnumerator StartUpdate(ResAsyncInfo info)
        {
            if (ResUpdateControler.IsNeedUpdate)
            {
                ResUpdateCallback.OnStartUpdate();

                ResUpdateControler.OnUpdateProgress(0.1f);
                ResUpdateControler.IsNeedLoadConfig = false;
                ResAsyncInfo loadClientResVersionInfo = ResVersionLoader.LoadClientResVersion();
                yield return loadClientResVersionInfo.CurCoroutine;
                if (loadClientResVersionInfo.IsError)
                {
                    ResLoadHelper.Log("加载本地资源列表错误");
                    info.IsError = true;
                    yield break;
                }
                ResUpdateControler.OnUpdateProgress(0.2f);

                ResAsyncInfo requestConfigInfo = ResUpdateControler.RequestConfig();
                yield return requestConfigInfo.CurCoroutine;
                if (requestConfigInfo.IsError)
                {
                    ResLoadHelper.Log("请求版本信息错误");
                    info.IsError = true;
                    yield break;
                }

                ResUpdateControler.OnUpdateProgress(0.5f);
                ResAsyncInfo startDownloadInfo = ResDownLoader.StartDownload();
                yield return startDownloadInfo.CurCoroutine;
                if (startDownloadInfo.IsError)
                {
                    ResLoadHelper.Log("更新资源错误");
                    info.IsError = true;
                    yield break;
                }
                ResUpdateControler.OnUpdateCompleted(true);
                ResUpdateCallback.OnEndUpdate();
            }
            else if (ResUpdateControler.IsNeedLoadConfig)
            {
                ResUpdateControler.OnUpdateProgress(0.1f);
                ResUpdateControler.IsNeedLoadConfig = false;
                ResAsyncInfo loadClientResVersionInfo = ResVersionLoader.LoadClientResVersion();
                yield return loadClientResVersionInfo.CurCoroutine;
                if (loadClientResVersionInfo.IsError)
                {
                    ResLoadHelper.Log("加载本地资源列表错误");
                    info.IsError = true;
                    yield break;
                }

                ResUpdateControler.OnUpdateProgress(0.2f);
                ResAsyncInfo loadConfigInfo = ResUpdateControler.LoadConfigAsync();
                yield return loadConfigInfo.CurCoroutine;
                if (loadConfigInfo.IsError)
                {
                    ResLoadHelper.Log("加载版本信息错误");
                    info.IsError = true;
                    yield break;
                }
                ResUpdateControler.OnUpdateCompleted(false);
            }

            string tip = Dict.Get(24);
            if (string.IsNullOrEmpty(tip))
            {
                tip = "正在获取服务器列表…";
            }
            ResUpdateControler.OnUpdateTip(tip);
            ResAsyncInfo requestServerListInfo = ServerConfigLoader.RequestServerList();
            yield return requestServerListInfo.CurCoroutine;
            if (requestServerListInfo.IsError)
            {
                UnityEngine.Debug.Log("获取服务器列表错误");
                ResUpdateControler.IsNeedPauseUpdate = true;
                info.IsError = true;
                yield break;
            }

            info.Progress = 1.0f;
            info.IsDone = true;
            ResUpdateControler.OnUpdateProgress(info.Progress);

        }
        internal static IEnumerator DetectVersion(ResAsyncInfo info)
        {
            ResUpdateCallback.OnStartDetectVersion();
            string tip = Dict.Get(25);
            if (string.IsNullOrEmpty(tip))
            {
                tip = "版本检测中…";
            }
            ResUpdateControler.OnUpdateTip(tip);

            if (ResUpdateControler.s_EnableResServerSkip
              && ResUpdateControler.GetReconnectNum() >= ResUpdateControler.GetReconnectSkipNumMax())
            {
                ResLoadHelper.Log("无法连接资源服务器，直接跳过资源更新！");
                ResUpdateControler.IsNeedUpdate = false;
            }
            else if (!ResUpdateControler.s_EnableResServer)
            {
                ResLoadHelper.Log("关闭资源服务器，要求资源都置于本地！");
                ResUpdateControler.IsNeedUpdate = false;
                yield break;
            }

            ResAsyncInfo loadClientVersionInfo = VersionLoader.RequestClientVersion();
            yield return loadClientVersionInfo.CurCoroutine;
            if (loadClientVersionInfo.IsError)
            {
                UnityEngine.Debug.Log("加载客户端版本信息错误");
                yield break;
            }

            //ResUpdateControler.OnUpdateProgress(0, "加载服务器版本...");
            ResAsyncInfo requestServerVersionInfo = VersionLoader.RequestServerVersion();
            yield return requestServerVersionInfo.CurCoroutine;
            if (requestServerVersionInfo.IsError)
            {
                ResLoadHelper.Log("加载服务器版本错误");
                info.IsError = true;
                yield break;
            }

            VersionNum serverVersionNum = ResUpdateControler.ServerVersionInfo.Version;
            VersionNum clientVersionNum = ResUpdateControler.ClientVersionInfo.Version;
            ResUpdateControler.IsNeedPauseUpdate = false;

            ResLoadHelper.Log(string.Format("ClientVersion:{0} ServerVersion:{1}",
              clientVersionNum.GetVersionStr(), serverVersionNum.GetVersionStr()));

            ResUpdateControler.IsNeedUpdate = false;
            if (clientVersionNum.GetAppVersionForceValue() < serverVersionNum.GetAppVersionForceValue())
            {
                ResUpdateControler.IsNeedPauseUpdate = true;

                string tipInfo = string.Format("有新版本可更新，版本号：" + serverVersionNum.GetVersionStr());
                ResLoadHelper.Log(tipInfo);

                Action<bool> fun = ResUpdateControler.OnForceDownload;
                string dlgTip = Dict.Get(5);
                if (string.IsNullOrEmpty(dlgTip))
                {
                    dlgTip = "版本过旧，你需要更新一下";
                }
                string dlgButton = Dict.Get(28);
                if (string.IsNullOrEmpty(dlgButton))
                {
                    dlgButton = "重试";
                }
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_yesornot", "ui", dlgTip, dlgButton, fun);
                //UnityEngine.Application.Quit();
            }
            else if (clientVersionNum.GetAppVersionForceValue() == serverVersionNum.GetAppVersionForceValue())
            {
                if (clientVersionNum.GetResVersionValue() < serverVersionNum.GetResVersionValue())
                {
                    //ResUpdateControler.OnUpdateProgress(0, );
                    ResLoadHelper.Log("检测到有资源需要更新:" + serverVersionNum.GetVersionStr());
                    ResUpdateControler.IsNeedUpdate = true;
                }
                else
                {
                    ResLoadHelper.Log("已经是最新版本:" + serverVersionNum.GetVersionStr());
                    //ResUpdateControler.OnUpdateProgress(0, "已经是最新版本:" + serverVersionNum.GetVersionStr());
                }
            }
            else
            {
                ResLoadHelper.Log("客户端版本高于服务器版本:" + serverVersionNum.GetVersionStr());
            }
            ResUpdateCallback.OnEndDetectVersion(ResUpdateControler.IsNeedUpdate);

            info.IsDone = true;
            info.Progress = 1.0f;
        }
        internal static IEnumerator RequestConfig(ResAsyncInfo info)
        {
            //ResUpdateControler.OnUpdateProgress(0, "请求版本资源...");
            ResAsyncInfo requestResVersionInfo = ResVersionLoader.RequestResVersion();
            yield return requestResVersionInfo.CurCoroutine;
            if (requestResVersionInfo.IsError)
            {
                ResLoadHelper.Log("请求版本资源错误");
                info.IsError = true;
                yield break;
            }
            //ResUpdateControler.OnUpdateProgress(0, "请求资源缓存列表...");
            ResAsyncInfo requestResCacheInfo = ResCacheLoader.RequestResCache();
            yield return requestResCacheInfo.CurCoroutine;
            if (requestResCacheInfo.IsError)
            {
                ResLoadHelper.Log("请求资源缓存列表错误");
                info.IsError = true;
                yield break;
            }
            ResUpdateControler.OnUpdateProgress(0.3f);
            //ResUpdateControler.OnUpdateProgress(0, "请求表格资源...");
            ResAsyncInfo requestResSheetInfo = ResSheetLoader.RequestResSheet();
            yield return requestResSheetInfo.CurCoroutine;
            if (requestResSheetInfo.IsError)
            {
                ResLoadHelper.Log("请求表格资源错误");
                info.IsError = true;
                yield break;
            }

            info.IsDone = true;
            info.Progress = 1.0f;
        }
        internal static IEnumerator LoadConfigAsync(ResAsyncInfo info)
        {
            //ResUpdateControler.OnUpdateProgress(0, "加载版本资源...");
            ResAsyncInfo loadResVersionInfo = ResVersionLoader.LoadResVersion();
            yield return loadResVersionInfo.CurCoroutine;
            if (loadResVersionInfo.IsError)
            {
                ResLoadHelper.Log("加载版本资源错误");
                info.IsError = true;
                yield break;
            }
            //ResUpdateControler.OnUpdateProgress(0, "加载资源缓存列表...");
            ResAsyncInfo loadResCacheInfo = ResCacheLoader.LoadResCache();
            yield return loadResCacheInfo.CurCoroutine;
            if (loadResCacheInfo.IsError)
            {
                ResLoadHelper.Log("加载缓存资源列表错误");
                info.IsError = true;
                yield break;
            }

            ResUpdateControler.OnUpdateProgress(0.3f);
            //ResUpdateControler.OnUpdateProgress(0, "加载表格资源...");
            ResAsyncInfo loadResSheetInfo = ResSheetLoader.LoadResSheet();
            yield return loadResSheetInfo.CurCoroutine;
            if (loadResSheetInfo.IsError)
            {
                ResLoadHelper.Log("加载表格资源列表错误");
                info.IsError = true;
                yield break;
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
    }
}