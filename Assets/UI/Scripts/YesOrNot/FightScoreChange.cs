using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ArkCrossEngine;

public class FightScoreChange : UnityEngine.MonoBehaviour
{

    public ArkCrossEngine.GameObject effectGo = null;
    public ArkCrossEngine.GameObject effect = null;

    public UnityEngine.GameObject goZhan = null;
    //public UISprite spFight = null;
    public UILabel lblTotalScore = null;
    public UILabel lblScoreChange = null;
    //public UnityEngine.GameObject ZiPos = null;

    public float stayMaxTime = 0.5f;//变化完后停留的时间
    public float longTime = 1.5f;//长时间，当变化值大于criticalValue时，使用这个变化时间
    public float shortTime = 0.5f;//短时间，当变化值小于criticalValue时，使用这个变化时间
    public float criticalValue = 10;//临界值，用于区分长短时间
    public float numWidth = 10f;//每个数字的宽度，基于其调节位置

    private float stayTime = 2f;
    private int m_RealValue = 0;
    private int m_Interval = 0;
    private float m_MinValue = 0f;
    private bool isUp = true;
    private bool canRun = false;
    private List<object> m_EventList = new List<object>();
    private float valueInterval = 0;
    private UnityEngine.Vector3 initZiPos;
    private UnityEngine.GameObject tempEffect = null;
    private bool isFirst = false;
    private int tempOldScore = 0;
    private int tempNewScore = 0;

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

