using System;
using System.Text;
using ArkCrossEngine;
using System.Collections.Generic;
public class UIPlayerInfo : UnityEngine.MonoBehaviour
{

    public UILabel level = null;
    public UILabel NickName = null;
    public UILabel MainCity = null;
    //战斗力
    public UILabel FightingValue = null;
    //体力
    public UILabel lblStamina = null;
    //头像
    public UISprite portrait = null;
    //经验条
    public UIProgressBar progressBar = null;
    public bool m_IsInitialized = false;
    //队伍
    public UnityEngine.Transform team = null;
    private int preLevel = -1;
    private int preFighting = -1;
    private int preCurStamina = -1;
    private int preMaxStamina = -1;
    private UnityEngine.GameObject teamMemberHead1;
    private UnityEngine.GameObject teamMemberHead2;
    /*事件list*/
    private List<object> eventList = new List<object>();
    private UnityEngine.GameObject pointing; // 组队提示
    private bool hasNotice = false;
    public UnityEngine.Color color1;
    public UnityEngine.Color color2;
    private UIProgressBar hpProgressBar1;
    private UIProgressBar mpProgressBar1;
    private UIProgressBar hpProgressBar2;
    private UIProgressBar mpProgressBar2;
    private GfxUserInfo userInfo1 = new GfxUserInfo();
    private GfxUserInfo userInfo2 = new GfxUserInfo();
    private SharedGameObjectInfo share_info1;
    private SharedGameObjectInfo share_info2;
    private RoleInfo roleInfo;
    public UISprite myFlag;
    public UnityEngine.Transform aMemberFlag;
    // Use this for initialization
    void Start()
    {
        try
        {
            AddSubscribe();
            AddPointing();
            teamMemberHead1 = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/Team/TeamMemberHead"));
            teamMemberHead2 = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/Team/TeamMemberHead"));
            UnityEngine.Transform tf = transform.Find("UIPanel_1/UIAnchor-TopLeft");
            teamMemberHead1 = NGUITools.AddChild(tf.gameObject, teamMemberHead1);
            aMemberFlag = teamMemberHead1.transform.Find("leader");
            teamMemberHead2 = NGUITools.AddChild(tf.gameObject, teamMemberHead2);
            teamMemberHead1.transform.localPosition = new UnityEngine.Vector3(70f, -183, 0f);
            teamMemberHead2.transform.localPosition = new UnityEngine.Vector3(70f, -283, 0f);
            NGUITools.SetActive(teamMemberHead1.gameObject, false);
            NGUITools.SetActive(teamMemberHead2.gameObject, false);
            hpProgressBar1 = teamMemberHead1.transform.Find("HeroView-HpBar").gameObject.GetComponent<UIProgressBar>();
            hpProgressBar2 = teamMemberHead2.transform.Find("HeroView-HpBar").gameObject.GetComponent<UIProgressBar>();
            hpProgressBar1.value = 1f;
            hpProgressBar2.value = 1f;
            roleInfo = LobbyClient.Instance.CurrentRole;
            if (roleInfo.Group.Count > 0)
            {
                SetMemberShow();
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    /*移除panel的Subscribe*/
    public void UnSubscribe()
    {
        try
        {
            if (null != eventList)
            {
                for (int i = 0; i < eventList.Count; i++)
                {
                    if (eventList[i] != null)
                    {
                        ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eventList[i]);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    /*添加监听 subscribe*/
    void AddSubscribe()
    {
        object eo;
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<bool>("show_pointing_team_notice", "team", TeamNotice);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_updata_member_info", "team", SetMemberShow);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<GroupInfo>("ge_sync_group_info", "group", BackGroupInfo);
        if (null != eo)
        {
            eventList.Add(eo);
        }
    }
    /*添加组队提示*/
    void AddPointing()
    {
        string path = UIManager.Instance.GetPathByName("Pointing");
        pointing = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.GetSharedResource(path));
        if (null != pointing)
        {
            if (team != null)
            {
                pointing = NGUITools.AddChild(team.gameObject, pointing);
                UISprite sp = team.GetComponent<UISprite>();
                if (sp != null)
                {
                    pointing.transform.localPosition = new UnityEngine.Vector3(sp.width / 3f + sp.height / 3f, 0);
                }
                pointing.name = "Pointing";
            }
            NGUITools.SetActive(pointing, false);
        }
    }
    /*组队信息返回*/
    void BackGroupInfo(GroupInfo groupInfo)
    {
        SetMemberShow();
    }
    /*组队提示设置可见*/
    void TeamNotice(bool show)
    {
        hasNotice = show;
        NGUITools.SetActive(pointing, show);
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            if (role_info != null)
            {
                //UpdateEx(role_info.Level, role_info.Exp);
                SetPlayerLevel(role_info.Level);
                SetFighting((int)role_info.FightingScore);
                if (preCurStamina != role_info.CurStamina || preMaxStamina != role_info.StaminaMax)
                {
                    preCurStamina = role_info.CurStamina;
                    StringBuilder sbuild = new StringBuilder();
                    sbuild.Append(role_info.CurStamina + "/" + role_info.StaminaMax);
                    if (lblStamina != null) lblStamina.text = sbuild.ToString();
                }
                if (!m_IsInitialized)
                {
                    Data_PlayerConfig playerData = PlayerConfigProvider.Instance.GetPlayerConfigById(role_info.HeroId);
                    if (playerData != null)
                    {
                        m_IsInitialized = true;
                        SetPortrait(playerData.m_Portrait);
                        SetNickname(role_info.Nickname);
                    }
                }
            }
            UpdatePerTime(RealTime.deltaTime);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // 定时更新
    private float time;
    void UpdatePerTime(float delta)
    {
        time += delta;
        if (time > 0.7f)
        {
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            if (role_info != null)
            {
                UpdataTeam(role_info.Group);
            }
            time = 0f;
        }
    }
    //更新经验值
    public void UpdateEx(int level, int exp)
    {
        int curent = 0, max = 0;
        int baseExp = 0;
        if (level == 1)
        {
            baseExp = 0;
        }
        else
        {
            PlayerLevelupExpConfig expCfg = PlayerConfigProvider.Instance.GetPlayerLevelupExpConfigById(level - 1);
            if (expCfg != null)
                baseExp = expCfg.m_ConsumeExp;
        }
        PlayerLevelupExpConfig expCfgHith = PlayerConfigProvider.Instance.GetPlayerLevelupExpConfigById(level);
        if (expCfgHith != null) max = expCfgHith.m_ConsumeExp - baseExp;
        curent = exp - baseExp;
        if (progressBar != null && max != 0)
        {
            progressBar.value = curent / (float)max;
        }
    }
    //设置战斗力
    public void SetFighting(int value)
    {
        if (preFighting != value)
        {
            preFighting = value;
            if (FightingValue != null)
            {
                FightingValue.text = value.ToString();
            }
        }
    }
    public void SetNickname(string name)
    {
        if (NickName != null)
        {
            NickName.text = name;
        }
    }
    public void SetCityName(string name)
    {
        if (MainCity != null)
        {
            MainCity.text = name;
        }
    }
    public void SetPlayerLevel(int playerLevel)
    {
        if (preLevel != playerLevel)
        {
            preLevel = playerLevel;
            if (level != null)
            {
                level.text = "Lv." + playerLevel.ToString();
            }
        }
    }
    public void SetPortrait(string name)
    {
        if (portrait != null)
        {
            portrait.spriteName = name;
            UIButton btn = portrait.GetComponent<UIButton>();
            if (btn != null)
                btn.normalSprite = name;
        }
    }
    public void OnPortraitClick()
    {
        UIManager.Instance.ShowWindowByName("GamePokey");
    }
    //购买体力
    public void OnBuyStamina()
    {
        UIManager.Instance.ShowWindowByName("TiliBuy");
    }
    public void ButtonShowOption()
    {
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("Option");
        if (go != null)
        {
            if (NGUITools.GetActive(go))
            {
                UIManager.Instance.HideWindowByName("Option");
            }
            else
            {
                UIManager.Instance.ShowWindowByName("Option");
            }
        }
    }
    /*队伍信息*/
    public void ClickTeam()
    {
        if (hasNotice)
        {
            TeamManager.currTable = 2;
        }
        else
        {
            TeamManager.currTable = 1;
        }
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        LogicSystem.PublishLogicEvent("ge_request_group_info", "lobby");
        UIManager.Instance.ShowWindowByName("Team");
    }
    /*更新队伍信息*/
    void UpdataTeam(GroupInfo group)
    {
        if (share_info1 == null && roleInfo != null && roleInfo.Group.Count > 0)
        {
            SetMemberShow();
        }
        if (group != null)
        {
            if (group.Count > 0)
            {
                NGUITools.SetActive(team.gameObject, true);
            }
            else
            {
                NGUITools.SetActive(team.gameObject, false);
                NGUITools.SetActive(teamMemberHead1.gameObject, false);
                NGUITools.SetActive(teamMemberHead2.gameObject, false);
            }
        }
    }
    //设置旗子的可见性 0:自己， 1：第一个队员，
    void SetFlagActive(int index)
    {
        switch (index)
        {
            case 0:
                NGUITools.SetActive(myFlag.gameObject, true);
                NGUITools.SetActive(aMemberFlag.gameObject, false);
                break;
            case 1:
                NGUITools.SetActive(myFlag.gameObject, false);
                NGUITools.SetActive(aMemberFlag.gameObject, true);
                break;
        }
    }
    //设置成员信息
    void SetMemberShow()
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        GroupInfo group = role.Group;
        if (group != null && role != null)
        {
            if (role.Guid == group.CreatorGuid)
            {
                SetFlagActive(0);
            }
            else
            {
                SetFlagActive(1);
            }
            if (group.Count == 2)
            { //队伍中人=1
                NGUITools.SetActive(teamMemberHead1.gameObject, true);
                NGUITools.SetActive(teamMemberHead2.gameObject, false);

            }
            else if (group.Count == 3)
            { //队伍中人=2
                NGUITools.SetActive(teamMemberHead1.gameObject, true);
                NGUITools.SetActive(teamMemberHead2.gameObject, true);
            }
            else
            {
                NGUITools.SetActive(teamMemberHead1.gameObject, false);
                NGUITools.SetActive(teamMemberHead2.gameObject, false);
            }
            bool member1Set = false;
            for (var i = 0; i < group.Members.Count; i++)
            {
                if (role.Guid != group.Members[i].Guid && !member1Set)
                {
                    SetMemberInfo(group.Members[i], teamMemberHead1);
                    member1Set = true;
                    for (int j = 0; j < DFMUiRoot.GfxUserInfoListForUI.Count; j++)
                    {
                        GfxUserInfo userinfo = DFMUiRoot.GfxUserInfoListForUI[j];
                        if (userinfo.m_Nick == group.Members[i].Nick)
                        {
                            userInfo1.m_ActorId = userinfo.m_ActorId;
                            userInfo1.m_Nick = userinfo.m_Nick;
                            userInfo1.m_Level = userinfo.m_Level;
                            userInfo1.m_HeroId = userinfo.m_HeroId;
                        }
                    }
                }
                if (role.Guid != group.Members[i].Guid && member1Set)
                {
                    SetMemberInfo(group.Members[i], teamMemberHead2);
                    for (int j = 0; j < DFMUiRoot.GfxUserInfoListForUI.Count; j++)
                    {
                        GfxUserInfo userinfo = DFMUiRoot.GfxUserInfoListForUI[j];
                        if (userinfo.m_Nick == group.Members[i].Nick)
                        {
                            userInfo2.m_ActorId = userinfo.m_ActorId;
                            userInfo2.m_Nick = userinfo.m_Nick;
                            userInfo2.m_Level = userinfo.m_Level;
                            userInfo2.m_HeroId = userinfo.m_HeroId;
                        }
                    }
                }
            }
        }
        share_info1 = LogicSystem.GetSharedGameObjectInfo(userInfo1.m_ActorId);
        share_info2 = LogicSystem.GetSharedGameObjectInfo(userInfo2.m_ActorId);
    }

    //设置成员信息
    void SetMemberInfo(GroupMemberInfo member, UnityEngine.GameObject go)
    {
        if (null != go && null != member)
        {
            UnityEngine.Transform transform;
            transform = go.transform.Find("name");
            if (null != transform)
            {
                UILabel uiLable = transform.gameObject.GetComponent<UILabel>();
                if (null != uiLable)
                {
                    uiLable.text = member.Nick;
                }
            }
            transform = go.transform.Find("level_label");
            if (null != transform)
            {
                UILabel uiLable = transform.gameObject.GetComponent<UILabel>();
                if (null != uiLable)
                {
                    uiLable.text = "Lv." + member.Level;
                }
            }
            transform = go.transform.Find("portrait");
            if (transform != null)
            {
                UISprite us = transform.gameObject.GetComponent<UISprite>();
                if (us != null)
                {
                    if (member.Status == UserState.DropOrOffline)
                    {
                        us.color = color1;
                    }
                    else
                    {
                        us.color = color2;
                    }
                    Data_PlayerConfig cg = PlayerConfigProvider.Instance.GetPlayerConfigById(member.HeroId);
                    us.spriteName = cg.m_Portrait;
                }
            }
        }
    }
}
