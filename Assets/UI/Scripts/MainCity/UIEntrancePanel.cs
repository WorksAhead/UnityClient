using UnityEngine;
using ArkCrossEngine;
using System;
using System.Collections.Generic;

public struct NewThings
{
    public string name;
    public int id;
    public int num;
    public UnityEngine.Transform tf;
    public GetNewThingsType type;
}
public enum MatchingType : byte
{
    PVP,
    Trial,
    Gold
}
public enum SystemNewTipType : byte
{
    Equip_Upgrade,
    Equip_Tip,
    GodEquip,
    Xhun,
    Partner,
    Mail,
    Skill,
    Task,
    Award,
    Activity,
    pvp,
    Settings
}
public class UIEntrancePanel : UnityEngine.MonoBehaviour
{
    //
    public UISprite entrance = null;
    private const int c_ButtonNumber = 13;
    public UnityEngine.GameObject[] buttons = new UnityEngine.GameObject[c_ButtonNumber];
    public UnityEngine.GameObject RightBtn = null;
    public UnityEngine.GameObject BottomBtn = null;
    public UnityEngine.GameObject pointingParent = null;
    public int C_TransOffset = 108;
    private bool hasAdded = false;
    public int c_TabButtons = 7;
    private UnityEngine.GameObject[] btsPointing = null;
    private List<NewThings> newThingsList = new List<NewThings>();
    private int tempMoney = 0;

