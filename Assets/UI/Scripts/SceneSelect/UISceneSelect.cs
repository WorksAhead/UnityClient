using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;
public enum SubSceneType : int
{
    Common = 0,
    Master = 1,
    UnKnown
}
public class UISceneSelect : UnityEngine.MonoBehaviour
{
    [HideInInspector]
    public int startChapterId = -1;
    //章节预设
    public UnityEngine.GameObject goChapter = null;
    public UILabel lblName = null;
    public UILabel lblStamina = null;//体力值
    public UILabel lblPlayerName = null;
    public UILabel lblPlayerLevel = null;
    public UILabel lblMoney = null;
    public UILabel lblDiamond = null;
    public UISprite spPortrait = null;
    public UIProgressBar staminaBar = null;

    public UICenterOnScene centerOnChild = null;

    public UIButton leftButton = null;
    public UIButton rightButton = null;
    public UIButton btnCommon = null;
    public UIButton btnMaster = null;
    //显示的场景ID
    public int StartSceneId = 9001;
    private int m_ChapterNumber = 0;
    private const int c_ScenePerChapter = 9;
    private int m_CurrentChapterId = 1;
    private SubSceneType m_SubSceneType = SubSceneType.Common;
    //

    //每一个页面的长度，用于定位
    private const int C_ChapterLength = 900;
    private int offset = 0;
    private int m_OnClickNum = 1;
    private SubSceneType m_TweenSubSceneType = SubSceneType.UnKnown;
    private int m_TweenChapter = -1;
    private int m_TweenScene = -1;
    private bool m_IsLeftClick = false;
    private bool m_IsRightClick = false;
    private UnityEngine.Vector3 delta = UnityEngine.Vector3.zero;
    private List<object> m_EventList = new List<object>();
    private const int c_SinglePVE = 1;
    private const string c_AshButton = "biao-qian-an-niu1";
    private const string c_BrightButton = "biao-qian-an-niu2";
    private List<UICurrentChapter> m_ChapterList = new List<UICurrentChapter>();

