using System;
using System.Collections.Generic;
using ArkCrossEngine;

public class FriendManage : UnityEngine.MonoBehaviour
{
    /*事件list*/
    private List<object> eventList = new List<object>();
    /*<好友item，好友信息>*/
    private Dictionary<UnityEngine.GameObject, ArkCrossEngine.FriendInfo> friendDic = new Dictionary<UnityEngine.GameObject, ArkCrossEngine.FriendInfo>();
    /*好友list*/
    private List<UnityEngine.GameObject> goList = new List<UnityEngine.GameObject>();
    /*当前要删除的按钮*/
    private UnityEngine.GameObject currentDeleteFriend;
    /*移除好友panel的Subscribe*/
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
                  if (null != eo) {
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eo);
                  }
                }*/
            }
            FriendDataClear();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    /*聊天*/
    public void FriendChat(UnityEngine.GameObject go)
    {
        try
        {
            string userId = string.Empty;
            if (friendDic.ContainsKey(go))
            {
                ArkCrossEngine.FriendInfo fInfo = friendDic[go];
                if (fInfo != null)
                {
                    userId = fInfo.Guid.ToString();
                }
            }
            if (!String.IsNullOrEmpty(userId))
            {
                //CYGTConnector.beginOneChatWithGameUserId(userId);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    /*组队*/
    public void Team(UnityEngine.GameObject go)
    {
        try
        {
            TinviteTeam(go);
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    /*邀请别人组队*/
    void TinviteTeam(UnityEngine.GameObject item)
    {
        ArkCrossEngine.RoleInfo roleInfo = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
        if (roleInfo.Group.Count == 0)
        {
            foreach (ArkCrossEngine.GroupMemberInfo member in roleInfo.Group.Members)
            {
                if (member.Guid == friendDic[item].Guid)
                {
                    SendScreeTipCenter(575, friendDic[item].Nickname);
                    return;
                }
            }
            SendScreeTipCenter(577, friendDic[item].Nickname);
            ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_pinvite_team", "lobby", roleInfo.Nickname, friendDic[item].Nickname);
        }
        else if (roleInfo.Guid == roleInfo.Group.CreatorGuid)
        {
            foreach (ArkCrossEngine.GroupMemberInfo member in roleInfo.Group.Members)
            {
                if (member.Guid == friendDic[item].Guid)
                {
                    SendScreeTipCenter(575, friendDic[item].Nickname);
                    return;
                }
            }
            SendScreeTipCenter(560, friendDic[item].Nickname);
            ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_pinvite_team", "lobby", roleInfo.Nickname, friendDic[item].Nickname);
        }
        else
        {
            SendScreeTipCenter(574);
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

    // Use this for initialization
    void Start()
    {
        try
        {
            FriendDataClear();
            AddFriendSubscribe();
            //ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_request_friends", "lobby");
            RefreshFriendNum(0 + "/" + 40);
            UIManager.Instance.HideWindowByName("Friend");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    /*清除字典数据*/
    void FriendDataClear()
    {
        if (null != eventList)
        {
            eventList.Clear();
        }
        if (null != friendDic)
        {
            friendDic.Clear();
        }
        if (null != goList)
        {
            goList.Clear();
        }
    }
    /*添加监听 好友subscribe*/
    void AddFriendSubscribe()
    {
        object eo;
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_sync_friend_list", "friend", RequestClientFriends);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<Dictionary<ulong, ArkCrossEngine.FriendInfo>>("ge_client_friends"
                                                                                                   , "friend", RequestFriends);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<ulong, string, ArkCrossEngine.Network.AddFriendResult>("ge_add_friend"
                                                                                                    , "friend", NewFriend);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<ulong, ArkCrossEngine.Network.DelFriendResult>("ge_del_friend", "friend", DeleteFriend);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<UnityEngine.GameObject>("ge_drag_Item_friend", "friend", DragItem);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<UnityEngine.GameObject>("ge_click_Item_friend", "friend", ClickItem);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<UnityEngine.GameObject>("ge_click_Delete_friend", "friend", ClickDelete);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<ulong>("ge_friend_online", "friend", Online);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<ulong>("ge_friend_offline", "friend", Offline);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<UnityEngine.GameObject>("ge_click_Team_friend", "friend", Team);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<UnityEngine.GameObject>("ge_click_chat_friend", "friend", FriendChat);
        if (null != eo)
        {
            eventList.Add(eo);
        }
    }
    /**好友上线*/
    void Online(ulong guid)
    {
        try
        {
            ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_request_client_friends", "ui");
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }

    }
    /**好友下线*/
    void Offline(ulong guid)
    {
        try
        {
            ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_request_client_friends", "ui");
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }

    }
    /*好友插入排序*/
    List<ArkCrossEngine.FriendInfo> SortFriend(Dictionary<ulong, ArkCrossEngine.FriendInfo> friendDic)
    {
        List<ArkCrossEngine.FriendInfo> friendList = new List<ArkCrossEngine.FriendInfo>();
        foreach (ulong guid in friendDic.Keys)
        {
            friendList.Add(friendDic[guid]);
        }
        friendList.Sort(FriendListComppare);
        return friendList;
    }
    /*friendItem发过来的 drag好友*/
    void DragItem(UnityEngine.GameObject item)
    {
        try
        {
            foreach (UnityEngine.GameObject go in friendDic.Keys)
            {
                if (go == item)
                {
                    HideCompent(go, "liaotian");
                    HideCompent(go, "zudui");
                    ShowCompent(go, "AddFriend");
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }

    }
    /*隐藏 按钮*/
    void HideCompent(UnityEngine.GameObject go, string name)
    {
        UnityEngine.Transform tf;
        tf = go.transform.Find(name);
        if (null != tf)
        {
            NGUITools.SetActive(tf.gameObject, false);
        }
    }
    /*显示 按钮*/
    void ShowCompent(UnityEngine.GameObject go, string name)
    {
        UnityEngine.Transform tf;
        tf = go.transform.Find(name);
        if (null != tf)
        {
            NGUITools.SetActive(tf.gameObject, true);
        }
    }
    /*friendItem发过来的 点击好友*/
    void ClickItem(UnityEngine.GameObject item)
    {
        try
        {
            foreach (UnityEngine.GameObject go in friendDic.Keys)
            {
                if (friendDic[go].IsOnline)
                {
                    ShowCompent(go, "liaotian");
                    ShowCompent(go, "zudui");
                }
                HideCompent(go, "AddFriend");
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }

    }
    /*friendItem发过来的 删除好友*/
    void ClickDelete(UnityEngine.GameObject item)
    {
        try
        {
            foreach (UnityEngine.GameObject go in friendDic.Keys)
            {
                if (go == item)
                {
                    ArkCrossEngine.MyAction<int> Func = HandleDialog;
                    currentDeleteFriend = go;

                    string chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(559);
                    string cancel = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(9);
                    string confirm = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(4);
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", chn_desc, null, cancel, confirm, Func, false);

                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }

    }
    /*删除好友回调*/
    void HandleDialog(int action)
    {
        if (action == 2)
        {
            ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_delete_friend", "lobby", friendDic[currentDeleteFriend].Guid);
        }
    }
    /*friendItem发过来的和好友 聊天*/
    void ClickChat(UnityEngine.GameObject item)
    {
        foreach (UnityEngine.GameObject go in friendDic.Keys)
        {
            if (go == item)
            {
                ArkCrossEngine.GfxSystem.PublishGfxEvent("ge_openCYGTSDKController", "gt");
            }
        }
    }
    /*新加好友*/
    void NewFriend(ulong guid, string name, ArkCrossEngine.Network.AddFriendResult result)
    {
        try
        {
            string chn_desc = "";
            string confirm = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(4);
            switch (result)
            {
                case ArkCrossEngine.Network.AddFriendResult.ADD_SUCCESS:
                    ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_request_client_friends", "ui");
                    SendScreeTipCenter(553);

                    //Note:通知rc
//                     CYGTConnector.addFriendSuccessWithFriendUserId(guid.ToString());
//                     string msgContent = string.Format("你与[{0}({1})]成为好友", name, guid);
//                     CYGTConnector.sendSpecialMsgWithTitle("系统", msgContent, "#c93d47");
                    break;
                case ArkCrossEngine.Network.AddFriendResult.ADD_NOTICE:
                    chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(554);
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", name + chn_desc, confirm, null, null, null, false);
                    break;
                case ArkCrossEngine.Network.AddFriendResult.ADD_OVERFLOW:
                    SendScreeTipCenter(555);

                    //Note:通知rc
                    //CYGTConnector.sendSpecialMsgWithTitle("系统", Dict.Get(555), "#c93d47");
                    break;
                case ArkCrossEngine.Network.AddFriendResult.ADD_OWN_ERROR:
                    SendScreeTipCenter(556);

                    //Note:通知rc
                    //CYGTConnector.sendSpecialMsgWithTitle("系统", Dict.Get(556), "#c93d47");
                    break;
                case ArkCrossEngine.Network.AddFriendResult.ADD_NONENTITY_ERROR:
                    SendScreeTipCenter(557);

                    //Note:通知rc
                    //CYGTConnector.sendSpecialMsgWithTitle("系统", Dict.Get(557), "#c93d47");
                    break;
                case ArkCrossEngine.Network.AddFriendResult.ERROR:
                case ArkCrossEngine.Network.AddFriendResult.ADD_PLAYERSELF_ERROR:
                    chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(558);
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", chn_desc, confirm, null, null, null, false);

                    //Note:通知rc
                    //CYGTConnector.sendSpecialMsgWithTitle("系统", Dict.Get(558), "#c93d47");
                    break;

            }

            RefreshFriendNum(friendDic.Count + "/" + 40);

            UnityEngine.Transform tf = gameObject.transform.Find("MetalFrame/Container/ScrollView/Grid");
            if (tf != null)
            {
                UIGrid ug = tf.gameObject.GetComponent<UIGrid>();
                if (ug != null)
                {
                    ug.repositionNow = true;
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }

    }
    //悬浮字中
    void SendScreeTipCenter(int id)
    {
        string chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(id);
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
    }
    //悬浮字中
    void SendScreeTipCenter(int id, string name)
    {
        string chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(id);
        chn_desc = string.Format(chn_desc, name);
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
    }
    /*删除好友 结果*/
    public void DeleteFriend(ulong guid, ArkCrossEngine.Network.DelFriendResult result)
    {
        try
        {
            switch (result)
            {
                case ArkCrossEngine.Network.DelFriendResult.DEL_NONENTITY_ERROR:
                case ArkCrossEngine.Network.DelFriendResult.ERROR:

                    break;
                case ArkCrossEngine.Network.DelFriendResult.DEL_SUCCESS:
                    if (null != currentDeleteFriend)
                    {
                        ConfirmDelete(currentDeleteFriend);
                    }
                    //CYGTConnector.cancelFriendSuccessWithFriendUserId(guid.ToString());
                    break;
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }

    }
    /*确定删除*/
    void ConfirmDelete(UnityEngine.GameObject friendGo)
    {
        friendDic.Remove(friendGo);
        NGUITools.DestroyImmediate(friendGo);
        RefreshFriendNum(friendDic.Count + "/" + 40);
        UnityEngine.Transform tf = gameObject.transform.Find("sp_heikuang/ScrollView/Grid");
        if (tf != null)
        {
            UIGrid ug = tf.gameObject.GetComponent<UIGrid>();
            if (ug != null)
            {
                ug.repositionNow = true;
            }
        }
    }
    /*清除好友item*/
    void ClearItem()
    {
        if (friendDic != null)
        {
            foreach (UnityEngine.GameObject go in friendDic.Keys)
            {
                NGUITools.DestroyImmediate(go);
            }
            if (friendDic.Count > 0)
                friendDic.Clear();
        }

    }
    void RequestClientFriends()
    {
        try
        {
            ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_request_client_friends", "ui");
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    /*请求好友*/
    void RequestFriends(Dictionary<ulong, ArkCrossEngine.FriendInfo> dic)
    {
        try
        {
            ClearItem();
            if (dic != null)
            {
                List<ArkCrossEngine.FriendInfo> friendList = new List<ArkCrossEngine.FriendInfo>();
                foreach (ArkCrossEngine.FriendInfo friendInfo in dic.Values)
                {
                    friendList.Add(friendInfo);
                }
                friendList.Sort(FriendListComppare);
                for (var i = 0; i < friendList.Count; i++)
                {
                    AddFriend(friendList[i]);
                }
                RefreshFriendNum(friendDic.Count + "/" + 40);
            }
            UnityEngine.Transform tf = gameObject.transform.Find("sp_heikuang/ScrollView/Grid");
            if (tf != null)
            {
                UIGrid ug = tf.gameObject.GetComponent<UIGrid>();
                if (ug != null)
                {
                    ug.repositionNow = true;
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    /*好友在线排序*/
    private int FriendListComppare(ArkCrossEngine.FriendInfo friendInfo1, ArkCrossEngine.FriendInfo friendInfo2)
    {
        int res = 0;
        if (friendInfo1.IsOnline == friendInfo2.IsOnline)
        {
            if (friendInfo1.FightingScore > friendInfo2.FightingScore)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        else if (friendInfo1.IsOnline)
        {
            return -1;
        }
        else if (friendInfo2.IsOnline)
        {
            return 1;
        }
        return res;

    }
    /*是否有此好友*/
    bool HaveThisFriend(ulong guid)
    {
        foreach (ArkCrossEngine.FriendInfo fi in friendDic.Values)
        {
            if (guid == fi.Guid)
            {
                return true;
            }
        }
        return false;
    }
    /*添加好友*/
    void AddFriend(ArkCrossEngine.FriendInfo friendInfo)
    {
        if (null != friendInfo)
        {
            UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/Friend/Friend"));
            UnityEngine.Transform tf = gameObject.transform.Find("sp_heikuang/ScrollView/Grid");
            if (null != tf)
            {
                go = NGUITools.AddChild(tf.gameObject, go);
                if (null != go)
                {
                    friendDic.Add(go, friendInfo);
                    SetFriendItemInfo(go, friendInfo);
                }
            }
            //玩家不在线
            if (null != go)
            {
                UnityEngine.Transform transform;
                UISprite us;
                if (!friendInfo.IsOnline)
                {

                    transform = go.transform.Find("DI2");
                    if (null != transform)
                    {
                        us = transform.gameObject.GetComponent<UISprite>();
                        if (null != us)
                        {
                            us.spriteName = "bg_2";
                        }
                    }
                    HideCompent(go, "liaotian");
                    HideCompent(go, "zudui");
                    HideCompent(go, "AddFriend");
                }
                else
                {
                    transform = go.transform.Find("DI2");
                    if (null != transform)
                    {
                        us = transform.gameObject.GetComponent<UISprite>();
                        if (null != us)
                        {
                            us.spriteName = "backgroud";
                        }
                    }
                    ShowCompent(go, "liaotian");
                    ShowCompent(go, "zudui");
                }
            }
        }
    }



    string TransformName(int sort)
    {
        string str = "";
        str = (100 + sort).ToString().Substring(1, 2);
        return name;
    }
    void TestTeam(UnityEngine.GameObject go)
    {
        //UnityEngine.Transform item ; 
        //item = go.transform.parent;

    }
    /*清除好友item信息*/
    void ClearItemIntroduce()
    {

    }
    /*设置好友item信息*/
    void SetFriendItemInfo(UnityEngine.GameObject go, ArkCrossEngine.FriendInfo friendInfo)
    {
        if (null != go && null != friendInfo)
        {
            UnityEngine.Transform transform;
            transform = go.transform.Find("name");
            if (null != transform)
            {
                UILabel uiLable = transform.gameObject.GetComponent<UILabel>();
                if (null != uiLable)
                {
                    uiLable.text = friendInfo.Nickname;
                }
            }
            transform = go.transform.Find("lv");
            if (null != transform)
            {
                UILabel uiLable = transform.gameObject.GetComponent<UILabel>();
                if (null != uiLable)
                {
                    uiLable.text = "Lv." + friendInfo.Level.ToString();
                }
            }
            transform = go.transform.Find("head");
            if (transform != null)
            {
                UISprite us = transform.gameObject.GetComponent<UISprite>();
                if (us != null)
                {
                    Data_PlayerConfig cg = PlayerConfigProvider.Instance.GetPlayerConfigById(friendInfo.HeroId);
                    us.spriteName = cg.m_PortraitForCell;
                }
            }
            transform = go.transform.Find("zhanli");
            if (null != transform)
            {
                UILabel uiLable = transform.gameObject.GetComponent<UILabel>();
                if (null != uiLable)
                {
                    uiLable.text = friendInfo.FightingScore.ToString();
                }
            }

        }
    }
    /*关闭好友面板*/
    public void CloseFriend()
    {
        ClearItem();
        UIManager.Instance.HideWindowByName("Friend");
    }
    /*刷新好友数量*/
    void RefreshFriendNum(string sNum)
    {
        UnityEngine.Transform tf;
        tf = transform.Find("shangxian/Label");
        if (null != tf)
        {
            UILabel uiLable = tf.gameObject.GetComponent<UILabel>();
            if (null != uiLable)
            {
                uiLable.text = sNum;
            }
        }
    }
    /*查找好友*/
    public void SearcherFriend()
    {
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("Verification");
        if (null != go)
        {
            go.GetComponent<UIVerification>().InitOpenType(UIVerification.OpenType.AddFriend);
        }
        UIManager.Instance.ShowWindowByName("Verification");
    }
}
