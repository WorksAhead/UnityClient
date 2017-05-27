using UnityEngine;
using System.Collections;

public class TweenProgressBar : UnityEngine.MonoBehaviour
{

    public UIProgressBar progress;
    public float perChange = 0.01f;

    private float m_value;
    private bool goMax;//是否先走满
    private bool run;

    public OnProgress onProgressFun;
    public delegate void OnProgress(float progress);
    public OnProgressFinish onProgressFinish;
    public delegate void OnProgressFinish();
    public OnProgressMax onProgressMax;
    public delegate void OnProgressMax();

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (progress == null || run == false)
                return;
            progress.value += perChange;
            if (onProgressFun != null)
            {
                onProgressFun(progress.value);
            }
            if (progress.value >= 1)
            {
                if (onProgressMax != null)
                {
                    onProgressMax();
                }
            }
            //先走满
            if (goMax == true)
            {
                if (progress.value >= 1)
                {
                    progress.value = 0;
                    goMax = false;
                }
            }
            else
            {
                if (progress.value >= m_value)
                {
                    progress.value = m_value;
                    run = false;
                    if (onProgressFinish != null)
                    {
                        onProgressFinish();
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void SetValue(float value, bool maxFirst = false)
    {
        m_value = value;
        run = true;
        //if (m_value > GetValue()) {
        //  goMax = false;
        //}
        //if (m_value < GetValue()) {
        //  goMax = true;
        //}
        goMax = maxFirst;
    }

    public float GetValue()
    {
        if (progress != null)
        {
            return progress.value;
        }
        return 0;
    }


}
