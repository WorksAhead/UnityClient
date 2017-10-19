using System.Collections.Generic;
using ArkCrossEngine;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class EditorIndicator_NPC : UnityEngine.MonoBehaviour
{
    public int LinkId = -1;
    public int CampId = 1;
    public string IdleAnimSet = "";
    public int AILogic = 0;

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

        if (LoadedPrefabId != LinkId)
        {
            FileReaderProxy.MakeSureAllHandlerRegistered();

            NpcConfigProvider.Instance.Clear();
            NpcConfigProvider.Instance.LoadNpcConfig(Application.dataPath + "/StreamingAssets/Public/NpcConfig.txt", "");
            Data_NpcConfig config = NpcConfigProvider.Instance.GetNpcConfigById(LinkId);
            if (config != null)
            {
                GameObject.DestroyImmediate(LoadedPrefab);

                // create new one
                Object obj = Resources.Load(config.m_Model);
                LoadedPrefab = GameObject.Instantiate(obj, gameObject.transform) as GameObject;

                LoadedPrefabId = LinkId;
            }
        }
#endif
    }
}