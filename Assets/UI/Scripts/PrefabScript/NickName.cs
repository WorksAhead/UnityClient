using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NickName : UnityEngine.MonoBehaviour
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
                eventlist.Clear();
            }
            NGUITools.DestroyImmediate(gameObject);
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Use this for initialization
    void Start()
    {
    }
    public void StartForShow()
    {
        if (eventlist != null) { eventlist.Clear(); }
        object eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (eo != null) eventlist.Add(eo);
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            if (needHide)
            {
                if (nicklabel != null && playergo != null && characterContr != null)
                {
                    if (playergo.activeSelf == false)
                    {
                        nicklabel.enabled = false;
                    }
                    else
                    {
                        nicklabel.enabled = true;
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void LateUpdate()
    {
        try
        {
            if (playergo != null && UnityEngine.Camera.main != null)
            {
                UnityEngine.Vector3 pos = playergo.transform.position;
                pos = UnityEngine.Camera.main.WorldToScreenPoint(new UnityEngine.Vector3(pos.x, pos.y + height, pos.z));
                pos.z = 0;
                pos = UICamera.mainCamera.ScreenToWorldPoint(pos);
                gameObject.transform.position = pos;
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void SetPlayerGameObjectAndNickName(UnityEngine.GameObject go, string nickname, UnityEngine.Color col, bool _needHide = false)
    {
        needHide = _needHide;
        characterContr = go.GetComponent<CharacterController>();
        playergo = go;

        UILabel ul = gameObject.GetComponent<UILabel>();
        if (ul != null && nickname != null)
        {
            ul.text = nickname;
            ul.color = col;
            nicklabel = ul;
        }

        Update();
        LateUpdate();
    }
    public void Reset()
    {
        characterContr = null;
        playergo = null;
        nicklabel = null;
        if (eventlist != null)
        {
            foreach (object eo in eventlist)
            {
                if (eo != null)
                {
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eo);
                }
            }
            eventlist.Clear();
        }
    }
    private CharacterController characterContr = null;
    private UnityEngine.GameObject playergo = null;
    private float height = 2.5f;
    private UILabel nicklabel = null;
    private bool needHide = false;
}
