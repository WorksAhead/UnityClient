using UnityEngine;
using System.Collections;
using ArkCrossEngine;
using System;
using System.Collections.Generic;
public class WipeoutAward : UnityEngine.MonoBehaviour
{

    /*事件list*/
    private List<object> eventList = new List<object>();
    public UISprite button;
    public UISprite title;
    public float DeltaForWipeout = 0.4f;//每个扫荡时间间隔

    public UnityEngine.GameObject goTweenContainer = null;// 容器
    public UnityEngine.AnimationCurve CurveForUp = null;// 移到上方轨迹
    public float DurationForUp = 1.2f;//移动上时间

    public UnityEngine.GameObject goEffectTitle = null;
    private UnityEngine.GameObject runtimeEffect;
    public UnityEngine.GameObject goTitle = null;//特效goEffectTitle播在该UnityEngine.GameObject上
    public UIDragScrollView dragScrollview;
    private bool isWipeing = true; // 正在扫荡
                                   /*移除panel的Subscribe*/
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

    // Use this for initialization
    void Start()
    {
        try
        {
            AddSubscribe();
            UIManager.Instance.HideWindowByName("WipeoutAward");
            InitEffect();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void InitEffect()
    {
        runtimeEffect = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.NewObject(goEffectTitle));
        if (runtimeEffect != null && goTitle != null)
        {
            runtimeEffect.transform.position = goTitle.transform.position;
        }
        DestroyEffect();
    }
    /*添加监听 subscribe*/
    void AddSubscribe()
    {
        object eo;
        eo = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        eo = LogicSystem.EventChannelForGfx.Subscribe<int, int, List<int>, List<int>>("ge_sync_wipeout_backInfo", "wipeout", BackWipeoutInfo);
        if (null != eo)
        {
            eventList.Add(eo);
        }
    }
    //销毁特效
    void DestroyEffect()
    {
        NGUITools.SetActive(runtimeEffect, false);
    }
    void ShowEffect()
    {
        NGUITools.SetActive(runtimeEffect, true);
    }
    void WipeComplete()
    {
        NGUITools.SetActive(button.gameObject, true);
        NGUITools.SetActive(title.gameObject, true);
        dragScrollview.enabled = true;
        isWipeing = false;
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("SceneIntroduce");
        UISceneIntroduce sceneIntriduce = go.GetComponent<UISceneIntroduce>();
        if (null != sceneIntriduce)
        {
            sceneIntriduce.InitWipeNum();
        }
        ShowEffect();
    }
    void HideWipeTile()
    {
        NGUITools.SetActive(button.gameObject, false);
        NGUITools.SetActive(title.gameObject, false);
        DestroyEffect();
    }
    // Update is called once per frame
    void Update()
    {

    }
    // 扫荡信息返回
    void BackWipeoutInfo(int money, int exp, List<int> idList, List<int> numList)
    {
        try
        {
            UIManager.Instance.ShowWindowByName("WipeoutAward");
            StartCoroutine(WipeBegin(DeltaForWipeout, money, exp, idList, numList));
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //添加扫荡物品
    public IEnumerator WipeBegin(float delta, int money, int exp, List<int> idList, List<int> numList)
    {
        ClearItemDic();
        UnityEngine.Transform tf = gameObject.transform.Find("sp_heikuang/ScrollView/Grid");
        tf.localPosition = new UnityEngine.Vector3(12f, -238f, 0f);
        isWipeing = true;
        if (idList.Count == 1)
        {
            Invoke("WipeComplete", DurationForUp);
            AddItem(money, money, exp, idList[0], numList[0], 0, true);
        }
        else if (idList.Count > 1)
        {
            dragScrollview.enabled = false;
            Invoke("WipeComplete", DeltaForWipeout * idList.Count);
            for (int index = 0; index < idList.Count; ++index)
            {
                AddItem(money, money, exp, idList[index], numList[index], index);
                if (index == idList.Count - 1)
                {
                    //Invoke("WipeComplete", DurationForUp);
                }
                yield return new WaitForSeconds(delta);
            }
        }
        else if (idList.Count == 0)
        {
            SendDialog(568, 4, null);
            UIManager.Instance.HideWindowByName("WipeoutAward");
            HideWipeTile();
        }
        yield return new WaitForSeconds(0f);
    }
    void ClearItemDic()
    {
        foreach (UnityEngine.GameObject go in itemDic)
        {
            if (go != null)
            {
                NGUITools.DestroyImmediate(go);
            }
        }
        itemDic.Clear();
    }
    /*发送对话框*/
    void SendDialog(int i_chn_desc, int i_confirm, params object[] insert_name)
    {
        string chn_desc = "";
        string confirm = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(i_confirm);
        chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(i_chn_desc);
        string str = chn_desc;
        if (insert_name != null)
        {
            str = string.Format(chn_desc, insert_name);
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", str, confirm, null, null, null, false);
    }
    private List<UnityEngine.GameObject> itemDic = new List<UnityEngine.GameObject>();
    void AddItem(float delta, int money, int exp, int id, int num, int battleTh, bool woipOne = false)
    {
        UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/SceneSelect/WipeoutAwardItem"));
        UnityEngine.Transform tf = gameObject.transform.Find("sp_heikuang/ScrollView/Grid");
        if (null != tf)
        {
            go = NGUITools.AddChild(tf.gameObject, go);
            itemDic.Add(go);
            if (null != go)
            {

                UnityEngine.Transform label = go.transform.Find("ExpAward/exp_value");
                if (label != null)
                {
                    UILabel ul = label.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        ul.text = exp.ToString();
                    }
                }
                label = go.transform.Find("MoneyAward/money_value");
                if (label != null)
                {
                    UILabel ul = label.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        if (id == DFMItemIconUtils.Instance.m_Money)
                        {
                            ul.text = (money + num).ToString();
                        }
                        else
                        {
                            ul.text = money.ToString();
                        }

                    }
                }
                label = go.transform.Find("TypeDescription");
                if (label != null)
                {
                    UILabel ul = label.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        string chn_desc = "";
                        if (woipOne)
                        {
                            chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(904);
                        }
                        else
                        {
                            chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(900);
                            chn_desc = string.Format(chn_desc, battleTh + 1);
                        }
                        ul.text = chn_desc;
                    }
                }
                if (money < 1 && exp < 1 && id < 1 && num < 1)
                {
                    label = go.transform.Find("Sprite/tip");
                    if (null != label)
                    {
                        NGUITools.SetActive(label.gameObject, true);
                    }
                }
                SetSomething(money, 0, exp, id, num, go);

            }
        }
        if (tf != null)
        {
            UIGrid ug = tf.gameObject.GetComponent<UIGrid>();
            if (ug != null)
            {
                ug.Reposition();
            }
        }
        UnityEngine.Transform tfScrollView = gameObject.transform.Find("sp_heikuang/ScrollView");
        if (null != tfScrollView)
        {
            if (goTweenContainer != null && battleTh > 0)
            {
                TweenPosition tweenPos = TweenPosition.Begin(goTweenContainer, DurationForUp
                              , new UnityEngine.Vector3(0, 238f * battleTh - tfScrollView.localPosition.y, 0f));
                if (tweenPos != null)
                {
                    tweenPos.animationCurve = CurveForUp;
                }
            }
            else if (goTweenContainer != null && battleTh == 0)
            {
                TweenPosition tweenPos = TweenPosition.Begin(goTweenContainer, DurationForUp
                              , new UnityEngine.Vector3(0, 20 - tfScrollView.localPosition.y, 0f));
                if (tweenPos != null)
                {
                    tweenPos.animationCurve = CurveForUp;
                    tweenPos.from = new UnityEngine.Vector3(0, -238f - tfScrollView.localPosition.y, 0f);
                }
            }
        }
    }
    public void SetSomething(int money, int diamond, int exp, int itemlist, int itemcount, UnityEngine.GameObject item)
    {
        golist.Clear();
        UnityEngine.Transform tfb = item.transform.Find("back");
        if (tfb != null)
        {
            if (money > 0)
            {
                UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/GameTask/AwardItem"));
                if (go != null)
                {
                    go = NGUITools.AddChild(tfb.gameObject, go);
                    if (go != null)
                    {
                        golist.Add(go);
                        DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Task_Award, go, DFMItemIconUtils.Instance.m_Money, money);
                    }
                }
            }
            if (diamond > 0)
            {
                UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/GameTask/AwardItem"));
                if (go != null)
                {
                    go = NGUITools.AddChild(tfb.gameObject, go);
                    if (go != null)
                    {
                        golist.Add(go);
                        DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Task_Award, go, DFMItemIconUtils.Instance.m_Diamond, diamond);
                    }
                }
            }
            if (exp > 0)
            {
                UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/GameTask/AwardItem"));
                if (go != null)
                {
                    go = NGUITools.AddChild(tfb.gameObject, go);
                    if (go != null)
                    {
                        golist.Add(go);
                        DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Task_Award, go, DFMItemIconUtils.Instance.m_Exp, exp);
                    }
                }
            }
            ArkCrossEngine.ItemConfig ic = ArkCrossEngine.ItemConfigProvider.Instance.GetDataById(itemlist);
            if (ic != null)
            {
                UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/GameTask/AwardItem"));
                if (go != null)
                {
                    go = NGUITools.AddChild(tfb.gameObject, go);
                    if (go != null)
                    {
                        golist.Add(go);
                        DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Task_Award, go, itemlist, itemcount);
                    }
                }
            }

        }
        int number = golist.Count;
        if (number == 0)
            return;
        int offset = 0;
        int start = 0;
        if (number % 2 != 0)
        {
            UnityEngine.GameObject go = golist[0];
            if (go != null)
            {
                go.transform.localPosition = new UnityEngine.Vector3(0.0f, 10f, 0.0f);
                start = 1;
                offset = 50;
            }
        }
        else
        {
            offset = -60;
        }
        for (int i = start; i < number; ++i)
        {
            int j = i;
            if (number % 2 == 0)
            {
                j = i + 1;
            }
            UnityEngine.GameObject go = golist[i];
            if (go != null)
            {
                if (j % 2 == 0)
                {
                    go.transform.localPosition = new UnityEngine.Vector3(j / 2 * (-120) - offset, 10, 0);
                }
                else
                {
                    go.transform.localPosition = new UnityEngine.Vector3((j / 2 + 1) * 120 + offset, 10f, 0.0f);
                }
            }
        }
    }
    public void OnclickButton()
    {
        if (isWipeing)
            return;
        UnityEngine.Transform tfScrollView = gameObject.transform.Find("sp_heikuang/ScrollView");
        UIScrollView scroll = tfScrollView.GetComponent<UIScrollView>();
        goTweenContainer.transform.localPosition = new UnityEngine.Vector3();
        scroll.ResetPosition();
        ClearItemDic();
        HideWipeTile();
        UIManager.Instance.HideWindowByName("WipeoutAward");
    }
    private List<UnityEngine.GameObject> golist = new List<UnityEngine.GameObject>();
}
