using System;
using System.Collections.Generic;
using ArkCrossEngine;

public class TouchCircle : UnityEngine.MonoBehaviour
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
            NGUITools.Destroy(gameObject);
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
            OKAlpha = false;
            time = 0.0f;
            isUp = true;
            isSkillWant = false;
            if (eventlist != null) { eventlist.Clear(); }
            object eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<UnityEngine.Vector2, SkillCategory, bool>("ge_ui_angle", "ui",
              (UnityEngine.Vector2 v, SkillCategory s, bool b) => { if (b) { SkillIsFalse(); } });
            if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<UnityEngine.Vector2, bool>("ge_finger_event", "ui", FinentEvent);
            if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_skill_false", "ui", SkillIsFalse);
            if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<bool>("ge_touch_dir", "ui", (bool want) => { isSkillWant = want; });
            if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (eo != null) eventlist.Add(eo);
            UIManager.SkillDrectorColor = new UnityEngine.Color(255, 255, 255);
            //NGUITools.SetActive(gameObject.transform.parent.gameObject, false);
            UIManager.Instance.HideWindowByName("TouchCircle");
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
            if (OKAlpha)
            {
                time += RealTime.deltaTime;
                int multiple = (int)System.Math.Round(time / 0.03f);
                switch (multiple)
                {
                    case 0: SetAlpha(1.0f); break;
                    case 1: SetAlpha(0.9f); break;
                    case 2: SetAlpha(0.8f); break;
                    case 3: SetAlpha(0.7f); break;
                    case 4: SetAlpha(0.6f); break;
                    case 5: SetAlpha(0.5f); break;
                    case 6: SetAlpha(0.4f); break;
                    case 7: SetAlpha(0.3f); break;
                    case 8: SetAlpha(0.2f); break;
                    case 9: SetAlpha(0.1f); break;
                    case 10: SetAlpha(0.0f); break;
                    default:
                        //NGUITools.SetActive(gameObject.transform.parent.gameObject, false);
                        UIManager.Instance.HideWindowByName("TouchCircle");
                        break;
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void FinentEvent(UnityEngine.Vector2 start, bool active)
    {
        try
        {
            if (active)
            {
                if (isUp || isSkillWant)
                {
                    isUp = false;
                    OKAlpha = false;
                    time = 0.0f;
                    UISprite us = this.gameObject.GetComponent<UISprite>();
                    if (us != null)
                    {
                        UIManager.SkillDrectorColor = new UnityEngine.Color(255, 255, 255);
                        us.color = new UnityEngine.Color(255, 255, 255);
                    }
                    UnityEngine.Vector3 pos = UICamera.mainCamera.ScreenToWorldPoint(new UnityEngine.Vector3(start.x, start.y, 0));
                    this.transform.position = pos;
                    //NGUITools.SetActive(gameObject.transform.parent.gameObject, true);
                    UIManager.Instance.ShowWindowByName("TouchCircle");
                }
            }
            else
            {
                isUp = true;
                OKAlpha = true;
                time = 0.0f;
                //NGUITools.SetActive(gameObject, false);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void SkillIsFalse()
    {
        UISprite us = this.gameObject.GetComponent<UISprite>();
        if (us != null)
        {
            UIManager.SkillDrectorColor = new UnityEngine.Color(255, 0, 0);
            us.color = new UnityEngine.Color(255, 0, 0);
        }
    }
    void SetAlpha(float wantalpha)
    {
        UISprite us = this.gameObject.GetComponent<UISprite>();
        if (us != null)
        {
            us.alpha = wantalpha;
        }
    }
    private bool OKAlpha = false;
    private float time = 0.0f;

    private bool isUp = true;
    private bool isSkillWant = false;
}
