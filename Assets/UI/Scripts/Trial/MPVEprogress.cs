using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ArkCrossEngine;

public class MPVEprogress : UnityEngine.MonoBehaviour
{
    private List<object> m_EventList = new List<object>();

    public UILabel labelCurrent = null;
    public UILabel labelBest = null;

    [HideInInspector]
    public int best;
    [HideInInspector]
    public int current;
    [HideInInspector]
    public int max;

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
              foreach (object eo in m_EventList) {
                if (eo != null) {
                  ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eo);
                }
              }*/
            m_EventList.Clear();
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
            object obj = null;
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null)
                m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int, int>("ge_mpve_progress", "ui", UpdateProgress);
            if (obj != null)
                m_EventList.Add(obj);
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

    public void UpdateProgress(int num, int max)
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        int currentMaxAwardId = role.AttemptAward;//当前最好的奖品ＩＤ
        best = currentMaxAwardId;
        current = num;
        this.max = max;
        if (labelCurrent != null && labelBest != null)
        {
            labelBest.text = StrDictionaryProvider.Instance.Format(869, currentMaxAwardId + "/" + max);
            labelCurrent.text = StrDictionaryProvider.Instance.Format(870, num + "/" + max);
        }
    }
}
