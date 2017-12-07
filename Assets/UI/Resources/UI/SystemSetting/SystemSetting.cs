using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class SystemSetting : MonoBehaviour {

    private Shader standardShader;
    private Shader standardSpecShader;
    private RenderingPath m_renderPath;
    private int m_screenWidth, m_screenHeight;
    private bool m_IsHDREnabled;
    private CYBloom m_bloom;
    private CYTonemapping m_tonemapping;
    private QualityLevel m_qualityLevel;

    const int SHADER_LOD_HIGH = 300;
    const int SHADER_LOD_MEDIUM = 200;
    const int SHADER_LOD_LOW = 150;
    
    public enum QualityLevel
    {
        High = 0,
        Medium = 1,
        Low = 2
    }

    // Use this for initialization
    void Start()
    {
        standardShader = Shader.Find("CY/Standard(Custom)");
        standardSpecShader = Shader.Find("CY/Standard (Specular setup)(Custom)");
        // 默认运行高配shader
        standardShader.maximumLOD = SHADER_LOD_LOW;
        standardSpecShader.maximumLOD = SHADER_LOD_LOW;

        m_screenWidth = Screen.currentResolution.width;
        m_screenHeight = Screen.currentResolution.height;
        m_renderPath = RenderingPath.Forward;
        m_IsHDREnabled = true;
        m_bloom = Camera.main.GetComponent<CYBloom>();
        m_tonemapping = Camera.main.GetComponent<CYTonemapping>();

        QualitySettings.SetQualityLevel((int)QualityLevel.Low);
        m_qualityLevel = QualityLevel.Low;

        SetHDREnabled(false);
        int nw = (int)(m_screenWidth * 0.75f);
        int nh = (int)(m_screenHeight * 0.75f);
        Screen.SetResolution(nw, nh, false);
    }

    public void SwitchRenderPath(RenderingPath p)
    {
        Camera.main.renderingPath = p;
        m_renderPath = p;
    }

    public void SetHDREnabled(bool bEnable)
    {
        if (m_bloom != null)
        {
            m_bloom.enabled = bEnable;
        }
        if (m_tonemapping != null)
        {
            m_tonemapping.enabled = bEnable;
        }
        m_IsHDREnabled = bEnable;
    }

    public void SwitchQualityLevelToHigh()
    {
        if (UIToggle.current.value)
        {
            QualitySettings.SetQualityLevel((int)QualityLevel.High);
            m_qualityLevel = QualityLevel.High;

            SetHDREnabled(true);
            Screen.SetResolution(m_screenWidth, m_screenHeight, false);
        }
    }

    public void SwitchQualityLevelToMedium()
    {
        if (UIToggle.current.value)
        {
            QualitySettings.SetQualityLevel((int)QualityLevel.Medium);
            m_qualityLevel = QualityLevel.Medium;

            SetHDREnabled(false);
            Screen.SetResolution(m_screenWidth, m_screenHeight, false);
        }
    }

    public void SwitchQualityLevelToLow()
    {
        if (UIToggle.current.value)
        {
            QualitySettings.SetQualityLevel((int)QualityLevel.Low);
            m_qualityLevel = QualityLevel.Low;

            SetHDREnabled(false);
            int nw = (int)(m_screenWidth * 0.75f);
            int nh = (int)(m_screenHeight * 0.75f);
            Screen.SetResolution(nw, nh, false);
        }
    }

    public void SwitchShaderLODToHigh()
    {
        if (UIToggle.current.value && standardShader)
        {
            standardShader.maximumLOD = SHADER_LOD_HIGH;
            standardSpecShader.maximumLOD = SHADER_LOD_HIGH;
        }
    }

    public void SwitchShaderLODToMedium()
    {
        if (UIToggle.current.value && standardShader)
        {
            standardShader.maximumLOD = SHADER_LOD_MEDIUM;
            standardSpecShader.maximumLOD = SHADER_LOD_MEDIUM;
        }
    }

    public void SwitchShaderLODToLow()
    {
        if (UIToggle.current.value && standardShader)
        {
            standardShader.maximumLOD = SHADER_LOD_LOW;
            standardSpecShader.maximumLOD = SHADER_LOD_LOW;
        }
    }
}
