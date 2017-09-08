using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

struct DialogMessageInfo
{
    public string message;
    public string button0;
    public string button1;
    public string button2;
    public ArkCrossEngine.MyAction<int> dofunction;
    public bool islogic;
}
public class Dialog : UnityEngine.MonoBehaviour
{
    //当正在显示一个对话时， 此时发过来要显示第二个对话，但上一个对话还没关闭，就用此list缓存起来，当地一个对话关闭时再显示。
    private List<DialogMessageInfo> dialogList = new List<DialogMessageInfo>();
    private void ResetTransZPos()
    {
        this.transform.localPosition = new UnityEngine.Vector3(0, 0, -1000);
    }
    // Use this for initialization
    void Awake()
    {
    }
    void Start()
    {
        try
        {
            ResetTransZPos();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void ManageDialog(string message, string button0, string button1, string button2, ArkCrossEngine.MyAction<int> dofunction, bool islogic)
    {
        try
        {
            // 不是自身调用，且有对话显示， 那么就缓存此数据退出
            if (!isSecond)
            {
                CacheDialog(message, button0, button1, button2, dofunction, islogic);
                if (dialogList.Count > 1)
                    return;
            }
            isSecond = false;

            doSomething = dofunction;
            isLogic = islogic;
            UIManager.Instance.ShowWindowByName("Dialog");

            UnityEngine.Transform tf = transform.Find("Sprite/Button0");
            if (tf != null)
            {
                if (button0 == null)
                {
                    if (NGUITools.GetActive(tf.gameObject))
                    {
                        NGUITools.SetActive(tf.gameObject, false);
                    }
                }
                else
                {
                    if (!NGUITools.GetActive(tf.gameObject))
                    {
                        NGUITools.SetActive(tf.gameObject, true);
                    }
                    tf = tf.Find("Label");
                    if (tf != null)
                    {
                        UILabel ul = tf.gameObject.GetComponent<UILabel>();
                        if (ul != null)
                        {
                            ul.text = button0;
                        }
                    }
                }
            }
            tf = transform.Find("Sprite/Button1");
            if (tf != null)
            {
                if (button1 == null)
                {
                    if (NGUITools.GetActive(tf.gameObject))
                    {
                        NGUITools.SetActive(tf.gameObject, false);
                    }
                }
                else
                {
                    if (!NGUITools.GetActive(tf.gameObject))
                    {
                        NGUITools.SetActive(tf.gameObject, true);
                    }
                    tf = tf.Find("Label");
                    if (tf != null)
                    {
                        UILabel ul = tf.gameObject.GetComponent<UILabel>();
                        if (ul != null)
                        {
                            ul.text = button1;
                        }
                    }
                }
            }
            tf = transform.Find("Sprite/Button2");
            if (tf != null)
            {
                if (button2 == null)
                {
                    if (NGUITools.GetActive(tf.gameObject))
                    {
                        NGUITools.SetActive(tf.gameObject, false);
                    }
                }
                else
                {
                    if (!NGUITools.GetActive(tf.gameObject))
                    {
                        NGUITools.SetActive(tf.gameObject, true);
                    }
                    tf = tf.Find("Label");
                    if (tf != null)
                    {
                        UILabel ul = tf.gameObject.GetComponent<UILabel>();
                        if (ul != null)
                        {
                            ul.text = button2;
                        }
                    }
                }
            }
            tf = transform.Find("Sprite/Label");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = message;
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void Button0()
    {
        if (doSomething != null)
        {
            if (isLogic)
            {
                ArkCrossEngine.LogicSystem.QueueLogicAction(doSomething, 0);
            }
            else
            {
                doSomething(0);
            }
        }
        doSomething = null;
        CheckCacheDialog();
    }
    public void Button1()
    {
        if (doSomething != null)
        {
            if (isLogic)
            {
                ArkCrossEngine.LogicSystem.QueueLogicAction(doSomething, 1);
            }
            else
            {
                doSomething(1);
            }
        }
        doSomething = null;
        CheckCacheDialog();
    }
    public void Button2()
    {
        if (doSomething != null)
        {
            if (isLogic)
            {
                ArkCrossEngine.LogicSystem.QueueLogicAction(doSomething, 2);
            }
            else
            {
                doSomething(2);
            }
        }
        doSomething = null;
        CheckCacheDialog();
    }
    private ArkCrossEngine.MyAction<int> doSomething = null;
    private bool isLogic = false;
    //加入缓存中
    void CacheDialog(string message, string button0, string button1, string button2, ArkCrossEngine.MyAction<int> dofunction, bool islogic)
    {
        DialogMessageInfo dialogInfo = new DialogMessageInfo();
        dialogInfo.message = message;
        dialogInfo.button0 = button0;
        dialogInfo.button1 = button1;
        dialogInfo.button2 = button2;
        dialogInfo.dofunction = dofunction;
        dialogInfo.islogic = islogic;
        dialogList.Add(dialogInfo);
    }
    private bool isSecond = false; // 自身第二次调用
                                   // 检验是否有缓存， 若有自身调用缓存的dialog
    void CheckCacheDialog()
    {
        if (dialogList.Count > 0)
        {
            dialogList.RemoveAt(0);
            if (dialogList.Count > 0)
            {
                isSecond = true;
                ManageDialog(dialogList[0].message, dialogList[0].button0, dialogList[0].button1, dialogList[0].button2
                             , dialogList[0].dofunction, dialogList[0].islogic);
            }
            else
            {
                isSecond = false;
                UIManager.Instance.HideWindowByName("Dialog");
            }
        }
    }
}
