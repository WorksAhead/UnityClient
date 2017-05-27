using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class UISkillPanel : UnityEngine.MonoBehaviour
{
    public UISkillTitle uiSkillTitle = null;
    public UISkillInfo uiSkillInfo = null;
    public UISkillSetting uiSkillSetting = null;
    public UISkillStorage uiSkillStorage = null;

    public UnityEngine.GameObject goSkillInfo = null;
    public UnityEngine.GameObject goSkillLiftInfo = null;
    public UnityEngine.GameObject goSkillLevelUpInfo = null;
    public UnityEngine.GameObject goSkillInfoButton = null;
    public UnityEngine.GameObject goSkillLevelUpButton = null;
    public UnityEngine.GameObject goSkillLiftUpButton = null;
    private int m_CurrentClickSkillId = -1;
    private int m_backVigor = 0;
    private int m_foreVigor = 0;
    private bool isEnable;
    private bool hasInitSkills = false;
    private const int CHECK_TICK_TIME = 6;
    //按钮
    public UIButton equipButton = null;

    private const string C_ConFirm = "OK";
    //
    private List<object> m_EventList = new List<object>();
    private bool hasCanUpgrade = false;
    //
    public enum PanelType : int
    {
        SkillInfo = 1,
        SkillLevelUpInfo = 2,
        SkillLiftUpInfo = 3
    }
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
            m_EventList.Clear();
            object obj = null;
            obj = LogicSystem.EventChannelForGfx.Subscribe<List<SkillInfo>>("ge_init_skills", "skill", InitSkills);
            if (null != obj) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int, int, int, ArkCrossEngine.Network.GeneralOperationResult>("ge_mount_skill", "skill", HandleLoadSkill);
            if (null != obj) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int, int, SlotPosition, SlotPosition, ArkCrossEngine.Network.GeneralOperationResult>("ge_swap_skill", "skill", HandleSwapSkill);
            if (null != obj) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int, int, ArkCrossEngine.Network.GeneralOperationResult>("ge_unmount_skill", "skill", OnUnloadedSkill);
            if (null != obj) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int, int, int, ArkCrossEngine.Network.GeneralOperationResult>("ge_upgrade_skill", "skill", HandleUpgradeSkill);
            if (null != obj) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int, int, int, ArkCrossEngine.Network.GeneralOperationResult>("ge_unlock_skill", "skill", HandleUnLockSkill);
            if (null != obj) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int, int, ArkCrossEngine.Network.GeneralOperationResult>("ge_lift_skill", "skill", HandleLiftSkill);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int>("ge_sync_player_vigor", "info", HandleSyncPlayerVigor);
            if (obj != null) m_EventList.Add(obj);
            obj = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_entrance_gold_change", "ui", CheckHasUpgrade);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null) m_EventList.Add(obj);
            //LogicSystem.PublishLogicEvent("ge_request_vigor", "lobby");
            LogicSystem.PublishLogicEvent("ge_request_skills", "ui");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Use this for initialization
    void Start()
    {
        try
        {
            //装备按钮的初始状态不可点击
            SetEquipButtonState(false);
            //ShowInfoPanel(PanelType.SkillLevelUpInfo);
            if (uiSkillStorage != null && uiSkillStorage.skillStorageArr.Length > 0)
            {
                SetSkillInfo(uiSkillStorage.skillStorageArr[0].SkillId);
            }

            //暂定5级之后不再教学
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            int level = 5;
            if (role_info != null) level = role_info.Level;
            if (UISkillGuide.Instance.GetSteps() < 3 && level <= 25)
            {
                UISkillGuide.Instance.ShowGuideInSlotHandler = this.AddGuidePointing;
                UISkillGuide.Instance.HandleGuidePointToUnlock = this.GuidePointToUnlock;
                UISkillGuide.Instance.ShowGuidePointing();
            }
            if (uiSkillInfo != null)
            {
                uiSkillInfo.onVigorChange = UpdateBackVigor;
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
            if (roleInfo != null)
            {
                SetMoneyCoin(Convert.ToInt32(roleInfo.Money));
                SetDiamond(Convert.ToInt32(roleInfo.Gold));
                SetHeroLevel(roleInfo.Level);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //
    void OnEnable()
    {
        try
        {
            isEnable = true;
            LogicSystem.PublishLogicEvent("ge_request_vigor", "lobby");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OnDisable()
    {
        try
        {
            isEnable = false;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    //
    public void ShowInfoPanel(PanelType index)
    {
        if (goSkillInfo == null || goSkillLevelUpInfo == null || goSkillLiftInfo == null)
            return;
        switch (index)
        {
            case PanelType.SkillInfo:
                NGUITools.SetActive(goSkillInfo, true);
                NGUITools.SetActive(goSkillLevelUpInfo, false);
                NGUITools.SetActive(goSkillLiftInfo, false);
                NGUITools.SetActive(goSkillInfoButton, true);
                NGUITools.SetActive(goSkillLevelUpButton, false);
                NGUITools.SetActive(goSkillLiftUpButton, false);
                break;
            case PanelType.SkillLevelUpInfo:
                NGUITools.SetActive(goSkillInfo, false);
                NGUITools.SetActive(goSkillLevelUpInfo, true);
                NGUITools.SetActive(goSkillLiftInfo, false);
                NGUITools.SetActive(goSkillInfoButton, false);
                NGUITools.SetActive(goSkillLevelUpButton, true);
                NGUITools.SetActive(goSkillLiftUpButton, false); break;
            case PanelType.SkillLiftUpInfo:
                NGUITools.SetActive(goSkillInfo, false);
                NGUITools.SetActive(goSkillLevelUpInfo, false);
                NGUITools.SetActive(goSkillLiftInfo, true);
                NGUITools.SetActive(goSkillInfoButton, false);
                NGUITools.SetActive(goSkillLevelUpButton, false);
                NGUITools.SetActive(goSkillLiftUpButton, true); break;
        }
    }
    //装备技能返回的消息
    public void HandleLoadSkill(int presetIndex, int skillId, int slotPositon, ArkCrossEngine.Network.GeneralOperationResult result)
    {
        try
        {
            if (ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed == result)
            {
                string msg = string.Format("LoadSkill:{0},{1},{2}", presetIndex, skillId, slotPositon);
                Debug.Log(msg);
                uiSkillSetting.OnLoadedSkill(presetIndex, skillId, slotPositon);
                uiSkillStorage.OnLoadedSkill(presetIndex, skillId, slotPositon);
                if (UISkillGuide.Instance.GetSteps() < 3)
                    GuideStepComplete(3, skillId);
            }
            else
            {
                string chn_desc = StrDictionaryProvider.Instance.GetDictString(363);
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //
    public void HandleSwapSkill(int presetIndex, int skillId, SlotPosition sourcePos, SlotPosition targetPos, ArkCrossEngine.Network.GeneralOperationResult result)
    {
        try
        {
            if (ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed == result)
            {
                string msg = string.Format("OnSwapSkill:{0},{1}", sourcePos, targetPos);
                Debug.Log(msg);
                int targetSkillId = uiSkillSetting.GetSkillId((int)targetPos);
                uiSkillSetting.ExchangeSlot(sourcePos, targetPos);
                uiSkillStorage.OnExchangeSkill(skillId, targetSkillId, sourcePos, targetPos);
            }
            else
            {
                string chn_desc = StrDictionaryProvider.Instance.GetDictString(364);
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    //
    public void HandleUpgradeSkill(int presetIndex, int curSkillId, int curSkillLevel, ArkCrossEngine.Network.GeneralOperationResult result)
    {
        try
        {
            if (ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed == result)
            {
                //string msg = string.Format("HandleUpGradeSkill in UI presetIndex:{0}, skillId:{1}", presetIndex, curSkillId);
                //Debug.Log(msg);
                SetSkillInfo(curSkillId, true);
                RoleInfo role_info = LobbyClient.Instance.CurrentRole;
                if (role_info != null && uiSkillInfo != null)
                    uiSkillInfo.SetPlayerVigor(role_info.Vigor);
                m_foreVigor = role_info.Vigor;
                m_backVigor = role_info.Vigor;
                CheckHasUpgrade();
            }
            else
            {
                string chn_desc = StrDictionaryProvider.Instance.GetDictString(365);
                switch (result)
                {
                    case ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_CostError:
                        chn_desc = StrDictionaryProvider.Instance.GetDictString(369);
                        break;
                    case ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_LevelError:
                        chn_desc = StrDictionaryProvider.Instance.GetDictString(370);
                        break;
                    case ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_Unknown:
                        chn_desc = StrDictionaryProvider.Instance.GetDictString(365);
                        break;
                    case ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_VigorError:
                        chn_desc = StrDictionaryProvider.Instance.GetDictString(371);
                        break;
                }
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //
    public void HandleUnLockSkill(int presetIndex, int skillId, int userLevel, ArkCrossEngine.Network.GeneralOperationResult result)
    {
        try
        {
            if (ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed == result)
            {
                //string msg = string.Format("HandleUnlockSkill in UI presetIndex:{0}, skillId:{1}", presetIndex, skillId);
                //Debug.Log(msg);
                uiSkillStorage.OnUnlockSkill(skillId);
                SetSkillInfo(skillId);
                //可装备、可升级
                SetEquipButtonState(true);
                SetActionButtonState(true);
                SetEquipFlag(true);
                AutoEquipSkill(skillId);
            }
            else
            {
                string chn_desc = StrDictionaryProvider.Instance.GetDictString(366);
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void AutoEquipSkill(int skill_id)
    {
        if (skill_id <= 0 || null == LobbyClient.Instance.CurrentRole)
            return;
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("SkillPanel");
        if (null != go)
        {
            SlotPosition pos = go.GetComponent<UISkillPanel>().uiSkillSetting.GetIdleSkillSlot();
            if ((int)pos > 0)
            {
                LogicSystem.PublishLogicEvent("ge_mount_skill", "lobby", UISkillSetting.presetIndex, skill_id, pos);
            }
        }
    }
    //处理升阶之后的信息
    public void HandleLiftSkill(int sourceId, int targetId, ArkCrossEngine.Network.GeneralOperationResult result)
    {
        try
        {
            if (ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed == result)
            {
                //string str = string.Format("HandleLiftSkill,sourceId:{0},targetId:{1}", sourceId, targetId);
                //Debug.Log(str);
                SetSkillInfo(targetId);
                uiSkillStorage.OnLiftSkill(sourceId, targetId);
                uiSkillSetting.OnLiftSkill(sourceId, targetId);
            }
            else
            {
                string chn_desc = StrDictionaryProvider.Instance.GetDictString(367);
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", chn_desc, C_ConFirm, null, null, null, false);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void UpdateBackVigor(int vigor)
    {
        m_foreVigor = vigor;
        CheckHasUpgrade();
    }

    //同步精力信息
    public void HandleSyncPlayerVigor(int vigor)
    {
        if (uiSkillInfo != null)
        {
            uiSkillInfo.SetPlayerVigor(vigor);
            m_foreVigor = vigor;
            m_backVigor = vigor;
        }
        CheckHasUpgrade();//先查一遍
        if (IsInvoking("CheckHasUpgradeTick"))
        {
            CancelInvoke("CheckHasUpgradeTick");//用于矫正vigor
        }
        InvokeRepeating("CheckHasUpgradeTick", CHECK_TICK_TIME, CHECK_TICK_TIME);//CHECK_TICK_TIME秒后，每CHECK_TICK_TIME秒执行一次
    }

    private void CheckHasUpgradeTick()
    {
        m_backVigor += 1;
        if (hasCanUpgrade == false)
        {//如果有可升级的则不检查（主要用于ui关闭的时候）
            CheckHasUpgrade();
        }
    }

    //检查是否有能升级
    private void CheckHasUpgrade()
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        if (role == null)
            return;
        bool has = false;
        List<SkillInfo> skillList = role.SkillInfos;
        foreach (SkillInfo info in skillList)
        {
            if (info != null)
            {
                bool currHas = false;
                if (info.SkillLevel > 0)
                {//已解锁
                    if (info.SkillLevel < role.Level)
                    {//等级
                        SkillLogicData skillCfg = SkillConfigProvider.Instance.ExtractData(SkillConfigType.SCT_SKILL, info.SkillId) as SkillLogicData;
                        if (skillCfg != null)
                        {
                            SkillLevelupConfig levelUpCfg = SkillLevelupConfigProvider.Instance.GetDataById(info.SkillLevel);
                            int needMoney = 0;
                            int needVigor = 0;
                            if (levelUpCfg != null)
                            {
                                if (skillCfg.LevelUpCostType >= 1 && skillCfg.LevelUpCostType <= levelUpCfg.m_TypeList.Count)
                                    needMoney = levelUpCfg.m_TypeList[skillCfg.LevelUpCostType - 1];
                                if (skillCfg.LevelUpVigorType >= 1 && skillCfg.LevelUpVigorType <= levelUpCfg.m_VigorList.Count)
                                    needVigor = levelUpCfg.m_VigorList[skillCfg.LevelUpVigorType - 1];
                                int vigor = isEnable ? m_foreVigor : m_backVigor;
                                if (role.Money >= needMoney && vigor >= needVigor)
                                {
                                    has = true;
                                    currHas = true;
                                }
                            }
                        }
                    }
                }
                if (hasInitSkills)
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_skillslot_state_change", "skill", info.SkillId, currHas);
            }
        }
        hasCanUpgrade = has;
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.Skill, has);

        //foreach (UISkillSlot slot in skillStorageArr) {
        //  if (null != slot) {
        //    if (slot.m_IsUnlock == true) {//已解锁
        //      if (slot.SkillLv < role.Level) {//等级
        //        SkillLogicData skillCfg = SkillConfigProvider.Instance.ExtractData(SkillConfigType.SCT_SKILL, slot.SkillId) as SkillLogicData;
        //        if (skillCfg != null) {
        //          SkillLevelupConfig levelUpCfg = SkillLevelupConfigProvider.Instance.GetDataById(slot.SkillLv);
        //          int needMoney = 0;
        //          int needVigor = 0;
        //          if (levelUpCfg != null) {
        //            if (skillCfg.LevelUpCostType >= 1 && skillCfg.LevelUpCostType <= levelUpCfg.m_TypeList.Count)
        //              needMoney = levelUpCfg.m_TypeList[skillCfg.LevelUpCostType - 1];
        //            if (skillCfg.LevelUpVigorType >= 1 && skillCfg.LevelUpVigorType <= levelUpCfg.m_VigorList.Count)
        //              needVigor = levelUpCfg.m_VigorList[skillCfg.LevelUpVigorType - 1];

        //            if (role.Money >= needMoney && uiSkillInfo != null && uiSkillInfo.PlayerVigor > needVigor) {
        //              has = true;
        //            }
        //          }
        //        }
        //      }
        //    }
        //  }
        //}
    }

    public void SetHeroLevel(int level)
    {
        if (null != uiSkillTitle)
        {
            uiSkillTitle.SetLevel(level);
        }
    }
    //设置金币数量
    public void SetMoneyCoin(int amount)
    {
        if (null != uiSkillTitle)
        {
            uiSkillTitle.SetMoneyCoin(amount);
        }
    }
    //设置钻石数量
    public void SetDiamond(int amount)
    {
        if (null != uiSkillTitle)
        {
            uiSkillTitle.SetDiamond(amount);
        }
    }
    public void SetSkillInfo(int skillId, bool isUpgrade = false)
    {
        m_CurrentClickSkillId = skillId;
        if (uiSkillInfo == null)
        {
            Debug.LogError("!!Did not initialize uiSkillInfo");
            return;
        }
        if (uiSkillInfo != null)
        {
            uiSkillInfo.SetLiftSkillInfo(skillId);
            uiSkillInfo.SetSkillLevelUpInfo(skillId, isUpgrade);
            uiSkillInfo.SetSkillPanelInfo(skillId);
        }
        if (uiSkillStorage != null)
            uiSkillStorage.OnUpgradeSkill(skillId, isUpgrade);
        //uiSkillInfo.SetText(LabelType.MsgCd, 2.ToString(), "3");
    }

    //根据玩家Id设置技能包
    public void InitSkills(List<SkillInfo> skillPresets)
    {
        try
        {
            if (uiSkillStorage == null) return;
            if (skillPresets.Count > 0)
                CurrentClickSkillId = skillPresets[0].SkillId;
            uiSkillStorage.InitSkillStorage(skillPresets);
            uiSkillSetting.InitSkillSetting(skillPresets);
            hasInitSkills = true;
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    //设置技能段
    public void SkillPoint(int skillId)
    {
    }
    public void OnUnloadedSkill(int presetIndex, int slotPos, ArkCrossEngine.Network.GeneralOperationResult result)
    {
        try
        {
            if (ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed == result)
            {
                int skillId = uiSkillSetting.GetSkillId(slotPos);
                uiSkillStorage.OnUnloadedSkill(presetIndex, skillId);
                uiSkillSetting.OnUnloadedSkill(slotPos);
            }
            else
            {
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", result.ToString(), C_ConFirm, null, null, null, false);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void OnUnloadedSkill(int skillId)
    {
        uiSkillStorage.OnUnloadedSkill(UISkillSetting.presetIndex, skillId);
    }
    //点击操作装备技能
    public void SendMsg(int targetId, SlotPosition targetPos)
    {
        if (uiSkillInfo == null) return;
        //uiSkillInfo.SkillId为SourceId,==-1则返回
        if (uiSkillInfo.SkillId == -1) return;
        int sourceId = uiSkillInfo.SkillId;
        UISkillSlot sourceSlot = uiSkillStorage.GetSlot(sourceId);
        int currentPreset = UISkillSetting.presetIndex;
        if (sourceSlot != null)
        {
            if (sourceSlot.EquipedPos != null && sourceSlot.EquipedPos.Presets[currentPreset] == SlotPosition.SP_None)
            {
                LogicSystem.PublishLogicEvent("ge_mount_skill", "lobby", currentPreset, sourceId, targetPos);
            }
            else
            {
                LogicSystem.PublishLogicEvent("ge_swap_skill", "lobby", currentPreset, sourceId, sourceSlot.EquipedPos.Presets[UISkillSetting.presetIndex], targetPos);
            }
        }
    }
    //点击技能图标，SkillSetting和SkillStorage上的对应图标也高亮
    public void ShowSlotHighlight(SlotType slotType)
    {
        if (uiSkillSetting != null && uiSkillStorage != null)
        {
            if (slotType == SlotType.SkillSetting)
            {
                uiSkillSetting.ShowSlotHighlight(CurrentClickSkillId);
                uiSkillStorage.ShowHighLight(CurrentClickSkillId);
            }
            else
            {
                uiSkillSetting.ShowSlotHighlight(CurrentClickSkillId);
                uiSkillStorage.ShowHighLight(CurrentClickSkillId);
            }
        }
    }
    //设置可装备
    public void SetEquipFlag(bool canEquip)
    {
        if (uiSkillSetting != null)
            uiSkillSetting.SetEquipFlag(canEquip);
    }

    //设置按钮状态为解锁或者升级
    public void SetActionButtonState(bool isUnlock)
    {
        uiSkillInfo.SetActionState(isUnlock);
    }
    //
    public void UpdateProperty(float money, float gold, int curStamina, int exp, int level)
    {
        //NGUIDebug.Log("UpdateProperty");
        try
        {
            SetMoneyCoin(Convert.ToInt32(money));
            SetDiamond(Convert.ToInt32(gold));
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void OnEquipPageButtonClick()
    {
        ShowInfoPanel(PanelType.SkillInfo);
        // ShowSlotHighlight(CurrentClickSkillId);
        if (uiSkillInfo != null)
        {
            uiSkillInfo.SetSkillPanelInfo(CurrentClickSkillId);

        }
    }
    public void OnLevelUpPageButtonClick()
    {
        ShowInfoPanel(PanelType.SkillLevelUpInfo);
        if (uiSkillInfo != null)
        {
            uiSkillInfo.SetSkillLevelUpInfo(CurrentClickSkillId);
        }
    }
    public void OnLiftUpPageButtonClick()
    {
        ShowInfoPanel(PanelType.SkillLiftUpInfo);
        if (uiSkillInfo != null)
        {
            uiSkillInfo.SetLiftSkillInfo(CurrentClickSkillId);
        }
    }

    //根据技能状态设置装备按钮状态
    public void SetEquipButtonState(bool enable)
    {
        //     if(equipButton!=null)
        //     equipButton.isEnabled = enable;
    }
    //当前点击技能ID
    public int CurrentClickSkillId
    {
        get
        {
            return m_CurrentClickSkillId;
        }
        set
        {
            m_CurrentClickSkillId = CurrentClickSkillId;
        }
    }
    //
    public bool IsSkillStorageTranslating()
    {
        if (uiSkillStorage != null)
            return uiSkillStorage.IsTranslate();
        return false;
    }
    //添加新手指引指示
    public void AddGuidePointing(SlotType slotType, int skillId)
    {
        string path = UIManager.Instance.GetPathByName("GuideHand");
        UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.GetSharedResource(path));
        if (go == null)
        {
            Debug.Log("!!!Load " + path + " failed.");
            return;
        }
        if (slotType == SlotType.SkillStorage)
        {
            DelGuidePointOnUnlock();
            if (uiSkillSetting != null)
                uiSkillSetting.DelGuidePointing(-1);
            if (uiSkillStorage != null)
            {
                uiSkillStorage.AddGuidePointing(go, skillId);
            }
        }
        else
        {
            if (uiSkillSetting != null)
            {
                uiSkillSetting.AddGuidePointing(go, -1);
                //如果没有第二步，只存在第一步到第三步则需要去掉Storage中的指示
                if (uiSkillStorage != null) uiSkillStorage.DelGuidePointing(skillId);
            }
        }
    }
    public void GuidePointToUnlock(int skillId)
    {
        if (uiSkillStorage != null) uiSkillStorage.DelGuidePointing(skillId);
        string path = UIManager.Instance.GetPathByName("GuideHand");
        UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.GetSharedResource(path));
        if (go == null)
        {
            Debug.Log("!!!Load " + path + " failed.");
            return;
        }
        DelGuidePointOnUnlock();
        UnityEngine.Transform transUnlock = this.transform.Find("SkillInfo/02LevelUpInfo/bt");
        if (transUnlock != null)
        {
            NGUITools.AddChild(transUnlock.gameObject, go);
        }
    }
    public void DelGuidePointOnUnlock()
    {
        UnityEngine.Transform transUnlock = this.transform.Find("SkillInfo/02LevelUpInfo/bt/GuideHand(Clone)");
        if (transUnlock != null)
        {
            NGUITools.Destroy(transUnlock.gameObject);
        }
    }
    public void GuideStepComplete(int stepIndex, int skillId = -1)
    {
        if (stepIndex == 2)
        {
            UISkillGuide.Instance.SetSteps(2);
            DelGuidePointOnUnlock();
            AddGuidePointing(SlotType.SkillSetting, -1);
        }
        if (stepIndex == 3)
        {
            UISkillGuide.Instance.SetSteps(3);
            if (uiSkillSetting != null)
                uiSkillSetting.DelGuidePointing(skillId);
            if (uiSkillStorage != null)
                uiSkillStorage.DelGuidePointing(UISkillGuide.Instance.m_CouldEquipSkillId);
        }
    }
    private UnityEngine.GameObject m_GuideHand = null;

}
