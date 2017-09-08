using UnityEngine;
using System.Collections;
using ArkCrossEngine;
using System.Collections.Generic;
using System;



public class TrialShiLian : UnityEngine.MonoBehaviour
{
    enum BoxState : int
    {
        Normal = 0,
        Current,
        Opened
    }
    private List<object> m_EventList = new List<object>();
    public UILabel labelTimeTip = null;
    public UILabel labelDesc = null;
    public UILabel labelCurrentBox = null;
    public UILabel labelNextBox = null;
    public UIButton btnMatch = null;
    public UIButton btnStart = null;
    public List<UnityEngine.GameObject> boxGoList = new List<UnityEngine.GameObject>();
    public float yNormal;
    public float yCurrent;
    public float yOpened;
    public UILabel labelTitle = null;
    public UnityEngine.GameObject boxLabelGO = null;

    [HideInInspector]
    public bool isMatching = false;

    private int sceneId = 4031;
    private string startTime = "";
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

    public void InitNotification()
    {
        object obj = null;
        obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (obj != null)
            m_EventList.Add(obj);
        obj = LogicSystem.EventChannelForGfx.Subscribe<String, int, int>("ge_mpve_match_result", "group", OnMatchResult);
        if (obj != null)
            m_EventList.Add(obj);
        obj = LogicSystem.EventChannelForGfx.Subscribe<int, int, List<int>, List<int>, ArkCrossEngine.Network.MpveAwardResult>("ge_mpve_attempt_award", "mpve", OnAwardResult);
        if (obj != null)
            m_EventList.Add(obj);
        Init();
    }

    void OnEnable()
    {
        try
        {
            if (labelTimeTip != null)
            {
                MpveTimeConfig timeConfig = MpveTimeConfigProvider.Instance.GetDataById(sceneId);
                startTime = "00:00";
                string endTime = "00:00";
                if (timeConfig != null)
                {
                    startTime = timeConfig.m_StartHour >= 10 ? timeConfig.m_StartHour.ToString() : "0" + timeConfig.m_StartHour;
                    startTime += ":" + (timeConfig.m_StartMinute >= 10 ? timeConfig.m_StartMinute.ToString() : "0" + timeConfig.m_StartMinute);

                    endTime = timeConfig.m_EndHour >= 10 ? timeConfig.m_EndHour.ToString() : "0" + timeConfig.m_EndHour;
                    endTime += ":" + (timeConfig.m_EndMinute >= 10 ? timeConfig.m_EndMinute.ToString() : "0" + timeConfig.m_EndMinute);
                }
                labelTimeTip.text = StrDictionaryProvider.Instance.Format(879, startTime, endTime);
            }
            if (labelTitle != null)
            {
                labelTitle.text = StrDictionaryProvider.Instance.GetDictString(874);
            }
            UpdateBoxState();
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
            labelDesc.text = StrDictionaryProvider.Instance.GetDictString(850).Replace("\\n", "\n");
        }
        //宝箱状态
        if (boxGoList != null)
        {
            for (int i = 0; i < boxGoList.Count; i++)
            {
                UIEventListener.Get(boxGoList[i]).onClick += this.OnClickBox;
            }

            //UpdateBoxState();
        }
        //更新按钮状态
        UpdateButtonState();
    }

    //更新宝箱状态
    private void UpdateBoxState()
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        int currentMaxAwardId = role.AttemptAward;//当前最好的奖品ＩＤ
        int hasGetTimes = role.AttemptCurAcceptedCount;//当天领取过奖品的次数
        int hasGetAwardId = role.AttemptAcceptedAward;//已领奖励id

        if (currentMaxAwardId == 0)
        {
            if (labelCurrentBox != null)
            {
                labelCurrentBox.text = StrDictionaryProvider.Instance.GetDictString(863).Replace("\\n", "\n");//暂无奖励
                labelNextBox.text = "";
            }
        }

