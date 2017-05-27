using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public enum UIParternSlotType : int
{
    None,
    StorageSlot,
    SettingSlot,
}
public class UIPartnerSlot : UnityEngine.MonoBehaviour
{

    public UIParternSlotType slotEnumType = UIParternSlotType.None;
    public UISprite spRankColor;
    public UISprite spPortrait;
    public UISprite spLock;
    public UISprite spSelect;//选中状态
    public UILabel lblFighting;
    public UILabel lblUnlockLevel;
    public UnityEngine.GameObject goFighting;
    public UnityEngine.GameObject goLock;

    private bool m_IsPartnerPlayed = false;
    private int m_PartnerId = -1;
    private bool m_IsUnlock = false;
    private int m_Fighting = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void InitPartnerInfo(int partnerId)
    {
        //*********
        PartnerInfo info = GetPartnerInfoById(partnerId);
        InitPartnerInfo(info);
    }
    public void InitPartnerInfo(PartnerInfo info)
    {
        if (info != null)
        {
            m_PartnerId = info.Id;
            if (goFighting != null && !NGUITools.GetActive(goFighting))
            {
                NGUITools.SetActive(goFighting, true);
            }
            if (lblFighting != null)
            {
                m_Fighting = GetPartnerFighting(info.GetAppendAttrConfigId());
                lblFighting.text = m_Fighting.ToString();
            }
            Data_NpcConfig npcCfg = NpcConfigProvider.Instance.GetNpcConfigById(info.LinkId);
            if (npcCfg != null && spPortrait != null)
            {
                spPortrait.spriteName = npcCfg.m_Portrait;
                UIButton btnComp = spPortrait.GetComponent<UIButton>();
                if (btnComp != null) btnComp.normalSprite = npcCfg.m_Portrait;
            }
            PartnerLevelUpConfig levelUpCfg = PartnerLevelUpConfigProvider.Instance.GetDataById(info.CurAdditionLevel);
            if (spRankColor != null && levelUpCfg != null)
            {
                spRankColor.spriteName = levelUpCfg.PartnerRankColor;
                UIButton btn = this.GetComponent<UIButton>();
                if (btn != null) btn.normalSprite = levelUpCfg.PartnerRankColor;
            }
        }
    }
    //出战阵容没解锁的Slot
    public void LockSlot(int unLevel)
    {
        if (goLock != null)
        {
            NGUITools.SetActive(goLock, true);
            string chn_des = StrDictionaryProvider.Instance.GetDictString(1107);
            if (lblUnlockLevel != null) lblUnlockLevel.text = chn_des + unLevel;
        }
        EnableButton(false);
        if (goFighting != null) NGUITools.SetActive(goFighting, false);
        m_PartnerId = -1;
        m_IsPartnerPlayed = false;
    }
    //解锁
    public void UnlockSlot()
    {
        m_IsUnlock = true;
        EnableButton(true);
        if (goLock != null) NGUITools.SetActive(goLock, false);
        if (goFighting != null) NGUITools.SetActive(goFighting, true);
    }
    //
    public void NotHasPartner()
    {
        m_PartnerId = -1;
        m_IsPartnerPlayed = false;
        if (spPortrait != null) spPortrait.spriteName = "";
        ResetSlotColor();
        if (goFighting != null) NGUITools.SetActive(goFighting, false);
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
    //设置Storage中的伙伴选中状态
    public void SetStoragePartnerPlayed(bool state)
    {
        if (spSelect != null)
        {
            NGUITools.SetActive(spSelect.gameObject, state);
        }
        m_IsPartnerPlayed = state;
    }
    //取消伙伴阵容中的伙伴出战状态
    public void CancelSettingPartnerPlayed()
    {
        if (spPortrait != null)
        {
            spPortrait.spriteName = "";
            ResetSlotColor();
        }
        m_PartnerId = -1;
        if (goFighting != null) NGUITools.SetActive(goFighting, false);
    }
    //重置伙伴边框颜色
    private void ResetSlotColor(string rankColor = "SEquipFrame1")
    {
        if (spRankColor != null)
        {
            spRankColor.spriteName = rankColor;
            UIButton btn = this.GetComponent<UIButton>();
            if (btn != null) btn.normalSprite = rankColor;
        }
    }
    void OnClick()
    {
        UIPartnerPvpRightInfo right_info = NGUITools.FindInParents<UIPartnerPvpRightInfo>(gameObject);
        if (right_info != null && right_info.CanChangePlayedPartner())
        {
            if (slotEnumType == UIParternSlotType.SettingSlot)
            {
                //先改变自己的状态
                right_info.CancelPartnerPlayed(m_PartnerId, UIParternSlotType.StorageSlot);
                CancelSettingPartnerPlayed();

            }
            else
            {
                if (m_IsPartnerPlayed)
                {
                    right_info.CancelPartnerPlayed(m_PartnerId, UIParternSlotType.SettingSlot);
                    SetStoragePartnerPlayed(false);
                }
                else
                {
                    if (right_info.PartnerPlayed(m_PartnerId))
                    {
                        //如果换阵容成功，则设置Storage中的状态
                        SetStoragePartnerPlayed(true);
                    }
                }
            }
        }
        if (right_info != null && !right_info.CanChangePlayedPartner())
        {
            //此时为匹配界面,相当于点击调整按钮
            right_info.OnChangeButtonClick();
        }
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
    private void EnableButton(bool enable)
    {
        UIButton btn = this.GetComponent<UIButton>();
        if (btn != null)
        {
            btn.isEnabled = enable;
        }
    }
    public int GetPartnerId()
    {
        return m_PartnerId;
    }
    public bool IsUnlock()
    {
        return m_IsUnlock;
    }
    public int GetFighting()
    {
        return m_Fighting;
    }
}
