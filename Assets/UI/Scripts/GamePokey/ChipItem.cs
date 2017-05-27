using UnityEngine;
using System.Collections;
using ArkCrossEngine;
using System;

public class ChipItem : UnityEngine.MonoBehaviour
{

    public UILabel lblNum = null;
    public UIButton btnCompound = null;
    public UIProgressBar progress = null;

    private int id;
    private int property;
    private int hasNum = 0;
    private int maxNum = 0;
    private bool m_CanCompound = false;

    public bool CanCompound
    {
        get
        {
            return m_CanCompound;
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal int Id()
    {
        return id;
    }
    internal int Property()
    {
        return property;
    }

    internal void SetItemInformation(int itemId, int _property, int itemNum)
    {
        ItemConfig item_data = ItemConfigProvider.Instance.GetDataById(itemId);
        if (item_data == null)
        {
            return;
        }
        id = itemId;
        property = _property;
        hasNum = itemNum;
        DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Equip_List, this.gameObject, itemId);
        ItemConfig ic = ItemConfigProvider.Instance.GetDataById(itemId);
        if (ic != null)
        {
            int compItemId = ic.m_CompoundItemId[0];
            if (compItemId > 0)
            {
                ItemCompoundConfig icc = ItemCompoundConfigProvider.Instance.GetDataById(compItemId);
                if (icc != null)
                {
                    maxNum = icc.m_PartNum;
                }
            }
        }
        if (lblNum != null)
        {
            lblNum.text = hasNum + "/" + maxNum;
        }
        if (progress != null)
        {
            float value = (float)hasNum / maxNum;
            progress.value = value > 1 ? 1 : value;
        }
        UpdateBtnState();
    }

    private void UpdateBtnState()
    {
        m_CanCompound = hasNum >= maxNum ? true : false;
        UnityEngine.Transform tfTip = transform.Find("Tip");
        if (tfTip != null)
        {
            //NGUITools.SetActive(tfTip.gameObject, m_CanCompound);
        }
        NGUITools.SetActive(btnCompound.gameObject, m_CanCompound);
    }

    public void OnClickCompound()
    {
        if (hasNum >= maxNum)
        {
            ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("compound_equip", "lobby", id);
        }
        else
        {
            UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("ItemSourceTips");
            if (go != null && !NGUITools.GetActive(go))
            {
                UIItemSourceTips ip = go.GetComponent<UIItemSourceTips>();
                ip.InitSourceTips(id);
            }
        }
    }

    internal void UpdateView(int itemNum)
    {
        RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
        if (roleInfo != null)
        {
            ItemDataInfo itemDataInfo = roleInfo.GetItemData(id, property);
            if (itemDataInfo != null)
            {
                hasNum = itemDataInfo.ItemNum;
            }
        }
        if (lblNum != null)
        {
            lblNum.text = hasNum + "/" + maxNum;
        }
        if (progress != null)
        {
            float value = (float)hasNum / maxNum;
            progress.value = value > 1 ? 1 : value;
        }
        UpdateBtnState();
    }
}
