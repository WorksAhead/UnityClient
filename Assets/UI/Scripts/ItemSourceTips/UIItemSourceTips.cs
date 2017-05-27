using System;
using System.Collections.Generic;
using ArkCrossEngine;

public class UIItemSourceTips : UnityEngine.MonoBehaviour
{
    public UILabel lblItemNum;
    public UILabel lblItemSourceDesc;
    public UIItemSourceItem[] sourceItemArr = new UIItemSourceItem[3];
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
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Use this for initialization
    void Awake()
    {
        try
        {
            if (eventlist != null) eventlist.Clear();
            object eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (eo != null) eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("ge_item_come_from", "ui", InitSourceTips);
            if (eo != null) eventlist.Add(eo);
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
    public void InitSourceTips(int itemId)
    {
        ItemConfig itemCfg = ItemConfigProvider.Instance.GetDataById(itemId);
        if (itemCfg == null) return;
        string chn_des = StrDictionaryProvider.Instance.GetDictString(165);
        chn_des = string.Format(chn_des, GetItemNum(itemId));
        if (lblItemNum != null) lblItemNum.text = chn_des;
        DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Item_Source, this.gameObject, itemId);
        if (itemCfg.m_ItemSourceScene != null && itemCfg.m_ItemSourceScene.Count > 0)
        {
            for (int i = 0; i < itemCfg.m_ItemSourceScene.Count; ++i)
            {
                if (itemCfg.m_ItemSourceScene[i] != -1)
                {
                    if (i < sourceItemArr.Length && sourceItemArr[i] != null)
                    {
                        sourceItemArr[i].SetSourceInfo(itemCfg.m_ItemSourceScene[i]);
                        NGUITools.SetActive(sourceItemArr[i].gameObject, true);
                    }
                }
            }
            for (int j = itemCfg.m_ItemSourceScene.Count; j < sourceItemArr.Length; ++j)
            {
                if (sourceItemArr[j] != null)
                    NGUITools.SetActive(sourceItemArr[j].gameObject, false);
            }
            //清空lbl内容，代替隐藏了
            if (lblItemSourceDesc != null) lblItemSourceDesc.text = "";
        }
        else
        {
            for (int index = 0; index < sourceItemArr.Length; ++index)
            {
                if (sourceItemArr[index] != null)
                    NGUITools.SetActive(sourceItemArr[index].gameObject, false);
            }
            string des = itemCfg.m_ItemSourceDesc.Replace("[\\n]", "\n");
            if (lblItemSourceDesc != null) lblItemSourceDesc.text = des;
        }
        UIManager.Instance.ShowWindowByName("ItemSourceTips");
    }
    public void OnCloseButtonClick()
    {
        UIManager.Instance.HideWindowByName("ItemSourceTips");
    }
    public void OnBackgroundButtonClick()
    {
        UIManager.Instance.HideWindowByName("ItemSourceTips");
    }
    private int GetItemNum(int itemId)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            if (itemId == ItemConfigProvider.Instance.GetDiamondId())
            {
                return role_info.Gold;
            }
            else if (itemId == ItemConfigProvider.Instance.GetGoldId())
            {
                return role_info.Money;
            }
            else if (role_info.Items != null)
            {
                for (int i = 0; i < role_info.Items.Count; ++i)
                {
                    if (role_info.Items[i] != null && role_info.Items[i].ItemId == itemId)
                        return role_info.Items[i].ItemNum;
                }
            }
        }
        return 0;
    }
}
