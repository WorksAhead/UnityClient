using UnityEngine;
using System.Collections;

public class ClosestPoint : UnityEngine.MonoBehaviour {
    public CurvySplineBase Target;
    public UnityEngine.Transform TargetTransform;

	// Update is called once per frame
	void Update () {
        if (Target && Target.IsInitialized && TargetTransform) {
            float tf = Target.GetNearestPointTF(transform.position);
            TargetTransform.position = Target.Interpolate(tf);
        }
	}
}
