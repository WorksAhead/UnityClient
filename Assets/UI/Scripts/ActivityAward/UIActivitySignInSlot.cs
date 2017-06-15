using System;
using System.Collections.Generic;
using ArkCrossEngine;

public class UIActivitySignInSlot : UnityEngine.MonoBehaviour
{

    public UITexture texItem;
    public UILabel lblItemNum;
    public UnityEngine.GameObject goSignFlag;
    public UnityEngine.GameObject goCanSignFlag;
    public UIButton btnSignIn;
    private bool m_CanSign = false;
    private bool m_HavenSigned = false;
    private int m_Day = -1;
    private int m_ItemId = -1;
    private int m_ItemNum = 0;
    public UnityEngine.GameObject self;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Init(int itemId, int num, bool signed, bool canSign, int day)
    {
        m_Day = day;
        m_CanSign = canSign;
        m_ItemId = itemId;
        m_ItemNum = num;

        if (m_CanSign)
        {
            if (goCanSignFlag != null) NGUITools.SetActive(goCanSignFlag, true);
        }
        else
        {
            if (goCanSignFlag != null) NGUITools.SetActive(goCanSignFlag, false);
        }
        //EnableButton(canSign);
        if (goSignFlag != null) NGUITools.SetActive(goSignFlag, signed);
    }
    //设置签到状态
    public void HandleSignInResult(bool signed)
    {
        //EnableButton(signed);
        if (signed)
        {
            m_CanSign = false;
            UnityEngine.GameObject goTaskAward = UIManager.Instance.GetWindowGoByName("TaskAward");
            if (goTaskAward != null)
            {
                TaskAward taskAward = goTaskAward.GetComponent<TaskAward>();
                List<int> items = new List<int>();
                List<int> nums = new List<int>();
                items.Add(m_ItemId);
                nums.Add(m_ItemNum);
                taskAward.SetAwardForActivity(items, nums);
                UIManager.Instance.ShowWindowByName("TaskAward");
            }
        }
        m_HavenSigned = signed;
        if (goSignFlag != null) NGUITools.SetActive(goSignFlag, signed);
        if (goCanSignFlag != null) NGUITools.SetActive(goCanSignFlag, !signed);
    }
    //设置成可签到
    public void EnableSignIn(bool enable)
    {
        //EnableButton(enable);
        m_CanSign = enable;
        if (goCanSignFlag != null) NGUITools.SetActive(goCanSignFlag, enable);
    }
    private void SetAwardItem(int itemId)
    {
        ItemConfig itemCfg = ItemConfigProvider.Instance.GetDataById(itemId);
        if (itemCfg == null) return;
        UnityEngine.Texture tex = ResourceSystem.GetSharedResource(itemCfg.m_ItemTrueName) as UnityEngine.Texture;
        if (texItem != null)
        {
            if (tex != null)
            {
                texItem.mainTexture = tex;
            }
        }
    }
    private void EnableButton(bool enable)
    {
        if (btnSignIn == null) return;
        btnSignIn.enabled = enable;
        btnSignIn.isEnabled = enable;
    }
    void OnClick()
    {
        if (m_CanSign)
        {
            LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
            LogicSystem.PublishLogicEvent("ge_signin_and_get_reward", "lobby");
        }
        else
        {
            if (m_HavenSigned)
            {
                //已经签完了
                string chn_des = StrDictionaryProvider.Instance.GetDictString(1150);
                chn_des = string.Format(chn_des, m_Day);
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_des, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            }
            else
            {
                //还没签且尚不能签
                int strId = 1151;
                if (IsExceedTheMaxSignDay())
                    strId = 1165;
                string chn_des = StrDictionaryProvider.Instance.GetDictString(strId);
                chn_des = string.Format(chn_des, m_Day);
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_des, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            }
        }
    }
    //有可签到次数但是已经超过了可签到日期
    private bool IsExceedTheMaxSignDay()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            return (role_info.SignInCountCurMonth >= DateTime.Now.Day && role_info.RestSignInCount > 0);
        }
        return false;
    }
}
