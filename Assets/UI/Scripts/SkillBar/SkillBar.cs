using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;
public enum UIHeroType
{
    Jianshi = 1,
    Cike = 2
};
public class SkillBar : UnityEngine.MonoBehaviour
{
    public delegate void OnCommonButtonDelegate();
    public delegate void OnButtonClickDelegate(SkillCategory skillType);
    public static OnButtonClickDelegate OnButtonClickedHandler;//用于新手关的回调
    public static OnCommonButtonDelegate OnCommomButtonClickHandler;
    public ArkCrossEngine.GameObject goEffect = null;
    private UnityEngine.GameObject m_RuntimeEffect = null;
    public UnityEngine.GameObject CommonSkillGo = null;
    public UISprite[] spBright = new UISprite[c_SkillNum];
    public UIButton[] btnSkills = new UIButton[c_SkillNum];
    public UISprite spFullEx = null;
    public UISprite spAshEx = null;
    public UISprite spAgeryValueEx = null;
    public float HighPecent = 0.2f;
    //新手引导
    private int m_TriggerSceneId = 1014;
    private int m_NewbieGuideId = 29;
    public float m_DelayForExGuideDlg = 1f;
    private bool m_IsInNewbieGuide = false;
    private bool m_IsNeedLockGameFrame = false;
    private List<object> m_EventList = new List<object>();

    public UnityEngine.GameObject SkillBarView = null;
    private SkillCategory m_OnPressedSkillCategory = SkillCategory.kNone;

