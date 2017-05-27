using UnityEngine;

[RequireComponent(typeof(UnityEngine.BoxCollider))]
class AirWallSwitch : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject m_DistroyEffect;
    private UnityEngine.BoxCollider m_BoxCollider;

    private void Start()
    {
        try
        {
            m_BoxCollider = gameObject.GetComponent<UnityEngine.BoxCollider>();
            if (!m_BoxCollider.isTrigger)
            {
                EnableParticlas();
            }
            else
            {
                DisableParticals();
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void OpenDoor()
    {
        m_BoxCollider.isTrigger = false;
        EnableParticlas();
    }

    public void CloseDoor()
    {
        m_BoxCollider.isTrigger = true;
        DisableParticals();
    }

    private void EnableParticlas()
    {
        ParticleSystem[] pss = gameObject.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < pss.Length; ++i)
        {
            if (pss[i] != null)
            {
                pss[i].Play();
            }
        }
    }
    private void DisableParticals()
    {
        ParticleSystem[] pss = gameObject.GetComponentsInChildren<ParticleSystem>();
        /*foreach (ParticleSystem ps in pss) {
          ps.Stop();
        }*/
        for (int i = 0; i < pss.Length; ++i)
        {
            if (pss[i] != null)
            {
                pss[i].Stop();
            }
        }
        if (null != m_DistroyEffect)
        {
            ParticleSystem[] deadPss = m_DistroyEffect.GetComponentsInChildren<ParticleSystem>();
            /*foreach (ParticleSystem ps in deadPss) {
              ps.Play();
            }*/
            for (int i = 0; i < deadPss.Length; ++i)
            {
                if (deadPss[i] != null)
                {
                    deadPss[i].Play();
                }
            }
        }
    }

    private void OnTriggerEnter(UnityEngine.Collider collider)
    {
        try
        {
            //Debug.Log("ontriggerenter");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void OnTriggerExit(UnityEngine.Collider collider)
    {
        try
        {
            //Debug.Log("ontriggerexit");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}
