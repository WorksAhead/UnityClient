using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class UIVictoryPanel : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject goEffectStar = null;
    public UnityEngine.GameObject goEffectInfo = null;
    public UnityEngine.GameObject goEffectTitle = null;
    private List<UnityEngine.GameObject> m_RunTimeEffectList = new List<UnityEngine.GameObject>();
    public UnityEngine.GameObject goTitle = null;//特效goEffectTitle播在该UnityEngine.GameObject上
    public UISprite[] spStarsArr = new UISprite[3];
    public UnityEngine.GameObject goItemLabel = null;//prefab
    public UIGrid gridForInfo = null;//用于组织击杀数、战斗用时等信息
    public UnityEngine.GameObject goForAward = null;//用于组织奖励信息
    public UnityEngine.GameObject goMasterAward = null;
    public UILabel lblPlayerCurrentExp = null;
    public UILabel lblPlayerLevel = null;
    public UILabel lblAwardMoney = null;//获得的奖励
    public UILabel lblAwardExp = null;
    public UILabel lblMasterHint = null;
    public UILabel lblFirstComplete = null;
    public UnityEngine.GameObject texAward = null;
    public UIProgressBar progressBar = null;//经验条
    public float m_DeltaForInfo = 0.6f;//每隔0.6s显示一条信息
    private float m_CountDown = 0.0f;
    private int m_CountForInfo = 0;//已经显示信息的条数
    private int m_FinishedCount = 0;//完成Item的次数
    private bool m_CanPlayStar = false;//是否开始播放星星动画
    private int m_StarCount = 0;//显示几个星星
    public UnityEngine.AnimationCurve CurveForStar;//星星动画曲线
    public UnityEngine.Vector3 ScaleOfToForStar = new UnityEngine.Vector3(1.2f, 1.2f, 1);
    public float DurationForStar = 1.0f;//每个星星动画的播放时间
    public float DeltaForStar = 0.4f;//两个星星播放的时间间隔
    private int m_StarFinishedCount = 0;

    private bool m_CanProgress = false;
    private List<object> m_EventList = new List<object>();
    private int m_PlayerCurrentLevel = 1;
    private int m_PlayerExp = 0;
    private int m_DropExp = 0;
    private int m_SceneId = -1;
    //角色最大等级写死了。。。。
    private const int m_MaxLevel = 60;
    public int OffsetV = -60;
    public int TransOffsetInfoItem = 400;
    public int TransOffsetStar = 300;
    public int TransOffsetAward = 273;

    private bool m_IsFirstComplete = true;
    private SceneSubTypeEnum subType = SceneSubTypeEnum.TYPE_ELITE;
    //public 
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
            obj = LogicSystem.EventChannelForGfx.Subscribe<int, int, int, int, int, int, int, bool>("ge_victory_panel", "ui", InitVictoryPanel);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null) m_EventList.Add(obj);
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
            UnityEngine.GameObject runtimeEffect = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.NewObject(goEffectTitle));
            if (runtimeEffect != null && goTitle != null)
            {
                runtimeEffect.transform.position = goTitle.transform.position;
                m_RunTimeEffectList.Add(runtimeEffect);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OnEnable()
    {
        try
        {
            //结束画面开始前结束普通攻击
            UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
            if (go != null)
            {
                SkillBar sBar = go.GetComponent<SkillBar>();
                if (sBar != null)
                {
                    sBar.StopAttack();
                    if (ArkCrossEngine.LogicSystem.PlayerSelf != null)
                        GfxModule.Skill.GfxSkillSystem.Instance.StopAttack(ArkCrossEngine.LogicSystem.PlayerSelf);
                }
            }
            /*
            UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("SkillBar");
            if (go != null) {
              SkillBar sBar = go.GetComponent<SkillBar>();
              if (sBar != null) {
                sBar.StopAttack();
                if (ArkCrossEngine.LogicSystem.PlayerSelf != null)
                  GfxModule.Skill.GfxSkillSystem.Instance.StopAttack(ArkCrossEngine.LogicSystem.PlayerSelf);
              }
            }*/
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
            if (m_CountForInfo < 4)
            {
                if (m_CountDown <= 0.0f)
                {
                    ShowCombatIntoItem(m_CountForInfo++);
                    m_CountDown = m_DeltaForInfo;
                }
                else
                {
                    m_CountDown -= RealTime.deltaTime;
                }
            }
            if (m_CanPlayStar)
            {
                StartCoroutine(ShowStars(DeltaForStar));
                m_CanPlayStar = false;
            }
            if (m_CanProgress)
            {
                m_CanProgress = false;
                StartCoroutine(SetPlayerExp(m_PlayerExp - m_DropExp, m_PlayerExp));
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OnDisable()
    {
        try
        {
            for (int i = 0; i < m_RunTimeEffectList.Count; ++i)
                if (m_RunTimeEffectList[i] != null)
                {
                    Destroy(m_RunTimeEffectList[i]);
                }
            m_RunTimeEffectList.Clear();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void InitVictoryPanel(int sceneId, int maxHit, int beHittTimes, int diedTimes, int time, int exp, int gold, bool isFirstComplete)
    {
        try
        {
            if (texAward != null) NGUITools.SetActive(texAward.gameObject, false);
            m_IsFirstComplete = isFirstComplete;
            m_SceneId = sceneId;
            if (lblAwardExp != null) lblAwardExp.text = "+" + exp.ToString();
            if (lblAwardMoney != null) lblAwardMoney.text = "+" + gold.ToString();
            values[0] = maxHit;
            values[1] = beHittTimes;
            values[2] = diedTimes;
            values[3] = time;
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            if (role_info != null)
            {
                m_PlayerCurrentLevel = role_info.Level;
                m_PlayerExp = role_info.Exp;
                Data_SceneDropOut dropCfg = null;
                Data_SceneConfig sceneCfg = SceneConfigProvider.Instance.GetSceneConfigById(sceneId);
                if (sceneCfg != null)
                {
                    if (sceneCfg.m_SubType == (int)SceneSubTypeEnum.TYPE_ELITE)
                        subType = SceneSubTypeEnum.TYPE_ELITE;
                    if (sceneCfg.m_SubType == (int)SceneSubTypeEnum.TYPE_STORY)
                        subType = SceneSubTypeEnum.TYPE_STORY;
                    dropCfg = SceneConfigProvider.Instance.GetSceneDropOutById(sceneCfg.m_DropId);
                }
                if (dropCfg != null) m_DropExp = dropCfg.m_Exp;
            }
            UIManager.Instance.ShowWindowByName("VictoryPanel");
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //服务器已经算完了等级，所以要算出升级之前的级数，用于UI表现
    private int GetLevelByExp(int curExp, int addedExp)
    {
        int lastExp = curExp - addedExp;
        int level = 1;
        while (level <= m_MaxLevel)
        {
            PlayerLevelupExpConfig expCfg = PlayerConfigProvider.Instance.GetPlayerLevelupExpConfigById(level);
            if (expCfg != null && expCfg.m_ConsumeExp > lastExp)
            {
                return level;
            }
            level++;
        }
        return m_MaxLevel;
    }
    //战斗胜利后显示击杀数等
    public void ShowCombatIntoItem(int index)
    {
        if (index >= values.Length) return;
        if (goItemLabel != null && gridForInfo != null)
        {
            UnityEngine.GameObject item = NGUITools.AddChild(gridForInfo.gameObject, goItemLabel);
            if (item == null) return;
            //给item定个初始位置，用于播放特效
            item.transform.localPosition = new UnityEngine.Vector3(0 + 50, OffsetV * index, 0);
            UIVictoryItem lbl = item.GetComponent<UIVictoryItem>();
            if (lbl != null)
            {
                //共四条属性
                if (index == 3)
                    lbl.SetValue(values[index], UIItemType.ForTime);
                else
                    lbl.SetValue(values[index], UIItemType.Common);
                lbl.SetItemName(nameArr[index]);
            }
            TweenPosition tweenPos = item.GetComponent<TweenPosition>();
            if (tweenPos != null)
            {
                //-182是itemlabel的长度、
                tweenPos.from = new UnityEngine.Vector3(-422, OffsetV * index, 0);
                tweenPos.to = new UnityEngine.Vector3(0, OffsetV * index, 0);
                tweenPos.enabled = true;
            }
            UnityEngine.GameObject runtimeEffect = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.NewObject(goEffectInfo));
            NGUITools.AddChild(item.gameObject, runtimeEffect);
            if (runtimeEffect != null)
            {
                runtimeEffect.transform.position = item.transform.position;
            }
            EventDelegate.Add(tweenPos.onFinished, OnShowInfoFinished);
        }
    }
    public void OnShowInfoFinished()
    {
        m_FinishedCount++;
        if (m_FinishedCount == c_InfoItemNum)
        {
            TransLeftToRight();
            m_FinishedCount = 0;
        }
    }
    //从左向右移动Award
    public void TransLeftToRight()
    {
        if (goForAward != null)
        {
            if (lblPlayerCurrentExp != null) NGUITools.SetActive(lblPlayerCurrentExp.gameObject, false);
            UnityEngine.Transform tsAward = goForAward.transform;
            UnityEngine.Vector3 pos = tsAward.localPosition;
            UnityEngine.Vector3 targetPos = new UnityEngine.Vector3(pos.x + 426, pos.y, 0);
            TweenPosition tweenPos = TweenPosition.Begin(tsAward.gameObject, 0.3f, targetPos);
            if (tweenPos != null) EventDelegate.Add(tweenPos.onFinished, OnLeft2RightFinished);
        }
    }
    //左--右移动结束
    public void OnLeft2RightFinished()
    {
        if (goForAward != null)
        {
            TweenPosition tweenPos = goForAward.GetComponent<TweenPosition>();
            if (tweenPos != null) Destroy(tweenPos);
        }
        m_CanProgress = true;
    }

    //显示小星星=、=
    public IEnumerator ShowStars(float delta)
    {
        for (int index = 0; index < m_StarCount; ++index)
        {
            if (index < spStarsArr.Length && spStarsArr[index] != null)
            {
                UISprite spStar = spStarsArr[index];
                spStar.spriteName = "da-xing-xing1";
                UnityEngine.GameObject go = spStar.gameObject;
                TweenScale scale = go.AddComponent<TweenScale>();
                scale.to = ScaleOfToForStar;
                scale.duration = DurationForStar;
                scale.animationCurve = CurveForStar;
                EventDelegate.Add(scale.onFinished, OnShowStarFinished);
                yield return new WaitForSeconds(delta);
            }
        }
        yield return new WaitForSeconds(0f);
    }
    public void OnShowStarFinished()
    {
        UnityEngine.GameObject runtimeEffect = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.NewObject(goEffectStar));
        if (runtimeEffect != null)
        {
            m_RunTimeEffectList.Add(runtimeEffect);
            if (m_StarFinishedCount < spStarsArr.Length && spStarsArr[m_StarFinishedCount] != null)
                runtimeEffect.transform.position = spStarsArr[m_StarFinishedCount].transform.position;
        }
        if (++m_StarFinishedCount == m_StarCount)
        {
            StartCoroutine(ShowMasterAward());
        }
    }
    public IEnumerator ShowMasterAward()
    {
        yield return new WaitForSeconds(0.2f);
        if (m_IsFirstComplete)
        {
            if (lblMasterHint != null) NGUITools.SetActive(lblMasterHint.gameObject, true);
            if (lblFirstComplete != null) NGUITools.SetActive(lblFirstComplete.gameObject, false);
            SetAwardItem();
        }
        yield return new WaitForSeconds(1.5f);
        try
        {
            UIManager.Instance.HideWindowByName("VictoryPanel");
            UIManager.Instance.ShowWindowByName("CombatWin");
            UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("CombatWin");
            if (null != go)
            {
                go.GetComponent<CombatWin>().TitleEffect();
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //UI上移
    public void MoveChildGameObject()
    {
        SetAwardVisible(false);
        //UnityEngine.Transform tsStar = this.transform.Find("ScrollView/Stars");
        UnityEngine.Transform tsAward = this.transform.Find("ScrollView/Award");
        UnityEngine.Transform tsCombatInfo = this.transform.Find("ScrollView/CombatInfo");
        if (tsAward == null || tsCombatInfo == null)
        {
            Debug.Log("Something is null in UI.");
        }
        else
        {
            UnityEngine.Vector3 pos = tsCombatInfo.localPosition;
            UnityEngine.Vector3 targetPos = new UnityEngine.Vector3(pos.x, pos.y + TransOffsetInfoItem, 0);
            TweenPosition.Begin(tsCombatInfo.gameObject, 0.3f, targetPos);
            //pos = tsStar.localPosition;
            //targetPos = new UnityEngine.Vector3(pos.x, pos.y + TransOffsetStar, 0);
            //TweenPosition.Begin(tsStar.gameObject, 0.3f, targetPos);
            //NGUITools.SetActive(tsAward.gameObject, true);
            pos = tsAward.localPosition;
            targetPos = new UnityEngine.Vector3(pos.x, pos.y + TransOffsetAward, 0);
            TweenPosition tweenPos = TweenPosition.Begin(tsAward.gameObject, 0.3f, targetPos);
            if (tweenPos != null)
                EventDelegate.Add(tweenPos.onFinished, OnMoveFinished);
        }
    }
    //上移结束
    public void OnMoveFinished()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            if (role_info.SceneInfo != null && role_info.SceneInfo.ContainsKey(m_SceneId))
                m_StarCount = role_info.SceneInfo[m_SceneId];//星级
        }
        if (!m_IsFirstComplete)
        {
            for (int i = 0; i < spStarsArr.Length; ++i)
            {
                if (spStarsArr[i] != null)
                    NGUITools.SetActive(spStarsArr[i].gameObject, true);
            }
            m_CanPlayStar = true;
        }
        else
        {
            //星级奖励
            if (subType == SceneSubTypeEnum.TYPE_ELITE)
            {
                for (int i = 0; i < spStarsArr.Length; ++i)
                {
                    if (spStarsArr[i] != null)
                        NGUITools.SetActive(spStarsArr[i].gameObject, true);
                }
                m_CanPlayStar = true;
            }
            else
            {
                //剧情首次通关奖励
                if (lblMasterHint != null) NGUITools.SetActive(lblMasterHint.gameObject, false);
                if (lblFirstComplete != null) NGUITools.SetActive(lblFirstComplete.gameObject, true);
                SetAwardItem();
                StartCoroutine(ReturnToMainCity());
            }
        }
        //todo：星级如何确定？
    }
    public void SetPlayerLevel(int level)
    {
        if (lblPlayerLevel != null)
        {
            lblPlayerLevel.text = "Lv." + level.ToString();
        }
    }
    //经验值动画
    public IEnumerator SetPlayerExp(int fromExp, int toExp)
    {
        if (lblPlayerCurrentExp != null) NGUITools.SetActive(lblPlayerCurrentExp.gameObject, true);
        int startLevel = GetLevelByExp(m_PlayerExp, m_DropExp);
        SetPlayerLevel(startLevel);
        if (startLevel == m_MaxLevel)
        {
            string str_des = StrDictionaryProvider.Instance.GetDictString(309);
            if (lblPlayerCurrentExp != null) lblPlayerCurrentExp.text = str_des;
        }
        //if (maxExp <= 0) yield break;
        int baseExp = GetTotleExpByLevel(startLevel - 1);
        int maxExp = GetTotleExpByLevel(startLevel) - baseExp;
        for (int exp = fromExp; exp <= toExp;)
        {
            int level = GetLevelByExp(exp, 0);
            if (level > startLevel)
            {
                startLevel = level;
                SetPlayerLevel(level);
                baseExp = GetTotleExpByLevel(level - 1);
                maxExp = GetLevelUpExpById(level);
            }
            if (maxExp > 0)
            {
                if (lblPlayerCurrentExp != null) lblPlayerCurrentExp.text = (exp - baseExp) + "/" + maxExp;
                if (progressBar != null && maxExp != 0) progressBar.value = (exp - baseExp) / (float)maxExp;
            }
            //以20贞为基准,保证最长时间为5s
            if (maxExp > 100)
            {
                exp += maxExp / 100;
            }
            else
            {
                exp++;
            }
            yield return new WaitForSeconds(0.0001f);
        }
        if (startLevel < m_PlayerCurrentLevel)
        {
            SetPlayerLevel(m_PlayerCurrentLevel);
            baseExp = GetTotleExpByLevel(m_PlayerCurrentLevel - 1);
            maxExp = GetTotleExpByLevel(m_PlayerCurrentLevel) - baseExp;
            if (maxExp > 0)
            {
                if (lblPlayerCurrentExp != null) lblPlayerCurrentExp.text = (toExp - baseExp) + "/" + maxExp;
                if (progressBar != null && maxExp != 0) progressBar.value = (toExp - baseExp) / (float)maxExp;
            }
        }
        if (m_IsFirstComplete == true || subType == SceneSubTypeEnum.TYPE_ELITE)
        {
            yield return new WaitForSeconds(1f);
            m_CanProgress = false;
            MoveChildGameObject();
        }
        else
        {
            yield return new WaitForSeconds(1f);
            UIManager.Instance.HideWindowByName("VictoryPanel");
            UIManager.Instance.ShowWindowByName("CombatWin");
            UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("CombatWin");
            if (null != go)
            {
                go.GetComponent<CombatWin>().TitleEffect();
            }
        }
    }
    private int GetLevelUpExpById(int level)
    {
        PlayerLevelupExpConfig expCfgHigh = PlayerConfigProvider.Instance.GetPlayerLevelupExpConfigById(level);
        if (level == 1 && expCfgHigh != null)
            return expCfgHigh.m_ConsumeExp;
        PlayerLevelupExpConfig expCfgLow = PlayerConfigProvider.Instance.GetPlayerLevelupExpConfigById(level - 1);
        if (expCfgHigh != null && expCfgLow != null)
        {
            return expCfgHigh.m_ConsumeExp - expCfgLow.m_ConsumeExp;
        }
        return 0;
    }
    private int GetTotleExpByLevel(int level)
    {
        if (level == 0) return 0;
        int exp = 0;
        PlayerLevelupExpConfig expCfg = PlayerConfigProvider.Instance.GetPlayerLevelupExpConfigById(level);
        if (expCfg != null) exp = expCfg.m_ConsumeExp;
        return exp;
    }
    private void SetAwardVisible(bool visible)
    {
        if (goMasterAward != null)
            NGUITools.SetActive(goMasterAward, visible);
        for (int i = 0; i < spStarsArr.Length; ++i)
        {
            if (spStarsArr[i] != null)
                NGUITools.SetActive(spStarsArr[i].gameObject, visible);
        }
    }
    private void SetAwardItem()
    {
        try
        {
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            if (role_info != null)
            {
                if (role_info.SceneInfo != null && role_info.SceneInfo.ContainsKey(m_SceneId))
                {
                    int level = role_info.SceneInfo[m_SceneId];//星级
                    Data_SceneConfig sceneCfg = SceneConfigProvider.Instance.GetSceneConfigById(m_SceneId);
                    if (sceneCfg == null) return;
                    int dropId = sceneCfg.GetCompletedRewardId(level);
                    Data_SceneDropOut dropCfg = SceneConfigProvider.Instance.GetSceneDropOutById(dropId);
                    if (dropCfg != null)
                    {
                        List<int> rewardItemList = dropCfg.GetRewardItemByHeroId(role_info.HeroId);
                        if (null != rewardItemList && rewardItemList.Count > 0)
                        {
                            int itemId = rewardItemList[0];
                            DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Victory, texAward, itemId, dropCfg.m_ItemCountList[0]);
                        }
                    }
                }
            }
            if (goMasterAward != null) NGUITools.SetActive(goMasterAward, true);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private IEnumerator ReturnToMainCity()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.HideWindowByName("VictoryPanel");
        UIManager.Instance.ShowWindowByName("CombatWin");
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("CombatWin");
        if (null != go)
        {
            go.GetComponent<CombatWin>().TitleEffect();
        }
    }
    private const int c_InfoItemNum = 4;
    private string[] nameArr = new string[c_InfoItemNum]{
    "最高连击",
    "被击次数",
    "死亡次数",
    "通关时间"
  };
    private int[] values = new int[c_InfoItemNum];
}
public enum UIItemType
{
    Common = 0,
    ForTime = 1,
}
