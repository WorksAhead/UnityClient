using UnityEngine;
using ArkCrossEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DynamicEquipment : UnityEngine.MonoBehaviour
{
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
                /*
	      foreach (object eo in eventlist) {
	        if (eo != null) {
	          ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eo);
	        }
	      }*/
                eventlist.Clear();
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Use this for initialization
    void Start()
    {
        try
        {
            if (eventlist != null) { eventlist.Clear(); }
            object eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, int, int, int, ArkCrossEngine.Network.GeneralOperationResult>("ge_fiton_equipment", "equipment", HeroPutOnEquipment);
            if (eo != null) { eventlist.Add(eo); }

            UIManager.Instance.HideWindowByName("DynamicEquipment");
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
    public void SetEquipment(ChangeNewEquip cne)
    {
        LevelLock info = LevelLockProvider.Instance.GetDataById(16);
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        if (info.m_Level > role.Level)
        {
            return;
        }
        if (cne != null)
        {
            id = cne.id;
            propertyid = cne.propertyid;
            ItemConfig ic = ItemConfigProvider.Instance.GetDataById(id);
            if (ic != null)
            {
                UnityEngine.Transform tf = transform.Find("bc/goods/Texture");
                if (tf != null)
                {
                    UITexture ut = tf.gameObject.GetComponent<UITexture>();
                    if (ut != null)
                    {
                        UnityEngine.Texture tt = GamePokeyManager.GetTextureByPicName(ic.m_ItemTrueName);
                        if (tt != null)
                        {
                            ut.mainTexture = tt;
                        }
                    }
                }
                tf = transform.Find("bc/goods");
                if (tf != null)
                {
                    UISprite us = tf.gameObject.GetComponent<UISprite>();
                    if (us != null)
                    {
                        us.spriteName = "EquipFrame" + ic.m_PropertyRank;
                    }
                }
            }
            UIManager.Instance.ShowWindowByName("DynamicEquipment");
        }
    }
    void DeleteNowCheckAnother()
    {
        ItemConfig ic = ItemConfigProvider.Instance.GetDataById(id);
        if (GamePokeyManager.changeitemDic != null && ic != null)
        {
            if (GamePokeyManager.changeitemDic.ContainsKey(ic.m_WearParts))
            {
                GamePokeyManager.changeitemDic.Remove(ic.m_WearParts);
            }
            foreach (int pos in GamePokeyManager.changeitemDic.Keys)
            {
                SetEquipment(GamePokeyManager.changeitemDic[pos]);
                break;
            }
        }
    }
    void HeroPutOnEquipment(int id, int pos, int itemLevel, int itemRandomProperty, ArkCrossEngine.Network.GeneralOperationResult result)
    {
        try
        {
            UIManager.Instance.HideWindowByName("DynamicEquipment");
            DeleteNowCheckAnother();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void WearButton()
    {
        ArkCrossEngine.ItemConfig itemconfig = ArkCrossEngine.LogicSystem.GetItemDataById(id);
        if (itemconfig != null)
        {
            ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_mount_equipment", "lobby", id, propertyid, itemconfig.m_WearParts);
        }
        UIManager.Instance.HideWindowByName("DynamicEquipment");
    }
    public void CloseButton()
    {
        UIManager.Instance.HideWindowByName("DynamicEquipment");
        DeleteNowCheckAnother();
    }
    public void ItemButton()
    {
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("GamePokey");
        if (go != null)
        {
            if (!NGUITools.GetActive(go))
            {
                ArkCrossEngine.ItemConfig itemconfig = ArkCrossEngine.LogicSystem.GetItemDataById(id);
                if (itemconfig != null)
                {
                    EquipmentInfo ei = GamePokeyManager.GetEquipmentInfo(itemconfig.m_WearParts);
                    if (ei != null)
                    {
                        go = UIManager.Instance.GetWindowGoByName("ItemProperty");
                        if (go != null && !NGUITools.GetActive(go))
                        {
                            ItemProperty ip = go.GetComponent<ItemProperty>();
                            if (ip != null)
                            {
                                ip.Compare(ei.id, ei.level, ei.propertyid, id, ei.level, propertyid, itemconfig.m_WearParts);
                                UIManager.Instance.ShowWindowByName("ItemProperty");
                            }
                        }
                    }
                }
            }
            else
            {
                UIManager.Instance.HideWindowByName("ItemProperty");
            }
        }
    }
    private int id = 0;
    private int propertyid = 0;
}
