using System.Collections.Generic;
using ArkCrossEngine;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class EditorIndicator_WayPoint : UnityEngine.MonoBehaviour
{
    public int Id = 0;

    private GameObject LoadedPrefab;
    private int LoadedPrefabId = -1;

    void Awake()
    {
        
    }

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

            // create new one
            Object obj = Resources.Load("Cylinder");
            LoadedPrefab = GameObject.Instantiate(obj, gameObject.transform) as GameObject;
        }
#endif
    }
}