    // Use this for initialization
    void Awake()
    {
        try
        {
            object obj = null;
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null)
                m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<float, float>("ge_fightscore_change", "lobby", OnScoreChange);
            if (obj != null)
                m_EventList.Add(obj);
            //obj = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("after_levelup", "ui_effect", LevelUPFinish);
            //if (obj != null)
            //  m_EventList.Add(obj);

            //if (ZiPos != null) {
            //  initZiPos = ZiPos.transform.localPosition;
            //}
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void Update()
    {
        try
        {
            if (canRun == false)
            {
                return;
            }
            if (isUp)
            {
                if ((int)m_MinValue <= m_RealValue)
                {
                    UpdateValue((int)m_MinValue);
                    m_MinValue += RealTime.deltaTime / stayTime * m_Interval;
                }
                else
                {
                    m_MinValue = m_RealValue;
                    UpdateValue(m_RealValue);
                    canRun = false;
                    StartCoroutine(AutoClose());
                }
            }
            else
            {
                if ((int)m_MinValue >= m_RealValue)
                {
                    UpdateValue((int)m_MinValue);
                    m_MinValue -= RealTime.deltaTime / stayTime * m_Interval;
                }
                else
                {
                    m_MinValue = m_RealValue;
                    UpdateValue(m_RealValue);
                    canRun = false;
                    StartCoroutine(AutoClose());
                }
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
            if (tempEffect != null)
            {
                NGUITools.SetActive(tempEffect, false);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void UpdateValue(int value)
    {
        //if (lblScoreChange != null) {
        //  if (isUp) {
        //    lblScoreChange.text = "+" + value;
        //  } else {
        //    lblScoreChange.text = "-" + value;
        //  }
        //}
        if (lblTotalScore != null)
        {
            lblTotalScore.text = value.ToString();
        }
    }

    private void OnScoreChange(float oldScore, float newScore)
    {
        tempOldScore = 0;
        tempNewScore = 0;
        if (oldScore <= 0 || isFirst)
        {//刚登陆时为0，从战斗中出来时isfirst=true，也不提示
            isFirst = false;
            return;
        }

        int oldS = (int)oldScore;
        int newS = (int)newScore;
        m_MinValue = oldS;//0f;
        m_RealValue = newS;//UnityEngine.Mathf.Abs(oldS - newS);
        m_Interval = UnityEngine.Mathf.Abs(oldS - newS);
        if (m_Interval <= 0)
        {//可能有战斗力加零点几的情况，强转int后一般大了，就不提示了
            return;
        }

        if (UIManager.Instance.IsWindowVisible("LevelUp"))
        {
            tempOldScore = oldS;
            tempNewScore = newS;
            Invoke("LevelUPFinish", 1.2f);
        }
        else
        {
            Play(oldS, newS);
        }
    }

    private void LevelUPFinish()
    {
        if (tempOldScore > 0 || tempNewScore > 0)
        {
            Play(tempOldScore, tempNewScore);
        }
    }

    private void Play(int oldS, int newS)
    {
        if (lblTotalScore != null)
        {
            lblTotalScore.text = oldS.ToString();
        }
        if (m_RealValue <= criticalValue)
        {
            stayTime = shortTime;
        }
        else
        {
            stayTime = longTime;
        }
        UIManager.Instance.HideWindowByName("FightScoreChange");
        UIManager.Instance.ShowWindowByName("FightScoreChange");
        ResetTweens();
        if (newS > oldS)
        {//提升
         //stayTime = shortTime;
         //m_MinValue = m_RealValue = newS;
            isUp = true;
            if (goZhan != null)
            {
                NGUITools.SetActive(goZhan, true);
            }
            //if (spFight != null) {
            //  spFight.spriteName = "battleup1";
            //}
            if (lblScoreChange != null)
            {
                lblScoreChange.text = "+" + m_Interval;
                lblScoreChange.color = new UnityEngine.Color(0f, (float)251 / 255, (float)75 / 255);
            }
        }
        else
        {//降
            isUp = false;
            if (goZhan != null)
            {
                NGUITools.SetActive(goZhan, false);
            }
            //if (spFight != null) {
            //  spFight.spriteName = "battledown1";
            //}
            if (lblScoreChange != null)
            {
                lblScoreChange.text = "-" + m_Interval;
                lblScoreChange.color = new UnityEngine.Color(1f, 0f, 0f);
            }
        }

        //float numLength = m_RealValue.ToString().Length - 1;
        //if (ZiPos != null) {
        //  ZiPos.transform.localPosition = new UnityEngine.Vector3(initZiPos.x - numLength * numWidth, initZiPos.y, initZiPos.z);
        //}

        PlayEffect();

        StopAllCoroutines();
        canRun = true;
    }

    private void PlayEffect()
    {
        if (tempEffect != null)
        {
            DestroyImmediate(tempEffect);
        }
        if (effect != null && isUp == true)
        {
            UnityEngine.GameObject ef = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.NewObject(effect));
            if (ef != null && effectGo != null)
            {
                tempEffect = ef;
                ef.transform.position = new UnityEngine.Vector3(effectGo.transform.position.x, effectGo.transform.position.y, effectGo.transform.position.z);
                Destroy(ef, stayMaxTime + longTime);
            }
        }
    }

    private void ResetTweens()
    {
        TweenAlpha[] tweenAlphas = gameObject.GetComponentsInChildren<TweenAlpha>();
        TweenPosition[] tweenPositions = gameObject.GetComponentsInChildren<TweenPosition>();
        TweenScale[] tweenScales = gameObject.GetComponentsInChildren<TweenScale>();
        TweenRotation[] tweenRotations = gameObject.GetComponentsInChildren<TweenRotation>();

        if (tweenAlphas != null)
        {
            foreach (TweenAlpha ta in tweenAlphas)
            {
                ta.ResetToBeginning();
                ta.PlayForward();
            }
        }
        if (tweenPositions != null)
        {
            foreach (TweenPosition tp in tweenPositions)
            {
                tp.ResetToBeginning();
                tp.PlayForward();
            }
        }
        if (tweenScales != null)
        {
            foreach (TweenScale ts in tweenScales)
            {
                ts.ResetToBeginning();
                ts.PlayForward();
            }
        }
        if (tweenRotations != null)
        {
            foreach (TweenRotation tr in tweenRotations)
            {
                tr.ResetToBeginning();
                tr.PlayForward();
            }
        }
    }

    private IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(stayMaxTime);
        UIManager.Instance.HideWindowByName("FightScoreChange");
    }
}
