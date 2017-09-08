using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class UIPartnerPvpPanel : UnityEngine.MonoBehaviour
{

    public UILabel lblMyRanking;
    public UILabel lblRankingAwardMoney;
    public UILabel lblRankingAwardDiamond;
    public UILabel lblRankingAwardItem;
    public UITexture texRankingAwardItem;
    public UnityEngine.GameObject goAwardItem;
    public UILabel lblAwardTime;//奖励发放时间
    public UILabel lblPvPNotice;//公告
    public UIPartnerPvpRightInfo uiPartnerPvpRightInfo;
    private List<object> m_EventList = new List<object>();
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
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Awake()
    {
        try
        {
            /*需要先调用InitArenaBaseInfo（）*/
            if (uiPartnerPvpRightInfo != null) uiPartnerPvpRightInfo.InitArenaBaseInfo();
            object obj = LogicSystem.EventChannelForGfx.Subscribe("arena_info_result", "arena", HandleArenaInfoResult);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe("match_group_result", "arena", HandleRefreshMatchGroupInfo);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int, int, int>("buy_fight_count_result", "arena", HandleBuyFightCount);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int>("start_challenge_result", "arena", HandleStartChallengeResult);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null) m_EventList.Add(obj);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Use this for initialization
    void Start()
    {
        try
        {
            QueryMatchGroup();
            //CYGTConnector.ShowCYGTSDK();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OnEnable()
    {
        try
        {
            if (NeedQueryBaseInfo())
            {
                LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
                LogicSystem.PublishLogicEvent("query_arena_info", "arena");
                if (uiPartnerPvpRightInfo != null)
                {
                    uiPartnerPvpRightInfo.RefreshAllPartners();
                }
            }
            else
            {
                UpdatePanelInfo();
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
    private void HandleArenaInfoResult()
    {
        LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, false);
        SetSelfBaseInfo();
    }
    //更新信息
    public void UpdatePanelInfo()
    {
        SetSelfBaseInfo();
        if (uiPartnerPvpRightInfo != null)
        {
            uiPartnerPvpRightInfo.RefreshAllPartners();
        }
    }
    //设置玩家自己的基本信息
    private void SetSelfBaseInfo()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null && role_info.ArenaStateInfo != null)
        {
            if (lblMyRanking != null)
            {
                string desc = role_info.ArenaStateInfo.Rank.ToString();
                if (role_info.ArenaStateInfo.Rank == -1)
                {
                    //-1表示未排名
                    desc = StrDictionaryProvider.Instance.GetDictString(1105);
                }
                lblMyRanking.text = desc;
            }
            ArenaPrizeConfig prizeCfg = GetPrizeCfg(role_info.ArenaStateInfo.Rank);
            if (prizeCfg != null)
            {
                if (lblRankingAwardDiamond != null) lblRankingAwardDiamond.text = prizeCfg.Gold.ToString();
                if (lblRankingAwardMoney != null) lblRankingAwardMoney.text = prizeCfg.Money.ToString();
                SetAwardItem(prizeCfg.Items);
            }
            else
            {
                if (lblRankingAwardDiamond != null) lblRankingAwardDiamond.text = "0";
                if (lblRankingAwardMoney != null) lblRankingAwardMoney.text = "0";
                if (goAwardItem != null) NGUITools.SetActive(goAwardItem, false);
            }
        }
        ArenaBaseConfig arenaBaseCfg = ArenaConfigProvider.Instance.GetBaseConfigById(1);
        if (arenaBaseCfg != null)
        {
            string chn_des = StrDictionaryProvider.Instance.GetDictString(1106);
            string desc = string.Format(chn_des, arenaBaseCfg.PrizePresentTime.Hour, arenaBaseCfg.PrizePresentTime.Minutes);
            if (lblAwardTime != null) lblAwardTime.text = desc;
        }
        if (uiPartnerPvpRightInfo != null)
        {
            uiPartnerPvpRightInfo.RefreshArenaBaseInfo(arenaBaseCfg);
            uiPartnerPvpRightInfo.RefreshPlayedPartner();
        }
    }
    //获得当前等级的奖励
    private ArenaPrizeConfig GetPrizeCfg(int ranking)
    {
        MyDictionary<int, object> prizeCfgDict = ArenaConfigProvider.Instance.PrizeConfig.GetData();
        List<int> prizeIdList = new List<int>();
        foreach (int prizeId in prizeCfgDict.Keys)
        {
            prizeIdList.Add(prizeId);
        }
        prizeIdList.Sort();
        int cfgId = -1;
        for (int index = 0; index < prizeIdList.Count; ++index)
        {
            if (index == prizeIdList.Count - 1 && ranking >= prizeIdList[index])
            {
                cfgId = prizeIdList[index];
                break;
            }
            else
            {
                if (ranking >= prizeIdList[index] && ranking < prizeIdList[index + 1])
                {
                    cfgId = prizeIdList[index];
                    break;
                }
            }
        }
        ArenaPrizeConfig prizeCfg = ArenaConfigProvider.Instance.PrizeConfig.GetDataById(cfgId);
        return prizeCfg;
    }
    private void SetAwardItem(List<PrizeItemConfig> items)
    {
        if (items == null || items.Count <= 0)
        {
            NGUITools.SetActive(goAwardItem, false);
        }
        else
        {
            PrizeItemConfig prizeItemCfg = items[0];
            if (prizeItemCfg != null)
            {
                if (lblRankingAwardItem != null) lblRankingAwardItem.text = "X " + prizeItemCfg.ItemNum;
                int itemId = prizeItemCfg.ItemId;
                ItemConfig itemCfg = ItemConfigProvider.Instance.GetDataById(itemId);
                if (itemCfg == null) return;
                UnityEngine.Texture tex = CrossObjectHelper.TryCastObject<UnityEngine.Texture>(ResourceSystem.GetSharedResource(itemCfg.m_ItemTrueName));
                if (texRankingAwardItem != null)
                {
                    if (tex != null)
                    {
                        texRankingAwardItem.mainTexture = tex;
                    }
                    NGUITools.SetActive(goAwardItem, true);
                }
            }
        }
    }
    //刷新匹配玩家
    private void HandleRefreshMatchGroupInfo()
    {
        try
        {
            LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, false);
            if (uiPartnerPvpRightInfo != null) uiPartnerPvpRightInfo.RefreshMatchPlayerInfo(true, true);
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //
    private void HandleBuyFightCount(int result, int currentTime, int curFightCount)
    {
        try
        {
            LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, false);
            if (result == (int)ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed)
            {
                ArenaBaseConfig arenaBaseCfg = ArenaConfigProvider.Instance.GetBaseConfigById(1);
                if (uiPartnerPvpRightInfo != null) uiPartnerPvpRightInfo.RefreshArenaBaseInfo(arenaBaseCfg);
            }
            else
            {
                string CHN = StrDictionaryProvider.Instance.GetDictString(1114);
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", CHN, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //
    private void HandleStartChallengeResult(int result)
    {
        try
        {
            if (result != (int)ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed)
            {
                string CHN = "";
                if (result == -1)
                {
                    CHN = StrDictionaryProvider.Instance.GetDictString(1120);
                }
                else
                {
                    CHN = StrDictionaryProvider.Instance.GetDictString(1118);
                }
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", CHN, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //点击名人堂按钮
    public void OnHOFButtonClick()
    {
        UnityEngine.GameObject item = UIManager.Instance.GetWindowGoByName("Ranking");
        if (item != null)
        {
            item.GetComponent<Ranking>().OpenType(2);
        }
    }
    //请求玩家匹配数据
    private void QueryMatchGroup()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null && role_info.ArenaStateInfo != null)
        {
            if (role_info.ArenaStateInfo.IsNeedQueryMatchGroup())
            {
                UIDataCache.Instance.curPlayerFightingScore = role_info.FightingScore;
                LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
                LogicSystem.PublishLogicEvent("query_match_group", "arena");
                return;
            }
            ArenaStateInfo arena_info = role_info.ArenaStateInfo;
            if (arena_info.MatchGroups.Count <= 0)
            {
                LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
                LogicSystem.PublishLogicEvent("query_match_group", "arena");
                return;
            }
            MatchGroup match_group = arena_info.MatchGroups[0];
            if (match_group == null)
            {
                LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
                LogicSystem.PublishLogicEvent("query_match_group", "arena");
                return;
            }
            if (match_group != null && match_group.One == null)
            {
                LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
                LogicSystem.PublishLogicEvent("query_match_group", "arena");
                return;
            }
        }
        //打完战神赛需要刷新一次
        bool needRefresh = (UIDataCache.Instance.prevSceneType == SceneSubTypeEnum.TYPE_PVAP);
        if (uiPartnerPvpRightInfo != null) uiPartnerPvpRightInfo.RefreshMatchPlayerInfo(needRefresh, false);
    }
    /*是否需要请求刷新数据*/
    private bool NeedQueryBaseInfo()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null && role_info.ArenaStateInfo != null)
            return role_info.ArenaStateInfo.IsNeedQueryInfo();
        return false;
    }
}
