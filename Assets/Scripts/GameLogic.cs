#if UNITY_ANDROID || UNITY_WEBGL
    //#define LoadDataTableFromCache
#endif

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using ArkCrossEngine;
using System.Collections.Generic;

/// <summary>
/// Game root entry
/// </summary>
public class GameLogic : UnityEngine.MonoBehaviour
{
    internal void Awake()
    {
        GlobalVariables.Instance.IsClient = true;
        DontDestroyOnLoad(this.gameObject);

        // initialize lua environment
        LuaManager.Instance.InitEnv();
    }
    // Use this for initialization
    internal void Start()
    {
#if (UNITY_IOS || UNITY_ANDROID) && !(UNITY_EDITOR)
        UnityEngine.Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 2;
        UnityEngine.Application.runInBackground = true;
#endif

#if UNITY_WEBGL
        UnityEngine.Application.runInBackground = true;
#endif
        try
        {
            if (!GameControler.IsInited)
            {
                // register to game thread
                ArkProfiler.RegisterOutput(LogicSystem.LogicLog);
                // register to gfx thread, output to game console
                ArkProfiler.RegisterOutput2(UnityEngine.Debug.Log);

                // hardware ability
                HardWareQuality.Clear();
                HardWareQuality.ComputeHardwarePerformance();
                HardWareQuality.SetResolution();
                HardWareQuality.SetQualityAll();
                
                // push notify message to sdk, notify user when time matches
                NotificationLogic.Instance.Init();

                // if in shipping mode, initialize resource update module
                if (GlobalVariables.Instance.IsPublish)
                {
                    ResUpdateControler.InitContext();
                }

                // register file read handler
                FileReaderProxy.RegisterReadFileHandler(EngineReadFileProxy, EngineFileExistsProxy);

                /// Unity Editor: <path_to_project_folder>/Assets
                /// iOS player: <path_to_player_app_bundle>/<AppName.app>/Data (this folder is read only, use Application.persistentDataPath to save data).
                /// Android: Normally it would point directly to the APK. The exception is if you are running a split binary build in which case it points to the the OBB instead.
                string dataPath = UnityEngine.Application.dataPath;
                /// Point to data path which have write permission
                string persistentDataPath = Application.persistentDataPath;
                /// Point to readonly data, note some platofrm like android points to compressed apk, witch cant be directory accesssed, use www. etc instead
                string streamingAssetsPath = UnityEngine.Application.streamingAssetsPath;
                /// Point to temp data path, may clean by system
                string tempPath = UnityEngine.Application.temporaryCachePath;
                LogicSystem.LogicLog("dataPath:{0} persistentDataPath:{1} streamingAssetsPath:{2} tempPath:{3}", dataPath, persistentDataPath, streamingAssetsPath, tempPath);
                Debug.Log(string.Format("dataPath:{0} persistentDataPath:{1} streamingAssetsPath:{2} tempPath:{3}", dataPath, persistentDataPath, streamingAssetsPath, tempPath));

                // store log in tempPath, others to persistentDataPath
#if (UNITY_ANDROID || UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
                GlobalVariables.Instance.IsDevice = true;
#else
                // if in editor, use streamingAssetsPath instead
                GlobalVariables.Instance.IsDevice = false;
#endif

#if UNITY_ANDROID || UNITY_WEBGL
                if (!UnityEngine.Application.isEditor)
                {
                    streamingAssetsPath = persistentDataPath + "/Tables";
                }
#endif

#if UNITY_WEBGL
                // init web socket before gamelogic initialize
                m_WebSocket = new WebGLSocket();
                ArkCrossEngine.Network.WebSocketWrapper.Instance.SetInstance(m_WebSocket);
#endif

                GameControler.Init(tempPath, streamingAssetsPath);

                // override log output
                LogSystem.OnOutput2 = (Log_Type type, string msg) =>
                {
#if DEBUG
                    if (Log_Type.LT_Error == type)
                    {
                        UnityEngine.Debug.LogError(msg);
                    }
                    else if (Log_Type.LT_Info != type)
                    {
                        UnityEngine.Debug.LogWarning(msg);
                    }
#endif
                };
                NormLog.Instance.Init();
                NormLog.Instance.Record(GameEventCode.GameStart);
            }

            // try preload all skills used by npc in spec scene, also character
            LogicSystem.OnAfterLoadScene += AfterLoadScene;
        }
        catch (Exception ex)
        {
            LogicSystem.LogicLog("GameLogic.Start throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
            Debug.Log(string.Format("GameLogic.Start throw exception:{0}\n{1}", ex.Message, ex.StackTrace));
        }
    }
    
    internal void Update()
    {
        try
        {
            //LogicSystem.BeginLoading();
            
            // if we are fisrt time start game, extract and loading game first
            if (!m_IsDataFileExtracted && !m_IsDataFileExtractedPaused)
            {
                StartCoroutine(HandleGameLoading());
                m_IsDataFileExtracted = true;
            }
            
            // if fully initialized, tick game
            if (m_IsInit)
            {
                // only for debug
                {
                    bool isLastHitUi = (UICamera.lastHit.collider != null);
                    LogicSystem.IsLastHitUi = isLastHitUi;
                    //DebugConsole.IsLastHitUi = isLastHitUi;
                }

#if UNITY_WEBGL
                m_WebSocket.Tick();
#endif

                GameControler.TickGame();

                LuaManager.Instance.Tick();
            }

            // Todo: try move to ui root
            ClickNpcManager.Instance.Tick();
        }
        catch (Exception ex)
        {
            LogicSystem.LogicErrorLog("GameLogic.Update throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
            Debug.LogError(string.Format("GameLogic.Update throw exception:{0}\n{1}", ex.Message, ex.StackTrace));
        }

        // simple frame counter
        try
        {
            m_TimeLeft -= UnityEngine.Time.deltaTime;
            m_Accum += UnityEngine.Time.timeScale / UnityEngine.Time.deltaTime;
            ++m_Frames;

            if (m_TimeLeft <= 0)
            {
                m_TimeLeft = c_UpdateInterval;
                m_Accum = 0;
                m_Frames = 0;
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    internal void OnApplicationPause(bool isPause)
    {
        try
        {
            Debug.LogWarning("OnApplicationPause:" + isPause);
            GameControler.PauseLogic(isPause);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    internal void OnApplicationQuit()
    {
        try
        {
            Debug.LogWarning("OnApplicationQuit");
            GameControler.StopLogic();
            GameControler.Release();
            AssetExManager.Instance.ClearAllAssetEx();
            UnityEngine.Resources.UnloadUnusedAssets();
            LuaManager.Instance.DisposeEnv();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    internal string GetFPS()
    {
        return string.Format("CityFPS:{0:f1}", m_Accum / m_Frames);
    }

    private void AfterLoadScene(string name, int sceneid)
    {
        try
        {
            ArkCrossEngine.RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
            if (ri != null)
            {
                GfxModule.Skill.GfxSkillSystem.Instance.PreLoadRoleSkills(ri.SkillInfos);
                GfxModule.Skill.GfxSkillSystem.Instance.PreLoadSceneNpcSkills(DFMUiRoot.NowSceneID);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
     
    /// Story Handlers
    public void TriggerStory(int storyId)
    {
        try
        {
            StoryDlg.StoryDlgInfo storyInfo = StoryDlg.StoryDlgManager.Instance.GetStoryInfoByID(storyId);
            if (null != storyInfo)
            {
                if (storyInfo.DlgType == StoryDlgPanel.StoryDlgType.Small)
                {
                    UIManager.Instance.ShowWindowByName("StoryDlgSmall");
                    UnityEngine.GameObject obj = UIManager.Instance.GetWindowGoByName("StoryDlgSmall");
                    if (null != obj)
                    {
                        StoryDlgPanel dlg = obj.GetComponent<StoryDlgPanel>();
                        dlg.OnTriggerStory(storyInfo);
                    }
                }
                else
                {
                    UnityEngine.GameObject obj = UIManager.Instance.GetWindowGoByName("StoryDlgBig");
                    if (null != obj)
                    {
                        StoryDlgPanel dlg = obj.GetComponent<StoryDlgPanel>();
                        dlg.OnTriggerStory(storyInfo);
                    }
                }
            }
            else
            {
                Debug.LogError("Wrong Story id = " + storyId);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void StopCurrentStory()
    {
        try
        {
            UnityEngine.GameObject obj = UIManager.Instance.GetWindowGoByName("StoryDlgSmall");
            if (null != obj)
            {
                StoryDlgPanel dlg = obj.GetComponent<StoryDlgPanel>();
                if (dlg != null)
                {
                    dlg.KillStoryDlg();
                }
            }
            UnityEngine.GameObject objB = UIManager.Instance.GetWindowGoByName("StoryDlgBig");
            if (null != objB)
            {
                StoryDlgPanel dlg = objB.GetComponent<StoryDlgPanel>();
                if (dlg != null)
                {
                    dlg.KillStoryDlg();
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private IEnumerator HandleGameLoading()
    {
        // fire ge_loading_start event handled by ui module, display splash screen & change to loading state
        LogicSystem.BeginLoading();
        yield return new WaitForSeconds(.1f);

        // if in shipping version, update resource from server
        if (GlobalVariables.Instance.IsPublish)
        {
            yield return StartCoroutine(HandleGameLoadingPublish());
        }

#if UNITY_ANDROID || UNITY_WEBGL
        // if not play game in editor, extract config data to disk
        else if (!UnityEngine.Application.isEditor)
        {
            string destPath = UnityEngine.Application.persistentDataPath + "/Tables";
            if (!Directory.Exists(destPath))
                yield return StartCoroutine(HandleGameLoadingNonEditor());
        }
#endif

        LogicSystem.UpdateLoadingProgress(0.45f);

        // async load notice string from server
        ResAsyncInfo requestNoticeConfigInfo = NoticeConfigLoader.RequestNoticeConfig();
        yield return requestNoticeConfigInfo.CurCoroutine;

        if (requestNoticeConfigInfo.IsError)
        {
            UnityEngine.Debug.Log("获取公告列表错误");
        }

        // init all game system and start game
        StartLogic();

        // fire ge_loading_finish event handled by ui module, notify ui finished
        LogicSystem.EndLoading();
    }

    private IEnumerator HandleGameLoadingPublish()
    {
        AssetExManager.Instance.Cleanup();
        
        ResUpdateControler.InitUpdate();
        ResUpdateControler.HandleUpdateFailed = ReExtractDataFileAndStartGame;

#region DetectVersion
        ResUpdateControler.OnUpdateProgress(0);
        ResAsyncInfo detectVersionInfo = ResUpdateControler.DetectVersion();
        yield return detectVersionInfo.CurCoroutine;
        if (detectVersionInfo.IsError)
        {
            ReExtractDataFileAndStartGame();
            yield break;
        }

        if (ResUpdateControler.IsNeedPauseUpdate)
        {
            PauseExtractDataFileAndStartGame();
            yield break;
        }
#endregion

#region StartUpdate
        int targetChapter = UnityEngine.Mathf.Max(ResUpdateControler.CurChapter, 1);
        ResAsyncInfo startUpdateInfo = ResUpdateControler.StartUpdate(targetChapter);
        yield return startUpdateInfo.CurCoroutine;
        if (ResUpdateControler.IsNeedPauseUpdate)
        {
            PauseExtractDataFileAndStartGame();
            yield break;
        }
        if (startUpdateInfo.IsError)
        {
            ReExtractDataFileAndStartGame();
            yield break;
        }
#endregion

#region LoadLevelAsync
        ResAsyncInfo loadCacheResForMainMenuInfo = ResLevelLoader.LoadLevelAsync(ResUpdateControler.s_LoadSceneId);
        yield return loadCacheResForMainMenuInfo.CurCoroutine;
        if (loadCacheResForMainMenuInfo.IsError)
        {
            ReExtractDataFileAndStartGame();
            yield break;
        }
#endregion

        ResUpdateControler.ExitUpdate();
    }

    private IEnumerator HandleGameLoadingNonEditor()
    {
        LogicSystem.UpdateLoadingTip("加载配置数据");
        string srcPath = UnityEngine.Application.streamingAssetsPath;
        string destPath = UnityEngine.Application.persistentDataPath + "/Tables";
        Debug.Log(srcPath);
        Debug.Log(destPath);

        if (!srcPath.Contains("://"))
            srcPath = "file://" + srcPath;
        string listPath = srcPath + "/list.txt";
        WWW listData = new WWW(listPath);
        yield return listData;

        string listTxt = listData.text;
        if (null != listTxt)
        {
            using (StringReader sr = new StringReader(listTxt))
            {
                string numStr = sr.ReadLine();
                float totalNum = 50;
                if (null != numStr)
                {
                    numStr = numStr.Trim();
                    totalNum = (float)int.Parse(numStr);
                    if (totalNum <= 0)
                        totalNum = 50;
                }
                for (float num = 1; ; num += 1)
                {
                    string path = sr.ReadLine();
                    if (null != path)
                    {
                        path = path.Trim();
                        string url = srcPath + "/" + path;
                        //Debug.Log("extract " + url);
                        string filePath = Path.Combine(destPath, path);
                        string dir = Path.GetDirectoryName(filePath);
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                        WWW temp = new WWW(url);
                        yield return temp;
                        if (null != temp.bytes)
                        {
                            try
                            {
#if LoadDataTableFromCache
                                byte[] newAlloced = new byte[temp.bytes.Length];
                                temp.bytes.CopyTo(newAlloced, 0);
                                CachedTables.Add(Path.GetFullPath(filePath).ToLower(), newAlloced);
#else
                                File.WriteAllBytes(filePath, temp.bytes);
#endif
                            }
                            catch (System.Exception ex)
                            {
                                LogicSystem.LogicErrorLog("ExtractDataFileAndStartGame copy config failed. ex:{0} st:{1}",
                                  ex.Message, ex.StackTrace);
                            }
                        }
                        else
                        {
                            //Debug.Log(path + " can't load");
                        }

                        temp.Dispose();
                        temp = null;
                    }
                    else
                    {
                        break;
                    }

                    LogicSystem.UpdateLoadingProgress(0.8f + 0.2f * num / totalNum);
                }
                sr.Close();
            }
            listData = null;
        }
        else
        {
            Debug.Log("Can't load list.txt");
        }
    }

    private void ReExtractDataFileAndStartGame()
    {
        try
        {
            ResUpdateControler.IncReconnectNum();
            m_IsDataFileExtractedPaused = true;
            string info = string.Empty;
            string dlgButton = string.Empty;

            if (ResUpdateControler.s_UpdateError == ResUpdateError.Network_Error)
            {
                info = Dict.Get(26);
                if (string.IsNullOrEmpty(info))
                {
                    info = "网络连接失败！";
                }
                dlgButton = Dict.Get(28);
                if (string.IsNullOrEmpty(dlgButton))
                {
                    dlgButton = "重试";
                }
            }
            else
            {
                string infoFormat = Dict.Get(27);
                if (string.IsNullOrEmpty(infoFormat))
                {
                    infoFormat = "更新失败，请稍后重试！\n失败原因（{0}）";
                }
                info = string.Format(infoFormat, (int)ResUpdateControler.s_UpdateError);
                dlgButton = Dict.Get(28);
                if (string.IsNullOrEmpty(dlgButton))
                {
                    dlgButton = "重试";
                }
            }

            Action<bool> fun = new Action<bool>(delegate (bool selected)
            {
                if (selected)
                {
                    m_IsDataFileExtractedPaused = false;
                    ResUpdateControler.ExitUpdate();
                    m_IsDataFileExtracted = false;
                    m_IsInit = false;
                }
            });
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_yesornot", "ui", info, dlgButton, fun);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void PauseExtractDataFileAndStartGame()
    {
        try
        {
            m_IsDataFileExtractedPaused = true;
            ResUpdateControler.ExitUpdate();
            m_IsDataFileExtracted = false;
            m_IsInit = false;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void StartLogic()
    {
        try
        {
            ArkProfiler.Start("StartLogic");

            // initialize all sub system of game, load data from world system
            GameControler.InitLogic();

            // start game logic thread
            GameControler.StartLogic();

            // load ui data table from disk
            UIManager.Instance.Init();

            // store name of loading bar scene to game logic thread
            LogicSystem.SetLoadingBarScene("LoadingBar");

#if LoadDataTableFromCache
            //CleanupCachedTables();
#endif

            // manual change to loading scene if not in shipping mode
            if (!GlobalVariables.Instance.IsPublish)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
            }

            ArkProfiler.Stop("StartLogic");

            m_IsInit = true;

            // unload all windows, then show login window
            LogicSystem.EventChannelForGfx.Publish("ge_show_login", "ui");
        }
        catch (System.Exception ex)
        {
            // at this time, logic queue may not initialize yet
            UnityEngine.Debug.LogErrorFormat("[Error]:Exception:{0}\n{1}",
              ex.Message, ex.StackTrace);
            LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}",
              ex.Message, ex.StackTrace);
        }
    }
    internal void RestartLogic()
    {
        try
        {
            LogicSystem.SetLoadingBarScene("LoadingBar");

            // change scene to login
            LogicSystem.PublishLogicEvent("ge_change_scene", "game", 0);

            m_IsInit = true;
        }
        catch (Exception ex)
        {
            LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    /// Messages Receivers
    /// ! DO NOT ! use these events in tick or update

    private void OnResetDsl(string script)
    {
        SkillSystem.SkillConfigManager.Instance.Clear();
        GfxModule.Skill.GfxSkillSystem.Instance.ClearSkillInstancePool();
        LogicSystem.PublishLogicEvent("ge_resetdsl", "game");
    }
    private void OnExecScript(string script)
    {
        LogicSystem.PublishLogicEvent("ge_execscript", "game", script);
    }
    private void OnExecCommand(string command)
    {
        LogicSystem.PublishLogicEvent("ge_execcommand", "game", command);
    }
    public void StartCountDown(int countDownTime)
    {
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_pvp_counttime", "ui", countDownTime);
    }

    private byte[] EngineReadFileProxy(string filePath)
    {
        try
        {
            // Todo: load from bundle
#if LoadDataTableFromCache
            filePath = Path.GetFullPath(filePath).ToLower();
            byte[] bytes;
            if (CachedTables.TryGetValue(filePath, out bytes))
            {
                return bytes;
            }
            else
            {
                return null;
            }
#else
            byte[] buffer = null;
            buffer = File.ReadAllBytes(filePath);
            return buffer;
#endif
        }
        catch (Exception e)
        {
            GfxSystem.GfxLog("Exception:{0}\n{1}", e.Message, e.StackTrace);
            return null;
        }
    }

    private bool EngineFileExistsProxy(string filePath)
    {
        // TODO: handle bundle
#if LoadDataTableFromCache
        filePath = Path.GetFullPath(filePath).ToLower();
        byte[] bytes;
        return CachedTables.TryGetValue(filePath, out bytes);
#else
        return File.Exists(filePath);
#endif
    }

#if LoadDataTableFromCache
    private void CleanupCachedTables()
    {
        CachedTables.Clear();
        GC.Collect();
    }
#endif

    private bool m_IsDataFileExtracted = false;
    private bool m_IsDataFileExtractedPaused = false;
    private bool m_IsSettingModified = false;
    private bool m_IsInit = false;
    
    private float m_Accum = 0;
    private float m_Frames = 0;
    private float m_TimeLeft = 0;
    private const float c_UpdateInterval = 1.0f;

#if UNITY_WEBGL
    private WebGLSocket m_WebSocket;
#endif

#if LoadDataTableFromCache
    private Dictionary<string, byte[]> CachedTables = new Dictionary<string, byte[]>();
#endif
}
