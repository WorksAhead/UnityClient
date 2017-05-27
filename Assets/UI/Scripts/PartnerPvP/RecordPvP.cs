using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using ArkCrossEngine;
public class RecordPvP : UnityEngine.MonoBehaviour
{
    /*事件list*/
    private List<object> eventList = new List<object>();
    /*移除panel的Subscribe*/
    public void UnSubscribe()
    {
        try
        {
            if (null != eventList)
            {
                for (int i = 0; i < eventList.Count; i++)
                {
                    if (eventList[i] != null)
                    {
                        ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eventList[i]);
                    }
                }
            }
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
            AddSubscribe();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    /*添加监听 subscribe*/
    void AddSubscribe()
    {
        object eo;
        eo = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = LogicSystem.EventChannelForGfx.Subscribe("query_history_result", "arena", RecordInfoHandler);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = LogicSystem.EventChannelForGfx.Subscribe<UnityEngine.GameObject>("click_record_item", "record", ClickItemHandler);
        if (null != eo)
        {
            eventList.Add(eo);
        }
    }
    void OnEnable()
    {
        try
        {
            if (UIDataCache.Instance.masterRecord)
            {
                LogicSystem.PublishLogicEvent("query_history", "arena");
                UIDataCache.Instance.masterRecord = false;
            }
            else
            {
                RecordInfoHandler();
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    // 返回记录信息
    void ClickItemHandler(UnityEngine.GameObject go)
    {
        try
        {
            //LogicSystem.EventChannelForGfx.Publish("click_record_item", "record", recordDic[go]);
            UnityEngine.GameObject rd = UIManager.Instance.GetWindowGoByName("PPVPRecordData");
            if (rd != null)
            {
                UIRecordData uird = rd.GetComponent<UIRecordData>();
                if (uird != null)
                {
                    uird.ShowRecord(recordDic[go]);
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // 返回记录信息
    void RecordInfoHandler()
    {
        try
        {
            ClearItem();
            RoleInfo role = LobbyClient.Instance.CurrentRole;
            role.ArenaStateInfo.ChallengeHistory.Sort(ChallengeSort);
            for (int index = 0; index < role.ArenaStateInfo.ChallengeHistory.Count; ++index)
            {
                AddItem(role.ArenaStateInfo.ChallengeHistory[index]);
            }
            GridSort();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //挑战排序
    int ChallengeSort(ChallengeInfo info1, ChallengeInfo info2)
    {
        if (info1.ChallengeEndTime < info2.ChallengeEndTime)
        {
            return 1;
        }
        else if (info1.ChallengeEndTime > info2.ChallengeEndTime)
        {
            return -1;
        }
        return 0;
    }
    //grid排序
    void GridSort()
    {
        if (grid != null)
        {
            UIGrid ug = grid.GetComponent<UIGrid>();
            if (ug != null)
            {
                ug.repositionNow = true;
            }
        }
    }
    // 添加item
    void AddItem(ChallengeInfo info)
    {
        if (info == null)
        {
            return;
        }
        UnityEngine.GameObject item = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/PartnerPvp/RecordCell"));
        item = NGUITools.AddChild(grid.gameObject, item);
        if (item != null)
        {
            RecordCell cell = item.GetComponent<RecordCell>();
            if (cell != null)
            {
                cell.InitItemInfo(info);
            }
        }

        recordDic.Add(item, info);
    }
    //清空
    void ClearItem()
    {
        if (recordDic != null)
        {
            foreach (UnityEngine.GameObject go in recordDic.Keys)
            {
                NGUITools.DestroyImmediate(go);
            }
            if (recordDic.Count > 0)
                recordDic.Clear();
        }

    }
    //关闭
    public void ClosePanel()
    {
        ClearItem();
        UIManager.Instance.HideWindowByName("Record");
    }
    public UIGrid grid;
    /*<记录信息item，记录信息>*/
    private Dictionary<UnityEngine.GameObject, ChallengeInfo> recordDic = new Dictionary<UnityEngine.GameObject, ArkCrossEngine.ChallengeInfo>();
}
