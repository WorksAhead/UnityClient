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
            Object obj = Resources.Load("BlueCylinder");
            LoadedPrefab = GameObject.Instantiate(obj, gameObject.transform) as GameObject;
        }
#endif
    }
}