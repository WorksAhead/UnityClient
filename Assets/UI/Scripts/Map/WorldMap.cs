using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WorldMap : UnityEngine.MonoBehaviour
{

    public UnityEngine.GameObject goCitys = null;

    private List<UnityEngine.GameObject> citys = new List<UnityEngine.GameObject>();
    // Use this for initialization
    void Start()
    {
        try
        {
            if (citys != null)
            {
                foreach (UnityEngine.GameObject go in citys)
                {
                    UnityEngine.Transform tfNow = go.transform.Find("now");
                    if (ArkCrossEngine.WorldSystem.Instance.GetCurSceneId().ToString().Equals(go.name))
                    {
                        NGUITools.SetActive(tfNow.gameObject, true);
                    }
                    else
                    {
                        NGUITools.SetActive(tfNow.gameObject, false);
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        try
        {
            if (goCitys != null)
            {
                int childNum = goCitys.transform.childCount;
                citys.Clear();
                for (int i = 0; i < childNum; i++)
                {
                    UnityEngine.GameObject go = goCitys.transform.GetChild(i).gameObject;
                    citys.Add(go);
                    UIEventListener.Get(go).onClick += this.OnButtonClick;
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void OnButtonClick(UnityEngine.GameObject go)
    {
        if (go != null)
        {
            try
            {
                int sceneId = Int32.Parse(go.name);
                UIManager.Instance.ShowWindowByName("Maptips");
                UnityEngine.GameObject tip = UIManager.Instance.GetWindowGoByName("Maptips");
                if (tip != null)
                {
                    MapTip script = tip.GetComponent<MapTip>();
                    if (script != null)
                        script.UpdateView(sceneId);
                }
                //ArkCrossEngine.Data_SceneConfig config = ArkCrossEngine.SceneConfigProvider.Instance.GetSceneConfigById(Int32.Parse(go.name));
                //if (config != null) {
                //  ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_change_scene", "game", Int32.Parse(go.name));
                //} else {
                //  string chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(31);
                //  ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
                //}
            }
            catch (Exception e)
            {
                Debug.Log("[worldmap] wrong name");
                //string chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(31);
                //ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
            }
        }

    }

    public void OnCloseClick()
    {
        UIManager.Instance.HideWindowByName("WorldMap");
    }
}
