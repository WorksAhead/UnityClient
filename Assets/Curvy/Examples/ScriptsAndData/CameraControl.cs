using UnityEngine;
using System.Collections;

public class CameraControl : UnityEngine.MonoBehaviour {
    public UnityEngine.Transform Character;
    public float Distance=10;
    public float Height = 2;
    UnityEngine.Transform mTransform;

	// Use this for initialization
	void Start () {
        mTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
        UnityEngine.Vector3 center = new UnityEngine.Vector3(0, Character.position.y, 0);
        UnityEngine.Vector3 charPos=Character.position;
        Ray R = new Ray(center,charPos-center);
        UnityEngine.Vector3 camPos = R.GetPoint((charPos-center).magnitude + Distance) + new UnityEngine.Vector3(0, Height, 0);
        // Damping
        mTransform.position = new UnityEngine.Vector3(UnityEngine.Mathf.Lerp(mTransform.position.x, camPos.x, 0.08f),
                                          UnityEngine.Mathf.Lerp(mTransform.position.y, camPos.y, 0.01f),
                                          UnityEngine.Mathf.Lerp(mTransform.position.z, camPos.z, 0.08f));  
            
        mTransform.LookAt(center);
	}
    
}
