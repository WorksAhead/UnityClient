using UnityEngine;

[System.Serializable]
public class StepAnimInfo
{
    public UnityEngine.AudioClip m_StepSound;
    public string m_AnimName;
    public float[] m_StepTimes;
}

public class StepSound : UnityEngine.MonoBehaviour
{
    public UnityEngine.AudioSource m_AudioSource;
    public StepAnimInfo[] m_StepAnimInfos;

    public bool m_IsDebug = false;
    public int m_DebugIndex = 0;
    public float m_AnimSpeed = 1;

    private string m_DebugAnimName;
    // Use this for initialization
    void Start()
    {
        try
        {
            foreach (StepAnimInfo info in m_StepAnimInfos)
            {
                AnimationClip animclip = GetComponent<Animation>()[info.m_AnimName].clip;
                if (animclip == null)
                {
                    continue;
                }
                foreach (float time in info.m_StepTimes)
                {
                    AnimationEvent ae = new AnimationEvent();
                    ae.time = time;
                    ae.functionName = "PlayStepSound";
                    ae.objectReferenceParameter = info.m_StepSound;
                    animclip.AddEvent(ae);
                }
            }
            if (m_IsDebug && m_DebugIndex >= 0 && m_DebugIndex < m_StepAnimInfos.Length)
            {
                StepAnimInfo debuginfo = m_StepAnimInfos[m_DebugIndex];
                m_DebugAnimName = debuginfo.m_AnimName;
                GetComponent<Animation>()[debuginfo.m_AnimName].speed = m_AnimSpeed;
                GetComponent<Animation>().Play(debuginfo.m_AnimName);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void Update()
    {
        try
        {
            if (m_IsDebug)
            {
                Debug.Log("anim-time=" + GetComponent<Animation>()[m_DebugAnimName].time);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void PlayStepSound(UnityEngine.AudioClip stepsound)
    {
        if (m_AudioSource != null)
        {
            m_AudioSource.PlayOneShot(stepsound);
        }
    }
}