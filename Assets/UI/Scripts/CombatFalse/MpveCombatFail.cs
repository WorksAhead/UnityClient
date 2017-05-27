using System.Collections.Generic;
using System;
using ArkCrossEngine;

public class MpveCombatFail : UnityEngine.MonoBehaviour
{

    private List<object> m_EventList = new List<object>();
    private float CD = 16f;
    private float time = 0.0f;
    public UILabel labelDesc = null;
    public UILabel timeLabel = null;
    public UILabel tuxiLabel = null;
    private int typefail = 0; // 1:多人pve 2：突袭时间到
    private bool timeOut = false;
    public void UnSubscribe()
    {
        try
        {
            for (int i = 0; i < m_EventList.Count; i++)
            {
                if (null != m_EventList[i])
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
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Awake()
    {
        try
        {
            object obj = null;
            obj = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null)
                m_EventList.Add(obj);
            obj = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_mpve_misson_failed", "ui", MissionFailed);
            if (obj != null)
                m_EventList.Add(obj);
            obj = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_tuxi_timeout", "ui", TuxiFailed);
            if (obj != null)
                m_EventList.Add(obj);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Use this for initialization
    void Start()
    {
        try
        {
            //m_EventList.Clear();

            UIManager.Instance.HideWindowByName("MpveCombatFail");

            if (labelDesc != null)
            {
                labelDesc.text = StrDictionaryProvider.Instance.GetDictString(875);
                tuxiLabel.text = StrDictionaryProvider.Instance.GetDictString(876);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            time += RealTime.deltaTime;

            int second = (int)(CD - time);
            if (timeLabel != null)
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
                timeLabel.text = str1 + ":" + str2;
            }
            if (second <= 0.0f && timeOut)
            {
                OnClickMainCity();
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void TuxiFailed()
    {
        timeOut = true;
        typefail = 2;
        UIManager.Instance.ShowWindowByName("MpveCombatFail");
        if (tuxiLabel != null)
        {
            NGUITools.SetActive(tuxiLabel.gameObject, true);
        }
    }
    public void MissionFailed()
    {
        NGUITools.SetActive(tuxiLabel.gameObject, false);
        typefail = 1;
        timeOut = true;
        UIManager.Instance.ShowWindowByName("MpveCombatFail");
    }

    public void OnClickMainCity()
    {
        switch (typefail)
        {
            case 1:
                LogicSystem.PublishLogicEvent("ge_quit_battle", "lobby", false);
                break;
            case 2:
                LogicSystem.SendStoryMessage("missionfailed");
                break;
        }
        timeOut = false;
    }
}
