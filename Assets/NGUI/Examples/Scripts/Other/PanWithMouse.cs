using UnityEngine;

/// <summary>
/// Placing this script on the game object will make that game object pan with mouse movement.
/// </summary>

[AddComponentMenu("NGUI/Examples/Pan With Mouse")]
public class PanWithMouse : UnityEngine.MonoBehaviour
{
	public UnityEngine.Vector2 degrees = new UnityEngine.Vector2(5f, 3f);
	public float range = 1f;

	UnityEngine.Transform mTrans;
	UnityEngine.Quaternion mStart;
	UnityEngine.Vector2 mRot = UnityEngine.Vector2.zero;

	void Start ()
	{
		mTrans = transform;
		mStart = mTrans.localRotation;
	}

	void Update ()
	{
		float delta = RealTime.deltaTime;
		UnityEngine.Vector3 pos = Input.mousePosition;

		float halfWidth = Screen.width * 0.5f;
		float halfHeight = Screen.height * 0.5f;
		if (range < 0.1f) range = 0.1f;
		float x = UnityEngine.Mathf.Clamp((pos.x - halfWidth) / halfWidth / range, -1f, 1f);
		float y = UnityEngine.Mathf.Clamp((pos.y - halfHeight) / halfHeight / range, -1f, 1f);
		mRot = UnityEngine.Vector2.Lerp(mRot, new UnityEngine.Vector2(x, y), delta * 5f);

		mTrans.localRotation = mStart * UnityEngine.Quaternion.Euler(-mRot.y * degrees.y, mRot.x * degrees.x, 0f);
	}
}
