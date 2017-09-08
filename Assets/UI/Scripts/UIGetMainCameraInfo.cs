using UnityEngine;
using System.Collections;
/// <summary>
/// 用于获取主相机信息并保存下来，再次返回该场景时重置相机位置
/// </summary>
public class UIGetMainCameraInfo : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        try
        {
            if (!UIDataCache.Instance.m_IsSceneCameraInit)
            {
                UIDataCache.Instance.MainSceneCameralocalEulerAngles = this.transform.localEulerAngles;
                UIDataCache.Instance.MainSceneCameraPos = this.transform.localPosition;
                UIDataCache.Instance.m_IsSceneCameraInit = true;
            }
            else
            {
                this.transform.localPosition = UIDataCache.Instance.MainSceneCameraPos;
                this.transform.localEulerAngles = UIDataCache.Instance.MainSceneCameralocalEulerAngles;
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}
