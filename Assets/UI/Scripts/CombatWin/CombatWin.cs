using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatWin : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject goEffectTitle = null;
    private UnityEngine.GameObject runtimeEffect;
    public UnityEngine.GameObject goTitle = null;//特效goEffectTitle播在该UnityEngine.GameObject上
    private List<object> eventlist = new List<object>();
    public UnityEngine.GameObject cardEffect = null; // 翻拍特效
    public UnityEngine.GameObject cloneEffect = null; // 特效副本
    public float m_DelayTimeToMaincity = 2f;
    private bool effectPlay = true;
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
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Use this for initialization
    void Awake()
    {
        try
        {
            if (eventlist != null) { eventlist.Clear(); }
            object eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, int>("ge_turnover_card", "ui", GetPrize);
            if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (eo != null) eventlist.Add(eo);


            UnityEngine.Transform tf = transform.Find("Time/Label");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    timelabel = ul;
                    ul.text = "16:00";
                }
            }
            for (int i = 0; i < 4; ++i)
            {
                UnityEngine.Transform tfcard = transform.Find(i.ToString());
                if (tfcard != null)
                {
                    UIEventListener.Get(tfcard.gameObject).onClick = CardClick;
                    tfcard.localPosition = new UnityEngine.Vector3(0f, -40f, 0f);
                }
            }
            //UIManager.Instance.HideWindowByName("CombatWin");
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
            if (Clickwhich != -1) { return; }
            time += RealTime.deltaTime;
            int second = (int)(16 - time);

            if (second > 15.0f)
            {
                second = 15;
            }
            else if (second <= 0.0f)
            {
                second = 0;
                if (Clickwhich == -1)
                {
                    UnityEngine.Transform tf = transform.Find("0");
                    if (tf != null)
                    {
                        CardClick(tf.gameObject);
                    }
                }
            }


            if (timelabel != null)
            {
                string str1 = (second / 60).ToString();
                if (str1.Length == 1)
                {
                    str1 = "0" + str1;
                }
                string str2 = (second % 60).ToString();
                if (str2.Length == 1)
                {
                    str2 = "0" + str2;
                }
                timelabel.text = str1 + ":" + str2;
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //播放翻拍特效
    public void PlayEffect(UnityEngine.GameObject go)
    {
        cloneEffect = ArkCrossEngine.ResourceSystem.NewObject(cardEffect) as UnityEngine.GameObject;
        if (cloneEffect != null && go != null)
        {
            UnityEngine.Transform tf = go.transform.Find("Texture");
            if (tf != null)
            {
                cloneEffect.transform.position = tf.position;
            }
        }
    }
    //标题特效
    public void TitleEffect()
    {
        runtimeEffect = ArkCrossEngine.ResourceSystem.NewObject(goEffectTitle) as UnityEngine.GameObject;
        if (runtimeEffect != null && goTitle != null)
        {
            runtimeEffect.transform.position = goTitle.transform.position;
        }
    }
    //销毁特效
    void DestroyEffect()
    {
        Destroy(cloneEffect);
    }
    public void ButtonClick()
    {
        //发送消息
        if (Clickwhich >= 0 && Clickwhich < 4)
        {
            DestroyEffect();
            UIManager.Instance.HideWindowByName("CombatWin");
            ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_return_maincity", "lobby");
            effectPlay = true;
        }
        else
        {
            UnityEngine.Transform tf = transform.Find("0");
            if (tf != null)
            {
                CardClick(tf.gameObject);
            }
        }
    }
    public void CardClick(UnityEngine.GameObject go)
    {
        if (effectPlay)
        {
            PlayEffect(go);
            effectPlay = false;
        }
        if (!firstclicked)
        {
            firstclicked = true;
            if (positionOK)
            {
                positionOK = false;
                int i;
                if (int.TryParse(go.transform.name, out i))
                {
                    Clickwhich = i;
                    //           UnityEngine.Transform tf = transform.Find(i.ToString());
                    //           if (tf != null) {
                    TweenRotation tr = /*tf.gameObject*/go.GetComponents<TweenRotation>()[0];
                    if (tr != null)
                    {
                        tr.PlayForward();
                    }
                    //}
                }
                UnityEngine.Transform tfc = transform.Find("ExtractNum");
                if (tfc != null)
                {
                    UILabel ul = tfc.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.Format(136, 0);
                    }
                }
            }
            for (int m = 0; m < 4; ++m)
            {
                UnityEngine.Transform tf = transform.Find(m.ToString());
                if (tf != null)
                {
                    UnityEngine.BoxCollider bc = tf.gameObject.GetComponent<UnityEngine.BoxCollider>();
                    if (bc != null)
                    {
                        bc.enabled = false;
                    }
                }
            }
        }
        else
        {
            if (go != null)
            {
                TweenRotation tr = go.GetComponents<TweenRotation>()[0];
                if (tr != null)
                {
                    tr.PlayForward();
                }
            }
        }
    }
    public void SetPositionOk()
    {
        positionOK = true;
        UnityEngine.Transform tf = transform.Find("Container");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, false);
        }
    }
    public void TweenPotationHalfOK()
    {
        try
        {
            if (!firsthalfed)
            {
                SetInfo(Clickwhich, prizeid, prizecount);
                firsthalfed = true;
                StartCoroutine(DelayForTurnedCard());
                UnityEngine.Transform tf = transform.Find(Clickwhich.ToString() + "/Light");
                if (tf != null)
                {
                    NGUITools.SetActive(tf.gameObject, true);
                }
                for (int i = 0; i < 4; ++i)
                {
                    UnityEngine.Transform tfcard = transform.Find(i.ToString());
                    if (tfcard != null)
                    {
                        UIEventListener.Get(tfcard.gameObject).onClick -= CardClick;
                    }
                }
            }
            else
            {
                if (firstsetall)
                {
                    UnityEngine.Transform tf = transform.Find("Button");
                    if (tf != null)
                    {
                        NGUITools.SetActive(tf.gameObject, true);
                    }
                    firstsetall = false;
                    ArkCrossEngine.Data_SceneConfig dsc = ArkCrossEngine.SceneConfigProvider.Instance.GetSceneConfigById(DFMUiRoot.NowSceneID);
                    ArkCrossEngine.RoleInfo roleInfo = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
                    if (dsc != null && null != roleInfo)
                    {
                        ArkCrossEngine.Data_SceneDropOut dsdo = ArkCrossEngine.SceneConfigProvider.Instance.GetSceneDropOutById(dsc.m_DropId);
                        if (dsdo != null)
                        {
                            List<int> itemIdList = dsdo.GetRewardItemByHeroId(roleInfo.HeroId);
                            if (null != itemIdList && itemIdList.Count > 0)
                            {
                                if (dsdo.m_ItemCount == 4 && itemIdList != null && itemIdList.Count == 4)
                                {
                                    bool sign = true;
                                    for (int j = 0; j < 4; ++j)
                                    {
                                        if (j != Clickwhich)
                                        {
                                            if (itemIdList[j] == prizeid)
                                            {
                                                if (sign)
                                                {
                                                    sign = false;
                                                    SetInfo(j, itemIdList[Clickwhich], dsdo.m_ItemCountList[Clickwhich]);
                                                }
                                                else
                                                {
                                                    SetInfo(j, itemIdList[j], dsdo.m_ItemCountList[j]);
                                                }
                                            }
                                            else
                                            {
                                                SetInfo(j, itemIdList[j], dsdo.m_ItemCountList[j]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception)
        {
            //
        }
    }
    public IEnumerator DelayForTurnedCard()
    {
        yield return new WaitForSeconds(0.8f);
        for (int j = 0; j < 4; ++j)
        {
            if (Clickwhich != j)
            {
                UnityEngine.Transform tf = transform.Find(j.ToString());
                if (tf != null && tf.gameObject != null)
                {
                    CardClick(tf.gameObject);
                }
            }
        }
        yield return new WaitForSeconds(2f);
        UIManager.Instance.HideWindowByName("CombatWin");
        ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_return_maincity", "lobby");
    }
    void SetInfo(int which, int id, int num = 1)
    {
        UnityEngine.Transform tf = transform.Find(which.ToString());
        if (tf != null)
        {
            TweenRotation tr = tf.gameObject.GetComponents<TweenRotation>()[1];
            if (tr != null)
            {
                tr.PlayForward();
            }
        }
        tf = transform.Find(which.ToString());
        if (tf != null)
        {
            UISprite us = tf.gameObject.GetComponent<UISprite>();
            if (us != null)
            {
                us.spriteName = "pai2";
            }
        }
        ArkCrossEngine.ItemConfig ic = ArkCrossEngine.LogicSystem.GetItemDataById(id);
        if (ic == null)
        {
            DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.CombatWin, tf.gameObject, 101001);
        }
        else
        {
            if (num > 1)
            {
                DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.CombatWin, tf.gameObject, id, num);
            }
            else
            {
                DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.CombatWin, tf.gameObject, id);
            }
        }

    }
    private void GetPrize(int id, int num)
    {
        try
        {
            prizeid = id;
            prizecount = num;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private bool positionOK = false;
    private int Clickwhich = -1;
    private float time = 0.0f;
    private UILabel timelabel = null;
    private int prizeid = 0;
    private int prizecount = 0;
    private bool firstclicked = false;
    private bool firsthalfed = false;
    private bool firstsetall = true;
}
