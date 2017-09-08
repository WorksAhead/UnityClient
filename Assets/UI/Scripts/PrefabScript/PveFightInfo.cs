using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PveFightInfo : UnityEngine.MonoBehaviour
{
    private List<object> eventlist = new List<object>();
    private bool signtime = false;
    private long StartTime = 0;
    private int countDownTime = 0;
    private int moneyall = 0;
    private int count = 0;
    private long lasttime = 0;
    private long alldeltatime = 0;

    public UILabel timeorsomelabel = null;
    public UISprite scrollus = null;
    public UILabel moneylabel = null;
    public UILabel defenselabel = null;
    public UISprite timeOrSomeSprite = null;
    public UnityEngine.GameObject timeOrSome = null;
    public UnityEngine.GameObject jinbi = null;
    public UnityEngine.GameObject someBack = null;
    public UnityEngine.GameObject pveFightInfo = null;

    public void UnSubscribe()
    {
        try
        {
            if (eventlist != null)
            {
                for (int i = 0; i < eventlist.Count; i++)
                {
                    if (eventlist[i] != null)
                    {
                        ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eventlist[i]);
                    }
                }
                /*
	      foreach (object eo in eventlist) {
	        if (eo != null) {
	          ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eo);
	        }
	      }*/
                eventlist.Clear();
            }
            NGUITools.DestroyImmediate(pveFightInfo);
            //DFMUiRoot.PveFightInfo = null;
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Use this for initialization
    void Start()
    {
        try
        {
            if (eventlist != null) { eventlist.Clear(); }
            object eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (eo != null) eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<long>("ge_send_scenestart_time", "ui", PveSceneStartTime);
            if (eo != null) eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<float, float, float, int>("ge_gain_money", "ui", PlusMoney);
            if (eo != null) eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, int, int, int, int, int, int, bool>("ge_victory_panel", "ui", VictoryEnd);
            if (eo != null) eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_role_dead", "ui", AboutHeroDead);
            if (eo != null) eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_loading_start", "ui", UnSubscribe);
            if (eo != null) eventlist.Add(eo);
            //UnityEngine.Transform tf = transform.Find("jinbi/Label");
            //if (tf != null) {
            //UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (moneylabel != null)
            {
                //moneylabel = ul;
                moneylabel.text = "0";
            }
            //}
            //tf = transform.Find("");

            UpdataFightUIByScene();

            ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_get_scenestart_time", "lobby");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (signtime)
            {
                long time = ArkCrossEngine.TimeUtility.GetServerMilliseconds();
                if ((time - lasttime) > 1000)
                {
                    alldeltatime += (time - lasttime);
                }
                lasttime = time;
                if (timeorsomelabel != null)
                {
                    long residuetime = (long)countDownTime - (ArkCrossEngine.TimeUtility.GetServerMilliseconds() - StartTime - alldeltatime) / 1000;

                    string str1 = (residuetime / 60).ToString();
                    if (str1.Length == 1)
                    {
                        str1 = "0" + str1;
                    }
                    string str2 = (residuetime % 60).ToString();
                    if (str2.Length == 1)
                    {
                        str2 = "0" + str2;
                    }
                    timeorsomelabel.text = str1 + ":" + str2;
                    if (residuetime <= 0)
                    {
                        signtime = false;
                        SetDefeat();
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void SetDefeat()
    {
        /*
        UnityEngine.Transform tf = transform.Find("TimeOrSome/Sprite");
        if (tf != null) {
          UISprite us = tf.gameObject.GetComponent<UISprite>();
          if (us != null) {
            us.spriteName = "tzshibai";
            us.width = 135;
            us.height = 42;
          }
        }
        */
        if (timeOrSomeSprite != null)
        {
            timeOrSomeSprite.spriteName = "tzshibai";
            timeOrSomeSprite.width = 135;
            timeOrSomeSprite.height = 42;
        }
    }
    public void SetInitInfo(int type, int num0, int num1, int num2)
    {
        count++;
        /*
    UnityEngine.Transform tf = transform.Find("TimeOrSome/Sprite");
    UISprite us = null;
    if (tf != null) {
      us = tf.gameObject.GetComponent<UISprite>();
    }
		*/
        switch (type)
        {
            case 0:
                if (timeOrSome != null)
                {
                    NGUITools.SetActive(timeOrSome, true);
                }
                if (timeOrSomeSprite != null)
                {
                    timeOrSomeSprite.spriteName = "zd_bjan";
                }
                //tf = transform.Find("TimeOrSome/Back");
                if (someBack != null)
                {
                    NGUITools.SetActive(someBack, false);
                }
                /*
                tf = transform.Find("TimeOrSome/Label");
                if (tf != null) {
                  UILabel ul = tf.gameObject.GetComponent<UILabel>();
                  if (ul != null) {
                    timeorsomelabel = ul;
                  }
                }
                */
                break;
            case 1:
                if (timeOrSome != null)
                {
                    NGUITools.SetActive(timeOrSome, true);
                }
                if (timeOrSomeSprite != null)
                {
                    timeOrSomeSprite.spriteName = "zd_fsan";
                }
                //tf = transform.Find("TimeOrSome/Label");
                if (timeorsomelabel != null)
                {
                    NGUITools.SetActive(timeorsomelabel.gameObject, false);
                }
                /*
                tf = transform.Find("TimeOrSome/Back/Label");
                if (tf != null) {
                  UILabel ul = tf.gameObject.GetComponent<UILabel>();
                  if (ul != null) {
                    defenselabel = ul;
                  }
                }
                tf = transform.Find("TimeOrSome/Back/Front");
                if (tf != null) {
                  UISprite uss = tf.gameObject.GetComponent<UISprite>();
                  if (uss != null) {
                    scrollus = uss;
                  }
                }
                */
                break;
            case 2:
                if (timeOrSome != null)
                {
                    NGUITools.SetActive(timeOrSome, true);
                }
                if (timeOrSomeSprite != null)
                {
                    timeOrSomeSprite.spriteName = "zd_tzan";
                }
                //tf = transform.Find("TimeOrSome/Back");
                if (someBack != null)
                {
                    NGUITools.SetActive(someBack, false);
                }
                /*
                tf = transform.Find("TimeOrSome/Label");
                if (tf != null) {
                  UILabel ul = tf.gameObject.GetComponent<UILabel>();
                  if (ul != null) {
                    timeorsomelabel = ul;
                  }
                }
                */
                SetTimeCountDownTime(num0);
                break;
            case 3:
                if (timeOrSome != null)
                {
                    NGUITools.SetActive(timeOrSome, true);
                }
                if (count == 2)
                {
                    StartTime = ArkCrossEngine.TimeUtility.GetServerMilliseconds();
                }
                if (timeOrSomeSprite != null)
                {
                    timeOrSomeSprite.spriteName = "zd_txan";
                }
                //tf = transform.Find("TimeOrSome/Back");
                if (someBack != null)
                {
                    NGUITools.SetActive(someBack, false);
                }
                /*
                tf = transform.Find("TimeOrSome/Label");
                if (tf != null) {
                  UILabel ul = tf.gameObject.GetComponent<UILabel>();
                  if (ul != null) {
                    timeorsomelabel = ul;
                  }
                }
                */
                SetTimeCountDownTime(num0);
                break;
            case 4:
                //tf = transform.Find("TimeOrSome");
                if (timeOrSome != null)
                {
                    NGUITools.SetActive(timeOrSome, false);
                }

                break;
        }

        SetUpdateInfo(type, num0, num1, num2);
    }
    public void SetUpdateInfo(int type, int num0, int num1, int num2)
    {
        switch (type)
        {
            case 0:
                if (num0 > num1) { num0 = num1; }
                if (timeorsomelabel != null)
                {
                    timeorsomelabel.text = num0.ToString() + "/" + num1;
                }
                if (num0 == num1)
                {
                    SetDefeat();
                }
                break;
            case 1:
                if (num1 > num2) { num1 = num2; }
                if (defenselabel != null)
                {
                    defenselabel.text = ArkCrossEngine.StrDictionaryProvider.Instance.Format(307, num0);
                }
                if (scrollus != null)
                {
                    scrollus.fillAmount = (float)(num1 * 1.0f / num2);
                }

                break;
            case 2:
                if (!signtime)
                {
                    SetTimeCountDownTime(num0);
                }
                break;
            case 3:
                if (!signtime)
                {
                    SetTimeCountDownTime(num0);
                }
                break;
        }
    }
    private void SetTimeCountDownTime(int time)
    {
        countDownTime = time;
        signtime = true;
        lasttime = ArkCrossEngine.TimeUtility.GetServerMilliseconds();
    }
    private void PveSceneStartTime(long starttime)
    {
        try
        {
            StartTime = starttime;
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void PlusMoney(float x, float y, float z, int moneynum)
    {
        try
        {
            moneyall += moneynum;
            if (moneylabel != null)
            {
                moneylabel.text = moneyall.ToString();
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void VictoryEnd(int one, int two, int three, int four, int five, int six, int seven, bool nine)
    {
        try
        {
            UnSubscribe();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void AboutHeroDead()
    {
        try
        {
            if (null != pveFightInfo)
            {
                SetActive(!NGUITools.GetActive(pveFightInfo));
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void SetActive(bool active)
    {
        if (null != pveFightInfo)
        {
            NGUITools.SetActive(pveFightInfo, active);
        }
    }

    private void UpdataFightUIByScene()
    {
        int sceneid = ArkCrossEngine.WorldSystem.Instance.GetCurSceneId();
        ArkCrossEngine.Data_SceneConfig dsc = ArkCrossEngine.SceneConfigProvider.Instance.GetSceneConfigById(sceneid);
        if (dsc.m_SubType == (int)ArkCrossEngine.SceneSubTypeEnum.TYPE_ATTEMPT)
        {
            UpdateFightUI(UISceneType.MultiPveScene);
        }
        if (dsc.m_SubType == (int)ArkCrossEngine.SceneSubTypeEnum.TYPE_GOLD)
        {
            UpdateFightUI(UISceneType.JinBiScene);
        }
        if (dsc.m_SubType == (int)ArkCrossEngine.SceneSubTypeEnum.TYPE_EXPEDITION)
        {
            UpdateFightUI(UISceneType.TreasureScene);
        }
        if (dsc.m_Type == (int)ArkCrossEngine.SceneTypeEnum.TYPE_PVP)
        {
            UpdateFightUI(UISceneType.PvpScene);
        }
    }

    public void UpdateFightUI(UISceneType _UISceneType)
    {
        switch (_UISceneType)
        {
            case UISceneType.MultiPveScene:
            case UISceneType.PvpScene:
                if (timeOrSome != null)
                {
                    NGUITools.SetActive(timeOrSome, false);
                }
                if (jinbi != null)
                {
                    NGUITools.SetActive(jinbi, false);
                }
                break;
            case UISceneType.JinBiScene:
            case UISceneType.TreasureScene:
                if (timeOrSome != null)
                {
                    NGUITools.SetActive(timeOrSome, false);
                }
                break;

        }
    }
}