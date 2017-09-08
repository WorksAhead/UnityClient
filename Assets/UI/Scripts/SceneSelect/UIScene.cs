using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class UIScene : UnityEngine.MonoBehaviour
{

    public UILabel lblName = null;
    public UISprite spSceneImage = null;
    public UISprite spLock = null;
    public UISprite spBossPortrait;
    public UnityEngine.GameObject goMasterIcon = null;
    public UnityEngine.GameObject goCommonIcon = null;
    public UnityEngine.GameObject goBossIcon;
    public UnityEngine.Vector3 masterModePos = new UnityEngine.Vector3(0, -68.9f, 0);
    private UnityEngine.Vector3 commonModePos = new UnityEngine.Vector3(0, -37f, 0);
    private UnityEngine.GameObject[] starArr = new UnityEngine.GameObject[3];

    private const string c_MasterSpirite = "hong-qi-zi";
    private const string c_CommonSpirite = "hong-qi-zi_1";
    private int m_SceneId = -1;
    private int m_SceneGrade = -1;
    private SubSceneType m_SubType = SubSceneType.Common;

    public float TweenDuration = 0.67f;
    public UnityEngine.Vector3 TweenFromScale = new UnityEngine.Vector3(1.0f, 1.0f, 1.0f);
    public UnityEngine.Vector3 TweenToScale = new UnityEngine.Vector3(1.1f, 1.1f, 1.1f);
    public UITweener.Style TweenStyle = UITweener.Style.PingPong;
    public UnityEngine.GameObject effectUnlock = null; //解锁特效
    private UnityEngine.GameObject effectClone = null; //clone特效

    /*事件list*/
    private List<object> eventList = new List<object>();
    // Use this for initialization
    void Start()
    {
        try
        {
            AddSubscribe();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    /*添加监听 subscribe*/
    void AddSubscribe()
    {
        object eo;
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("guanqia_unlock_effect", "effect", ClearEffect);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (null != eo)
        {
            eventList.Add(eo);
        }

    }
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
                /*
                foreach (object eo in eventList) {
                  if (eo != null) {
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eo);
                  }
                }*/
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void ClearEffect()
    {
        if (effectClone != null)
        {
            Destroy(effectClone);
        }
    }
    public void InitStarArr()
    {
        for (int index = 1; index <= 3; ++index)
        {
            string name = "star" + index;
            UnityEngine.Transform star = this.transform.Find(name);
            if (star != null && index <= starArr.Length)
            {
                starArr[index - 1] = star.gameObject;
            }
        }
    }
    public void InitScene(int sceneId, bool isLock, SubSceneType subType, int grade)
    {
        if (sceneId == -1)
        {
            NGUITools.SetActive(this.gameObject, false);
        }
        else
        {
            if (!NGUITools.GetActive(this.gameObject))
                NGUITools.SetActive(this.gameObject, true);
        }
        InitStarArr();
        m_SceneId = sceneId;
        m_SceneGrade = grade;
        m_SubType = subType;
        NewLockScene(isLock, sceneId);
        LockScene(isLock);
        int bossId = -1;
        Data_SceneConfig sceneCfg = SceneConfigProvider.Instance.GetSceneConfigById(sceneId);
        if (null != sceneCfg)
        {
            SetSceneName((sceneCfg.m_Order + 1).ToString());
            bossId = sceneCfg.m_BossLinkId;
        }
        ShowSceneMode(subType, grade, bossId);
    }
    void NewLockScene(bool isLock, int sceneId)
    {
        if (!isLock && UICurrentChapter.m_UnLockNextScene)
        {
            if (sceneId == UIDataCache.Instance.curUnlockSceneId || sceneId == UIDataCache.Instance.curMasterUnlockSceneId)
            {
                UIDataCache.Instance.justLogin = false;
                return;
            }
            else
            {
                switch (m_SubType)
                {
                    case SubSceneType.Common:
                        UIDataCache.Instance.curUnlockSceneId = sceneId;
                        break;
                    case SubSceneType.Master:
                        UIDataCache.Instance.curMasterUnlockSceneId = sceneId;
                        break;
                }
                if (UIDataCache.Instance.justLogin)
                {
                    UIDataCache.Instance.justLogin = false;
                    return;
                }
            }
            if (effectClone == null)
            {
                effectClone = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/DefenseEffect/SceneUnlockEffect"));
                effectClone = NGUITools.AddChild(gameObject, effectClone);
            }

        }
    }
    public void SetSceneName(string name)
    {
        if (null != lblName)
            lblName.text = name;
    }
    //点击跳转到场景介绍界面
    void OnClick()
    {
        //UIManager.Instance.HideWindowByName("SceneSelect");
        UISceneSelect sceneSelect = NGUITools.FindInParents<UISceneSelect>(gameObject);
        if (sceneSelect != null)
        {
            if (sceneSelect.IsChapterMoving())
                return;
        }
        UIManager.Instance.ShowWindowByName("SceneIntroduce");
        LogicSystem.EventChannelForGfx.Publish("ge_init_sceneintroduce", "ui", m_SceneId, m_SceneGrade, m_SubType);

    }
    public int GetSceneId()
    {
        return m_SceneId;
    }
    public void HightLight()
    {

        TweenScale tween = TweenScale.Begin(this.gameObject, TweenDuration, TweenToScale);
        if (tween != null)
        {
            tween.style = TweenStyle;
            tween.from = TweenFromScale;
        }
    }
    public void DelTweenOnScene()
    {
        TweenScale tween = this.GetComponent<TweenScale>();
        if (tween != null) Destroy(tween);
    }
    public void LockScene(bool islock)
    {
        if (spLock != null)
        {
            NGUITools.SetActive(spLock.gameObject, islock);
            UIButton uiButton = this.GetComponent<UIButton>();
            if (uiButton != null)
            {
                //临时注释掉
                //uiButton.isEnabled = !islock;
            }
        }
    }
    //显示与隐藏精英模式
    public void ShowSceneMode(SubSceneType subType, int grade, int npcId)
    {
        bool visible = subType == SubSceneType.Master ? true : false;
        if (goMasterIcon != null)
            NGUITools.SetActive(goMasterIcon, visible);
        if (goCommonIcon != null)
            NGUITools.SetActive(goCommonIcon, !visible);
        if (subType == SubSceneType.Master)
        {
            if (spSceneImage != null)
            {
                UIButton btn = this.GetComponent<UIButton>();
                if (btn != null) btn.normalSprite = c_MasterSpirite;
                spSceneImage.spriteName = c_MasterSpirite;
                spSceneImage.transform.localPosition = masterModePos;
                for (int i = 0; i < starArr.Length; ++i)
                {
                    if (i < grade)
                    {
                        if (starArr[i] != null) NGUITools.SetActive(starArr[i], true);
                    }
                    else
                    {
                        if (starArr[i] != null) NGUITools.SetActive(starArr[i], false);
                    }
                }
            }
        }
        else
        {
            if (spSceneImage != null)
            {
                UIButton btn = this.GetComponent<UIButton>();
                if (btn != null) btn.normalSprite = c_CommonSpirite;
                spSceneImage.spriteName = c_CommonSpirite;
                spSceneImage.transform.localPosition = commonModePos;
                for (int i = 0; i < starArr.Length; ++i)
                {
                    if (starArr[i] != null) NGUITools.SetActive(starArr[i], false);
                }
            }
        }
        //如果npcId！=-1  把头像给加上
        if (npcId != -1)
        {
            //boss关
            Data_NpcConfig npcCfg = NpcConfigProvider.Instance.GetNpcConfigById(npcId);
            if (npcCfg != null && spBossPortrait != null)
            {
                spBossPortrait.spriteName = npcCfg.m_Portrait;
            }
            if (goBossIcon != null) NGUITools.SetActive(goBossIcon, true);
            if (goMasterIcon != null) NGUITools.SetActive(goMasterIcon, false);
            if (goCommonIcon != null) NGUITools.SetActive(goCommonIcon, false);
        }
        else
        {
            if (goBossIcon != null) NGUITools.SetActive(goBossIcon, false);
        }
    }

}
