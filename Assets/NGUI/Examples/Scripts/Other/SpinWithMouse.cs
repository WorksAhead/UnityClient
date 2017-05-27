using UnityEngine;

[AddComponentMenu("NGUI/Examples/Spin With Mouse")]
public class SpinWithMouse : UnityEngine.MonoBehaviour
{
	public UnityEngine.Transform target;
	public float speed = 1f;

	UnityEngine.Transform mTrans;

	void Start ()
	{
		mTrans = transform;
	}

	void OnDrag (UnityEngine.Vector2 delta)
	{
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;

		if (target != null)
		{
			target.localRotation = UnityEngine.Quaternion.Euler(0f, -0.5f * delta.x * speed, 0f) * target.localRotation;
		}
		else
		{
			mTrans.localRotation = UnityEngine.Quaternion.Euler(0f, -0.5f * delta.x * speed, 0f) * mTrans.localRotation;
		}
	}
}