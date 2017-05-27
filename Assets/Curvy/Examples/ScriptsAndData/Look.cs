using UnityEngine;
using System.Collections;

public class Look : UnityEngine.MonoBehaviour {
    public UnityEngine.Transform Target;

    UnityEngine.Transform mTransform;

	// Use this for initialization
	void Start () {
        mTransform = transform;
	}
	
	
	void LateUpdate () {
        if (Target)
            mTransform.LookAt(Target);
	}
}
