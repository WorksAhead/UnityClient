using UnityEngine;

[AddComponentMenu("NGUI/Examples/Item Attachment Point")]
public class InvAttachmentPoint : UnityEngine.MonoBehaviour
{
	/// <summary>
	/// Item slot that this attachment point covers.
	/// </summary>

	public InvBaseItem.Slot slot;

	UnityEngine.GameObject mPrefab;
	UnityEngine.GameObject mChild;

	/// <summary>
	/// Attach an instance of the specified game object.
	/// </summary>

	public UnityEngine.GameObject Attach (UnityEngine.GameObject prefab)
	{
		if (mPrefab != prefab)
		{
			mPrefab = prefab;

			// Remove the previous child
			if (mChild != null) Destroy(mChild);

			// If we have something to create, let's do so now
			if (mPrefab != null)
			{
				// Create a new instance of the game object
				UnityEngine.Transform t = transform;
				mChild = Instantiate(mPrefab, t.position, t.rotation) as UnityEngine.GameObject;

				// Parent the child to this object
				UnityEngine.Transform ct = mChild.transform;
				ct.parent = t;

				// Reset the pos/rot/scale, just in case
				ct.localPosition = UnityEngine.Vector3.zero;
				ct.localRotation = UnityEngine.Quaternion.identity;
				ct.localScale = UnityEngine.Vector3.one;
			}
		}
		return mChild;
	}
}