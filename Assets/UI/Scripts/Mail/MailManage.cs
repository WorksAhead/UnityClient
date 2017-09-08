using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class MailManage : UnityEngine.MonoBehaviour
{
    private bool hasGetInitPosition = false;
    private UnityEngine.Vector3 scrollPosition;
    private UnityEngine.Vector2 scrollOffset;
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
            MailDic.Clear();
            MailStateDic.Clear();
            golist.Clear();
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
            if (MailDic != null) { MailDic.Clear(); }
            if (MailStateDic != null) { MailStateDic.Clear(); }
            if (golist != null) { golist.Clear(); }
            if (eventlist != null) { eventlist.Clear(); }
            object eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<List<ArkCrossEngine.MailInfo>>("ge_sync_mail_list", "mail", SyncMailList);
            if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_notify_new_mail", "mail", NewMail);
            if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (eo != null) eventlist.Add(eo);

            ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_get_mail_list", "lobby");
            UIManager.Instance.HideWindowByName("Mail");
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

    void NewMail()
    {
        try
        {
            ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_get_mail_list", "lobby");
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void CloseMail()
    {
        UIManager.Instance.HideWindowByName("Mail");
    }

    void SyncMailList(List<ArkCrossEngine.MailInfo> maillist)
    {
        try
        {
            if (maillist != null)
            {
                foreach (ArkCrossEngine.MailInfo mi in maillist)
                {
                    if (!HaveThisMail(mi.m_MailGuid))
                    {
                        AddMail(mi);
                    }
                }
            }
            UpdateScrollView();
            CheckHasUnReadMail();
            UnityEngine.Transform tf = gameObject.transform.Find("MetalFrame/Container/ScrollView/Grid");
            if (tf != null)
            {
                UIGrid ug = tf.gameObject.GetComponent<UIGrid>();
                if (ug != null)
                {
                    ug.repositionNow = true;
                }
                if (!hasGetInitPosition)
                {
                    hasGetInitPosition = true;
                    scrollPosition = tf.parent.localPosition;
                    UIPanel panel = tf.parent.GetComponent<UIPanel>();
                    if (panel != null)
                        scrollOffset = panel.clipOffset;
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    bool HaveThisMail(ulong mailguid)
    {
        foreach (ArkCrossEngine.MailInfo mi in MailDic.Values)
        {
            if (mailguid == mi.m_MailGuid)
            {
                return true;
            }
        }
        return false;
    }

    void AddMail(ArkCrossEngine.MailInfo mailinfo)
    {
        if (mailinfo != null)
        {
            UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/Mail/MailItem"));
            if (go != null)
            {
                UnityEngine.Transform tf = gameObject.transform.Find("MetalFrame/Container/ScrollView/Grid");
                if (tf != null)
                {
                    go = NGUITools.AddChild(tf.gameObject, go);
                    if (go != null)
                    {
                        UIEventListener.Get(go).onClick = MailItemClick;
                        MailDic.Add(go, mailinfo);
                        MailStateDic.Add(mailinfo.m_MailGuid, mailinfo.m_AlreadyRead);
                        SetMailItemInfo(go, mailinfo);
                    }
                }
            }
        }
    }

    void SetMailItemInfo(UnityEngine.GameObject go, ArkCrossEngine.MailInfo mailinfo)
    {
        if (go != null && mailinfo != null)
        {
            UnityEngine.Transform tf = go.transform.Find("Date");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = mailinfo.m_SendTime.ToString("MM/dd/HH/mm/ss");
                }
            }
            tf = go.transform.Find("Name");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = mailinfo.m_Title;
                }
            }
            tf = go.transform.Find("Sender");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = mailinfo.m_Sender;
                }
            }
            tf = go.transform.Find("MailImage");
            if (tf != null)
            {
                UISprite us = tf.gameObject.GetComponent<UISprite>();
                if (us != null)
                {
                    if (mailinfo.m_AlreadyRead)
                    {
                        us.spriteName = "mail2";
                    }
                    else
                    {
                        //             if (mailinfo.m_Gold == 0 && mailinfo.m_Money == 0 && (mailinfo.m_Items == null || mailinfo.m_Items.Count == 0)) {
                        //               us.spriteName = "xingfeng";
                        //             } else {
                        us.spriteName = "mail1";
                        //}
                    }
                }
            }
        }
    }

    void SetMailIntroduceInfo(ArkCrossEngine.MailInfo mi)
    {
        UnityEngine.Transform tfo = transform.Find("MetalFrame/RoleInfo/DragThing");
        if (tfo != null)
        {
            tfo.localPosition = new UnityEngine.Vector3(0.0f, 11.0f, 0.0f);
        }
        else
        {
            return;
        }
        UnityEngine.Transform tf = tfo.Find("Label");
        if (tf != null)
        {
            if (mi != null)
            {
                nowread = mi.m_MailGuid;
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    string str = "";
                    //str += (mi.m_Title + "\n");
                    str += (mi.m_SendTime.ToString("yyyy/MM/dd/HH/mm/ss") + "\n");
                    str += mi.m_Text;
                    ul.text = str;
                }
                bool sign = false;
                UnityEngine.Vector3 pos = tf.localPosition;
                pos = new UnityEngine.Vector3(pos.x, pos.y - ul.localSize.y - 15, 0.0f);
                if (mi.m_Money != 0)
                {
                    sign = true;
                    tf = tfo.Find("Money");
                    if (tf != null)
                    {
                        UnityEngine.GameObject go = tf.gameObject;
                        if (go != null)
                        {
                            UISprite us = go.GetComponent<UISprite>();

                            UnityEngine.Transform tf2 = go.transform.Find("Amount");
                            if (tf2 != null)
                            {
                                UILabel ul1 = tf2.gameObject.GetComponent<UILabel>();
                                if (ul1 != null)
                                {
                                    ul1.text = "X " + mi.m_Money;
                                }
                            }
                            go.transform.localPosition = pos;
                            NGUITools.SetActive(go, true);
                            if (us != null)
                            {
                                pos = new UnityEngine.Vector3(pos.x, pos.y - us.localSize.y - 15, 0.0f);
                            }
                        }
                    }
                }
                if (mi.m_Gold != 0)
                {
                    sign = true;
                    tf = tfo.Find("Diamond");
                    if (tf != null)
                    {
                        UnityEngine.GameObject go = tf.gameObject;
                        if (go != null)
                        {
                            UISprite us = go.GetComponent<UISprite>();

                            UnityEngine.Transform tf2 = go.transform.Find("Amount");
                            if (tf2 != null)
                            {
                                UILabel ul1 = tf2.gameObject.GetComponent<UILabel>();
                                if (ul1 != null)
                                {
                                    ul1.text = "X " + mi.m_Gold;
                                }
                            }
                            go.transform.localPosition = pos;
                            NGUITools.SetActive(go, true);
                            if (us != null)
                            {
                                pos = new UnityEngine.Vector3(pos.x, pos.y - us.localSize.y - 15, 0.0f);
                            }
                        }
                    }
                }
                //         if (mi.m_Gold != 0) {
                //           sign = true;
                //           tf = tfo.Find("Exp");
                //           if (tf != null) {
                //             UnityEngine.GameObject go = tf.gameObject;
                //             if (go != null) {
                //               UISprite us = go.GetComponent<UISprite>();
                // 
                //               UnityEngine.Transform tf2 = go.transform.Find("Amount");
                //               if (tf2 != null) {
                //                 UILabel ul1 = tf2.gameObject.GetComponent<UILabel>();
                //                 if (ul1 != null) {
                //                   ul1.text = "X " + mi.m_Gold;
                //                 }
                //               }
                //               go.transform.localPosition = pos;
                //               NGUITools.SetActive(go, true);
                //               if (us != null) {
                //                 pos = new UnityEngine.Vector3(pos.x, pos.y - us.localSize.y - 15, 0.0f);
                //               }
                //             }
                //           }
                //         }
                if (mi.m_Items != null)
                {
                    foreach (ArkCrossEngine.MailItem mailitem in mi.m_Items)
                    {
                        if (mailitem != null)
                        {
                            sign = true;
                            ArkCrossEngine.ItemConfig ic = ArkCrossEngine.LogicSystem.GetItemDataById(mailitem.m_ItemId);
                            if (ic != null)
                            {
                                UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/Mail/MailAward"));
                                if (go != null)
                                {
                                    UITexture ut = go.GetComponent<UITexture>();
                                    if (ut != null)
                                    {
                                        UnityEngine.Texture tt = GamePokeyManager.GetTextureByPicName(ic.m_ItemTrueName);
                                        if (tt != null)
                                        {
                                            ut.mainTexture = tt;
                                        }
                                    }
                                    UnityEngine.Transform tf2 = go.transform.Find("Amount");
                                    if (tf2 != null)
                                    {
                                        UILabel ul1 = tf2.gameObject.GetComponent<UILabel>();
                                        if (ul1 != null)
                                        {
                                            ul1.text = "X " + mailitem.m_ItemNum;
                                        }
                                    }
                                    go = NGUITools.AddChild(tfo.gameObject, go);
                                    if (go != null)
                                    {
                                        go.transform.localPosition = pos;
                                        golist.Add(go);
                                    }
                                    pos = new UnityEngine.Vector3(pos.x, pos.y - ut.localSize.y - 15, 0.0f);
                                }
                            }
                        }
                    }
                }
                if (sign)
                {
                    tf = transform.Find("MetalFrame/RoleInfo/DragThing/ReceiveButton");
                    if (tf != null)
                    {
                        tf.localPosition = new UnityEngine.Vector3(0.0f, pos.y, 0.0f);
                        NGUITools.SetActive(tf.gameObject, true);
                    }
                }
            }
        }
        tf = transform.Find("sp_hongdi1/Label");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                ul.text = mi.m_Title;
            }
        }
        //     tf = transform.Find("sp_hongdi2/Label");
        //     if (tf != null) {
        //       UILabel ul = tf.gameObject.GetComponent<UILabel>();
        //       if (ul != null) {
        //         ul.text = mi.m_Title;
        //       }
        //     }
    }

    void DeleteMail(ulong mailid)
    {
        foreach (UnityEngine.GameObject go in MailDic.Keys)
        {
            ArkCrossEngine.MailInfo minfo = MailDic[go];
            if (minfo != null && mailid == minfo.m_MailGuid)
            {
                MailDic.Remove(go);
                NGUITools.DestroyImmediate(go);

                MailStateDic.Remove(mailid);
                break;
            }
        }
        UpdateScrollView();
        UnityEngine.Transform tf = transform.Find("MetalFrame/Container/ScrollView/Grid");
        if (tf != null)
        {
            UIGrid ug = tf.gameObject.GetComponent<UIGrid>();
            if (ug != null)
            {
                ug.repositionNow = true;
            }
        }
        lastclickgo = null;
    }

    private void UpdateScrollView()
    {
        UnityEngine.Transform tf = transform.Find("MetalFrame/Container/ScrollView");
        if (tf != null)
        {
            UIScrollView scroll = tf.GetComponent<UIScrollView>();
            if (scroll != null)
            {
                if (MailDic.Count < 6)
                {
                    scroll.enabled = false;
                }
                else
                {
                    scroll.enabled = true;
                }
            }
        }
    }

    void MailItemClick(UnityEngine.GameObject go)
    {
        if (go != null && MailDic.ContainsKey(go))
        {
            ClearIntroduce();
            ArkCrossEngine.MailInfo minfo = MailDic[go];
            SetMailIntroduceInfo(minfo);

            if (MailStateDic[minfo.m_MailGuid] == false)
            {
                ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_read_mail", "lobby", minfo.m_MailGuid);
                MailStateDic[minfo.m_MailGuid] = true;
                CheckHasUnReadMail();
            }

            UnityEngine.Transform tf = go.transform.Find("MailImage");
            if (tf != null)
            {
                UISprite us = tf.gameObject.GetComponent<UISprite>();
                if (us != null)
                {
                    us.spriteName = "mail2";
                }
            }
            tf = go.transform.Find("Frame");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, true);
            }
            if (lastclickgo != null)
            {
                tf = lastclickgo.transform.Find("Frame");
                if (tf != null)
                {
                    NGUITools.SetActive(tf.gameObject, false);
                }
            }
            lastclickgo = go;
        }
    }

    public void ReceiveButton()
    {
        ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_receive_mail", "lobby", nowread);
        DeleteMail(nowread);
        ClearIntroduce();
    }

    void ClearIntroduce()
    {
        UnityEngine.Transform tf = transform.Find("MetalFrame/RoleInfo/DragThing/Label");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                ul.text = "";
            }
        }
        tf = transform.Find("sp_hongdi1/Label");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                ul.text = "";
            }
        }
        tf = transform.Find("sp_hongdi2/Label");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                ul.text = "";
            }
        }
        foreach (UnityEngine.GameObject go in golist)
        {
            if (go != null)
            {
                NGUITools.DestroyImmediate(go);
            }
        }
        golist.Clear();
        tf = transform.Find("MetalFrame/RoleInfo/DragThing/ReceiveButton");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, false);
        }
        tf = transform.Find("MetalFrame/RoleInfo/DragThing/Money");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, false);
        }
        tf = transform.Find("MetalFrame/RoleInfo/DragThing/Diamond");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, false);
        }
        tf = transform.Find("MetalFrame/RoleInfo/DragThing/Exp");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, false);
        }
    }

    private void CheckHasUnReadMail()
    {
        bool has = false;
        foreach (ArkCrossEngine.MailInfo mi in MailDic.Values)
        {
            if (mi != null && MailStateDic[mi.m_MailGuid] == false)
            {
                has = true;
                break;
            }
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.Mail, has);
    }

    private Dictionary<UnityEngine.GameObject, ArkCrossEngine.MailInfo> MailDic = new Dictionary<UnityEngine.GameObject, ArkCrossEngine.MailInfo>();
    private Dictionary<ulong, bool> MailStateDic = new Dictionary<ulong, bool>();
    private List<UnityEngine.GameObject> golist = new List<UnityEngine.GameObject>();
    private ulong nowread = 0;
    private UnityEngine.GameObject lastclickgo = null;
}
