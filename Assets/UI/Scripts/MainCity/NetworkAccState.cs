using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArkCrossEngine;

public class NetworkAccState : MonoBehaviour {

    UILabel lblSelf;
	// Use this for initialization
	void Start () {
        lblSelf = GetComponent<UILabel>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (lblSelf == null)
        {
            return;
        }

        if (DelayManager.IsDelayEnabled)
        {
            lblSelf.text = "[ff0000]网络加速未开启[-]";
        }
        else
        {
            lblSelf.text = "[00ff00]网络加速已开启[-]";
        }
	}
}
