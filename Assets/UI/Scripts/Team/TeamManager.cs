using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ArkCrossEngine;
using ArkCrossEngine.Network;
public class TeamManager : UnityEngine.MonoBehaviour
{

    /*事件list*/
    private List<object> eventList = new List<object>();
    /*1:队伍信息， 2： 待确认列表*/
    public static int currTable = 1;
    /*队伍信息*/
    private Dictionary<UnityEngine.GameObject, GroupMemberInfo> teamDic = new Dictionary<UnityEngine.GameObject, GroupMemberInfo>();
    /*待确认信息*/
    private Dictionary<UnityEngine.GameObject, GroupMemberInfo> confirmDic = new Dictionary<UnityEngine.GameObject, GroupMemberInfo>();
    /*邀请人的名字*/
    private List<string> inviteName = new List<string>();
    // Use this for initialization
    private UnityEngine.GameObject pointing;
    public UISprite teamBtn = null;
    public UISprite listBtn = null;
    void Start()
    {
        try
        {
            AddSubscribe();
            UIManager.Instance.HideWindowByName("Team");
            AddPointing();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

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
                /*
                foreach (object eo in eventList) {
                  if (eo != null) {
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eo);
                  }
                }*/
            }
            DataClear();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    /*添加监听 subscribe*/
    void AddSubscribe()
    {
        object eo;
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<GroupInfo>("ge_sync_group_info", "group", BackGroupInfo);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<string, string>("ge_pinvite_team_message", "group", PinviteTeamHandle);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<UnityEngine.GameObject>("ge_click_reject_team", "team", RejectTeam);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<UnityEngine.GameObject>("ge_click_agree_team", "team", AgreeTeam);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<UnityEngine.GameObject>("ge_click_leave_team", "team", LeaveTeam);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<string, TeamOperateResult>("ge_confirm_join_group_result", "group", ConfirmJoinResult);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<string, TeamOperateResult>("ge_request_join_group_result", "group", RequestJoinResult);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<string, TeamOperateResult>("ge_leave_group_result", "group", LeaveTeamResult);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_change_captain", "group", ChangeCaptain);
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
    void ChangeCaptain()
    {
        try
        {
            SendScreeTipCenter(584);
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
            case TeamOperateResult.OR_Succeed:
                SendScreeTipCenter(571);
                role.Group.Reset();
                LogicSystem.EventChannelForGfx.Publish("ge_updata_member_info", "team");
                ClearItem();
                UIManager.Instance.HideWindowByName("Team");
                break;
            case TeamOperateResult.OR_Kickout:
                SendScreeTipCenter(572);
                role.Group.Reset();
                LogicSystem.EventChannelForGfx.Publish("ge_updata_member_info", "team");
                ClearItem();
                UIManager.Instance.HideWindowByName("Team");
                break;
            case TeamOperateResult.OR_Dismiss:
                SendScreeTipCenter(578);
                role.Group.Reset();
                ClearItem();
                LogicSystem.EventChannelForGfx.Publish("ge_updata_member_info", "team");
                UIManager.Instance.HideWindowByName("Team");
                break;
            case TeamOperateResult.OR_Notice:
                SendScreeTipCenter(580, name);
                break;
        }

    }
    /*确认加入结果*/
    void ConfirmJoinResult(string name, TeamOperateResult result)
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        switch (result)
        {
            case TeamOperateResult.OR_Succeed:
                if (role.Guid == role.Group.CreatorGuid)
                {
                    SendScreeTipCenter(570, name);
                }
                else
                {
                    SendScreeTipCenter(565, name);
                }
                break;
            case TeamOperateResult.OR_Busyness:
                SendScreeTipCenter(564);
                break;
            case TeamOperateResult.OR_Exists:
                SendScreeTipCenter(583);
                break;
            case TeamOperateResult.OR_Overflow:
                SendScreeTipCenter(567);
                break;
            case TeamOperateResult.OR_Unknown:
                SendScreeTipCenter(568);
                break;
            case TeamOperateResult.OR_Notice:
                SendScreeTipCenter(581, name);
                break;
            case TeamOperateResult.OR_OutDate:
                SendScreeTipCenter(582);
                break;
            case TeamOperateResult.OR_HasTeam:
                SendScreeTipCenter(569);
                break;
        }
    }
    /*s申请加入结果*/
    void RequestJoinResult(string name, TeamOperateResult result)
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        switch (result)
        {
            case TeamOperateResult.OR_Succeed:
                if (role.Guid == role.Group.CreatorGuid)
                {
                }
                break;
            case TeamOperateResult.OR_Busyness:
                SendScreeTipCenter(564);
                break;
            case TeamOperateResult.OR_Exists:
                SendScreeTipCenter(576);
                break;
            case TeamOperateResult.OR_Overflow:
                SendScreeTipCenter(567);
                break;
            case TeamOperateResult.OR_Unknown:
                SendScreeTipCenter(568);
                break;
            case TeamOperateResult.OR_HasTeam:
                SendScreeTipCenter(569);
                break;
        }
    }
    /*离开队伍*/
    void LeaveTeam(UnityEngine.GameObject go)
    {
        try
        {
            if (null != go)
            {
                RoleInfo role = LobbyClient.Instance.CurrentRole;
                if (null != role)
                {
                    string name = teamDic[go].Nick;
                    if (WorldSystem.Instance.WaitMatchSceneId > 0)
                    {
                        LogicSystem.PublishLogicEvent("ge_cancel_match", "lobby", (MatchSceneEnum)WorldSystem.Instance.WaitMatchSceneId);
                    }
                    LogicSystem.PublishLogicEvent("ge_quit_group", "lobby", name);
                    confirmDic.Remove(go);
                    NGUITools.DestroyImmediate(go);
                    if (role.Guid == teamDic[go].Guid)
                    {
                        role.Group.Reset();
                        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_updata_member_info", "team");
                    }
                    RefreshGrid();
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    /*同意组队*/
    void AgreeTeam(UnityEngine.GameObject go)
    {
        try
        {
            if (null != go)
            {
                RoleInfo role = LobbyClient.Instance.CurrentRole;
                if (null != role)
                {
                    if (role.Group.Count < 3)
                    {
                        ulong guid = confirmDic[go].Guid;
                        LogicSystem.PublishLogicEvent("ge_confirm_join_group", "lobby", role.Nickname, guid);
                        //confirmDic.Remove(go);
                        //NGUITools.DestroyImmediate(go);
                        //RefreshGrid();
                    }
                    else
                    {
                        SendScreeTipCenter(567);
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    /*拒绝组队*/
    void RejectTeam(UnityEngine.GameObject go)
    {
        try
        {
            if (null != go)
            {
                ulong guid = confirmDic[go].Guid;
                LogicSystem.PublishLogicEvent("ge_refuse_request", "lobby", guid);
                RoleInfo role = LobbyClient.Instance.CurrentRole;
                LogicSystem.PublishLogicEvent("ge_request_group_info", "lobby");
                //confirmDic.Remove(go);
                //NGUITools.DestroyImmediate(go);
                //RefreshGrid();
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    /*别人发来组队邀请*/
    private List<string> pinviteList = new List<string>();
    void PinviteTeamHandle(string sponsor, string leader)
    {
        try
        {
            if (sponsor == leader)
            {
                if (inviteName.Count > 0)
                {
                    if (inviteName.Contains(leader))
                    {
                        return;
                    }
                    else
                    {
                        inviteName.Add(leader);
                    }
                }
                else
                {
                    inviteName.Add(leader);
                    SendDialog(561, 4, 9, HandleDialog, leader);
                }
            }
            else
            {
                if (inviteName.Count > 0)
                {
                    if (inviteName.Contains(sponsor))
                    {
                        return;
                    }
                    else
                    {
                        inviteName.Add(sponsor);
                    }
                }
                else
                {
                    inviteName.Add(sponsor);
                    SendDialog(579, 4, 9, HandleDialog, sponsor);
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
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
    /*发送对话框*/
    void SendDialog(int i_chn_desc, int i_confirm, int i_cancel, ArkCrossEngine.MyAction<int> Func, params object[] insert_name)
    {
        string chn_desc = "";
        string confirm = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(i_confirm);
        string cancel = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(i_cancel);
        chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(i_chn_desc);
        string str = chn_desc;
        if (insert_name != null)
        {
            str = string.Format(chn_desc, insert_name);
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", str, null, confirm, cancel, Func, false);
    }
    //悬浮字中
    void SendScreeTipCenter(int id)
    {
        string chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(id);
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_screen_tip_invoke", "ui", chn_desc, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
    }
    //悬浮字中
    void SendScreeTipCenter(int id, string name)
    {
        string chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(id);
        chn_desc = string.Format(chn_desc, name);
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_screen_tip_invoke", "ui", chn_desc, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
    }
    /*确认加入队伍回调*/
    void HandleDialog(int action)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (action == 1)
        {
            if (inviteName.Count > 0)
            {
                ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_confirm_join_group", "lobby", inviteName[0], role_info.Guid);
                inviteName.Clear();
            }
        }
        else if (action == 2)
        {
            if (inviteName.Count > 0)
            {
                inviteName.RemoveAt(0);
                if (inviteName.Count > 0)
                {
                    SendDialog(561, 4, 9, HandleDialog, inviteName[0]);
                }
            }
        }
    }
    /*组队信息返回*/
    void BackGroupInfo(GroupInfo groupInfo)
    {
        try
        {
            if (null != groupInfo)
            {
                if (NeedRefreshNotice(groupInfo, confirmDic))
                {
                    NGUITools.SetActive(pointing, true);
                    LogicSystem.EventChannelForGfx.Publish("show_pointing_team_notice", "team", true);
                }
                switch (currTable)
                {
                    case 1:
                        RefreshTeamList(groupInfo);
                        break;
                    case 2:
                        RefreshConfirmList(groupInfo);
                        break;
                }
                RefrenshTabelIcon();
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_updata_member_info", "team");
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    /*刷新队伍列表*/
    void RefreshTeamList(GroupInfo groupInfo)
    {
        if (!NeedRefreshTeam(groupInfo, teamDic))
        {
            return;
        }
        ClearItem();
        if (groupInfo != null && groupInfo.Members.Count > 0)
        {
            groupInfo.Members.Sort(TeamListSort);
            for (var i = 0; i < groupInfo.Members.Count; i++)
            {
                AddItem(groupInfo.Members[i]);
            }
        }
        RefreshGrid();

    }
    /*队伍显示列表排序*/
    private int TeamListSort(GroupMemberInfo memberFirst, GroupMemberInfo memberSecond)
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        if (memberFirst.Guid == role.Group.CreatorGuid)
        {
            return -1;
        }
        else if (memberSecond.Guid == role.Group.CreatorGuid)
        {
            return 1;
        }
        else if (memberFirst.Guid == role.Guid)
        {
            return -1;
        }
        else if (memberSecond.Guid == role.Guid)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
    /*刷新待确认列表*/
    void RefreshConfirmList(GroupInfo groupInfo)
    {
        if (!NeedRefreshConfirm(groupInfo, confirmDic))
        {
            return;
        }
        ClearItem();
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        if (role.Guid == role.Group.CreatorGuid)
        {
            if (groupInfo != null && groupInfo.Confirms.Count > 0)
            {
                for (var i = 0; i < groupInfo.Confirms.Count; i++)
                {
                    if (groupInfo.Confirms[i].Guid == LobbyClient.Instance.CurrentRole.Guid)
                    {
                        continue;
                    }
                    AddItem(groupInfo.Confirms[i]);
                }
            }
            RefreshGrid();
        }
    }
    /*添加item*/
    void AddItem(GroupMemberInfo member)
    {
        if (null != member)
        {
            UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/Team/member"));
            ItemShow(member, go);
            if (null != go)
            {
                UnityEngine.Transform tf = gameObject.transform.Find("bc/sp_heikuang/ScrollView/Grid");
                if (null != tf)
                {
                    go = NGUITools.AddChild(tf.gameObject, go);
                    if (null != go)
                    {
                        switch (currTable)
                        {
                            case 1:
                                teamDic.Add(go, member);
                                break;
                            case 2:
                                confirmDic.Add(go, member);
                                break;
                        }
                        SetItemInfo(go, member);
                    }
                }
            }
        }
    }
    /*item显示状态*/
    void ItemShow(GroupMemberInfo member, UnityEngine.GameObject go)
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        if (member.Guid == role.Group.CreatorGuid)
        {
            HideOrShowCompent(go, "leader", true);
        }
        else
        {
            HideOrShowCompent(go, "leader", false);
        }
        switch (currTable)
        {
            case 2:
                HideOrShowCompent(go, "No", true);
                HideOrShowCompent(go, "Yes", true);
                HideOrShowCompent(go, "leave", false);
                break;
            case 1:
                HideOrShowCompent(go, "No", false);
                HideOrShowCompent(go, "Yes", false);

                UnityEngine.Transform transform;

                transform = go.transform.Find("leave/Label");
                if (null != transform)
                {
                    UILabel uiLable = transform.gameObject.GetComponent<UILabel>();
                    if (null != uiLable)
                    {
                        if (member.Guid == role.Guid)
                        {
                            HideOrShowCompent(go, "leave", true);
                            uiLable.text = StrDictionaryProvider.Instance.GetDictString(562);
                        }
                        else if (role.Guid == role.Group.CreatorGuid)
                        {
                            HideOrShowCompent(go, "leave", true);
                            uiLable.text = StrDictionaryProvider.Instance.GetDictString(563);
                        }
                        else
                        {
                            HideOrShowCompent(go, "leave", false);
                        }
                    }
                }

                break;
        }
    }
    /*隐藏 或者显示组件*/
    void HideOrShowCompent(UnityEngine.GameObject go, string name, bool show)
    {
        UnityEngine.Transform tf;
        tf = go.transform.Find(name);
        if (null != tf)
        {
            NGUITools.SetActive(tf.gameObject, show);
        }
    }
    /*清除item*/
    void ClearItem()
    {
        foreach (UnityEngine.GameObject go in teamDic.Keys)
        {
            NGUITools.DestroyImmediate(go);
        }
        foreach (UnityEngine.GameObject go in confirmDic.Keys)
        {
            NGUITools.DestroyImmediate(go);
        }
        teamDic.Clear();
        confirmDic.Clear();
    }
    /*设置item信息*/
    void SetItemInfo(UnityEngine.GameObject go, GroupMemberInfo info)
    {
        if (null != go && null != info)
        {
            UnityEngine.Transform transform;
            transform = go.transform.Find("name");
            if (null != transform)
            {
                UILabel uiLable = transform.gameObject.GetComponent<UILabel>();
                if (null != uiLable)
                {
                    uiLable.text = info.Nick;
                }
            }
            transform = go.transform.Find("lv");
            if (null != transform)
            {
                UILabel uiLable = transform.gameObject.GetComponent<UILabel>();
                if (null != uiLable)
                {
                    uiLable.text = "LV " + info.Level.ToString();
                }
            }
            transform = go.transform.Find("Sprite");
            if (transform != null)
            {
                UISprite us = transform.gameObject.GetComponent<UISprite>();
                if (us != null)
                {
                    Data_PlayerConfig cg = PlayerConfigProvider.Instance.GetPlayerConfigById(info.HeroId);
                    us.spriteName = cg.m_PortraitForCell;
                }
            }
            transform = go.transform.Find("zhanli");
            if (null != transform)
            {
                UILabel uiLable = transform.gameObject.GetComponent<UILabel>();
                if (null != uiLable)
                {
                    uiLable.text = info.FightingScore.ToString();
                }
            }

        }
    }
    /*清楚数据*/
    void DataClear()
    {
    }
    /*关闭面板*/
    public void ClosePanel()
    {
        UIManager.Instance.HideWindowByName("Team");
    }
    /*点击成员按钮*/
    public void MemberListBtn()
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        currTable = 1;
        RefreshTeamList(role.Group);
        RefrenshTabelIcon();
    }
    /*点击待确认列表按钮*/
    public void ConfirmListBtn()
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        currTable = 2;
        NGUITools.SetActive(pointing, false);
        LogicSystem.EventChannelForGfx.Publish("show_pointing_team_notice", "team", false);
        RefreshConfirmList(role.Group);
        RefrenshTabelIcon();
    }
    //刷新tabel键图片
    void RefrenshTabelIcon()
    {
        UIButton bt;
        switch (currTable)
        {
            case 1:
                teamBtn.spriteName = "biao-qian-an-niu2";
                bt = teamBtn.GetComponent<UIButton>();
                bt.normalSprite = "biao-qian-an-niu2";
                listBtn.spriteName = "biao-qian-an-niu1";
                bt = listBtn.GetComponent<UIButton>();
                bt.normalSprite = "biao-qian-an-niu1";
                break;
            case 2:
                teamBtn.spriteName = "biao-qian-an-niu1";
                bt = teamBtn.GetComponent<UIButton>();
                bt.normalSprite = "biao-qian-an-niu1";
                listBtn.spriteName = "biao-qian-an-niu2";
                bt = listBtn.GetComponent<UIButton>();
                bt.normalSprite = "biao-qian-an-niu2";
                break;
        }
    }
    /*刷新 grid*/
    void RefreshGrid()
    {
        UnityEngine.Transform tf = gameObject.transform.Find("bc/sp_heikuang/ScrollView/Grid");
        if (tf != null)
        {
            UIGrid ug = tf.gameObject.GetComponent<UIGrid>();
            if (ug != null)
            {
                ug.repositionNow = true;
            }
        }
    }
    /*是否需要刷新， true ：刷新*/
    bool NeedRefreshConfirm(GroupInfo groupInfo, Dictionary<UnityEngine.GameObject, GroupMemberInfo> dic)
    {
        bool refresh = false;
        if (groupInfo.Confirms.Count == 0 || groupInfo.Confirms.Count < dic.Count)
            return true;
        foreach (GroupMemberInfo member in groupInfo.Confirms)
        {
            refresh = true;
            foreach (UnityEngine.GameObject go in dic.Keys)
            {
                if (dic[go].Guid == member.Guid)
                {
                    refresh = false;
                }
            }
            if (refresh)
            {
                return refresh;
            }
        }
        return refresh;
    }
    /*是否需要刷新， true ：刷新*/
    bool NeedRefreshTeam(GroupInfo groupInfo, Dictionary<UnityEngine.GameObject, GroupMemberInfo> dic)
    {
        bool refresh = false;
        if (groupInfo.Members.Count == 0 || groupInfo.Members.Count != dic.Count)
            return true;
        foreach (GroupMemberInfo member in groupInfo.Members)
        {
            refresh = true;
            foreach (UnityEngine.GameObject go in dic.Keys)
            {
                if (dic[go].Guid == member.Guid)
                {
                    refresh = false;
                }
            }
            if (refresh)
            {
                return refresh;
            }
        }

        return refresh;
    }
    /*是否需要刷新， true ：刷新*/
    bool NeedRefreshNotice(GroupInfo groupInfo, Dictionary<UnityEngine.GameObject, GroupMemberInfo> dic)
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        bool refresh = false;
        if (role != null && role.Guid != groupInfo.CreatorGuid)
        {
            return false;
        }
        if (groupInfo.Confirms.Count == 0)
        {
            refresh = false;
            NGUITools.SetActive(pointing, false);
            LogicSystem.EventChannelForGfx.Publish("show_pointing_team_notice", "team", false);
            return refresh;
        }
        foreach (GroupMemberInfo member in groupInfo.Confirms)
        {
            refresh = true;
            foreach (UnityEngine.GameObject go in dic.Keys)
            {
                if (dic[go].Guid == member.Guid)
                {
                    refresh = false;
                }
            }
            if (refresh)
            {
                return refresh;
            }
        }

        return refresh;
    }
    void AddPointing()
    {
        string path = UIManager.Instance.GetPathByName("Pointing");
        pointing = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.GetSharedResource(path));
        if (null != pointing)
        {
            UnityEngine.Transform tf = gameObject.transform.Find("bc/sp_heikuang/point");
            if (tf != null)
            {
                pointing = NGUITools.AddChild(tf.gameObject, pointing);
                UISprite sp = tf.GetComponent<UISprite>();
                if (sp != null)
                {
                    pointing.transform.localPosition = new UnityEngine.Vector3(sp.width / 3f, sp.height / 3f, 0);
                }
                pointing.name = "Pointing";
            }
            NGUITools.SetActive(pointing, false);
        }
    }
}
