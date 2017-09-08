using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ArkCrossEngine;

public class TrialJinBi : UnityEngine.MonoBehaviour
{

    private List<object> m_EventList = new List<object>();

    public UILabel labelDesc = null;
    public UILabel labelTitle = null;
    public UILabel labelTimeTip = null;
    public UILabel label1 = null;
    public UILabel label2 = null;
    public UIButton btnMatch = null;
    public UIButton btnStart = null;

    [HideInInspector]
    public bool isMatching = false;

    private int sceneId = 4021;
    private string startTime;
    public UnityEngine.GameObject matchTimeUI = null;//匹配时间ui
    public UILabel matchTime = null;
    public UILabel matchNote = null;
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
            m_EventList.Clear();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void InitNotification()
    {
        object obj = null;
        obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (obj != null)
            m_EventList.Add(obj);
        obj = LogicSystem.EventChannelForGfx.Subscribe<String, int, int>("ge_mpve_match_result", "group", OnMatchResult);
        if (obj != null)
            m_EventList.Add(obj);

        Init();
        //更新按钮状态
        UpdateButtonState();
    }

    void OnEnable()
    {
        try
        {
            if (labelTimeTip != null)
            {
                MpveTimeConfig timeConfig = MpveTimeConfigProvider.Instance.GetDataById(sceneId);
                startTime = "00:00";
                //string endTime = "00:00";
                if (timeConfig != null)
                {
                    startTime = timeConfig.m_StartHour >= 10 ? timeConfig.m_StartHour.ToString() : "0" + timeConfig.m_StartHour;
                    startTime += ":" + (timeConfig.m_StartMinute >= 10 ? timeConfig.m_StartMinute.ToString() : "0" + timeConfig.m_StartMinute);

                    //endTime = timeConfig.m_EndHour > 10 ? timeConfig.m_EndHour.ToString() : "0" + timeConfig.m_EndHour;
                    //endTime += ":" + (timeConfig.m_EndMinute > 10 ? timeConfig.m_EndMinute.ToString() : "0" + timeConfig.m_EndMinute);
                }
                //labelTimeTip.text = StrDictionaryProvider.Instance.Format(879, startTime, endTime);
                RoleInfo role = LobbyClient.Instance.CurrentRole;
                if (role != null)
                {
                    labelTimeTip.text = StrDictionaryProvider.Instance.Format(886, 2 - role.GoldCurAcceptedCount, 2);
                }
            }
            if (labelTitle != null)
            {
                labelTitle.text = StrDictionaryProvider.Instance.GetDictString(882);
            }
            if (label1 != null)
            {
                label1.text = StrDictionaryProvider.Instance.GetDictString(884).Replace("\\n", "\n");
            }
            if (label2 != null)
            {
                label2.text = StrDictionaryProvider.Instance.GetDictString(885).Replace("\\n", "\n");
            }
            SetMatchTimeUIActive(false);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void Init()
    {
        if (labelDesc != null)
        {
            labelDesc.text = StrDictionaryProvider.Instance.GetDictString(883).Replace("\\n", "\n");
        }

        //更新按钮状态
        UpdateButtonState();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            UpdataMatchTimeUI(RealTime.deltaTime);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    //更新按钮状态
    private void UpdateButtonState()
    {
        if (btnMatch != null && btnStart != null)
        {
            UnityEngine.Transform tf = btnMatch.transform.Find("Label");
            if (tf != null)
            {
                UILabel label = tf.gameObject.GetComponent<UILabel>();
                if (label != null)
                {
                    SetMatchTimeUIActive(isMatching);
                    if (isMatching == true)
                    {
                        label.text = StrDictionaryProvider.Instance.GetDictString(872);
                    }
                    else
                    {
                        label.text = StrDictionaryProvider.Instance.GetDictString(873);
                    }
                }
            }
        }
    }
    //设置匹配时间ui可见性
    private bool isShowMatchTimeUI = false;
    void SetMatchTimeUIActive(bool show)
    {
        time = 0f;
        isShowMatchTimeUI = show;
        NGUITools.SetActive(matchTimeUI, show);
    }
    //更新显示
    private float time = 0;
    void UpdataMatchTimeUI(float delta)
    {
        if (isShowMatchTimeUI)
        {
            time += delta;

            if (matchTime != null)
            {
                string str1 = ((int)time / 60).ToString();
                if (str1.Length == 1)
                {
                    str1 = "0" + str1;
                }
                string str2 = ((int)time % 60).ToString();
                if (str2.Length == 1)
                {
                    str2 = "0" + str2;
                }
                matchTime.text = str1 + ":" + str2;
            }
            if (matchNote != null)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder(ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(137));
                sb.Append('.', (int)(time * 10 % 7));
                if (matchNote != null)
                {
                    matchNote.text = sb.ToString();
                }
            }
        }
    }
    public void OnClickMatch()
    {
        if (CheckMatchingOther())
        {//正在匹配其他
            return;
        }
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        GroupInfo group = role.Group;
        if (isMatching == true)
        {
            if (null != group && group.Members.Count > 1)
            {
                if (group.CreatorGuid == role.Guid)
                {
                    LogicSystem.PublishLogicEvent("ge_cancel_match", "lobby", MatchSceneEnum.Gold);
                }
                else
                {
                    SendScreeTipCenter(853);
                }
            }
            else
            {
                LogicSystem.PublishLogicEvent("ge_cancel_match", "lobby", MatchSceneEnum.Gold);
            }
        }
        else
        {
            if (group.Count == 0)
            {//无组队
                LogicSystem.PublishLogicEvent("ge_request_match_mpve", "lobby", sceneId);
            }
            else
            {//有组队
                if (group.CreatorGuid == role.Guid)
                {//自己是队长
                    if (group.Members.Count >= GroupInfo.c_MemberNumMax)
                    {//已满
                        SendScreeTipCenter(567);//提示队伍已满
                    }
                    else
                    {
                        LogicSystem.PublishLogicEvent("ge_request_match_mpve", "lobby", sceneId);
                    }
                }
                else
                {//其他人队长
                    SendScreeTipCenter(853);//提示 你不是队长
                }
            }
        }
    }

    public void OnClickStart()
    {
        if (CheckMatchingOther())
        {//正在匹配其他
            return;
        }

        if (isMatching == true)
        {
            SendScreeTipCenter(868);//匹配中
        }
        else
        {
            RoleInfo role = LobbyClient.Instance.CurrentRole;
            GroupInfo group = role.Group;
            if (group.Count < GroupInfo.c_MemberNumMax)
            {//组队未满
             //提示单人进入？
                ArkCrossEngine.MyAction<int> Func = SendStart;
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(888), null,
                  ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(855),
                  ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(9), Func, false);
            }
            else
            {//有组队
                if (group.CreatorGuid == role.Guid)
                {//自己是队长
                    LogicSystem.PublishLogicEvent("ge_start_mpve", "lobby", sceneId);
                }
                else
                {//其他人队长
                    SendScreeTipCenter(853);//提示 你不是队长
                }
            }
        }
    }

    private void SendStart(int action)
    {
        if (action == 1)
        {
            LogicSystem.PublishLogicEvent("ge_start_mpve", "lobby", sceneId/*(int)MatchSceneEnum.Gold*/);
        }
    }

    private void SendScreeTipCenter(int id, string name = "")
    {
        string chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.Format(id, name);
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_screen_tip_invoke", "ui", chn_desc, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
    }

    private void OnMatchResult(String nick, int result, int scene)
    {
        if (scene != sceneId)
        {
            return;
        }
        if (result == (int)ArkCrossEngine.Network.TeamOperateResult.OR_Succeed)
        {
            isMatching = true;
            UpdateButtonState();
            LogicSystem.EventChannelForGfx.Publish("ge_match_state_change", "matching", MatchingType.Gold, true);
        }
        else if (result == (int)ArkCrossEngine.Network.TeamOperateResult.OR_Busyness)
        { //有人在忙（打副本啥的）
            SendScreeTipCenter(864, nick);
        }
        else if (result == (int)ArkCrossEngine.Network.TeamOperateResult.OR_LevelError)
        {//有人等级不够
            SendScreeTipCenter(865, nick);
        }
        else if (result == (int)ArkCrossEngine.Network.TeamOperateResult.OR_NotCaptain)
        {//不是队长
            SendScreeTipCenter(866);
        }
        else if (result == (int)ArkCrossEngine.Network.TeamOperateResult.OR_CancelMatch)
        {//取消了匹配
            isMatching = false;
            UpdateButtonState();
            LogicSystem.EventChannelForGfx.Publish("ge_match_state_change", "matching", MatchingType.Gold, false);
            SendScreeTipCenter(867, nick);
        }
        else if (result == (int)ArkCrossEngine.Network.TeamOperateResult.OR_Overflow)
        {//次数用光
            SendScreeTipCenter(887, nick);
        }
        else if (result == (int)ArkCrossEngine.Network.TeamOperateResult.OR_TimeError)
        {
            SendScreeTipCenter(880, startTime);
        }
    }

    private bool CheckMatchingOther()
    {
        if (WorldSystem.Instance.WaitMatchSceneId > 0 && WorldSystem.Instance.WaitMatchSceneId != sceneId)
        {
            Data_SceneConfig config = SceneConfigProvider.Instance.GetSceneConfigById(WorldSystem.Instance.WaitMatchSceneId);
            SendScreeTipCenter(852, config.m_SceneName);//正在匹配{0}
            return true;
        }
        return false;
    }
}
