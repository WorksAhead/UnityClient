using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class GoldBuy : UnityEngine.MonoBehaviour
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
            }
            eventlist.Clear();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Use this for initialization
    void Start()
    {
        try
        {
            if (eventlist != null) { eventlist.Clear(); }
            object eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (eo != null) eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<ArkCrossEngine.Network.GeneralOperationResult>("ge_midas_touch", "midastouch", Buyresult);
            if (eo != null) eventlist.Add(eo);

            SetGoldBuyInfo();
            UIManager.Instance.HideWindowByName("GoldBuy");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void SetGoldBuyInfo()
    {
        ArkCrossEngine.RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
        if (ri != null)
        {
            UnityEngine.Transform tf = transform.Find("bk/tip");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ArkCrossEngine.VipConfig config_data = ArkCrossEngine.VipConfigProvider.Instance.GetDataById(ri.Vip);
                    ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.Format(145, ri.BuyMoneyCount, null == config_data ? (ri.Vip > 0 ? ri.Vip * 10 : 10) : config_data.m_BuyGold);
                }
            }
            ArkCrossEngine.BuyMoneyConfig bmc = ArkCrossEngine.BuyMoneyConfigProvider.Instance.GetDataById(ri.BuyMoneyCount + 1);
            if (bmc != null)
            {
                tf = transform.Find("bk/money/mount");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        ul.text = bmc.m_GainMoney.ToString();
                    }
                }
                tf = transform.Find("bk/zuan/mount");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        ul.text = bmc.m_CostGold.ToString();
                    }
                }
            }
        }
    }
    public void BuyOne()
    {
        signtenbuy = false;
        ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_midas_touch", "lobby");
    }
    public void BuyTen()
    {
        signshowtip = false;
        StartCoroutine(BuyTenDelay());
    }
    public IEnumerator BuyTenDelay()
    {
        int i = 0;
        while (i < 10)
        {
            BuyOne();
            signtenbuy = true;
            ++i;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void CloseWindow()
    {
        UIManager.Instance.HideWindowByName("GoldBuy");
    }
    void Buyresult(ArkCrossEngine.Network.GeneralOperationResult result)
    {
        try
        {
            if (result == ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed)
            {
                SetGoldBuyInfo();
                ArkCrossEngine.RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
                if (ri != null)
                {
                    ArkCrossEngine.BuyMoneyConfig bmc = ArkCrossEngine.BuyMoneyConfigProvider.Instance.GetDataById(ri.BuyMoneyCount);
                    if (bmc != null)
                    {
                        BuyMoneyTip(bmc.m_GainMoney);
                    }
                }
            }
            else
            {
                int i = 0;
                if (result == ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_CostError)
                {
                    i = 123;
                }
                if (result == ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_Overflow)
                {
                    i = 150;
                }
                if (signtenbuy)
                {
                    if (signshowtip)
                    {
                        return;
                    }
                    else
                    {
                        signshowtip = true;
                    }
                }
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(i),
                ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(140), null, null, null, false);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void BuyMoneyTip(int num)
    {
        string path = UIManager.Instance.GetPathByName("GoldBuyDlg");
        UnityEngine.Object obj = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.NewObject(path, 5f));
        UnityEngine.GameObject go = obj as UnityEngine.GameObject;
        if (null != go)
        {
            UnityEngine.Transform tf = go.transform.Find("Label/value");
            if (tf != null)
            {
                UILabel bloodPanel = tf.gameObject.GetComponent<UILabel>();
                if (null != bloodPanel)
                {
                    bloodPanel.text = num.ToString();
                }
            }
            UnityEngine.GameObject cube = null;
            tf = transform.parent.Find("ScreenTipPanel");
            if (tf != null)
            {
                cube = NGUITools.AddChild(tf.gameObject, obj);
            }
            if (cube != null)
            {
                BloodAnimation ba = cube.GetComponent<BloodAnimation>();
                if (ba != null)
                {
                    ba.PlayAnimation();
                }
                NGUITools.SetActive(cube, true);
            }
        }
    }
    private bool signtenbuy = false;
    private bool signshowtip = false;
}
