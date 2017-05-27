using UnityEngine;
using System.Collections;

public class SceneUnlockEffect : UnityEngine.MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Invoke("ClearEffect", 2.3f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    //清除特效
    void ClearEffect()
    {
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("guanqia_unlock_effect", "effect");
    }
}