    private int m_UnlockCommonChapterMax = 1;//当前解锁剧情章节数
    private int m_UnlockMasterChapterMax = 0;//当前解锁精英章节数
    private int m_UnlockChapterMax = 1;
    private bool m_IsChapterMoving = false;
    // Use this for initialization
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
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Awake()
    {
        try
        {
            object obj = null;
            if (LobbyClient.Instance.CurrentRole != null)
            {
                obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
                if (obj != null) m_EventList.Add(obj);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Start()
    {
        try
        {
            //StartChapterByPres();
            //UIManager.Instance.HideWindowByName("SceneSelect");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private bool hasCommonChapter = false;
    private bool hasMasterChapter = false;
    void OnEnable()
    {
        try
        {
            m_IsChapterMoving = false;
            RoleInfo info = LobbyClient.Instance.CurrentRole;
            if (null != info)
            {
                if (lblDiamond != null) lblDiamond.text = info.Gold.ToString();
                if (lblMoney != null) lblMoney.text = info.Money.ToString();
                if (lblPlayerLevel != null) lblPlayerLevel.text = info.Level.ToString();
                if (lblPlayerName != null) lblPlayerName.text = info.Nickname;
                UserInfo user_info = info.GetPlayerSelfInfo();
                if (user_info != null)
                {
                    Data_PlayerConfig playerData = PlayerConfigProvider.Instance.GetPlayerConfigById(info.HeroId);
                    if (playerData != null)
                    {
                        if (spPortrait != null) spPortrait.spriteName = playerData.m_Portrait;
                    }
                }
            }
            hasCommonChapter = HasCommonChapter();
            hasMasterChapter = HasMasterChapter();
            if (startChapterId > 0)
            {
                StartChapter(startChapterId);
                startChapterId = 0;
            }
            else
            {
                if (!hasCommonChapter)
                {
                    m_SubSceneType = SubSceneType.Master;
                }
                else if (!hasMasterChapter)
                {
                    m_SubSceneType = SubSceneType.Common;
                }
                InitChapters(m_SubSceneType);
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
        try
        {
            RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
            if (roleInfo != null)
            {
                UpdateProperty((int)roleInfo.Money, roleInfo.Gold, roleInfo.CurStamina, roleInfo.StaminaMax);
            }
            //按钮点击时，移动
            if (delta != UnityEngine.Vector3.zero)
            {
                centerOnChild.transform.localPosition += delta;
                offset += 50;
                if (offset >= C_ChapterLength * m_OnClickNum)
                {
                    delta = UnityEngine.Vector3.zero;
                    offset = 0;
                    m_OnClickNum = 1;
                    if (m_IsRightClick) leftButton.isEnabled = true;
                    if (m_IsLeftClick) rightButton.isEnabled = true;
                    m_IsChapterMoving = false;
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OnDestroy()
    {
        try
        {
            //PlayerPrefs.SetInt("ChapterIndex", m_CurrentChapterId);
            //PlayerPrefs.SetInt("ChapterType", (int)m_SubSceneType);
            UnSubscribe();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //初始化所有章
    private void InitChapters(SubSceneType subType)
    {
        m_ChapterList.Clear();
        if (subType == SubSceneType.Common)
        {
            m_SubSceneType = SubSceneType.Common;
            SetButtonSprite(true);
        }
        else
        {
            m_SubSceneType = SubSceneType.Master;
            SetButtonSprite(false);
        }
        //初始化一些数据
        UICurrentChapter.m_UnLockNextScene = false;
        if (goChapter == null || centerOnChild == null) return;
        //应该读取所有章节信息
        UIGrid grid = centerOnChild.GetComponent<UIGrid>();
        if (grid == null) return;
        int chapterIndex = 1;
        int[] chapterInfo = new int[c_ScenePerChapter];
        ClearArr(chapterInfo);
        int UnlockChapterMax = -1;
        bool isNextChapterUnlock = true;
        while (GetChapterInfo(chapterIndex, chapterInfo, subType) && isNextChapterUnlock)
        {
            UnityEngine.GameObject go = AddChapter(centerOnChild.gameObject, goChapter, chapterIndex - 1);
            UICurrentChapter currentChapter = go.GetComponent<UICurrentChapter>();
            if (currentChapter != null)
            {
                bool isLockChapter = currentChapter.InitChapterById(chapterIndex, chapterInfo, subType, ref isNextChapterUnlock);
                m_ChapterList.Add(currentChapter);
                if (!isLockChapter && chapterIndex > UnlockChapterMax)
                    UnlockChapterMax = chapterIndex;
            }
            m_UnlockChapterMax = UnlockChapterMax;
            if (m_SubSceneType == SubSceneType.Common)
            {
                m_UnlockCommonChapterMax = UnlockChapterMax;
            }
            else
            {
                m_UnlockMasterChapterMax = UnlockChapterMax;
            }
            ClearArr(chapterInfo);
            grid.Reposition();
            chapterIndex++;

        }
        CenterOnChild(UnlockChapterMax);
        //删除多余章节
        DelChapter(centerOnChild.gameObject, chapterIndex);
        m_ChapterNumber = chapterIndex - 1;
    }
    //增加章节
    public UnityEngine.GameObject AddChapter(UnityEngine.GameObject father, UnityEngine.GameObject child, int index)
    {
        if (father == null) return null;
        if (child == null) return null;
        if (index < 0) return null;
        if (father.transform.childCount > index)
        {
            UnityEngine.Transform ts = father.transform.GetChild(index);
            if (ts != null)
            {
                return ts.gameObject;
            }
        }
        else
        {
            UnityEngine.GameObject go = NGUITools.AddChild(father, child);
            return go;
        }
        return null;
    }
    //删除多余章节
    public void DelChapter(UnityEngine.GameObject father, int index)
    {
        if (father == null) return;
        if (index < 0) return;
        while (father.transform.childCount > index - 1)
        {
            UnityEngine.Transform ts = father.transform.GetChild(index - 1);
            if (ts != null) NGUITools.Destroy(ts.gameObject);
        }
    }
    //
    private void ClearArr(int[] arr)
    {
        if (arr == null) return;
        for (int index = 0; index < arr.Length; ++index)
        {
            arr[index] = -1;
        }
    }
    //获取各章节的信息
    private bool GetChapterInfo(int chapterIndex, int[] chapterInfo, SubSceneType subType)
    {
        if (chapterInfo == null) return false;
        //配表需要从第一章开始
        bool hasChapter = false;
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        int arrLength = chapterInfo.Length;
        MyDictionary<int, object> allSceneConfig = SceneConfigProvider.Instance.GetAllSceneConfig();
        foreach (Data_SceneConfig sceneCfg in allSceneConfig.Values)
        {
            if (sceneCfg != null && sceneCfg.m_Type == c_SinglePVE && sceneCfg.m_SubType == (int)subType && sceneCfg.m_RelatedMainCity == role.CitySceneId)
            {
                if (sceneCfg.m_Order < arrLength && sceneCfg.m_Order >= 0)
                {
                    chapterInfo[sceneCfg.m_Order] = sceneCfg.GetId();
                    hasChapter = true;
                }
            }
        }
        return hasChapter;
    }
    //是否有当期普通章节
    private bool HasCommonChapter()
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        MyDictionary<int, object> allSceneConfig = SceneConfigProvider.Instance.GetAllSceneConfig();
        foreach (Data_SceneConfig sceneCfg in allSceneConfig.Values)
        {
            if (sceneCfg != null && sceneCfg.m_Type == c_SinglePVE && sceneCfg.m_SubType == (int)SubSceneType.Common && sceneCfg.m_RelatedMainCity == role.CitySceneId)
            {
                return true;
            }
        }
        return false;
    }
    //是否有当期精英章节
    private bool HasMasterChapter()
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        MyDictionary<int, object> allSceneConfig = SceneConfigProvider.Instance.GetAllSceneConfig();
        foreach (Data_SceneConfig sceneCfg in allSceneConfig.Values)
        {
            if (sceneCfg != null && sceneCfg.m_Type == c_SinglePVE && sceneCfg.m_SubType == (int)SubSceneType.Master && sceneCfg.m_RelatedMainCity == role.CitySceneId)
            {
                return true;
            }
        }
        return false;
    }
    //初始化显示的章节
    public void StartChapter(int sceneId)
    {
        //UIManager.Instance.ShowWindowByName("SceneSelect");
        Data_SceneConfig cfg = SceneConfigProvider.Instance.GetSceneConfigById(sceneId);
        if (cfg != null)
        {
            if (cfg.m_Type != c_SinglePVE || cfg.m_Chapter <= 0)
            {
                Debug.LogError("!!!Scene type error,sceneId:" + sceneId);
                return;
            }

            if (cfg.m_SubType == (int)SceneSubTypeEnum.TYPE_ELITE)
            {
                InitChapters(SubSceneType.Master);
                CenterOnChild(cfg.m_Chapter);
                m_TweenSubSceneType = SubSceneType.Master;
            }
            else
            {
                InitChapters(SubSceneType.Common);
                m_TweenSubSceneType = SubSceneType.Common;
                CenterOnChild(cfg.m_Chapter);
            }
            AddTweenOnScene(m_CurrentChapterId, sceneId);

        }
        else
        {
            Debug.Log("Can not get sceneconfig " + sceneId);
        }
    }
    //
    public void StartChapterByPres()
    {
        int subSceneType = PlayerPrefs.GetInt("ChapterType");
        if (subSceneType == (int)SubSceneType.Master)
        {
            InitChapters(SubSceneType.Master);
        }
        else
        {
            InitChapters(SubSceneType.Common);
        }
    }
    //
    private void CenterOnChild(int chapterIndex)
    {
        if (centerOnChild != null)
        {
            UnityEngine.GameObject goGrid = centerOnChild.gameObject;
            goGrid.transform.localPosition += new UnityEngine.Vector3((m_CurrentChapterId - chapterIndex) * C_ChapterLength, 0, 0);
        }
        m_CurrentChapterId = chapterIndex;
        SetCurrentChapter(m_CurrentChapterId);
    }
    //设置章节名
    public void SetChapterName(int chapterId)
    {
        int sceneId = -1;

        if (m_ChapterList != null)
        {
            for (int index = 0; index < m_ChapterList.Count; ++index)
            {
                if (m_ChapterList[index] != null && m_ChapterList[index].m_ChapterId == chapterId)
                {
                    UICurrentChapter uiChapter = m_ChapterList[index];
                    int j = 0;
                    for (j = 0; j < uiChapter.sceneArr.Length; ++j)
                    {
                        if (uiChapter.sceneArr[j] != null && uiChapter.sceneArr[j].GetSceneId() != -1)
                        {
                            sceneId = uiChapter.sceneArr[j].GetSceneId();
                            break;
                        }
                    }
                    if (j < uiChapter.sceneArr.Length) break;
                }
            }
        }
        Data_SceneConfig sceneCfg = SceneConfigProvider.Instance.GetSceneConfigById(sceneId);
        //todo:根据Id读取章节名
        if (lblName != null && sceneCfg != null)
        {
            lblName.text = sceneCfg.m_ChapterName;
        }
    }
    public void AddTweenOnScene(int chapterId, int sceneId)
    {
        foreach (UICurrentChapter chapter in m_ChapterList)
        {
            if (chapter.m_ChapterId == chapterId)
            {
                m_TweenChapter = chapter.m_ChapterId;
                m_TweenScene = sceneId;
                chapter.SetStartScene(sceneId);
                break;
            }
        }
    }
    public void DelTweenOnScene(int chapter, int sceneId)
    {
        for (int i = 0; i < m_ChapterList.Count; ++i)
        {
            if (m_ChapterList[i] != null && m_ChapterList[i].m_ChapterId == chapter)
            {
                m_ChapterList[i].DelTweenOnScene(sceneId);
            }
        }
    }
    public void OnReturnBtnClick()
    {
        DelTweenOnScene(m_TweenChapter, m_TweenScene);
        m_TweenScene = -1;
        m_TweenChapter = -1;
        m_TweenSubSceneType = SubSceneType.UnKnown;
        UIManager.Instance.HideWindowByName("SceneSelect");
    }
    //普通副本按钮
    public void OnCommonBtnClick()
    {
        if (!hasCommonChapter && m_SubSceneType == SubSceneType.Master)
        {
            LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui",
              "该章节未开放，请耐心等待", UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            return;
        }
        if (IsChapterMoving()) return;
        SetButtonSprite(true);
        if (m_TweenSubSceneType == SubSceneType.Master)
        {
            DelTweenOnScene(m_TweenChapter, m_TweenScene);
        }
        InitChapters(SubSceneType.Common);
        if (m_TweenSubSceneType == SubSceneType.Common)
        {
            AddTweenOnScene(m_TweenChapter, m_TweenScene);
        }
    }
    //精英副本按钮
    public void OnEliteBtnClick()
    {
        if (IsChapterMoving()) return;
        if (m_UnlockChapterMax < 2 && m_SubSceneType == SubSceneType.Common)
        {
            //临时注释掉
            //LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui",
            //  "请先完成剧情第一章", UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            //return;
        }
        if (!hasMasterChapter && m_SubSceneType == SubSceneType.Common)
        {
            string chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(43);
            LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui",
              chn_desc, UIScreenTipPosEnum.AlignTop, UnityEngine.Vector3.zero);
            return;
        }
        SetButtonSprite(false);
        if (m_TweenSubSceneType == SubSceneType.Common)
        {
            DelTweenOnScene(m_TweenChapter, m_TweenScene);
        }
        InitChapters(SubSceneType.Master);
        if (m_TweenSubSceneType == SubSceneType.Master)
        {
            AddTweenOnScene(m_TweenChapter, m_TweenScene);
        }
    }
    //设置普通、精英按钮状态
    private void SetButtonSprite(bool isCommom)
    {
        if (isCommom)
        {
            if (btnCommon != null)
            {
                UISprite sp = btnCommon.GetComponent<UISprite>();
                if (sp != null) sp.spriteName = c_BrightButton;
                btnCommon.normalSprite = c_BrightButton;
            }
            if (btnMaster != null)
            {
                UISprite sp = btnMaster.GetComponent<UISprite>();
                if (sp != null) sp.spriteName = c_AshButton;
                btnMaster.normalSprite = c_AshButton;
            }
        }
        else
        {
            if (btnCommon != null)
            {
                UISprite sp = btnCommon.GetComponent<UISprite>();
                if (sp != null) sp.spriteName = c_AshButton;
                btnCommon.normalSprite = c_AshButton;
            }
            if (btnMaster != null)
            {
                UISprite sp = btnMaster.GetComponent<UISprite>();
                if (sp != null) sp.spriteName = c_BrightButton;
                btnMaster.normalSprite = c_BrightButton;
            }
        }
    }
    //上一章节
    public void OnForeSceneBtnClick()
    {
        if (m_CurrentChapterId > 1)
        {
            m_CurrentChapterId--;
            if (delta == UnityEngine.Vector3.zero)
            {
                delta = new UnityEngine.Vector3(50, 0, 0);
                offset = 0;
                m_OnClickNum = 1;
            }
            else
            {
                //点击次数加1
                m_OnClickNum++;
            }
            m_IsLeftClick = true;
            m_IsRightClick = false;
            m_IsChapterMoving = true;
            NGUITools.SetActive(rightButton.gameObject, true);
            rightButton.isEnabled = false;
            SetChapterName(m_CurrentChapterId);
        }
        if (m_CurrentChapterId == 1)
        {
            NGUITools.SetActive(leftButton.gameObject, false);
        }
    }

    public void OnNextSceneBtnClick()
    {
        if (m_CurrentChapterId < m_ChapterNumber)
        {
            m_CurrentChapterId++;
            if (delta == UnityEngine.Vector3.zero)
            {
                delta = new UnityEngine.Vector3(-50, 0, 0);
                offset = 0;
                m_OnClickNum = 1;
            }
            else
            {
                m_OnClickNum++;
            }
            m_IsLeftClick = false;
            m_IsRightClick = true;
            m_IsChapterMoving = true;
            NGUITools.SetActive(leftButton.gameObject, true);
            leftButton.isEnabled = false;
            SetChapterName(m_CurrentChapterId);
        }
        if (m_CurrentChapterId == m_ChapterNumber)
        {
            NGUITools.SetActive(rightButton.gameObject, false);
        }
    }
    //设置chapterIndex为当前章节及其它信息
    public void SetCurrentChapter(int chapterIndex)
    {

        m_CurrentChapterId = chapterIndex;
        if (chapterIndex == 1)
        {
            NGUITools.SetActive(leftButton.gameObject, false);
        }
        else
        {
            NGUITools.SetActive(leftButton.gameObject, true);
        }
        if (chapterIndex == m_UnlockChapterMax)
        {
            NGUITools.SetActive(rightButton.gameObject, false);
        }
        else
        {
            NGUITools.SetActive(rightButton.gameObject, true);
        }
        SetChapterName(m_CurrentChapterId);
    }
    //判断是否可以向左移动
    public bool CanMoveLeft()
    {
        if (m_CurrentChapterId == 1) return false;
        return true;
    }
    public bool CanMoveRight()
    {
        if (m_CurrentChapterId == m_UnlockChapterMax) return false;
        return true;
    }

    //设置体力值
    public void UpdateProperty(float money, float gold, int curStamina, int maxStamina)
    {
        try
        {
            if (lblStamina != null)
            {
                lblStamina.text = curStamina + "/" + maxStamina;
                if (staminaBar != null) staminaBar.value = curStamina / (float)maxStamina;
            }
            if (lblDiamond != null) lblDiamond.text = gold.ToString();
            if (lblMoney != null) lblMoney.text = money.ToString();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void OnBuyCoinClick()
    {
        UIManager.Instance.ShowWindowByName("GoldBuy");
    }
    //购买体力
    public void OnBuyStamina()
    {
        UIManager.Instance.ShowWindowByName("TiliBuy");
    }
    //判断章节是否在运动中
    public bool IsChapterMoving()
    {
        return m_IsChapterMoving;
    }
}
