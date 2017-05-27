using UnityEngine;
using System.Collections;
using ArkCrossEngine;

public class Quit : UnityEngine.MonoBehaviour
{
    // Use this for initialization
    internal void Start()
    {
        if (WorldSystem.Instance.IsPvpScene() || //pvp不显示退出
            WorldSystem.Instance.IsPvapScene() /*||//伙伴pvp
        WorldSystem.Instance.GetCurSceneId() == 1001 || //序章
        WorldSystem.Instance.GetCurSceneId() == 1002*/)
        {
            NGUITools.SetActive(gameObject, false);
        }
    }
    // Update is called once per frame
    internal void Update()
    {

    }

    public void OnClickQuit()
    {
        int desId = 8;
        if (WorldSystem.Instance.IsMultiPveScene())
        {
            desId = 878;
        }
        LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", Dict.Get(desId), null, Dict.Get(4), Dict.Get(9), (MyAction<int>)((int btn) =>
        {
            if (btn == 1)
            {
                if (WorldSystem.Instance.IsMultiPveScene())
                    LogicSystem.PublishLogicEvent("ge_quit_battle", "lobby", true);
                else
                    LogicSystem.PublishLogicEvent("ge_quit_battle", "lobby", false);
            }
        }), false);
    }
}
