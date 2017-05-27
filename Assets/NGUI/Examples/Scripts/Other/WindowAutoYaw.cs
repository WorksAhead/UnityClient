using UnityEngine;

/// <summary>
/// Attaching this script to an object will make it turn as it gets closer to left/right edges of the screen.
/// Look at how it's used in Example 6.
/// </summary>

[AddComponentMenu("NGUI/Examples/Window Auto-Yaw")]
public class WindowAutoYaw : UnityEngine.MonoBehaviour
{
	public int updateOrder = 0;
	public UnityEngine.Camera uiCamera;
	public float yawAmount = 20f;

	UnityEngine.Transform mTrans;

	void OnDisable ()
	{
		mTrans.localRotation = UnityEngine.Quaternion.identity;
	}

	void OnEnable ()
	{
		if (uiCamera == null) uiCamera = NGUITools.FindCameraForLayer(gameObject.layer);
		mTrans = transform;
	}

	void Update ()
	{
		if (uiCamera != null)
		{
			UnityEngine.Vector3 pos = uiCamera.WorldToViewportPoint(mTrans.position);
			mTrans.localRotation = UnityEngine.Quaternion.Euler(0f, (pos.x * 2f - 1f) * yawAmount, 0f);
		}
	}
}
