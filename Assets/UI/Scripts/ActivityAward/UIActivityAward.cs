using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class UIActivityAward : UnityEngine.MonoBehaviour
{
    public UILabel lblTipForToday;
    public UILabel lblSignInCountThisMonth;//本月签到次数
    public UILabel lblLeftSignInCount;//剩余签到次数
    public UILabel lblMonthTitle;
    public UISprite[] spSelectArr = new UISprite[4];
    public UIActivitySignIn uiActivitySignIn;//签到
    public UIActivityGift uiActivityGift;//大礼包
    public UIActivityLoginAward uiActivityLoginAward;//登录奖励
    public UnityEngine.GameObject goLoginButtonTab;
    public UnityEngine.GameObject goSignButtonTab;
    private const string HighLight = "biao-qian-an-niu2";
    private const string AshLight = "biao-qian-an-niu1";
    private List<object> m_EventList = new List<object>();
    private enum ButtonsTypeEnum
    {
        SignIn,
        WeeklyLogin,
        GiftBag,
        OnlineReward,
    }
    // Use this for initialization
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
            m_EventList.Clear();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OnEnable()
    {
        try
        {
            RefreshTapButtons();
            OnSignInBtnClick();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Awake()
    {
        try
        {
            SetSignInInfo();
            object ob = LogicSystem.EventChannelForGfx.Subscribe<bool>("ge_sign_award_result", "ui", HandleActivitySignInResult);
            if (ob != null) m_EventList.Add(ob);
            ob = LogicSystem.EventChannelForGfx.Subscribe("ge_sync_sign_count", "ui", HandleSyncSignInCount);
            if (ob != null) m_EventList.Add(ob);
            ob = LogicSystem.EventChannelForGfx.Subscribe<ArkCrossEngine.Network.GeneralOperationResult, int>("ge_exchange_gift_result", "ui", HandleExchangeGiftResult);
            if (ob != null) m_EventList.Add(ob);
            ob = LogicSystem.EventChannelForGfx.Subscribe<int>("ge_get_weekly_reward", "ui", HandleGetWeeklyReward);
            if (ob != null) m_EventList.Add(ob);
            CheckHasLoginAward();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    //设置签到信息
    public void SetSignInInfo()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            string chn_des = StrDictionaryProvider.Instance.GetDictString(1160);
            if (lblSignInCountThisMonth != null)
                lblSignInCountThisMonth.text = string.Format(chn_des, role_info.SignInCountCurMonth);
            chn_des = StrDictionaryProvider.Instance.GetDictString(1161);
            if (lblMonthTitle != null)
            {
                lblMonthTitle.text = string.Format(chn_des, DateTime.Now.Month);
            }
            if (lblLeftSignInCount != null) lblLeftSignInCount.text = role_info.RestSignInCount.ToString();
        }
    }
    private void RefreshTapButtons()
    {
        bool visible = WeeklyLoginConfigProvider.Instance.IsUnderProgress();
        if (goLoginButtonTab != null) NGUITools.SetActive(goLoginButtonTab, visible);
    }

    //点击签到
    public void OnSignInBtnClick()
    {
        SetButtonsState(ButtonsTypeEnum.SignIn);
        if (uiActivityGift != null) NGUITools.SetActive(uiActivityGift.gameObject, false);
        if (uiActivitySignIn != null) NGUITools.SetActive(uiActivitySignIn.gameObject, true);
        if (uiActivityLoginAward != null) NGUITools.SetActive(uiActivityLoginAward.gameObject, false);
    }
    //点击礼包
    public void OnGiftBtnClick()
    {
        SetButtonsState(ButtonsTypeEnum.GiftBag);
        if (uiActivityGift != null) NGUITools.SetActive(uiActivityGift.gameObject, true);
        if (uiActivitySignIn != null) NGUITools.SetActive(uiActivitySignIn.gameObject, false);
        if (uiActivityLoginAward != null) NGUITools.SetActive(uiActivityLoginAward.gameObject, false);
    }
    //点击七天登录
    public void OnLoginRewardClick()
    {
        SetButtonsState(ButtonsTypeEnum.WeeklyLogin);
        if (uiActivityGift != null) NGUITools.SetActive(uiActivityGift.gameObject, false);
        if (uiActivitySignIn != null) NGUITools.SetActive(uiActivitySignIn.gameObject, false);
        if (uiActivityLoginAward != null) NGUITools.SetActive(uiActivityLoginAward.gameObject, true);
    }
    private void SetButtonsState(ButtonsTypeEnum btnType)
    {
        for (int index = 0; index < spSelectArr.Length; ++index)
        {
            if (index == (int)btnType)
            {
                spSelectArr[index].spriteName = HighLight;
                UIButton btn = spSelectArr[index].GetComponent<UIButton>();
                if (btn != null) btn.normalSprite = HighLight;
            }
            else
            {
                spSelectArr[index].spriteName = AshLight;
                UIButton btn = spSelectArr[index].GetComponent<UIButton>();
                if (btn != null) btn.normalSprite = AshLight;
            }
        }
    }
    //点击签到结果
    private void HandleActivitySignInResult(bool successed)
    {
        try
        {
            SetSignInInfo();
            LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, false);
            if (uiActivitySignIn != null) uiActivitySignIn.HandleActivitySignIn(successed);
            if (successed) CheckHasLoginAward();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //同步签到数据
    private void HandleSyncSignInCount()
    {
        try
        {
            SetSignInInfo();
            if (uiActivitySignIn != null) uiActivitySignIn.HandleSyncActivityData();
            CheckHasLoginAward();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //礼品码兑换结果
    private void HandleExchangeGiftResult(ArkCrossEngine.Network.GeneralOperationResult result, int giftId)
    {
        try
        {
            LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, false);
            string chn_desc = "";
            switch (result)
            {
                case ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed:
                    chn_desc = StrDictionaryProvider.Instance.GetDictString(1152);
                    break;//礼品兑换成功
                case ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_Unknown:
                    chn_desc = StrDictionaryProvider.Instance.GetDictString(1153);
                    break;       //礼品码兑换失败
                case ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_Code_Used:
                    chn_desc = StrDictionaryProvider.Instance.GetDictString(1154);
                    break;     //礼品码已经被使用,无效
                case ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_Code_Error:
                    chn_desc = StrDictionaryProvider.Instance.GetDictString(1155);
                    break;    //礼品码错误
                case ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_Overflow:
                    chn_desc = StrDictionaryProvider.Instance.GetDictString(1156);
                    break;      //礼品领取次数超过限制
            }
            LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            if (result == ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed)
            {
                GiftConfig giftConfig = GiftConfigProvider.Instance.GetDataById(giftId);
                if (giftConfig != null)
                {
                    UnityEngine.GameObject goTaskAward = UIManager.Instance.GetWindowGoByName("TaskAward");
                    if (goTaskAward != null)
                    {
                        TaskAward taskAward = goTaskAward.GetComponent<TaskAward>();
                        taskAward.SetAwardForActivity(giftConfig.ItemIdList, giftConfig.ItemNumList);
                        UIManager.Instance.ShowWindowByName("TaskAward");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //七天奖励
    private void HandleGetWeeklyReward(int result)
    {
        LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, false);
        if (result == (int)ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed)
        {
            if (uiActivityLoginAward != null) uiActivityLoginAward.HandleGetRewardSuccess();
            CheckHasLoginAward();
        }
        else
        {
            string chn_desc = StrDictionaryProvider.Instance.GetDictString(1164);
            LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
        }
    }

    private void CheckHasLoginAward()
    {
        bool has1 = false;
        bool has2 = false;
        DateTime dtNow = DateTime.Now;
        int daysInMonth = DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            for (int day = 1; day <= daysInMonth; ++day)
            {
                int itemId, itemNum;
                if (SignInRewardConfigProvider.Instance.GetDataByDate(dtNow.Month, day, out itemId, out itemNum))
                {
                    if ((day == role_info.SignInCountCurMonth + 1) && role_info.RestSignInCount > 0)
                    {//签到
                        has1 = true;
                        break;
                    }
                }
            }

            if (WeeklyLoginConfigProvider.Instance.IsUnderProgress())
            {//7天
                int todayIndex = WeeklyLoginConfigProvider.Instance.GetTodayIndex();
                if (!role_info.WeeklyLoginRewardRecord.Contains(todayIndex))
                {
                    has2 = true;
                }
            }

            // 显示签到按钮上提示
            UnityEngine.Transform tfTip = null;
            if (goSignButtonTab != null)
            {
                tfTip = goSignButtonTab.transform.Find("Tip");
                if (tfTip != null)
                {
                    NGUITools.SetActive(tfTip.gameObject, has1);
                }
            }
            // 显示登陆按钮上提示
            if (goLoginButtonTab != null)
            {
                tfTip = goLoginButtonTab.transform.Find("Tip");
                if (tfTip != null)
                {
                    NGUITools.SetActive(tfTip.gameObject, has2);
                }
            }
        }

        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.Award, has1 || has2);
    }

    public void OnCloseButtonClick()
    {
        UIManager.Instance.HideWindowByName("ActivityAward");
    }
}
