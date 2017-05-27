using UnityEngine;

[AddComponentMenu("NGUI/Examples/Load Level On Click")]
public class LoadLevelOnClick : UnityEngine.MonoBehaviour
{
	public string levelName;

	void OnClick ()
	{
		if (!string.IsNullOrEmpty(levelName))
		{
			UnityEngine.Application.LoadLevel(levelName);
		}
	}
}