using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class Option : UnityEngine.MonoBehaviour
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
            object eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_return_login", "ui", ReturnLogin);
            if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (eo != null) eventlist.Add(eo);

            UIManager.Instance.HideWindowByName("Option");
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

    public void SwitchID()
    {
        ArkCrossEngine.NoticeConfigLoader.s_NoticeContent = "";
        ArkCrossEngine.MyAction<int> fun = SwitchIDButtonWhich;
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(17), null,
        ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(140), ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(157), fun, false);
    }

    public void DebugPVP()
    {
        PlayerControl.Instance.JoinPlatformDefense(0, (int)Keyboard.Event.Up);
    }

    public void DebugPVE()
    {
        PlayerControl.Instance.MatchMpve(0, (int)Keyboard.Event.Up);
    }

    public void DebugLevelup()
    {
        PlayerControl.Instance.ToolPool(0, (int)Keyboard.Event.Up);
    }

    void SwitchIDButtonWhich(int which)
    {
        if (which == 1)
        {
            ArkCrossEngine.RoleInfo roleInfo = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
            if (roleInfo != null)
            {
                roleInfo.FightingScore = 0f;//目的是 避免切换角色时弹出战力值变化的特效
            }
            ArkCrossEngine.LogicSystem.QueueLogicAction(ArkCrossEngine.WorldSystem.Instance.ReturnToLogin);
        }
    }
    void ReturnLogin()
    {
        UnityEngine.GameObject go = UnityEngine.GameObject.Find(ArkCrossEngine.GlobalVariables.cGameRootName);
        if (go != null)
        {
            GameLogic gameLogic = go.GetComponent<GameLogic>();
            if (gameLogic != null) gameLogic.RestartLogic();
        }
    }
    public void OnCloseButtonClick()
    {
        UIManager.Instance.HideWindowByName("Option");
    }
}
