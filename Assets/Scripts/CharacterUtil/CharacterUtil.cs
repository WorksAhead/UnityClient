using UnityEngine;
using ArkCrossEngine;

public class CharacterUtil : UnityEngine.MonoBehaviour
{
    public GameObject m_MeetEnemyEffect;
    public string m_MeetEnemyEffectBone;
    public GameObject m_DeadEffect;
    public GameObject m_DeadEffectAsPartner;
    public GameObject m_OnHitGroundEffect;

    public void OnEnable()
    {
        try
        {
            UnityEngine.BoxCollider[] bcs = gameObject.GetComponentsInChildren<UnityEngine.BoxCollider>();
            for (int i = 0; i < bcs.Length; i++)
            {
                bcs[i].isTrigger = false;
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void OnEventMeetEnemy()
    {
        try
        {
            if (null != m_MeetEnemyEffect && !string.IsNullOrEmpty(m_MeetEnemyEffectBone))
            {
                GameObject obj = ResourceSystem.NewObject(m_MeetEnemyEffect, 2.0f) as GameObject;
                Transform parent = LogicSystem.FindChildRecursive(
                    transform,
                    m_MeetEnemyEffectBone);
                if (null != parent)
                {
                    obj.transform.parent = parent;
                    obj.transform.localPosition = UnityEngine.Vector3.zero;
                    obj.transform.localRotation = UnityEngine.Quaternion.identity;
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void OnEventDead(int npcType)
    {
        try
        {
            GameObject deadEffect = m_DeadEffect;
            if (npcType == (int)NpcTypeEnum.Partner)
            {
                if (null != m_DeadEffectAsPartner)
                {
                    deadEffect = m_DeadEffectAsPartner;
                }
            }
            GameObject obj = ResourceSystem.NewObject(deadEffect, 2.0f) as GameObject;
            if (null != obj)
            {
                UnityEngine.Vector3 upos = this.transform.position + new UnityEngine.Vector3(0f, 0.5f, 0.0f);
                obj.transform.position = new UnityEngine.Vector3(upos.x, upos.y, upos.z);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void OnEventEmptyBlood()
    {
        try
        {
            UnityEngine.BoxCollider[] bcs = gameObject.GetComponentsInChildren<UnityEngine.BoxCollider>();
            for (int i = 0; i < bcs.Length; i++)
            {
                bcs[i].isTrigger = true;
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void OnHitGround()
    {
        try
        {
            if (null != m_OnHitGroundEffect)
            {
                GameObject obj = ResourceSystem.NewObject(m_OnHitGroundEffect, 2.0f) as GameObject;
                if (null != obj)
                {
                    obj.transform.position = new UnityEngine.Vector3(transform.position.x, transform.position.y, transform.position.z);
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}
