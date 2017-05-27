using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public enum UIPartnerInfoTypeEnum
{
    Biography,
    Strengthen,
    SkillLift
}
public class UIPartnerInfo : UnityEngine.MonoBehaviour
{

    public UnityEngine.GameObject goBiography;
    public UnityEngine.GameObject goStrengthen;
    public UnityEngine.GameObject goSkillLift;
    private UnityEngine.GameObject m_GoRuntimeShowInfo;
    //传记
    public UILabel lblBiography;
    //强化
    private const int c_AppendAttrNumMax = 5;
    public UILabel lblCurrentInheritAttack;
    public UILabel lblCurrentInheritDefence;
    public UILabel lblNextInheritAttack;
    public UILabel lblNextInheritDefence;
    public UILabel[] lblCurrentAppendAttrArr = new UILabel[c_AppendAttrNumMax];
    public UILabel[] lblNextAppendAttrArr = new UILabel[c_AppendAttrNumMax];
    public UILabel lblSuccessRate;
    public UILabel lblFailedResult;
    public UILabel lblSItemName;
    public UILabel lblSItemNum;
    public UILabel lblStrengthenBtn;
    public UIButton btnStrengthen;
    public UnityEngine.GameObject goFullLevel;//满级
    public UnityEngine.GameObject goAttrContainer;
    //技能进阶
    private const int c_SkillNumMax = 4;
    public UILabel[] lblLifts = new UILabel[c_SkillNumMax];
    public UILabel lblLiftItemName = null;
    public UILabel lblLiftItemNum = null;

    public UISprite[] spLift = new UISprite[c_SkillNumMax];
    public UISprite[] spArrow = new UISprite[3];

    public UIProgressBar itemProgressBar;
    public UITexture textureLiftGoods = null;
    public UIButton btnLiftUp = null;
    public UnityEngine.Color SectionColor = new UnityEngine.Color();
    public UnityEngine.Color AshColor = new UnityEngine.Color();
    public UnityEngine.Color LightColor = new UnityEngine.Color();

