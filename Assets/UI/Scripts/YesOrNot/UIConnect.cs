using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public enum UIConnectEnumType
{
    None,
    Reconnect,
    ChangeScene,
    RoleEnter,
}

public class UIConnect : UnityEngine.MonoBehaviour
{
    // for callback of login
    // FixMe: remove me
    public delegate void ConnectDelegate();
    public static ConnectDelegate UILoginConnectResultDelegate;
    // ui
    public UnityEngine.GameObject goBackground;
    public UnityEngine.GameObject goTweenAnim;
    public UIButton btnConfirm = null;
    public UISprite spConnect = null;
    public UILabel lblHintMsg = null;
    // timeout limit
    public float ConnectDelta = 10f;
    public float ReconnectCountDown = 10f;
    /// privates
    private float c_EndTime = 0f;
    private const int c_ConnectCountMax = 3;
    private UIConnectEnumType m_ConnectShowType = UIConnectEnumType.None;
    private float m_ConnectCD = 0f;
    private float m_ReconnectCD = 0f;
    private List<object> m_EventList = new List<object>();
    
    public void UnSubscribe()
    {
        try
        {
            for (int i = 0; i < m_EventList.Count; i++)
            {
                if (m_EventList[i] != null)
                {
                    LogicSystem.EventChannelForGfx.Unsubscribe(m_EventList[i]);
                }
            }
            m_EventList.Clear();
        }
        catch (Exception ex)
        {
            LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Awake()
    {
        try
        {
            object obj = LogicSystem.EventChannelForGfx.Subscribe<bool, bool>("ge_ui_connect_hint", "ui", OnShowConnectHint);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<UIConnectEnumType, bool, float>("ge_connect_hint", "ui", OnShowConnectHint);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null) m_EventList.Add(obj);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Start()
    {
    }
    void OnDestroy()
    {
        try
        {
            ClearDelegate();
            JoyStickInputProvider.SetActive(true);
            m_ConnectShowType = UIConnectEnumType.None;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void OnEnable()
    {
        try
        {
            if (btnConfirm != null)
                NGUITools.SetActive(btnConfirm.gameObject, false);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void Update()
    {
        try
        {
            UpdateInfo();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void OnConfirmClick()
    {
        try
        {
            switch (m_ConnectShowType)
            {
                case UIConnectEnumType.ChangeScene:
                    UIManager.Instance.HideWindowByName("Connect");
                    break;
                case UIConnectEnumType.Reconnect:
                    DFMUtils.Instance.RestartGame();
                    UIManager.Instance.HideWindowByName("Connect");
                    break;
                case UIConnectEnumType.RoleEnter:
                    DFMUtils.Instance.RestartGame();
                    UIManager.Instance.HideWindowByName("Connect");
                    break;
            }
            m_ConnectShowType = UIConnectEnumType.None;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    
    public void OnShowConnectHint(bool isConnect, bool active)
    {
        try
        {
            if (active)
            {
                // hide all widget first
                ShowHintWidget(false);
                // only accept none type
                if (m_ConnectShowType != UIConnectEnumType.None)
                    return;
                // is connect
                if (isConnect)
                {
                    m_ConnectShowType = UIConnectEnumType.ChangeScene;
                    m_ConnectCD = ConnectDelta;
                }
                else
                {
                    m_ConnectShowType = UIConnectEnumType.Reconnect;
                    m_ReconnectCD = ReconnectCountDown;
                    if (null != LobbyClient.Instance.CurrentRole
                      && null != LobbyClient.Instance.CurrentRole.Group)
                        LobbyClient.Instance.CurrentRole.Group.Reset();
                }
                // set and show hint
                string CHN_Connect = StrDictionaryProvider.Instance.GetDictString(15);
                if (lblHintMsg != null) lblHintMsg.text = CHN_Connect;
                UIManager.Instance.ShowWindowByName("Connect");
                // disable input
                JoyStickInputProvider.SetActive(false);
            }
            else
            {
                // enable input
                JoyStickInputProvider.SetActive(true);
                m_ConnectShowType = UIConnectEnumType.None;
                UIManager.Instance.HideWindowByName("Connect");
                if (UILoginConnectResultDelegate != null) UILoginConnectResultDelegate();
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    
    public void OnShowConnectHint(UIConnectEnumType connectType, bool active, float duration)
    {
        try
        {
            if (active)
            {
                ShowHintWidget(false);
                if (m_ConnectShowType != UIConnectEnumType.None)
                    return;
                c_EndTime = duration + UnityEngine.Time.time;
                m_ConnectShowType = connectType;
                string CHN_Connect = StrDictionaryProvider.Instance.GetDictString(15);
                if (lblHintMsg != null) lblHintMsg.text = CHN_Connect;
                UIManager.Instance.ShowWindowByName("Connect");
                JoyStickInputProvider.SetActive(false);
            }
            else
            {
                ResetEndTime();
                JoyStickInputProvider.SetActive(true);
                m_ConnectShowType = UIConnectEnumType.None;
                UIManager.Instance.HideWindowByName("Connect");
                if (UILoginConnectResultDelegate != null) UILoginConnectResultDelegate();
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void ClearDelegate()
    {
        UILoginConnectResultDelegate = null;
    }
    private void UpdateInfo()
    {
        if (m_ConnectShowType == UIConnectEnumType.ChangeScene)
        {
            if (m_ConnectCD > 0)
            {
                m_ConnectCD -= UnityEngine.Time.deltaTime;
                if (spConnect != null) spConnect.transform.Rotate(new UnityEngine.Vector3(0, 0, -10f));
            }
            else
            {
                if (UILoginConnectResultDelegate != null) UILoginConnectResultDelegate();
                string CHN_Failure = StrDictionaryProvider.Instance.GetDictString(21);
                if (lblHintMsg != null)
                {
                    lblHintMsg.text = CHN_Failure;
                }
                ShowHintWidget(true);
            }
        }

        if (m_ConnectShowType == UIConnectEnumType.Reconnect)
        {
            if (m_ReconnectCD > 0)
            {
                m_ReconnectCD -= UnityEngine.Time.deltaTime;
                if (spConnect != null) spConnect.transform.Rotate(new UnityEngine.Vector3(0, 0, -10f));
            }
            else
            {
                string CHN_Failure = StrDictionaryProvider.Instance.GetDictString(16);
                if (lblHintMsg != null)
                {
                    lblHintMsg.text = CHN_Failure;
                }
                ShowHintWidget(true);
            }
        }

        if (c_EndTime > 0f)
        {
            if (c_EndTime > UnityEngine.Time.time)
            {
                if (spConnect != null) spConnect.transform.Rotate(new UnityEngine.Vector3(0, 0, -10f));
            }
            else
            {
                ResetEndTime();
                string CHN_Failure = StrDictionaryProvider.Instance.GetDictString(21);
                if (lblHintMsg != null)
                {
                    lblHintMsg.text = CHN_Failure;
                }
                ShowHintWidget(true);
            }
        }
    }
    private void ShowHintWidget(bool show)
    {
        if (btnConfirm != null) NGUITools.SetActive(btnConfirm.gameObject, show);
        if (goBackground != null) NGUITools.SetActive(goBackground, show);
        if (lblHintMsg != null) NGUITools.SetActive(lblHintMsg.gameObject, show);
        if (goTweenAnim != null) NGUITools.SetActive(goTweenAnim, !show);
    }
    private void ResetEndTime()
    {
        c_EndTime = 0f;
    }
}
