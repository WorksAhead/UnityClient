using UnityEngine;
using System;
using System.Collections.Generic;
using ArkCrossEngine;

public class UIVerification : UnityEngine.MonoBehaviour
{
    public enum OpenType : int
    {
        Verification = 0,  //面板为 激活码 panel
        AddFriend,         //面板为 加好友 panel   
    }
    public enum ActivateAccountState : int
    {
        Activated = 0,  //已激活
        Unactivated,    //未激活    
        Banned,         //账号封停
    }
    public enum ActivateAccountResult : int
    {
        Success = 0,    //激活成功
        InvalidCode,    //失效的激活码（该激活码已经被使用）
        MistakenCode,   //错误的激活码（该激活码不存在）
        Error,          //激活失败(系统问题)
    }

    // Use this for initialization
    public UILabel lblHint = null;
    public UIInput uiInput = null;
    private List<object> m_EventList = new List<object>();
    private ActivateAccountState m_ActivateState = ActivateAccountState.Unactivated;
    private int m_AcivateFailureCount = 0;
    private const int c_BanCount = 30;
    private const float c_TimeoutInterval = 30.0f;    //激活未响应超时时间30s
    public void UnSubscribe()
    {
        try
        {
            for (int i = 0; i < m_EventList.Count; i++)
            {
                if (m_EventList[i] != null)
                {
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(m_EventList[i]);
                }
            }
            /*
      foreach (object obj in m_EventList) {
        if (null != obj) LogicSystem.EventChannelForGfx.Unsubscribe(obj);
      }*/
            m_EventList.Clear();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Awake()
    {
        try
        {
            object obj = null;
            obj = LogicSystem.EventChannelForGfx.Subscribe<int>("ge_activate_result", "lobby", OnActivateResult);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null) m_EventList.Add(obj);

            m_AcivateFailureCount = PlayerPrefs.GetInt("AcivateFailureCount");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnReturn()
    {
        UIManager.Instance.HideWindowByName("Verification");
        UIManager.Instance.ShowWindowByName("LoginPrefab");
    }
    public void OnSubmit()
    {
        string hintStr = "";
        if (m_AcivateFailureCount > c_BanCount)
        {
            //封停
            m_ActivateState = ActivateAccountState.Banned;
            string chn_des = StrDictionaryProvider.Instance.GetDictString(202);
            hintStr = chn_des;
        }
        if (m_ActivateState == ActivateAccountState.Unactivated)
        {
            if (uiInput != null)
            {
                string code = uiInput.value.Trim();
                if (code.Equals(String.Empty))
                {
                    string chn_des = StrDictionaryProvider.Instance.GetDictString(203);
                    hintStr = chn_des;
                }
                else
                {
                    LogicSystem.PublishLogicEvent("ge_activate_account", "lobby", code);
                    Invoke("OnActivateTimeout", c_TimeoutInterval);
                }
            }
        }
        if (lblHint != null)
        {
            lblHint.text = hintStr;
            NGUITools.SetActive(lblHint.gameObject, true);
        }
    }
    public void OnActivateResult(int ret)
    {
        try
        {
            CancelInvoke("OnActivateTimeout");
            string hintStr = "";
            if (ret == (int)ActivateAccountResult.Success)
            {
                //获取角色列表成功，跳转到角色选择页面
                m_AcivateFailureCount = 0;
                UIManager.Instance.HideWindowByName("Verification");
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_create_hero_scene", "ui", true);
            }
            else if (ret == (int)ActivateAccountResult.InvalidCode)
            {
                m_AcivateFailureCount++;
                string chn_des = StrDictionaryProvider.Instance.GetDictString(204);
                hintStr = chn_des;
            }
            else if (ret == (int)ActivateAccountResult.MistakenCode)
            {
                m_AcivateFailureCount++;
                string chn_des = StrDictionaryProvider.Instance.GetDictString(205);
                hintStr = chn_des;
            }
            else
            {
                string chn_des = StrDictionaryProvider.Instance.GetDictString(206);
                hintStr = chn_des;
            }
            if (lblHint != null)
            {
                lblHint.text = hintStr;
                NGUITools.SetActive(lblHint.gameObject, true);
            }
            PlayerPrefs.SetInt("AcivateFailureCount", m_AcivateFailureCount);
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void OnActivateTimeout()
    {
        this.OnActivateResult((int)ActivateAccountResult.Error);
    }
    /**初始化面板类型  by 李齐  because : 设为公用面板*/
    public void InitOpenType(OpenType type)
    {
        switch (type)
        {
            case OpenType.Verification:
                InitVerificationPanel();
                break;
            case OpenType.AddFriend:
                InitFriendPanel();
                break;
        }
    }
    /*初始化 激活码面板*/
    void InitVerificationPanel()
    {
        UnityEngine.Transform tf;
        tf = gameObject.transform.Find("Submit");
        if (null != tf)
        {
            NGUITools.SetActive(tf.gameObject, true);
        }
        tf = gameObject.transform.Find("Confirm");
        if (null != tf)
        {
            NGUITools.SetActive(tf.gameObject, false);
        }
        tf = gameObject.transform.Find("Cancel");
        if (null != tf)
        {
            NGUITools.SetActive(tf.gameObject, false);
        }
        tf = gameObject.transform.Find("Label");
        UILabel uiLable;
        if (null != tf)
        {
            uiLable = tf.gameObject.GetComponent<UILabel>();
            if (null != uiLable)
            {
                string chn_des = StrDictionaryProvider.Instance.GetDictString(207);
                uiLable.text = chn_des;
            }
        }
        tf = gameObject.transform.Find("InputField/Label");
        if (null != tf)
        {
            uiLable = tf.gameObject.GetComponent<UILabel>();
            if (null != uiLable)
            {
                string chn_des = StrDictionaryProvider.Instance.GetDictString(207);
                uiLable.text = chn_des;
            }
        }
    }
    /*初始化 添加好友面板*/
    void InitFriendPanel()
    {
        UnityEngine.Transform tf;
        tf = gameObject.transform.Find("Submit");
        if (null != tf)
        {
            NGUITools.SetActive(tf.gameObject, false);
        }
        tf = gameObject.transform.Find("Confirm");
        if (null != tf)
        {
            NGUITools.SetActive(tf.gameObject, true);
        }
        tf = gameObject.transform.Find("Cancel");
        if (null != tf)
        {
            NGUITools.SetActive(tf.gameObject, true);
        }
        tf = gameObject.transform.Find("Label");
        UILabel uiLable;
        if (null != tf)
        {
            uiLable = tf.gameObject.GetComponent<UILabel>();
            if (null != uiLable)
            {
                string chn_des = StrDictionaryProvider.Instance.GetDictString(551);
                uiLable.text = chn_des;
            }
        }
        tf = gameObject.transform.Find("InputField/Label");
        if (null != tf)
        {
            uiLable = tf.gameObject.GetComponent<UILabel>();
            if (null != uiLable)
            {
                string chn_des = StrDictionaryProvider.Instance.GetDictString(552);
                uiLable.text = chn_des;
            }
        }
        uiInput.isSelected = true;
    }
    /*取消 按钮*/
    public void Cancel()
    {
        UIManager.Instance.HideWindowByName("Verification");
    }
    /*确定 按钮*/
    public void Confirm()
    {
        string inputName = "";
        if (uiInput != null)
        {
            inputName = uiInput.value.Trim();
        }
        if (CheckIsFriend(inputName))
        {

        }
        else
        {
            LogicSystem.PublishLogicEvent("ge_add_friend", "lobby", inputName);
        }

        uiInput.value = "";
        UIManager.Instance.HideWindowByName("Verification");
    }
    /*检查是否为好友*/
    bool CheckIsFriend(string name)
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        string chn_desc = "";
        if (null != name)
        {
            Dictionary<ulong, FriendInfo> friends = LobbyClient.Instance.CurrentRole.Friends;
            if (name == "")
            {

                string confirm = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(4);
                chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(552);
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
                return true;
            }
            else if (role.Nickname == name)
            {
                chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(550);
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
                return true;
            }
            else if (null != friends && friends.Count > 0)
            {
                foreach (FriendInfo fi in friends.Values)
                {
                    if (fi.Nickname == name)
                    {
                        chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(556);
                        LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
