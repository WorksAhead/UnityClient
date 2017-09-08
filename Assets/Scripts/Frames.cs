using UnityEngine;

public class Frames : UnityEngine.MonoBehaviour
{
    public GUIText m_Target;
    public float m_UpdateInterval = 0.5f;

    // Use this for initialization
    internal void Start()
    {
        try
        {
            if (!m_Target)
            {
                enabled = false;
            }
            else
            {
                m_TimeLeft = m_UpdateInterval;
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    internal void Update()
    {
        try
        {
            m_TimeLeft -= Time.deltaTime;
            m_Accum += Time.timeScale / Time.deltaTime;
            ++m_Frames;

            if (m_TimeLeft <= 0)
            {
                m_Target.text = "" + (m_Accum / m_Frames);
                m_TimeLeft = m_UpdateInterval;
                m_Accum = 0;
                m_Frames = 0;
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private float m_Accum = 0;
    private float m_Frames = 0;
    private float m_TimeLeft = 0;
}
