using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ArkCrossEngine;

public class LoginNotice : UnityEngine.MonoBehaviour
{
    public UILabel newNotice = null;

    void Start()
    {
        try
        {
            newNotice.text = NoticeConfigLoader.s_NoticeContent;
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClosePanel()
    {
        UIManager.Instance.HideWindowByName("LoginNotice");
    }
}
