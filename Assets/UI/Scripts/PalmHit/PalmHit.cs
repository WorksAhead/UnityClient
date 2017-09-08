using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PalmHit : UnityEngine.MonoBehaviour
{
    public void Left()
    {
        try
        {
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_ui_leftpalm", "ui");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void Right()
    {
        try
        {
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_ui_rightpalm", "ui");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}