    public UnityEngine.AnimationCurve CurveOut = null;
    public float DurationFor = 2.0f;
    public UnityEngine.AnimationCurve CurveAlpha = null;
    public UnityEngine.AnimationCurve CurveBack = null;
    public float DurationBack = 2.0f;
    public float DurationOut = 2.0f;
    // Use this for initialization
    void Awake()
    {
        try
        {
            btsPointing = new UnityEngine.GameObject[c_TabButtons];
            InitBtsPointing();

            object obj = LogicSystem.EventChannelForGfx.Subscribe<SystemNewTipType, bool>("ge_systemnewtip_state_change", "ui", OnSystemNewTipStateChange);//需保证此ui在其他发此消息的ui之前加载，目前为装备，神器，魂，伙伴，邮件
            if (obj != null)
                eventlist.Add(obj);

            RoleInfo role = LobbyClient.Instance.CurrentRole;
            if (role.LevelUp)
            {
                InitLevelLock(role.Level - 1);
            }
            else
            {
                InitLevelLock(role.Level);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private List<object> eventlist = new List<object>();
    public void UnSubscribe()
    {
        try
        {
            if (eventlist != null)
            {
                for (int i = 0; i < eventlist.Count; i++)
                {
                    if (eventlist[i] != null)
                    {
                        ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eventlist[i]);
                    }
                }
            }
            eventlist.Clear();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Start()
    {
        try
        {
            for (int index = 0; index < buttons.Length; ++index)
            {
                if (buttons[index] != null)
                    UIEventListener.Get(buttons[index]).onClick += this.OnButtonClick;
            }
            object eo;
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (eo != null)
                eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("after_levelup", "ui_effect", UserLevelUP);
            if (eo != null)
                eventlist.Add(eo);
            object obj = LogicSystem.EventChannelForGfx.Subscribe<int>("ge_add_partner", "ui", HandlerAddPartner);
            if (obj != null)
                eventlist.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<MatchingType, bool>("ge_match_state_change", "matching", OnMatchStateChange);
            if (obj != null)
                eventlist.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<string>("get_new_function_effect", "ui_effect", OnFunctionEffect);
            if (obj != null)
                eventlist.Add(obj);

            RoleInfo role = LobbyClient.Instance.CurrentRole;
            if (role != null)
            {
                tempMoney = role.Money;
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OnFunctionEffect(string btn)
    {
        if (btn != "")
            SetBtnaActive(btn, true);
    }
    void ShowAllWindows()
    {
        UIManager.Instance.OpenExclusionWindow("");
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
                CheckUnlockSkill();
                HasNewThings();
                time = 0f;
            }

            RoleInfo role = LobbyClient.Instance.CurrentRole;
            if (role != null)
            {
                if (tempMoney != role.Money)
                {//金钱变化了
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_entrance_gold_change", "ui");
                }
                tempMoney = role.Money;
            }
            //RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            //if (null != role_info && role_info.Level > m_RoleCurrentLevel) {
            //m_RoleCurrentLevel = role_info.Level;
            //RepositionTable(role_info.Level);
            //}
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private bool isShowBtn = false;
    public void OnEntranceBtnClick()
    {
        isShowBtn = !isShowBtn;
        if (RightBtn != null && BottomBtn != null)
        {
            int len = rightButtonList.Count > bottomButtonList.Count ? rightButtonList.Count : bottomButtonList.Count;
            if (!isShowBtn)
            {
                TweenRightBackBtns(RightBtn, new UnityEngine.Vector3(0f, 0f, 0f), len);
                TweenBottonBackBtns(BottomBtn, new UnityEngine.Vector3(0f, 0f, 0f), len);
                TweenButtonRotation(entrance.gameObject, 45, len);
                SetBtsPointingActive(false);
                TweenButtonAlpha(RightBtn, 0, len);
                TweenButtonAlpha(BottomBtn, 0, len);
            }
            else
            {
                NGUITools.SetActive(RightBtn, true);
                SetBtsPointingActive(true);
                TweenOutBtns(RightBtn, new UnityEngine.Vector3(0f, rightButtonList.Count * btnDistance, 0f), len);
                TweenOutBtns(BottomBtn, new UnityEngine.Vector3(-bottomButtonList.Count * btnDistance, 0f, 0f), len);
                TweenButtonRotation(entrance.gameObject, 0, len);
                TweenButtonAlpha(RightBtn, 1, len);
                TweenButtonAlpha(BottomBtn, 1, len);
            }
        }
    }
    void OnEnable()
    {
        try
        {
            RoleInfo role = LobbyClient.Instance.CurrentRole;
            if (wipeLevelUp)
            {
                wipeLevelUp = false;
                CheckNewThings(role.Level);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //播放获得新物品特效 关闭所有ui
    void CloseUiAfterGetNewThing()
    {
        if (!UIManager.Instance.IsAnyExclusionWindowVisble())
        {
            UIManager.Instance.CloseExclusionWindow("MainCityUI");
        }
    }
    //检查新物品
    void CheckNewThings(int level)
    {
        IsUnlockFunction(level);
        HasNewSkill();
        HasNewPartener();
        CloseUiAfterGetNewThing();
        HasNewThings();
        Invoke("ShowAllWindows", newThingsList.Count * DurationFor);
    }
    private bool wipeLevelUp = false;
    void UserLevelUP(int level)
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        if (UIManager.Instance.IsWindowVisible("WipeoutAward"))
        {
            wipeLevelUp = true;
            return;
        }
        if (CheckUnlockFunction(level))
        {
            isShowBtn = true;
            RightBtn.transform.localPosition = new UnityEngine.Vector3(0f, rightButtonList.Count * btnDistance, 0f);
            BottomBtn.transform.localPosition = new UnityEngine.Vector3(0f - bottomButtonList.Count * btnDistance, 0f, 0f);
            BottomBtn.GetComponent<UIWidget>().alpha = 1f;
            RightBtn.GetComponent<UIWidget>().alpha = 1f;
        }
        CheckNewThings(level);
    }
    //有新伙伴
    void HasNewPartener()
    {
        if (m_partnerId != 0)
        {
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            if (null != role_info && role_info.PartnerStateInfo != null)
            {
                List<PartnerInfo> partners = role_info.PartnerStateInfo.GetAllPartners();
                if (null == partners)
                    return;
                for (int index = 0; index < partners.Count; ++index)
                {
                    if (partners[index] != null && partners[index].Id == m_partnerId)
                    {
                        //找到添加伙伴信息
                        PartnerInfo info = partners[index];
                        Data_NpcConfig npcCfg = NpcConfigProvider.Instance.GetNpcConfigById(info.LinkId);
                        if (npcCfg != null)
                        {
                            NewThings newThings = new NewThings();
                            newThings.id = info.LinkId;
                            newThings.type = GetNewThingsType.T_Partner;
                            newThings.tf = GetBtnGameObject("Entrance-Partner").transform;
                            newThingsList.Add(newThings);
                        }
                    }
                }
            }
            m_partnerId = 0;
        }
    }
    private int m_partnerId = 0;
    private void HandlerAddPartner(int partnerId)
    {
        UnityEngine.GameObject btn = GetBtnGameObject("Entrance-Partner");
        if (!NGUITools.GetActive(btn))
        {
            m_partnerId = partnerId;
            return;
        }
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (null != role_info && role_info.PartnerStateInfo != null)
        {
            List<PartnerInfo> partners = role_info.PartnerStateInfo.GetAllPartners();
            if (null == partners)
                return;
            for (int index = 0; index < partners.Count; ++index)
            {
                if (partners[index] != null && partners[index].Id == partnerId)
                {
                    //找到添加伙伴信息
                    PartnerInfo info = partners[index];
                    Data_NpcConfig npcCfg = NpcConfigProvider.Instance.GetNpcConfigById(info.LinkId);
                    if (npcCfg != null)
                    {
                        NewThings newThings = new NewThings();
                        newThings.id = info.LinkId;
                        newThings.type = GetNewThingsType.T_Partner;
                        newThings.tf = GetBtnGameObject("Entrance-Partner").transform;
                        newThingsList.Add(newThings);
                    }
                }
            }
        }
    }
    public void SetBtsPointingActive(bool active)
    {
        for (int idx = 0; idx < c_TabButtons; ++idx)
        {
            if (btsPointing[idx] != null)
            {
                NGUITools.SetActive(btsPointing[idx], active);
            }
        }
    }
    public void InitBtsPointing()
    {
        for (int idx = 0; idx < c_TabButtons; ++idx)
        {
            if (btsPointing[idx] != null)
            {
                btsPointing[idx] = null;
            }
        }
    }
    public void AddBtsPointing(UnityEngine.GameObject obj)
    {
        for (int idx = 0; idx < c_TabButtons; ++idx)
        {
            if (btsPointing[idx] == null)
            {
                btsPointing[idx] = obj;
                return;
            }
        }
    }
    public void RemoveBtsPointing(string name)
    {
        for (int idx = 0; idx < c_TabButtons; ++idx)
        {
            if (btsPointing[idx] != null && btsPointing[idx].name == name)
            {
                NGUITools.Destroy(btsPointing[idx]);
                btsPointing[idx] = null;
                return;
            }
        }
    }

    public void OnButtonClick(UnityEngine.GameObject go)
    {
        if (go == null)
            return;
        delayBtnName = "";
        switch (go.name)
        {
            //case "Entrance-Equipment":
            //  OpenAndCloseWindow("GamePokey");
            //  break;
            //case "Entrance-Skill":
            //  OpenAndCloseWindow("SkillPanel");
            //  break;
            //case "Entrance-GodEquip":
            //  OpenAndCloseWindow("ArtifactPanel");
            //  break;
            //case "Entrance-Mission":
            //  OpenAndCloseWindow("GameTask");
            //  JoyStickInputProvider.JoyStickEnable = false;
            //  ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_reload_missions", "lobby");
            //  if (WorldSystem.Instance.IsPureClientScene()) {
            //    CYGTConnector.HideCYGTSDK();
            //  }
            //  break;
            case "Entrance-Match": /*EnterInScene(2);*/
                                   //OpenAndCloseWindow("PvPEntrance");
                LogicSystem.SendStoryMessage("cityplayermove", 2);
                break;
            //case "Entrance-Treasure":
            //  RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            //  if (role_info != null && role_info.Level < ExpeditionPlayerInfo.c_UnlockLevel) {
            //    string chn_desc = StrDictionaryProvider.Instance.GetDictString(456);
            //    string chn_confirm = StrDictionaryProvider.Instance.GetDictString(4);
            //    ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", chn_desc, chn_confirm, null, null, null, false);
            //  } else {
            //    OpenAndCloseWindow("cangbaotu");
            //  }
            //  break;
            //case "Entrance-Mail":
            //  OpenAndCloseWindow("Mail");
            //  break;
            //case "Entrance-Friend":
            //  LogicSystem.PublishLogicEvent("ge_request_friends", "lobby");
            //  OpenAndCloseWindow("Friend");
            //  break;
            //case "Entrance-XHun":
            //  OpenAndCloseWindow("XHun");
            //  break;
            //case "Entrance-Partner":
            //  OpenAndCloseWindow("Partner");
            //  break;
            case "Entrance-Trial":
                //LogicSystem.SendStoryMessage("cityplayermove", 1);
                break;
            //case "Entrance-Shop":
            //  OpenAndCloseWindow("Store");
            //  break;
            //case "Entrance-Award":
            //  OpenAndCloseWindow("ActivityAward");
            //  break;
            default:
                delayBtnName = go.name;
                UnityEngine.GameObject window = UIManager.Instance.GetWindowGoByName("MainCityUI");
                if (window != null)
                {
                    MainCityTopTween script = window.GetComponentInChildren<MainCityTopTween>();
                    if (script != null)
                    {
                        script.Up();
                        Invoke("OpenUi", 0.3f);
                    }
                    else
                    {
                        OpenUi();
                    }
                }
                break;
        }
    }
    private string delayBtnName = "";
    private void OpenUi()
    {
        if (string.IsNullOrEmpty(delayBtnName))
        {
            return;
        }
        UnityEngine.GameObject window = UIManager.Instance.GetWindowGoByName("MainCityUI");
        if (window != null)
        {
            MainCityTopTween script = window.GetComponentInChildren<MainCityTopTween>();
            if (script != null)
            {
                script.Down();
            }
        }
        switch (delayBtnName)
        {
            case "Entrance-Equipment":
                OpenAndCloseWindow("GamePokey");
                break;
            case "Entrance-Skill":
                OpenAndCloseWindow("SkillPanel");
                break;
            case "Entrance-GodEquip":
                OpenAndCloseWindow("ArtifactPanel");
                break;
            case "Entrance-Mission":
                OpenAndCloseWindow("GameTask");
                JoyStickInputProvider.JoyStickEnable = false;
                ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_reload_missions", "lobby");
                break;
            //case "Entrance-Match": /*EnterInScene(2);*/
            //  //OpenAndCloseWindow("PvPEntrance");
            //  LogicSystem.SendStoryMessage("cityplayermove", 2);
            //  break;
            case "Entrance-Treasure":
                RoleInfo role_info = LobbyClient.Instance.CurrentRole;
                if (role_info != null && role_info.Level < ExpeditionPlayerInfo.c_UnlockLevel)
                {
                    string chn_desc = StrDictionaryProvider.Instance.GetDictString(456);
                    string chn_confirm = StrDictionaryProvider.Instance.GetDictString(4);
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", chn_desc, chn_confirm, null, null, null, false);
                }
                else
                {
                    OpenAndCloseWindow("cangbaotu");
                }
                break;
            case "Entrance-Mail":
                //OpenAndCloseWindow("Mail");
                break;
            case "Entrance-Friend":
                LogicSystem.PublishLogicEvent("ge_request_friends", "lobby");
                OpenAndCloseWindow("Friend");
                break;
            case "Entrance-XHun":
                OpenAndCloseWindow("XHun");
                break;
            case "Entrance-Partner":
                OpenAndCloseWindow("Partner");
                break;
            //case "Entrance-Trial":
            //  LogicSystem.SendStoryMessage("cityplayermove", 1);
            //  break;
            case "Entrance-Shop":
                OpenAndCloseWindow("Store");
                break;
            case "Entrance-Award":
                //OpenAndCloseWindow("ActivityAward");
                break;
            case "Entrance-Settings":
                break;
        }
    }

    private void InputModeSwitch()
    {
        if (DFMUiRoot.InputMode == InputType.Joystick)
        {
            DFMUiRoot.InputMode = InputType.Touch;
        }
        else
        {
            DFMUiRoot.InputMode = InputType.Joystick;
        }
    }
    private void OpenAndCloseWindow(string window)
    {
        UIManager.Instance.ToggleWindowVisible(window);
    }
    //临时增加PvP与MPvE入口
    private void EnterInScene(int sceneId)
    {
        if (sceneId != -1)
        {
            //Debug.Log("ge_select_scene sceneId:" + sceneId);
            ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_select_scene", "lobby", sceneId);
        }
        else
        {
            Debug.Log("sceneId is -1!!");
        }
    }

    //检查是否有可解锁技能
    private void CheckUnlockSkill()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null && role_info.SkillInfos != null)
        {
            SkillInfo skill_info = null;
            int index = 0;
            for (index = 0; index < role_info.SkillInfos.Count; ++index)
            {
                skill_info = role_info.SkillInfos[index];
                if (skill_info != null && skill_info.SkillLevel <= 0)
                {
                    if (skill_info.ConfigData != null && skill_info.ConfigData.ActivateLevel <= role_info.Level)
                    {

                        //有可解锁技能
                        AddPointingToSkill(true);
                        AutoUnlockSkill(skill_info);
                        break;
                    }
                }
            }
            if (index >= role_info.SkillInfos.Count)
                AddPointingToSkill(false);//没有可解锁技能
        }
    }
    private void AutoUnlockSkill(SkillInfo info)
    {
        if (null != info & info.SkillLevel <= 0)
        {
            LogicSystem.PublishLogicEvent("ge_unlock_skill", "lobby", UISkillSetting.presetIndex, info.SkillId);
        }
    }
    //新技能
    void HasNewSkill()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null && role_info.SkillInfos != null)
        {
            SkillInfo skill_info = null;
            int index = 0;
            for (index = 0; index < role_info.SkillInfos.Count; ++index)
            {
                skill_info = role_info.SkillInfos[index];
                if (skill_info != null && skill_info.SkillLevel <= 0)
                {
                    if (skill_info.ConfigData != null && skill_info.ConfigData.ActivateLevel <= role_info.Level)
                    {
                        NewThings newThings = new NewThings();
                        newThings.id = skill_info.ConfigData.SkillId;
                        newThings.type = GetNewThingsType.T_Skill;
                        newThings.tf = GetBtnGameObject("Entrance-Skill").transform;
                        newThingsList.Add(newThings);
                    }
                }
            }
        }
    }
    //是否有新的物品
    void HasNewThings()
    {
        if (newThingsList.Count > 0)
        {
            OnGetNewThings(newThingsList[0]);
            newThingsList.RemoveAt(0);
        }
    }
    //技能开关上添加、删除指引图标
    private void AddPointingToSkill(bool canUnlock)
    {
        if (canUnlock)
        {
            if (!hasAdded)
            {
                string path = UIManager.Instance.GetPathByName("Pointing");
                UnityEngine.GameObject go = ResourceSystem.GetSharedResource(path) as UnityEngine.GameObject;
                if (null != go)
                {
                    UnityEngine.GameObject goFather = GetBtnGameObject("Entrance-Skill");
                    if (goFather != null)
                    {
                        UnityEngine.GameObject parent = new UnityEngine.GameObject();
                        parent = NGUITools.AddChild(pointingParent, parent);
                        parent.transform.localPosition = goFather.transform.localPosition;
                        parent.name = goFather.name;
                        AddBtsPointing(parent);
                        go = NGUITools.AddChild(parent, go);
                        UISprite sp = goFather.GetComponent<UISprite>();
                        if (sp != null)
                        {
                            go.transform.localPosition = new UnityEngine.Vector3(sp.width / 3f, sp.height / 3f, 0);
                        }
                        go.name = "Pointing";
                        hasAdded = true;
                        // 			TweenPosition tween = go.GetComponent<TweenPosition>();
                        // 			if(tween!=null){
                        // 				tween.from = go.transform.localPosition;
                        // 				UnityEngine.Vector3 pos = go.transform.localPosition;
                        // 				tween.to = new UnityEngine.Vector3(pos.x,pos.y+5,0);
                        // 			tween.enabled = true;
                        // 			}
                    }
                }
            }
        }
        else
        {
            //UnityEngine.GameObject goFather = GetBtnGameObject("Entrance-Skill");
            if (pointingParent != null)
            {
                RemoveBtsPointing("Entrance-Skill");
                //UnityEngine.Transform tween = pointingParent.transform.Find("Entrance-Skill");
                //if (tween != null) NGUITools.Destroy(tween.gameObject);
            }
        }
    }

    private UnityEngine.GameObject GetBtnGameObject(string name)
    {
        for (int index = 0; index < buttons.Length; ++index)
        {
            if (buttons[index] != null && buttons[index].name == name)
                return buttons[index];
        }
        return null;
    }
    /**获得新物品*/
    private UnityEngine.GameObject getNewThings;
    void OnGetNewThings(NewThings newThings)
    {
        getNewThings = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/Common/GetNewThings"));
        //UnityEngine.Transform panel = transform.Find("UIPanel_1"); //UIAnchor-Center
        getNewThings = NGUITools.AddChild(gameObject, getNewThings);
        GetNewThings getNewScript = getNewThings.GetComponent<GetNewThings>();
        getNewScript.InitPanel(newThings);
    }

    private void OnMatchStateChange(MatchingType type, bool isShow)
    {
        string btnName = "";
        switch (type)
        {
            case MatchingType.Gold:
            case MatchingType.Trial:
                btnName = "Entrance-Trial";
                break;
            case MatchingType.PVP:
                btnName = "Entrance-Match";
                break;

        }
        for (int index = 0; index < buttons.Length; ++index)
        {
            if (buttons[index] != null && buttons[index].name == btnName)
            {
                MatchStateChange item = buttons[index].GetComponent<MatchStateChange>();
                if (item != null)
                {
                    item.SetState(isShow);
                    break;
                }
            }
        }
    }
    bool CheckUnlockFunction(int level)
    {
        bool isHas = false;
        foreach (LevelLock info in LevelLockProvider.Instance.GetData().Values)
        {
            if ((info.m_Area == 1 || info.m_Area == 2) && info.m_Type == 1
                  && !rightButtonList.Contains(info) && info.m_Level <= level)
            {
                isHas = true;
                break;
            }
        }
        return isHas;
    }
    //是否解锁新功能
    private int btnDistance = 90;
    private int rightNewBtnNum = 0; // 右侧新增按钮数
    private int bottomNewBtnNum = 0;//底部新增按钮数
    void IsUnlockFunction(int level)
    {
        rightNewBtnNum = 0;
        bottomNewBtnNum = 0;
        foreach (LevelLock info in LevelLockProvider.Instance.GetData().Values)
        {
            if (info.m_Area == 1 && info.m_Type == 1 && !rightButtonList.Contains(info) && info.m_Level <= level)
            {
                rightButtonList.Add(info);
                rightNewBtnNum++;
                NewThings newThings = new NewThings();
                newThings.id = info.m_Id;
                newThings.type = GetNewThingsType.T_Function;
                newThings.tf = GetBtnGameObject(info.m_Name).transform;
                newThings.name = info.m_Name;
                newThingsList.Add(newThings);
            }
            if (info.m_Area == 2 && info.m_Type == 1 && !bottomButtonList.Contains(info) && info.m_Level <= level)
            {
                bottomButtonList.Add(info);
                bottomNewBtnNum++;
                NewThings newThings = new NewThings();
                newThings.id = info.m_Id;
                newThings.type = GetNewThingsType.T_Function;
                newThings.tf = GetBtnGameObject(info.m_Name).transform;
                newThings.name = info.m_Name;
                newThingsList.Add(newThings);
            }
        }
        int num = 0;
        if (rightNewBtnNum > 0)
        {
            num = LayoutRightBtns();
            for (int i = 1; i <= rightNewBtnNum; i++)
            {
                SetBtnaActive(rightButtonList[num - i].m_Name, false);
            }
            TweenBtns(RightBtn, new UnityEngine.Vector3(0f, num * btnDistance, 0f), rightNewBtnNum);
        }
        if (bottomNewBtnNum > 0)
        {
            num = LayoutBottmBtns();
            for (int i = 1; i <= bottomNewBtnNum; i++)
            {
                SetBtnaActive(bottomButtonList[num - i].m_Name, false);
            }
            TweenBtns(BottomBtn, new UnityEngine.Vector3(-num * btnDistance, 0f, 0f), bottomNewBtnNum + rightNewBtnNum);
        }
        time = 0f;
    }
    //设置按钮可见性
    void SetBtnaActive(string btn, bool isShow)
    {
        for (int index = 0; index < buttons.Length; ++index)
        {
            if (buttons[index] != null && btn == buttons[index].name)
                NGUITools.SetActive(buttons[index], isShow);
        }
    }
    //移动按钮组
    void TweenBtns(UnityEngine.GameObject goTweenContainer, UnityEngine.Vector3 vec, float time)
    {
        if (goTweenContainer != null)
        {
            TweenPosition tweenPos = TweenPosition.Begin(goTweenContainer, DurationFor * time, vec);
            if (tweenPos != null)
            {
                tweenPos.animationCurve = CurveOut;
            }
        }
    }
    //移动按钮组
    void TweenOutBtns(UnityEngine.GameObject goTweenContainer, UnityEngine.Vector3 vec, float time)
    {
        if (goTweenContainer != null)
        {
            TweenPosition tweenPos = TweenPosition.Begin(goTweenContainer, DurationOut * time, vec);
            if (tweenPos != null)
            {
                tweenPos.animationCurve = CurveOut;
            }
        }
    }
    void TweenRightBackBtns(UnityEngine.GameObject goTweenContainer, UnityEngine.Vector3 vec, float time)
    {
        if (goTweenContainer != null)
        {
            TweenPosition tweenPos = TweenPosition.Begin(goTweenContainer, DurationBack * time, vec);
            if (tweenPos != null)
            {
                tweenPos.animationCurve = CurveBack;
            }
        }
    }
    void TweenBottonBackBtns(UnityEngine.GameObject goTweenContainer, UnityEngine.Vector3 vec, float time)
    {
        if (goTweenContainer != null)
        {
            TweenPosition tweenPos = TweenPosition.Begin(goTweenContainer, DurationBack * time, vec);
            if (tweenPos != null)
            {
                tweenPos.animationCurve = CurveBack;
            }
        }
    }
    void TweenButtonRotation(UnityEngine.GameObject goTweenContainer, int rot, float time)
    {
        if (goTweenContainer != null)
        {
            TweenRotation tweenPos = TweenRotation.Begin(goTweenContainer, DurationBack * time, new UnityEngine.Quaternion());
            if (tweenPos != null)
            {
                tweenPos.animationCurve = CurveBack;
                tweenPos.to = new UnityEngine.Vector3(0f, 0f, rot);
            }
        }
    }
    void TweenButtonAlpha(UnityEngine.GameObject goTweenContainer, int end, float time)
    {
        if (goTweenContainer != null)
        {
            TweenAlpha tweenPos = TweenAlpha.Begin(goTweenContainer, DurationBack * time, end);
            if (tweenPos != null)
            {
                tweenPos.to = end;
                tweenPos.animationCurve = CurveAlpha;
            }
        }
    }
    //初始化解锁
    void InitLevelLock(int level)
    {
        foreach (LevelLock info in LevelLockProvider.Instance.GetData().Values)
        {
            if (info.m_Area == 1 && info.m_Type == 1 && info.m_Level <= level)
            {
                rightButtonList.Add(info);
            }
            if (info.m_Area == 2 && info.m_Type == 1 && info.m_Level <= level)
            {
                bottomButtonList.Add(info);
            }
        }

        rightButtonList.Sort(ButtonSort);
        bottomButtonList.Sort(ButtonSort);
        LayoutAll();
    }
    void LayoutAll()
    {
        isShowBtn = true;
        int num = 0;
        SetAllButttonUnactive();
        num = LayoutRightBtns();
        RightBtn.transform.localPosition = new UnityEngine.Vector3(0f, num * btnDistance, 0f);
        num = LayoutBottmBtns();
        BottomBtn.transform.localPosition = new UnityEngine.Vector3(0f - num * btnDistance, 0f, 0f);
    }
    int LayoutRightBtns()
    {
        int num = 0;
        for (int i = 0; i < rightButtonList.Count; i++)
        {
            num++;
            UnityEngine.GameObject button = GetBtnGameObject(rightButtonList[i].m_Name);
            if (null != button)
            {
                button.transform.localPosition = new UnityEngine.Vector3(405f, -260f - i * btnDistance, 0f);
                NGUITools.SetActive(button, true);
            }
        }
        return num;
    }
    int LayoutBottmBtns()
    {
        int num = 0;
        for (int j = 0; j < bottomButtonList.Count; j++)
        {
            num++;
            UnityEngine.GameObject button1 = GetBtnGameObject(bottomButtonList[j].m_Name);
            if (null != button1)
            {
                button1.transform.localPosition = new UnityEngine.Vector3(-60 + j * btnDistance, 54f, 0f);
                NGUITools.SetActive(button1, true);
            }
        }
        return num;
    }

    public void OnSystemSettingClicked()
    {
        //GfxSystem.SetEquipmentColor(LobbyClient.Instance.CurrentRole.HeroId, EquipmentType.E_Clothes, 
            //new UnityEngine.Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), 1.0f));
    }

    public void OnPVEClicked()
    {
        UIManager.Instance.ShowWindowByName("Trial");
    }

    public void OnNetworkSpeedupClicked()
    {
        bool isAlreadySpeedup = !DelayManager.IsDelayEnabled;
        Action<bool> fun_y = new Action<bool>(delegate (bool selected)
        {
            if (selected)
            {
                string info = "开启网络加速成功！";
                DelayManager.IsDelayEnabled = false;
                GfxSystem.PublishGfxEvent("ge_highlight_prompt", "ui", info);
            }
        });

        Action<bool> fun_n = new Action<bool>(delegate (bool selected)
        {
            if (selected)
            {
                DelayManager.IsDelayEnabled = true;
                string info = "关闭网络加速成功！";
                GfxSystem.PublishGfxEvent("ge_highlight_prompt", "ui", info);
            }
        });

        if (isAlreadySpeedup)
        {
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_yesornot", "ui", "关闭网络加速", "确认关闭", fun_n);
        }
        else
        {
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_yesornot", "ui", "开启网络加速", "确认开启", fun_y);
        }
        
    }

    public void OnPVPClicked()
    {
        UIManager.Instance.ShowWindowByName("PvPEntrance");
    }

    void SetAllButttonUnactive()
    {
        for (int index = 0; index < buttons.Length; ++index)
        {
            if (buttons[index] != null && /*buttons[index].name != "Entrance-Mission" && buttons[index].name != "Entrance-Shop" && buttons[index].name != "Entrance-Award"*/buttons[index].name != "Entrance-Award")
                NGUITools.SetActive(buttons[index], false);
        }
    }
    //按钮排序
    int ButtonSort(LevelLock button1, LevelLock button2)
    {
        if (button1.m_Order < button2.m_Order)
        {
            return -1;
        }
        else if (button1.m_Order > button2.m_Order)
        {
            return 1;
        }
        return -1;
    }
    private List<LevelLock> rightButtonList = new List<LevelLock>();//屏幕右侧按钮
    private List<LevelLock> bottomButtonList = new List<LevelLock>();//屏幕底按钮

    private void OnSystemNewTipStateChange(SystemNewTipType type, bool isShow)
    {
        string btnName = "";
        switch (type)
        {
            case SystemNewTipType.Equip_Upgrade:
            case SystemNewTipType.Equip_Tip:
                btnName = "Entrance-Equipment";
                btnName = "Entrance-Equipment";
                break;
            case SystemNewTipType.GodEquip:
                btnName = "Entrance-GodEquip";
                break;
            case SystemNewTipType.Mail:
                btnName = "Entrance-Mail";
                break;
            case SystemNewTipType.Partner:
                btnName = "Entrance-Partner";
                break;
            case SystemNewTipType.Xhun:
                btnName = "Entrance-XHun";
                break;
            case SystemNewTipType.Skill:
                btnName = "Entrance-Skill";
                break;
            case SystemNewTipType.Task:
                btnName = "Entrance-Mission";
                break;
            case SystemNewTipType.Award:
                btnName = "Entrance-Award";
                break;
            case SystemNewTipType.Activity:
                btnName = "Entrance-Trial";
                break;
            case SystemNewTipType.pvp:
                btnName = "Entrance-Match";
                break;
        }
        for (int index = 0; index < buttons.Length; ++index)
        {
            if (buttons[index] != null && buttons[index].name == btnName)
            {
                UnityEngine.Transform tf = buttons[index].transform.Find("Entrance-New");
                if (tf != null)
                {
                    NGUITools.SetActive(tf.gameObject, isShow);
                    break;
                }
            }
        }
    }
}
