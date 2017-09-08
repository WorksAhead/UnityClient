using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class UIPartnerList : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject goPartnerItem = null;
    public UnityEngine.GameObject goItemContainer = null;
    private const int c_PartnerNumMax = 99;
    private List<UIPartnerItem> m_PartnerItemList = new List<UIPartnerItem>();
    private Dictionary<int, bool> canUpgradeDic = new Dictionary<int, bool>();
    // Use this for initialization
    void Awake()
    {
        try
        {
            //新手引导时必须存在
            InitPartnerList();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Start()
    {
        try
        {
            InitPartnerList();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
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
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            if (role_info != null)
            {
                PartnerStateInfo state_info = role_info.PartnerStateInfo;
                if (state_info == null) return;
                int active_id = state_info.GetActivePartnerId();
                PartnerReposition(active_id);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void InitPartnerList(int test)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            PartnerStateInfo state_info = role_info.PartnerStateInfo;
            if (state_info == null) return;
            int active_id = state_info.GetActivePartnerId();
            List<PartnerInfo> partnerList = state_info.GetAllPartners();
            if (partnerList != null)
            {
                for (int index = 0; index < partnerList.Count; ++index)
                {
                    //AddExitPartnerItem(partnerList[index],active_id);
                }

                int firstPartnerId = PartnerReposition(active_id);
                UIPartnerPanel partnerPanel = NGUITools.FindInParents<UIPartnerPanel>(this.gameObject);
                if (partnerPanel != null) partnerPanel.SetPartnerInfo(firstPartnerId);
            }
        }
    }
    //初始化伙伴列表
    public void InitPartnerList()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info == null || role_info.PartnerStateInfo == null) return;
        List<PartnerConfig> partnerCfgList = PartnerConfigProvider.Instance.GetAllData();
        int active_id = role_info.PartnerStateInfo.GetActivePartnerId();
        int havenAddedPartnerCount = 0;
        for (int index = 0; index < partnerCfgList.Count; ++index)
        {
            if (partnerCfgList[index] != null)
            {
                int partnerId = partnerCfgList[index].Id;
                if (role_info.PartnerStateInfo.IsHavePartner(partnerId))
                {
                    PartnerInfo info = role_info.PartnerStateInfo.GetPartnerInfoById(partnerId);
                    if (AddExitPartnerItem(info, active_id, havenAddedPartnerCount))
                        havenAddedPartnerCount++;
                }
                else
                {
                    if (AddNotHavenPartnerItem(partnerCfgList[index], havenAddedPartnerCount))
                        havenAddedPartnerCount++;
                }
            }
        }
        int firstPartnerId = PartnerReposition(active_id);
        UIScrollView scrollview = NGUITools.FindInParents<UIScrollView>(goItemContainer);
        if (scrollview != null)
        {
            scrollview.ResetPosition();
        }
        UIPartnerPanel partnerPanel = NGUITools.FindInParents<UIPartnerPanel>(this.gameObject);
        if (partnerPanel != null) partnerPanel.SetPartnerInfo(firstPartnerId);
    }
    //添加已经获得的伙伴
    private bool AddExitPartnerItem(PartnerInfo partner_info, int active_id, int partnerIndex)
    {
        if (partner_info == null) return false;
        UIPartnerItem partner_item = TryGetPartnerItem(partnerIndex);
        if (partner_item != null)
        {
            partner_item.InitPartnerItem(partner_info, active_id);
            if (canUpgradeDic != null)
            {
                bool value = false;
                canUpgradeDic.TryGetValue(partner_info.Id, out value);
                partner_item.SetTipActive(value);
            }
            return true;
        }
        return false;
    }
    //添加暂未获得伙伴
    private bool AddNotHavenPartnerItem(PartnerConfig cfg, int partnerIndex)
    {
        if (cfg == null) return false;
        int ownItemNum = GetItemNum(cfg.PartnerFragId);
        //当前拥有碎片数小于等于0的不加载
        if (ownItemNum <= 0) return false;
        UIPartnerItem partner_item = TryGetPartnerItem(partnerIndex);
        if (null != partner_item)
        {
            partner_item.InitNotHavenPartner(cfg);
            return true;
        }
        return false;
    }
    //
    private UIPartnerItem TryGetPartnerItem(int index)
    {
        if (index < m_PartnerItemList.Count && m_PartnerItemList[index] != null)
        {
            return m_PartnerItemList[index];
        }
        if (index < m_PartnerItemList.Count && m_PartnerItemList[index] == null) m_PartnerItemList.Remove(m_PartnerItemList[index]);
        if (goItemContainer != null && goPartnerItem != null)
        {
            UnityEngine.GameObject goChild = NGUITools.AddChild(goItemContainer, goPartnerItem);
            if (goChild != null)
            {
                UIPartnerItem partner_item = goChild.GetComponent<UIPartnerItem>();
                if (partner_item != null)
                {
                    m_PartnerItemList.Add(partner_item);
                    return partner_item;
                }
            }
        }
        return null;
    }
    //根据Id设置伙伴出战
    public void SetPartnerPlayed(int prePlayedId, int curPlayedId)
    {
        for (int index = 0; index < m_PartnerItemList.Count; ++index)
        {
            if (m_PartnerItemList[index] != null)
            {
                if (m_PartnerItemList[index].PartnerId == prePlayedId)
                {
                    m_PartnerItemList[index].SetPlayed(false);
                }
                if (m_PartnerItemList[index].PartnerId == curPlayedId)
                {
                    m_PartnerItemList[index].SetPlayed(true);
                    PartnerReposition(curPlayedId);
                }
            }
        }
    }
    //设置PartnerItem的选中状态
    public void SetPartnerSelected(int partnerId, bool visible)
    {
        UIPartnerItem item = GetPartnerItemById(partnerId);
        if (item != null)
            item.SetSelectedFlagVisible(visible);
    }
    //伙伴根据战力排序,返回最上面的PartnerId
    public int PartnerReposition(int playedPartnerId)
    {
        m_PartnerItemList.Sort(Comp);
        for (int i = 0; i < m_PartnerItemList.Count; ++i)
        {
            if (m_PartnerItemList[i] != null)
                m_PartnerItemList[i].gameObject.name = (c_PartnerNumMax - i).ToString();
        }
        UIPartnerItem partnerItem = GetPartnerItemById(playedPartnerId);
        if (null != partnerItem) partnerItem.name = "0";
        if (goItemContainer != null)
        {
            UIGrid grid = goItemContainer.GetComponent<UIGrid>();
            if (grid != null) grid.Reposition();
        }
        if (playedPartnerId != -1) return playedPartnerId;
        if (m_PartnerItemList.Count > 0)
            return m_PartnerItemList[m_PartnerItemList.Count - 1].PartnerId;
        return -1;
    }
    //
    public int Comp(UIPartnerItem item1, UIPartnerItem item2)
    {
        if (item1 == null && item2 == null) return 0;
        if (item1 == null)
            return -1;
        if (item2 == null)
            return 1;
        if (item1.GetPartnerFighting() >= item2.GetPartnerFighting())
            return 1;
        else
            return -1;
    }
    //
    public void UpdatePartnerInfo(PartnerInfo info)
    {
        if (info == null) return;
        UIPartnerItem partner_item = GetPartnerItemById(info.Id);
        if (partner_item != null)
        {
            partner_item.UpdatePartnerInfo(info);
        }
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            PartnerStateInfo state_info = role_info.PartnerStateInfo;
            if (state_info == null) return;
            int active_id = state_info.GetActivePartnerId();
            PartnerReposition(active_id);
        }
    }
    //
    private UIPartnerItem GetPartnerItemById(int partnerId)
    {
        for (int i = 0; i < m_PartnerItemList.Count; ++i)
        {
            if (m_PartnerItemList[i] != null && m_PartnerItemList[i].PartnerId == partnerId)
                return m_PartnerItemList[i];
        }
        return null;
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

    public void SetItemTipActive(int partnerId, bool active)
    {
        if (canUpgradeDic == null)
        {
            canUpgradeDic = new Dictionary<int, bool>();
        }
        canUpgradeDic[partnerId] = active;
        if (m_PartnerItemList != null)
        {
            for (int i = 0; i < m_PartnerItemList.Count; i++)
            {
                if (m_PartnerItemList[i].PartnerId == partnerId)
                {
                    m_PartnerItemList[i].SetTipActive(active);
                }
            }
        }
    }
}
