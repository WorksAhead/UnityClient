using UnityEngine;

public class ScreenBloom : UnityEngine.MonoBehaviour
{
    //UnityEngine.Color
    public UnityEngine.Color m_Color;

    public UnityEngine.Shader shader;
    private Material m_Material;
    private bool m_IsActive = false;
    private float m_TotalTime = 0;
    private float m_StartTime = 0;
    private UnityEngine.Color m_StartColor = new UnityEngine.Color();
    private UnityEngine.Color m_EndColor = new UnityEngine.Color();
    //Properties
    protected Material material
    {
        get
        {
            if (m_Material == null)
            {
                m_Material = new Material(shader);
                m_Material.hideFlags = HideFlags.HideAndDontSave;
            }
            return m_Material;
        }
    }
    //Methods
    private void Update()
    {
        try
        {
            if (m_IsActive)
            {
                if (Time.time - m_StartTime < m_TotalTime)
                {
                    m_Color = UnityEngine.Color.Lerp(m_StartColor, m_EndColor, (Time.time - m_StartTime) / m_TotalTime);
                }
                else
                {
                    m_IsActive = false;
                    m_Color = m_EndColor;
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    protected void OnDisable()
    {
        if (m_Material)
        {
            DestroyImmediate(m_Material);
        }
    }


    void OnEnable()
    {
        shader = UnityEngine.Shader.Find("Hidden/DFM/BloomSimple");
    }
    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        try
        {
            material.SetColor("_Color", m_Color);
            Graphics.Blit(source, destination, material);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // message
    void DimScreen(long time)
    {
        m_IsActive = true;
        m_TotalTime = time / 1000.0f;
        m_StartTime = Time.time;
        m_StartColor = UnityEngine.Color.white;
        m_EndColor = UnityEngine.Color.black;
    }
    void LightScreen(long time)
    {
        m_IsActive = true;
        m_TotalTime = time / 1000.0f;
        m_StartTime = Time.time;
        m_StartColor = UnityEngine.Color.black;
        m_EndColor = UnityEngine.Color.white;
    }
}

