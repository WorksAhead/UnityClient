using System.Collections.Generic;
using ArkCrossEngine;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class EditorIndicator_RevivePoint : UnityEngine.MonoBehaviour
{
    private GameObject LoadedPrefab;

    void Update()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            return;
        }

        if (LoadedPrefab == null)
        {
            int count = this.gameObject.transform.childCount;
            for (int i = 0; i < count; ++i)
            {
                GameObject.DestroyImmediate(gameObject.transform.GetChild(i).gameObject);
            }

            Object obj = Resources.Load("BlueCylinder");
            LoadedPrefab = GameObject.Instantiate(obj, gameObject.transform) as GameObject;
        }
#endif
    }
}