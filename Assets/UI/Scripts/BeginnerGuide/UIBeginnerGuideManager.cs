/*新手引导类*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public enum UINewbieGuideTriggerType
{
    T_MainCity = 0,
    T_MissionPanel = 1,
    T_SceneSelect = 2,
    T_SceneIntroduce = 3,
    T_Equiment = 4,
    T_EquimentInfo = 5,
    T_SkillPanel = 6,
    T_PartnerPanel = 7,
    T_PartnerStrenthen = 8,
    T_PartnerLiftSkill = 9,
    T_Activity = 10,
    T_SkillBar = 11,
    T_PartnerSummon = 12,
    T_None
}
public class UIBeginnerGuideManager
{

    private UnityEngine.GameObject m_LastGuideHand;
    private UnityEngine.GameObject m_CurrentGuideHand;
    private const int c_GuideFinished = 1;
    private const int c_GuideUnfinished = 0;
    private const int c_SkillDragGuideId = 24;
    private const int c_TriggerNum = 100;
    private int[] TriggerStateArr = new int[100];
    private int m_CurrentGuideId = int.MaxValue;
    private int m_LastGuideId = int.MinValue;
    private int m_FinishedId = int.MaxValue;
    private UINewbieGuideTriggerType m_CurrentTriggerType = UINewbieGuideTriggerType.T_None;
    public bool IsBeginnerGuiderStarted = false;
    private UnityEngine.GameObject m_CurrentParent = null;
    private List<NewbieGuideConfig> m_NewbieGuideCfgList = new List<NewbieGuideConfig>();
    /*key:和UINewBieGuideTriggerType枚举类型对应，value为当前界面所有的新手引导Id   */
    private Dictionary<int, List<int>> m_GuideTriggerDic = new Dictionary<int, List<int>>();
    /*key:新手引导GroupId   value:Group成员的新手引导Id*/
    private Dictionary<int, List<int>> m_GuideGroupDic = new Dictionary<int, List<int>>();
    // Use this for initialization
    /*初始化所有数据*/
    public void InitNewbieGuideData()
    {
        //LogicSystem.EventChannelForGfx.Subscribe("ge_sync_newbie_action_flag", "newbie", HandleSyncNewbieFlag);
        TriggerStateArr.Initialize();
        LogicSystem.EventChannelForGfx.Subscribe("ge_sync_newbie_flag", "newbie", HandleSyncNewbieFlag);
        m_NewbieGuideCfgList = NewbieGuideProvider.Instance.GetAllConfig();
        for (int index = 0; index < m_NewbieGuideCfgList.Count; ++index)
        {
            NewbieGuideConfig cfg = m_NewbieGuideCfgList[index];
            if (cfg != null)
            {
                //初始化m_GuideTriggerDic
                List<int> NewBieCfgIdList;
                if (m_GuideTriggerDic.TryGetValue(cfg.m_TriggerUiId, out NewBieCfgIdList))
                {
                    NewBieCfgIdList.Add(cfg.Id);
                    NewBieCfgIdList.Sort();
                }
                else
                {
                    NewBieCfgIdList = new List<int>();
                    NewBieCfgIdList.Add(cfg.Id);
                    //从小打到排序
                    NewBieCfgIdList.Sort();
                    m_GuideTriggerDic.Add(cfg.m_TriggerUiId, NewBieCfgIdList);
                }

                //初始化m_GuideGroupDic
                List<int> guideIds;
                if (m_GuideGroupDic.TryGetValue(cfg.m_GroupId, out guideIds))
                {
                    guideIds.Add(cfg.Id);
                    guideIds.Sort();
                }
                else
                {
                    guideIds = new List<int>();
                    guideIds.Add(cfg.Id);
                    m_GuideGroupDic.Add(cfg.m_GroupId, guideIds);
                }
            }
        }
        //Test();
    }
    private void Test()
    {
        Debug.Log("触发窗口字典");
        foreach (int index in m_GuideTriggerDic.Keys)
        {
            Debug.Log(index + ":***********************************");
            for (int j = 0; j < m_GuideTriggerDic[index].Count; ++j)
            {
                Debug.Log(m_GuideTriggerDic[index][j]);
            }
        }
        Debug.Log("组字典");
        foreach (int index in m_GuideGroupDic.Keys)
        {
            Debug.Log(index + ":***********************************");
            for (int j = 0; j < m_GuideGroupDic[index].Count; ++j)
            {
                Debug.Log(m_GuideGroupDic[index][j]);
            }
        }
    }
    /*触发新手引导*/
    public void TriggerNewbieGuide(UINewbieGuideTriggerType trigger_type, UnityEngine.GameObject parent)
    {
        if (parent == null) return;
        List<int> guideIds = new List<int>();
        if (m_GuideTriggerDic.TryGetValue((int)trigger_type, out guideIds))
        {
            NewbieGuideConfig guideCfg = GetCurrentGuideCfg(guideIds);
            if (guideCfg == null)
            {
                //Debug.Log("GuideCfg is null.");
                UIManager.Instance.HideWindowByName("GuideDialog");
                return;
            }
            UnityEngine.Transform trans = parent.transform.Find(guideCfg.m_TargetChildPath);
            if (trans == null)
            {
                Debug.Log("Find " + guideCfg.m_TargetChildPath + " failed.");
                UIManager.Instance.HideWindowByName("GuideDialog");
                return;
            }
            //guideCfg.TargetChildIndex!=-1表示，需要找到tran的第index-1个孩子（孩子数从0开始数）
            if (guideCfg.m_TargetChildIndex != -1)
            {
                if (trans.childCount > 0)
                {
                    trans = trans.GetChild(guideCfg.m_TargetChildIndex - 1);
                }
                else
                {
                    UIManager.Instance.HideWindowByName("GuideDialog");
                    return;
                }
            }
            if (!NGUITools.GetActive(trans.gameObject))
            {
                //目标UI不可见返回
                return;
            }
            //*************NGUIDebug.Log("TriggerNewbieGuide:" + guideCfg.Id);
            m_LastGuideId = m_CurrentGuideId;//(m_LastGuideId == m_CurrentGuideId)?int.MaxValue:m_CurrentGuideId;
            m_CurrentGuideId = guideCfg.Id;
            m_FinishedId = guideCfg.Id;
            if (m_LastGuideId == int.MaxValue)
            {
                m_LastGuideId = m_CurrentGuideId;
            }
            m_CurrentParent = parent;
            m_CurrentTriggerType = trigger_type;

            UIEventListener event_listner = null;
            if (guideCfg.Id == c_SkillDragGuideId)
            {
                ////技能拖动引导
                event_listner = trans.gameObject.AddComponent<UIEventListener>();
                if (event_listner != null)
                {
                    event_listner.onPress = OnTargetPress;
                }
            }
            else
            {
                event_listner = trans.gameObject.AddComponent<UIEventListener>();
                if (event_listner != null)
                {
                    event_listner.onClick = OnTargetClick;
                }
            }
            if (guideCfg.m_AlwaysNeedGuideDlg || (guideCfg.m_NeedGuideDlg && !IsGuideAppeared(guideCfg.Id)))
            {
                SetAppearedGuide(guideCfg.Id);
                ShowGuideDialog(guideCfg);
            }
            else
            {
                UIManager.Instance.HideWindowByName("GuideDialog");
            }
            UnityEngine.GameObject obj = ShowGuideHand(trans, trans.position);
            if (guideCfg.Id == c_SkillDragGuideId)
            {
                //技能拖动引导
                AddTweenerPositionForSkill(obj, trans);
            }
            if (trigger_type == UINewbieGuideTriggerType.T_MainCity)
            {
                parent = UIManager.Instance.GetWindowGoByName("MainCityUI");
            }
            if (parent != null)
            {
                UIGuideScript guideScript = parent.GetComponent<UIGuideScript>();
                if (guideScript != null) guideScript.StoreGuideInfo(obj, event_listner, m_CurrentGuideId);
            }
        }
    }
    /*获取当前的新手Config*/
    private NewbieGuideConfig GetCurrentGuideCfg(List<int> guideIds)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            for (int index = 0; index < guideIds.Count; ++index)
            {
                if (!IsGuideFinished(guideIds[index]))
                {
                    NewbieGuideConfig cfg = NewbieGuideProvider.Instance.GetDataById(guideIds[index]);
                    if (cfg == null) continue;
                    if (IsTriggerSceneFinished(role_info, cfg.m_TriggerSceneId) && role_info.Level >= cfg.m_TriggerLevelMini && role_info.Level <= cfg.m_TriggerLevelMax)
                        return cfg;
                }
            }
        }
        return null;
    }
    /*需要完成特定的场景或者正在该场景中（副本中的新手引导）才能触发该新手引导*/
    private bool IsTriggerSceneFinished(RoleInfo info, int sceneId)
    {
        if (sceneId == -1) return true;
        if (info == null) return false;
        if (UIDataCache.Instance.curSceneId == sceneId || info.SceneInfo != null && info.SceneInfo.ContainsKey(sceneId))
            return true;
        return false;
    }
    /*点击回调*/
    private void OnTargetClick(UnityEngine.GameObject go)
    {
        int guideId = -1;
        if (go != null)
        {
            UIGuideScript guideScript = NGUITools.FindInParents<UIGuideScript>(go);
            if (guideScript != null)
            {
                guideId = guideScript.GetCurrentGuideId();
                guideScript.Clear();
            }
        }
        FinishNewbieGuide(guideId);
        m_LastGuideId = m_CurrentGuideId;
        m_FinishedId = m_LastGuideId;
    }
    /*按下回调*/
    private void OnTargetPress(UnityEngine.GameObject go, bool state)
    {
        if (state)
        {
            int guideId = -1;
            if (go != null)
            {
                UIGuideScript guideScript = NGUITools.FindInParents<UIGuideScript>(go);
                if (guideScript != null)
                {
                    guideId = guideScript.GetCurrentGuideId();
                    guideScript.Clear();
                }
            }
            FinishNewbieGuide(guideId);
            m_LastGuideId = m_CurrentGuideId;
            m_FinishedId = m_LastGuideId;
        }
    }
    /*结束当前Id的引导*/
    public void FinishNewbieGuide(int guideId)
    {
        if (guideId == m_CurrentGuideId)
            UIManager.Instance.HideWindowByName("GuideDialog");
        //*************NGUIDebug.Log("FinishNewbieGuide:" + guideId);
        NewbieGuideConfig cfg = NewbieGuideProvider.Instance.GetDataById(guideId);
        if (cfg == null) return;
        List<int> groupIds;
        if (m_GuideGroupDic.TryGetValue(cfg.m_GroupId, out groupIds))
        {
            if (groupIds != null)
            {
                RecordGuideState(guideId);
                SetGuideState(guideId, c_GuideFinished);
                for (int index = 0; index < groupIds.Count; ++index)
                {
                    //将该Group中在guideId之前的引导置为已完成（m_GuideGroupDic.values已经从小到大排序）
                    if (groupIds[index] == guideId) break;
                    SetGuideState(groupIds[index], c_GuideFinished);
                }
                //触发同一引导组且在同一界面下的UI
                TriggerNextNewbieGuide(guideId, groupIds);
            }
        }
    }
    private void TriggerNextNewbieGuide(int guideId, List<int> groupIds)
    {
        for (int index = 0; index < groupIds.Count; ++index)
        {
            //（m_GuideGroupDic.values已经从小到大排序）
            if (groupIds[index] == guideId && groupIds.Count > (index + 1))
            {
                int nextGuideId = groupIds[index + 1];
                NewbieGuideConfig currentGuideCfg = NewbieGuideProvider.Instance.GetDataById(guideId);
                NewbieGuideConfig nextGuideCfg = NewbieGuideProvider.Instance.GetDataById(nextGuideId);
                if (currentGuideCfg != null && nextGuideCfg != null
                  && nextGuideCfg.m_TriggerUiId == currentGuideCfg.m_TriggerUiId)
                    TriggerNewbieGuide(m_CurrentTriggerType, m_CurrentParent);
                break;
            }
        }
    }
    public void UnFinishNewbieGuide(int unFinishId)
    {
        if (!IsGuideFinished(unFinishId))
        {
            //*************NGUIDebug.Log("UnFinishNewbieGuide:" + unFinishId);
            if (unFinishId == m_CurrentGuideId)
                UIManager.Instance.HideWindowByName("GuideDialog");
            ResetNewbieGuide(unFinishId);
            m_FinishedId = unFinishId;
            m_LastGuideId = int.MaxValue;
            m_LastGuideHand = null;
        }
    }
    /*当新手引导==guideId未完成，则重置其对应某些新手引导,新手引导不存在循环重置的情况，所以不需考虑循环重置的情况*/
    public void ResetNewbieGuide(int guideId)
    {
        NewbieGuideConfig cfg = NewbieGuideProvider.Instance.GetDataById(guideId);
        if (cfg == null)
        {
            return;
        }
        List<int> groupIds;
        if (m_GuideGroupDic.TryGetValue(cfg.m_GroupId, out groupIds))
        {
            if (groupIds != null)
            {
                bool isInRange = false;//记录是否在可重置区间
                for (int index = 0; index < groupIds.Count; ++index)
                {
                    //（m_GuideGroupDic.values已经从小到大排序）
                    if (groupIds[index] == cfg.m_ResetToGuideId || isInRange)
                    {
                        SetGuideState(groupIds[index], c_GuideUnfinished);
                        isInRange = true;
                    }
                }
            }
        }
    }
    /*显示引导手势*/
    private UnityEngine.GameObject ShowGuideHand(UnityEngine.Transform trans, UnityEngine.Vector3 worldPos)
    {
        m_LastGuideHand = m_CurrentGuideHand;//(m_LastGuideHand == m_CurrentGuideHand)?null:m_CurrentGuideHand;
        if (m_LastGuideHand != null) NGUITools.Destroy(m_LastGuideHand);
        string path = UIManager.Instance.GetPathByName("GuideHand");
        UnityEngine.Object obj = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.NewObject(path));
        m_CurrentGuideHand = NGUITools.AddChild(trans.gameObject, obj);
        if (m_LastGuideHand == null)
        {
            //第一次触发
            m_LastGuideHand = m_CurrentGuideHand;
        }
        return m_CurrentGuideHand;
    }
    /*显示对话*/
    private void ShowGuideDialog(NewbieGuideConfig guideCfg)
    {
        if (guideCfg == null) return;
        UnityEngine.GameObject guideDialog = UIManager.Instance.GetWindowGoByName("GuideDialog");
        if (guideDialog == null)
        {
            //第一次会动态加载该UI
            guideDialog = UIManager.Instance.LoadWindowByName("GuideDialog", UICamera.mainCamera);
        }
        if (guideDialog != null)
        {
            UIManager.Instance.ShowWindowByName("GuideDialog");
            UnityEngine.Vector3 screenPos = new UnityEngine.Vector3();
            screenPos.z = 0f;
            screenPos.x = guideCfg.m_RelativeScreenPos[0] * Screen.width;
            screenPos.y = guideCfg.m_RelativeScreenPos[1] * Screen.height;

            UnityEngine.Vector3 worldPos = UICamera.mainCamera.ScreenToWorldPoint(screenPos);
            guideDialog.transform.position = worldPos;
            UIGuideDlg guideDlgScript = guideDialog.GetComponent<UIGuideDlg>();
            if (guideDlgScript != null) guideDlgScript.SetDescription(guideCfg.m_Words, guideCfg.m_IsSpeakerAtLeft);
        }
    }
    //技能的新手引导小手
    private void AddTweenerPositionForSkill(UnityEngine.GameObject guideHand, UnityEngine.Transform transFrom)
    {
        if (transFrom == null) return;

        UnityEngine.GameObject objSkillPanel = UIManager.Instance.GetWindowGoByName("SkillPanel");
        if (objSkillPanel != null)
        {
            UISkillPanel uiSkillPanel = objSkillPanel.GetComponent<UISkillPanel>();
            if (uiSkillPanel != null)
            {
                UISkillSlot uiSkillSlot = uiSkillPanel.uiSkillSetting.skillStorageArr[0];
                if (uiSkillSlot != null)
                {
                    UnityEngine.Transform targetTrans = uiSkillSlot.transform.Find("icon");
                    TweenTransform tweenTransform = TweenTransform.Begin(guideHand, 2f, targetTrans);
                    tweenTransform.from = transFrom;
                    tweenTransform.style = UITweener.Style.Loop;
                    UIGuideHandScript guideHandScript = guideHand.GetComponent<UIGuideHandScript>();
                    if (guideHandScript != null)
                    {
                        UnityEngine.Transform iconUnderHand = transFrom.Find("icon");
                        if (iconUnderHand == null) return;
                        UISprite sp = iconUnderHand.GetComponent<UISprite>();
                        if (sp != null)
                            guideHandScript.SkillStyle(sp.atlas, sp.spriteName);
                    }
                }
            }
        }
    }
    //设置新手引导状态
    public void SetGuideState(int guideId, int guideState)
    {
        if (guideId < TriggerStateArr.Length)
        {
            TriggerStateArr[guideId] = guideState;
        }
        //LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
        LogicSystem.PublishLogicEvent("ge_set_newbie_flag", "lobby", guideId, guideState);
    }
    public void RecordGuideState(int guideId)
    {
        LogicSystem.PublishLogicEvent("ge_record_newbie_flag", "lobby", guideId);
    }
    /*判断当前新手引导是否已经完成*/
    public bool IsGuideFinished(int guideId)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (TriggerStateArr.Length > guideId && TriggerStateArr[guideId] == c_GuideFinished)
            return true;
        if (role_info != null && role_info.IsDoneGuide(guideId))
        {
            return true;
        }
        return false;
    }
    //设置新手引导是否已经出现
    public void SetAppearedGuide(int guideId)
    {
        //LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
        LogicSystem.PublishLogicEvent("ge_set_newbie_action_flag", "lobby", guideId, 1);
    }
    //是否已经出现过
    public bool IsGuideAppeared(int guideId)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null && role_info.IsDoneGuideAction(guideId))
        {
            return true;
        }
        return false;
    }
    /*根据触发类型获取配置表：限于同一触发器指触发一个新手的引导情况（Ex技能、伙伴chuzh）*/
    public NewbieGuideConfig GetNewbieGuideCfg(UINewbieGuideTriggerType trigger)
    {
        List<NewbieGuideConfig> cfgs = NewbieGuideProvider.Instance.GetAllConfig();
        if (cfgs != null)
        {
            for (int index = 0; index < cfgs.Count; ++index)
            {
                if (cfgs[index] != null && cfgs[index].m_TriggerUiId == (int)trigger)
                    return cfgs[index];
            }
        }
        return null;
    }
    private void HandleSyncNewbieFlag()
    {
        LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, false);
    }
    public void ResetLocalTriggerData()
    {
        //这里在是新手引导过程中的临时数据，每一次登录游戏都需要初始化（切换账号也需要）
        for (int index = 0; index < TriggerStateArr.Length; ++index)
        {
            TriggerStateArr[index] = c_GuideUnfinished;
        }
    }
    private static UIBeginnerGuideManager m_Instance = new UIBeginnerGuideManager();
    public static UIBeginnerGuideManager Instance
    {
        get
        {
            return m_Instance;
        }
    }
}
