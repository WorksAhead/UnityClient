using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHuaWeiEffect : MonoBehaviour {

    private GameObject m_effect;

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

    // Use this for initialization
    void Start () {
        m_effect = FindObject1(gameObject, "buff_jiasu（pbrdemo）");
    }
	
	// Update is called once per frame
	void Update () {
        if (m_effect != null)
        {
            if ((!ArkCrossEngine.DelayManager.IsDelayEnabled) && (ArkCrossEngine.LogicSystem.PlayerSelf == gameObject))
            {
                m_effect.SetActive(true);
            }
            else
            {
                m_effect.SetActive(false);
            }
        }
    }
}
