using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class TiliBuy : UnityEngine.MonoBehaviour
{
    public UILabel lblReStamina = null;//体力回复
    public int m_ReStaminaPerMin = 5;
    private float m_RestaminaStartTime = -1f;
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
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
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
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<ArkCrossEngine.Network.GeneralOperationResult>("ge_buy_stamina", "stamina", Buyresult);
            if (eo != null) eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<ArkCrossEngine.Network.GeneralOperationResult>("ge_startgame_failure", "game", StartGameFailure);
            if (eo != null) eventlist.Add(eo);
            SetBuyStaminaInfo();
            UIManager.Instance.HideWindowByName("TiliBuy");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            CalculateRestaminaTime();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void SetBuyStaminaInfo()
    {
        ArkCrossEngine.RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
        if (ri != null)
        {
            UnityEngine.Transform tf = transform.Find("tip");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ArkCrossEngine.VipConfig config_data = ArkCrossEngine.VipConfigProvider.Instance.GetDataById(ri.Vip);
                    ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.Format(146, ri.BuyStaminaCount, null == config_data ? ri.Vip + 1 : config_data.m_Stamina);
                }
            }
            ArkCrossEngine.BuyStaminaConfig bsc = ArkCrossEngine.BuyStaminaConfigProvider.Instance.GetDataById(ri.BuyStaminaCount + 1);
            if (bsc != null)
            {
                tf = transform.Find("bk/zuan/mount");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        ul.text = bsc.m_CostGold.ToString();
                    }
                }
                tf = transform.Find("bk/money/mount");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        ul.text = bsc.m_GainStamina.ToString();
                    }
                }
            }
        }
    }

    private void StartGameFailure(ArkCrossEngine.Network.GeneralOperationResult result)
    {
        try
        {
            if (result == ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_CostError)
            {
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(306),
                ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(140), null, null, null, false);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void Buyresult(ArkCrossEngine.Network.GeneralOperationResult result)
    {
        try
        {
            if (result == ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed)
            {
                SetBuyStaminaInfo();
                RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
                if (ri != null)
                {
                    ArkCrossEngine.BuyStaminaConfig bsc = ArkCrossEngine.BuyStaminaConfigProvider.Instance.GetDataById(ri.BuyStaminaCount);
                    if (bsc != null)
                    {
                        GfxSystem.PublishGfxEvent("ge_screen_tip", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.Format(172, bsc.m_GainStamina), UIScreenTipPosEnum.AlignCenter, UnityEngine.Vector3.zero);
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
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(i),
                ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(140), null, null, null, false);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void ButtonForYes()
    {
        ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_buy_stamina", "lobby");
    }

    public void CloseWindow()
    {
        UIManager.Instance.HideWindowByName("TiliBuy");
    }
    //计算体力回满所需要的时间
    private void CalculateRestaminaTime()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null && lblReStamina != null)
        {
            if (role_info.CurStamina >= role_info.StaminaMax)
            {
                string str_des = StrDictionaryProvider.Instance.GetDictString(19);
                lblReStamina.text = str_des;
                return;
            }
            if (DFMUiRoot.m_RestaminaStartTime != float.MinValue)
            {
                m_RestaminaStartTime = DFMUiRoot.m_RestaminaStartTime;
                //等于-1表示服务器还没向客户端同步体力回复的开始时间
                long passedTime = (long)(UnityEngine.Time.time - m_RestaminaStartTime);
                passedTime = passedTime % (m_ReStaminaPerMin * 60);//计算秒数
                int leftTime = m_ReStaminaPerMin * 60 - (int)passedTime;
                int min0 = leftTime / 60;
                int second = leftTime % 60;
                int total_min = (role_info.StaminaMax - role_info.CurStamina + 1) * m_ReStaminaPerMin;
                total_min += min0;
                if (total_min < 0) total_min = 0;
                int hours = total_min / 60;
                int min1 = total_min % 60;
                string str_des_0 = StrDictionaryProvider.Instance.GetDictString(18);
                lblReStamina.text = string.Format(str_des_0, m_ReStaminaPerMin, min0, second, hours, min1);
            }
        }
    }
}
