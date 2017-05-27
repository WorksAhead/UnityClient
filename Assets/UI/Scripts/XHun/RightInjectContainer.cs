using UnityEngine;
using System;
using System.Collections;
using ArkCrossEngine;

public enum HunType : int
{
    None,
    Small,
    Middle,
    Big
}
public class RightInjectContainer : UnityEngine.MonoBehaviour
{

    public UILabel labelSmallNum = null;
    public UILabel labelMiddleNum = null;
    public UILabel labelBigNum = null;

    public XHunItem itemSmall = null;
    public XHunItem itemMiddle = null;
    public XHunItem itemBig = null;

    public UIButton injectButton = null;

    private int numSmall = 0;
    private int numMiddle = 0;
    private int numBig = 0;

    void Start()
    {

    }

    public HunType GetSelectHunType()
    {
        if (itemSmall != null && itemSmall.isSelect == true)
        {
            return HunType.Small;
        }
        if (itemMiddle != null && itemMiddle.isSelect == true)
        {
            return HunType.Middle;
        }
        if (itemBig != null && itemBig.isSelect == true)
        {
            return HunType.Big;
        }
        return HunType.None;
    }

    public void UpdateHunNum(int[] hunIds)
    {
        ItemDataInfo need_item;
        if (labelSmallNum != null)
        {

            labelSmallNum.text = "0";
            if (itemSmall != null)
                itemSmall.UpdateView(hunIds[0]);
            need_item = GetItem(hunIds[0]);
            numSmall = need_item == null ? 0 : need_item.ItemNum;
            DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Xhun_item, itemSmall.gameObject, hunIds[0], numSmall);
        }
        if (labelMiddleNum != null)
        {
            labelMiddleNum.text = "0";
            if (itemMiddle != null)
                itemMiddle.UpdateView(hunIds[1]);
            need_item = GetItem(hunIds[1]);
            numMiddle = need_item == null ? 0 : need_item.ItemNum;
            DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Xhun_item, itemMiddle.gameObject, hunIds[1], numMiddle);
        }
        if (labelBigNum != null)
        {
            labelBigNum.text = "0";
            if (itemBig != null)
                itemBig.UpdateView(hunIds[2]);
            need_item = GetItem(hunIds[2]);
            numBig = need_item == null ? 0 : need_item.ItemNum;
            DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Xhun_item, itemBig.gameObject, hunIds[2], numBig);
        }

        UpdateInjectButtonState();
    }

    private ItemDataInfo GetItem(int itemid)
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        foreach (ItemDataInfo item in role.Items)
        {
            if (item.ItemId == itemid)
            {
                return item;
            }
        }
        return null;
    }

    public void UpdateInjectButtonState()
    {
        HunType type = GetSelectHunType();
        switch (type)
        {
            case HunType.Small:
                EnableButton(numSmall > 0);
                break;
            case HunType.Middle:
                EnableButton(numMiddle > 0);
                break;
            case HunType.Big:
                EnableButton(numBig > 0);
                break;
        }
    }

    private void EnableButton(bool hasNum)
    {
        if (injectButton != null)
        {
            injectButton.isEnabled = true;//先激活一下，确保false时会变色。
            injectButton.isEnabled = hasNum;
        }
    }
}
