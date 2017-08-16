using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class Switch : MonoBehaviour
{
	void Start ()
    {
	}
	
	void Update ()
    {
	}

    public void StartNetworkAccelerating()
    {
        DelayManager.IsDelayEnabled = false;
        CloseWindow();
    }

    public void StopNetworkAccelerating()
    {
        DelayManager.IsDelayEnabled = true;
        CloseWindow();
    }

    public void CloseWindow()
    {
        UIManager.Instance.HideWindowByName("HuaweiNASwitch");
    }
}
