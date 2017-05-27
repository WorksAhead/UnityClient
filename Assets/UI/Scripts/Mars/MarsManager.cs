using ArkCrossEngine;
using System;
using System.Collections.Generic;

public class MarsManager : UnityEngine.MonoBehaviour
{
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
            golist.Clear();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
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
            //eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<List<GowDataForMsg>>("ge_sync_gowstar_list", "gowstar", SyncMars);
            //if (eo != null) eventlist.Add(eo);
            //eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_update_role_dynamic_property", "ui", UpdateDynamicProperty);
            //if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<ArkCrossEngine.GowMatchResult>("ge_pvpmatch_result", "gow", PvpMatchResult);
            if (eo != null) { eventlist.Add(eo); }

            ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_get_gowstar_list", "lobby", 0, 100);
            SetStaticProperty();
            UIManager.Instance.HideWindowByName("Mars");
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
            SetAwardProperty();
            //ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_get_gowstar_list", "lobby", 0, 100);
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
            if (ismatching)
            {
                time += RealTime.deltaTime;

                if (timelabel != null)
                {
                    string str1 = ((int)time / 60).ToString();
                    if (str1.Length == 1)
                    {
                        str1 = "0" + str1;
                    }
                    string str2 = ((int)time % 60).ToString();
                    if (str2.Length == 1)
                    {
                        str2 = "0" + str2;
                    }
                    timelabel.text = str1 + ":" + str2;
                }
                if (matchlabel != null)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder(ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(137));
                    sb.Append('.', (int)(time * 10 % 7));
                    if (matchlabel != null)
                    {
                        matchlabel.text = sb.ToString();
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void PvpMatchResult(GowMatchResult type)
    {
        int strid = 156;
        if (type == GowMatchResult.TYPE_ALREADYGOW)
        {
            strid = 167;
        }
        if (type == GowMatchResult.TYPE_ALREADYATTEMPT)
        {
            strid = 166;
            iscancancelmatch = false;
        }
        if (type == GowMatchResult.TYPE_LEVELWRONG)
        {
            strid = 173;
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(strid),
                                                          ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(140), null, null, null, false);
        if (ismatching)
        {
            Matching();
        }
    }

    private void SetStaticProperty()
    {
        ArkCrossEngine.RoleInfo player = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
        if (player != null)
        {
            UnityEngine.Transform tf = transform.Find("Head/name_label");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = player.Nickname;
                }
            }
            tf = transform.Find("LeftFrame/Top/Label1");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    Data_PlayerConfig dpc = PlayerConfigProvider.Instance.GetPlayerConfigById(player.HeroId);
                    if (dpc != null)
                    {
                        ul.text = dpc.m_Name;
                    }
                }
            }
            tf = transform.Find("Head/headPic");
            if (tf != null)
            {
                UISprite us = tf.gameObject.GetComponent<UISprite>();
                if (us != null)
                {
                    Data_PlayerConfig cg = PlayerConfigProvider.Instance.GetPlayerConfigById(player.HeroId);
                    us.spriteName = cg.m_Portrait;
                }
            }
            tf = transform.Find("Right0/Button/Label");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(138);
                }
            }
        }
    }

    private void SetAwardProperty()
    {
        ArkCrossEngine.RoleInfo player = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
        if (player != null)
        {
            if (myscorelabel != null)
            {//积分
                myscorelabel.text = player.Gow.GowElo.ToString();
            }
            if (myRanklabel != null)
            {//排名
                myRanklabel.text = GetRankTxt(player);
            }
            GowPrizeConfig config = GowConfigProvider.Instance.FindGowPrizeConfig(player.Gow.RankId);
            if (config != null)
            {
                if (lblAwardDiamond != null)
                {
                    lblAwardDiamond.text = config.Gold.ToString();
                }
                if (lblAwardMoney != null)
                {
                    lblAwardMoney.text = config.Money.ToString();
                }
            }
        }
    }

    private string GetRankTxt(RoleInfo roleInfo)
    {
        string txt = StrDictionaryProvider.Instance.GetDictString(35);
        List<GowDataForMsg> list = roleInfo.Gow.GowTop;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].m_Guid == roleInfo.Guid)
            {
                txt = (i + 1).ToString();
                break;
            }
        }
        return txt;
    }

    public void Return()
    {
        if (ismatching)
        {
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(139), ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(140), null, null, null, false);
            return;
        }
        UIManager.Instance.HideWindowByName("Mars");
    }
    public void ChangeShow()
    {
        if (ismatching)
        {
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(139), ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(140), null, null, null, false);
            return;
        }
        UnityEngine.Transform tf = transform.Find("Button/Label");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(142);
            }
        }
        tf = transform.Find("TitleBack/Title");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(141);
            }
        }
        UnityEngine.GameObject item = UIManager.Instance.GetWindowGoByName("Ranking");
        if (item != null)
        {
            item.GetComponent<Ranking>().OpenType(1);
        }
    }

    public void ShowSkill()
    {
        if (ismatching)
        {
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(139), ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(140), null, null, null, false);
            return;
        }
        UIManager.Instance.HideWindowByName("Mars");
        UIManager.Instance.ShowWindowByName("SkillPanel");
    }

    public void Matching()
    {
        string str = "";
        if (ismatching)
        {
            //停止匹配
            str = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(138);
            if (iscancancelmatch)
            {
                ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_cancel_match", "lobby", MatchSceneEnum.Gow);
            }
            else
            {
                iscancancelmatch = true;
            }
            if (matchlabel != null)
            {
                timelabel.text = "00:00";
            }
            if (matchlabel != null)
            {
                matchlabel.text = "";
            }
        }
        else
        {
            //开始匹配
            str = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(143);
            ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_select_scene", "lobby", 3001);
        }
        time = 0.0f;
        ismatching = !ismatching;
        UnityEngine.Transform tf = transform.Find("Right0/Button/Label");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                ul.text = str;
            }
        }
        tf = transform.Find("Right0/Time");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, ismatching);
        }
    }

    private bool ismatching = false;
    private bool iscancancelmatch = true;
    private float time = 0.0f;

    public UILabel timelabel = null;
    public UILabel matchlabel = null;
    public UILabel myscorelabel = null;
    public UILabel myRanklabel = null;
    public UILabel lblAwardDiamond = null;
    public UILabel lblAwardMoney = null;
    private List<UnityEngine.GameObject> golist = new List<UnityEngine.GameObject>();
}
