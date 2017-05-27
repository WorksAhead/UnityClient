public class LoadingSword : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        try
        {
            signforfake = false;
            time = 0.0f;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (signforfake)
            {
                float dt = RealTime.deltaTime;
                if (dt <= 0.5f)
                {
                    time += dt;
                }
                if (time <= 2.0f)
                {
                    UISlider us = this.GetComponent<UISlider>();
                    if (us != null)
                    {
                        us.value = time / 1.8f;
                    }
                }
                else
                {
                    signforfake = false;
                    time = 0.0f;
                    UISlider us = this.GetComponent<UISlider>();
                    if (us != null)
                    {
                        us.value = 0.0f;
                    }
                    NGUITools.Destroy(this.transform.parent.gameObject);
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void EndLoading()
    {
        signforfake = true;
        time = 0.0f;
        UISlider us = this.GetComponent<UISlider>();
        if (us != null)
        {
            us.value = 0.0f;
        }
        UnityEngine.GameObject parentGO = UnityEngine.GameObject.FindGameObjectWithTag("UI");
        if (parentGO != null)
        {
            UnityEngine.Transform parentTransform = parentGO.transform;  //剧情对话框的父UnityEngine.Transform
            UnityEngine.Transform t = parentTransform.Find("StoryDlg");
            if (t != null)
            {
                NGUITools.SetActive(t.gameObject, false);
            }
        }
    }
    private bool signforfake = false;
    private float time = 0.0f;
}
