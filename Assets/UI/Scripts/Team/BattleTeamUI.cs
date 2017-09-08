using ArkCrossEngine;
using System;
using System.Collections.Generic;
using ArkCrossEngine.Network;

public class BattleTeamUI : UnityEngine.MonoBehaviour
{
    /*事件list*/
    private List<object> eventList = new List<object>();
    public UISprite flag;
    // Use this for initialization
    void Awake()
    {
        InitTeamUI();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            UpdataMemberInfo();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // 初始化组队ui
    private UnityEngine.GameObject teamMemberHead1;
    private UnityEngine.GameObject teamMemberHead2;
    private UIProgressBar hpProgressBar1;
    private UIProgressBar mpProgressBar1;
    private UIProgressBar hpProgressBar2;
    private UIProgressBar mpProgressBar2;
    private GfxUserInfo userInfo1 = new GfxUserInfo();
    private GfxUserInfo userInfo2 = new GfxUserInfo();
    SharedGameObjectInfo share_info1;
    SharedGameObjectInfo share_info2;
    private RoleInfo roleInfo;
    void InitTeamUI()
    {
        AddSubscribe();
        teamMemberHead1 = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/Team/TeamMemberHead"));
        teamMemberHead2 = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/Team/TeamMemberHead"));
        UnityEngine.Transform tf = transform.Find("UIPanel_1/UIAnchor-TopLeft");
        teamMemberHead1 = NGUITools.AddChild(tf.gameObject, teamMemberHead1);
        teamMemberHead2 = NGUITools.AddChild(tf.gameObject, teamMemberHead2);
        teamMemberHead1.transform.localPosition = new UnityEngine.Vector3(70f, -203, 0f);
        teamMemberHead2.transform.localPosition = new UnityEngine.Vector3(70f, -303, 0f);
        NGUITools.SetActive(teamMemberHead1.gameObject, false);
        NGUITools.SetActive(teamMemberHead2.gameObject, false);

        hpProgressBar1 = teamMemberHead1.transform.Find("HeroView-HpBar").gameObject.GetComponent<UIProgressBar>();
        hpProgressBar2 = teamMemberHead2.transform.Find("HeroView-HpBar").gameObject.GetComponent<UIProgressBar>();
        hpProgressBar1.value = 1f;
        hpProgressBar2.value = 2f;
        roleInfo = LobbyClient.Instance.CurrentRole;
        SetMemberShow();

    }
    /*添加监听 subscribe*/
    void AddSubscribe()
    {
        object eo;
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<string, TeamOperateResult>("ge_leave_group_result", "group", LeaveTeamResult);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<GroupInfo>("ge_sync_group_info", "group", BackGroupInfo);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (null != eo)
        {
            eventList.Add(eo);
        }
    }
    /*组队信息返回*/
    void BackGroupInfo(GroupInfo groupInfo)
    {
        SetMemberShow();
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
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    /*离开队伍结果*/
    void LeaveTeamResult(string name, TeamOperateResult result)
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        switch (result)
        {
            case TeamOperateResult.OR_Notice:
                SendScreeTipCenter(580, name);
                SetMemberShow();
                break;
        }

    }
    //悬浮字中
    void SendScreeTipCenter(int id, string name)
    {
        string chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(id);
        chn_desc = string.Format(chn_desc, name);
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_screen_tip_invoke", "ui", chn_desc, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
    }
    /*发送对话框*/
    void SendDialog(int i_chn_desc, int i_confirm, params object[] insert_name)
    {
        string chn_desc = "";
        string confirm = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(i_confirm);
        chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(i_chn_desc);
        string str = chn_desc;
        if (insert_name != null)
        {
            str = string.Format(chn_desc, insert_name);
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", str, confirm, null, null, null, false);
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
                flag.transform.localPosition = new UnityEngine.Vector3(15f, -30f, 0);
            }
            else
            {
                flag.transform.localPosition = new UnityEngine.Vector3(30f, -170f, 0);
            }
            if (group.Count > 0)
            {
                NGUITools.SetActive(flag.gameObject, true);
            }
            else
            {
                NGUITools.SetActive(flag.gameObject, false);
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
                    foreach (GfxUserInfo userinfo in DFMUiRoot.GfxUserInfoListForUI)
                    {
                        if (userinfo.m_Nick == group.Members[i].Nick)
                        {
                            member1Set = true;
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
                    foreach (GfxUserInfo userinfo in DFMUiRoot.GfxUserInfoListForUI)
                    {
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
            share_info1 = LogicSystem.GetSharedGameObjectInfo(userInfo1.m_ActorId);
            share_info2 = LogicSystem.GetSharedGameObjectInfo(userInfo2.m_ActorId);
        }

    }
    //设置成员信息
    public UnityEngine.Color color1;
    public UnityEngine.Color color2;
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
                        us.color = color2;
                    }
                    else
                    {
                        us.color = color1;
                    }
                    Data_PlayerConfig cg = PlayerConfigProvider.Instance.GetPlayerConfigById(member.HeroId);
                    if (cg != null)
                    {
                        us.spriteName = cg.m_Portrait;
                    }
                }
            }
        }
    }
    //更新队员血量等
    void UpdataMemberInfo()
    {
        if (share_info1 == null && roleInfo != null && roleInfo.Group.Count > 0)
        {
            SetMemberShow();
        }
        if (null != share_info1)
        {
            UpdateHealthBar((int)share_info1.Blood, (int)share_info1.MaxBlood, hpProgressBar1);
        }
        if (null != share_info2)
        {
            UpdateHealthBar((int)share_info2.Blood, (int)share_info2.MaxBlood, hpProgressBar2);
        }
    }
    //更新血条
    void UpdateHealthBar(int curValue, int maxValue, UIProgressBar hpProgressBar)
    {
        if (maxValue <= 0 || curValue < 0)
            return;
        float value = curValue / (float)maxValue;
        if (null != hpProgressBar)
        {
            hpProgressBar.value = value;
        }
    }
    //掉线
    void Offline(UnityEngine.GameObject go, SharedGameObjectInfo share_info)
    {
        if (null != go && null != share_info)
        {
            if (share_info != null)
            {
                UnityEngine.Transform tf;
                tf = go.transform.Find("portrait");
                if (tf != null)
                {
                    UISprite us = transform.gameObject.GetComponent<UISprite>();
                    if (us != null)
                    {
                        if (share_info.IsDead)
                        {
                            us.color = color2;
                        }
                        else
                        {
                            us.color = color1;
                        }
                    }
                }
            }
        }
    }




}
