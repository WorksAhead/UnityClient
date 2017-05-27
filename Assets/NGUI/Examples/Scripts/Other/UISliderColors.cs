//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// This script automatically changes the color of the specified sprite based on the value of the slider.
/// </summary>

[RequireComponent(typeof(UIProgressBar))]
[AddComponentMenu("NGUI/Examples/Slider Colors")]
public class UISliderColors : UnityEngine.MonoBehaviour
{
	public UISprite sprite;

	public UnityEngine.Color[] colors = new UnityEngine.Color[] { UnityEngine.Color.red, UnityEngine.Color.yellow, UnityEngine.Color.green };

	UIProgressBar mBar;

	void Start () { mBar = GetComponent<UIProgressBar>(); Update(); }

	void Update ()
	{
		if (sprite == null || colors.Length == 0) return;

		float val = mBar.value;
		val *= (colors.Length - 1);
		int startIndex = UnityEngine.Mathf.FloorToInt(val);

		UnityEngine.Color c = colors[0];

		if (startIndex >= 0)
		{
			if (startIndex + 1 < colors.Length)
			{
				float factor = (val - startIndex);
				c = UnityEngine.Color.Lerp(colors[startIndex], colors[startIndex + 1], factor);
			}
			else if (startIndex < colors.Length)
			{
				c = colors[startIndex];
			}
			else c = colors[colors.Length - 1];
		}

		c.a = sprite.color.a;
		sprite.color = c;
	}
}
