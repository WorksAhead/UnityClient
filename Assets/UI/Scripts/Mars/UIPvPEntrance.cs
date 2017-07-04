using System.Collections.Generic;
using ArkCrossEngine;
using System;

public class UIPvPEntrance : UnityEngine.MonoBehaviour
{
    private const int c_MarsUnlockLevelId = 18;
    private const int c_ArenaUnlockLevelId = 17;

    public UIButton btnMars;
    public UIButton btnArena;
    private int m_MarsUnlockLevel;
    private int m_ArenaUnlockLevel;
    private bool m_IsMarsUnlook = false;
    private bool m_IsArenaUnlock = false;
    public UILabel coinNumUl;//金币数量文本
    private int currencyId = 200007;
    //private List<UnityEngine.GameObject> buttons = new List<UnityEngine.GameObject>();
    // Use this for initialization
    private List<object> eventlist = new List<object>();
    public void UnSubscribe()
    {
        try
        {
            if (eventlist != null)
            {
                for (int i = 0; i < eventlist.Count; i++)
                {
                    if (eventlist[i] != null)
                    {
                        ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eventlist[i]);
                    }
                }
            }
            eventlist.Clear();
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
            object eo;
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (eo != null)
                eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("ge_user_levelup", "property", UserLevelUP);
            if (eo != null)
                eventlist.Add(eo);

            //buttons.Clear();
            //buttons.Add(btnMars.gameObject);
            //buttons.Add(btnArena.gameObject);

            LevelLock levelLockCfg = LevelLockProvider.Instance.GetDataById(c_MarsUnlockLevelId);
            if (levelLockCfg != null)
            {
                m_MarsUnlockLevel = levelLockCfg.m_Level;
            }
            levelLockCfg = LevelLockProvider.Instance.GetDataById(c_ArenaUnlockLevelId);
            if (levelLockCfg != null)
            {
                m_ArenaUnlockLevel = levelLockCfg.m_Level;
            }

            List<GowTimeConfig> timeCfgs = GowConfigProvider.Instance.GowTimeConfigMgr.GetData();
            string marsOpenTime = "00:00-24:00";
            if (timeCfgs.Count >= (int)GowTimeConfig.TimeTypeEnum.MatchTime)
            {
                //设置战神赛开赛时间
                GowTimeConfig openTimeCfg = timeCfgs[(int)GowTimeConfig.TimeTypeEnum.MatchTime - 1];
                if (openTimeCfg != null/* && lblMarsOpenTime!=null*/)
                {
                    int startHour = openTimeCfg.m_StartHour;
                    marsOpenTime = string.Format("{0}:{1:d2}-{2}:{3:d2}",
                    openTimeCfg.m_StartHour,
                    openTimeCfg.m_StartMinute,
                    openTimeCfg.m_EndHour,
                    openTimeCfg.m_EndMinute
                    );
                    //lblMarsOpenTime.text = openTime;
                }
            }
            if (btnMars != null)
            {
                TrialUnit unit = btnMars.GetComponent<TrialUnit>();
                if (unit != null)
                {
                    unit.SetLblLock(m_MarsUnlockLevel);
                    unit.SetLblTime(marsOpenTime);
                }
            }
            if (btnArena != null)
            {
                TrialUnit unit = btnArena.GetComponent<TrialUnit>();
                if (unit != null)
                {
                    unit.SetLblLock(m_ArenaUnlockLevel);
                }
            }
            //buttons.Sort(SortByLv);
            CheckHasTip();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
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
            if (roleInfo.Level >= LevelLockProvider.Instance.GetDataById(c_ArenaUnlockLevelId).m_Level && !roleInfo.IsDoneAction((int)GuideActionEnum.Arena) ||
                roleInfo.Level >= LevelLockProvider.Instance.GetDataById(c_MarsUnlockLevelId).m_Level && !roleInfo.IsDoneAction((int)GuideActionEnum.Gow))
            {
                has = true;
            }
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.pvp, has);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnEnable()
    {
        try
        {
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            if (role_info != null)
            {
                if (role_info.Level >= m_MarsUnlockLevel)
                {
                    m_IsMarsUnlook = true;
                    if (!role_info.IsDoneAction((int)GuideActionEnum.Gow))
                    {
                        PlayUnLock(btnMars.gameObject);
                    }
                }
                else
                {
                    m_IsMarsUnlook = false;
                }
                if (role_info.Level >= m_ArenaUnlockLevel)
                {
                    m_IsArenaUnlock = true;
                    if (!role_info.IsDoneAction((int)GuideActionEnum.Arena))
                    {
                        PlayUnLock(btnArena.gameObject);
                    }
                }
                else
                {
                    m_IsArenaUnlock = false;
                }

                if (btnMars != null)
                {
                    TrialUnit unit = btnMars.GetComponent<TrialUnit>();
                    if (unit != null)
                    {
                        unit.UpdateOpen(m_IsMarsUnlook);
                    }
                }
                if (btnArena != null)
                {
                    TrialUnit unit = btnArena.GetComponent<TrialUnit>();
                    if (unit != null)
                    {
                        unit.UpdateOpen(m_IsArenaUnlock);
                    }
                }

                //if (role_info.HasPvpTip) {
                //  role_info.HasPvpTip = false;
                //  CheckHasTip();
                //  PlayUnLock();
                //}
            }

            SetCoin();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
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
            if (unit.HasOpen)
            {//播放第一个为开放的（等级从大到小排的序）
                //unit.PlayUnLock();
                unit.UnLockFinish();
            }
        }
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
                store.ChangeStore(200007, "xuanzhang");
            }
        }
        UIManager.Instance.ShowWindowByName("Store");
    }
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

    //点击战神赛
    public void OnSinglePvpClick()
    {
        if (!m_IsMarsUnlook)
        {
            string chn_desc = StrDictionaryProvider.Instance.GetDictString(1200);
            LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", string.Format(chn_desc, m_MarsUnlockLevel), UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            return;
        }
        UIManager.Instance.ShowWindowByName("Mars");
    }
    //点击名人战
    public void OnArenaButtonClick()
    {
        if (!m_IsArenaUnlock)
        {
            string chn_desc = StrDictionaryProvider.Instance.GetDictString(1200);
            LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", string.Format(chn_desc, m_ArenaUnlockLevel), UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            return;
        }
        UIManager.Instance.ShowWindowByName("PartnerPvp");
    }
    public void OnCloseClick()
    {
        UIManager.Instance.HideWindowByName("PvPEntrance");
    }
}
