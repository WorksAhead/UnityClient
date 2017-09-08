using UnityEngine;
using ArkCrossEngine;
using System.Collections.Generic;
using System;

public class SkipStory : UnityEngine.MonoBehaviour
{

    private bool hasClick = false;

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
              foreach (object eo in m_EventList) {
                if (eo != null) {
                  ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eo);
                }
              }*/
            m_EventList.Clear();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Start()
    {
    }

    void Awake()
    {
        try
        {
            object obj = null;
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null)
                m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int>("ge_show_skip", "ui", ShowSkip);
            if (obj != null)
                m_EventList.Add(obj);

            //ShowSkip(0);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
    }

    public void OnSkipClick()
    {
        if (hasClick == false)
        {
            hasClick = true;
            LogicSystem.SendStoryMessage("SkipStory");
            ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_skip_story", "ui");
        }
    }

    //开关，1开0关
    public void ShowSkip(int visiable)
    {
        if (visiable == 1)
        {
            hasClick = false;
            NGUITools.SetActive(gameObject, true);
        }
        else
        {
            NGUITools.SetActive(gameObject, false);
        }
    }

}
