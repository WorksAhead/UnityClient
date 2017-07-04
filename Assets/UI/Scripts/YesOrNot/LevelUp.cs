using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelUp : UnityEngine.MonoBehaviour
{
    private List<object> eventlist = new List<object>();

    public float hideTime = 0f;
    //public UnityEngine.AudioClip audio;// 特效声音
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
    void Awake()
    {
        try
        {
            object eo;
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (eo != null)
                eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("ge_user_levelup", "property", UserLevelUP);
            if (eo != null)
                eventlist.Add(eo);
            ArkCrossEngine.RoleInfo role = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
            Invoke("HasLevelUp", 1f);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void HasLevelUp()
    {
        ArkCrossEngine.RoleInfo role = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
        if (role.LevelUp)
        {
            if (!UIManager.Instance.IsWindowVisible("TaskAward"))
            {
                UserLevelUP(role.Level);
            }
        }
    }
    private void UserLevelUP(int level)
    {
        ArkCrossEngine.RoleInfo role = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
        if (role.LevelUp)
        {
            role.LevelUp = false;
        }
        if (UIManager.Instance.IsWindowVisible("LevelUp"))
        {
            return;
        }
        if (UIManager.Instance.IsWindowVisible("GameTask"))
        {
            UIManager.Instance.HideWindowByName("GameTask");
        }
        UIManager.Instance.ShowWindowByName("LevelUp");
        NGUITools.PlaySound(GetComponent<AudioClip>(), 1f, 1f);
        ResetTweens();//必须先显示，才会执行
        Invoke("PublishNewThings", hideTime);
    }

    private void ResetTweens()
    {
        TweenAlpha[] tweenAlphas = gameObject.GetComponentsInChildren<TweenAlpha>();
        TweenPosition[] tweenPositions = gameObject.GetComponentsInChildren<TweenPosition>();
        TweenScale[] tweenScales = gameObject.GetComponentsInChildren<TweenScale>();
        TweenRotation[] tweenRotations = gameObject.GetComponentsInChildren<TweenRotation>();

        if (tweenAlphas != null)
        {
            foreach (TweenAlpha ta in tweenAlphas)
            {
                ta.ResetToBeginning();
                ta.PlayForward();
                ta.enabled = true;
            }
        }
        if (tweenPositions != null)
        {
            foreach (TweenPosition tp in tweenPositions)
            {
                tp.ResetToBeginning();
                tp.PlayForward();
                tp.enabled = true;
            }
        }
        if (tweenScales != null)
        {
            foreach (TweenScale ts in tweenScales)
            {
                ts.ResetToBeginning();
                ts.PlayForward();
                ts.enabled = true;
            }
        }
        if (tweenRotations != null)
        {
            foreach (TweenRotation tr in tweenRotations)
            {
                tr.ResetToBeginning();
                tr.PlayForward();
                tr.enabled = true;
            }
        }
    }

    void PublishNewThings()
    {
        UIManager.Instance.HideWindowByName("LevelUp");
        UIManager.Instance.HideWindowByName("FightScoreChange");
        ArkCrossEngine.RoleInfo role = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
        //ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("after_levelup", "ui_effect", role.Level);
    }
}
