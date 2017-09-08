using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class UIPartnerPvpRightInfo : UnityEngine.MonoBehaviour
{
    private enum Button0Type
    {
        Change,
        Buy
    }
    public UnityEngine.GameObject goPartnerSlot;
    public UnityEngine.GameObject goPartnerSlotContainer;

    public UnityEngine.GameObject goAllPartners;//伙伴界面
    public UnityEngine.GameObject goMatchPlayers;//匹配玩家界面
    public UnityEngine.GameObject goButtonConfirm;//确定按钮
    public UnityEngine.GameObject goButtonChange;//调整按钮

    public UILabel lblTotleFighting;
    public UILabel lblCanChallengeCount;//剩余挑战次数
    public UILabel lblFightCountHint;//剩余挑战次数（0次 设置为红色）
    public UILabel lblCanBuyCount;//剩余购买次数
    public UILabel lblDiamondOfBuyCount;//购买挑战次数所需要的钻石
    public UILabel lblChallengeCd;//挑战CD

    public UIButton btnChangeBtn;
    public UILabel lblChangeBtn;//换一批--购买
    public UnityEngine.GameObject goBuyCount;//购买的钻石显示
    public UnityEngine.GameObject goBuyDesc;//购买次数提示
    public UnityEngine.GameObject goCd;//倒计时提示

    private const int c_PartnersNumInPvp = 3;
    private const int c_ArenaPlayerGroupNum = 3;//每一组的人数
    public UIPartnerSlot[] partnersInPvp = new UIPartnerSlot[c_PartnersNumInPvp];
    private int[] UnlockLevelArr = new int[3] { 1, 10, 20 };
    private int m_MaxBattleCount;//最大挑战次数
    private int m_MaxBuyCount;//最多购买次数
    private int m_LeftBattleCount = 1;//剩余挑战次数
    private long m_BattleCd = 2;//秒
    private float m_BattleCdDelta = 0.0f;
    public UIArenaPlayer[] arenaPlayerArr = new UIArenaPlayer[c_ArenaPlayerGroupNum];
    private List<UIPartnerSlot> m_AllPartners = new List<UIPartnerSlot>();//玩家拥有的伙伴

    private const int c_Refresh = 3;
    private static int m_CurrentChallengeGroupIndex = 0;
    private bool m_CanChangePlayedPartner = false;
    private bool m_IsChallengeButtonEnable = true;
    private bool m_IsInCd = true;
    private Button0Type m_Button0Type = Button0Type.Change;
    private bool m_IsCanBuyFightCount = true;//是否可以购买
                                             // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            UpdateBattleCd();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OnEnable()
    {
        try
        {
            ResetPageStyle();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //
    public void InitArenaBaseInfo()
    {
        ArenaBaseConfig cfg = ArenaConfigProvider.Instance.GetBaseConfigById(1);
        UnlockLevelArr[0] = cfg.MaxParterLimit[0];
        UnlockLevelArr[1] = cfg.MaxParterLimit[2];
        UnlockLevelArr[2] = cfg.MaxParterLimit[4];
        m_MaxBattleCount = cfg.MaxBattleCount;
        m_BattleCd = cfg.BattleCd / 1000;
    }
    //刷新基本信息
    public void RefreshArenaBaseInfo(ArenaBaseConfig cfg)
    {
        if (cfg == null) return;
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info == null || role_info.ArenaStateInfo == null) return;
        int leftBattleCount = role_info.ArenaStateInfo.LeftFightCount;
        m_LeftBattleCount = leftBattleCount;
        if (lblCanChallengeCount != null) lblCanChallengeCount.text = leftBattleCount + "/" + m_MaxBattleCount;
        if (leftBattleCount > 0)
        {
            //倒计时
            if (lblFightCountHint != null) lblFightCountHint.color = UnityEngine.Color.white;
            if (lblCanChallengeCount != null) lblCanChallengeCount.color = UnityEngine.Color.white;
            ChangeButton0State(Button0Type.Change);
            CanBuyFightCount(true);
        }
        else
        {
            if (lblFightCountHint != null) lblFightCountHint.color = UnityEngine.Color.red;
            if (lblCanChallengeCount != null) lblCanChallengeCount.color = UnityEngine.Color.red;
            ChangeButton0State(Button0Type.Buy);
            SetFightCountBuyTime();
            EnableChallengeButton(false);
        }
        m_IsInCd = true;
    }
    //设置可购买信息
    private void SetFightCountBuyTime()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info == null || role_info.ArenaStateInfo == null) return;
        int CurFightCountBuyTime = role_info.ArenaStateInfo.CurFightCountBuyTime;
        int playerVipLevel = role_info.Vip;
        MyDictionary<int, object> countCfgDic = ArenaConfigProvider.Instance.BuyFightCountConfig.GetData();
        int maxCount = 0;
        foreach (int times in countCfgDic.Keys)
        {
            if (times > maxCount && countCfgDic[times] != null)
            {
                ArenaBuyFightCountConfig cfg = (ArenaBuyFightCountConfig)countCfgDic[times];
                if (cfg.RequireVipLevel <= playerVipLevel)
                    maxCount = times;
            }
        }
        int leftFightCountBuyTime = maxCount - CurFightCountBuyTime;
        leftFightCountBuyTime = leftFightCountBuyTime < 0 ? 0 : leftFightCountBuyTime;
        if (lblCanBuyCount != null) lblCanBuyCount.text = leftFightCountBuyTime + "/" + maxCount;
        ArenaBuyFightCountConfig countCfg = ArenaConfigProvider.Instance.BuyFightCountConfig.GetDataById(CurFightCountBuyTime + 1);
        if (countCfg != null)
        {
            if (lblDiamondOfBuyCount != null) lblDiamondOfBuyCount.text = countCfg.Cost.ToString();
            if (leftFightCountBuyTime > 0 && countCfg.Cost <= role_info.Gold)
            {
                CanBuyFightCount(true);
            }
            else
            {
                CanBuyFightCount(false);
            }
        }
        else
        {
            if (lblDiamondOfBuyCount != null) lblDiamondOfBuyCount.text = "0";
            CanBuyFightCount(false);
        }
    }
    //设置是否可以购买标记
    private void CanBuyFightCount(bool enable)
    {
        m_IsCanBuyFightCount = enable;
    }
    //换一批和购买之间的转换
    private void ChangeButton0State(Button0Type btn_type)
    {
        bool isChange = btn_type == Button0Type.Change;
        if (goBuyCount != null) NGUITools.SetActive(goBuyCount, !isChange);
        if (goBuyDesc != null) NGUITools.SetActive(goBuyDesc, !isChange);
        if (goCd != null) NGUITools.SetActive(goCd, isChange);
        m_Button0Type = btn_type;
        string chn_des = "";
        if (isChange)
        {
            //换一批
            chn_des = StrDictionaryProvider.Instance.GetDictString(1110);
        }
        else
        {
            chn_des = StrDictionaryProvider.Instance.GetDictString(1109);
        }
        if (lblChangeBtn != null) lblChangeBtn.text = chn_des;
    }
    //更新CD
    private void UpdateBattleCd()
    {
        if (lblChallengeCd != null && m_IsInCd)
        {
            if (m_BattleCdDelta <= 0)
            {
                RoleInfo role_info = LobbyClient.Instance.CurrentRole;
                if (role_info != null && role_info.ArenaStateInfo != null)
                {
                    DateTime last_time = role_info.ArenaStateInfo.LastBattleServerTime;
                    DateTime current_time = DateTime.Now;
                    long delta_time = (long)(((TimeSpan)(current_time - last_time)).TotalSeconds);
                    long left_time = m_BattleCd - delta_time;
                    if (left_time < 0)
                    {
                        m_IsInCd = false;
                        if (m_LeftBattleCount > 0)
                        {
                            //有挑战次数
                            string chn_des = StrDictionaryProvider.Instance.GetDictString(1108);
                            lblChallengeCd.text = chn_des;
                            EnableChallengeButton(true);
                        }
                        else
                        {
                            string chn_des = StrDictionaryProvider.Instance.GetDictString(1115);
                            lblChallengeCd.text = chn_des;
                            EnableChallengeButton(false);
                        }
                        return;
                    }
                    EnableChallengeButton(false);
                    m_IsInCd = true;
                    int hour = (int)left_time / 3600;
                    int minute = (int)left_time % 3600 / 60;
                    int second = (int)left_time % 60;
                    string desc = "";
                    if (hour > 0)
                    {
                        desc = string.Format("{0}:{1:d2}:{2:d2}", hour, minute, second);
                    }
                    else
                    {
                        desc = string.Format("{0:d2}:{1:d2}", minute, second);
                    }
                    lblChallengeCd.text = desc;
                    m_BattleCdDelta = 1f;
                }
            }
            else
            {
                m_BattleCdDelta -= UnityEngine.Time.deltaTime;
            }
        }
    }
    //设置挑战按钮的状态
    private void EnableChallengeButton(bool enable)
    {
        if (m_IsChallengeButtonEnable == enable) return;
        m_IsChallengeButtonEnable = enable;
        for (int index = 0; index < arenaPlayerArr.Length; ++index)
        {
            if (arenaPlayerArr[index] != null)
                arenaPlayerArr[index].EnableChallengeButton(enable);
        }
    }
    //刷新伙伴阵容
    public void RefreshPlayedPartner()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            List<int> fightPartners = role_info.ArenaStateInfo.FightPartners;
            if (fightPartners == null) return;
            for (int index = 0; index < partnersInPvp.Length; ++index)
            {
                if (partnersInPvp[index] != null)
                {
                    if (role_info.Level < UnlockLevelArr[index])
                    {
                        partnersInPvp[index].LockSlot(UnlockLevelArr[index]);
                    }
                    else
                    {
                        partnersInPvp[index].UnlockSlot();
                        if (fightPartners.Count > index)
                        {
                            partnersInPvp[index].InitPartnerInfo(fightPartners[index]);
                        }
                        else
                        {
                            //已解锁，但是没这么多伙伴
                            partnersInPvp[index].NotHasPartner();
                        }
                    }
                }
            }
        }
        if (lblTotleFighting != null) lblTotleFighting.text = CalculateTotleFighting().ToString();
    }
    //刷新玩家
    public void RefreshMatchPlayerInfo(bool isRefresh, bool resetIndex)
    {
        if (!isRefresh)
        {
            //从副本（除战神赛）中回到主城不需要刷新拼配伙伴
            if (m_CurrentChallengeGroupIndex > 0 && m_CurrentChallengeGroupIndex <= 3)
                m_CurrentChallengeGroupIndex--;
        }
        if (resetIndex) m_CurrentChallengeGroupIndex = 0;//从服务器取的新数据
        if (m_CurrentChallengeGroupIndex == 3)
        {
            //缓存数据已更新完  需要重新发送消息更新缓存数据
            LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
            m_CurrentChallengeGroupIndex = 0;
            LogicSystem.PublishLogicEvent("query_match_group", "arena");
            return;
        }
        else
        {
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            if (role_info != null && role_info.ArenaStateInfo != null)
            {
                ArenaStateInfo arena_info = role_info.ArenaStateInfo;
                if (m_CurrentChallengeGroupIndex < arena_info.MatchGroups.Count)
                {
                    //默认arena_info.MatchGroups一直顺序存放着数据且数据正确
                    MatchGroup match_group = arena_info.MatchGroups[m_CurrentChallengeGroupIndex];
                    if (match_group == null) return;
                    if (arenaPlayerArr.Length == 3)
                    {
                        if (arenaPlayerArr[0] != null) arenaPlayerArr[0].InitPlayerInfo(match_group.One);
                        if (arenaPlayerArr[1] != null) arenaPlayerArr[1].InitPlayerInfo(match_group.Two);
                        if (arenaPlayerArr[2] != null) arenaPlayerArr[2].InitPlayerInfo(match_group.Three);
                    }
                }
                m_CurrentChallengeGroupIndex++;
            }
        }
    }
    //刷新自己拥有的伙伴
    public void RefreshAllPartners()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null && role_info.PartnerStateInfo != null)
        {
            List<PartnerInfo> allPartners = role_info.PartnerStateInfo.GetAllPartners();
            if (allPartners == null) return;
            for (int index = 0; index < allPartners.Count; ++index)
            {
                UIPartnerSlot partner_slot = TryGetPartnerItem(index);
                if (partner_slot != null) partner_slot.InitPartnerInfo(allPartners[index]);
                bool actived = IsInPlayedParnters(allPartners[index].Id);
                partner_slot.SetStoragePartnerPlayed(actived);
            }
            bool setActive = false;
            if (!NGUITools.GetActive(goAllPartners))
            {
                //如果goAllPartners不可见，Reposition()将不起作用，需要将其设为可见
                NGUITools.SetActive(goAllPartners, true);
                setActive = true;
            }
            if (goPartnerSlotContainer != null)
            {
                UITable table = goPartnerSlotContainer.GetComponent<UITable>();
                if (table != null)
                {
                    table.Reposition();
                }
            }
            if (setActive)
            {
                //如果设置为可见，这里需要还原回去
                NGUITools.SetActive(goAllPartners, false);
            }
        }
    }
    //
    private UIPartnerSlot TryGetPartnerItem(int index)
    {
        if (index < m_AllPartners.Count && m_AllPartners[index] != null)
        {
            return m_AllPartners[index];
        }
        if (index < m_AllPartners.Count && m_AllPartners[index] == null) m_AllPartners.Remove(m_AllPartners[index]);
        if (goPartnerSlotContainer != null && goPartnerSlot != null)
        {
            UnityEngine.GameObject goChild = NGUITools.AddChild(goPartnerSlotContainer, goPartnerSlot);
            if (goChild != null)
            {
                UIPartnerSlot partner_item = goChild.GetComponent<UIPartnerSlot>();
                if (partner_item != null)
                {
                    m_AllPartners.Add(partner_item);
                    return partner_item;
                }
            }
        }
        return null;
    }
    //伙伴出战
    public bool PartnerPlayed(int partnerId)
    {
        for (int index = 0; index < partnersInPvp.Length; ++index)
        {
            UIPartnerSlot slot = partnersInPvp[index];
            if (slot != null && slot.GetPartnerId() == -1 && slot.IsUnlock())
            {
                slot.InitPartnerInfo(partnerId);
                if (lblTotleFighting != null) lblTotleFighting.text = CalculateTotleFighting().ToString();
                return true;
            }
        }
        return false;
    }
    //取消出战
    public void CancelPartnerPlayed(int partnerId, UIParternSlotType slotType)
    {
        if (slotType == UIParternSlotType.SettingSlot)
        {
            UIPartnerSlot slot = GetPartnerSlotFromSetting(partnerId);
            if (slot != null) slot.CancelSettingPartnerPlayed();
        }
        else
        {
            UIPartnerSlot slot = GetPartnerSlotFromStorage(partnerId);
            if (slot != null) slot.SetStoragePartnerPlayed(false);
        }
        if (lblTotleFighting != null) lblTotleFighting.text = CalculateTotleFighting().ToString();
    }
    //
    private UIPartnerSlot GetPartnerSlotFromSetting(int partnerId)
    {
        for (int index = 0; index < partnersInPvp.Length; ++index)
        {
            if (partnersInPvp[index] != null && partnersInPvp[index].GetPartnerId() == partnerId)
                return partnersInPvp[index];
        }
        return null;
    }
    //
    private UIPartnerSlot GetPartnerSlotFromStorage(int partnerId)
    {
        for (int index = 0; index < m_AllPartners.Count; ++index)
        {
            if (m_AllPartners[index] != null && m_AllPartners[index].GetPartnerId() == partnerId)
                return m_AllPartners[index];
        }
        return null;
    }
    //点击规则说明
    public void OnRuleButtonClick()
    {
        //UIManager.Instance.ShowWindowByName("PPVPRuleIntro");
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("PPVPRuleIntro");
        if (go != null)
        {
            UIRuleIntro ruleIntro = go.GetComponent<UIRuleIntro>();
            if (ruleIntro != null)
                ruleIntro.ShowIntro();
        }
    }
    //点击对战记录
    public void OnRecordButtonClick()
    {
        UIManager.Instance.ShowWindowByName("Record");
    }
    //点击换一批
    public void OnRefreshPlayerClick()
    {
        if (m_Button0Type == Button0Type.Change)
        {
            //刷新
            RefreshMatchPlayerInfo(true, false);
        }
        else
        {
            //购买
            if (m_IsCanBuyFightCount)
            {
                LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
                LogicSystem.PublishLogicEvent("buy_fight_count", "arena");
            }
            else
            {
                //提示不可以购买
                string chn_desc = StrDictionaryProvider.Instance.GetDictString(1119);
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);

            }
        }
    }
    //点击调整按钮
    public void OnChangeButtonClick()
    {
        if (goMatchPlayers != null) NGUITools.SetActive(goMatchPlayers, false);
        if (goAllPartners != null) NGUITools.SetActive(goAllPartners, true);
        if (goButtonChange != null) NGUITools.SetActive(goButtonChange, false);
        if (goButtonConfirm != null) NGUITools.SetActive(goButtonConfirm, true);
        m_CanChangePlayedPartner = true;
    }
    //点击确定按钮
    public void OnConfirmButtonClick()
    {
        if (goMatchPlayers != null) NGUITools.SetActive(goMatchPlayers, true);
        if (goAllPartners != null) NGUITools.SetActive(goAllPartners, false);
        if (goButtonChange != null) NGUITools.SetActive(goButtonChange, true);
        if (goButtonConfirm != null) NGUITools.SetActive(goButtonConfirm, false);
        m_CanChangePlayedPartner = false;
        //需要发送消息：
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null && role_info.ArenaStateInfo != null)
        {
            List<int> partners = role_info.ArenaStateInfo.FightPartners;
            List<int> playedPartners = GetPlayedParnters();
            if (partners.Count != playedPartners.Count)
            {
                LogicSystem.PublishLogicEvent("change_partners", "arena", playedPartners.ToArray());
                return;
            }
            if (partners != null)
            {
                for (int index = 0; index < partners.Count; ++index)
                {
                    if (!IsInPlayedParnters(partners[index]))
                    {
                        LogicSystem.PublishLogicEvent("change_partners", "arena", playedPartners.ToArray());
                        break;
                    }
                }
            }
        }
    }
    private void ResetPageStyle()
    {
        if (goMatchPlayers != null) NGUITools.SetActive(goMatchPlayers, true);
        if (goAllPartners != null) NGUITools.SetActive(goAllPartners, false);
        if (goButtonChange != null) NGUITools.SetActive(goButtonChange, true);
        if (goButtonConfirm != null) NGUITools.SetActive(goButtonConfirm, false);
        m_CanChangePlayedPartner = false;
    }
    //判断阵容是否有变化
    private bool IsInPlayedParnters(int partnerId)
    {
        for (int index = 0; index < partnersInPvp.Length; ++index)
        {
            if (partnersInPvp[index] != null && partnersInPvp[index].GetPartnerId() == partnerId)
                return true;
        }
        return false;
    }
    //
    private List<int> GetPlayedParnters()
    {
        List<int> ret = new List<int>();
        for (int index = 0; index < partnersInPvp.Length; ++index)
        {
            if (partnersInPvp[index] != null && partnersInPvp[index].GetPartnerId() != -1)
                ret.Add(partnersInPvp[index].GetPartnerId());
        }
        return ret;
    }
    private int CalculateTotleFighting()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        float totleFighting = 0f;
        if (role_info != null)
        {
            for (int index = 0; index < partnersInPvp.Length; ++index)
            {
                if (partnersInPvp[index] != null && partnersInPvp[index].GetPartnerId() != -1)
                {
                    totleFighting += partnersInPvp[index].GetFighting();
                }
            }
            totleFighting += role_info.FightingScore;
        }
        return UnityEngine.Mathf.FloorToInt(totleFighting);
    }
    public bool CanChangePlayedPartner()
    {
        return m_CanChangePlayedPartner;
    }
}
