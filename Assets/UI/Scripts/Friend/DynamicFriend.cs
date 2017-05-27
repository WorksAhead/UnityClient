using System.Collections.Generic;
using ArkCrossEngine;

public class DynamicFriend : UnityEngine.MonoBehaviour
{

    /*关闭按钮*/
    public void CloseBtn()
    {
        UIManager.Instance.HideWindowByName("DynamicFriend");
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /*面板初始化*/
    public void InitPanel(GfxUserInfo userInfo, UnityEngine.Vector3 vec)
    {
        //UnityEngine.Vector3 pos = UnityEngine.Camera.main.WorldToScreenPoint(vec);
        //pos.z = 0;
        //pos = UICamera.mainCamera.ScreenToWorldPoint(pos);
        //transform.position = pos;
        SetHeadInfo(userInfo);
    }
    void ClickItem()
    {
    }
    /*添加好友*/
    public void AddFriend()
    {
        string inputName = "";
        UnityEngine.Transform transform;
        transform = gameObject.transform.Find("HeadInfo/Name");
        if (null != transform)
        {
            UILabel uiLable = transform.gameObject.GetComponent<UILabel>();
            if (null != uiLable)
            {
                inputName = uiLable.text;
            }
        }
        LogicSystem.PublishLogicEvent("ge_add_friend", "lobby", inputName);
    }
    public void Dare()
    {
        string inputName = "";
        UnityEngine.Transform transform;
        transform = gameObject.transform.Find("HeadInfo/Name");
        if (null != transform)
        {
            UILabel uiLable = transform.gameObject.GetComponent<UILabel>();
            if (null != uiLable)
            {
                inputName = uiLable.text;
            }
        }
        LogicSystem.PublishLogicEvent("ge_request_dare_by_nickname", "lobby", inputName);
    }
    /*组队*/
    public void Team()
    {
        string inputName = "";
        UnityEngine.Transform transform;
        transform = gameObject.transform.Find("HeadInfo/Name");
        if (null != transform)
        {
            UILabel uiLable = transform.gameObject.GetComponent<UILabel>();
            if (null != uiLable)
            {
                inputName = uiLable.text;
            }
        }

        ArkCrossEngine.RoleInfo roleInfo = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
        if (roleInfo.Group.Count == 0)
        {
            foreach (ArkCrossEngine.GroupMemberInfo member in roleInfo.Group.Members)
            {
                if (member.Nick == inputName)
                {
                    SendScreeTipCenter(575, inputName);
                    return;
                }
            }
            SendScreeTipCenter(577, inputName);
            ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_pinvite_team", "lobby", roleInfo.Nickname, inputName);
        }
        else if (roleInfo.Guid == roleInfo.Group.CreatorGuid)
        {
            foreach (ArkCrossEngine.GroupMemberInfo member in roleInfo.Group.Members)
            {
                if (member.Nick == inputName)
                {
                    SendScreeTipCenter(575, inputName);
                    return;
                }
            }
            SendScreeTipCenter(560, inputName);
            ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_pinvite_team", "lobby", roleInfo.Nickname, inputName);
        }
        else
        {
            SendScreeTipCenter(574);
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
    /*设置好友头像区的信息*/
    void SetHeadInfo(GfxUserInfo userInfo)
    {
        SharedGameObjectInfo share_info = LogicSystem.GetSharedGameObjectInfo(userInfo.m_ActorId);
        UnityEngine.Transform transform;
        transform = gameObject.transform.Find("HeadInfo/Head");
        if (transform != null)
        {
            UISprite us = transform.gameObject.GetComponent<UISprite>();
            if (us != null)
            {
                Data_PlayerConfig cg = PlayerConfigProvider.Instance.GetPlayerConfigById(userInfo.m_HeroId);
                us.spriteName = cg.m_Portrait;
            }
        }
    }
    /*设置战斗力信息*/
    public void SetScoreInfo(string name, int level, int score)
    {
        UnityEngine.Transform transform;
        transform = gameObject.transform.Find("HeadInfo/Name");
        if (null != transform)
        {
            UILabel uiLable = transform.gameObject.GetComponent<UILabel>();
            if (null != uiLable)
            {
                uiLable.text = name;
            }
        }
        transform = gameObject.transform.Find("HeadInfo/Combat");
        if (null != transform)
        {
            UILabel uiLable = transform.gameObject.GetComponent<UILabel>();
            if (null != uiLable)
            {
                uiLable.text = score.ToString();
            }
        }
        transform = gameObject.transform.Find("HeadInfo/Level");
        if (null != transform)
        {
            UILabel uiLable = transform.gameObject.GetComponent<UILabel>();
            if (null != uiLable)
            {
                uiLable.text = "LV." + level.ToString();
            }
        }
    }
    /*检查是否为好友*/
    bool CheckIsFriend(string name)
    {
        if (null != name)
        {
            Dictionary<ulong, FriendInfo> friends = LobbyClient.Instance.CurrentRole.Friends;
            if (null != friends && friends.Count > 0)
            {
                foreach (FriendInfo fi in friends.Values)
                {
                    if (fi.Nickname == name)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }



}
