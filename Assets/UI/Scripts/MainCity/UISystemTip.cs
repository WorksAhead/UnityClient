using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ArkCrossEngine;

public class UISystemTip : UnityEngine.MonoBehaviour
{

    private const float c_SecondPerVigorPlus = 6f;//每6秒增加一点
    private float m_PlayerVigorPlus = 0f;
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
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
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
            eo = LogicSystem.EventChannelForGfx.Subscribe("ge_item_change", "item", OnItemChange);
            if (eo != null)
                eventlist.Add(eo);
            eo = LogicSystem.EventChannelForGfx.Subscribe<List<ArkCrossEngine.MailInfo>>("ge_sync_mail_list", "mail", SyncMailList);
            if (eo != null)
                eventlist.Add(eo);
            eo = LogicSystem.EventChannelForGfx.Subscribe("ge_notify_new_mail", "mail", NewMail);
            if (eo != null)
            {
                eventlist.Add(eo);
            }
            eo = LogicSystem.EventChannelForGfx.Subscribe<int>("ge_sync_player_vigor", "info", HandleSyncPlayerVigor);
            if (eo != null)
            {
                eventlist.Add(eo);
            }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_entrance_gold_change", "ui", CheckSkill);
            if (eo != null) eventlist.Add(eo);
            eo = LogicSystem.EventChannelForGfx.Subscribe<int, int, int, ArkCrossEngine.Network.GeneralOperationResult>("ge_unlock_skill", "skill", HandleUnLockSkill);
            if (null != eo) eventlist.Add(eo);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void Start()
    {
        try
        {
            ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_get_mail_list", "lobby");//请求邮件数据
            LogicSystem.PublishLogicEvent("ge_request_vigor", "lobby");

            CheckPvPEntrance();
            CheckArtifact();
            CheckXhun();
            CheckPartner();
            CheckActivity();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Update()
    {
        try
        {
            UpdatePlayerVigor();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //初始化邮件数据
    private void SyncMailList(List<MailInfo> maillist)
    {
        UIDataCache.Instance.maillist = maillist;
        CheckMail();
    }

    //新邮件通知
    private void NewMail()
    {
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("Mail");
        if (go == null)
        {//如果界面存在则 避免重复请求
            ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_get_mail_list", "lobby");
        }
    }

    //升级
    private void UserLevelUP(int lv)
    {
        CheckPvPEntrance();
        CheckActivity();
    }

    //物品变化
    private void OnItemChange()
    {
        CheckArtifact();
        CheckXhun();
        CheckPartner();
    }

    //获取物品信息
    private static ItemDataInfo GetItemDataInfoById(int itemId)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            for (int i = 0; i < role_info.Items.Count; i++)
            {
                if (role_info.Items[i] != null && role_info.Items[i].ItemId == itemId)
                {
                    return role_info.Items[i];
                }
            }
        }
        return null;
    }

    //检查pvp相关
    public static void CheckPvPEntrance()
    {
        bool has = false;
        RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
        if (roleInfo != null)
        {
            if (roleInfo.HasPvpTip)
            {
                has = true;
            }
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.pvp, has);
    }

    //检查神器相关
    public static void CheckArtifact()
    {
        bool has = false;
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            foreach (ItemDataInfo item_info in role_info.Legacys)
            {
                if (item_info.IsUnlock)
                {//已解锁的
                    if (item_info.Level < role_info.Level)
                    {
                        LegacyLevelupConfig legacyLvUpCfg = LegacyLevelupConfigProvider.Instance.GetDataById(item_info.Level);
                        if (legacyLvUpCfg != null)
                        {
                            int legacyIndex = -1;
                            for (int index = 0; index < role_info.Legacys.Length; ++index)
                            {
                                if (role_info.Legacys[index] != null && item_info.ItemId == role_info.Legacys[index].ItemId)
                                {
                                    legacyIndex = index;
                                    break;
                                }
                            }
                            if (legacyIndex >= 0 && legacyIndex < legacyLvUpCfg.m_CostItemList.Count)
                            {
                                int costitemId = legacyLvUpCfg.m_CostItemList[legacyIndex];
                                int currentNum = 0;
                                ItemDataInfo costItemDataInfo = GetItemDataInfoById(costitemId);
                                if (costItemDataInfo != null)
                                {
                                    currentNum = costItemDataInfo.ItemNum;
                                }
                                int max = legacyLvUpCfg.m_CostNum;
                                if (currentNum >= max)
                                {//物品够
                                    has = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.GodEquip, has);
    }

    //检查xhun相关
    public static void CheckXhun()
    {
        bool has = false;
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        XSoulInfo<XSoulPartInfo> xsoul_info = role.GetXSoulInfo();
        if (xsoul_info == null)
            return;
        XSoulPartInfo xsoul_part_into = xsoul_info.GetXSoulPartData(XSoulPart.kWeapon);
        if (xsoul_part_into == null)
            return;
        ItemDataInfo itemInfo = xsoul_part_into.XSoulPartItem;
        if (itemInfo == null)
            return;
        int curExp = itemInfo.CurLevelExperience;
        int id = itemInfo.ItemId;
        int lv = itemInfo.Level;
        XSoulLevelConfig config = XSoulLevelConfigProvider.Instance.GetDataById(id);
        if (lv < config.m_MaxLevel)
        {
            int maxExp = 0;
            int nextLv = lv + 1 <= config.m_MaxLevel ? lv + 1 : config.m_MaxLevel;
            config.m_LevelExperience.TryGetValue(nextLv, out maxExp);
            int remainExp = maxExp - curExp;//剩余经验
            int[] hunIds = config.m_ExperienceProvideItems;
            int hunExp = 0;//魂丹总经验
            for (int i = 0; i < hunIds.Length; i++)
            {
                ItemDataInfo item = GetItemDataInfoById(hunIds[i]);
                if (item != null)
                {
                    ItemConfig itemConfig = ItemConfigProvider.Instance.GetDataById(hunIds[i]);
                    if (itemConfig != null)
                    {
                        hunExp += item.ItemNum * itemConfig.m_ExperienceProvide;
                    }
                }
            }
            if (remainExp <= hunExp)
            {
                has = true;
            }
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.Xhun, has);
    }

    public static void CheckMail(Dictionary<UnityEngine.GameObject, MailInfo> mailDic = null, Dictionary<ulong, bool> mailStateDic = null)
    {
        bool has = false;
        if (mailDic == null && mailStateDic == null)
        {//表示非ui脚本调用
            if (UIDataCache.Instance.maillist != null)
            {
                foreach (MailInfo mail in UIDataCache.Instance.maillist)
                {
                    if (mail.m_AlreadyRead == false)
                    {
                        has = true;
                        break;
                    }
                }
            }
        }
        else
        {//由ui内传参调用
            foreach (MailInfo mi in mailDic.Values)
            {
                if (mi != null && mailStateDic[mi.m_MailGuid] == false)
                {
                    has = true;
                    break;
                }
            }
        }

        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.Mail, has);
    }

    //检查伙伴相关
    public static void CheckPartner()
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
                        int ownFragNum = GetItemDataInfoById(fragId).ItemNum;
                        if (ownFragNum >= fragNeedNum)
                        {//可召唤
                            has = true;
                            break;
                        }
                    }
                }
                else
                {//已拥有
                    if (CheckPartnerCanUpgrade(partnerId))
                    {
                        has = true;
                        break;
                    }
                }
            }
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.Partner, has);
    }
    private static bool CheckPartnerCanUpgrade(int partnerId)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        PartnerInfo partnerInfo = role_info.PartnerStateInfo.GetPartnerInfoById(partnerId);
        if (partnerInfo != null && partnerInfo.CurSkillStage < 4)
        {//未满级
            int ownItemNum = GetItemDataInfoById(partnerInfo.StageUpItemId).ItemNum;
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

    //检查活动（多人）
    public static void CheckActivity()
    {
        bool has = false;
        RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
        if (roleInfo != null)
        {
            if (roleInfo.HasActivityTip)
            {
                has = true;
            }
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.Activity, has);
    }
    private void HandleSyncPlayerVigor(int value)
    {
        UIDataCache.Instance.vigor = value;
        CheckSkill();
    }
    //检查是否有能升级
    public static void CheckSkill()
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
                                int vigor = UIDataCache.Instance.vigor;
                                if (role.Money >= needMoney && vigor >= needVigor)
                                {
                                    has = true;
                                    currHas = true;
                                }
                            }
                        }
                    }
                }
                if (UIManager.Instance.IsWindowVisible("SkillPanel"))
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_skillslot_state_change", "skill", info.SkillId, currHas);
            }
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.Skill, has);
    }
    private void UpdatePlayerVigor()
    {
        //精力默认每m_SecondPerVigorPlus秒+1
        if (m_PlayerVigorPlus >= c_SecondPerVigorPlus)
        {
            m_PlayerVigorPlus = 0;
            UIDataCache.Instance.vigor++;
            CheckSkill();
        }
        else
        {
            m_PlayerVigorPlus += UnityEngine.Time.deltaTime;
        }
    }
    public void HandleUnLockSkill(int presetIndex, int skillId, int userLevel, ArkCrossEngine.Network.GeneralOperationResult result)
    {
        try
        {
            if (ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed == result && !UIManager.Instance.IsWindowVisible("SkillPanel"))
            {
                AutoEquipSkill(skillId);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void AutoEquipSkill(int skill_id)
    {
        if (skill_id <= 0 || null == LobbyClient.Instance.CurrentRole)
            return;
        UnityEngine.GameObject go = UIManager.Instance.TryGetWindowGameObject("SkillPanel");
        if (null != go)
        {
            SlotPosition pos = go.GetComponent<UISkillPanel>().uiSkillSetting.GetIdleSkillSlot();
            if ((int)pos > 0)
            {
                LogicSystem.PublishLogicEvent("ge_mount_skill", "lobby", UISkillSetting.presetIndex, skill_id, pos);
            }
        }
    }
}
