using UnityEngine;
using ArkCrossEngine;

public class CharacterCamp : UnityEngine.MonoBehaviour
{
    public int m_CampId;
    [HideInInspector]
    public bool m_IsEndure = false; // 是否霸体。
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

    public void SetEndure(bool isEndure)
    {
        m_IsEndure = isEndure;
    }

    public void Start()
    {
        m_effect = FindObject1(gameObject, "buff_jiasu（pbrdemo）");
    }

    public void Update()
    {
        if (m_effect != null)
        {
            m_effect.SetActive(!DelayManager.IsDelayEnabled);
        }
    }
}