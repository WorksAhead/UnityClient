using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArkCrossEngine;

public class DayNightSwitch : MonoBehaviour
{

    private LevelLightmapData LocalLevelLightmapData;
    private int LightingScenarioSelector;
    private int lightingScenariosCount;
    private GameObject DayLight;
    private GameObject NightLight;
    private ReflectionProbe GlobalReflectionProbe;
    private float ReflectionProbeIntensity;
    private bool IsNight = true;

    // Use this for initialization
    void Start()
    {
        LocalLevelLightmapData = FindObjectOfType<LevelLightmapData>();
        LightingScenarioSelector = 0;
        lightingScenariosCount = LocalLevelLightmapData.lightingScenariosCount;
        LocalLevelLightmapData.LoadLightingScenario(0);
        Debug.Log("Load default lighting scenario");

        var parent = GameObject.Find("UPBR_Lights");
        DayLight = FindObject1(parent, "sun");
        NightLight = FindObject1(parent, "nightLight");
        GlobalReflectionProbe = FindObject1(parent, "Reflection Probe").GetComponent<ReflectionProbe>();
        ReflectionProbeIntensity = GlobalReflectionProbe.intensity;

        DayLight.SetActive(true);
        NightLight.SetActive(false);
        GlobalReflectionProbe.RenderProbe();
    }

    public static GameObject FindObject1(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }

    void OnTriggerEnter(Collider collider)
    {
        UnityEngine.GameObject obj = collider.gameObject;
        if (null != obj)
        {
            if (obj == LogicSystem.PlayerSelf)
            {
                if (!IsNight)
                {
                    LocalLevelLightmapData.LoadLightingScenario(0);
                    Debug.Log("Switch to DayLight");
                    DayLight.SetActive(true);
                    NightLight.SetActive(false);
                    //    GlobalReflectionProbe.intensity = ReflectionProbeIntensity;
                    GlobalReflectionProbe.RenderProbe();
                    IsNight = true;
                }
                else
                {
                    LocalLevelLightmapData.LoadLightingScenario(1);
                    Debug.Log("Switch to NightLight");
                    DayLight.SetActive(false);
                    NightLight.SetActive(true);
                    //    GlobalReflectionProbe.intensity = ReflectionProbeIntensity / 2.0f;
                    GlobalReflectionProbe.RenderProbe();
                    IsNight = false;
                }
            }
        }
    }
}

