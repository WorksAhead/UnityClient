using UnityEngine;
using System.Collections;
using ArkCrossEngine;
using System.Collections.Generic;

public class UIFighterIntro : UnityEngine.MonoBehaviour
{

    public UISprite spHead = null;
    public UILabel lblName = null;
    public UILabel lblFightScore = null;
    public UILabel lblRank = null;
    public UILabel lblTotalFightScore = null;
    public UIGrid gridPartnersParent = null;
    public UnityEngine.GameObject gridEquipParent = null;
    public UnityEngine.GameObject partnerSlot = null;

    private const int c_PartnerNum = 3;
    private bool hasStart = false;
    private List<UIPartnerSlot> partnerSlotList = new List<UIPartnerSlot>();
    private EquipmentInfo[] equiparry = { new EquipmentInfo(), new EquipmentInfo(),
                                        new EquipmentInfo(), new EquipmentInfo(),
                                        new EquipmentInfo(), new EquipmentInfo() };
    static private string[] defaultEquipSprite = { "wu-qi-biao", "tou-kui-biao",
                                                 "yi-fu-biao", "xie-zi-biao",
                                                 "gua-shi-biao", "jie-zhi-biao" };

    void Start()
    {
        try
        {
            if (hasStart == false)
            {
                if (gridEquipParent != null)
                {
                    int childNum = gridEquipParent.transform.childCount;
                    for (int i = 0; i < childNum; i++)
                    {
                        UnityEngine.GameObject go = gridEquipParent.transform.GetChild(i).gameObject;
                        int index = -1;
                        System.Int32.TryParse(go.name, out index);
                        if (index >= 0 && index < childNum)
                        {
                            equiparry[index].equipSlot = go;
                        }
                        UIEventListener.Get(go).onClick = SlotButtonClick;
                    }
                }
                if (gridPartnersParent != null)
                {
                    int oldDataNum = gridPartnersParent.transform.childCount;
                    for (int i = 0; i < oldDataNum; i++)
                    {
                        DestroyImmediate(gridPartnersParent.transform.GetChild(i).gameObject);
                    }
                    for (int i = 0; i < c_PartnerNum; i++)
                    {
                        if (partnerSlot != null)
                        {
                            UnityEngine.GameObject item = NGUITools.AddChild(gridPartnersParent.gameObject, partnerSlot);
                            UIPartnerSlot slot = item.GetComponent<UIPartnerSlot>();
                            if (slot != null)
                            {
                                partnerSlotList.Add(slot);
                            }
                        }
                    }
                    gridPartnersParent.repositionNow = true;
                }
            }
            hasStart = true;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    //对外接口，初始化信息
    public void ShowIntro(ArenaTargetInfo info)
    {
        if (hasStart == false)
        {
            Start();
        }
        if (info != null)
        {
            if (lblName != null)
            {
                lblName.text = info.Nickname;
            }
            if (lblFightScore != null)
            {
                lblFightScore.text = info.FightingScore.ToString();
            }
            if (lblRank != null)
            {
                lblRank.text = info.Rank.ToString();
            }
            if (spHead != null)
            {
                Data_PlayerConfig cg = PlayerConfigProvider.Instance.GetPlayerConfigById(info.HeroId);
                spHead.spriteName = cg.m_PortraitForCell;
            }
            //装备
            for (int i = 0; i < equiparry.Length; i++)
            {
                equiparry[i].hasEquip = false;
            }
            for (int i = 0; i < info.Equips.Length; i++)
            {
                ItemDataInfo item = info.Equips[i];
                if (item != null)
                {
                    ItemConfig config = ItemConfigProvider.Instance.GetDataById(item.ItemId);
                    if (config != null && config.m_WearParts < equiparry.Length)
                    {
                        equiparry[config.m_WearParts].SetEquipmentInfo(item.ItemId, item.Level, item.RandomProperty, 1);
                    }
                }
            }
            for (int i = 0; i < equiparry.Length; i++)
            {
                if (equiparry[i].hasEquip == false)
                {
                    equiparry[i].SetFrameAndIcon(defaultEquipSprite[i]);//无装备设为默认图标
                }
            }
            //伙伴
            int totalFightScore = info.FightingScore;
            for (int i = 0; i < partnerSlotList.Count; i++)
            {
                if (i < info.FightPartners.Count)
                {
                    partnerSlotList[i].InitPartnerInfo(info.FightPartners[i]);
                    totalFightScore += partnerSlotList[i].GetFighting();
                    NGUITools.SetActive(partnerSlotList[i].gameObject, true);
                }
                else
                {
                    NGUITools.SetActive(partnerSlotList[i].gameObject, false);
                }
            }
            if (gridPartnersParent != null)
            {
                gridPartnersParent.repositionNow = true;
            }
            if (lblTotalFightScore != null)
            {
                lblTotalFightScore.text = totalFightScore.ToString();//所有伙伴战力
            }

            UIManager.Instance.ShowWindowByName("PPVPFighterIntro");
        }
    }

    //装备点击（装备tip）
    void SlotButtonClick(UnityEngine.GameObject go)
    {
        if (go == null)
            return;
        int pos = 0;
        System.Int32.TryParse(go.name, out pos);

        UnityEngine.GameObject ipgo = UIManager.Instance.GetWindowGoByName("ItemProperty");
        if (ipgo != null && !NGUITools.GetActive(ipgo))
        {
            ItemProperty ip = ipgo.GetComponent<ItemProperty>();
            ip.SetItemProperty(equiparry[pos].id, pos, equiparry[pos].level, equiparry[pos].propertyid, false, true);
            UIManager.Instance.ShowWindowByName("ItemProperty");
        }
    }

    public void OnClickClose()
    {
        UIManager.Instance.HideWindowByName("ItemProperty");
        UIManager.Instance.HideWindowByName("PPVPFighterIntro");
    }
}
