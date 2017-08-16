///PVP延迟UI
using UnityEngine;
using System.Collections;
using ArkCrossEngine;

public class UIPvpDelay : UnityEngine.MonoBehaviour
{

    public float RefreshTime = 1.0f;//刷新时间
    private float CountDown = 0f;
    public UILabel lblDelayValue;
    public UILabel lblUIDelay;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (CountDown <= 0f)
            {
                CountDown = RefreshTime;
                UpdateDelayValue();
            }
            else
            {
                CountDown -= UnityEngine.Time.deltaTime;
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void UpdateDelayValue()
    {
        //long delay_time = TimeUtility.AverageRoundtripTime;
        long delay_time = DelayManager.GetFakePingValue();
        string prefix = DelayManager.IsDelayEnabled ? "[ff0000]" : "[00ff00]";
        string suffix = "[-]";

        if (lblDelayValue != null)
            lblDelayValue.text = prefix + delay_time.ToString() + suffix;
        if (lblUIDelay != null)
        {
            lblUIDelay.text = prefix + "ping:[-]";
        }

        if (delay_time > 130)
        {
            lblUIDelay.color = UnityEngine.Color.red;
        }
        else
        {
            lblUIDelay.color = UnityEngine.Color.red;
        }

    }
}
