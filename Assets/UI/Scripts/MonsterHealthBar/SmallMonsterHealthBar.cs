using UnityEngine;
using System;
using System.Collections.Generic;

public class SmallMonsterHealthBar : UnityEngine.MonoBehaviour
{
    private bool m_CanCast = false;
    public float countdown = -1f;
    public float waitTime = 3f;
    public int testDelta = 20;
    public int testValue = 1000;
    private UnityEngine.Vector3 positionVec3 = UnityEngine.Vector3.zero;
    public UILabel healthVal = null;
    public UILabel nameValue = null;
    public UILabel healthState = null;
    public UISprite forDel = null;
    public UISprite white = null;
    public UIProgressBar progressBar = null;
    public UnityEngine.Transform healthBarView = null;

    private UnityEngine.Vector3 viewStartPos = UnityEngine.Vector3.zero;
    private UnityEngine.Vector3 viewCurPos = UnityEngine.Vector3.zero;
    private UnityEngine.Vector3 tempVec = UnityEngine.Vector3.zero;
    private UnityEngine.Vector3 outPos = new UnityEngine.Vector3(-1000f, -1000f, -1000f);
    // Use this for initialization
    private List<object> m_EventList = new List<object>();
    void Awake()
    {
        try
        {
            if (healthBarView != null)
            {
                viewStartPos = healthBarView.localPosition;
                viewCurPos = viewStartPos;
            }
            if (null == progressBar)
            {
                Debug.LogError("Can Not Find HealthBar");
            }
            object obj = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, int, int, string>("ge_small_monster_healthbar", "ui", UpdateHealthBar);
            if (null != obj) m_EventList.Add(obj);
            obj = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
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
            SetActive(false);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
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
            if (m_CanCast)
                CastAnimation();
            if (countdown > 0)
            {
                countdown -= RealTime.deltaTime;
                if (countdown <= 0)
                    SetActive(false);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void SetActive(bool isActive)
    {
        /*
		if (healthBarView!=null){
	      NGUITools.SetActive(healthBarView, isActive);
		}*/
        if (isActive)
        {
            ActiveView(healthBarView, viewCurPos);
        }
        else
        {
            ActiveView(healthBarView, outPos);
        }
    }

    private void ActiveView(UnityEngine.Transform tf, UnityEngine.Vector3 pos)
    {
        if (null != tf)
        {
            tf.localPosition = pos;
        }
    }

    void SetHealthValueText(int current, int max)
    {
        /*
        UnityEngine.Transform trans = this.transform.Find("HealthBar/healthValue");
        if (null == trans)
          return;
        UnityEngine.GameObject go = trans.gameObject;
        if (null != go) {
          UILabel label = go.GetComponent<UILabel>();
          if (null != label)
            label.text = current.ToString() + "/" + max.ToString();
        }
        */
        if (null != healthVal)
        {
            SetActive(true);
            healthVal.text = current.ToString() + "/" + max.ToString();
        }
    }

    private void SetName(string name)
    {
        /*
        UnityEngine.Transform trans = transform.Find("name");
        if (trans == null)
          return;
        UnityEngine.GameObject go = trans.gameObject;
        UILabel label = null;
        if (go != null)
          label = go.GetComponent<UILabel>();
        if (null != label) {
          label.text = name.ToString();
        }
        */
        if (null != nameValue)
        {
            nameValue.text = name.ToString();
        }
    }
    void ShakeHealthBar()
    {
        if (healthBarView != null)
        {
            tempVec.x = positionVec3.x + 25f;
            tempVec.y = positionVec3.y + 25f;
            tempVec.z = positionVec3.z;
            //TweenPosition.Begin(healthBarView.gameObject, 0.5f, tempVec, positionVec3);
            viewCurPos = positionVec3;
            /*
	    TweenPosition tweenPosition = healthBarView.GetComponent<TweenPosition>();
	    if (tweenPosition != null)
	      Destroy(tweenPosition);
	    tweenPosition = healthBarView.AddComponent<TweenPosition>();
	    tweenPosition.from = new UnityEngine.Vector3(positionVec3.x + 25, positionVec3.y + 25, positionVec3.z);
	    tweenPosition.to = positionVec3;
	    tweenPosition.duration = 0.5f;
	    */
        }
    }

    void UpdateHealthBar(int curValue, int maxValue, int hpDamage, string name)
    {
        try
        {
            if (healthBarView != null)
            {
                if (UnityEngine.Vector3.zero == positionVec3)
                {
                    positionVec3 = viewStartPos;
                }
                SetName(name);
                SetActive(true);
                ShakeHealthBar();
                SetProgressValue(curValue, maxValue, hpDamage);
                countdown = waitTime;
                if (maxValue <= 0)
                    return;
                SetHealthValueText(curValue, maxValue);
                float value = curValue / (float)maxValue;
                /*
                UIProgressBar progressBar = null;
                progressBar = goHealthBar.GetComponent<UIProgressBar>();
                if (null != progressBar) {
                  progressBar.value = value;
                  TweenSpriteAlpha(goHealthBar);
                  if (value <= 0) {
                    SetLable("Dead");
                  } else {
                    SetLable("x1");
                  }
                }
                */
                if (null != progressBar)
                {
                    progressBar.value = value;
                    TweenSpriteAlpha();
                    if (value <= 0)
                    {
                        SetLable("Dead");
                    }
                    else
                    {
                        SetLable("x1");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void SetProgressValue(int curValue, int maxValue, int damage)
    {
        if (maxValue == 0)
            return;
        //因为damage是负值
        float percent = (curValue - damage) / (float)maxValue;
        /*if (goHealthBar != null) {
          UnityEngine.Transform trans = goHealthBar.transform.Find("white");
          if (trans == null) return;
          UnityEngine.GameObject go = trans.gameObject;
          UISprite sp = go.GetComponent<UISprite>();
          if (sp != null)
            sp.fillAmount = percent;
          trans = goHealthBar.transform.Find("forDel");
          if (trans == null) return;
          go = trans.gameObject;
          sp = go.GetComponent<UISprite>();
          if (sp != null)
            sp.fillAmount = percent;
        }
        */
        if (white != null)
        {
            white.fillAmount = percent;
        }
        if (forDel != null)
        {
            forDel.fillAmount = percent;
        }
    }

    void TweenSpriteAlpha()
    {
        /*UnityEngine.GameObject goBack = null;
        UIProgressBar progressBar = null;
        if (null != father) {
          progressBar = father.GetComponent<UIProgressBar>();
          UnityEngine.Transform trans = father.transform.Find("white");
          if (trans != null)
            goBack = trans.gameObject;

        }
        if (goBack == null)
          return;
        UISprite spBack = null;
        if (null != goBack) {
          spBack = goBack.GetComponent<UISprite>();

        }
        */
        if (null != white && null != progressBar)
        {
            if (white.fillAmount <= progressBar.value)
            {
                white.fillAmount = progressBar.value;
                SetCastFlag(true);
            }
            else
            {

                TweenAlpha tween = white.transform.GetComponent<TweenAlpha>();
                if (null == tween)
                    return;
                tween.enabled = true;
                tween.ResetToBeginning();
                tween.PlayForward();
            }
        }
    }

    void CastAnimation()
    {
        /*UnityEngine.GameObject goBack = null;

        UIProgressBar progress = null;
        if (null != father) {
          UnityEngine.Transform trans = father.transform.Find("forDel");
          if (trans != null)
            goBack = trans.gameObject;
          progress = father.GetComponent<UIProgressBar>();
        }
        */
        if (null != forDel && null != progressBar)
        {
            if (forDel.fillAmount <= progressBar.value)
            {
                forDel.fillAmount = progressBar.value;
                SetCastFlag(false);
            }
            else
            {
                forDel.fillAmount -= RealTime.deltaTime * 0.2f;
            }
        }

    }

    private void SetLable(string state)
    {
        /*string path = "HealthBar/state";
        UnityEngine.Transform trans = this.transform.FindChild(path);
        if (trans == null)
          return;
        UnityEngine.GameObject go = trans.gameObject;
        if (null != go) {
          UILabel label = go.GetComponent<UILabel>();
          if (null != label)
            label.text = state;
        }
        */
        if (null != healthState)
        {
            healthState.text = state;
        }
    }

    public void OnTweenAlphaFinished()
    {
        SetCastFlag(true);
        /*
        UnityEngine.GameObject goBack = null;
        UIProgressBar progressBar = null;
        if (null != goHealthBar) {
          UnityEngine.Transform trans = goHealthBar.transform.Find("white");
          if (trans != null)
            goBack = trans.gameObject;
          progressBar = goHealthBar.GetComponent<UIProgressBar>();
        }
        if (white == null)
          return;

        */
        if (null != white && null != progressBar)
        {
            if (white.fillAmount >= progressBar.value)
            {
                white.fillAmount = progressBar.value;
                //Debug.Log("TweenAlpha is not null!!");
            }
        }

    }

    public void SetCastFlag(bool canCast)
    {
        m_CanCast = canCast;
    }

    //public void TestButton()
    //{
    //  if (progressBar != null) {
    //    ShakeHealthBar();
    //    testValue -= testDelta;
    //    UpdateHealthBar(testValue, 500, -100);
    //  }
    //}
}