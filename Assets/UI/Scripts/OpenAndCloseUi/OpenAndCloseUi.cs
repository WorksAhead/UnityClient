using System.Collections.Generic;

public class OpenAndCloseUi : UnityEngine.MonoBehaviour
{

    public int openTweenGroup = 8;
    public int closeTweenGroup = 9;
    public int revertTweenGroup = 10;
    public float closeWaitTime = 0.3f;

    private string windowName = "";

    private List<UITweener> m_OpenTweenList = new List<UITweener>();
    private List<UITweener> m_CloseTweenList = new List<UITweener>();
    private List<UITweener> m_RevertTweenList = new List<UITweener>();
    private List<object> m_EventList = new List<object>();

    //public void UnSubscribe ()
    //{
    //  try {
    //    for (int i = 0; i < m_EventList.Count; i++) {
    //      if (m_EventList[i] != null) {
    //        ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(m_EventList[i]);
    //      }
    //    }
    //    m_EventList.Clear();
    //  } catch (Exception ex) {
    //    ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
    //  }
    //}

    //private void InitSubscribe ()
    //{
    //  object obj = null;
    //  obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
    //  if (obj != null)
    //    m_EventList.Add(obj);

    //}

    void Awake()
    {
        UITweener[] tweenList = GetComponentsInChildren<UITweener>();
        foreach (UITweener tween in tweenList)
        {
            if (tween.tweenGroup == openTweenGroup)
            {
                m_OpenTweenList.Add(tween);
            }
            else if (tween.tweenGroup == closeTweenGroup)
            {
                m_CloseTweenList.Add(tween);
            }
            else if (tween.tweenGroup == revertTweenGroup)
            {
                m_RevertTweenList.Add(tween);
            }
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        if (IsInvoking("HideWindow"))
        {
            CancelInvoke("HideWindow");
        }
        foreach (UITweener ts in m_OpenTweenList)
        {
            ts.ResetToBeginning();
            ts.PlayForward();
        }
        foreach (UITweener tween in m_RevertTweenList)
        {
            tween.PlayForward();
            tween.ResetToBeginning();//这两行顺序不可颠倒，因为关闭时会playreverse,先执行reset会置为end，详见函数说明
        }
    }

    internal void OnCloseUI(string HideWindowName)
    {
        windowName = HideWindowName;
        foreach (UITweener ts in m_CloseTweenList)
        {
            ts.ResetToBeginning();
            ts.PlayForward();
        }
        foreach (UITweener tween in m_RevertTweenList)
        {
            tween.enabled = true;
            tween.PlayReverse();
        }
        Invoke("HideWindow", closeWaitTime);
    }

    private void HideWindow()
    {
        UIManager.Instance.HideWindowByName(windowName);
    }
}
