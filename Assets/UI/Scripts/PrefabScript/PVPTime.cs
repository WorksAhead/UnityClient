using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PVPTime : UnityEngine.MonoBehaviour
{
    private List<object> eventlist = new List<object>();
    public void UnSubscribe()
    {
        try
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
            NGUITools.DestroyImmediate(gameObject);
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
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<long>("ge_send_scenestart_time", "ui", PvpSceneStartTime);
            if (eo != null) eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, int, int, int, int, int, int, int, int, int, string>("ge_pvp_result", "ui", PvpResult);
            if (eo != null) eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<ArkCrossEngine.ChallengeInfo>("ge_partnerpvp_result", "ui", OnPvapResult);
            if (eo != null) eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_stop_countdown", "ui", Stop);
            if (eo != null) eventlist.Add(eo);

            UnityEngine.Transform tf = transform.Find("Label");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    timelabel = ul;
                }
            }
            ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_get_scenestart_time", "lobby");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (timelabel != null)
            {
                long residuetime = (long)countDownTime - (ArkCrossEngine.TimeUtility.GetServerMilliseconds() - StartTime) / 1000;
                string str1 = (residuetime / 60).ToString();
                if (str1.Length == 1)
                {
                    str1 = "0" + str1;
                }
                string str2 = (residuetime % 60).ToString();
                if (str2.Length == 1)
                {
                    str2 = "0" + str2;
                }
                timelabel.text = str1 + ":" + str2;
                if (residuetime <= 0)
                {
                    str1 = "00";
                    str2 = "00";
                    enabled = false;
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void PvpSceneStartTime(long starttime)
    {
        try
        {
            if (!ArkCrossEngine.WorldSystem.Instance.IsPvapScene())
            {
                StartTime = starttime;
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void SetCountDownTime(int time)
    {
        StartTime = ArkCrossEngine.TimeUtility.GetServerMilliseconds();
        countDownTime = time;
    }
    public void Stop()
    {
        try
        {
            PVPTime pt = gameObject.GetComponent<PVPTime>();
            if (pt != null)
            {
                pt.enabled = false;
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void PvpResult(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, string k)
    {
        Stop();
    }
    void OnPvapResult(ArkCrossEngine.ChallengeInfo info)
    {
        Stop();
    }

    private long StartTime = 0;
    private int countDownTime = 0;
    private UILabel timelabel = null;
    private bool IsStoped = false;
}
