using UnityEngine;

/// <summary>
/// Attach to a game object to make its rotation always lag behind its parent as the parent rotates.
/// </summary>

[AddComponentMenu("NGUI/Examples/Lag Rotation")]
public class LagRotation : UnityEngine.MonoBehaviour
{
	public int updateOrder = 0;
	public float speed = 10f;
	public bool ignoreTimeScale = false;
	
	UnityEngine.Transform mTrans;
	UnityEngine.Quaternion mRelative;
	UnityEngine.Quaternion mAbsolute;
	
	void OnEnable()
	{
		mTrans = transform;
		mRelative = mTrans.localRotation;
		mAbsolute = mTrans.rotation;
	}

	void Update ()
	{
		UnityEngine.Transform parent = mTrans.parent;
		
		if (parent != null)
		{
			float delta = ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime;
			mAbsolute = UnityEngine.Quaternion.Slerp(mAbsolute, parent.rotation * mRelative, delta * speed);
			mTrans.rotation = mAbsolute;
		}
	}
}
