using UnityEngine;
using ArkCrossEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UISceneIntroduce : UnityEngine.MonoBehaviour
{
    private int m_SceneId = -1;
    public UILabel lblIntroduce1 = null;
    public UILabel lblSceneIndex = null;//章节序号
    public UILabel lblFightMy = null; //
    public UILabel lblFightRecommend = null;
    public UILabel lblCostPower = null;//体力消耗
    public UILabel lblTitle = null;
    public UILabel lblCurStamina = null;//当前体力
    public UILabel lblChallengeInfo = null;//剩余挑战次数

    public UnityEngine.GameObject lblAwardExp = null;
    public UnityEngine.GameObject lblAwardCoin = null;
    public UnityEngine.GameObject lblAwardItem = null;
    public UILabel lblAwardItemName = null;
    public UITexture texAwardItem = null;
    public UISprite spRank = null;
    public UISprite[] starArr = new UISprite[3];
    public UISceneAward uiSceneAward = null;
    private const string c_AshStar = "da-xing-xing2";
    private const string c_BrightStar = "da-xing-xing1";
    private List<object> m_EventList = new List<object>();
    private int m_CompleteCount = 0;
    private SubSceneType m_SubSceneType = SubSceneType.UnKnown;
    public UILabel wipeOutNum = null;//扫荡劵的数量
    public UISprite wipeBtn0;//扫荡10次按钮
    public UISprite wipeBtn1;//扫荡按钮
    public UILabel wipeBtnLabel = null;//扫荡n次
    public int maxStarNum = 3;
    public UnityEngine.GameObject wipePanel;
    public UILabel wipeNote = null;//扫荡规则文本
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
              foreach (object eo in m_EventList) {
                if (eo != null) {
                  ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eo);
                }
              }*/
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
            object obj = LogicSystem.EventChannelForGfx.Subscribe<int, int, SubSceneType>("ge_init_sceneintroduce", "ui", this.InitSceneIntroduce);
            if (null != obj) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null) m_EventList.Add(obj);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
    void OnDestroy()
    {
        try
        {
            UnSubscribe();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void InitSceneIntroduce(int sceneId, int grade, SubSceneType subType)
    {
        try
        {
            NGUITools.SetActive(wipePanel, false);
            NGUITools.SetActive(wipeBtn0.gameObject, false);
            NGUITools.SetActive(wipeBtn1.gameObject, false);
            if (subType == SubSceneType.Common)
            {
                m_SubSceneType = SubSceneType.Common;
                for (int i = 0; i < starArr.Length; ++i)
                {
                    if (starArr[i] != null) NGUITools.SetActive(starArr[i].gameObject, false);
                }
                RoleInfo role = LobbyClient.Instance.CurrentRole;
                if (role.SceneInfo.ContainsKey(sceneId))
                {
                    NGUITools.SetActive(wipePanel, true);
                    NGUITools.SetActive(wipeBtn0.gameObject, true);
                    NGUITools.SetActive(wipeBtn1.gameObject, true);
                }
            }
            else
            {
                m_SubSceneType = SubSceneType.Master;
                for (int i = 0; i < starArr.Length; ++i)
                {
                    if (starArr[i] != null) NGUITools.SetActive(starArr[i].gameObject, true);
                    if (i < grade)
                    {
                        if (starArr[i] != null) starArr[i].spriteName = c_BrightStar;
                    }
                    else
                    {
                        if (starArr[i] != null) starArr[i].spriteName = c_AshStar;
                    }
                }
                if (grade >= maxStarNum)
                {
                    NGUITools.SetActive(wipePanel, true);
                    NGUITools.SetActive(wipeBtn0.gameObject, true);
                    NGUITools.SetActive(wipeBtn1.gameObject, true);
                }
            }
            if (uiSceneAward != null) uiSceneAward.ShowAwardInfo(sceneId, subType, grade);
            m_SceneId = sceneId;
            Data_SceneConfig sceneCfg = SceneConfigProvider.Instance.GetSceneConfigById(m_SceneId);
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            if (sceneCfg != null)
            {

                SetName(sceneCfg.m_SceneName);
                SetRecommendFight(sceneCfg.m_RecommendFighting);
                SetCostStatima(sceneCfg.m_CostStamina);
                if (lblSceneIndex != null) lblSceneIndex.text = (1 + sceneCfg.m_Order).ToString();
                string des = sceneCfg.m_SceneDescription.Replace("[\\n]", "\n");
                if (lblIntroduce1 != null) lblIntroduce1.text = des;
                if (role_info != null)
                    SetFightingScore((int)role_info.FightingScore);
                if (lblCurStamina != null) lblCurStamina.text = role_info.CurStamina.ToString();
                //设置掉落数据
                Data_SceneDropOut dropCfg = SceneConfigProvider.Instance.GetSceneDropOutById(sceneCfg.m_DropId);
                if (dropCfg != null)
                {
                    DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Scene_Award, lblAwardExp, DFMItemIconUtils.Instance.m_Exp, dropCfg.m_Exp);
                    DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Scene_Award, lblAwardCoin, DFMItemIconUtils.Instance.m_Money, dropCfg.m_GoldSum);

                    SetAwardItem(dropCfg.GetRewardItemByHeroId(role_info.HeroId), dropCfg.m_ItemCountList);
                }
            }
            InitWipeNum();
            InitBtnName();
        }
        catch (Exception ex)
        {
            LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // 初始化扫荡劵
    public void InitWipeNum()
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        if (null != role)
        {
            ItemDataInfo info = role.GetItemData(ItemConfigProvider.Instance.GetSweepStageItemId(), 0);
            if (null != info)
            {
                wipeOutNum.text = info.ItemNum.ToString();
            }
            else
            {
                wipeOutNum.text = "0";
            }
        }
        //显示剩余次数
        if (m_SubSceneType == SubSceneType.Master)
        {
            if (lblChallengeInfo != null && lblChallengeInfo.transform.parent != null)
                NGUITools.SetActive(lblChallengeInfo.transform.parent.gameObject, true);
            int complete_count = role.GetCompletedSceneCount(m_SceneId);
            m_CompleteCount = complete_count;
            complete_count = complete_count > 3 ? 3 : complete_count;
            if (lblChallengeInfo != null)
                lblChallengeInfo.text = (3 - complete_count) + " / 3";
        }
        else
        {
            if (lblChallengeInfo != null && lblChallengeInfo.transform.parent != null)
                NGUITools.SetActive(lblChallengeInfo.transform.parent.gameObject, false);
        }

    }
    // 初始化扫荡按钮 文字
    void InitBtnName()
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        string chn_desc = "";
        chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(903);
        string wipeStr = "";
        if (m_SubSceneType == SubSceneType.Common)
        {
            chn_desc = string.Format(chn_desc, 10);
            wipeStr = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(905);
        }
        else if (m_SubSceneType == SubSceneType.Master)
        {
            int complete_count = role.GetCompletedSceneCount(m_SceneId);
            complete_count = complete_count > 3 ? 3 : complete_count;
            chn_desc = string.Format(chn_desc, 3);
            wipeStr = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(906);
        }
        if (wipeBtnLabel != null)
        {
            wipeBtnLabel.text = chn_desc;
        }
        if (wipeNote != null)
        {
            wipeNote.text = wipeStr;
        }
    }
    public void SetAwardItem(List<int> items, List<int> counts)
    {
        if (items == null || counts == null) return;
        if (items.Count <= 0 || counts.Count <= 0) return;
        int itemId = items[0];
        int itemCount = counts[0];
        DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Scene_Award2, lblAwardItem, itemId, itemCount);
        ItemConfig itemCfg = ItemConfigProvider.Instance.GetDataById(itemId);
        if (itemCfg != null)
        {
            if (texAwardItem != null)
            {
                UISceneIntroduceSlot scriptSlot = texAwardItem.GetComponent<UISceneIntroduceSlot>();
                if (scriptSlot != null) scriptSlot.SetId(itemId);
            }
            if (lblAwardItemName != null) lblAwardItemName.text = itemCfg.m_ItemName;
        }
    }

    //进入副本
    public void OnChallengeBtnClick()
    {
        if (m_CompleteCount >= 3 && m_SubSceneType == SubSceneType.Master)
        {
            string chn_desc = StrDictionaryProvider.Instance.GetDictString(308);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", chn_desc, "OK", null, null, null, false);
            return;
        }
        //判断是否有匹配
        if (WorldSystem.Instance.WaitMatchSceneId > 0)
        {
            Data_SceneConfig config = SceneConfigProvider.Instance.GetSceneConfigById(WorldSystem.Instance.WaitMatchSceneId);
            string str = ArkCrossEngine.StrDictionaryProvider.Instance.Format(852, config.m_SceneName);
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", str, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
            return;
        }
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        Data_SceneConfig config_data = SceneConfigProvider.Instance.GetSceneConfigById(m_SceneId);
        if (null != role_info && -1 != m_SceneId && null != config_data)
        {
            if (role_info.CurStamina >= config_data.m_CostStamina)
            {
                LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
            }
        }
        if (m_SceneId != -1)
        {
            ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_select_scene", "lobby", m_SceneId);
        }
        else
        {
            Debug.LogError("sceneId is -1!!");
        }
    }
    //扫荡事件
    public void OnWipeoutBtnClick()
    {
        if (m_SubSceneType == SubSceneType.Master)
        {

        }
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        if (null != role)
        {
            ItemDataInfo info = role.GetItemData(ItemConfigProvider.Instance.GetSweepStageItemId(), 0);
            if (null != info)
            {
                Data_SceneConfig sceneCfg = SceneConfigProvider.Instance.GetSceneConfigById(m_SceneId);
                if (role.CurStamina < sceneCfg.m_CostStamina)
                {
                    SendDialog(907, 4, null);
                    return;
                }
                if (info.ItemNum < 1)
                {
                    SendDialog(901, 4, null);
                    return;
                }
                if (m_SubSceneType == SubSceneType.Common)
                {
                    if (info.ItemNum >= 1)
                    {
                        LogicSystem.PublishLogicEvent("ge_sweep_stage", "lobby", m_SceneId, 1);
                        return;
                    }
                }
                else if (m_SubSceneType == SubSceneType.Master)
                {
                    if (info.ItemNum >= 1)
                    {
                        int complete_count = role.GetCompletedSceneCount(m_SceneId);
                        if (complete_count < 3)
                        {
                            LogicSystem.PublishLogicEvent("ge_sweep_stage", "lobby", m_SceneId, 1);
                            return;
                        }
                        else
                        {
                            SendDialog(902, 4, null);
                            return;
                        }
                    }
                }
            }
            else
            {
                SendDialog(901, 4, null);
            }
        }
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
    //扫荡10事件
    public void OnWipeoutTenBtnClick()
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        if (null != role)
        {
            ItemDataInfo info = role.GetItemData(ItemConfigProvider.Instance.GetSweepStageItemId(), 0);
            Data_SceneConfig sceneCfg = SceneConfigProvider.Instance.GetSceneConfigById(m_SceneId);
            if (null != info)
            {
                if (m_SubSceneType == SubSceneType.Common)
                {
                    if (role.CurStamina < 10 * sceneCfg.m_CostStamina)
                    {
                        SendDialog(907, 4, null);
                        return;
                    }
                    if (info.ItemNum < 10)
                    {
                        SendDialog(901, 4, null);
                        return;
                    }
                    LogicSystem.PublishLogicEvent("ge_sweep_stage", "lobby", m_SceneId, 10);
                }
                else if (m_SubSceneType == SubSceneType.Master)
                {
                    if (role.CurStamina < 3 * sceneCfg.m_CostStamina)
                    {
                        SendDialog(907, 4, null);
                        return;
                    }
                    if (info.ItemNum < 3)
                    {
                        SendDialog(901, 4, null);
                        return;
                    }
                    int complete_count = role.GetCompletedSceneCount(m_SceneId);
                    if (complete_count <= 0)
                    {
                        LogicSystem.PublishLogicEvent("ge_sweep_stage", "lobby", m_SceneId, 3);
                    }
                    else
                    {
                        SendDialog(902, 4, null);
                    }
                }
            }
            else
            {
                SendDialog(901, 4, null);
            }
        }
    }
    //
    public void OnCloseBtnClick()
    {
        UIManager.Instance.HideWindowByName("SceneIntroduce");
        //UIManager.Instance.ShowWindowByName("SceneSelect");
    }
    //
    public void OnBackgroundButtonClick()
    {
        UIManager.Instance.HideWindowByName("SceneIntroduce");
    }
    public void SetName(string name)
    {
        if (lblTitle != null)
            lblTitle.text = "" + name + "";
    }
    //设置玩家当前的战斗力
    private void SetFightingScore(int value)
    {
        if (lblFightMy != null)
        {
            lblFightMy.text = "" + value + "";
        }
    }
    //设置推荐战力
    private void SetRecommendFight(int value)
    {
        if (lblFightRecommend != null)
        {
            lblFightRecommend.text = "" + value + "";
        }
    }
    //设置体力消耗
    private void SetCostStatima(int value)
    {
        if (lblCostPower != null)
        {
            lblCostPower.text = "" + value + "";
        }
    }
    public int SceneId
    {
        get { return m_SceneId; }
        set { m_SceneId = value; }
    }
}
