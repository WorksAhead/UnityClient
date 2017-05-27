using UnityEngine;

/// <summary>
/// Simple script used by Tutorial 11 that sets the color of the sprite based on the string value.
/// </summary>

[ExecuteInEditMode]
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/Examples/Set UnityEngine.Color on Selection")]
public class SetColorOnSelection : UnityEngine.MonoBehaviour
{
	UIWidget mWidget;

	public void SetSpriteBySelection ()
	{
		if (UIPopupList.current == null) return;
		if (mWidget == null) mWidget = GetComponent<UIWidget>();

		switch (UIPopupList.current.value)
		{
			case "White":	mWidget.color = UnityEngine.Color.white;	break;
			case "Red":		mWidget.color = UnityEngine.Color.red;		break;
			case "Green":	mWidget.color = UnityEngine.Color.green;	break;
			case "Blue":	mWidget.color = UnityEngine.Color.blue;		break;
			case "Yellow":	mWidget.color = UnityEngine.Color.yellow;	break;
			case "Cyan":	mWidget.color = UnityEngine.Color.cyan;		break;
			case "Magenta": mWidget.color = UnityEngine.Color.magenta;	break;
		}
	}
}
