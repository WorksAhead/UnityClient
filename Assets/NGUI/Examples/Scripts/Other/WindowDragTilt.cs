using UnityEngine;

/// <summary>
/// Attach this script to a child of a draggable window to make it tilt as it's dragged.
/// Look at how it's used in Example 6.
/// </summary>

[AddComponentMenu("NGUI/Examples/Window Drag Tilt")]
public class WindowDragTilt : UnityEngine.MonoBehaviour
{
	public int updateOrder = 0;
	public float degrees = 30f;

	UnityEngine.Vector3 mLastPos;
	UnityEngine.Transform mTrans;
	float mAngle = 0f;

	void OnEnable ()
	{
		mTrans = transform;
		mLastPos = mTrans.position;
	}

	void Update ()
	{
		UnityEngine.Vector3 deltaPos = mTrans.position - mLastPos;
		mLastPos = mTrans.position;

		mAngle += deltaPos.x * degrees;
		mAngle = NGUIMath.SpringLerp(mAngle, 0f, 20f, Time.deltaTime);

		mTrans.localRotation = UnityEngine.Quaternion.Euler(0f, 0f, -mAngle);
	}
}
