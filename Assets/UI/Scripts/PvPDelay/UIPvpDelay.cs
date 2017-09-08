///PVP延迟UI
using UnityEngine;
using System.Collections;
using ArkCrossEngine;

public class UIPvpDelay : UnityEngine.MonoBehaviour
{

    public float RefreshTime = 1.0f;//刷新时间
    private float CountDown = 0f;
    public UILabel lblDelayValue;
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
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void UpdateDelayValue()
    {
        long delay_time = TimeUtility.AverageRoundtripTime;
        if (lblDelayValue != null)
            lblDelayValue.text = delay_time.ToString();
    }
}
