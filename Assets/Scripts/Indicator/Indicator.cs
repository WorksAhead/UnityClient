using System.Collections.Generic;
using ArkCrossEngine;
using UnityEngine;

public class Indicator : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    private float m_Dir;
    private UnityEngine.GameObject m_Owner;
    private IndicatorType m_IndicatorTargetType = IndicatorType.NPC;
    private bool m_IndicatorTypeChanged = true;
    private string[] m_RoadSign = { "ADoor", "BDoor", "CDoor", "DDoor" };
    public string[] m_RoadSignPlus = { "AtoB", "BtoC" };
    private List<string> m_TriggeredSign = new List<string>();
    private List<UnityEngine.GameObject> m_RoadSignObject = new List<UnityEngine.GameObject>();
    private bool m_HideIndicator = false;
    private bool m_HideIndicatorByStory = false;
    public float m_InvisibleDis = 3.0f;
    public UnityEngine.GameObject m_DoorEffect;
    public UnityEngine.GameObject m_MonEffect;
    private UnityEngine.GameObject m_CurEffect;
    private List<object> m_EventList = new List<object>();

    private enum IndicatorType
    {
        ROAD_SING = 0,
        NPC = 1,
        NONE = 2,
    }
    void Start()
    {
        try
        {
            m_EventList.Clear();
            m_EventList.Add(LogicSystem.EventChannelForGfx.Subscribe("ge_set_indicator_visible", "indicator", SetIndicatorVisible));
            m_EventList.Add(LogicSystem.EventChannelForGfx.Subscribe("ge_set_indicator_invisible", "indicator", SetIndicatorInvisible));
            m_TriggeredSign.Clear();
            for (int i = 0; i < m_RoadSign.Length; ++i)
            {
                UnityEngine.GameObject roadSign = UnityEngine.GameObject.Find(m_RoadSign[i]);
                if (null != roadSign)
                {
                    m_RoadSignObject.Add(roadSign);
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (null != m_Owner)
            {
                SharedGameObjectInfo shareInfo = LogicSystem.GetSharedGameObjectInfo(m_Owner);
                if (null != shareInfo)
                {
                    m_HideIndicator = !shareInfo.IsIndicatorVisible;
                    m_Dir = shareInfo.IndicatorDir;
                    SetIndicatorTarget(shareInfo.IndicatorType);
                }
                if (m_HideIndicator || m_HideIndicatorByStory)
                {
                    this.transform.localPosition = UnityEngine.Vector3.zero;
                }
                else
                {
                    this.transform.localPosition = new UnityEngine.Vector3(m_Owner.transform.position.x, m_Owner.transform.position.y, m_Owner.transform.position.z) + new UnityEngine.Vector3(0, 0.3f, 0);
                }
                if (m_IndicatorTypeChanged)
                {
                    if (m_IndicatorTargetType == IndicatorType.NPC)
                    {
                        SetVisible(m_DoorEffect, false);
                        SetVisible(m_MonEffect, true);
                        m_CurEffect = m_MonEffect;
                    }
                    else if (m_IndicatorTargetType == IndicatorType.ROAD_SING)
                    {
                        SetVisible(m_DoorEffect, true);
                        SetVisible(m_MonEffect, false);
                        m_CurEffect = m_DoorEffect;
                    }
                    else
                    {
                        SetVisible(m_DoorEffect, false);
                        SetVisible(m_MonEffect, false);
                    }
                    m_IndicatorTypeChanged = false;
                }
            }
            if (IndicatorType.NPC == m_IndicatorTargetType)
            {
                this.transform.localRotation = UnityEngine.Quaternion.Euler(0, LogicSystem.RadianToDegree(m_Dir), 0);
            }
            else if (IndicatorType.ROAD_SING == m_IndicatorTargetType)
            {
                UnityEngine.GameObject roadSign = GetRoadSign();
                if (null != roadSign)
                {
                    UnityEngine.Vector3 scrPos = this.transform.position;
                    UnityEngine.Vector3 tarPos = roadSign.transform.position;
                    if (UnityEngine.Vector2.Distance(new UnityEngine.Vector2(scrPos.x, scrPos.z), new UnityEngine.Vector2(tarPos.x, tarPos.z)) < m_InvisibleDis)
                    {
                        SetVisible(gameObject, false);
                    }
                    else
                    {
                        SetVisible(m_CurEffect, true);
                        UnityEngine.Vector3 dir = roadSign.transform.position - this.transform.position;
                        dir.y = 0;
                        this.transform.localRotation = UnityEngine.Quaternion.LookRotation(dir, UnityEngine.Vector3.up);
                    }
                }
                else
                {
                    SetVisible(gameObject, false);
                }
                for (int i = 0; i < m_RoadSignObject.Count; i++)
                {
                    UnityEngine.BoxCollider bc = m_RoadSignObject[i].GetComponent<UnityEngine.BoxCollider>();
                    if (null != bc)
                    {
                        if (bc.bounds.Contains(new UnityEngine.Vector3(m_Owner.transform.position.x, m_Owner.transform.position.y, m_Owner.transform.position.z) + new UnityEngine.Vector3(0, 1, 0)))
                        {
                            if (!m_TriggeredSign.Contains(m_RoadSignObject[i].name))
                            {
                                m_TriggeredSign.Add(m_RoadSignObject[i].name);
                            }
                        }
                    }
                }
            }
            /*
            foreach (UnityEngine.GameObject roadSign in m_RoadSignObject) {
              UnityEngine.BoxCollider bc = roadSign.GetComponent<UnityEngine.BoxCollider>();
              if (null != bc) {
                if (bc.bounds.Contains(this.transform.position + new UnityEngine.Vector3(0, 1, 0))) {
                  m_TriggeredSign.Add(roadSign.name);
                }
              }
            }*/
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void OnDestroy()
    {
        try
        {
            for (int i = 0; i < m_EventList.Count; i++)
            {
                LogicSystem.EventChannelForGfx.Unsubscribe(m_EventList[i]);
            }
            /*
            foreach (object o in m_EventList) {
              LogicSystem.EventChannelForGfx.Unsubscribe(o);
            }*/
            m_EventList.Clear();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void SetVisible(UnityEngine.GameObject go, bool visible)
    {
        UnityEngine.Renderer[] renderers = go.GetComponentsInChildren<UnityEngine.Renderer>();
        for (int i = 0; i < renderers.Length; ++i)
        {
            renderers[i].enabled = visible;
        }
    }
    private UnityEngine.GameObject GetRoadSign()
    {
        for (int i = 0; i < m_RoadSignObject.Count; ++i)
        {
            UnityEngine.GameObject roadSign = m_RoadSignObject[i];
            if (null != roadSign)
            {
                UnityEngine.BoxCollider bc = roadSign.GetComponent<UnityEngine.BoxCollider>();
                if (null != bc && bc.isTrigger && !m_TriggeredSign.Contains(roadSign.name))
                {
                    return roadSign;
                }
            }
        }
        return null;
    }

    public void SetIndicatorDir(float dir)
    {
        m_Dir = dir;
    }
    public void SetOwner(int id)
    {
        m_Owner = LogicSystem.GetGameObject(id);
    }
    public void SetIndicatorTarget(int targetType)
    {
        if (m_IndicatorTargetType != (IndicatorType)targetType)
        {
            m_IndicatorTargetType = (IndicatorType)targetType;
            m_IndicatorTypeChanged = true;
        }
    }
    private void SetIndicatorVisible()
    {
        m_HideIndicatorByStory = false;
        LogSystem.Debug("SetIndicatorVisible");
    }
    private void SetIndicatorInvisible()
    {
        m_HideIndicatorByStory = true;
        LogSystem.Debug("SetIndicatorInvisible");
    }
}
