using System;
using System.Collections.Generic;
using ArkCrossEngine;
public class Ranking : UnityEngine.MonoBehaviour
{
    /*战神信息*/
    private Dictionary<UnityEngine.GameObject, GroupMemberInfo> godDic = new Dictionary<UnityEngine.GameObject, GroupMemberInfo>();
    /*名人赛信息*/
    private Dictionary<UnityEngine.GameObject, GroupMemberInfo> masterDic = new Dictionary<UnityEngine.GameObject, GroupMemberInfo>();
    /*事件list*/
    private List<object> eventList = new List<object>();
    /*移除panel的Subscribe*/
    public UIGrid godGrid;
    public UIGrid masterGrid;
    public UIScrollView godView;
    public UIScrollView masterView;
    public UIDragScrollView godDrag;
    public UIDragScrollView masterDrag;

    private UnityEngine.Vector3 outPos = new UnityEngine.Vector3(-1000f, -1000f, -1000f);
    private UnityEngine.Vector3 upPos = new UnityEngine.Vector3(0f, 108f, 0f);
    private UnityEngine.Vector3 downPos = new UnityEngine.Vector3(0f, -202f, 0f);
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
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Awake()
    {
        try
        {
            AddSubscribe();
            role = LobbyClient.Instance.CurrentRole;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Use this for initialization
    void Start()
    {
        try
        {
            InitPanel();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }

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
        eo = LogicSystem.EventChannelForGfx.Subscribe("query_rank_result", "arena", GodBackInfo);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = LogicSystem.EventChannelForGfx.Subscribe<UnityEngine.GameObject>("record_click_item", "record", ClickItemHandler);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<List<GowDataForMsg>>("ge_sync_gowstar_list", "gowstar", SyncMars);
        if (eo != null)
            eventList.Add(eo);
    }
    private void SyncMars(List<GowDataForMsg> marslist)
    {
        try
        {
            if (marslist != null)
            {
                int mlcount = marslist.Count;
                int glcount = golist.Count;
                UnityEngine.GameObject item = UIManager.Instance.GetWindowGoByName("Ranking");
                UnityEngine.Transform tfr = item.transform.Find("ScrollView2/Grid");
                if (tfr == null)
                    return;
                for (int i = 0; i < mlcount; ++i)
                {
                    if (i < glcount)
                    {
                        UnityEngine.GameObject go = golist[i];
                        if (go != null)
                        {
                            GowDataForMsg gdfm = marslist[i];
                            if (gdfm != null)
                            {
                                SetMarsCellInfo(go, gdfm, i);
                            }
                        }
                    }
                    else
                    {
                        UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/Mars/MarsCell"));
                        if (go != null)
                        {
                            go = NGUITools.AddChild(tfr.gameObject, go);
                            if (go != null)
                            {
                                golist.Add(go);
                                GowDataForMsg gdfm = marslist[i];
                                if (gdfm != null)
                                {
                                    SetMarsCellInfo(go, gdfm, i);
                                }
                            }
                        }
                    }
                }
                if (glcount > mlcount)
                {
                    for (int j = mlcount; j < glcount; ++j)
                    {
                        UnityEngine.GameObject go = golist[j];
                        if (go != null)
                        {
                            NGUITools.DestroyImmediate(go);
                        }
                    }
                    golist.RemoveRange(mlcount, glcount - mlcount);
                }
                UIGrid ug = tfr.gameObject.GetComponent<UIGrid>();
                if (ug != null)
                {
                    ug.repositionNow = true;
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void SetMarsCellInfo(UnityEngine.GameObject go, GowDataForMsg gdfm, int order)
    {
        if (go != null && gdfm != null)
        {
            UnityEngine.Transform tf = go.transform.Find("Label4/Sprite");
            if (tf != null)
            {
                UISprite us = tf.gameObject.GetComponent<UISprite>();
                if (us != null)
                {
                    us.spriteName = "no" + (order + 1);
                }
            }
            tf = go.transform.Find("Head");
            if (tf != null)
            {
                UISprite us = tf.gameObject.GetComponent<UISprite>();
                if (us != null)
                {
                    Data_PlayerConfig cg = PlayerConfigProvider.Instance.GetPlayerConfigById(gdfm.m_Heroid);
                    us.spriteName = cg.m_PortraitForCell;
                }
            }
            tf = go.transform.Find("Label0");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = "Lv." + gdfm.m_Level;
                }
            }
            tf = go.transform.Find("Label1");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = gdfm.m_FightingScore.ToString();
                }
            }
            tf = go.transform.Find("Label2");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = gdfm.m_GowElo.ToString();
                }
            }
            tf = go.transform.Find("Label3");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = gdfm.m_Nick;
                }
            }
            tf = go.transform.Find("Label4");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    if (order + 1 < 4)
                    {
                        ul.text = "NOD" + (char)('A' + order);
                    }
                    else
                    {
                        ul.text = (order + 1).ToString() + "ETH";
                    }
                }
            }
        }
    }
    void ClickItemHandler(UnityEngine.GameObject go)
    {
        switch (currTable)
        {
            case 1:
                LogicSystem.EventChannelForGfx.Publish("", "", godDic[go]);
                break;
            case 2:
                LogicSystem.EventChannelForGfx.Publish("", "", masterDic[go]);
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            MyItemIsShow();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // 定时更新
    private float time;
    private float perTime = 0.8f;
    private bool mayRefresh = true;

    void GodBackInfo()
    {
        Masters();
        RecordInfoHandler(role.ArenaStateInfo.RankList);
    }
    // 返回记录信息
    void RecordInfoHandler(List<ArenaTargetInfo> RankList)
    {
        try
        {
            bool addMy = false;
            bool addPoint = false;
            int pointRank = 16; // 显示名省略号的名次
            int topRank = 10;
            ArenaBaseConfig cg = ArenaConfigProvider.Instance.GetBaseConfigById(1);
            if (cg != null)
            {
                topRank = cg.QueryTopRankCount;
                pointRank = cg.QueryTopRankCount + cg.QueryFrontRankCount + 1;
                if (role.ArenaStateInfo.Rank > pointRank)
                {
                    addPoint = true;
                }
            }
            for (int index = 0; index < RankList.Count; ++index)
            {
                if (RankList[index].Rank > topRank && addPoint)
                {
                    AddPointItem();
                    addPoint = false;
                }
                if (RankList[index].Rank > role.ArenaStateInfo.Rank && !addMy && role.ArenaStateInfo.Rank > 0)
                {
                    AddMyItem();
                    addMy = true;
                }
                AddItem(RankList[index]);
            }
            if (role.ArenaStateInfo.Rank < 0)
            {
                AddMyItem();
            }
            GridSort();
            curPosition = SeacherMyPosition();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //找到自己的位置
    int SeacherMyPosition()
    {
        int position = -1;
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].GetComponent<RankingCell>().playerName.text == role.Nickname)
            {
                position = i;
            }
        }
        return position;
    }
    // 我的item是否在显示区
    void MyItemIsShow()
    {
        if (currTable == 1)
        {
            return;
        }
        float gridY = masterView.transform.localPosition.y;
        if (gridY > curPosition * cellHeight + viewHeight + 8)
        { // 在显示区上
            ActiveView(upItem, upPos);
            ActiveView(downItem, outPos);
        }
        else if (gridY > (curPosition - showItemNum + 1) * cellHeight + viewHeight)
        { // 显示区内
            ActiveView(upItem, outPos);
            ActiveView(downItem, outPos);
        }
        else
        { // 在显示区下
            ActiveView(upItem, outPos);
            ActiveView(downItem, downPos);
        }
    }
    private void ActiveView(UnityEngine.GameObject go, UnityEngine.Vector3 pos)
    {
        if (null != go)
        {
            go.transform.localPosition = pos;
        }
    }
    //grid排序
    void GridSort()
    {
        if (masterGrid != null)
        {
            UIGrid ug = masterGrid.GetComponent<UIGrid>();
            if (ug != null)
            {
                ug.repositionNow = true;
            }
        }
    }
    //grid排序
    void GridSortGod()
    {
        if (godGrid != null)
        {
            UIGrid ug = godGrid.GetComponent<UIGrid>();
            if (ug != null)
            {
                ug.repositionNow = true;
            }
        }
    }
    //添加item
    void AddItem(ArenaTargetInfo info)
    {
        if (info == null)
        {
            return;
        }
        UnityEngine.GameObject item = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/PartnerPvp/RankingCell"));
        item = NGUITools.AddChild(masterGrid.gameObject, item);
        if (item != null)
        {
            RankingCell cell = item.GetComponent<RankingCell>();
            if (cell != null)
            {
                cell.InitItemInfo(info);
            }
        }
        itemList.Add(item);
    }
    void AddPointItem()
    {
        UnityEngine.GameObject item = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/PartnerPvp/RankingCell"));
        if (item != null)
        {
            RankingCell cell = item.GetComponent<RankingCell>();
            if (cell != null)
            {
                cell.InitItemInfo(null);
            }
        }
        item = NGUITools.AddChild(masterGrid.gameObject, item);
        itemList.Add(item);
    }
    void AddMyItem()
    {
        ArenaTargetInfo info = new ArenaTargetInfo();
        info.Nickname = role.Nickname;
        info.Rank = role.ArenaStateInfo.Rank;
        info.Level = role.Level;
        info.HeroId = role.HeroId;
        info.Guid = role.Guid;
        info.FightingScore = (int)role.FightingScore;
        AddItem(info);
        InitUpAndDownItem(info);
        UIDataCache.Instance.curRank = role.ArenaStateInfo.Rank;
    }
    //初始化 上下item
    void InitUpAndDownItem(ArenaTargetInfo info)
    {
        if (upItem != null)
        {
            RankingCell cell = upItem.GetComponent<RankingCell>();
            if (cell != null)
            {
                cell.InitItemInfo(info);
            }
        }
        if (downItem != null)
        {
            RankingCell cell = downItem.GetComponent<RankingCell>();
            if (cell != null)
            {
                cell.InitItemInfo(info);
            }
        }
    }
    //清空
    public void ClearItem()
    {
        if (itemList != null)
        {
            foreach (UnityEngine.GameObject go in itemList)
            {
                NGUITools.DestroyImmediate(go);
            }
            if (itemList.Count > 0)
                itemList.Clear();
        }
        ActiveView(upItem, outPos);
        ActiveView(downItem, outPos);
    }
    //关闭
    public void ClosePanel()
    {
        UIManager.Instance.HideWindowByName("Ranking");
    }
    /*1:队伍信息， 2： 待确认列表*/
    public static int currTable = 1;
    /*战神赛*/
    public void GodWar()
    {
        if (currTable != 1)
        {
            currTable = 1;
            NGUITools.SetActive(godView.gameObject, true);
            NGUITools.SetActive(masterView.gameObject, false);
            NGUITools.SetActive(godDrag.gameObject, true);
            NGUITools.SetActive(masterDrag.gameObject, false);
            curPosition = -1;
            ActiveView(upItem, outPos);
            ActiveView(downItem, outPos);
            RefrenshTabelIcon();
        }
    }
    //重置scorllview
    void ResetScrollView()
    {
        UnityEngine.Transform tfScrollView = gameObject.transform.Find("ScrollView");
        UIScrollView scroll = tfScrollView.GetComponent<UIScrollView>();
        scroll.ResetPosition();
    }
    /*名人赛*/
    public void Masters()
    {
        if (currTable != 2)
        {
            NGUITools.SetActive(godView.gameObject, false);
            NGUITools.SetActive(masterView.gameObject, true);
            NGUITools.SetActive(godDrag.gameObject, false);
            NGUITools.SetActive(masterDrag.gameObject, true);
            currTable = 2;
            RoleInfo role = LobbyClient.Instance.CurrentRole;
            curPosition = SeacherMyPosition();
            RefrenshTabelIcon();
        }
    }
    //刷新tabel键图片
    void RefrenshTabelIcon()
    {
        UIButton bt;
        switch (currTable)
        {
            case 1:
                btnGod.spriteName = "biao-qian-an-niu2";
                bt = btnGod.GetComponent<UIButton>();
                bt.normalSprite = "biao-qian-an-niu2";
                btnMaster.spriteName = "biao-qian-an-niu1";
                bt = btnMaster.GetComponent<UIButton>();
                bt.normalSprite = "biao-qian-an-niu1";
                break;
            case 2:
                btnGod.spriteName = "biao-qian-an-niu1";
                bt = btnGod.GetComponent<UIButton>();
                bt.normalSprite = "biao-qian-an-niu1";
                btnMaster.spriteName = "biao-qian-an-niu2";
                bt = btnMaster.GetComponent<UIButton>();
                bt.normalSprite = "biao-qian-an-niu2";
                break;
        }
    }
    public void InitPanel()
    {
        ClearItem();
        GfxSystem.EventChannelForLogic.Publish("ge_get_gowstar_list", "lobby", 0, 10);
        if (role.ArenaStateInfo.IsNeedQueryTopRank())
        {
            LogicSystem.PublishLogicEvent("query_rank_list", "arena");
        }
        else
        {
            RecordInfoHandler(role.ArenaStateInfo.RankList);
        }
    }
    // type 1:打开战神赛 2：名人赛
    public void OpenType(int type)
    {
        currTable = -1;
        switch (type)
        {
            case 1:
                GodWar();
                break;
            case 2:
                Masters();
                break;
        }
        UIManager.Instance.ShowWindowByName("Ranking");
    }
    private List<UnityEngine.GameObject> golist = new List<UnityEngine.GameObject>();
    public RoleInfo role;
    private int curPosition;// 当前我的位置
    private float cellHeight = 101;
    private int maxItemNum = 20;//最大item数
    private int showItemNum = 4;//窗口显示的item数
    public UnityEngine.GameObject upItem;//上面显示的自己item
    public UnityEngine.GameObject downItem;//下面显示自己的item
                                           /*item list*/
    private List<UnityEngine.GameObject> itemList = new List<UnityEngine.GameObject>();
    private int viewHeight = 200;//view的y坐标
    private bool needRefresh = true;
    public UISprite btnGod;
    public UISprite btnMaster;
}
