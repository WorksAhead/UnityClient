using UnityEngine;
using System.Collections;

/*
 * This script demonstrates the usage of UserValues:
 * 
 * Here we use the x value of the UserValue to scale the cube
 * 
 */
/// <summary>
/// Example of how to work with User Values
/// </summary>
public class UserValues : UnityEngine.MonoBehaviour {
    
    SplineWalker walkerScript;
    Material mMat;

	// Use this for initialization
	void Start () {
        walkerScript = GetComponent<SplineWalker>();
        mMat = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        if (walkerScript && walkerScript.Spline.IsInitialized) {
            // Scale is interpolated from the Control Point's scale
            transform.localScale = walkerScript.Spline.InterpolateScale(walkerScript.TF);
            // UnityEngine.Color is stored as UnityEngine.Vector3 in the UserValues array. We transform it back and set the material's color
            mMat.color = Vector3ToColor(walkerScript.Spline.InterpolateUserValue(walkerScript.TF, 0));
        }
	}

    UnityEngine.Color Vector3ToColor(UnityEngine.Vector3 v)
    {
        return new UnityEngine.Color(v.x, v.y, v.z);
    }
}
