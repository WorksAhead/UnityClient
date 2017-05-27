using UnityEngine;

/// <summary>
/// Attaching this script to an object will make that object face the specified target.
/// The most ideal use for this script is to attach it to the camera and make the camera look at its target.
/// </summary>

[AddComponentMenu("NGUI/Examples/Look At Target")]
public class LookAtTarget : UnityEngine.MonoBehaviour
{
	public int level = 0;
	public UnityEngine.Transform target;
	public float speed = 8f;

	UnityEngine.Transform mTrans;

	void Start ()
	{
		mTrans = transform;
	}

	void LateUpdate ()
	{
		if (target != null)
		{
			UnityEngine.Vector3 dir = target.position - mTrans.position;
			float mag = dir.magnitude;

			if (mag > 0.001f)
			{
				UnityEngine.Quaternion lookRot = UnityEngine.Quaternion.LookRotation(dir);
				mTrans.rotation = UnityEngine.Quaternion.Slerp(mTrans.rotation, lookRot, UnityEngine.Mathf.Clamp01(speed * Time.deltaTime));
			}
		}
	}
}