    // Use this for initialization
    void Awake()
    {
        try
        {
            object obj = LogicSystem.EventChannelForGfx.Subscribe<List<SkillInfo>>("ge_equiped_skills", "ui", InitSkillBar);
            if (obj != null) m_EventList.Add(obj);
            LogicSystem.PublishLogicEvent("ge_request_equiped_skills", "ui");
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
            AddEventDelegate();
            InitEventListner();
            object obj = LogicSystem.EventChannelForGfx.Subscribe<string, float>("ge_cast_skill_cd", "ui", CastCDByGroup);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<SkillCannotCastType>("ge_skill_cannot_cast", "ui", HandleCastSkillResult);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null) m_EventList.Add(obj);

            //SetActive(false);
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
            if (m_RuntimeEffect != null)
            {
                Destroy(m_RuntimeEffect);
                m_RuntimeEffect = null;
            }
            RemoveEventDelegate();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void AddEventDelegate()
    {
        UIManager.Instance.OnHideUiDelegate += OnHideWindow;
    }
    private void RemoveEventDelegate()
    {
        UIManager.Instance.OnHideUiDelegate -= OnHideWindow;
    }

    private void OnHideWindow(string windowName)
    {
        if (windowName == "FightUI")
        {
            if (m_IsAttact == true)
            {
                //表示正在持续攻击
                m_IsPressed = false;
                m_IsAttact = false;
                GfxModule.Skill.GfxSkillSystem.Instance.StopAttack(ArkCrossEngine.LogicSystem.PlayerSelf);
            }
            if (m_OnPressedSkillCategory != SkillCategory.kNone)
            {
                GfxModule.Skill.GfxSkillSystem.Instance.BreakSkill(ArkCrossEngine.LogicSystem.PlayerSelf, m_OnPressedSkillCategory);
                m_OnPressedSkillCategory = SkillCategory.kNone;
            }
        }
    }
    //初始化按钮事件监听
    private void InitEventListner()
    {
        if (null != CommonSkillGo)
            UIEventListener.Get(CommonSkillGo).onPress = OnCommonButtonPress;
        for (int index = 0; index < btnSkills.Length; ++index)
        {
            if (btnSkills[index] != null)
            {
                UIEventListener.Get(btnSkills[index].gameObject).onPress = OnSkillButtonPress;
            }
        }
    }
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
    // Update is called once per frame
    void Update()
    {
        try
        {
            //LockGameFrame();
            UpdateAngryValue();
            if (m_TipsCD >= 0f)
            {
                m_TipsCD -= UnityEngine.Time.deltaTime;
            }
            //按钮不可点击时停止普通攻击
            if (CommonSkillGo != null)
            {
                UIButton btn = CommonSkillGo.GetComponent<UIButton>();
                if (!btn.isEnabled) m_IsPressed = false;
            }
            if (m_IsPressed)
            {
                m_IsAttact = true;
                GfxModule.Skill.GfxSkillSystem.Instance.StartAttack(ArkCrossEngine.LogicSystem.PlayerSelf, ArkCrossEngine.Vector3.zero);
            }
            else
              if (m_IsAttact == true)
            {
                m_IsAttact = false;
                GfxModule.Skill.GfxSkillSystem.Instance.StopAttack(ArkCrossEngine.LogicSystem.PlayerSelf);
            }

            if (time > 0)
            {
                time -= RealTime.deltaTime;
                if (time <= 0)
                {
                    UnityEngine.BoxCollider[] collider = m_GoList.ToArray();
                    for (int i = 0; i < collider.Length; ++i)
                    {
                        if (collider[i] != null)
                        {
                            collider[i].enabled = true;
                        }
                    }
                    m_GoList.Clear();
                }
            }
            for (int index = 0; index < c_SkillNum; ++index)
            {
                if (remainCdTime[index] > 0)
                {
                    string path = "Skill" + index.ToString() + "/skill0/CD";
                    UnityEngine.GameObject go = GetGoByPath(path);
                    if (null != go)
                    {
                        UISprite sp = go.GetComponent<UISprite>();
                        if (null != sp)
                        {
                            if (GetCDTimeByIndex(index) == 0)
                            {
                                sp.fillAmount = 0;
                            }
                            else
                            {
                                sp.fillAmount -= RealTime.deltaTime / GetCDTimeByIndex(index);
                            }
                            remainCdTime[index] = sp.fillAmount;
                            if (remainCdTime[index] <= 0)
                            {
                                IconFlashByIndex(index);
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OnDisable()
    {
        StopAttack();
        GfxModule.Skill.GfxSkillSystem.Instance.StopAttack(ArkCrossEngine.LogicSystem.PlayerSelf);
        if (m_RuntimeEffect != null)
        {
            Destroy(m_RuntimeEffect);
            m_RuntimeEffect = null;
        }
    }
    public void InitSkillBar(List<SkillInfo> skillInfos)
    {
        try
        {
            if (UIDataCache.Instance.IsMultPveScene())
            {
                NGUITools.SetActive(spAshEx.gameObject, false);
            }
            NewbieGuideConfig guideCfg = UIBeginnerGuideManager.Instance.GetNewbieGuideCfg(UINewbieGuideTriggerType.T_SkillBar);
            if (guideCfg != null)
            {
                m_TriggerSceneId = guideCfg.m_TriggerSceneId;
                m_NewbieGuideId = guideCfg.Id;
            }
            if (m_TriggerSceneId != UIDataCache.Instance.curSceneId && !UIBeginnerGuideManager.Instance.IsGuideFinished(m_NewbieGuideId))
            {
                if (!IsSceneFinished(m_TriggerSceneId))
                    NGUITools.SetActive(spAshEx.gameObject, false);
            }

            for (int i = 0; i < m_CategoryArray.Length; ++i)
            {
                UnlockSkill(m_CategoryArray[i], false);
            }
            if (skillInfos != null && skillInfos.Count > 0)
            {
                SkillInfo[] skillInfoArray = skillInfos.ToArray();
                for (int i = 0; i < skillInfoArray.Length; ++i)
                {
                    if (skillInfoArray[i] != null)
                    {
                        SlotPosition skill_pos = skillInfoArray[i].Postions.Presets[0];
                        SkillCategory cur_skill_pos = SkillCategory.kNone;
                        if (skill_pos == SlotPosition.SP_A)
                        {
                            cur_skill_pos = SkillCategory.kSkillA;
                        }
                        else if (skill_pos == SlotPosition.SP_B)
                        {
                            cur_skill_pos = SkillCategory.kSkillB;
                        }
                        else if (skill_pos == SlotPosition.SP_C)
                        {
                            cur_skill_pos = SkillCategory.kSkillC;
                        }
                        else if (skill_pos == SlotPosition.SP_D)
                        {
                            cur_skill_pos = SkillCategory.kSkillD;
                        }
                        if (SkillCategory.kNone != cur_skill_pos)
                        {
                            UnlockSkill(cur_skill_pos, true, skillInfoArray[i].ConfigData);
                        }
                        ///
                        // Debug.Log("skillinit skill_id : {" + skillInfoArray[i].SkillId + "}, skill_level : {" + skillInfoArray[i].SkillLevel + "}, skill_slotpos : {" + skill_pos + "}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //发送普通攻击消息
    //press == true 表示按下
    //press == false 表示抬起
    void OnCommonButtonPress(UnityEngine.GameObject ob, bool press)
    {
        //Debug.Log("Common attack!" + press);
        LogicSystem.EventChannelForGfx.Publish("ge_attack", "game", press);
        m_IsPressed = press;
        if (OnCommomButtonClickHandler != null)
        {
            OnCommomButtonClickHandler();
        }
    }
    public void OnSkillButtonPress(UnityEngine.GameObject pressed, bool press)
    {
        if (pressed == null) return;
        if (press)
        {
            string clickSkillName = pressed.transform.parent.gameObject.name;
            switch (clickSkillName)
            {
                case "Skill0": m_OnPressedSkillCategory = SkillCategory.kSkillA; break;
                case "Skill1": m_OnPressedSkillCategory = SkillCategory.kSkillB; break;
                case "Skill2": m_OnPressedSkillCategory = SkillCategory.kSkillC; break;
                case "Skill3": m_OnPressedSkillCategory = SkillCategory.kSkillD; break;
                default: m_OnPressedSkillCategory = SkillCategory.kNone; break;
            }
            CastSkillBySkillType(m_OnPressedSkillCategory);
        }
        else
        {
            GfxModule.Skill.GfxSkillSystem.Instance.BreakSkill(ArkCrossEngine.LogicSystem.PlayerSelf, m_OnPressedSkillCategory);
            m_OnPressedSkillCategory = SkillCategory.kNone;
        }
    }
    //按下是否技能
    public void CastSkillBySkillType(SkillCategory skillType)
    {
        //向上层逻辑发送释放技能的消息
        //Debug.Log("!! Skill type is " + skillType);
        if (null != OnButtonClickedHandler)
        {
            OnButtonClickedHandler(skillType);
        }
        else
        {
            GfxModule.Skill.GfxSkillSystem.Instance.PushSkill(ArkCrossEngine.LogicSystem.PlayerSelf, skillType, ArkCrossEngine.Vector3.zero);
        }
    }
    private void IconFlashByIndex(int index)
    {
        //Debug.Log("IconFlashByIndex:"+ index);
        string path = "Skill" + index.ToString() + "/skill0/bright";
        UnityEngine.GameObject go = GetGoByPath(path);
        if (go == null)
            return;
        NGUITools.SetActive(go, true);
        TweenAlpha tweenAlpha = go.GetComponent<TweenAlpha>();
        if (null == tweenAlpha)
        {
            tweenAlpha = go.AddComponent<TweenAlpha>();
            if (null == tweenAlpha)
                return;
        }
        tweenAlpha.from = 0;
        tweenAlpha.to = 1;
        tweenAlpha.duration = 0.4f;
        tweenAlpha.animationCurve = tweenAnimation;
        UnityEngine.GameObject goSkill = GetGoByPath("Skill" + index.ToString() + "/skill0");
        if (null != goSkill)
        {
            UIButton button = goSkill.GetComponent<UIButton>();
            if (button != null) button.isEnabled = true;
        }
    }

    public void SetSkillEnableByIndex(int index, int period, bool enable)
    {
        string childPath = "Skill" + index.ToString() + "/skill" + period.ToString() + "/CD";
        UnityEngine.GameObject go = GetGoByPath(childPath);
        if (go == null)
            return;
        UISprite sp = go.GetComponent<UISprite>();
        if (null != sp)
        {
            if (enable == false)
                sp.fillAmount = 1f;
            else
            {
                sp.fillAmount = 0f;
            }

        }
    }

    public void CastCDByGroup(string group, float cdTime)
    {
        try
        {
            if (cdTime < 0) return;
            int index = GetIndexByGroup(group);
            if (index == -1 || index >= c_SkillNum)
                return;
            skillsCDTime[index] = cdTime;
            string path = "Skill" + index.ToString() + "/skill0/CD";
            UnityEngine.GameObject go = GetGoByPath(path);
            if (null != go)
            {
                UISprite sp = go.GetComponent<UISprite>();
                if (null != sp)
                {
                    sp.fillAmount = 1;
                    remainCdTime[index] = sp.fillAmount;
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private UnityEngine.GameObject GetGoByPath(string path)
    {
        UnityEngine.GameObject go = null;
        if (SkillBarView != null)
        {
            UnityEngine.Transform trans = SkillBarView.transform.Find(path);
            if (null != trans)
            {
                go = trans.gameObject;
            }
            else
            {
                Debug.Log("Can not find " + path);
            }
        }
        return go;
    }
    private UnityEngine.GameObject GetGoByIndexAndName(int index, string name)
    {
        UnityEngine.GameObject go = null;
        if (SkillBarView != null)
        {
            string path = "Skill" + index.ToString() + "/" + name;
            UnityEngine.Transform trans = SkillBarView.transform.Find(path);
            if (trans != null)
            {
                go = trans.gameObject;
            }
        }
        return go;
    }
    public float GetCDTimeByIndex(int index)
    {
        float ret = 0;
        ret = skillsCDTime[index];
        return ret;
    }

    public void UnlockSkill(SkillCategory category, bool isActive, SkillLogicData skillData = null)
    {
        try
        {
            int index = GetIndexByGroup(category);
            if (-1 != index)
            {
                string goPath = "Skill" + index.ToString();
                //NGUIDebug.Log(this.name);
                if (SkillBarView != null)
                {
                    UnityEngine.Transform ts = SkillBarView.transform.Find(goPath);
                    if (null != ts)
                    {
                        UnityEngine.GameObject go = ts.gameObject;
                        if (skillData != null)
                        {
                            ts = go.transform.Find("skill0");
                            if (ts != null)
                            {
                                UISprite sp = ts.gameObject.GetComponent<UISprite>();
                                UIButton btn = ts.GetComponent<UIButton>();
                                UnityEngine.GameObject goAtlas = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.GetSharedResource(skillData.ShowAtlasPath));
                                if (goAtlas != null)
                                {
                                    UIAtlas atlas = goAtlas.GetComponent<UIAtlas>();
                                    if (atlas != null && sp != null)
                                    {
                                        sp.atlas = atlas;
                                    }
                                }
                                else
                                {
                                    Debug.LogError("!!!Load atlas failed.");
                                }
                                if (btn != null && sp != null)
                                {
                                    btn.normalSprite = skillData.ShowIconName;
                                    sp.spriteName = skillData.ShowIconName;
                                }
                            }
                        }
                        if (isActive)
                        {
                            SetActive(isActive);
                        }
                        NGUITools.SetActive(go, isActive);
                    }
                    else
                    {
                        Debug.Log("!!can not find " + goPath);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void HandleCastSkillResult(SkillCannotCastType result)
    {
        try
        {
            string CHN = StrDictionaryProvider.Instance.GetDictString(12);
            switch (result)
            {
                case SkillCannotCastType.kInCD:
                    break;
                case SkillCannotCastType.kCostNotEnough:
                    CHN = StrDictionaryProvider.Instance.GetDictString(13);
                    break;
                case SkillCannotCastType.kUnknow:
                    CHN = StrDictionaryProvider.Instance.GetDictString(14);
                    break;
                default:
                    CHN = StrDictionaryProvider.Instance.GetDictString(14);
                    break;
            }
            if (m_TipsCD <= 0f)
            {
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", CHN, UIScreenTipPosEnum.AlignBottom, UnityEngine.Vector3.zero);
                m_TipsCD = m_TipsDelta;
            }

        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    //更新怒气值
    public void UpdateAngryValue()
    {
        SharedGameObjectInfo share_info = LogicSystem.PlayerSelfInfo;
        if (share_info != null)
        {
            float value = share_info.Rage / share_info.MaxRage;
            float spTempValue = value;
            if (value > 0.9f && value < 1f)
            {
                //让玩家更容易分辨出当前为怒气未满状态
                spTempValue = 0.9f;
            }
            if (spAgeryValueEx != null) spAgeryValueEx.fillAmount = spTempValue;
            //增加一个判断条件：等loading界面结束
            if (value >= 1 && spFullEx != null && UIDataCache.Instance.isLoadingEnd)
            {
                if (NGUITools.GetActive(spAshEx.gameObject) && !UIBeginnerGuideManager.Instance.IsGuideFinished(m_NewbieGuideId) && m_IsInNewbieGuide == false)
                {
                    //第一次释放技能，需要新手引导
                    UIBeginnerGuideManager.Instance.TriggerNewbieGuide(UINewbieGuideTriggerType.T_SkillBar, SkillBarView);
                    m_IsInNewbieGuide = true;
                    m_IsNeedLockGameFrame = true;
                    SetLockFrame(true);
                }
                if (spAgeryValueEx != null) spAgeryValueEx.fillAmount = 1f;
                NGUITools.SetActive(spFullEx.gameObject, true);
                //播放特效
                if (goEffect != null && m_RuntimeEffect == null && spAshEx != null && NGUITools.GetActive(spAshEx.gameObject))
                {
                    m_RuntimeEffect = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.NewObject(goEffect));
                    if (m_RuntimeEffect != null) m_RuntimeEffect.transform.position = spFullEx.transform.position;
                }
            }
            else
            {
                if (spFullEx != null)
                {
                    NGUITools.SetActive(spFullEx.gameObject, false);
                    if (m_RuntimeEffect != null)
                    {
                        Destroy(m_RuntimeEffect);
                        m_RuntimeEffect = null;
                    }
                }
            }
        }
    }
    //
    public void OnExButtonClick()
    {
        SkillCategory skillcategory = SkillCategory.kEx;
        ArkCrossEngine.Data_SceneConfig dsc = ArkCrossEngine.SceneConfigProvider.Instance.GetSceneConfigById(DFMUiRoot.NowSceneID);
        if (dsc != null)
        {
            if (dsc.m_Type == (int)SceneTypeEnum.TYPE_PVP || dsc.m_SubType == (int)SceneSubTypeEnum.TYPE_PVAP)
            {
                skillcategory = SkillCategory.kExPvp;
            }
        }
        GfxModule.Skill.GfxSkillSystem.Instance.PushSkill(ArkCrossEngine.LogicSystem.PlayerSelf, skillcategory, ArkCrossEngine.Vector3.zero);
        if (m_IsInNewbieGuide && m_IsNeedLockGameFrame)
        {
            m_IsNeedLockGameFrame = false;
            SetLockFrame(false);
            StartCoroutine(ShowExGuideDlg());
        }
    }
    //
    private IEnumerator ShowExGuideDlg()
    {
        yield return new WaitForSeconds(m_DelayForExGuideDlg);
        m_IsNeedLockGameFrame = true;
        SetLockFrame(true);
        m_IsInNewbieGuide = false;
        UnityEngine.GameObject go = UIManager.Instance.LoadWindowByName("ExGuideDlg", UICamera.mainCamera);
        UIExGuideDlg guideDlg = go.GetComponent<UIExGuideDlg>();
        if (guideDlg != null)
        {
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            if (role_info != null && role_info.HeroId == (int)UIHeroType.Jianshi)
            {
                string chn = StrDictionaryProvider.Instance.GetDictString(506);
                guideDlg.SetTeachWords(chn);
                guideDlg.SetTweenAlphaDelay(UIHeroType.Jianshi);
            }
            if (role_info != null && role_info.HeroId == (int)UIHeroType.Cike)
            {
                string chn = StrDictionaryProvider.Instance.GetDictString(507);
                guideDlg.SetTeachWords(chn);
                guideDlg.SetTweenAlphaDelay(UIHeroType.Cike);
            }
        }
    }
    private void LockGameFrame()
    {
        if (m_IsNeedLockGameFrame)
        {
            UnityEngine.Time.timeScale = 0f;
        }
        else
        {
            if (UnityEngine.Time.timeScale == 0f) UnityEngine.Time.timeScale = 1f;
        }
    }
    public int GetIndexByGroup(SkillCategory category)
    {
        int ret = -1;
        switch (category)
        {
            case SkillCategory.kSkillA: ret = 0; break;
            case SkillCategory.kSkillB: ret = 1; break;
            case SkillCategory.kSkillC: ret = 2; break;
            case SkillCategory.kSkillD: ret = 3; break;
            default: ret = -1; break;
        }
        return ret;
    }

    public int GetIndexByGroup(string group)
    {
        int ret = -1;
        switch (group)
        {
            case "SkillA": ret = 0; break;
            case "SkillB": ret = 1; break;
            case "SkillC": ret = 2; break;
            case "SkillD": ret = 3; break;
            default: ret = -1; break;
        }
        return ret;
    }
    //当游戏结束时、按钮被隐藏时、需要调用该方法停止普通攻击
    public void StopAttack()
    {
        m_IsPressed = false;
    }
    //PVP模式下先隐藏Ex技能
    public void SetExButtonVisble(bool visible)
    {
        if (spAshEx != null)
            NGUITools.SetActive(spAshEx.gameObject, visible);
    }

    public void SetActive(bool active)
    {
        if (SkillBarView != null && NGUITools.GetActive(SkillBarView) != active)
        {
            NGUITools.SetActive(SkillBarView, active);
            StopAttack();
            if (m_RuntimeEffect != null) Destroy(m_RuntimeEffect);
        }
    }
    public void SetLockFrame(bool isLock)
    {
        float timeScale = isLock ? 0.0f : 1f;
        UnityEngine.Time.timeScale = timeScale;
        m_IsNeedLockGameFrame = isLock;
    }
    //判断是否完成改关
    private bool IsSceneFinished(int sceneId)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            return role_info.SceneInfo.ContainsKey(sceneId);
        }
        return false;
    }
    public float m_TipsCD = 0f;
    public float m_TipsDelta = 0.5f;
    public bool m_IsAttact = false;
    private bool m_IsPressed = false;
    private float time = 0.6f;
    public UnityEngine.AnimationCurve tweenAnimation = null;
    public const float c_DisableScale = 1.2f;
    private const int c_SkillNum = 4;
    private float[] remainCdTime = new float[c_SkillNum];
    private Dictionary<int, UnityEngine.Vector3> m_OriginalPos;
    private float[] skillsCDTime = new float[c_SkillNum];
    private List<UnityEngine.BoxCollider> m_GoList = new List<UnityEngine.BoxCollider>();
    private UnityEngine.Vector3 m_SkillBarPos;
    private SkillCategory[] m_CategoryArray = new SkillCategory[4]{
    SkillCategory.kSkillA,
    SkillCategory.kSkillB,
    SkillCategory.kSkillC,
    SkillCategory.kSkillD
  };
}
