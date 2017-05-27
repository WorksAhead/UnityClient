using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class UIPartnerPanel : UnityEngine.MonoBehaviour
{

    //伙伴基本信息
    public UILabel lblPartnerName = null;
    public UILabel lblPartnerFighting;
    public UILabel lblPartnerCd = null;
    public UILabel lblInheritAttack;
    public UILabel lblInheritDefence;
    public UISprite spPartnerPortrait;
    public UISprite spRankColor;
    public UILabel lblRankOffset;
    public UISprite spFightingCompare;//战力对比
    public UILabel lblFightingCompare;
    //属性加成
    private const int c_AppendAttrNumMax = 5;
    public UILabel[] lblAppendAttrArr = new UILabel[c_AppendAttrNumMax];
    //技能
    private const int c_SkillNumMax = 4;
    public UISprite[] spSkillLifts = new UISprite[c_SkillNumMax];
    public UISprite[] spSkillLocks = new UISprite[c_SkillNumMax];
    //点击按钮提示
    public UISprite spBiography;
    public UISprite spStrengthen;
    public UISprite spSkillLift;
    private UISprite m_SpPreviorsClick;//上一次点击的按钮
                                       //
    public UnityEngine.Color AshColor = UnityEngine.Color.gray;//用于灰化按钮
    public UnityEngine.Color WhiteColor = UnityEngine.Color.white;//恢复按钮原有颜色
    public UIButton btnPlayed = null;//出战按钮
    public UILabel lblUnderBtnPlayed;
    public UILabel lblPlayed = null;
    public UISprite spPlayed = null;
    public UIPartnerInfo uiPartnerInfo = null;
    public UIPartnerList uiPartnerList = null;
    public UnityEngine.GameObject goLeftButtons;
    public UnityEngine.GameObject goLeftInfo;
    public UnityEngine.GameObject goLeftTitle;
    //
    private int m_ActivePartnerId = -1;
    private int m_SelectedPartnerId = -1;//当前选中的伙伴Id
                                         // Use this for initialization
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
    void Start()
    {
        try
        {
            object obj = LogicSystem.EventChannelForGfx.Subscribe<int>("ge_partner_select_result", "ui", HandlerPartnerPlayed);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int>("ge_partner_upgrade_level_result", "ui", HandlerUpgradePartnerLevel);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int>("ge_partner_upgrade_skill_result", "ui", HandlerUpagradeSkillStage);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int>("ge_partner_compound_result", "ui", HandlerCompoundPartner);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_item_change", "item", CheckPartnerTip);
            if (obj != null) m_EventList.Add(obj);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void Awake()
    {
        try
        {
            object obj = LogicSystem.EventChannelForGfx.Subscribe("ge_refresh_partner", "ui", RefreshParnterList);
            if (obj != null) m_EventList.Add(obj);
            CheckPartnerTip();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnEnable()
    {
        try
        {
            SetPartnerInfo(m_SelectedPartnerId);
            RefreshParnterList();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //没有伙伴的时候关闭左侧界面
    private void EnableLeftPanel(bool enable)
    {
        if (goLeftButtons != null) NGUITools.SetActive(goLeftButtons, enable);
        if (goLeftInfo != null) NGUITools.SetActive(goLeftInfo, enable);
        if (goLeftTitle != null) NGUITools.SetActive(goLeftTitle, enable);
    }
    private void CheckPartnerTip()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;

        bool has = false;
        if (role_info != null)
        {
            List<PartnerInfo> partnerList = role_info.PartnerStateInfo.GetAllPartners();
            for (int i = 0; i < partnerList.Count; i++)
            {
                int partnerId = partnerList[i].Id;
                if (!role_info.PartnerStateInfo.IsHavePartner(partnerId))
                {//未拥有
                    PartnerConfig partner_cfg = PartnerConfigProvider.Instance.GetDataById(partnerId);
                    if (partner_cfg != null)
                    {
                        int fragId = partner_cfg.PartnerFragId;
                        int fragNeedNum = partner_cfg.PartnerFragNum;
                        int ownFragNum = GetItemNum(fragId);
                        if (ownFragNum >= fragNeedNum)
                        {//可召唤
                            has = true;
                        }
                    }
                }
                else
                {//已拥有
                    bool canUpgrade = false;
                    if (CheckCanUpgrade(partnerId))
                    {
                        has = true;
                        canUpgrade = true;
                    }
                    if (uiPartnerList != null)
                    {
                        uiPartnerList.SetItemTipActive(partnerId, canUpgrade);
                    }
                }
            }
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.Partner, has);
    }

    private bool CheckCanUpgrade(int partnerId)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        PartnerInfo partnerInfo = role_info.PartnerStateInfo.GetPartnerInfoById(partnerId);
        if (partnerInfo != null && partnerInfo.CurSkillStage < 4)
        {//未满级
            int ownItemNum = GetItemNum(partnerInfo.StageUpItemId);
            PartnerStageUpConfig stageUpCfg = PartnerStageUpConfigProvider.Instance.GetDataById(partnerInfo.CurSkillStage);
            int itemNeedNum = 0;
            if (stageUpCfg != null)
            {
                itemNeedNum = stageUpCfg.ItemCost;
                if (ownItemNum >= itemNeedNum)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //获取itemID的物品数量
    private int GetItemNum(int itemId)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null && role_info.Items != null)
        {
            for (int i = 0; i < role_info.Items.Count; ++i)
            {
                if (role_info.Items[i] != null && role_info.Items[i].ItemId == itemId)
                    return role_info.Items[i].ItemNum;
            }
        }
        return 0;
    }

    //刷新伙伴技能
    public void RefreshParnterList()
    {
        if (uiPartnerList != null)
            uiPartnerList.InitPartnerList();
    }
    //设置选中的伙伴信息
    private void SetPartnerInfo(PartnerInfo info)
    {
        if (uiPartnerInfo != null && info != null) uiPartnerInfo.SetPartnerInfo(info);
    }
    //设置选中的伙伴信息
    public void SetPartnerInfo(int partner_id)
    {
        if (uiPartnerList != null && m_SelectedPartnerId != partner_id)
        {
            uiPartnerList.SetPartnerSelected(m_SelectedPartnerId, false);
        }
        RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
        if (roleInfo != null && roleInfo.PartnerStateInfo.GetAllPartners() != null)
        {
            if (roleInfo.PartnerStateInfo.GetAllPartners().Count > 0)
                EnableLeftPanel(true);
            else
                EnableLeftPanel(false);
        }
        else
        {
            EnableLeftPanel(false);
        }
        m_SelectedPartnerId = partner_id;
        if (IsPlayedPartner(partner_id))
        {
            m_ActivePartnerId = partner_id;
        }
        CompareFighting(m_ActivePartnerId, m_SelectedPartnerId);
        PartnerInfo info = GetPartnerInfoById(partner_id);
        if (info == null) return;
        if (info.CurAdditionLevel <= RankOffset.Length)
        {
            if (RankOffset[info.CurAdditionLevel - 1] == 0)
            {
                if (lblRankOffset != null) NGUITools.SetActive(lblRankOffset.gameObject, false);
            }
            else
            {
                if (lblRankOffset != null)
                {
                    NGUITools.SetActive(lblRankOffset.gameObject, true);
                    lblRankOffset.text = "+" + RankOffset[info.CurAdditionLevel - 1];
                }
            }
        }

        if (info.CurAdditionLevel <= RankColor.Length)
        {
            lblRankOffset.color = RankColor[info.CurAdditionLevel - 1];
            spRankColor.color = RankColor[info.CurAdditionLevel - 1];
            if (lblPartnerName != null) lblPartnerName.color = RankColor[info.CurAdditionLevel - 1];
        }
        Data_NpcConfig npcCfg = NpcConfigProvider.Instance.GetNpcConfigById(info.LinkId);
        if (npcCfg != null)
        {
            if (lblPartnerName != null) lblPartnerName.text = npcCfg.m_Name;
            if (spPartnerPortrait != null) spPartnerPortrait.spriteName = npcCfg.m_Portrait;
        }
        PartnerConfig partnerCfg = PartnerConfigProvider.Instance.GetDataById(info.Id);
        if (partnerCfg != null && lblPartnerCd != null) lblPartnerCd.text = partnerCfg.CoolDown.ToString() + "s";
        float inheritAttack = info.GetInheritAttackAttrPercent();
        if (lblInheritAttack != null) lblInheritAttack.text = (inheritAttack * 100) + "%";
        float inheritDefence = info.GetInheritDefenceAttrPercent();
        if (lblInheritDefence != null) lblInheritDefence.text = (inheritDefence * 100) + "%";
        if (lblPartnerFighting != null) lblPartnerFighting.text = "+" + GetPartnerFighting(info.GetAppendAttrConfigId()).ToString();
        EnablePlayedButton(!IsPlayedPartner(partner_id));
        SetLeftSkillInfo(info.Id, info.CurSkillStage);
        SetLeftAppendAttr(info.GetAppendAttrConfigId());
        SetPartnerInfo(info);
        //是否能训练
        bool canUpgrade = false;
        if (CheckCanUpgrade(partner_id))
        {
            canUpgrade = true;
        }
        UnityEngine.Transform tf = transform.Find("left/Buttons/03Lift/Tip");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, canUpgrade);
        }
    }
    //对比战力
    private void CompareFighting(int activeId, int selectId)
    {
        if (spFightingCompare != null) NGUITools.SetActive(spFightingCompare.gameObject, false);
        if (lblFightingCompare != null) NGUITools.SetActive(lblFightingCompare.gameObject, false);
        return;// ///************暂时将对比战力给隐藏
               /*
               PartnerInfo activeInfo = GetPartnerInfoById(activeId);
               PartnerInfo selectedInfo = GetPartnerInfoById(selectId);
               if (activeId == selectId || activeInfo == null || selectedInfo == null) {
                 //隐藏
                 if (spFightingCompare != null) NGUITools.SetActive(spFightingCompare.gameObject, false);
                 if (lblFightingCompare != null) NGUITools.SetActive(lblFightingCompare.gameObject, false);
               } else {
                 //对比
                 if (spFightingCompare == null) return;
                 int activeFighting = GetPartnerFighting(activeInfo.GetAppendAttrConfigId());
                 int selectedFighting = GetPartnerFighting(selectedInfo.GetAppendAttrConfigId());
                 int compareValue = selectedFighting - activeFighting;
                 if (compareValue >= 0) {
                   spFightingCompare.spriteName = "Up";
                   if (lblFightingCompare != null) {
                     lblFightingCompare.text = compareValue.ToString();
                     lblFightingCompare.color = UnityEngine.Color.green;
                   }

                 } else {
                   spFightingCompare.spriteName = "down";
                   if (lblFightingCompare != null) {
                     lblFightingCompare.text = (-compareValue).ToString();
                     lblFightingCompare.color = UnityEngine.Color.red;
                   }
                 }
                 NGUITools.SetActive(spFightingCompare.gameObject, true);
                 if (lblFightingCompare != null) NGUITools.SetActive(lblFightingCompare.gameObject, true);
               }*/
    }
    //设置左侧伙伴加成属性
    public void SetLeftAppendAttr(int appendAttrId)
    {
        List<string> attrList = UIAppendAttrManager.Instance.GetAppendProperty(appendAttrId);
        if (attrList != null)
        {
            for (int index = 0; index < lblAppendAttrArr.Length; ++index)
            {

                if (index < attrList.Count && !string.IsNullOrEmpty(attrList[index]))
                {
                    string attr = attrList[index];
                    if (lblAppendAttrArr[index] != null)
                        lblAppendAttrArr[index].text = attr;
                    NGUITools.SetActive(lblAppendAttrArr[index].gameObject, true);
                }
                else
                {
                    if (lblAppendAttrArr[index] != null)
                        NGUITools.SetActive(lblAppendAttrArr[index].gameObject, false);
                }
            }
        }
    }
    //设置左侧进阶技能信息
    public void SetLeftSkillInfo(int parterId, int stage)
    {
        PartnerInfo info = GetPartnerInfoById(parterId);
        if (info != null)
        {
            PartnerConfig ptCfg = PartnerConfigProvider.Instance.GetDataById(info.Id);
            if (ptCfg != null)
            {
                List<string> iconList = new List<string>();
                iconList.Add(ptCfg.Icon0);
                iconList.Add(ptCfg.Icon1);
                iconList.Add(ptCfg.Icon2);
                iconList.Add(ptCfg.Icon3);
                UIAtlas atlas = null;
                UnityEngine.GameObject goAtlas = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.GetSharedResource(ptCfg.AtlasPath));
                if (goAtlas != null)
                    atlas = goAtlas.GetComponent<UIAtlas>();
                for (int index = 0; index < spSkillLifts.Length; ++index)
                {
                    if (spSkillLifts[index] != null && index < iconList.Count)
                    {
                        spSkillLifts[index].atlas = atlas;
                        spSkillLifts[index].spriteName = iconList[index];
                    }
                    if (stage >= index + 1)
                    {
                        spSkillLifts[index].color = WhiteColor;
                        if (spSkillLocks[index] != null) NGUITools.SetActive(spSkillLocks[index].gameObject, false);
                    }
                    else
                    {
                        spSkillLifts[index].color = AshColor;
                        if (spSkillLocks[index] != null && !NGUITools.GetActive(spSkillLocks[index].gameObject))
                            NGUITools.SetActive(spSkillLocks[index].gameObject, true);
                    }
                }
            }
        }
    }
    //显示伙伴列表、隐藏高亮
    public void OnClosePartnerInfo()
    {
        if (uiPartnerList != null)
            NGUITools.SetActive(uiPartnerList.gameObject, true);
        if (m_SpPreviorsClick != null)
            NGUITools.SetActive(m_SpPreviorsClick.gameObject, false);
    }
    //点击传记
    public void OnBiographyClick()
    {
        SetButtonHighLight(spBiography);
        if (IsParterListVisible())
            NGUITools.SetActive(uiPartnerList.gameObject, false);
        if (uiPartnerInfo != null)
            uiPartnerInfo.ShowPartnerInfoType(UIPartnerInfoTypeEnum.Biography);

    }
    //点击强化
    public void OnStrengthenClick()
    {
        SetButtonHighLight(spStrengthen);
        if (IsParterListVisible())
            NGUITools.SetActive(uiPartnerList.gameObject, false);
        if (uiPartnerInfo != null)
            uiPartnerInfo.ShowPartnerInfoType(UIPartnerInfoTypeEnum.Strengthen);
    }
    //点击进阶
    public void OnSkillLiftClick()
    {
        SetButtonHighLight(spSkillLift);
        if (IsParterListVisible())
            NGUITools.SetActive(uiPartnerList.gameObject, false);
        if (uiPartnerInfo != null)
            uiPartnerInfo.ShowPartnerInfoType(UIPartnerInfoTypeEnum.SkillLift);
    }
    //点击出战
    public void OnPlayedClick()
    {
        LogicSystem.PublishLogicEvent("ge_select_partner", "partner", m_SelectedPartnerId);
    }
    //点击关闭按钮
    public void OnClosePanelClick()
    {
        UIManager.Instance.HideWindowByName("Partner");
    }
    //伙伴列表是否可见
    private bool IsParterListVisible()
    {
        if (uiPartnerList != null)
            return NGUITools.GetActive(uiPartnerList);
        return false;
    }
    //获取伙伴战力
    private int GetPartnerFighting(int partnerAttrId)
    {
        float ret = 0;
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            ArkCrossEngine.CharacterInfo user_info = role_info.GetPlayerSelfInfo();
            if (user_info != null)
            {
                ret = AttrCalculateUtility.CalculateAppendAttrFightingScore(user_info, partnerAttrId);
            }
        }
        return UnityEngine.Mathf.FloorToInt(ret);
    }
    //伙伴信息是否可见
    private bool IsPartnerInfoVisible()
    {
        if (uiPartnerInfo != null)
            return NGUITools.GetActive(uiPartnerInfo);
        return false;
    }
    //点击传记、强化、进阶按钮时，设置高亮
    private void SetButtonHighLight(UISprite sp)
    {
        if (null == sp) return;
        if (m_SpPreviorsClick != null && NGUITools.GetActive(m_SpPreviorsClick))
            NGUITools.SetActive(m_SpPreviorsClick.gameObject, false);
        NGUITools.SetActive(sp.gameObject, true);
        m_SpPreviorsClick = sp;
    }
    // 设置出战按钮的状态
    private void EnablePlayedButton(bool enable)
    {
        if (btnPlayed == null) return;
        UISprite sp = btnPlayed.GetComponent<UISprite>();
        float alpha = enable ? 1 : 0f;
        if (enable)
        {
            if (sp != null) sp.spriteName = "button_small3";//先这么处理了。。。
        }
        else
        {
            if (sp != null) sp.spriteName = "";
        }
        NGUITools.SetActive(btnPlayed.gameObject, enable);
        if (lblUnderBtnPlayed != null) lblUnderBtnPlayed.alpha = alpha;
        if (lblPlayed != null) NGUITools.SetActive(lblPlayed.gameObject, enable);
        if (spPlayed != null) NGUITools.SetActive(spPlayed.gameObject, !enable);
    }
    private PartnerInfo GetPartnerInfoById(int partner_id)
    {
        RoleInfo info = LobbyClient.Instance.CurrentRole;
        if (info != null && info.PartnerStateInfo != null)
        {
            List<PartnerInfo> partnerList = info.PartnerStateInfo.GetAllPartners();
            if (partnerList != null)
            {
                for (int i = 0; i < partnerList.Count; ++i)
                {
                    if (partnerList[i] != null && partnerList[i].Id == partner_id)
                        return partnerList[i];
                }
            }
        }
        return null;
    }
    //判断伙伴是否已出战
    private bool IsPlayedPartner(int partnerId)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null && role_info.PartnerStateInfo != null)
        {
            int activeId = role_info.PartnerStateInfo.GetActivePartnerId();
            return (activeId == partnerId);
        }
        return false;
    }
    //
    public void HandlerPartnerPlayed(int result)
    {
        try
        {
            if (result == (int)PartnerMsgResultEnum.SUCCESS)
            {
                EnablePlayedButton(false);
                if (uiPartnerList != null) uiPartnerList.SetPartnerPlayed(m_ActivePartnerId, m_SelectedPartnerId);
                CompareFighting(m_SelectedPartnerId, m_SelectedPartnerId);
                m_ActivePartnerId = m_SelectedPartnerId;
                RefreshParnterList();
            }
            else
            {
                string chn_desc = StrDictionaryProvider.Instance.GetDictString(700);
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //处理强化结果
    public void HandlerUpgradePartnerLevel(int result)
    {
        try
        {
            SetPartnerInfo(m_SelectedPartnerId);
            if (uiPartnerList != null)
            {
                PartnerInfo info = GetPartnerInfoById(m_SelectedPartnerId);
                uiPartnerList.UpdatePartnerInfo(info);
            }
            if (result == (int)PartnerMsgResultEnum.SUCCESS)
            {
                //todo:
                ShowIntensityResult(true);
            }
            else
            {
                //ShowIntensityResult(false);
                string chn_desc = StrDictionaryProvider.Instance.GetDictString(701);
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //处理升阶结果
    public void HandlerUpagradeSkillStage(int result)
    {
        try
        {
            if (result == (int)PartnerMsgResultEnum.SUCCESS)
            {
                SetPartnerInfo(m_SelectedPartnerId);
                if (uiPartnerList != null)
                {
                    PartnerInfo info = GetPartnerInfoById(m_SelectedPartnerId);
                    uiPartnerList.UpdatePartnerInfo(info);
                }
                CheckPartnerTip();
            }
            else
            {
                string chn_desc = StrDictionaryProvider.Instance.GetDictString(702);
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //处理兑换结果
    public void HandlerCompoundPartner(int result)
    {
        try
        {
            if (result == (int)PartnerMsgResultEnum.SUCCESS)
            {
                RefreshParnterList();
                string chn_desc = StrDictionaryProvider.Instance.GetDictString(707);
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
                CheckPartnerTip();
            }
            else
            {
                string chn_desc = StrDictionaryProvider.Instance.GetDictString(708);
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //处理伙伴加入
    private void HandlerAddPartner(int partnerId)
    {
        try
        {
            //RefreshParnterList();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void ShowIntensityResult(bool successed)
    {
        if (successed)
        {
            UnityEngine.GameObject go = UIManager.Instance.LoadWindowByName("IntensitySuccessed", UICamera.mainCamera);
            Destroy(go, 4f);
        }
        else
        {
            UnityEngine.GameObject go = UIManager.Instance.LoadWindowByName("IntensityFailed", UICamera.mainCamera);
            Destroy(go, 4f);
        }
    }
    public UnityEngine.Color[] RankColor = new UnityEngine.Color[9]
    {
    UnityEngine.Color.white,
    UnityEngine.Color.green,
    UnityEngine.Color.green,
    UnityEngine.Color.blue,
    UnityEngine.Color.blue,
    UnityEngine.Color.blue,
    UnityEngine.Color.gray,
    UnityEngine.Color.gray,
    UnityEngine.Color.gray,
    };
    private int[] RankOffset = new int[9] { 0, 0, 1, 0, 1, 2, 0, 1, 2 };
}
