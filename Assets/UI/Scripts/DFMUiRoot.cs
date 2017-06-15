using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using StoryDlg;
using ArkCrossEngine;

public class DFMUiRoot : UnityEngine.MonoBehaviour
{
    public static string INPUT_MODE = "INPUT_MODE";
    public static string INPUT_MODE_TOUCH = "INPUT_MODE_TOUCH";
    public static string INPUT_MODE_JOYSTICK = "INPUT_MODE_JOYSTICK";

    public SceneTypeEnum m_SceneType = SceneTypeEnum.TYPE_UNKNOWN;
    public SceneSubTypeEnum m_SubSceneType = SceneSubTypeEnum.TYPE_UNKNOWN;
    private UnityEngine.GameObject loading = null;
    private float timeRecycle = 10.0f;
    private UnityEngine.GameObject m_StoryDlgSmallGO = null;
    private UnityEngine.GameObject m_StoryDlgBigGO = null;
    private UnityEngine.GameObject DynamicWidgetPanel = null;
    private UnityEngine.GameObject ScreenTipPanel = null;
    private UnityEngine.GameObject PalmHitGo = null;
    private int m_EnemyNum = 0;

    static public List<GfxUserInfo> GfxUserInfoListForUI = new List<GfxUserInfo>();
    static private Dictionary<int, UnityEngine.GameObject> NickNameGameObjectDic = new Dictionary<int, UnityEngine.GameObject>();
    static public Dictionary<UnityEngine.GameObject, UnityEngine.GameObject> NpcGameObjectS = new Dictionary<UnityEngine.GameObject, UnityEngine.GameObject>();
    static public int NowSceneID = 0;
    //pve战斗信息单独处理,type = 0,1,2,3(被击，防御，挑战，突袭)
    //static public UnityEngine.GameObject PveFightInfo = null;
    //该数据同步不一定在主城完成，所以需要放在DFMUIRoot
    static public float m_RestaminaStartTime = float.MinValue;
    // Use this for initialization
    void Start()
    {
        try
        {
            DontDestroyOnLoad(this.gameObject.transform.parent);
            UIDataCache.Instance.Init(this.gameObject);
            UIManager.Instance.Init(this.gameObject);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<float, float, float, int>("ge_hero_blood", "ui", ShowAddHPForHero);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<float, float, float, int>("ge_hero_energy", "ui", ShowAddMPForHero);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<float, float, float, int, bool>("ge_npc_odamage", "ui", ShowbloodFlyTemplate);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<float, float, float, int, bool>("ge_npc_cdamage", "ui", ShowCriticalDamage);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<float, float, float, int>("ge_gain_money", "ui", ShowGainMoney);
            //hit
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("ge_hitcount", "ui", this.OnChainBeat);
            //蓄力
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<float, float, float, float, int>("ge_monster_power", "ui", ShowMonsterPrePower);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("ge_enter_scene", "ui", EnterInScene);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_loading_start", "ui", StartLoading);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_loading_finish", "ui", EndLoading);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_loading_tip_random", "ui", ChangeRandomNotice);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_show_login", "ui", ShowLogin);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("ge_pvp_counttime", "ui", PvpCountTime);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("ge_just_counttime", "ui", JustCountTime);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("ge_hide_name_plate", "ui", HideHeroNickName);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<GfxUserInfo>("ge_show_name_plate", "ui", CreateHeroNickName);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<List<GfxUserInfo>>("ge_show_name_plates", "ui", CreateHeroNickName);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<GfxUserInfo>("ge_show_npc_name_plate", "ui", CreateNpcNickName);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<string, string, Action<bool>>("ge_show_yesornot", "ui", ShowYesOrNot);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<List<int>>("ge_show_newbieguide", "ui", ShowNewbieGuide);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<string>("ge_highlight_prompt", "ui", ShowHighlightPrompt);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<string, UIScreenTipPosEnum, UnityEngine.Vector3>("ge_screen_tip", "ui", ShowScreenTip);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<string, UIScreenTipPosEnum, UnityEngine.Vector3>("ge_screen_tip_invoke", "ui", ShowScreenTipEnvok);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, int>("ge_sell_item_income", "bag", SellAward);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, int, int, int>("ge_pve_fightinfo", "ui", SetPveFightInfo);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, int>("ge_start_monster_sheild", "ui", CreateMonestSheildBlood);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_hide_input_ui", "ui", HideInputUi);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("player_self_created", "ui", OnPlayerSelfCreated);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<bool>("set_gesture_enable", "ui", SetGestureEnable);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_restamina_time", "ui", HandlerRestaminaTime);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_trigger_newbie_guide", "ui", this.TriggerNewbieGuide);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<ArkCrossEngine.Network.GeneralOperationResult>("ge_request_expedition_failure", "expedition", this.ExpeditionFailure);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_show_marsloading", "ui", this.ShowMarsLoading);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("ge_show_pathfinding", "ui", this.ShowPathFinding);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<string, string, string, string, bool>("ge_show_exception_dialog", "ui", this.ShowExceptionDialog);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_serverselect_network_failed", "ui", HandleServerselectNetworkFailed);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_enter_city", "ui", EnterCityWrap);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_server_shutdown", "lobby", HandlerShutDown);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<string>("ge_request_dare", "dare", HandlerRequestDare);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<string, ArkCrossEngine.Network.GeneralOperationResult>("ge_request_dare_result", "dare", HandlerRequestDare);
            //法师EX技能UI
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<bool>("ge_show_palm", "ui", ShowPalmHit);
            DynamicWidgetPanel = transform.Find("DynamicWidget").gameObject;
            ScreenTipPanel = transform.Find("ScreenTipPanel").gameObject;
            InitInputMode();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void ShowPalmHit(bool vis)
    {
        if (vis)
        {
            if (PalmHitGo == null)
            {
                string path = UIManager.Instance.GetPathByName("PalmHit");
                PalmHitGo = ResourceSystem.GetSharedResource(/*path*/"UI/PalmHit/PalmHit") as GameObject;
                if (PalmHitGo != null)
                {
                    PalmHitGo = NGUITools.AddChild(gameObject, PalmHitGo);
                    if (PalmHitGo != null)
                    {
                        NGUITools.SetActive(PalmHitGo, true);
                    }
                }
            }
            else
            {
                NGUITools.SetActive(PalmHitGo, true);
            }
        }
        else
        {
            if (PalmHitGo != null)
            {
                NGUITools.SetActive(PalmHitGo, false);
            }
        }
    }
    private void HandlerShutDown()
    {
        SendDialog(41, 4, HandleDialog, null);
    }
    private void HandlerRequestDare(string offense)
    {
        string CHN_CONFIRM = StrDictionaryProvider.Instance.GetDictString(4);
        string CHN_CANCEL = StrDictionaryProvider.Instance.GetDictString(9);
        string str = StrDictionaryProvider.Instance.GetDictString(1201);
        string CHN_DESC = string.Format(str, offense);
        LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", CHN_DESC, null, CHN_CONFIRM, CHN_CANCEL, (MyAction<int>)((int btn) =>
        {
            if (btn == 1)
            {
                ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_accepted_dare", "lobby", offense);
            }
        }), false);
    }
    private void HandlerRequestDare(string nickname, ArkCrossEngine.Network.GeneralOperationResult ret)
    {
        string CHN_CONFIRM = StrDictionaryProvider.Instance.GetDictString(4);
        int des_index = 0;
        if (ArkCrossEngine.Network.GeneralOperationResult.LC_Failuer_Busy == ret)
        {
            des_index = 1202;
        }
        else if (ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_NotUnLock == ret)
        {
            des_index = 1203;
        }
        else if (ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_InCd == ret)
        {
            des_index = 1204;
        }
        string str = StrDictionaryProvider.Instance.GetDictString(des_index);
        string CHN_DES = str.Length > 0 ? string.Format(str, nickname) : "";
        LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", CHN_DES, CHN_CONFIRM, null, null, null, false);
    }
    /*发送对话框*/
    void SendDialog(int i_chn_desc, int i_confirm, ArkCrossEngine.MyAction<int> Func, params object[] insert_name)
    {
        string chn_desc = "";
        string confirm = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(i_confirm);
        chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(i_chn_desc);
        string str = chn_desc;
        if (insert_name != null)
        {
            str = string.Format(chn_desc, insert_name);
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", str, confirm, null, null, Func, false);
    }
    /*确认加入队伍回调*/
    void HandleDialog(int action)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (action == 0)
        {
            DFMUtils.Instance.RestartGame();
        }
    }
    private void HandlerRestaminaTime()
    {
        try
        {
            DFMUiRoot.m_RestaminaStartTime = UnityEngine.Time.time;
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void EnterCityWrap()
    {
        StartCoroutine(DoEnterCity());
    }
    IEnumerator DoEnterCity()
    {
        yield return new WaitForSeconds(4f);
        string fps = "";
        string hardwarename = "";
        UnityEngine.GameObject go = UnityEngine.GameObject.Find(ArkCrossEngine.GlobalVariables.cGameRootName);
        if (go != null)
        {
            GameLogic gameLogic = go.GetComponent<GameLogic>();
            if (gameLogic != null)
            {
                fps = gameLogic.GetFPS();
            }
        }
#if UNITY_ANDROID
        if(null != NormLog.Instance.Info)
            hardwarename = NormLog.Instance.Info.hardwarename;
#elif UNITY_IPHONE
        hardwarename = "IPhone";
#else
        hardwarename = "PC";
#endif
        if (fps.Length > 0 && hardwarename.Length > 0)
            ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_upload_fps", "lobby", fps, hardwarename);
    }
    private void InitInputMode()
    {
        if (PlayerPrefs.HasKey(INPUT_MODE))
        {
            if (PlayerPrefs.GetString(INPUT_MODE).Equals(INPUT_MODE_TOUCH))
            {
                InputMode = InputType.Touch;
            }
            else
            {
                InputMode = InputType.Joystick;
            }
        }
        else
        {
            InputMode = InputType.Joystick;
        }
    }

    // Update is called once per frame
    private float time;
    void Update()
    {
        try
        {
            time += RealTime.deltaTime;
            if (time > 1f)
            {
                showTips();
                time = 0f;
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void ShowPathFinding(int show)
    {
        bool b_show = show == 1 ? true : false;
        UnityEngine.Transform tfPF = this.transform.Find("ScreenTipPanel/Wayfinding(Clone)");
        if (tfPF == null)
        {
            UnityEngine.GameObject go = ResourceSystem.NewObject("UI/Map/Wayfinding") as GameObject;
            if (null != go)
            {
                UnityEngine.Transform tf = go.transform;
                if (ScreenTipPanel != null && tf.parent != ScreenTipPanel.transform)
                {
                    tf.parent = ScreenTipPanel.transform;
                    tf.localScale = new UnityEngine.Vector3(1, 1, 1);
                }
                NGUITools.SetActive(go, b_show);
            }
        }
        else
        {
            NGUITools.SetActive(tfPF.gameObject, b_show);
        }
    }
    void ShowExceptionDialog(string message, string button0, string button1, string button2, bool islogic)
    {
        UIManager.Instance.LoadWindowByName("Dialog", UICamera.mainCamera);
        ArkCrossEngine.MyAction<int> cb = ReturnMainCity;
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", message, button0, button1, button2, cb, islogic);
    }
    void ReturnMainCity(int key)
    {
        ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_return_maincity", "lobby");
    }
    void SellAward(int money, int gold)
    {
        try
        {
            string path = UIManager.Instance.GetPathByName("SellAward");
            UnityEngine.Object obj = ArkCrossEngine.ResourceSystem.NewObject(path, timeRecycle);
            UnityEngine.GameObject go = obj as UnityEngine.GameObject;
            if (null != go)
            {
                UnityEngine.Transform tf = go.transform.Find("Label/Money/MLabel");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (null != ul)
                    {
                        ul.text = money.ToString();
                    }
                }
                tf = go.transform.Find("Label/Diamond/DLabel");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (null != ul)
                    {
                        ul.text = gold.ToString();
                    }
                }
                if (ScreenTipPanel != null)
                {
                    go = NGUITools.AddChild(ScreenTipPanel, obj);
                }
                if (go != null)
                {
                    tf = go.transform.Find("Label/Money");
                    if (tf != null)
                    {
                        if (money == 0)
                        {
                            NGUITools.SetActive(tf.gameObject, false);
                        }
                        else
                        {
                            NGUITools.SetActive(tf.gameObject, true);
                        }
                    }
                    tf = go.transform.Find("Label/Diamond");
                    if (tf != null)
                    {
                        if (gold == 0)
                        {
                            NGUITools.SetActive(tf.gameObject, false);
                        }
                        else
                        {
                            NGUITools.SetActive(tf.gameObject, true);
                        }
                    }
                    BloodAnimation ba = go.GetComponent<BloodAnimation>();
                    if (ba != null)
                    {
                        ba.PlayAnimation();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void ShowHighlightPrompt(string tip)
    {
        ShowScreenTip(tip, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
    }
    //显示屏幕提示--悬浮字
    void ShowScreenTip(string tip, UIScreenTipPosEnum posType, UnityEngine.Vector3 vec3)
    {
        try
        {
            UnityEngine.GameObject go = GameObjectManager.Instance.NewObject("ScreenTip", timeRecycle);
            if (null != go)
            {
                UnityEngine.Transform tf = go.transform;
                if (ScreenTipPanel != null && tf.parent != ScreenTipPanel.transform)
                {
                    tf.parent = ScreenTipPanel.transform;
                }
                UIScreenTip script = go.GetComponent<UIScreenTip>();
                if (script != null)
                {
                    script.ShowScreenTip(tip, posType, vec3);
                }
            }
            /*
      string path = UIManager.Instance.GetPathByName("ScreenTip");
      UnityEngine.Object obj = ArkCrossEngine.ResourceSystem.NewObject(path, timeRecycle);
      UnityEngine.GameObject cube = null;
      if (ScreenTipPanel != null) {
        cube = NGUITools.AddChild(ScreenTipPanel, obj);
        if (cube != null) {
          UIScreenTip script = cube.GetComponent<UIScreenTip>();
          if (script != null) script.ShowScreenTip(tip, posType, vec3);
        }
      }*/
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //显示屏幕提示--悬浮字
    private List<string> invokeList = new List<string>();
    void ShowScreenTipEnvok(string tip, UIScreenTipPosEnum posType, UnityEngine.Vector3 vec3)
    {
        try
        {
            invokeList.Add(tip);
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void showTips()
    {
        if (invokeList.Count > 0)
        {
            ShowScreenTip(invokeList[0], UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3());
            invokeList.RemoveAt(0);
        }
    }

    private void ShowMarsLoading()
    {
        if (UIDataCache.Instance.isLoadingEnd)
        {
            UIManager.Instance.ShowWindowByName("Marsloading");
            UIDataCache.Instance.needShowMarsLoading = false;
        }
        else
        {
            UIDataCache.Instance.needShowMarsLoading = true;
        }
    }
    //PVP开始。。
    public void PvpCountTime(int counttime)
    {
        try
        {
            string path = UIManager.Instance.GetPathByName("PVPTime");
            UnityEngine.GameObject go = ArkCrossEngine.ResourceSystem.GetSharedResource(path) as GameObject;
            if (go != null)
            {
                UnityEngine.Transform tf = go.transform.Find("Label");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (null != ul)
                    {
                        ul.text = "00:00";
                    }
                }
                go = NGUITools.AddChild(gameObject, go);
                if (go != null)
                {
                    PVPTime pt = go.GetComponent<PVPTime>();
                    if (pt != null)
                    {
                        pt.SetCountDownTime(counttime);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void JustCountTime(int counttime)
    {
        try
        {
            string path = UIManager.Instance.GetPathByName("PVPTime");
            UnityEngine.GameObject go = ArkCrossEngine.ResourceSystem.GetSharedResource(path) as GameObject;
            if (go != null)
            {
                UnityEngine.Transform tf = go.transform.Find("Label");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (null != ul)
                    {
                        ul.text = "00:00";
                    }
                }
                go = NGUITools.AddChild(gameObject, go);
                if (go != null)
                {
                    PVPTime pt = go.GetComponent<PVPTime>();
                    if (pt != null)
                    {
                        pt.SetCountDownTime(counttime);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void HidePanelCreateUser()
    {
        UnityEngine.Transform ts = this.transform.Find("PanelCreateUser");
        if (ts != null)
        {
            if (null != ts.gameObject)
            {
                NGUITools.SetActive(ts.gameObject, false);
            }
        }
    }

    public void ShowLogin()
    {
        try
        {
            UIManager.Instance.UnLoadAllWindow();
            UIManager.Instance.LoadAllWindows(UISceneType.LoginScene, UICamera.mainCamera);
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void LoadUiInGame(UISceneType ui_type)
    {
        UIManager.Instance.LoadAllWindows(ui_type, UICamera.mainCamera);
    }

    //加载UI
    public void EnterInScene(int sceneId)
    {
        /*不同场景类型加载不同的UI
         * 登录、主城、单人PVP、PVP、远征
         */
        try
        {

            ArkProfiler.Start("LoadLevel.EnterInScene.LoadUi");

            /*EnterInScene在场景刚开始加载时被调用，逻辑层还未向UI同步GfxUserInfo*/
            DFMUiRoot.GfxUserInfoListForUI.Clear();
            DFMUiRoot.NickNameGameObjectDic.Clear();
            UIBeginnerGuideManager.Instance.IsBeginnerGuiderStarted = false;
            NowSceneID = sceneId;
            SceneSubTypeEnum prevSceneType = m_SubSceneType;//上一场景类型
            UIDataCache.Instance.prevSceneType = prevSceneType;
            m_EnemyNum = 0;
            Data_SceneConfig dsc = SceneConfigProvider.Instance.GetSceneConfigById(sceneId);
            if (dsc != null)
            {
                //登录
                if (dsc.m_Type == (int)SceneTypeEnum.TYPE_SERVER_SELECT)
                {
                    m_SceneType = SceneTypeEnum.TYPE_SERVER_SELECT;
                    LoadUiInGame(UISceneType.LoginScene);
                    UIBeginnerGuideManager.Instance.ResetLocalTriggerData();
                }
                //主城
                if (dsc.m_Type == (int)SceneTypeEnum.TYPE_PURE_CLIENT_SCENE)
                {
                    m_SceneType = SceneTypeEnum.TYPE_PURE_CLIENT_SCENE;
                    LoadUiInGame(UISceneType.MainCityScene);
                    InitStoryDlg();
                    GameObjectManager.Instance.ClearAllObjectList();
                    if (prevSceneType != SceneSubTypeEnum.TYPE_EXPEDITION)
                        StartCoroutine(DelayForNewbieGuide());
                    LoadResourcesInMainCity();
                }
                //PVP
                if (dsc.m_Type == (int)SceneTypeEnum.TYPE_PVP)
                {
                    m_SceneType = SceneTypeEnum.TYPE_PVP;
                    LoadUiInGame(UISceneType.PvpScene);
                    if (UIManager.Instance.OnAllUiLoadedDelegate != null)
                    {
                        UIManager.Instance.OnAllUiLoadedDelegate();
                    }
                    LoadSomeResources();
                }
                //PVAP
                if (dsc.m_SubType == (int)SceneSubTypeEnum.TYPE_PVAP)
                {
                    m_SceneType = SceneTypeEnum.TYPE_PVE;
                    m_SubSceneType = SceneSubTypeEnum.TYPE_PVAP;
                    LoadUiInGame(UISceneType.PvpScene);
                    if (UIManager.Instance.OnAllUiLoadedDelegate != null)
                    {
                        UIManager.Instance.OnAllUiLoadedDelegate();
                    }
                    LoadSomeResources();
                }

                //MPVE（多人PVE）
                if (dsc.m_SubType == (int)SceneSubTypeEnum.TYPE_ATTEMPT)
                {
                    m_SceneType = SceneTypeEnum.TYPE_MULTI_PVE;
                    m_SubSceneType = SceneSubTypeEnum.TYPE_ATTEMPT;
                    LoadUiInGame(UISceneType.MultiPveScene);
                    LoadSomeResources();
                }
                //刷金
                if (dsc.m_SubType == (int)SceneSubTypeEnum.TYPE_GOLD)
                {
                    m_SceneType = SceneTypeEnum.TYPE_MULTI_PVE;
                    m_SubSceneType = SceneSubTypeEnum.TYPE_GOLD;
                    LoadUiInGame(UISceneType.JinBiScene);
                    SetPveFightInfo(4, 0, 0, 0);
                }
                if (dsc.m_SubType == (int)SceneSubTypeEnum.TYPE_PLATFORM_DEFENSE)
                {
                    m_SceneType = SceneTypeEnum.TYPE_MULTI_PVE;
                    m_SubSceneType = SceneSubTypeEnum.TYPE_PLATFORM_DEFENSE;
                    LoadUiInGame(UISceneType.PlatformDefense);
                }
                //PVE(剧情和精英副本)
                if (dsc.m_Type == (int)SceneTypeEnum.TYPE_PVE && dsc.m_SubType != (int)SceneSubTypeEnum.TYPE_EXPEDITION
                    && dsc.m_SubType != (int)SceneSubTypeEnum.TYPE_PVAP)
                {
                    m_SceneType = SceneTypeEnum.TYPE_PVE;
                    LoadUiInGame(UISceneType.PveScene);
                    InitStoryDlg();
                    if (UIManager.Instance.OnAllUiLoadedDelegate != null)
                    {
                        UIManager.Instance.OnAllUiLoadedDelegate();
                    }
                    SetPveFightInfo(4, 0, 0, 0);
                    LoadSomeResources();
                }
                //远征
                if (dsc.m_SubType == (int)SceneSubTypeEnum.TYPE_EXPEDITION)
                {
                    m_SceneType = SceneTypeEnum.TYPE_PVE;
                    m_SubSceneType = SceneSubTypeEnum.TYPE_EXPEDITION;
                    LoadUiInGame(UISceneType.TreasureScene);
                    LoadSomeResources();
                }
                else if (dsc.m_SubType == (int)SceneSubTypeEnum.TYPE_PVAP ||
                         dsc.m_SubType == (int)SceneSubTypeEnum.TYPE_ATTEMPT ||
                         dsc.m_SubType == (int)SceneSubTypeEnum.TYPE_GOLD)
                {
                    //此else块 只是为了用于排除不重置为unknown的特列
                }
                else
                {
                    m_SubSceneType = SceneSubTypeEnum.TYPE_UNKNOWN;
                }
                //如果刚打完远征，则打开远征界面
                if (prevSceneType == SceneSubTypeEnum.TYPE_EXPEDITION && m_SceneType == SceneTypeEnum.TYPE_PURE_CLIENT_SCENE)
                {
                    UIManager.Instance.ShowWindowByName("cangbaotu");
                }
                //如果刚打完名人战，则打开名人战界面
                if (prevSceneType == SceneSubTypeEnum.TYPE_PVAP && m_SceneType == SceneTypeEnum.TYPE_PURE_CLIENT_SCENE)
                {
                    UIManager.Instance.ShowWindowByName("PartnerPvp");
                }
                //如果刚打完刷金，则打开活动界面
                if (prevSceneType == SceneSubTypeEnum.TYPE_GOLD && m_SceneType == SceneTypeEnum.TYPE_PURE_CLIENT_SCENE)
                {
                    UIManager.Instance.ShowWindowByName("TrialIntro");
                    UnityEngine.GameObject trialIntro = UIManager.Instance.GetWindowGoByName("TrialIntro");
                    if (trialIntro != null)
                    {
                        TrialIntro script = trialIntro.GetComponent<TrialIntro>();
                        if (script != null)
                        {
                            script.SetIntroType(TrialIntroType.JinBi);
                        }
                    }
                }
                //如果刚打完多人，则打开活动界面
                if (prevSceneType == SceneSubTypeEnum.TYPE_ATTEMPT && m_SceneType == SceneTypeEnum.TYPE_PURE_CLIENT_SCENE)
                {
                    UIManager.Instance.ShowWindowByName("TrialIntro");
                    UnityEngine.GameObject trialIntro = UIManager.Instance.GetWindowGoByName("TrialIntro");
                    if (trialIntro != null)
                    {
                        TrialIntro script = trialIntro.GetComponent<TrialIntro>();
                        if (script != null)
                        {
                            script.SetIntroType(TrialIntroType.ShiLian);
                        }
                    }

                }

                ///
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_init_backgroud_music", "music", sceneId);
            }
            ArkProfiler.Stop("LoadLevel.EnterInScene.LoadUi");

        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public bool IsCombatWithGhost(List<GfxUserInfo> gfxUsers)
    {
        if (gfxUsers == null) return false;
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info == null) return false;
        UserInfo user_info = role_info.GetPlayerSelfInfo();
        if (user_info == null) return false;
        for (int i = 0; i < gfxUsers.Count; ++i)
        {
            if (gfxUsers[i] != null)
            {
                SharedGameObjectInfo share_info = LogicSystem.GetSharedGameObjectInfo(gfxUsers[i].m_ActorId);
                if (share_info != null && user_info.GetCampId() != share_info.CampId)
                    return true;
            }
        }
        return false;
    }

    //pve创建
    public void CreatePvPHeroPanel(List<GfxUserInfo> gfxUsers)
    {
        m_EnemyNum = 0;
        if (gfxUsers != null)
        {
            if (m_SubSceneType == SceneSubTypeEnum.TYPE_EXPEDITION && IsCombatWithGhost(gfxUsers))
            {
                UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
                if (go != null)
                {
                    HeroPanel heroView = go.GetComponent<HeroPanel>();
                    if (heroView != null)
                    {
                        heroView.LayoutQuitBtn();
                        heroView.SetActive(false);
                    }
                    PveFightInfo pfi = go.GetComponent<PveFightInfo>();
                    if (pfi != null)
                    {
                        pfi.UnSubscribe();
                    }
                }
                RoleInfo role_info = LobbyClient.Instance.CurrentRole;
                if (role_info == null)
                {
                    Debug.Log("!!RoleInfo is null.");
                    return;
                }
                UserInfo user_info = role_info.GetPlayerSelfInfo();
                if (user_info == null)
                {
                    Debug.Log("!!UserInfo is null");
                    return;
                }
                int self_campId = user_info.GetCampId();
                for (int index = 0; index < gfxUsers.Count; ++index)
                {
                    if (gfxUsers[index] != null)
                    {
                        SharedGameObjectInfo shared_info = LogicSystem.GetSharedGameObjectInfo(gfxUsers[index].m_ActorId);
                        if (shared_info != null)
                        {
                            if (self_campId == shared_info.CampId)
                            {
                                //如果是自己阵营，因为只有单人Pvp所以就是自己
                                go = UIManager.Instance.LoadWindowByName("PVPmyHero", UICamera.mainCamera);
                                if (go == null)
                                {
                                    Debug.Log("!!!PVPmyHero is null.");
                                    continue;
                                }
                                HeroPanel scriptHp = go.GetComponent<HeroPanel>();
                                if (scriptHp != null) scriptHp.SetUserInfo(gfxUsers[index]);
                            }
                            else
                            {
                                //敌人
                                go = UIManager.Instance.LoadWindowByName("PVPmyEnemy", UICamera.mainCamera);
                                if (go == null)
                                {
                                    Debug.Log("!!!PVPmyEnemy is null."); continue;
                                }
                                go.transform.localPosition += new UnityEngine.Vector3(0, -90f * (m_EnemyNum), 0);
                                HeroPanel scriptHp = go.GetComponent<HeroPanel>();
                                if (scriptHp != null) scriptHp.SetUserInfo(gfxUsers[index]);
                                m_EnemyNum++;//敌人数加1
                            }
                        }
                    }
                }

            }
            else
              if (m_SceneType != SceneTypeEnum.TYPE_PURE_CLIENT_SCENE)
            {
                //
                if (gfxUsers.Count > 0 && gfxUsers[0] != null)
                {
                    UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
                    if (go == null)
                    {
                        Debug.Log("!!!HeroPanel is null.");
                        return;
                    }
                    HeroPanel scriptHp = go.GetComponent<HeroPanel>();
                    if (scriptHp != null) scriptHp.SetUserInfo(gfxUsers[0]);
                }
            }
        }
    }
    ///
    private void CreateHeroPanel(GfxUserInfo gfxUser)
    {
        if (gfxUser != null && (m_SceneType == SceneTypeEnum.TYPE_PVP || m_SubSceneType == SceneSubTypeEnum.TYPE_PVAP))
        {
            UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
            if (go != null)
            {
                HeroPanel heroView = go.GetComponent<HeroPanel>();
                if (heroView != null)
                {
                    heroView.SetActive(false);
                    heroView.SetExitActive(false);
                }
            }
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            if (role_info != null)
            {
                UserInfo user_info = role_info.GetPlayerSelfInfo();
                if (user_info == null)
                {
                    Debug.Log("!!!user_info is null.");
                    return;
                }
                SharedGameObjectInfo share_info = LogicSystem.GetSharedGameObjectInfo(gfxUser.m_ActorId);
                if (share_info == null)
                {
                    if (user_info == null)
                    {
                        Debug.Log("!!!share_info is null.");
                        return;
                    }
                }
                if (user_info.GetCampId() == share_info.CampId)
                {
                    //如果是自己阵营，因为只有单人Pvp所以就是自己
                    go = UIManager.Instance.LoadWindowByName("PVPmyHero", UICamera.mainCamera);
                    if (go == null)
                    {
                        Debug.Log("!!!PVPmyHero is null.");
                        return;
                    }
                    HeroPanel scriptHp = go.GetComponent<HeroPanel>();
                    if (scriptHp != null)
                        scriptHp.SetUserInfo(gfxUser);
                }
                else
                {
                    //敌人
                    go = UIManager.Instance.LoadWindowByName("PVPmyEnemy", UICamera.mainCamera);
                    if (go == null)
                    {
                        Debug.Log("!!!PVPmyEnemy is null.");
                        return;
                    }
                    go.transform.localPosition += new UnityEngine.Vector3(0, -90f * (m_EnemyNum), 0);
                    HeroPanel scriptHp = go.GetComponent<HeroPanel>();
                    if (scriptHp != null)
                        scriptHp.SetUserInfo(gfxUser);
                    m_EnemyNum++;//敌人数加1
                }
            }
        }
    }
    ///
    public void CreateMpveHeroPanel(GfxUserInfo gfxUser)
    {
        if (gfxUser != null && m_SceneType == SceneTypeEnum.TYPE_MULTI_PVE)
        {
            UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
            if (go != null)
            {
                HeroPanel scriptHp = go.GetComponent<HeroPanel>();
                if (scriptHp != null)
                    scriptHp.SetUserInfo(gfxUser);
            }
        }
    }
    //Pvp下创建
    public void CreatePvPHeroPanel(GfxUserInfo gfxUser)
    {
        if (gfxUser != null && (m_SceneType == SceneTypeEnum.TYPE_PVP || m_SubSceneType == SceneSubTypeEnum.TYPE_PVAP))
        {
            CreateHeroPanel(gfxUser);
        }
        if (gfxUser != null && m_SceneType == SceneTypeEnum.TYPE_MULTI_PVE)
        {
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            if (role_info != null && role_info.Nickname == gfxUser.m_Nick)
            {
                UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
                if (go == null)
                {
                    Debug.Log("!!!HeroPanel is null.");
                    return;
                }
                HeroPanel scriptHp = go.GetComponent<HeroPanel>();
                if (scriptHp != null) scriptHp.SetUserInfo(gfxUser);
            }
        }
    }
    private void HideHeroNickName(int actorid)
    {
        UnityEngine.GameObject go;
        if (NickNameGameObjectDic.TryGetValue(actorid, out go))
        {
            NickName nn = go.GetComponent<NickName>();
            if (nn != null)
            {
                nn.Reset();
                UnityEngine.GameObject _gameobject = go;//new UnityEngine.GameObject(go);
                ArkCrossEngine.ResourceSystem.RecycleObject(_gameobject);
            }
            GfxUserInfo gui = GfxUserInfoListForUI.Find(g => g.m_ActorId == actorid);
            if (gui != null)
            {
                GfxUserInfoListForUI.Remove(gui);
            }
            NickNameGameObjectDic.Remove(actorid);
        }
    }
    private bool HavenCreatedHeroPanel(GfxUserInfo gfxUser)
    {
        return (GfxUserInfoListForUI.Find(g => g.m_Nick == gfxUser.m_Nick) != null);
    }
    public void CreateHeroNickName(GfxUserInfo gfxUser)
    {
        try
        {
            if (!HavenCreatedHeroPanel(gfxUser))
            {
                CreatePvPHeroPanel(gfxUser);
            }
            else
            {
                //断线重连之后，已经创建的玩家血条需要更新GfxUserInfo
                LogicSystem.EventChannelForGfx.Publish("ge_update_gfxuserinfo", "ui", gfxUser);
            }
            RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
            if (ri != null)
            {
                UserInfo ui = ri.GetPlayerSelfInfo();
                if (ui != null)
                {
                    AboutHeroNickName(gfxUser, ui.GetCampId());
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void CreateHeroNickName(List<GfxUserInfo> gfxUsers)
    {
        try
        {
            GfxUserInfoListForUI.Clear();
            NickNameGameObjectDic.Clear();
            CreatePvPHeroPanel(gfxUsers);
            RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
            if (ri != null)
            {
                UserInfo ui = ri.GetPlayerSelfInfo();
                if (ui != null)
                {
                    for (int i = 0; i < gfxUsers.Count; i++)
                    {
                        if (gfxUsers[i] != null)
                        {
                            AboutHeroNickName(gfxUsers[i], ui.GetCampId());
                        }
                    }
                    /*
                    foreach (GfxUserInfo gui in gfxUsers) {
                      AboutHeroNickName(gui, ui.GetCampId());
                    }*/
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void AboutHeroNickName(GfxUserInfo gui, int cmpid)
    {
        if (gui != null)
        {
            if (GfxUserInfoListForUI.Find(g => g.m_ActorId == gui.m_ActorId) == null)
            {
                GfxUserInfoListForUI.Add(gui);
                UnityEngine.GameObject pargo = ArkCrossEngine.LogicSystem.GetGameObject(gui.m_ActorId);
                //         UnityEngine.Transform trans = GfxModule.Skill.Trigers.TriggerUtil.GetChildNodeByName(pargo, "ef_head");
                //         if (trans != null && pargo != null) {
                //           string path = UIManager.Instance.GetPathByName("NicknamePanel");
                //           UnityEngine.Object obj = ResourceSystem.NewObject(path);
                //           NGUITools.AddChild(trans.gameObject, obj);
                //                
                SharedGameObjectInfo sgoi = ArkCrossEngine.LogicSystem.GetSharedGameObjectInfo(gui.m_ActorId);
                if (pargo != null && sgoi != null)
                {
                    string path = UIManager.Instance.GetPathByName("NickName");
                    UnityEngine.Object obj = ResourceSystem.NewObject(path);
                    if (obj != null)
                    {
                        UnityEngine.GameObject go = obj as UnityEngine.GameObject;
                        if (go != null && DynamicWidgetPanel != null)
                        {
                            go = NGUITools.AddChild(DynamicWidgetPanel, obj);
                            if (go != null)
                            {
                                NickName nn = go.GetComponent<NickName>();
                                if (nn != null)
                                {
                                    nn.StartForShow();
                                    nn.SetPlayerGameObjectAndNickName(pargo, gui.m_Nick, m_SceneType == SceneTypeEnum.TYPE_PURE_CLIENT_SCENE ? UnityEngine.Color.white : (sgoi.CampId == cmpid ? UnityEngine.Color.white : UnityEngine.Color.red));
                                    NickNameGameObjectDic.Add(gui.m_ActorId, go);
                                }
                            }
                        }
                    }
                }

            }
        }
    }
    private void CreateNpcNickName(GfxUserInfo gfxNpc)
    {
        try
        {
            AboutNpcNickName(gfxNpc);
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void AboutNpcNickName(GfxUserInfo gui)
    {
        if (gui != null)
        {
            Data_NpcConfig dnc = NpcConfigProvider.Instance.GetNpcConfigById(gui.m_HeroId);
            if (dnc != null && dnc.m_ShowName)
            {
                UnityEngine.GameObject pargo = ArkCrossEngine.LogicSystem.GetGameObject(gui.m_ActorId);
                if (!NpcGameObjectS.ContainsKey(pargo))
                {
                    SharedGameObjectInfo sgoi = ArkCrossEngine.LogicSystem.GetSharedGameObjectInfo(gui.m_ActorId);
                    if (pargo != null && sgoi != null)
                    {
                        string path = UIManager.Instance.GetPathByName("NickName");
                        UnityEngine.Object obj = ResourceSystem.NewObject(path);
                        if (obj != null)
                        {
                            UnityEngine.GameObject go = obj as UnityEngine.GameObject;
                            if (go != null && DynamicWidgetPanel != null)
                            {
                                go = NGUITools.AddChild(DynamicWidgetPanel, obj);
                                if (go != null)
                                {
                                    NpcGameObjectS.Add(pargo, go);
                                    NickName nn = go.GetComponent<NickName>();
                                    if (nn != null)
                                    {
                                        nn.StartForShow();
                                        UnityEngine.Color color = UnityEngine.Color.white;
                                        UserInfo curself = null;
                                        if (LobbyClient.Instance.CurrentRole != null)
                                        {
                                            curself = LobbyClient.Instance.CurrentRole.GetPlayerSelfInfo();
                                        }
                                        if (m_SubSceneType == SceneSubTypeEnum.TYPE_PVAP && curself != null && sgoi.CampId != curself.GetCampId())
                                        {//敌方伙伴 红色
                                            color = UnityEngine.Color.red;
                                        }
                                        if (m_SceneType == SceneTypeEnum.TYPE_PURE_CLIENT_SCENE && dnc.m_NpcType == (int)NpcTypeEnum.Normal)
                                        {//主城的npc 颜色要特殊
                                            color = new UnityEngine.Color(23 / 255f, 178 / 255f, 1f);
                                        }
                                        nn.SetPlayerGameObjectAndNickName(pargo, dnc.m_Name, color, dnc.m_NpcType == (int)NpcTypeEnum.Partner ? true : false);//伙伴的话需要判断隐藏显示
                                        NickNameGameObjectDic.Add(gui.m_ActorId, go);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    UnityEngine.GameObject go = NpcGameObjectS[pargo];
                    if (go != null)
                    {
                        UILabel ul = go.GetComponent<UILabel>();
                        if (ul != null)
                        {
                            ul.text = dnc.m_Name;
                        }
                    }
                }
            }
        }
    }
    public IEnumerator DelayForNewbieGuide()
    {
        yield return new WaitForSeconds(0.5f);
        try
        {
            UIBeginnerGuideManager.Instance.IsBeginnerGuiderStarted = true;
            UIBeginnerGuideManager.Instance.TriggerNewbieGuide(UINewbieGuideTriggerType.T_MainCity, gameObject);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void TriggerNewbieGuide()
    {
        UIBeginnerGuideManager.Instance.IsBeginnerGuiderStarted = true;
        UIBeginnerGuideManager.Instance.TriggerNewbieGuide(UINewbieGuideTriggerType.T_MainCity, gameObject);
    }
    private void ExpeditionFailure(ArkCrossEngine.Network.GeneralOperationResult result)
    {
        if (result == ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_InMatching)
        {
            string chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.Format(458);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
        }
    }
    public void OnChainBeat(int number)
    {
        try
        {
            UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
            if (go != null)
            {
                ChainBeat cb = go.GetComponent<ChainBeat>();
                if (cb != null)
                {
                    cb.UpdateHitCount(number);
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void InitStoryDlg()
    {
        StoryDlgManager.Instance.Init();
        ClickNpcManager.Instance.Init();
    }
    private void OnEnterNewScene()
    {
        //隐藏剧情对话框
        if (m_StoryDlgSmallGO != null)
        {
            NGUITools.SetActive(m_StoryDlgSmallGO, false);
        }
        if (m_StoryDlgBigGO != null)
        {
            NGUITools.SetActive(m_StoryDlgBigGO, false);
        }
    }
    //蓄力
    public void ShowMonsterPrePower(float x, float y, float z, float duration, int monsterId)
    {
        try
        {
            if (duration <= 0)
                return;
            UnityEngine.Vector3 pos = new UnityEngine.Vector3(x, y, z);
            if (UnityEngine.Camera.main != null)
                pos = UnityEngine.Camera.main.WorldToScreenPoint(pos);
            pos.z = 0;
            UnityEngine.Vector3 nguiPos = UnityEngine.Vector3.zero;
            if (UICamera.mainCamera != null)
            {
                nguiPos = UICamera.mainCamera.ScreenToWorldPoint(pos);
            }

            UnityEngine.GameObject go = ArkCrossEngine.ResourceSystem.GetSharedResource("UI/MonsterPrePower") as GameObject;
            UnityEngine.GameObject prePowerGo = NGUITools.AddChild(this.gameObject, go);
            if (prePowerGo == null)
                return;
            prePowerGo.transform.position = nguiPos;
            MonsterPrePower power = prePowerGo.GetComponent<MonsterPrePower>();
            if (power != null)
            {
                power.Duration = duration;
                power.PowerId = monsterId;
                power.Position = new UnityEngine.Vector3(x, y, z);
            }
            else
            {
                NGUITools.SetActive(prePowerGo, false);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //打断蓄力
    public void BreakPrePower(int monsterId)
    {
        for (int index = 0; index < this.transform.childCount; ++index)
        {
            UnityEngine.Transform trans = this.transform.GetChild(index);
            UnityEngine.GameObject go = null;
            if (trans != null)
                go = trans.gameObject;
            if (go != null && go.name == "MonsterPrePower(Clone)")
            {
                MonsterPrePower power = go.GetComponent<MonsterPrePower>();
                if (power == null)
                    return;
                if (power.PowerId == monsterId)
                {
                    NGUITools.SetActive(go, false);
                    NGUITools.Destroy(go);
                }
            }
        }
    }
    //********************************************************************
    public void ShowbloodFlyTemplate(float x, float y, float z, int blood, bool isOrdinaryDamage)
    {
        try
        {
            UnityEngine.GameObject go = GameObjectManager.Instance.NewObject("AttackEffect", timeRecycle);
            if (null != go)
            {
                UnityEngine.Transform tf = go.transform;
                if (tf.parent != null && tf.parent != DynamicWidgetPanel.transform)
                {
                    tf.parent = DynamicWidgetPanel.transform;
                }
                UnityEngine.Vector3 pos = new UnityEngine.Vector3(x, y, z);
                pos = UnityEngine.Camera.main.WorldToScreenPoint(pos);
                pos.y += (100 + offsetheight);
                pos.z = 0;
                offsetheight *= -1;
                pos = UICamera.mainCamera.ScreenToWorldPoint(pos);
                tf.position = pos;
                BloodAnimationScript bas = tf.GetComponent<BloodAnimationScript>();
                if (bas != null)
                {
                    bas.SetText(blood.ToString());
                    if (isOrdinaryDamage)
                    {
                        bas.SetTextColor(new UnityEngine.Color(1.0f, 1.0f, 1.0f));
                    }
                    else
                    {
                        bas.SetTextColor(new UnityEngine.Color(0.92549f, 0.7098f, 0.0f));
                    }
                    bas.PlayAnimation();
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private int offsetheight = 20;
    //**************************************************************
    public void ShowAddHPForHero(float x, float y, float z, int blood)
    {
        try
        {
            int offset = 0;
            UnityEngine.GameObject go = null;
            if (blood > 0)
            {
                go = GameObjectManager.Instance.NewObject("DamageForAddHero", timeRecycle);
                offset = 100;
            }
            else
            {
                go = GameObjectManager.Instance.NewObject("DamageForCutHero", timeRecycle);
                offset = -50;
            }
            if (null != go)
            {
                UnityEngine.Transform tf = go.transform;
                if (DynamicWidgetPanel != null && tf.parent != DynamicWidgetPanel.transform)
                {
                    tf.parent = DynamicWidgetPanel.transform;
                }
                UnityEngine.Vector3 pos = new UnityEngine.Vector3(x, y, z);
                pos = UnityEngine.Camera.main.WorldToScreenPoint(pos);
                pos.z = 0; pos.y += offset;
                pos = UICamera.mainCamera.ScreenToWorldPoint(pos);

                tf.position = pos;
                BloodAnimationScript bas = tf.GetComponent<BloodAnimationScript>();
                if (bas != null)
                {
                    bas.SetText(blood.ToString());
                    bas.PlayAnimation();
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void ShowAddMPForHero(float x, float y, float z, int energy)
    {
        try
        {
            int offset = 0;
            UnityEngine.GameObject go = null;
            if (energy > 0)
            {
                go = GameObjectManager.Instance.NewObject("EnergyAdd", timeRecycle);
                offset = 100;
            }
            else
            {
                go = GameObjectManager.Instance.NewObject("EnergyCut", timeRecycle);
                offset = -50;
            }
            if (null != go)
            {
                UnityEngine.Transform tf = go.transform;
                if (DynamicWidgetPanel != null && tf.parent != DynamicWidgetPanel.transform)
                {
                    tf.parent = DynamicWidgetPanel.transform;
                }
                UnityEngine.Vector3 pos = new UnityEngine.Vector3(x, y, z);
                pos = UnityEngine.Camera.main.WorldToScreenPoint(pos);
                pos.z = 0; pos.y += offset;
                pos = UICamera.mainCamera.ScreenToWorldPoint(pos);
                tf.position = pos;
                BloodAnimationScript bas = tf.GetComponent<BloodAnimationScript>();
                if (bas != null)
                {
                    bas.SetText(energy.ToString());
                    bas.PlayAnimation();
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //*********************************************
    public void ShowCriticalDamage(float x, float y, float z, int blood, bool isOrdinaryDamage)
    {
        try
        {
            UnityEngine.GameObject go = GameObjectManager.Instance.NewObject("CriticalDamage", timeRecycle);
            if (null != go)
            {
                UnityEngine.Transform tf = go.transform;
                if (DynamicWidgetPanel != null && tf.parent != DynamicWidgetPanel.transform)
                {
                    tf.parent = DynamicWidgetPanel.transform;
                }
                UnityEngine.Vector3 pos = new UnityEngine.Vector3(x, y, z);
                pos = UnityEngine.Camera.main.WorldToScreenPoint(pos);
                pos.y += (100 + offsetheight);
                pos.z = 0;
                offsetheight *= -1;
                pos = UICamera.mainCamera.ScreenToWorldPoint(pos);
                tf.position = pos;
                BloodAnimationScript bas = tf.GetComponent<BloodAnimationScript>();
                if (bas != null)
                {
                    bas.SetText(blood.ToString());
                    if (isOrdinaryDamage)
                    {
                        bas.SetTextColor(new UnityEngine.Color(1.0f, 1.0f, 1.0f));
                    }
                    else
                    {
                        bas.SetTextColor(new UnityEngine.Color(0.92549f, 0.7098f, 0.0f));
                    }
                    bas.PlayAnimation();
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void ShowGainMoney(float x, float y, float z, int num)
    {
        try
        {
            UnityEngine.GameObject go = GameObjectManager.Instance.NewObject("GainMoney", timeRecycle);
            if (null != go)
            {
                UnityEngine.Transform tf = go.transform;
                if (DynamicWidgetPanel != null && tf.parent != DynamicWidgetPanel.transform)
                {
                    tf.parent = DynamicWidgetPanel.transform;
                }
                UnityEngine.Vector3 pos = new UnityEngine.Vector3(x, y, z);
                pos = UnityEngine.Camera.main.WorldToScreenPoint(pos);
                pos.y += (100 + offsetheight);
                pos.z = 0;
                offsetheight *= -1;
                pos = UICamera.mainCamera.ScreenToWorldPoint(pos);
                tf.position = pos;
                BloodAnimationScript bas = tf.GetComponent<BloodAnimationScript>();
                if (bas != null)
                {
                    bas.SetText(num > 0 ? "+" + num : num.ToString());
                    bas.PlayAnimation();
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void LoadResourcesInMainCity()
    {
        GameObjectManager.Instance.NewObject("ScreenTip", 10, ScreenTipPanel);
    }
    public void LoadSomeResources()
    {
        try
        {
            GameObjectManager.Instance.NewObject("AttackEffect", 20, DynamicWidgetPanel);
            GameObjectManager.Instance.NewObject("CriticalDamage", 10, DynamicWidgetPanel);
            GameObjectManager.Instance.NewObject("DamageForAddHero", 6, DynamicWidgetPanel);
            GameObjectManager.Instance.NewObject("DamageForCutHero", 6, DynamicWidgetPanel);
            GameObjectManager.Instance.NewObject("EnergyAdd", 6, DynamicWidgetPanel);
            GameObjectManager.Instance.NewObject("EnergyCut", 6, DynamicWidgetPanel);
            GameObjectManager.Instance.NewObject("ScreenTip", 10, ScreenTipPanel);
            GameObjectManager.Instance.NewObject("GainMoney", 6, DynamicWidgetPanel);
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void ShowStageClear(string type)
    {
        UnityEngine.Transform existTrans = this.transform.Find("StageClear(Clone)");
        if (null != existTrans)
        {
            UnityEngine.GameObject existGo = existTrans.gameObject;
            if (null != existGo)
                Destroy(existGo);
        }

        UnityEngine.GameObject go = ArkCrossEngine.ResourceSystem.GetSharedResource("UI/StageClear") as GameObject;
        if (null == go)
            return;
        go = NGUITools.AddChild(this.gameObject, go);
        if (go == null)
            return;
        StageClear stageClear = go.GetComponent<StageClear>();
        if (stageClear != null)
            stageClear.SetClearType(type);
    }

    //*******************************************
    void StartLoading()
    {
        try
        {
            UIDataCache.Instance.isLoadingEnd = false;

            // clear npc nick name
            NpcGameObjectS.Clear();
            // unload all ui window
            UIManager.Instance.UnLoadAllWindow();

            // hide connect window
            UnityEngine.GameObject goConnect = UIManager.Instance.GetWindowGoByName("Connect");
            if (goConnect != null)
            {
                GfxSystem.PublishGfxEvent("ge_ui_connect_hint", "ui", false, false);
            }

            // hide mars window
            UnityEngine.GameObject mgo = UIManager.Instance.GetWindowGoByName("Mars");
            if (mgo != null)
            {
                UIManager.Instance.HideWindowByName("Mars");
            }

            // disable joystick while loading
            if (InputType.Joystick == DFMUiRoot.InputMode)
            {
                JoyStickInputProvider.JoyStickEnable = false;
            }

            // skip if already in loading state
            if (loading != null) return;

            // show loading prefab, fill background picture and notice runtime
            UnityEngine.GameObject go = ResourceSystem.GetSharedResource("Loading/Loading2") as GameObject;
            if (go != null)
            {
                // change loading tips
                ChangeRandomNotice();
                // change background picture
                ChangeBg(go);
                loading = NGUITools.AddChild(gameObject, go);
                if (loading != null)
                {
                    loading.transform.localPosition = new UnityEngine.Vector3(0, 0, 0);
                    NGUITools.SetActive(loading, true);
                }
            }
        }
        catch (Exception ex)
        {
            LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    
    void ChangeBg(UnityEngine.GameObject go)
    {
        // read picture id from table, set it if found one, or use default instead
        int num = CrossEngineHelper.Random.Next(1, 3);
        LoadingChange lg = LoadingChangeProvider.Instance.GetDataById(num);
        if (lg != null)
        {
            SetBgTexture(lg.m_Path, go);
        }
        else
        {
            SetBgTexture("UI_AtlasDynamic/LoadingBg/background_login_v1", go);
        }
    }
    
    void SetBgTexture(string path, UnityEngine.GameObject go)
    {
        UnityEngine.Texture tex = ResourceSystem.GetSharedResource(path) as Texture;
        UnityEngine.Transform tf = go.transform.Find("DarkBack/Back");
        UITexture ut = tf.GetComponent<UITexture>();
        if (ut != null)
        {
            if (tex != null)
            {
                ut.mainTexture = tex;
            }
        }
    }
    
    void ChangeRandomNotice()
    {
        string notice = string.Empty;

        // try get tips string id from table for current loading scene
        foreach (LoadingChange info in LoadingChangeProvider.Instance.GetData().Values)
        {
            if (info.m_Id == UIDataCache.Instance.curSceneId && info.m_Name == "no")
            {
                notice = StrDictionaryProvider.Instance.GetDictString((int)info.m_strId);
                break;
            }
        }

        // if no matching tips, random one
        if (string.IsNullOrEmpty(notice))
        {
            // FixMe: get range of tips
            int num = CrossEngineHelper.Random.Next(0, 30);
            notice = StrDictionaryProvider.Instance.GetDictString(650 + num);
        }

        // still not found? use default instead
        if (string.IsNullOrEmpty(notice))
        {
            notice = "加载场景不耗流量";
        }

        // cache current loading tips
        LogicSystem.UpdateLoadingTip(notice);
    }

    void EndLoading()
    {
        try
        {
            if (loading != null)
            {
                loading.transform.Find("ProgressBar").SendMessage("EndLoading");
                loading = null;
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void HideInputUi()
    {
        try
        {
            //UIManager.Instance.HideWindowByName("SkillBar");

            UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
            SkillBar sBar = null;
            if (go != null)
            {
                sBar = go.GetComponent<SkillBar>();
                if (sBar != null) sBar.SetActive(false);
            }
            JoyStickInputProvider.JoyStickEnable = false;
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void OnPlayerSelfCreated()
    {
        if (InputType.Joystick == DFMUiRoot.InputMode)
        {
            DFMUiRoot.InputMode = InputType.Joystick;
            GfxModule.Skill.GfxSkillSystem.Instance.ChangeSkillControlMode(ArkCrossEngine.LogicSystem.PlayerSelf, SkillControlMode.kJoystick);
        }
        else
        {
            DFMUiRoot.InputMode = InputType.Touch;
            GfxModule.Skill.GfxSkillSystem.Instance.ChangeSkillControlMode(ArkCrossEngine.LogicSystem.PlayerSelf, SkillControlMode.kTouch);
        }
    }

    private void SetGestureEnable(bool value)
    {
        if (InputMode == InputType.Joystick)
        {
            return;
        }
        TouchManager.GestureEnable = value;
    }

    private static InputType inputMode = InputType.Joystick;
    public static InputType InputMode
    {
        get
        {
            return inputMode;
        }
        set
        {
            UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
            SkillBar sBar = null;
            if (go != null)
            {
                sBar = go.GetComponent<SkillBar>();
            }
            inputMode = value;
            if (InputType.Joystick == inputMode)
            {
                JoyStickInputProvider.JoyStickEnable = true;
                TouchManager.GestureEnable = false;
                if (sBar != null) sBar.SetActive(true);
                //UIManager.Instance.ShowWindowByName("SkillBar");
            }
            else
            {
                TouchManager.GestureEnable = true;
                JoyStickInputProvider.JoyStickEnable = false;
                if (sBar != null) sBar.SetActive(false);
                //UIManager.Instance.HideWindowByName("SkillBar");

            }
        }
    }
    //***********************
    private void ShowYesOrNot(string message, string button, Action<bool> dofunction)
    {
        try
        {
            if (message != null && dofunction != null)
            {
                UnityEngine.GameObject go = ArkCrossEngine.ResourceSystem.GetSharedResource("UI/ConfirmDlg/ConfirmDlg") as GameObject;
                if (go != null)
                {
                    go = NGUITools.AddChild(gameObject, go);
                    if (go != null)
                    {
                        YesOrNot yon = go.GetComponent<YesOrNot>();
                        yon.SetMessageAndDO(message, button, dofunction);
                    }
                    NGUITools.SetActive(go, true);
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void ShowNewbieGuide(List<int> idlist)
    {
        try
        {
            if (idlist != null)
            {
                NewbieGuideManager ngm = gameObject.AddComponent<NewbieGuideManager>();
                if (ngm != null)
                {
                    ngm.SetMySelf(ngm, transform);
                    ngm.DoInitGuid(idlist);
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void SetPveFightInfo(int type, int num0, int num1, int num2)
    {
        Debug.Log("----pvap: in pvap info!!");
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
        if (go != null)
        {
            PveFightInfo pfi = go.GetComponent<PveFightInfo>();
            if (pfi != null)
            {
                pfi.SetActive(true);
                pfi.SetInitInfo(type, num0, num1, num2);
                if (type == 4)
                {
                    //pfi.enabled = false;
                    //pfi.SetActive(false);
                }
                else
                {
                    //pfi.SetActive(true);
                    pfi.enabled = true;
                    if (pfi.timeOrSome != null)
                    {
                        NGUITools.SetActive(pfi.timeOrSome, true);
                    }
                }
            }
        }
    }

    //小怪盾条
    private void CreateMonestSheildBlood(int actorid, int type)
    {
        try
        {
            UnityEngine.GameObject pargo = LogicSystem.GetGameObject(actorid);
            if (pargo != null)
            {
                string path = UIManager.Instance.GetPathByName("Sheild");
                UnityEngine.GameObject go = ArkCrossEngine.ResourceSystem.GetSharedResource(path) as GameObject;
                if (go != null && DynamicWidgetPanel != null)
                {
                    go = NGUITools.AddChild(DynamicWidgetPanel, go);
                    if (go != null)
                    {
                        Sheild dun = go.GetComponent<Sheild>();
                        if (dun != null)
                        {
                            dun.InitSheild(actorid, type, pargo);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void HandleServerselectNetworkFailed()
    {
        ArkCrossEngine.MyAction<int> Func = ServerSelectNetworkFailedCallBack;
        string CHN_CONFIRM = StrDictionaryProvider.Instance.GetDictString(4); //确定
                                                                              //string CHN_CANCEL = StrDictionaryProvider.Instance.GetDictString(9);  //取消
        string CHN_DESC = StrDictionaryProvider.Instance.GetDictString(16);
        LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", CHN_DESC, CHN_CONFIRM, null, null, Func, false);
    }
    private void ServerSelectNetworkFailedCallBack(int action)
    {
        ArkCrossEngine.GfxSystem.PublishGfxEvent("ge_enable_accountlogin", "ui");
        UIManager.Instance.ShowWindowByName("LoginPrefab");
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_create_hero_scene", "ui", false);
        UnityEngine.GameObject go = UnityEngine.GameObject.Find(ArkCrossEngine.GlobalVariables.cGameRootName);
        if (go != null)
        {
            GameLogic gameLogic = go.GetComponent<GameLogic>();
            if (gameLogic != null) gameLogic.RestartLogic();
        }
    }

}

