using UnityEngine;
using System.Collections;

public class SkillUnlockEffect : UnityEngine.MonoBehaviour
{

    public UnityEngine.GameObject unlockEffect1;
    public float time1;
    public float time2;
    public UISprite us;
    // Use this for initialization
    void Start()
    {
        try
        {
            PlayEffectStep1();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    //播放特效
    void PlayEffectStep1()
    {
        UnityEngine.GameObject go = ArkCrossEngine.ResourceSystem.NewObject(unlockEffect1) as UnityEngine.GameObject;
        go.transform.position = new UnityEngine.Vector3(transform.position.x, transform.position.y, transform.position.z);
        us.spriteName = "wei-jie-suo-ji-neng1";
        Invoke("PlayEffectStep2", time1);
    }
    //播放特效
    void PlayEffectStep2()
    {
        us.spriteName = "wei-jie-suo-ji-neng2";
        Invoke("ClearEffect", time2);
    }
    //清除特效
    void ClearEffect()
    {
        us.spriteName = "";
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("skill_unlock_effect", "effect");
    }
}
