using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class GameTask : UnityEngine.MonoBehaviour
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
            }
            eventlist.Clear();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Use this for initialization
    void Start()
    {
        try
        {
            if (taskDic != null) { taskDic.Clear(); }
            if (finishtask != null) { finishtask.Clear(); }
            //if (awardtask != null) { awardtask.Clear(); }

            if (eventlist != null) { eventlist.Clear(); }
            object eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, ArkCrossEngine.MissionOperationType, string>("ge_about_task", "task", GetTaskIdAndOperator);
            if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (eo != null) eventlist.Add(eo);


            ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_reload_missions", "lobby");
            UIManager.Instance.HideWindowByName("GameTask");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GetTaskIdAndOperator(int id, ArkCrossEngine.MissionOperationType oper, string schedule)
    {
        try
        {
            ArkCrossEngine.MissionConfig missionconfig = ArkCrossEngine.LogicSystem.GetMissionDataById(id);
            ArkCrossEngine.RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
            if (missionconfig != null && ri != null && ri.Level < missionconfig.LevelLimit)
            {
                return;
            }
            switch (oper)
            {
                case ArkCrossEngine.MissionOperationType.ADD:
                    AddTask(id, schedule);
                    break;
                case ArkCrossEngine.MissionOperationType.FINISH:
                    if (!finishtask.Contains(id))
                    {
                        if (!taskDic.ContainsKey(id))
                        {
                            AddTask(id, schedule);
                        }
                        else
                        {
                            SetTaskInfo(taskDic[id], id, schedule);
                        }

                        UnityEngine.Transform tf = taskDic[id].transform.Find("New");
                        if (tf != null)
                        {
                            UISprite us = tf.gameObject.GetComponent<UISprite>();
                            if (us != null)
                            {
                                us.spriteName = "lingj";
                            }
                        }
                        tf = taskDic[id].transform.Find("Schedule");
                        if (tf != null)
                        {
                            NGUITools.SetActive(tf.gameObject, false);
                        }
                        finishtask.Add(id);
                    }
                    if (missionconfig.MissionType == 1)
                    {
                        UIManager.Instance.ShowWindowByName("TaskAward");
                        UnityEngine.GameObject god = UIManager.Instance.GetWindowGoByName("TaskAward");
                        if (god != null)
                        {
                            TaskAward ta = god.GetComponent<TaskAward>();
                            if (ta != null)
                            {
                                ta.SetAwardProperty(id);
                                ta.InitTaskId(id, TaskCompleteType.T_common);
                            }
                        }
                    }
                    break;
                case ArkCrossEngine.MissionOperationType.DELETE:
                    DeleteTask(id);
                    if (missionconfig.MissionType == 1)
                    {
                        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("TaskAward");
                        if (!NGUITools.GetActive(go))
                        {
                            if (go != null)
                            {
                                TaskAward ta = go.GetComponent<TaskAward>();
                                if (ta != null && ta.TaskId != id)
                                {
                                    ta.SetAwardProperty(id);
                                    UIManager.Instance.HideWindowByName("GameTask");
                                    UIManager.Instance.ShowWindowByName("TaskAward");
                                }
                            }
                        }
                        else
                        {
                            TaskAward ta = go.GetComponent<TaskAward>();
                            if (ta.TaskId != id)
                                awardtask.Add(id);
                        }
                    }
                    break;
                case ArkCrossEngine.MissionOperationType.UPDATA:
                    break;
                default:
                    break;
            }
            CheckHasFinish();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void CheckHasFinish()
    {
        bool has = false;
        if (finishtask.Count > 0)
        {
            has = true;
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.Task, has);
    }

    private void AddTask(int taskid, string schedule)
    {
        if (!taskDic.ContainsKey(taskid))
        {
            UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/GameTask/TaskItem"));
            if (go != null)
            {
                UnityEngine.Transform tf = gameObject.transform.Find("Container/Scroll View/Grid");
                if (tf != null)
                {
                    go = NGUITools.AddChild(tf.gameObject, go);
                    if (go != null)
                    {
                        UIEventListener.Get(go).onClick = TaskItemClick;
                        NGUITools.SetActive(go, true);
                        taskDic.Add(taskid, go);
                        SetTaskInfo(go, taskid, schedule, true);
                    }
                }
                if (tf != null)
                {
                    UIGrid ug = tf.gameObject.GetComponent<UIGrid>();
                    if (ug != null)
                    {
                        ug.repositionNow = true;
                    }
                    if (tf.parent != null)
                    {
                        tf.parent.localPosition = new UnityEngine.Vector3(0, 0, 0);
                    }
                }
            }
        }
        else
        {
            SetTaskInfo(taskDic[taskid], taskid, schedule);
        }
    }

    private void DeleteTask(int id)
    {
        if (finishtask.Contains(id))
        {
            finishtask.Remove(id);
            GfxSystem.PublishGfxEvent("ge_ui_connect_hint", "ui", false, false);
            //UIManager.Instance.ShowWindowByName("TaskAward");
            //UnityEngine.GameObject god = UIManager.Instance.GetWindowGoByName("TaskAward");
            //if (god != null) {
            //  TaskAward ta = god.GetComponent<TaskAward>();
            //  if (ta != null) {
            //    ta.SetAwardProperty(id);
            //  }
            //}
        }
        if (taskDic.ContainsKey(id))
        {
            if (taskDic[id] != null)
            {
                NGUITools.DestroyImmediate(taskDic[id]);
            }
            taskDic.Remove(id);
        }
        UnityEngine.Transform tf = gameObject.transform.Find("Container/Scroll View/Grid");
        if (tf != null)
        {
            UIGrid ug = tf.gameObject.GetComponent<UIGrid>();
            if (ug != null)
            {
                ug.repositionNow = true;
            }
            if (tf.parent != null)
            {
                tf.parent.localPosition = new UnityEngine.Vector3(0, 0, 0);
            }
        }
    }

    private void SetTaskInfo(UnityEngine.GameObject go, int taskid, string schedule, bool sign = false)
    {
        ArkCrossEngine.MissionConfig missionconfig = ArkCrossEngine.LogicSystem.GetMissionDataById(taskid);
        if (missionconfig != null && go != null)
        {
            UnityEngine.Transform tf = go.transform.Find("Name");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = missionconfig.Name + "：" + missionconfig.Description;
                    if (missionconfig.MissionType == (int)MissionType.MonthCard)
                    {
                        ul.text += StrDictionaryProvider.Instance.Format(32, schedule);
                    }
                }
            }
            tf = go.transform.Find("TaskType");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    if (missionconfig.MissionType == 1)
                    {
                        ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(124);
                        go.transform.name = "0";
                    }
                    if (missionconfig.MissionType == 2)
                    {
                        ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(125);
                        go.transform.name = "1";
                    }
                    if (missionconfig.MissionType == 3 || missionconfig.MissionType == 4)
                    {
                        ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(148);
                        go.transform.name = "2";
                    }
                }
            }
            tf = go.transform.Find("Schedule");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    if (schedule != null)
                    {
                        ul.text = schedule;//ArkCrossEngine.StrDictionaryProvider.Instance.Format(126, schedule);
                    }
                    else
                    {
                        ul.text = "";
                    }
                }
            }
            tf = go.transform.Find("Award");
            if (sign)
            {
                SetAwardAndPosition(tf, missionconfig.DropId, missionconfig.Id);
            }
        }
        //     ArkCrossEngine.RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
        //     if (ri != null && missionconfig != null) {
        //       if (ri.Level < missionconfig.LevelLimit) {
        //         if (NGUITools.GetActive(go)) {
        //           NGUITools.SetActive(go, false);
        //           UnityEngine.Transform tf = gameObject.transform.Find("Container/Scroll View/Grid");
        //           if (tf != null) {
        //             UIGrid ug = tf.gameObject.GetComponent<UIGrid>();
        //             if (ug != null) {
        //               ug.repositionNow = true;
        //             }
        //           }
        //         }
        //       } else {
        //         if (!NGUITools.GetActive(go)) {
        //           NGUITools.SetActive(go, true);
        //           UnityEngine.Transform tf = gameObject.transform.Find("Container/Scroll View/Grid");
        //           if (tf != null) {
        //             UIGrid ug = tf.gameObject.GetComponent<UIGrid>();
        //             if (ug != null) {
        //               ug.repositionNow = true;
        //             }
        //           }
        //         }
        //       }
        //     }
    }

    private void SetAwardAndPosition(UnityEngine.Transform tf, int dropid, int missionId)
    {
        ArkCrossEngine.Data_SceneDropOut dsdo = ArkCrossEngine.SceneConfigProvider.Instance.GetSceneDropOutById(dropid);
        UnityEngine.Vector3 pos = new UnityEngine.Vector3(80.0f, 0.0f, 0.0f);
        if (tf != null && dsdo != null)
        {
            if (dsdo.m_GoldSum > 0)
            {
                UnityEngine.Transform tt = tf.Find("Money");
                if (tt != null)
                {
                    //NGUITools.SetActive(tt.gameObject, true);
                    pos = tt.localPosition;
                    pos = new UnityEngine.Vector3(pos.x + 55, pos.y, 0.0f);

                    tt = tt.Find("Label");
                    if (tt != null)
                    {
                        UILabel ul = tt.gameObject.GetComponent<UILabel>();
                        if (ul != null)
                        {
                            ul.text = "X" + dsdo.m_GoldSum;
                        }

                        pos = new UnityEngine.Vector3(pos.x + ul.localSize.x, pos.y, 0.0f);
                    }
                }
            }
            else
            {
                UnityEngine.Transform tt = tf.Find("Money");
                if (tt != null)
                {
                    NGUITools.SetActive(tt.gameObject, false);
                }
            }
            if (dsdo.m_Exp > 0)
            {
                UnityEngine.Transform tt = tf.Find("Exp");
                RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
                if (tt != null && roleInfo != null)
                {
                    //NGUITools.SetActive(tt.gameObject, true);
                    tt.localPosition = pos;
                    pos = tt.localPosition;
                    pos = new UnityEngine.Vector3(pos.x + 55, pos.y, 0.0f);

                    tt = tt.Find("Label");
                    if (tt != null)
                    {
                        UILabel ul = tt.gameObject.GetComponent<UILabel>();
                        if (ul != null)
                        {
                            ul.text = "X" + roleInfo.GetMissionStateInfo().GetMissionsExpReward(missionId, roleInfo.Level);
                        }

                        pos = new UnityEngine.Vector3(pos.x + ul.localSize.x, pos.y, 0.0f);
                    }
                }
            }
            else
            {
                UnityEngine.Transform tt = tf.Find("Exp");
                if (tt != null)
                {
                    NGUITools.SetActive(tt.gameObject, false);
                }
            }
            if (dsdo.m_Diamond > 0)
            {
                UnityEngine.Transform tt = tf.Find("Diamond");
                if (tt != null)
                {
                    //NGUITools.SetActive(tt.gameObject, true);
                    tt.localPosition = pos;
                    pos = tt.localPosition;
                    pos = new UnityEngine.Vector3(pos.x + 55, pos.y, 0.0f);

                    tt = tt.Find("Label");
                    if (tt != null)
                    {
                        UILabel ul = tt.gameObject.GetComponent<UILabel>();
                        if (ul != null)
                        {
                            ul.text = "X" + dsdo.m_Diamond;
                        }

                        pos = new UnityEngine.Vector3(pos.x + ul.localSize.x, pos.y, 0.0f);
                    }
                }
            }
            else
            {
                UnityEngine.Transform tt = tf.Find("Diamond");
                if (tt != null)
                {
                    NGUITools.SetActive(tt.gameObject, false);
                }
            }
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            if (null != role_info)
            {
                List<int> rewardItemIdList = dsdo.GetRewardItemByHeroId(role_info.HeroId);
                if (null != rewardItemIdList && rewardItemIdList.Count > 0)
                {
                    int count = rewardItemIdList.Count;
                    pos = new UnityEngine.Vector3(pos.x + 30, pos.y, 0f);
                    for (int i = 0; i < count; ++i)
                    {
                        ArkCrossEngine.ItemConfig ic = ArkCrossEngine.ItemConfigProvider.Instance.GetDataById(rewardItemIdList[i]);
                        if (ic != null)
                        {
                            UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/GameTask/Item"));
                            if (go != null)
                            {
                                go = NGUITools.AddChild(tf.gameObject, go);
                                if (go != null)
                                {
                                    go.transform.localPosition = pos;
                                    pos = go.transform.localPosition;
                                    pos = new UnityEngine.Vector3(pos.x + 65, pos.y, 0.0f);
                                    UnityEngine.Texture utt = GamePokeyManager.GetTextureByPicName(ic.m_ItemTrueName);
                                    UITexture ut = go.GetComponent<UITexture>();
                                    if (ut != null)
                                    {
                                        if (utt != null)
                                        {
                                            ut.mainTexture = utt;
                                        }
                                    }
                                    UnityEngine.Transform tt = go.transform.Find("Frame");
                                    if (tt != null)
                                    {
                                        UISprite us = tt.gameObject.GetComponent<UISprite>();
                                        if (us != null)
                                        {
                                            us.spriteName = "EquipFrame" + ic.m_PropertyRank;
                                        }
                                    }
                                    tt = go.transform.Find("Label");
                                    if (tt != null)
                                    {
                                        UILabel ul = tt.gameObject.GetComponent<UILabel>();
                                        if (ul != null)
                                        {
                                            ul.text = "X" + dsdo.m_ItemCountList[i];
                                        }
                                        pos = new UnityEngine.Vector3(pos.x + ul.localSize.x, pos.y, 0.0f);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void TaskItemClick(UnityEngine.GameObject go)
    {
        if (go != null)
        {
            foreach (int id in finishtask)
            {
                if (taskDic.ContainsKey(id))
                {
                    UnityEngine.GameObject godic = taskDic[id];
                    if (godic != null && godic == go)
                    {
                        // by leeQ
                        UIManager.Instance.ShowWindowByName("TaskAward");
                        UnityEngine.GameObject god = UIManager.Instance.GetWindowGoByName("TaskAward");
                        if (god != null)
                        {
                            TaskAward ta = god.GetComponent<TaskAward>();
                            if (ta != null)
                            {
                                ta.SetAwardProperty(id);
                                ta.InitTaskId(id, TaskCompleteType.T_common);
                            }
                        }
                        ////发送已读消息
                        //ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_read_finish", "lobby", id);
                        //LogicSystem.EventChannelForGfx.Publish("ge_ui_award_finished", "ui");//通关副本按钮
                        //UIManager.Instance.HideWindowByName("GameTask");
                        //GfxSystem.PublishGfxEvent("ge_ui_connect_hint", "ui", false, true);
                        return;
                    }
                }
            }
        }
        CloseGameTask();

        foreach (int key in taskDic.Keys)
        {
            if (taskDic[key] == go)
            {
                ArkCrossEngine.MissionConfig missionconfig = ArkCrossEngine.LogicSystem.GetMissionDataById(key);
                if (GotoTargetUI(missionconfig))
                {
                    break;
                }
            }
        }
    }

    private bool GotoTargetUI(ArkCrossEngine.MissionConfig missionconfig)
    {
        bool result = false;
        if (missionconfig != null)
        {
            switch (missionconfig.TargetUI)
            {
                case 1://副本
                    UnityEngine.GameObject goc = UIManager.Instance.TryGetWindowGameObject("SceneSelect");
                    if (goc != null)
                    {
                        LogicSystem.SendStoryMessage("cityplayermove", 0);//寻路
                        UISceneSelect uss = goc.GetComponent<UISceneSelect>();
                        if (uss != null)
                        {
                            //uss.StartChapter(missionconfig.SceneId);
                            uss.startChapterId = missionconfig.SceneId;
                        }
                    }
                    result = true;
                    break;
                case 2://pvp
                    LogicSystem.SendStoryMessage("cityplayermove", 2);
                    result = true;
                    break;
                case 3://活动
                    LogicSystem.SendStoryMessage("cityplayermove", 1);
                    result = true;
                    break;
            }
        }
        return result;
    }

    public void CloseGameTask()
    {
        UIManager.Instance.HideWindowByName("GameTask");
        JoyStickInputProvider.JoyStickEnable = true;
        if (WorldSystem.Instance.IsPureClientScene())
        {
           // CYGTConnector.ShowCYGTSDK();
        }
    }
    private List<int> finishtask = new List<int>();
    private Dictionary<int, UnityEngine.GameObject> taskDic = new Dictionary<int, UnityEngine.GameObject>();
    static public List<int> awardtask = new List<int>();
}
