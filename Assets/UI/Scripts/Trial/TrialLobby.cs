using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;
using System;

public enum TrialIntroType : int
{
    None = 0,
    JinBi,
    ShiLian
}

public class TrialLobby : UnityEngine.MonoBehaviour
{

    //private const int c_ButtonNumber = 3;
    private List<object> m_EventList = new List<object>();
    private List<UnityEngine.GameObject> buttons = new List<UnityEngine.GameObject>();//new UnityEngine.GameObject[c_ButtonNumber];

    public UIGrid grid = null;
    public UIScrollView scrollView = null;
    private UnityEngine.GameObject trialIntro = null;

    private const string NAME_GOLD = "Unit-Gold";
    private const string NAME_TRIAL = "Unit-Trial";
    private const string NAME_TREASURE = "Unit-Treasure";

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

    void Awake()
    {
        try
        {
            object obj = null;
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null)
                m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<MatchingType, bool>("ge_match_state_change", "matching", OnMatchStateChange);
            if (obj != null)
                m_EventList.Add(obj);
            obj = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("ge_user_levelup", "property", UserLevelUP);
            if (obj != null)
                m_EventList.Add(obj);

            if (grid != null)
            {
                int childNum = grid.transform.childCount;
                buttons.Clear();
                //buttons = new List<UnityEngine.GameObject>();
                for (int i = 0; i < childNum; i++)
                {
                    UnityEngine.GameObject go = grid.transform.GetChild(i).gameObject;
                    buttons.Add(go);
                    UIEventListener.Get(go).onClick += this.OnButtonClick;
                }
            }
            //UIManager.Instance.HideWindowByName("Trial");

            CheckHasTip();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void OnEnable()
    {
        try
        {
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            for (int i = 0; i < buttons.Count; i++)
            {
                bool open = false;
                int openLv = 0;
                string btnName = buttons[i].name;
                string unitName = "";
                string openTime = "";
                int type = -1;
                switch (btnName.TrimStart("1234567890".ToCharArray()))
                {
                    case NAME_GOLD:
                        openLv = LevelLockProvider.Instance.GetDataById(15).m_Level;//RoleInfo.c_GoldOpenLv;
                        unitName = StrDictionaryProvider.Instance.GetDictString(882);
                        openTime = GetOpenTime((int)MatchSceneEnum.Gold);
                        type = (int)GuideActionEnum.Gold;
                        break;
                    case NAME_TREASURE:
                        openLv = LevelLockProvider.Instance.GetDataById(13).m_Level;//ExpeditionPlayerInfo.c_UnlockLevel;
                        unitName = StrDictionaryProvider.Instance.GetDictString(881);
                        openTime = "00:00-24:00";
                        type = (int)GuideActionEnum.Expedition;
                        break;
                    case NAME_TRIAL:
                        openLv = LevelLockProvider.Instance.GetDataById(14).m_Level;//RoleInfo.c_TrialOpenLv;
                        unitName = StrDictionaryProvider.Instance.GetDictString(874);
                        openTime = GetOpenTime((int)MatchSceneEnum.Attempt);
                        type = (int)GuideActionEnum.Attempt;
                        break;
                }
                if (role_info != null && role_info.Level >= openLv)
                {
                    open = true;
                    if (!role_info.IsDoneAction(type))
                    {
                        PlayUnLock(buttons[i]);
                    }
                }

                TrialUnit unit = buttons[i].GetComponent<TrialUnit>();
                if (unit != null)
                {
                    unit.UpdateData(unitName, open, openTime, openLv);
                }
            }
            CheckHasTip();
            ////播放解锁特效
            //buttons.Sort(SortByLv);
            //if (role_info != null) {
            //  if (role_info.HasActivityTip) {
            //    role_info.HasActivityTip = false;
            //    CheckHasTip();
            //    PlayUnLock();
            //  }
            //}

            if (grid != null)
            {
                grid.repositionNow = true;
            }
            if (scrollView != null)
            {
                int num = buttons.Count;
                if (num <= 4 && num > 0)
                {
                    scrollView.enabled = false;
                    //强制居中
                    UIPanel panel = scrollView.gameObject.GetComponent<UIPanel>();
                    if (panel != null && buttons.Count > 0)
                    {
                        float interval = grid.cellWidth - buttons[0].GetComponent<UISprite>().width;
                        float totalWidth = grid.cellWidth * num - interval;
                        float scrollWidth = panel.GetViewSize().x;
                        float tempX = scrollWidth / 2 - totalWidth / 2 - 25;
                        UnityEngine.Vector2 tempVec2 = panel.clipOffset;
                        panel.clipOffset = new UnityEngine.Vector2(-tempX, tempVec2.y);
                        UnityEngine.Vector3 tempLocal = scrollView.gameObject.transform.localPosition;
                        scrollView.gameObject.transform.localPosition = new UnityEngine.Vector3(tempX, tempLocal.y, tempLocal.z);
                    }
                }
                else
                {
                    scrollView.enabled = true;
                }
            }
            SetCoin();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void PlayUnLock(UnityEngine.GameObject button)
    {
        //for (int i = 0; i < buttons.Count; i++) {
        //  TrialUnit unit = buttons[i].GetComponent<TrialUnit>();
        //  if (unit != null) {
        //    if (unit.HasOpen) {//播放第一个为开放的（等级从大到小排的序）
        //      unit.PlayUnLock();
        //      break;
        //    }
        //  }
        //}
        TrialUnit unit = button.GetComponent<TrialUnit>();
        if (unit != null)
        {
            unit.PlayUnLock();
        }
    }

    ////按等级从大到小排序
    //private int SortByLv (UnityEngine.GameObject a, UnityEngine.GameObject b)
    //{
    //  TrialUnit ua = a.GetComponent<TrialUnit>();
    //  TrialUnit ub = b.GetComponent<TrialUnit>();
    //  if (ua != null && ub != null) {
    //    if (ua.OpenLv > ub.OpenLv) {
    //      return -1;
    //    } else if (ua.OpenLv < ub.OpenLv) {
    //      return 1;
    //    } else {
    //      return 0;
    //    }
    //  }
    //  return 0;
    //}

    private void UserLevelUP(int lv)
    {
        CheckHasTip();
    }

    private void CheckHasTip()
    {
        bool has = false;
        RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
        if (roleInfo != null)
        {
            if ((!roleInfo.IsDoneAction((int)GuideActionEnum.Gold) && roleInfo.Level >= LevelLockProvider.Instance.GetDataById(15).m_Level) ||
                (!roleInfo.IsDoneAction((int)GuideActionEnum.Attempt) && roleInfo.Level >= LevelLockProvider.Instance.GetDataById(14).m_Level) ||
                (!roleInfo.IsDoneAction((int)GuideActionEnum.Expedition) && roleInfo.Level >= LevelLockProvider.Instance.GetDataById(13).m_Level))
            {
                has = true;
            }
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.Activity, has);
    }

    private string GetOpenTime(int sceneId)
    {
        MpveTimeConfig timeConfig = MpveTimeConfigProvider.Instance.GetDataById(sceneId);
        string startTime = "00:00";
        string endTime = "24:00";
        if (timeConfig != null)
        {
            startTime = timeConfig.m_StartHour >= 10 ? timeConfig.m_StartHour.ToString() : "0" + timeConfig.m_StartHour;
            startTime += ":" + (timeConfig.m_StartMinute >= 10 ? timeConfig.m_StartMinute.ToString() : "0" + timeConfig.m_StartMinute);

            endTime = timeConfig.m_EndHour >= 10 ? timeConfig.m_EndHour.ToString() : "0" + timeConfig.m_EndHour;
            endTime += ":" + (timeConfig.m_EndMinute >= 10 ? timeConfig.m_EndMinute.ToString() : "0" + timeConfig.m_EndMinute);
        }
        return startTime + "-" + endTime;
    }

    public void OnButtonClick(UnityEngine.GameObject go)
    {
        if (go == null)
            return;
        trialIntro = UIManager.Instance.GetWindowGoByName("TrialIntro");
        TrialIntro script = null;
        if (trialIntro != null)
        {
            script = trialIntro.GetComponent<TrialIntro>();
        }
        TrialUnit tu = go.GetComponent<TrialUnit>();
        if (tu.HasOpen == false)
        {//未开放
            SendScreeTipCenter(1200, tu.OpenLv.ToString());
            return;
        }
        switch (go.name.TrimStart("1234567890".ToCharArray()))
        {
            case NAME_GOLD:
                UIManager.Instance.ShowWindowByName("TrialIntro");

                if (script != null)
                {
                    script.SetIntroType(TrialIntroType.JinBi);
                }
                break;
            case NAME_TREASURE:
                UIManager.Instance.ToggleWindowVisible("cangbaotu");
                break;
            case NAME_TRIAL:
                UIManager.Instance.ShowWindowByName("TrialIntro");
                if (script != null)
                {
                    script.SetIntroType(TrialIntroType.ShiLian);
                }
                break;
            default:
                break;
        }
    }
    private void SendScreeTipCenter(int id, string name = "")
    {
        string chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.Format(id, name);
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
    }
    void Update()
    {

    }

    public void OnClickCloseButton()
    {
        UIManager.Instance.HideWindowByName("Trial");
    }
    //点击商店
    public void ClickStore()
    {
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("Store");
        if (go != null)
        {
            Store store = go.GetComponent<Store>();
            if (store != null)
            {
                store.ChangeStore(200001, "duihuansuipian");
            }
        }
        UIManager.Instance.ShowWindowByName("Store");
    }
    public UILabel coinNumUl;//金币数量文本
    private int currencyId = 200001;
    //金币数量
    private void SetCoin()
    {
        coinNumUl.text = GetItemNum(currencyId).ToString();
    }
    public void CurrencyButton()
    {
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_item_come_from", "ui", currencyId);
    }
    private int GetItemNum(int itemId)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            if (itemId == ItemConfigProvider.Instance.GetDiamondId())
            {
                return role_info.Gold;
            }
            else if (itemId == ItemConfigProvider.Instance.GetGoldId())
            {
                return role_info.Money;
            }
            else if (role_info.Items != null)
            {
                for (int i = 0; i < role_info.Items.Count; ++i)
                {
                    if (role_info.Items[i] != null && role_info.Items[i].ItemId == itemId)
                        return role_info.Items[i].ItemNum;
                }
            }
        }
        return 0;
    }
    private void OnMatchStateChange(MatchingType type, bool isShow)
    {
        string name = "";
        switch (type)
        {
            case MatchingType.Trial:
                name = NAME_TRIAL;
                break;
            case MatchingType.Gold:
                name = NAME_GOLD;
                break;
        }
        for (int i = 0; i < buttons.Count; i++)
        {
            string btnName = buttons[i].name;
            btnName = btnName.TrimStart("1234567890".ToCharArray());
            if (btnName == name)
            {
                MatchStateChange item = buttons[i].GetComponent<MatchStateChange>();
                if (item != null)
                {
                    item.SetState(isShow);
                    break;
                }
            }
        }
    }
}
