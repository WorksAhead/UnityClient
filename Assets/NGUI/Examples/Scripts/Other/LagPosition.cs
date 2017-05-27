using UnityEngine;

/// <summary>
/// Attach to a game object to make its position always lag behind its parent as the parent moves.
/// </summary>

[AddComponentMenu("NGUI/Examples/Lag Position")]
public class LagPosition : UnityEngine.MonoBehaviour
{
	public int updateOrder = 0;
	public UnityEngine.Vector3 speed = new UnityEngine.Vector3(10f, 10f, 10f);
	public bool ignoreTimeScale = false;
	
	UnityEngine.Transform mTrans;
	UnityEngine.Vector3 mRelative;
	UnityEngine.Vector3 mAbsolute;

	void OnEnable ()
	{
		mTrans = transform;
		mAbsolute = mTrans.position;
		mRelative = mTrans.localPosition;
	}

	void Update ()
	{
		UnityEngine.Transform parent = mTrans.parent;
		
		if (parent != null)
		{
			float delta = ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime;
			UnityEngine.Vector3 target = parent.position + parent.rotation * mRelative;
			mAbsolute.x = UnityEngine.Mathf.Lerp(mAbsolute.x, target.x, UnityEngine.Mathf.Clamp01(delta * speed.x));
			mAbsolute.y = UnityEngine.Mathf.Lerp(mAbsolute.y, target.y, UnityEngine.Mathf.Clamp01(delta * speed.y));
			mAbsolute.z = UnityEngine.Mathf.Lerp(mAbsolute.z, target.z, UnityEngine.Mathf.Clamp01(delta * speed.z));
			mTrans.position = mAbsolute;
		}
	}
}
