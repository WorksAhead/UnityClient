using UnityEngine;
using ArkCrossEngine;

public class FingerUpEvent : FingerEvent
{
    float timeHeldDown = 0;
    public float TimeHeldDown
    {
        get
        {
            return timeHeldDown;
        }
        set
        {
            timeHeldDown = value;
        }
    }
}

public class FingerUp : FingerEventDetector<FingerUpEvent>
{
    public FingerEventHandler OnFingerUp;
    public string MessageName = "OnFingerUp";

    protected override void ProcessFinger(TouchManager.Finger finger)
    {
        if (!finger.IsDown && finger.WasDown)
        {
            FingerUpEvent e = GetEvent(finger);
            if (null != e)
            {
                e.Name = MessageName;
                e.TimeHeldDown = UnityEngine.Mathf.Max(0, UnityEngine.Time.time - finger.StarTime);
                if (OnFingerUp != null)
                {
                    OnFingerUp(e);
                }
                TrySendMessage(e);
            }
            ///
            UpdateRegognizerScript();
        }
    }
    private void UpdateRegognizerScript()
    {
        ///
        TouchManager.curTouchState = ArkCrossEngine.TouchType.Regognizer;
    }
}
