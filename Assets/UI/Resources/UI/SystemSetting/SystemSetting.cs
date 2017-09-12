using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSetting : MonoBehaviour {

    private Shader standardShader;
    private Shader standardSpecShader;

    const int SHADER_LOD_HIGH = 300;
    const int SHADER_LOD_MEDIUM = 200;
    const int SHADER_LOD_LOW = 150;

    // Use this for initialization
    void Start()
    {
        standardShader = Shader.Find("CY/Standard(Custom)");
        standardSpecShader = Shader.Find("CY/Standard (Specular setup)(Custom)");
        // 默认运行高配shader
        standardShader.maximumLOD = SHADER_LOD_HIGH;
        standardSpecShader.maximumLOD = SHADER_LOD_HIGH;
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
