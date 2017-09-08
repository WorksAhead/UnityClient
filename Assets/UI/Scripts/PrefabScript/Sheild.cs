using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Sheild : UnityEngine.MonoBehaviour
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
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Use this for initialization
    void Start()
    {
        try
        {
            if (eventlist != null) { eventlist.Clear(); }
            object eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (eo != null) eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, float>("ge_update_monster_sheild", "ui", UpdateSheild);
            if (eo != null) eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("ge_end_monster_sheild", "ui", EndSheild);
            if (eo != null) eventlist.Add(eo);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        try
        {
            if (mygameobject != null)
            {
                UnityEngine.Vector3 pos = mygameobject.transform.position;
                pos = UnityEngine.Camera.main.WorldToScreenPoint(new UnityEngine.Vector3(pos.x, pos.y + 2, pos.z));
                pos.z = 0;
                pos = UICamera.mainCamera.ScreenToWorldPoint(pos);
                gameObject.transform.position = pos;
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void UpdateSheild(int actorid, float progress)
    {
        try
        {
            if (myactorid == actorid && myuisprite != null)
            {
                myuisprite.fillAmount = progress;
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void EndSheild(int actorid)
    {
        try
        {
            if (actorid == myactorid)
            {
                UnSubscribe();
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //type 0是蓝，1是黄。
    public void InitSheild(int actorid, int type, UnityEngine.GameObject go)
    {
        myactorid = actorid;
        mygameobject = go;
        UISprite us = gameObject.GetComponent<UISprite>();
        if (us != null)
        {
            if (type == 0)
            {
                us.spriteName = "bt_1";
            }
            else
            {
                us.spriteName = "hd_1";
            }
        }
        UnityEngine.Transform tf = transform.Find("Sprite");
        if (tf != null)
        {
            us = tf.gameObject.GetComponent<UISprite>();
            if (us != null)
            {
                if (type == 0)
                {
                    us.spriteName = "bt_2";
                }
                else
                {
                    us.spriteName = "hd_2";
                }
                myuisprite = us;
            }
        }
        LateUpdate();
    }
    private int myactorid = 0;
    private UnityEngine.GameObject mygameobject = null;
    private UISprite myuisprite = null;
}