        if (hasGetTimes <= 0)
        { //未领过
            for (int i = 1; i <= boxGoList.Count; i++)
            {
                if (i != currentMaxAwardId)
                {
                    ChangeBoxView(i, BoxState.Normal);
                }
                else
                {
                    ChangeBoxView(i, BoxState.Current);
                    if (labelCurrentBox != null)
                    {
                        labelCurrentBox.text = StrDictionaryProvider.Instance.Format(851, GetBoxName(i)).Replace("\\n", "\n");//当前可以领取{0}
                    }
                    if (labelNextBox != null)
                    {
                        if (i != boxGoList.Count)
                        {
                            labelNextBox.text = StrDictionaryProvider.Instance.GetDictString(862).Replace("\\n", "\n");//不满意？继续
                        }
                        else
                        {
                            labelNextBox.text = "";
                        }
                    }
                }
            }
        }
        else
        { //已领取
            for (int i = 1; i <= boxGoList.Count; i++)
            {
                if (i != hasGetAwardId)
                {
                    ChangeBoxView(i, BoxState.Normal);
                }
                else
                {
                    ChangeBoxView(i, BoxState.Opened);
                    if (labelCurrentBox != null)
                    {
                        labelCurrentBox.text = StrDictionaryProvider.Instance.Format(856, GetBoxName(i)).Replace("\\n", "\n");//已领取{0}
                    }
                    if (labelNextBox != null)
                    {
                        labelNextBox.text = "";
                    }
                }
            }
        }
    }

    private void ChangeBoxView(int index, BoxState state)
    {
        UnityEngine.GameObject box = boxGoList[index - 1];
        UnityEngine.Vector3 position = box.transform.localPosition;
        UISprite sprite = box.GetComponent<UISprite>();
        UIButton button = box.GetComponent<UIButton>();
        UnityEngine.BoxCollider collider = box.GetComponent<UnityEngine.BoxCollider>();

        if (sprite != null)
        {
            switch (state)
            {
                case BoxState.Normal:
                    sprite.spriteName = "xiangzi4";
                    sprite.SetDimensions(100, 100);
                    box.transform.localPosition = new UnityEngine.Vector3(position.x, yNormal, position.z);
                    button.enabled = false;
                    collider.enabled = false;
                    break;
                case BoxState.Current:
                    sprite.spriteName = "xiangzi2";
                    sprite.SetDimensions(160, 160);
                    box.transform.localPosition = new UnityEngine.Vector3(position.x, yCurrent, position.z);
                    button.enabled = true;
                    collider.enabled = true;
                    break;
                case BoxState.Opened:
                    sprite.spriteName = "xiangzi5";
                    sprite.SetDimensions(210, 260);
                    box.transform.localPosition = new UnityEngine.Vector3(position.x, yOpened, position.z);
                    button.enabled = false;
                    collider.enabled = false;
                    break;
            }
        }
        //设置宝箱文本的高度相对摄像机固定
        UnityEngine.Transform tf = box.transform.Find("LabelBox");
        if (tf != null)
        {
            UILabel label = tf.gameObject.GetComponent<UILabel>();
            if (label != null)
            {
                UnityEngine.Vector3 labelPos = label.transform.position;
                if (boxLabelGO != null)
                {
                    label.transform.position = new UnityEngine.Vector3(labelPos.x, boxLabelGO.transform.position.y, labelPos.z);
                }
                if (state == BoxState.Normal)
                {
                    label.color = new UnityEngine.Color(1f, 1f, 1f);
                }
                else
                {
                    label.color = new UnityEngine.Color(1f, (float)90 / 255, 0f);
                }
            }
        }
    }

    private string GetBoxName(int index)
    {
        string name = "";
        switch (index)
        {
            case 1:
                name = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(857);
                break;
            case 2:
                name = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(858);
                break;
            case 3:
                name = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(859);
                break;
            case 4:
                name = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(860);
                break;
            case 5:
                name = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(861);
                break;
        }
        return name;
    }

    public void OnClickBox(UnityEngine.GameObject go)
    {
        LogicSystem.PublishLogicEvent("ge_mpve_attempt_award", "lobby");
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
                    LogicSystem.PublishLogicEvent("ge_cancel_match", "lobby", MatchSceneEnum.Attempt);
                }
                else
                {
                    SendScreeTipCenter(853);
                }
            }
            else
            {
                LogicSystem.PublishLogicEvent("ge_cancel_match", "lobby", MatchSceneEnum.Attempt);
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
                        ShowTip(ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(567));//提示队伍已满
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
            ShowTip(ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(868));//匹配中
        }
        else
        {
            RoleInfo role = LobbyClient.Instance.CurrentRole;
            GroupInfo group = role.Group;
            if (group.Count < GroupInfo.c_MemberNumMax)
            {//组队未满
             //提示单人进入？
                ArkCrossEngine.MyAction<int> Func = SendStart;
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(854), null,
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
            LogicSystem.PublishLogicEvent("ge_start_mpve", "lobby", sceneId);
        }
    }

    private void ShowTip(String str)
    {
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", str,
              ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(140), null, null, null/*fun*/, false);
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
            LogicSystem.EventChannelForGfx.Publish("ge_match_state_change", "matching", MatchingType.Trial, true);
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
            LogicSystem.EventChannelForGfx.Publish("ge_match_state_change", "matching", MatchingType.Trial, false);
            SendScreeTipCenter(867, nick);
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

    private void OnAwardResult(int awardIndex, int addMoney, List<int> itemIdList, List<int> itemNumList, ArkCrossEngine.Network.MpveAwardResult result)
    {
        if (result == ArkCrossEngine.Network.MpveAwardResult.Succeed)
        {
            UpdateBoxState();
            //获得奖励的表现
            UnityEngine.GameObject goTaskAward = UIManager.Instance.GetWindowGoByName("TaskAward");
            if (goTaskAward != null)
            {
                TaskAward taskAward = goTaskAward.GetComponent<TaskAward>();
                taskAward.SetAwardForTrial(addMoney, 0, 0, itemIdList, itemNumList);
            }
            UIManager.Instance.ShowWindowByName("TaskAward");
        }
    }
}
