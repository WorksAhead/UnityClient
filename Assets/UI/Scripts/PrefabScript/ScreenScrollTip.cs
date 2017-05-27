using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ScreenScrollTip : UnityEngine.MonoBehaviour
{
    public int ScrollViewOffsetRight = 44;
    private UnityEngine.Vector3 m_TweenPosFrom = new UnityEngine.Vector3();
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
    void Awake()
    {
        InitPanelSoftClip();
    }
    // Use this for initialization
    void Start()
    {
        try
        {
            if (eventlist != null) { eventlist.Clear(); }
            if (tipStrDic != null) { tipStrDic.Clear(); }
            object eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<string, int>("ge_notice", "notice", GetScreenScrollTip);
            if (eo != null) eventlist.Add(eo);
            if (TweenLabel != null)
            {
                NGUITools.SetActive(TweenLabel, true);
            }
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
    //根据

    //根据不同的屏幕大小，调整Label的位置
    void InitPanelSoftClip()
    {
        UIPanel panel = this.GetComponent<UIPanel>();
        if (panel != null)
        {
            Vector4 finalVec4 = panel.finalClipRegion;
            if (TweenLabel != null)
            {
                UnityEngine.Vector3 worldPos = UICamera.mainCamera.ScreenToWorldPoint(new UnityEngine.Vector3(Screen.width - ScrollViewOffsetRight, Screen.height, 0));
                TweenLabel.transform.position = worldPos;
                UnityEngine.Vector3 pos = TweenLabel.transform.localPosition;
                TweenLabel.transform.localPosition = new UnityEngine.Vector3(pos.x, finalVec4.y, pos.z);
                m_TweenPosFrom = TweenLabel.transform.localPosition;
            }
        }
    }
    void OnDestroy()
    {
        try
        {
            UnSubscribe();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    float GetOffsetWidth()
    {
        float width = 0;
        UIPanel up = gameObject.GetComponent<UIPanel>();
        if (up != null)
        {
            width -= up.width;
            if (TweenLabel != null)
            {
                UILabel ul = TweenLabel.GetComponent<UILabel>();
                if (ul != null)
                {
                    width -= ul.width;
                    return width;
                }
            }
        }
        return 0f;
    }
    void CheckAndDeleteStr()
    {
        if (TweenLabel != null)
        {
            UILabel ul = TweenLabel.GetComponent<UILabel>();
            if (ul != null && ul.text.Length > 0)
            {
                string strtip = "";
                List<string> strlist = new List<string>();
                foreach (string str in tipStrDic.Keys)
                {
                    if (str != null)
                    {
                        if (tipStrDic[str].count == 0)
                        {
                            strlist.Add(str);
                        }
                        else
                        {
                            tipStrDic[str].count -= 1;
                            strtip += str;
                        }
                    }
                }
                for (int i = 0; i < strlist.Count; ++i)
                {
                    string str2 = strlist[i];
                    if (str2 != null && tipStrDic.ContainsKey(str2))
                    {
                        tipStrDic.Remove(str2);
                    }
                }
                ul.text = strtip;
                strcount = strtip.Length;
            }
        }
    }
    void GetScreenScrollTip(string info, int num)
    {
        try
        {
            if (info == null || num == 0) { return; }
            if (tipStrDic.ContainsKey(info))
            {
                tipStrDic[info].count += num;
            }
            else
            {
                if (tipStrDic.Count == 0)
                {
                    tipStrDic.Add(info, new TipCount(--num));
                    if (TweenLabel != null)
                    {
                        UILabel ul = TweenLabel.GetComponent<UILabel>();
                        if (ul != null)
                        {
                            ul.text = info;
                            strcount = info.Length;
                        }
                    }
                }
                else
                {
                    tipStrDic.Add(info, new TipCount(num));
                }
                if (!NGUITools.GetActive(TweenLabel))
                {
                    NGUITools.SetActive(TweenLabel, true);
                }
                TweenPosition tp = TweenLabel.GetComponent<TweenPosition>();
                if (tp != null)
                {
                    tp.ResetToBeginning();
                    tp.from = m_TweenPosFrom;
                    tp.to = new UnityEngine.Vector3(GetOffsetWidth(), m_TweenPosFrom.y, tp.to.z);
                    tp.duration = DelayTime + strcount * 0.3f;
                    tp.enabled = true;
                }

            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void OnceEnd()
    {
        try
        {
            if (TweenLabel != null)
            {
                CheckAndDeleteStr();
                TweenPosition tp = TweenLabel.GetComponent<TweenPosition>();
                if (tp != null)
                {
                    tp.ResetToBeginning();
                    tp.from = m_TweenPosFrom;
                    tp.to = new UnityEngine.Vector3(GetOffsetWidth(), m_TweenPosFrom.y, tp.to.z);
                    tp.duration = DelayTime + strcount * 0.3f;
                    tp.enabled = true;
                }
            }
            if (tipStrDic.Count == 0)
            {
                NGUITools.SetActive(TweenLabel, false);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private float DelayTime = 5.0f;
    private int strcount = 0;
    public UnityEngine.GameObject TweenLabel = null;
    private Dictionary<string, TipCount> tipStrDic = new Dictionary<string, TipCount>();
}
public class TipCount
{
    public TipCount(int num)
    {
        count = num;
    }
    public int count = 0;
}