using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class HDREnable : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeHDRState()
    {
        Camera main = Camera.main;
        if (main != null)
        {
            CYBloom bloom = main.GetComponent<CYBloom>();
            if (bloom != null)
            {
                bloom.enabled = !bloom.enabled;
            }

            CYTonemapping hdr = main.GetComponent<CYTonemapping>();
            if (hdr != null)
            {
                hdr.enabled = !hdr.enabled;
            }
        }
    }
}
