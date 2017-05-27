using UnityEngine;

/// <summary>
/// Want something to spin? Attach this script to it. Works equally well with rigidbodies as without.
/// </summary>

[AddComponentMenu("NGUI/Examples/Spin")]
public class Spin : UnityEngine.MonoBehaviour
{
	public UnityEngine.Vector3 rotationsPerSecond = new UnityEngine.Vector3(0f, 0.1f, 0f);
	public bool ignoreTimeScale = false;

	Rigidbody mRb;
	UnityEngine.Transform mTrans;

	void Start ()
	{
		mTrans = transform;
		mRb = GetComponent<Rigidbody>();
	}

	void Update ()
	{
		if (mRb == null)
		{
			ApplyDelta(ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime);
		}
	}

	void FixedUpdate ()
	{
		if (mRb != null)
		{
			ApplyDelta(Time.deltaTime);
		}
	}

	public void ApplyDelta (float delta)
	{
		delta *= UnityEngine.Mathf.Rad2Deg * UnityEngine.Mathf.PI * 2f;
		UnityEngine.Quaternion offset = UnityEngine.Quaternion.Euler(rotationsPerSecond * delta);

		if (mRb == null)
		{
			mTrans.rotation = mTrans.rotation * offset;
		}
		else
		{
			mRb.MoveRotation(mRb.rotation * offset);
		}
	}
}
