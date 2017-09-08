using System;
using System.Collections.Generic;
using ArkCrossEngine;

public class CombatFalse : UnityEngine.MonoBehaviour
{
    private List<object> m_EventList = new List<object>();
    public void UnSubscribe()
    {
        try
        {
            for (int i = 0; i < m_EventList.Count; i++)
            {
                if (m_EventList[i] != null)
                {
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(m_EventList[i]);
                }
            }
            /*
            foreach (object obj in m_EventList) {
              if (null != obj) {
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(obj);
              }
            }*/
            m_EventList.Clear();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Use this for initialization
    void Awake()
    {
        try
        {
            m_EventList.Clear();
            object obj = null;
            obj = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null) m_EventList.Add(obj);
            obj = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_role_dead", "ui", RoleDead);
            if (obj != null) m_EventList.Add(obj);
            obj = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_on_role_relive", "ui", OnRoleRelive);
            if (obj != null) m_EventList.Add(obj);
            obj = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_mpve_role_dead", "ui", MpveRoleDead);
            if (obj != null) m_EventList.Add(obj);

            UnityEngine.Transform tf = transform.Find("Time/Label");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    timelabel = ul;
                    ul.text = "16:00";
                }
            }
            time = 0.0f;
            //UIManager.Instance.HideWindowByName("CombatFalse");
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
            if (isRun == false)
            {
                return;
            }
            time += RealTime.deltaTime;

            int second = (int)(CD - time);
            if (timelabel != null)
            {
                string str1 = (second / 60).ToString();
                if (str1.Length == 1)
                {
                    str1 = "0" + str1;
                }
                string str2 = (second % 60).ToString();
                if (str2.Length == 1)
                {
                    str2 = "0" + str2;
                }
                timelabel.text = str1 + ":" + str2;
            }
            if (second <= 0.0f)
            {
                isRun = false;
                if (isInMpve)
                {//mpve
                    if (m_ReLiveButton != null)
                    {
                        m_ReLiveButton.isEnabled = true;
                    }
                }
                else
                {
                    //退回主城
                    GoBack();
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void MpveRoleDead()
    {
        isInMpve = true;
        if (m_ReLiveButton != null)
        {
            //m_ReLiveButton.isEnabled = true;
            m_ReLiveButton.isEnabled = false;
        }
        if (m_DescLabel != null)
        {
            m_DescLabel.text = StrDictionaryProvider.Instance.GetDictString(871);
        }
        CD = 11f;
        RoleDead();
    }

    private void OnRoleRelive()
    {
        try
        {
            UIManager.Instance.HideWindowByName("CombatFalse");
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, false);
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void RoleDead()
    {
        try
        {
            isRun = true;
            UIManager.Instance.ShowWindowByName("CombatFalse");
            ArkCrossEngine.RoleInfo roleInfo = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
            if (null != roleInfo)
            {
                int reliveStoneId = ArkCrossEngine.ItemConfigProvider.Instance.GetReliveStoneId();
                int reliveStoneCount = 0;
                ArkCrossEngine.ItemDataInfo item = roleInfo.GetItemData(reliveStoneId, 0);
                if (null != item)
                {
                    reliveStoneCount = item.ItemNum;
                }
                if (reliveStoneCount <= 0)
                {
                    // 钻石复活
                    SetDiamondActive(true);
                    SetReliveStoneActive(false);
                }
                else
                {
                    // 复活石复活
                    SetDiamondActive(false);
                    SetReliveStoneActive(true);
                }
                UnityEngine.Transform tf = transform.Find("FightBack/shiNum");
                if (null != tf)
                {
                    UILabel reliveStoneLabel = tf.gameObject.GetComponent<UILabel>();
                    if (null != reliveStoneLabel)
                    {
                        reliveStoneLabel.text = reliveStoneCount.ToString();
                    }
                }
                UILabel costStoneLabel = m_ReliveStoneNum.GetComponent<UILabel>();
                if (costStoneLabel != null)
                {
                    costStoneLabel.text = "1";
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void SetDiamondActive(bool active)
    {
        m_DiamondNum.SetActive(active);
        m_DiamondIcon.SetActive(active);
    }
    private void SetReliveStoneActive(bool active)
    {
        m_ReliveStoneIcon.SetActive(active);
        m_ReliveStoneNum.SetActive(active);
    }
    public void GoBack()
    {
        ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_request_relive", "lobby", false);
        UIManager.Instance.HideWindowByName("CombatFalse");
    }
    public void HeroRelive()
    {
        ArkCrossEngine.RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
        if (ri != null)
        {
            int reliveStoneId = ArkCrossEngine.ItemConfigProvider.Instance.GetReliveStoneId();
            int reliveStoneCount = 0;
            ArkCrossEngine.ItemDataInfo item = ri.GetItemData(reliveStoneId, 0);
            if (null != item)
            {
                reliveStoneCount = item.ItemNum;
            }
            if (ri.Gold < 50 && reliveStoneCount <= 0)
            {
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui",
                ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(149), ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(140), null, null, null, false);
            }
            else
            {
                UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
                if (go != null)
                {
                    PveFightInfo pfi = go.GetComponent<PveFightInfo>();
                    if (pfi != null)
                    {
                        pfi.AboutHeroDead();
                    }
                }
                /*
                    if (DFMUiRoot.PveFightInfo != null) {
                      PveFightInfo pfi = DFMUiRoot.PveFightInfo.GetComponent<PveFightInfo>();
                      if (pfi != null) {
                        pfi.AboutHeroDead();
                      }
                    }*/
                time = 0.0f;
                ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_request_relive", "lobby", true);
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
            }
        }
    }

    bool isInMpve = false;
    private bool isRun = false;
    private float CD = 16f;
    private float time = 0.0f;
    private UILabel timelabel = null;
    public UnityEngine.GameObject m_ReliveStoneIcon;
    public UnityEngine.GameObject m_ReliveStoneNum;
    public UnityEngine.GameObject m_DiamondIcon;
    public UnityEngine.GameObject m_DiamondNum;
    public UIButton m_ReLiveButton;
    public UILabel m_DescLabel;
}
