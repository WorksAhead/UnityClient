using System;
using System.Collections.Generic;
using ArkCrossEngine;

public class UIActivityLoginSlot : UnityEngine.MonoBehaviour
{

    public UILabel lblDays;
    public UnityEngine.GameObject goCurrentFlag;//当天可点击状态
    public UnityEngine.GameObject goDonotSignFlag;
    public UnityEngine.GameObject goDoSignFlag;//已经签完的标记
    private int m_ItemId;
    private int m_ItemNum;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Init(bool signed, int todayIndex, int day, int itemId, int itemNum, DateTime startDate)
    {
        m_ItemId = itemId;
        m_ItemNum = itemNum;

        DateTime currentDate = startDate.AddDays((double)day);
        int month = currentDate.Month;
        int dayOfMonth = currentDate.Day;
        string chn_desc = StrDictionaryProvider.Instance.GetDictString(1163);
        if (lblDays != null) lblDays.text = string.Format(chn_desc, month, dayOfMonth);

        DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Login_Award, this.gameObject, itemId, itemNum);
        bool isCurrent = day == todayIndex;
        bool isPass = day < todayIndex;
        SetState(signed, isCurrent, isPass);
    }

    public void SetState(bool signed, bool isCurrent, bool isPass)
    {
        if (signed)
        {
            //已经领完
            if (goDoSignFlag != null) NGUITools.SetActive(goDoSignFlag, true);
            if (goDonotSignFlag != null) NGUITools.SetActive(goDonotSignFlag, false);
            if (goCurrentFlag != null) NGUITools.SetActive(goCurrentFlag, false);
            EnableButton(false);
        }
        else
        {
            if (goDoSignFlag != null) NGUITools.SetActive(goDoSignFlag, false);
            if (goDonotSignFlag != null) NGUITools.SetActive(goDonotSignFlag, !isPass);
            if (isCurrent)
            {
                if (goCurrentFlag != null) NGUITools.SetActive(goCurrentFlag, true);
                EnableButton(true);
            }
            else
            {
                EnableButton(false);
                if (goCurrentFlag != null) NGUITools.SetActive(goCurrentFlag, false);
            }
        }
    }
    public void HandleSetGetReward()
    {
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
        if (goDoSignFlag != null)
            NGUITools.SetActive(goDoSignFlag, true);
        EnableButton(false);
    }
    void OnClick()
    {
        LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
        LogicSystem.PublishLogicEvent("ge_get_weekly_login_reward", "lobby");
    }
    public void EnableButton(bool enable)
    {
        UIButton btn = this.GetComponent<UIButton>();
        if (btn != null)
        {
            btn.isEnabled = enable;
        }
    }
}
