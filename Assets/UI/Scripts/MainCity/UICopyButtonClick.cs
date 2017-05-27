using System;
using System.Collections.Generic;
using ArkCrossEngine;

public class UICopyButtonClick : UnityEngine.MonoBehaviour
{

    public UnityEngine.GameObject goAwardHint = null;
    private List<object> eventlist = new List<object>();
    private bool m_IsCopy = true;
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
    // Use this for initialization
    void Start()
    {
        object eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, ArkCrossEngine.MissionOperationType, string>("ge_about_task", "task", OnTaskFinished);
        if (eo != null) { eventlist.Add(eo); }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_award_finished", "ui", OnAwardFinished);
        if (eo != null) eventlist.Add(eo);
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (eo != null) eventlist.Add(eo);
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("check_player_info", "ui", OnCheckPlayerInfo);
        if (null != eo) { eventlist.Add(eo); }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<string, int, int>("ge_sync_player_info", "info", BackPlayerInfo);
        if (null != eo)
        {
            eventlist.Add(eo);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /**点击玩家 弹出加好友，组队功能界面*/
    void OnCheckPlayerInfo(int actorid)
    {
        try
        {
            SharedGameObjectInfo share_info = ArkCrossEngine.LogicSystem.GetSharedGameObjectInfo(actorid);
            GfxUserInfo userInfo;
            bool sign = true;
            for (int i = 0; i < DFMUiRoot.GfxUserInfoListForUI.Count; i++)
            {
                if (DFMUiRoot.GfxUserInfoListForUI[i].m_ActorId == actorid)
                {
                    sign = false;
                    userInfo = DFMUiRoot.GfxUserInfoListForUI[i];
                    UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("DynamicFriend");
                    if (null != go)
                    {
                        go.GetComponent<DynamicFriend>().InitPanel(userInfo, new UnityEngine.Vector3());
                    }
                    LogicSystem.PublishLogicEvent("ge_request_player_info", "lobby", userInfo.m_Nick);
                    break;
                }
            }
            if (sign)
            {
                GfxSystem.PublishGfxEvent("ge_screen_tip", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(557), UIScreenTipPosEnum.AlignCenter, UnityEngine.Vector3.zero);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }

    }
    void BackPlayerInfo(string name, int level, int score)
    {
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("DynamicFriend");
        if (null != go)
        {
            go.GetComponent<DynamicFriend>().SetScoreInfo(name, level, score);
        }
        UIManager.Instance.ShowWindowByName("DynamicFriend");
    }
    void OnClick()
    {
        if (m_IsCopy)
        {
            //UIManager.Instance.ToggleWindowVisible("SceneSelect");
            LogicSystem.SendStoryMessage("cityplayermove", 0);//寻路
            UnityEngine.GameObject goc = UIManager.Instance.GetWindowGoByName("SceneSelect");
            if (goc != null)
            {
                UISceneSelect uss = goc.GetComponent<UISceneSelect>();
                if (uss != null)
                {
                    uss.startChapterId = 0;//无指引
                }
            }
        }
        else
        {
            UIManager.Instance.ToggleWindowVisible("GameTask");
        }
    }
    private void OnTaskFinished(int taskId, ArkCrossEngine.MissionOperationType opType, string schedule)
    {
        try
        {
            if (opType == ArkCrossEngine.MissionOperationType.FINISH)
            {
                MissionConfig missionconfig = ArkCrossEngine.LogicSystem.GetMissionDataById(taskId);
                if (missionconfig == null) return;
                if (missionconfig.MissionType == 1)
                {
                    //主线任务完成
                    if (goAwardHint != null) NGUITools.SetActive(goAwardHint, true);
                    m_IsCopy = false;
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void OnAwardFinished()
    {
        try
        {
            if (goAwardHint != null)
            {
                NGUITools.SetActive(goAwardHint, false);
                m_IsCopy = true;
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}
