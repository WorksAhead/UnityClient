using System.Collections.Generic;

public class MainCityTopTween : UnityEngine.MonoBehaviour
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

    void Awake()
    {
        UITweener[] tweenList = GetComponentsInChildren<UITweener>();
        foreach (UITweener tween in tweenList)
        {
            if (tween is TweenPosition)
            {
                UnityEngine.Vector3 lp = tween.transform.localPosition;
                (tween as TweenPosition).from = new UnityEngine.Vector3(lp.x, lp.y + 100, lp.z);
                (tween as TweenPosition).to = lp;
            }

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

    internal void Down()
    {
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

    internal void Up()
    {
        foreach (UITweener ts in m_OpenTweenList)
        {
            ts.ResetToBeginning();
            ts.PlayForward();
        }
        foreach (UITweener tween in m_RevertTweenList)
        {
            tween.enabled = true;
            tween.PlayReverse();//这两行顺序不可颠倒，因为关闭时会playreverse,先执行reset会置为end，详见函数说明
        }
    }
}
