using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingScenarioSwitcher : MonoBehaviour {

    public LevelLightmapData LocalLevelLightmapData;
    private int LightingScenarioSelector;
    private int lightingScenariosCount;
    public GameObject DayLight;
    public GameObject NightLight;
    public ReflectionProbe GlobalReflectionProbe;
    public float ReflectionProbeIntensity;
    [SerializeField]
    public int DefaultLightingScenario;

    // Use this for initialization
    void Start ()
    {
        LocalLevelLightmapData = FindObjectOfType<LevelLightmapData>();
        LightingScenarioSelector = DefaultLightingScenario;
        lightingScenariosCount = LocalLevelLightmapData.lightingScenariosCount;
        LocalLevelLightmapData.LoadLightingScenario(DefaultLightingScenario);
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
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            LocalLevelLightmapData.LoadLightingScenario(0);
            Debug.Log("Switch to DayLight");
            DayLight.SetActive(true);
            NightLight.SetActive(false);
        //    GlobalReflectionProbe.intensity = ReflectionProbeIntensity;
            GlobalReflectionProbe.RenderProbe();
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            LocalLevelLightmapData.LoadLightingScenario(1);
            Debug.Log("Switch to NightLight");
            DayLight.SetActive(false);
            NightLight.SetActive(true);
        //    GlobalReflectionProbe.intensity = ReflectionProbeIntensity / 2.0f;
            GlobalReflectionProbe.RenderProbe();
        }
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
}
