using ArkCrossEngine;

public class LoadingProgressBar : UnityEngine.MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        try
        {
            // disable joyStick while loading
            if (JoyStickInputProvider.JoyStickEnable)
            {
                JoyStickInputProvider.JoyStickEnable = false;
            }

            // enable decrate icon when first time loading start
            if (sign1 && LogicSystem.GetLoadingProgress() > 0)
            {
                sign1 = false;
                UnityEngine.Transform tf = gameObject.transform.Find("Background/Panel/Icon");
                if (tf != null)
                {
                    NGUITools.SetActive(tf.gameObject, true);
                }
            }

            // loading end, begin destroy stage
            if (sign3)
            {
                time += RealTime.deltaTime;
                if (time >= 2.0f)
                {
                    DestoryLoading();
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void LateUpdate()
    {
        try
        {
            // update progress value
            UISlider us = this.GetComponent<UISlider>();
            float progressvalue = ArkCrossEngine.LogicSystem.GetLoadingProgress();
            if (us != null)
            {
                us.value = progressvalue;
            }

            // update tips
            UnityEngine.Transform tipObj = gameObject.transform.Find("Tip");
            if (tipObj != null)
            {
                UILabel tipLabel = tipObj.gameObject.GetComponent<UILabel>();
                if (tipLabel != null)
                {
                    tipLabel.text = ArkCrossEngine.LogicSystem.GetLoadingTip();
                }
            }

            // disable decrate icon enabled in update
            if (progressvalue >= 0.9999f)
            {
                UnityEngine.Transform tf = gameObject.transform.Find("Background/Panel/Icon");
                if (tf != null)
                {
                    NGUITools.SetActive(tf.gameObject, false);
                }
            }

            // update version info
            string versionInfo = ArkCrossEngine.LogicSystem.GetVersionInfo();
            UnityEngine.Transform versionInfoObj = gameObject.transform.Find("VersionInfo");
            if (versionInfoObj != null && !string.IsNullOrEmpty(versionInfo))
            {
                UILabel versionInfoLabel = versionInfoObj.gameObject.GetComponent<UILabel>();
                if (versionInfoLabel != null)
                {
                    versionInfoLabel.text = versionInfo;
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    
    // message send from gameLogic
    void EndLoading()
    {
        sign2 = false;
        time = 0.0f;
    }

    void DestoryLoading()
    {
        // show joyStick if needed
        if (InputType.Joystick == DFMUiRoot.InputMode)
        {
            JoyStickInputProvider.JoyStickEnable = UIManager.Instance.IsUIVisible;
        }

        // disable joyStick input for some special cases
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("cangbaotu");
        if (go != null && NGUITools.GetActive(go))
        {
            JoyStickInputProvider.JoyStickEnable = false;
        }
        UnityEngine.GameObject parGo = UIManager.Instance.GetWindowGoByName("PartnerPvp");
        if (parGo != null && NGUITools.GetActive(parGo))
        {
            JoyStickInputProvider.JoyStickEnable = false;
        }
        go = UIManager.Instance.GetWindowGoByName("TrialIntro");
        if (go != null && NGUITools.GetActive(go))
        {
            JoyStickInputProvider.JoyStickEnable = false;
        }

        // reset and destroy unused ui elements
        sign1 = true;
        sign2 = true;
        sign3 = true;
        time = 0f;
        NGUITools.DestroyImmediate(this.transform.parent.gameObject);

        // notify loading ended
        UIDataCache.Instance.isLoadingEnd = true;
        
        // show external ui for mars mode
        if (UIDataCache.Instance.needShowMarsLoading)
        {
            LogicSystem.EventChannelForGfx.Publish("ge_show_marsloading", "ui");
            UIDataCache.Instance.needShowMarsLoading = false;
        }

        // begin to play background music
        LogicSystem.EventChannelForGfx.Publish("ge_play_backgroud_music", "music");
    }

    private bool sign1 = true;
    private bool sign2 = true;
    private bool sign3 = true;
    private float time = 0f;
}
