using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuaweiAcc : MonoBehaviour
{
	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void quitBattle()
    {
        ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_quit_battle", "lobby", false);
    }

    public void showSwitchWindow()
    {
        UIManager.Instance.ShowWindowByName("HuaweiNASwitch");
    }
}

