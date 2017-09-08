using UnityEngine;
using System;
using ArkCrossEngine;

public class UIActivitySignIn : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject goSlot;
    public UITable tableContainer;
    private UIActivitySignInSlot[] SignInSlotArr;
    private bool m_HavenInit = false;
    // Use this for initialization
    void Start()
    {
        try
        {
            InitAwardItems();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Awake()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    public void InitAwardItems()
    {
        if (goSlot == null || tableContainer == null)
        {
            Debug.Log("goSlot or tableContainer is null!!!");
            return;
        }
        DateTime dtNow = DateTime.Now;
        int daysInMonth = DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
        SignInSlotArr = new UIActivitySignInSlot[daysInMonth];
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            for (int day = 1; day <= daysInMonth; ++day)
            {
                int itemId, itemNum;
                if (SignInRewardConfigProvider.Instance.GetDataByDate(dtNow.Month, day, out itemId, out itemNum))
                {
                    UnityEngine.GameObject go = NGUITools.AddChild(tableContainer.gameObject, goSlot);
                    if (go != null)
                    {
                        UIActivitySignInSlot signInSlot = go.GetComponent<UIActivitySignInSlot>();
                        if (signInSlot == null) return;
                        SignInSlotArr[day - 1] = signInSlot;
                        bool signed = (day <= role_info.SignInCountCurMonth);
                        bool canSign = ((day == role_info.SignInCountCurMonth + 1) && role_info.RestSignInCount > 0);
                        signInSlot.Init(itemId, itemNum, signed, canSign, day);
                        DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Sign_in, go, itemId, itemNum);
                    }
                }
            }
            tableContainer.Reposition();
            m_HavenInit = true;
        }
    }
    public void HandleActivitySignIn(bool successed)
    {
        if (successed)
        {
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            if (role_info != null)
            {
                int shedule = role_info.SignInCountCurMonth;
                if (shedule <= SignInSlotArr.Length && SignInSlotArr[shedule - 1] != null)
                {
                    SignInSlotArr[shedule - 1].HandleSignInResult(true);
                }
                if (!IsExceedTheMaxSignDay() && shedule < SignInSlotArr.Length && SignInSlotArr[shedule] != null && role_info.RestSignInCount > 0)
                {
                    SignInSlotArr[shedule].EnableSignIn(true);
                }
            }
        }
    }
    //判断是否超过最大可签到次数（这个月已经过去的天数）
    private bool IsExceedTheMaxSignDay()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            return role_info.SignInCountCurMonth >= DateTime.Now.Day;
        }
        return true;
    }
    public void HandleSyncActivityData()
    {
        if (m_HavenInit)
        {
            //如果还没初始化则不再同步，因为打开的时候会同步一次
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
                        UIActivitySignInSlot signInSlot = SignInSlotArr[day - 1];
                        bool signed = (day <= role_info.SignInCountCurMonth);
                        bool canSign = ((day == role_info.SignInCountCurMonth + 1) && role_info.RestSignInCount > 0);
                        if (signInSlot != null) signInSlot.Init(itemId, itemNum, signed, canSign, day);
                    }
                }
            }
        }
    }

}
