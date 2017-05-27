using System;
using System.Collections.Generic;
using ArkCrossEngine;
public class MarsVictory : UnityEngine.MonoBehaviour
{
    public UISprite win = null;
    public UISprite lose = null;
    private List<object> eventlist = new List<object>();
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
            }
            eventlist.Clear();
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
            if (eventlist != null) { eventlist.Clear(); }
            object eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (eo != null) eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, int, int, int, int, int, int, int, int, int, string>("ge_pvp_result", "ui", PvpResult);
            if (eo != null) eventlist.Add(eo);

            //UIManager.Instance.HideWindowByName("MarsVictory");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ReturnMainCity()
    {
        UIManager.Instance.HideWindowByName("MarsVictory");
        ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_return_maincity", "lobby");
    }

    void PvpResult(int result, int enemyheroid, int oldelo, int elo, int enemyoldelo, int enemyelo, int damage, int enemydamage, int maxhitcount, int enemyhitcount, string enemynickname)
    {
        try
        {
            if (JoyStickInputProvider.JoyStickEnable)
            {
                JoyStickInputProvider.JoyStickEnable = false;
            }
            UIManager.Instance.ShowWindowByName("MarsVictory");
            if (result == 0)
            {
                if (win != null)
                {
                    NGUITools.SetActive(win.gameObject, true);
                    NGUITools.SetActive(lose.gameObject, false);
                }
            }
            else
            {
                if (lose != null)
                {
                    NGUITools.SetActive(win.gameObject, false);
                    NGUITools.SetActive(lose.gameObject, true);
                }
            }
            ArkCrossEngine.RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
            UnityEngine.Transform tf;
            if (ri != null)
            {
                tf = transform.Find("sp_headMeBottom/lb_nameMe");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        ul.text = ri.Nickname;
                    }
                }
                tf = transform.Find("sp_headMeBottom/sp_headMe");
                if (tf != null)
                {
                    UISprite us = tf.gameObject.GetComponent<UISprite>();
                    if (us != null)
                    {
                        Data_PlayerConfig cg = PlayerConfigProvider.Instance.GetPlayerConfigById(ri.HeroId);
                        us.spriteName = cg.m_Portrait;
                    }
                }
            }
            tf = transform.Find("sp_headYouBottom/sp_headYou");
            if (tf != null)
            {
                UISprite us = tf.gameObject.GetComponent<UISprite>();
                if (us != null)
                {
                    Data_PlayerConfig cg = PlayerConfigProvider.Instance.GetPlayerConfigById(enemyheroid);
                    us.spriteName = cg.m_Portrait;
                }
            }
            tf = transform.Find("sp_headYouBottom/lb_nameMe");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = enemynickname;
                }
            }
            tf = transform.Find("go_mainRect/go_jifen/lb_jifenMe");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = oldelo.ToString();
                }
            }
            tf = transform.Find("go_mainRect/go_jifen/lb_jifenMeJia");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = (elo - oldelo) >= 0 ? "+" + (elo - oldelo) : (elo - oldelo).ToString();
                }
            }
            tf = transform.Find("go_mainRect/go_jifen/lb_jifenYou");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = enemyoldelo.ToString();
                }
            }
            tf = transform.Find("go_mainRect/go_jifen/lb_jifenYouJia");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = (enemyelo - enemyoldelo) >= 0 ? "+" + (enemyelo - enemyoldelo) : (enemyelo - enemyoldelo).ToString();
                }
            }
            tf = transform.Find("go_mainRect/go_hurt/lb_hurtMe");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = damage.ToString();
                }
            }
            tf = transform.Find("go_mainRect/go_hurt/lb_hurtYou");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = enemydamage.ToString();
                }
            }
            tf = transform.Find("go_mainRect/go_lianji/lb_lianjiMe");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = maxhitcount.ToString();
                }
            }
            tf = transform.Find("go_mainRect/go_lianji/lb_lianjiYou");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = enemyhitcount.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}
