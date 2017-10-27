using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class Login : UnityEngine.MonoBehaviour
{
    // text hint click to start game
    public UILabel lblStartNotice;
    // sprite hint click to start game
    public UnityEngine.GameObject goLoginHint;
    // logo of game
    public UISprite spLogo;
    // server name display on screen
    public UILabel lblServerName = null;
    // server change button
    public UnityEngine.GameObject goServerBtn = null;
    // controller args for curve animation list up & down
    public UnityEngine.AnimationCurve CurveForUpwards = null;
    public UnityEngine.AnimationCurve CurveForDown = null;
    public float DurationForUpwards = 0.3f;
    public float DurationForDown = 0.3f;
    public float TweenOffset = 150;

    private int m_ServerId = 0;
    private int m_LogicServerId = 1;
    private List<object> m_EventList = new List<object>();

    public enum AccountLoginResult
    {
        Success = 0,
        FirstLogin,
        Error,
        Wait,
        Banned,
        AlreadyOnline,
        Queueing,
    }

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

    void Start()
    {
        try
        {
            // if get notice content successfully, show notice window
            OpenNotice();

            // register events
            object obj = LogicSystem.EventChannelForGfx.Subscribe<int>("ge_set_current_server", "ui", SetCurrentServer);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int, string>("ge_login_result", "lobby", OnLoginResult);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<bool>("ge_rolelist_result", "lobby", OnRoleListResult);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null) m_EventList.Add(obj);

            // try get server last login, or read from server table
            m_ServerId = PlayerPrefs.GetInt("LastLoginServerId");
            SetCurrentServer(m_ServerId);

            // try get logic server last login
            m_LogicServerId = PlayerPrefs.GetInt("LastLoginLogicServerId");
            SetCurrentLogicServer(m_LogicServerId);

            // update client version
            string version = "";

            // get device info ,including uuid, system info, etc.
#if UNITY_ANDROID || UNITY_IOS
            string deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
#elif UNITY_EDITOR
            string deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier + ((uint)UnityEngine.Application.dataPath.GetHashCode()).ToString();
#else
            string deviceUniqueIdentifier = System.Guid.NewGuid().ToString();
#endif

            Debug.Log("device uuid: " + deviceUniqueIdentifier);
            string system = "all";
            LogicSystem.PublishLogicEvent("ge_device_info", "lobby", deviceUniqueIdentifier, version, system);
            NormLog.Instance.UpdateDeviceidAndVersion(deviceUniqueIdentifier, version);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    
    void OpenNotice()
    {
        
    }

    public void OnLoginButtonClick()
    {
        // login result callback
        UIConnect.UILoginConnectResultDelegate = this.UIConnectResult;

        // query server config
        ServerConfig serverConfig = ServerConfigProvider.Instance.GetDataById(m_ServerId);
        if (serverConfig == null)
        {
            Debug.LogError("Can't read server info!!!");
            return;
        }

        // disable login according to table configuration
        if (!CheckCanLogin(serverConfig.ServerState))
        {
            return;
        }

        NormLog.Instance.Record(GameEventCode.StartLogin);

        // store last login info
        PlayerPrefs.SetInt("LastLoginServerId", m_ServerId);
        m_LogicServerId = serverConfig.LogicServerId;
        PlayerPrefs.SetInt("LastLoginLogicServerId", m_LogicServerId);

        // save server connection info
        string nodeIp = serverConfig.NodeIp;
        int nodePort = serverConfig.NodePort;
        string serverAddress = "ws://" + nodeIp + ":" + nodePort;
        LogicSystem.PublishLogicEvent("ge_select_server", "lobby", serverAddress, m_LogicServerId);

        // direct login
        // FixMe: replace by login sdk
        LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
        LogicSystem.PublishLogicEvent("ge_direct_login", "lobby");
    }
    public void OnLoginResult(int result, string accountId)
    {
        try
        {
            // mark account login
            GfxSystem.PublishGfxEvent("ge_enable_accountlogin", "ui");

            // close connection window
            LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, false);

            // vairy login result
            AccountLoginResult ret = (AccountLoginResult)result;
            if (ret == AccountLoginResult.Success)
            {
                NormLog.Instance.UpdateUserid(accountId);
            }
            else if (ret == AccountLoginResult.FirstLogin)
            {
                UIManager.Instance.HideWindowByName("LoginPrefab");
                UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("Verification");
                if (null != go)
                {
                    go.GetComponent<UIVerification>().InitOpenType(UIVerification.OpenType.Verification);
                }
                UIManager.Instance.ShowWindowByName("Verification");
                NormLog.Instance.UpdateUserid(accountId);
            }
            // login failed, back to login scene
            else
            {
                // get description of login result
                string chn_desc = string.Empty;
                if (ret == AccountLoginResult.Banned)
                {
                    chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(209);
                }
                else if (ret == AccountLoginResult.AlreadyOnline)
                {
                    chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(210);
                }
                else
                {
                    chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(208);
                }
                string reloginTip = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(211);

                // display YesOrNo dialog, call relogin if needed
                LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", chn_desc, reloginTip, null, null, (MyAction<int>)OnRelogin, false);

                // close socket connection
                LogicSystem.PublishLogicEvent("ge_stop_login", "lobby");
            }
            NormLog.Instance.Record(GameEventCode.VerifyResult);
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    
    public void OnRelogin(int i)
    {
        LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
        LogicSystem.PublishLogicEvent("ge_direct_login", "lobby");
    }

    public void OnRoleListResult(bool isSuccess)
    {
        try
        {
            if (isSuccess == true)
            {
                // get role list success, jump to create hero scene
                UIManager.Instance.HideWindowByName("LoginPrefab");
                LogicSystem.EventChannelForGfx.Publish("ge_create_hero_scene", "ui", true);
            }
            else
            {
                // error
                // FixMe: relogin
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void SetCurrentServer(int serverId)
    {
        try
        {
            m_ServerId = serverId;
            
            // find server list from table
            ServerConfig serverConfig = ServerConfigProvider.Instance.GetDataById(serverId);
            if (serverConfig == null)
            {
                MyDictionary<int, object> serverDict = ServerConfigProvider.Instance.GetData();
                foreach (ServerConfig cfg in serverDict.Values)
                {
                    if (cfg != null)
                    {
                        serverConfig = cfg;
                        m_ServerId = cfg.GetId();
                        break;
                    }
                }
            }

            // update server configuration
            if (serverConfig != null)
            {
                StrDictionary strDic = StrDictionaryProvider.Instance.GetDataById(201);
                if (strDic != null)
                    lblServerName.text = "" + m_ServerId + strDic.m_String + serverConfig.ServerName;
                else
                    lblServerName.text = "" + m_ServerId + serverConfig.ServerName;
                NormLog.Instance.ServerName = serverConfig.ServerName;
            }
        }
        catch (Exception ex)
        {
            LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void SetCurrentLogicServer(int serverId)
    {
        try
        {
            m_LogicServerId = serverId;
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // called by button event
    public void OnChangeServer()
    {
        // show server select window
        UIManager.Instance.ShowWindowByName("ServerSelect");

        // hide login hint
        TweenAlphaLoginHint(1f, 0f);

        // set current select server
        LogicSystem.EventChannelForGfx.Publish("ge_recent_server", "ui", m_ServerId);

        // show server select panel with animatin
        if (goServerBtn != null)
        {
            UIButton uibtn = goServerBtn.GetComponent<UIButton>();
            if (uibtn != null) uibtn.isEnabled = false;
            UnityEngine.Vector3 serverBtnPos = goServerBtn.transform.localPosition;
            TweenPosition tweenPos = TweenPosition.Begin(goServerBtn, DurationForUpwards, new UnityEngine.Vector3(serverBtnPos.x, serverBtnPos.y + TweenOffset, 0f));
            if (tweenPos != null)
            {
                tweenPos.animationCurve = CurveForUpwards;
                EventDelegate.Add(tweenPos.onFinished, OnTweenUpwardsFinished);
            }
        }
    }

    private void OnTweenUpwardsFinished()
    {
        if (goServerBtn != null)
        {
            UIButton uibtn = goServerBtn.GetComponent<UIButton>();
            if (uibtn != null) uibtn.isEnabled = true;
            TweenPosition tween = goServerBtn.GetComponent<TweenPosition>();
            if (tween != null) Destroy(tween);
            NGUITools.SetActive(goServerBtn, false);
        }
    }

    public void TweenDownServerBtn()
    {
        if (null != goServerBtn)
        {
            NGUITools.SetActive(goServerBtn, true);
            UnityEngine.Vector3 serverBtnPos = goServerBtn.transform.localPosition;
            TweenPosition tweenPos = TweenPosition.Begin(goServerBtn, DurationForDown, new UnityEngine.Vector3(serverBtnPos.x, serverBtnPos.y - TweenOffset, 0f));
            if (tweenPos != null)
            {
                tweenPos.animationCurve = CurveForDown;
            }
        }
    }

    public void TweenAlphaLoginHint(float from, float to)
    {
        if (goLoginHint == null) return;
        TweenAlpha tween = TweenAlpha.Begin<TweenAlpha>(goLoginHint, 0.2f);
        tween.from = from;
        tween.to = to;
    }

    public void AccountLogoutBtnClicked()
    {
        Debug.LogError("Account logout...");
        // FixMe: add sdk
    }

    private void ShowLogin()
    {
        m_IsReadyForLogin = true;
        EnableLoginUI(true);

    }
    private void EnableLoginUI(bool enable)
    {
        if (goServerBtn != null) NGUITools.SetActive(goServerBtn, enable);
        if (spLogo != null) NGUITools.SetActive(spLogo.gameObject, enable);
        if (lblStartNotice != null) NGUITools.SetActive(lblStartNotice.gameObject, enable);
    }

    public IEnumerator PlayLogoAnimation()
    {
        if (spLogo == null) yield break;
        NGUITools.SetActive(spLogo.gameObject, true);
        for (float fillAmount = 0.0f; fillAmount <= 1f; fillAmount += 0.04f)
        {
            if (m_IsReadyForLogin)
            {
                spLogo.fillAmount = 1f;
                yield break;
            }
            if (spLogo != null) spLogo.fillAmount = fillAmount;
            yield return new WaitForSeconds(0.005f);
        }
        ShowLogin();
    }

    bool CheckCanLogin(int state)
    {
        bool canLogin = true;
        if (state == 0)
        {
            SendDialog(33, 4, null);
            canLogin = false;
        }
        return canLogin;
    }

    void SendDialog(int i_chn_desc, int i_confirm, params object[] insert_name)
    {
        string chn_desc = "";
        string confirm = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(i_confirm);
        chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(i_chn_desc);
        string str = chn_desc;
        if (insert_name != null)
        {
            str = string.Format(chn_desc, insert_name);
        }
        LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", str, confirm, null, null, null, false);
    }

    private void UIConnectResult()
    {
        UIConnect.UILoginConnectResultDelegate -= UIConnectResult;
        GfxSystem.PublishGfxEvent("ge_enable_accountlogin", "ui");
    }

    private bool m_IsReadyForLogin = false;
    private string m_CameraObjName = "2_loading_xiangji";
}
