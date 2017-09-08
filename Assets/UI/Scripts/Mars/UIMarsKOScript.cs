using System;
using System.Collections.Generic;
public class UIMarsKOScript : UnityEngine.MonoBehaviour
{

    private List<object> eventlist = new List<object>();
    public void UnSubscribe()
    {
        try
        {
            if (eventlist != null)
            {
                for (int i = 0; i < eventlist.Count; i++)
                {
                    if (eventlist[i] != null)
                    {
                        ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eventlist[i]);
                    }
                }
                /*
        foreach (object eo in eventlist) {
          if (eo != null) {
            ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eo);
          }
        }*/
            }
            eventlist.Clear();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Use this for initialization
    void Awake()
    {
        object eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (eo != null) eventlist.Add(eo);
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ko_animation", "ui", PlayKOAnimation);
        if (eo != null) eventlist.Add(eo);
    }

    // Update is called once per frame
    void Update()
    {

    }
    //播放KO动画
    public void PlayKOAnimation()
    {
        UIManager.Instance.ShowWindowByName("MarsKO");
    }
}
