using UnityEngine;
using System;
using ArkCrossEngine;
using System.Collections;
using System.Collections.Generic;

public enum UIPartnerItemStyle
{
    PartnerPlayed,
    PartnerFrag
}
public class UIPartnerItem : UnityEngine.MonoBehaviour
{

    public UIProgressBar progressBar = null;
    public UILabel lblPartnerFragNum = null;
    public UILabel lblPartnerName;
    public UILabel lblFighting;
    public UILabel lblRankOffset;
    public UISprite spRankColor;
    public UISprite spPortait = null;
    public UnityEngine.GameObject goPlayed = null;
    public UnityEngine.GameObject goFighting;
    public UnityEngine.GameObject goSummonBtn;
    public UISprite spSelectFlag = null;
    public int m_PartnerFighting = 0;
    private int m_PartnerFragId = -1;//伙伴碎片Id（用于兑换伙伴）
    private bool m_Isplayed = false;
    private bool m_IsPartnerFrag = false;//true--伙伴碎片   false--伙伴
    private bool m_CanCompoundPartner = false;//是否可兑换伙伴
                                              // Use this for initialization
    void Start()
    {
        try
        {
            if (spSelectFlag != null && !m_Isplayed)
            {
                NGUITools.SetActive(spSelectFlag.gameObject, false);
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
    void OnClick()
    {
        if (m_IsPartnerFrag)
        {  //todo:为伙伴灵魂石时，点击响应还待商讨
            if (m_CanCompoundPartner)
            {
                LogicSystem.PublishLogicEvent("ge_compound_partner", "partner", m_PartnerId);
            }
            else
            {
                LogicSystem.EventChannelForGfx.Publish("ge_item_come_from", "ui", m_PartnerFragId);
            }
        }
        else
        {
            UIPartnerPanel partnerPanel = NGUITools.FindInParents<UIPartnerPanel>(gameObject);
            if (partnerPanel != null) partnerPanel.SetPartnerInfo(m_PartnerId);
            if (spSelectFlag != null) NGUITools.SetActive(spSelectFlag.gameObject, true);
        }
    }
    public void InitPartnerItem(PartnerInfo info, int active_id)
    {
        if (info == null) return;
        m_IsPartnerFrag = false;
        SetItemStyle(UIPartnerItemStyle.PartnerPlayed);
        //设置战力
        int partnerFighting = GetPartnerFighting(info.GetAppendAttrConfigId());
        m_PartnerFighting = partnerFighting;
        if (lblFighting != null) lblFighting.text = partnerFighting.ToString();
        if (info.CurAdditionLevel <= RankOffset.Length)
        {
            if (RankOffset[info.CurAdditionLevel - 1] == 0)
            {
                if (lblRankOffset != null) lblRankOffset.text = "";
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
        m_PartnerId = info.Id;
        bool isPlayed = (m_PartnerId == active_id);
        m_Isplayed = isPlayed;
        SetPlayed(isPlayed);
        SetSelectedFlagVisible(isPlayed);
        Data_NpcConfig npcCfg = NpcConfigProvider.Instance.GetNpcConfigById(info.LinkId);
        if (npcCfg != null)
        {
            if (lblPartnerName != null) lblPartnerName.text = npcCfg.m_Name;
            if (spPortait != null) spPortait.spriteName = npcCfg.m_Portrait;
        }
    }
    public void InitNotHavenPartner(PartnerConfig partner_cfg)
    {
        if (partner_cfg == null) return;
        m_IsPartnerFrag = true;
        SetItemStyle(UIPartnerItemStyle.PartnerFrag);
        if (spSelectFlag != null) NGUITools.SetActive(spSelectFlag.gameObject, false);
        m_PartnerFighting = -1;
        m_PartnerId = partner_cfg.Id;
        int fragId = partner_cfg.PartnerFragId;
        m_PartnerFragId = fragId;
        int fragNeedNum = partner_cfg.PartnerFragNum;
        int ownFragNum = GetItemNum(fragId);
        if (ownFragNum >= fragNeedNum)
        {
            if (goSummonBtn != null) NGUITools.SetActive(goSummonBtn, true);
        }
        else
        {
            if (goSummonBtn != null) NGUITools.SetActive(goSummonBtn, false);
        }
        if (lblPartnerFragNum != null) lblPartnerFragNum.text = ownFragNum + "/" + fragNeedNum;
        if (progressBar != null && fragNeedNum != 0)
        {
            progressBar.value = ownFragNum / (float)fragNeedNum;
        }
        m_CanCompoundPartner = ownFragNum >= fragNeedNum;//可兑换伙伴
        Data_NpcConfig npcCfg = NpcConfigProvider.Instance.GetNpcConfigById(partner_cfg.LinkId);
        if (npcCfg != null)
        {
            if (spPortait != null) spPortait.spriteName = npcCfg.m_Portrait;
        }
        ItemConfig itemCfg = ItemConfigProvider.Instance.GetDataById(m_PartnerFragId);
        if (itemCfg != null)
        {
            if (lblPartnerName != null) lblPartnerName.text = itemCfg.m_ItemName;
        }
        spRankColor.color = RankColor[0];
    }
    public void UpdatePartnerInfo(PartnerInfo info)
    {
        if (info == null) return;
        //设置战力
        int partnerFighting = GetPartnerFighting(info.GetAppendAttrConfigId());
        m_PartnerFighting = partnerFighting;
        if (lblFighting != null) lblFighting.text = partnerFighting.ToString();
        if (info.CurAdditionLevel <= RankOffset.Length)
        {
            if (RankOffset[info.CurAdditionLevel - 1] == 0)
            {
                if (lblRankOffset != null) lblRankOffset.text = "";
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
            if (lblPartnerName != null) lblPartnerName.color = RankColor[info.CurAdditionLevel - 1];
            lblRankOffset.color = RankColor[info.CurAdditionLevel - 1];
            spRankColor.color = RankColor[info.CurAdditionLevel - 1];
        }
    }
    //是否出战
    public void SetPlayed(bool isPlayed)
    {
        m_Isplayed = isPlayed;
        if (isPlayed)
        {
            if (goPlayed != null)
            {
                UISprite sp = goPlayed.GetComponent<UISprite>();
                if (sp != null) sp.enabled = true;
                NGUITools.SetActive(goPlayed, true);
            }

        }
        else
        {
            if (goPlayed != null)
            {
                UISprite sp = goPlayed.GetComponent<UISprite>();
                if (sp != null) sp.enabled = false;
                NGUITools.SetActive(goPlayed, false);
            }
        }
    }
    //
    public void SetSelectedFlagVisible(bool visible)
    {
        if (spSelectFlag != null)
        {
            NGUITools.SetActive(spSelectFlag.gameObject, visible);
        }
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
    public int GetPartnerFighting()
    {
        return m_PartnerFighting;
    }
    public bool CanCompound()
    {
        return m_CanCompoundPartner;
    }
    private void SetItemStyle(UIPartnerItemStyle style)
    {
        bool visible = (style == UIPartnerItemStyle.PartnerPlayed);
        if (goFighting != null) NGUITools.SetActive(goFighting, visible);
        if (goPlayed != null) NGUITools.SetActive(goPlayed, visible);
        if (lblRankOffset != null)
        {
            NGUITools.SetActive(lblRankOffset.gameObject, visible);
            if (!visible) lblRankOffset.text = "";//发现会存在deactive不隐藏的情况
        }
        if (progressBar != null) NGUITools.SetActive(progressBar.gameObject, !visible);
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

    public void SetTipActive(bool active)
    {
        UnityEngine.Transform tf = transform.Find("Tip");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, active);
        }
    }

    public int PartnerId
    {
        get
        {
            return m_PartnerId;
        }
    }
    private int m_PartnerId = -1;
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
