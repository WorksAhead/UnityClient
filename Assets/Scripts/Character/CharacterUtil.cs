using UnityEngine;
using ArkCrossEngine;

public class CharacterUtil : UnityEngine.MonoBehaviour
{

    public ArkCrossEngine.GameObject m_MeetEnemyEffect;
    public string m_MeetEnemyEffectBone;
    public ArkCrossEngine.GameObject m_DeadEffect;
    public ArkCrossEngine.GameObject m_DeadEffectAsPartner;
    public ArkCrossEngine.GameObject m_OnHitGroundEffect;

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
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void OnEventMeetEnemy()
    {
        try
        {
            if (null != m_MeetEnemyEffect && !string.IsNullOrEmpty(m_MeetEnemyEffectBone))
            {
                ArkCrossEngine.GameObject obj = ResourceSystem.NewObject(m_MeetEnemyEffect, 2.0f) as ArkCrossEngine.GameObject;
                ArkCrossEngine.Transform parent = LogicSystem.FindChildRecursive(
                    ArkCrossEngine.ObjectFactory.Create<ArkCrossEngine.Transform>(transform)/*new ArkCrossEngine.Transform(transform)*/,
                    m_MeetEnemyEffectBone);
                if (null != parent)
                {
                    obj.transform.parent = parent;
                    obj.transform.localPosition = ArkCrossEngine.Vector3.zero;
                    obj.transform.localRotation = ArkCrossEngine.Quaternion.identity;
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void OnEventDead(int npcType)
    {
        try
        {
            ArkCrossEngine.GameObject deadEffect = m_DeadEffect;
            if (npcType == (int)NpcTypeEnum.Partner)
            {
                if (null != m_DeadEffectAsPartner)
                {
                    deadEffect = m_DeadEffectAsPartner;
                }
            }
            ArkCrossEngine.GameObject obj = ResourceSystem.NewObject(deadEffect, 2.0f) as ArkCrossEngine.GameObject;
            if (null != obj)
            {
                UnityEngine.Vector3 upos = this.transform.position + new UnityEngine.Vector3(0f, 0.5f, 0.0f);
                obj.transform.position = new ArkCrossEngine.Vector3(upos.x, upos.y, upos.z);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
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
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void OnHitGround()
    {
        try
        {
            if (null != m_OnHitGroundEffect)
            {
                ArkCrossEngine.GameObject obj = ResourceSystem.NewObject(m_OnHitGroundEffect, 2.0f) as ArkCrossEngine.GameObject;
                if (null != obj)
                {
                    obj.transform.position = new ArkCrossEngine.Vector3(transform.position.x, transform.position.y, transform.position.z);
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}