    private int m_PartnerLevelUpItemId = -1;
    private int m_PartnerSkillLiftUpItemId = -1;
    private int m_PartnerId = -1;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //点击强化按钮
    public void OnStrenthenClick()
    {
        LogicSystem.PublishLogicEvent("ge_upgrade_partner_level", "partner", m_PartnerId);
    }
    //点进阶按钮
    public void OnLiftSkillClick()
    {
        LogicSystem.PublishLogicEvent("ge_upgrade_partner_stage", "partner", m_PartnerId);
    }
    //点击升阶材料按钮
    public void OnLiftSkillItemClick()
    {
        LogicSystem.EventChannelForGfx.Publish("ge_item_come_from", "ui", m_PartnerSkillLiftUpItemId);
    }
    //点击伙伴升级材料按钮
    public void OnPartnerLevelUpItemClick()
    {
        LogicSystem.EventChannelForGfx.Publish("ge_item_come_from", "ui", m_PartnerLevelUpItemId);
    }
    //根据类型显示伙伴信息
    public void ShowPartnerInfoType(UIPartnerInfoTypeEnum info_type)
    {
        if (!NGUITools.GetActive(this))
            NGUITools.SetActive(this.gameObject, true);
        NGUITools.SetActive(m_GoRuntimeShowInfo, false);
        switch (info_type)
        {
            case UIPartnerInfoTypeEnum.Biography:
                ShowPartnerInfo(goBiography);
                break;
            case UIPartnerInfoTypeEnum.Strengthen:
                ShowPartnerInfo(goStrengthen);
                break;
            case UIPartnerInfoTypeEnum.SkillLift:
                ShowPartnerInfo(goSkillLift);
                break;
            default: break;
        }
    }
    private void ShowPartnerInfo(UnityEngine.GameObject go)
    {
        m_GoRuntimeShowInfo = go;
        NGUITools.SetActive(m_GoRuntimeShowInfo, true);
    }
    public void OnCloseButtonClick()
    {
        NGUITools.SetActive(this.gameObject, false);
        UIPartnerPanel partnerPanel = NGUITools.FindInParents<UIPartnerPanel>(gameObject);
        if (partnerPanel != null)
            partnerPanel.OnClosePartnerInfo();
    }
    //设置伙伴信息
    public void SetPartnerInfo(PartnerInfo info)
    {
        if (null == info) return;
        m_PartnerId = info.Id;
        PartnerConfig cfg = PartnerConfigProvider.Instance.GetDataById(info.Id);

        if (cfg != null)
        {
            SetBiographyDesc(cfg.Story);
            SetLiftSkillInfo(cfg, info.CurSkillStage, info.StageUpItemId);
            int currentId = info.GetAppendAttrConfigId();
            int nextAddtionLevel = info.CurAdditionLevel + 1;
            int nextId = -1;
            if (nextAddtionLevel <= cfg.AttrAppendList.Count)
                nextId = cfg.AttrAppendList[nextAddtionLevel - 1];
            //设置Current继承属性
            if (nextId == -1)
            {
                //表示已满级
                if (goAttrContainer != null) NGUITools.SetActive(goAttrContainer, false);
                if (goFullLevel != null) NGUITools.SetActive(goFullLevel, true);
                string str_des = StrDictionaryProvider.Instance.GetDictString(705);
                if (lblStrengthenBtn != null) lblStrengthenBtn.text = str_des;
                if (btnStrengthen != null)
                {
                    btnStrengthen.defaultColor = AshColor;
                    btnStrengthen.isEnabled = false;
                }
            }
            else
            {
                if (goAttrContainer != null && !NGUITools.GetActive(goAttrContainer)) NGUITools.SetActive(goAttrContainer, true);
                if (goFullLevel != null && !NGUITools.GetActive(goFullLevel)) NGUITools.SetActive(goFullLevel, false);
                string str_des = StrDictionaryProvider.Instance.GetDictString(706);
                if (lblStrengthenBtn != null) lblStrengthenBtn.text = str_des;
                if (btnStrengthen != null)
                {
                    btnStrengthen.defaultColor = LightColor;
                    btnStrengthen.isEnabled = true;
                }
            }
            float inheritAttack = info.GetInheritAttackAttrPercent();
            if (lblCurrentInheritAttack != null) lblCurrentInheritAttack.text = (inheritAttack * 100) + "%";
            float inheritDefence = info.GetInheritDefenceAttrPercent();
            if (lblCurrentInheritDefence != null) lblCurrentInheritDefence.text = (inheritDefence * 100) + "%";
            //设置Next继承属性
            inheritAttack = info.GetInheritAttackAttrPercent(nextAddtionLevel);
            if (lblNextInheritAttack != null) lblNextInheritAttack.text = (inheritAttack * 100) + "%";
            inheritDefence = info.GetInheritDefenceAttrPercent(nextAddtionLevel);
            if (lblNextInheritDefence != null) lblNextInheritDefence.text = (inheritDefence * 100) + "%";
            SetStrengthenAttr(currentId, nextId, info);
        }
    }
    public void SetBiographyDesc(string des)
    {
        if (lblBiography != null)
            des = des.Replace("[\\n]", "\n");
        lblBiography.text = des;
    }
    //设置伙伴属性信息
    public void SetStrengthenAttr(int currentAppendAttrId, int nextAppendAttrId, PartnerInfo info)
    {
        SetStrengthenAttr(currentAppendAttrId, lblCurrentAppendAttrArr);
        SetStrengthenAttr(nextAppendAttrId, lblNextAppendAttrArr);
        if (info == null) return;
        PartnerLevelUpConfig partnerLvcfg = PartnerLevelUpConfigProvider.Instance.GetDataById(info.CurAdditionLevel);
        if (partnerLvcfg != null)
        {
            if (lblSuccessRate != null) lblSuccessRate.text = partnerLvcfg.Rate * 100 + "%";
            if (partnerLvcfg.IsFailedDemote)
            {
                //降品质
                PartnerLevelUpConfig cfg = PartnerLevelUpConfigProvider.Instance.GetDataById(info.CurAdditionLevel - 1);
                //if (cfg != null && lblFailedResult != null) lblFailedResult.text = cfg.PartnerRank;
                string chn_des = StrDictionaryProvider.Instance.GetDictString(703);
                if (lblFailedResult != null)
                {
                    lblFailedResult.text = chn_des;
                    NGUITools.SetActive(lblFailedResult.gameObject, true);
                }
            }
            else
            {
                if (lblFailedResult != null)
                {
                    NGUITools.SetActive(lblFailedResult.gameObject, false);
                }
            }
        }
        m_PartnerLevelUpItemId = info.LevelUpItemId;
        ItemConfig itemCfg = ItemConfigProvider.Instance.GetDataById(info.LevelUpItemId);
        int ownItemNum = GetItemNum(info.LevelUpItemId);
        if (itemCfg != null)
        {
            if (lblSItemName != null)
                lblSItemName.text = itemCfg.m_ItemName + "x" + partnerLvcfg.ItemCost;
            DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Partner_Strengthen, goStrengthen, info.LevelUpItemId);
        }
        if (lblSItemNum != null && partnerLvcfg != null)
        {
            lblSItemNum.text = ownItemNum.ToString();
            EnableButton(btnStrengthen, ownItemNum >= partnerLvcfg.ItemCost && nextAppendAttrId != -1);
        }
    }

    private void SetStrengthenAttr(int appendAttrId, UILabel[] lblAppendAttrArr)
    {
        if (lblAppendAttrArr == null) return;
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
    //设置升阶信息
    public void SetLiftSkillInfo(PartnerConfig partnerCfg, int stage, int itemId)
    {
        if (partnerCfg == null) return;
        List<string> iconList = new List<string>();
        if (iconList == null) return;
        iconList.Add(partnerCfg.Icon0);
        iconList.Add(partnerCfg.Icon1);
        iconList.Add(partnerCfg.Icon2);
        iconList.Add(partnerCfg.Icon3);
        List<string> DescList = new List<string>();
        if (DescList == null) return;
        DescList.Add(partnerCfg.StageDescription0);
        DescList.Add(partnerCfg.StageDescription1);
        DescList.Add(partnerCfg.StageDescription2);
        DescList.Add(partnerCfg.StageDescription3);
        UIAtlas atlas = null;
        UnityEngine.GameObject goAtlas = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.GetSharedResource(partnerCfg.AtlasPath));
        if (goAtlas != null)
            atlas = goAtlas.GetComponent<UIAtlas>();
        for (int index = 0; index < c_SkillNumMax; ++index)
        {
            if (spLift[index] != null && index < iconList.Count)
            {
                spLift[index].spriteName = iconList[index];
                spLift[index].atlas = atlas;
            }
            //设置技能图标
            if (lblLifts[index] != null && index < DescList.Count)
            {
                lblLifts[index].text = DescList[index];
            }
            if (stage >= index + 1)
                spLift[index].color = LightColor;
            else
                spLift[index].color = AshColor;

            //设置进阶箭头
            if (index < spArrow.Length && spArrow[index] != null)
            {
                if (stage > index + 1)
                {
                    spArrow[index].spriteName = "sheng-ji-jian-tou1";
                }
                else
                {
                    spArrow[index].spriteName = "sheng-ji-jian-tou2";
                }
            }
            if (stage >= index + 1)
            {
                lblLifts[index].color = SectionColor;
            }
            else
            {
                lblLifts[index].color = AshColor;
            }
        }

        m_PartnerSkillLiftUpItemId = itemId;
        DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Partner_Skill, goSkillLift, itemId);
        if (stage < 4)
        {
            //if (textureLiftGoods != null) NGUITools.SetActive(textureLiftGoods.gameObject, true);
            if (lblLiftItemNum != null) NGUITools.SetActive(lblLiftItemNum.gameObject, true);
            if (btnLiftUp != null) NGUITools.SetActive(btnLiftUp.gameObject, true);
            int itemNeedNum = 0;
            ItemConfig itemCfg = ItemConfigProvider.Instance.GetDataById(itemId);
            if (itemCfg != null)
            {
                PartnerStageUpConfig stageUpCfg = PartnerStageUpConfigProvider.Instance.GetDataById(stage);
                if (stageUpCfg != null)
                {
                    itemNeedNum = stageUpCfg.ItemCost;
                }
                int ownItemNum = GetItemNum(itemId);
                if (lblLiftItemNum != null) lblLiftItemNum.text = ownItemNum + "/" + itemNeedNum;
                if (itemProgressBar != null && itemNeedNum != 0) itemProgressBar.value = ownItemNum / (float)itemNeedNum;
                bool enable = (ownItemNum >= itemNeedNum);
                EnableButton(btnLiftUp, enable);
            }
        }
        else
        {
            string CHN = StrDictionaryProvider.Instance.GetDictString(359);
            if (lblLiftItemName != null) lblLiftItemName.text = CHN;
            if (itemProgressBar != null) itemProgressBar.value = 1;
            if (btnLiftUp != null) NGUITools.SetActive(btnLiftUp.gameObject, false);
            //if (textureLiftGoods != null) NGUITools.SetActive(textureLiftGoods.gameObject, false);
            if (lblLiftItemNum != null) NGUITools.SetActive(lblLiftItemNum.gameObject, false);
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
    private void EnableButton(UIButton btn, bool enable)
    {
        if (btn == null) return;
        if (enable)
        {
            btn.defaultColor = UnityEngine.Color.white;
            btn.enabled = true;
            btn.isEnabled = true;
        }
        else
        {
            btn.defaultColor = AshColor;
            btn.enabled = false;
            btn.isEnabled = false;
        }
    }
}
