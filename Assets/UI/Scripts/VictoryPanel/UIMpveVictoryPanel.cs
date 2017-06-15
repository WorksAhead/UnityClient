using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class UIMpveVictoryPanel : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject goEffectInfo = null;
    public UnityEngine.GameObject goEffectTitle = null;
    private List<UnityEngine.GameObject> m_RunTimeEffectList = new List<UnityEngine.GameObject>();
    public UnityEngine.GameObject goTitle = null;//特效goEffectTitle播在该UnityEngine.GameObject上
    public UnityEngine.GameObject goItemLabel = null;//prefab
    public UIGrid gridForInfo = null;//用于组织击杀数、战斗用时等信息
    public UnityEngine.GameObject goForShiLian = null;

    public UILabel lblBest = null;
    public UILabel lblCurrent = null;

    public UIProgressBar progressBar = null;//经验条
    public float m_DeltaForInfo = 0.6f;//每隔0.6s显示一条信息
    private float m_CountDown = 0.0f;
    private int m_CountForInfo = 0;//已经显示信息的条数
    private int m_FinishedCount = 0;//完成Item的次数

    private bool m_CanProgress = false;
    private List<object> m_EventList = new List<object>();
    public int OffsetV = -60;

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
        object obj = null;
        obj = LogicSystem.EventChannelForGfx.Subscribe<int, int, int, int>("ge_mpve_tollgate_succeed", "ui", InitVictoryPanel);
        if (obj != null)
            m_EventList.Add(obj);
        obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (obj != null)
            m_EventList.Add(obj);
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
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
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
            //UIManager.Instance.HideWindowByName("MpveCombatFail");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
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

            if (m_CanProgress)
            {
                m_CanProgress = false;
                //StartCoroutine(SetPlayerExp(m_PlayerExp - m_DropExp, m_PlayerExp));
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
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
    private void InitVictoryPanel(int maxHit, int beHittTimes, int diedTimes, int time)
    {
        try
        {
            if (!WorldSystem.Instance.IsAttemptScene() && !WorldSystem.Instance.IsGoldScene())
            {
                return;
            }
            values[0] = maxHit;
            values[1] = beHittTimes;
            values[2] = diedTimes;
            values[3] = time;

            UnityEngine.GameObject progressgo = UIManager.Instance.GetWindowGoByName("MPVEprogress");
            if (progressgo != null)
            {
                MPVEprogress script = progressgo.GetComponent<MPVEprogress>();
                if (script != null)
                {
                    if (lblBest != null)
                    {
                        int best = script.best > script.current ? script.best : script.current;
                        lblBest.text = best + "/" + script.max;
                    }
                    if (lblCurrent != null)
                    {
                        lblCurrent.text = script.current + "/" + script.max;
                    }
                    if (progressBar != null)
                    {
                        progressBar.value = (float)script.current / script.max;
                    }
                }
            }
            UIManager.Instance.ShowWindowByName("MpveVictoryPanel");
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    //战斗胜利后显示击杀数等
    public void ShowCombatIntoItem(int index)
    {
        if (index >= values.Length)
            return;
        if (goItemLabel != null && gridForInfo != null)
        {
            UnityEngine.GameObject item = NGUITools.AddChild(gridForInfo.gameObject, goItemLabel);
            if (item == null)
                return;
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
        if (goForShiLian != null)
        {
            UnityEngine.Transform tsAward = goForShiLian.transform;
            UnityEngine.Vector3 pos = tsAward.localPosition;
            UnityEngine.Vector3 targetPos = new UnityEngine.Vector3(pos.x + 426, pos.y, 0);
            TweenPosition tweenPos = TweenPosition.Begin(tsAward.gameObject, 0.3f, targetPos);
            if (tweenPos != null)
                EventDelegate.Add(tweenPos.onFinished, OnLeft2RightFinished);
        }
    }
    //左--右移动结束
    public void OnLeft2RightFinished()
    {
        //m_CanProgress = true;
        if (goForShiLian != null)
        {
            TweenPosition tweenPos = goForShiLian.GetComponent<TweenPosition>();
            if (tweenPos != null)
            {
                Destroy(tweenPos);
            }
        }
        canClick = true;
        StartCoroutine(ReturnToMainCity());
    }

    public void ClickToMainCity()
    {
        if (canClick)
        {
            //UIManager.Instance.HideWindowByName("MpveVictoryPanel");
            LogicSystem.PublishLogicEvent("ge_quit_battle", "lobby", true);
        }
    }

    private IEnumerator ReturnToMainCity()
    {
        yield return new WaitForSeconds(5f);
        //UIManager.Instance.HideWindowByName("MpveVictoryPanel");
        LogicSystem.PublishLogicEvent("ge_quit_battle", "lobby", false);
    }

    private bool canClick = false;
    private const int c_InfoItemNum = 4;
    private string[] nameArr = new string[c_InfoItemNum]{
    "最高连击",
    "被击次数",
    "死亡次数",
    "通关时间"
  };
    private int[] values = new int[c_InfoItemNum];
}
