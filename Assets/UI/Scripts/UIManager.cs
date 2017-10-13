using System;
using UnityEngine;
using System.Collections.Generic;
using ArkCrossEngine;

public enum UILoadType : int
{
    DontLoad = -1,
    NoneActive = 0,
    Active = 1
}
public enum UISceneType : int
{
    LoginScene = 0,
    MainCityScene = 1,
    PveScene = 3,
    PvpScene = 4,
    TreasureScene = 5,//远征
    MultiPveScene = 6,
    JinBiScene = 7,
    PvapScene = 8,
    PlatformDefense = 9,
}

public class UIManager
{
    public delegate void VoidDelegate();
    public delegate void StringDelegate(string name);
    public VoidDelegate OnAllUiLoadedDelegate;
    public VoidDelegate OnHideAllUIDelegate;
    public StringDelegate OnHideUiDelegate;
    public VoidDelegate OnTriggerNewbieGuideDelegate;
    public void Init()
    {
        uiConfigDataDic = UiConfigProvider.Instance.GetData();
    }
    public void Init(UnityEngine.GameObject rootWindow)
    {
        m_RootWindow = rootWindow;
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<string>("show_ui", "ui", OnSkillShowUI);
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<string>("hide_ui", "ui", HideWindowByName);
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_toggle_all_ui", "ui", ToggleAllUI);
    }
    /// <summary>
    /// 给策划拍视频时临时增加的方法，不要调用
    /// </summary>
    private bool tempUiVisible = true;
    private void ToggleAllUI()
    {
        tempUiVisible = !tempUiVisible;
        SetAllUiVisible(tempUiVisible);
    }
    private void OnSkillShowUI(string name)
    {
        if (DFMUiRoot.InputMode == InputType.Touch && name.Equals("SkillBar"))
        {
            return;
        }
        ShowWindowByName(name);
    }
    public void ShowJoystick(bool enable)
    {
        JoyStickInputProvider.JoyStickEnable = enable;
        if (m_VisibleWindow.ContainsKey("StoryDlgSmall"))
        {
            if (DFMUiRoot.InputMode == InputType.Joystick)
            {
                JoyStickInputProvider.JoyStickEnable = false;
            }
        }
        if (m_VisibleWindow.ContainsKey("VictoryPanel"))
        {
            if (DFMUiRoot.InputMode == InputType.Joystick)
            {
                JoyStickInputProvider.JoyStickEnable = false;
            }
        }
    }
    public void Clear()
    {
        m_IsLoadedWindow.Clear();
        m_VisibleWindow.Clear();
        m_UnVisibleWindow.Clear();
        m_ExclusionWindow.Clear();
        m_ExclusionWindowStack.Clear();
    }
    //获取已经加载的窗口UnityEngine.GameObject
    public UnityEngine.GameObject GetWindowGoByName(string windowName)
    {
        if (windowName == null) return null;
        if (m_IsLoadedWindow.ContainsKey(windowName.Trim()))
            return m_IsLoadedWindow[windowName];
        return null;
    }
    //如果windowName还没加载且该窗口为Dynamic类型，则主动加载一个并返回
    public UnityEngine.GameObject TryGetWindowGameObject(string windowName)
    {
        if (windowName == null) return null;
        if (m_IsLoadedWindow.ContainsKey(windowName.Trim()))
            return m_IsLoadedWindow[windowName];
        UiConfig uiCfg = GetUiConfigByName(windowName);
        if (uiCfg != null && uiCfg.m_IsDynamic)
        {
            return LoadDeactiveWindow(uiCfg.m_WindowName, UICamera.mainCamera);
        }
        return null;
    }
    //获取UI的路径
    public string GetPathByName(string windowName)
    {
        UiConfig uiCfg = GetUiConfigByName(windowName);
        if (uiCfg != null)
        {
            return uiCfg.m_WindowPath;
        }
        return null;
    }
    public UiConfig GetUiConfigByName(string name)
    {
        foreach (UiConfig uiCfg in uiConfigDataDic.Values)
        {
            if (uiCfg != null && uiCfg.m_WindowName == name)
            {
                return uiCfg;
            }
        }
        return null;
    }
    public UnityEngine.GameObject LoadWindowByName(string windowName, UnityEngine.Camera cam)
    {
        UnityEngine.GameObject window = null;
        UiConfig uiCfg = GetUiConfigByName(windowName);
        if (null != uiCfg)
        {
            window = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource(uiCfg.m_WindowPath));
            if (null != window)
            {
                window = NGUITools.AddChild(m_RootWindow, window);
                UnityEngine.Vector3 screenPos = CalculateUiPos(uiCfg.m_OffsetLeft, uiCfg.m_OffsetRight, uiCfg.m_OffsetTop, uiCfg.m_OffsetBottom);
                if (null != window && cam != null)
                    window.transform.position = cam.ScreenToWorldPoint(screenPos);
                string name = uiCfg.m_WindowName;
                while (m_IsLoadedWindow.ContainsKey(name))
                {
                    name += "ex";
                }
                m_IsLoadedWindow.Add(name, window);
                m_VisibleWindow.Add(name, window);
                return window;
            }
            else
            {
                Debug.Log("!!!load " + uiCfg.m_WindowPath + " failed");
            }
        }
        else
        {
            Debug.Log("!!!load " + windowName + " failed");
        }
        return null;
    }

    //加载窗口，默认不显示()
    private UnityEngine.GameObject LoadDeactiveWindow(string windowName, UnityEngine.Camera cam)
    {
        UnityEngine.GameObject window = null;
        UiConfig uiCfg = GetUiConfigByName(windowName);
        if (null != uiCfg)
        {
            window = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource(uiCfg.m_WindowPath));
            if (null != window)
            {
                //ArkCrossEngine.ResourceSystem.GetSharedResource;
                window = NGUITools.AddChild(m_RootWindow, window);
                UnityEngine.Vector3 screenPos = CalculateUiPos(uiCfg.m_OffsetLeft, uiCfg.m_OffsetRight, uiCfg.m_OffsetTop, uiCfg.m_OffsetBottom);
                if (null != window && cam != null)
                    window.transform.position = cam.ScreenToWorldPoint(screenPos);
                string name = uiCfg.m_WindowName;
                while (m_IsLoadedWindow.ContainsKey(name))
                {
                    name += "ex";
                }
                NGUITools.SetActive(window, false);
                m_IsLoadedWindow.Add(name, window);
                m_UnVisibleWindow.Add(name, window);
                return window;
            }
            else
            {
                Debug.Log("!!!load " + uiCfg.m_WindowPath + " failed");
            }
        }
        else
        {
            Debug.Log("!!!load " + windowName + " failed");
        }
        return null;
    }
    public void LoadAllWindows(UISceneType sceneType, UnityEngine.Camera cam)
    {
        if (null == m_RootWindow)
            return;

        foreach (UiConfig info in uiConfigDataDic.Values)
        {
            if (info.m_ShowType != (int)(UILoadType.DontLoad) && info.m_OwnToSceneList.Contains((int)sceneType))
            {
                //Debug.Log(info.m_WindowName);
                UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource(info.m_WindowPath));
                if (go == null)
                {
                    Debug.Log("!!!Load ui " + info.m_WindowPath + " failed.");
                    continue;
                }
                UnityEngine.GameObject child = NGUITools.AddChild(m_RootWindow, go);
                if (info.m_ShowType == (int)(UILoadType.Active))
                {
                    NGUITools.SetActive(child, true);
                    if (!m_VisibleWindow.ContainsKey(info.m_WindowName))
                    {
                        m_VisibleWindow.Add(info.m_WindowName, child);
                    }
                }
                else
                {
                    NGUITools.SetActive(child, false);
                    if (!m_UnVisibleWindow.ContainsKey(info.m_WindowName))
                    {
                        m_UnVisibleWindow.Add(info.m_WindowName, child);
                    }
                }
                UnityEngine.Vector3 screenPos = CalculateUiPos(info.m_OffsetLeft, info.m_OffsetRight, info.m_OffsetTop, info.m_OffsetBottom);
                if (!m_IsLoadedWindow.ContainsKey(info.m_WindowName))
                {
                    m_IsLoadedWindow.Add(info.m_WindowName, child);
                }
                if (null != child && cam != null)
                    child.transform.position = cam.ScreenToWorldPoint(screenPos);
            }
        }
        IsUIVisible = true;
    }
    public void UnLoadAllWindow()
    {
        //每一个订阅事件的窗口UI都需要一个UnSubscribe函数用于消除事件
        LogicSystem.EventChannelForGfx.Publish("ge_ui_unsubscribe", "ui");
        foreach (UnityEngine.GameObject window in m_IsLoadedWindow.Values)
        {
            if (null != window)
                //NGUIDebug.Log(window.name);
                NGUITools.DestroyImmediate(window);
        }
        Clear();
    }
    //卸载窗口
    public void UnLoadWindowByName(string name)
    {
        UnityEngine.GameObject go = GetWindowGoByName(name);
        if (go != null)
        {
            NGUITools.Destroy(go);
            if (m_IsLoadedWindow.ContainsKey(name)) m_IsLoadedWindow.Remove(name);
            if (m_VisibleWindow.ContainsKey(name)) m_VisibleWindow.Remove(name);
            if (m_UnVisibleWindow.ContainsKey(name)) m_UnVisibleWindow.Remove(name);
        }
    }
    public void ShowWindowByName(string windowName)
    {
        try
        {
            if (windowName == null) return;
            if (m_VisibleWindow.ContainsKey(windowName))
                return;
            UiConfig uiCfg = GetUiConfigByName(windowName);
            if (m_UnVisibleWindow.ContainsKey(windowName))
            {
                UnityEngine.GameObject window = m_UnVisibleWindow[windowName];
                if (null != window)
                {
                    NGUITools.SetActive(window, true);
                    m_VisibleWindow.Add(windowName, window);
                    m_UnVisibleWindow.Remove(windowName);
                }
            }
            else
            {
                //走到这里正常情况为Dynamic类型的UI
                if (uiCfg != null && uiCfg.m_IsDynamic)
                {
                    LoadWindowByName(uiCfg.m_WindowName, UICamera.mainCamera);
                }
            }
            if (uiCfg != null && uiCfg.m_IsExclusion == true)
            {
                CloseExclusionWindow(windowName);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void ShowWindow(string windowName)
    {
        if (windowName == null) return;
        if (m_VisibleWindow.ContainsKey(windowName))
            return;
        if (m_UnVisibleWindow.ContainsKey(windowName))
        {
            UnityEngine.GameObject window = m_UnVisibleWindow[windowName];
            if (null != window)
            {
                NGUITools.SetActive(window, true);
                m_VisibleWindow.Add(windowName, window);
                m_UnVisibleWindow.Remove(windowName);
            }
        }
        else
        {
            //走到这里，正常情况下为Dynamic类型UI
            LoadWindowByName(windowName, UICamera.mainCamera);
        }
    }
    public void HideWindowByName(string windowName)
    {
        try
        {
            if (windowName == null) return;
            if (m_UnVisibleWindow.ContainsKey(windowName))
                return;
            UiConfig uiCfg = GetUiConfigByName(windowName);
            if (m_VisibleWindow.ContainsKey(windowName))
            {
                if (uiCfg != null && uiCfg.m_IsDynamic)
                {
                    //如果为动态类型则卸载
                    UnLoadWindowByName(windowName);
                }
                else
                {
                    UnityEngine.GameObject window = m_VisibleWindow[windowName];
                    if (null != window)
                    {
                        NGUITools.SetActive(window, false);
                        if (OnHideUiDelegate != null)
                        {
                            OnHideUiDelegate(windowName);
                        }
                        m_UnVisibleWindow.Add(windowName, window);
                        m_VisibleWindow.Remove(windowName);
                    }
                }
            }
            if (uiCfg != null && uiCfg.m_IsExclusion == true)
            {
                OpenExclusionWindow(windowName);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void HideWindow(string windowName)
    {
        if (windowName == null) return;
        if (m_UnVisibleWindow.ContainsKey(windowName))
            return;

        if (m_VisibleWindow.ContainsKey(windowName))
        {
            //UiConfig uiCfg = GetUiConfigByName(windowName);
            //if (uiCfg != null && uiCfg.m_IsDynamic) {
            //  //如果为动态类型则卸载
            //  UnLoadWindowByName(windowName);
            //} else {
            //因为后续会重新打开ui，为了保存数据不按动态类型销毁，直接隐藏
            UnityEngine.GameObject window = m_VisibleWindow[windowName];
            if (null != window)
            {
                NGUITools.SetActive(window, false);
                m_UnVisibleWindow.Add(windowName, window);
                m_VisibleWindow.Remove(windowName);
            }
            //}
        }
    }
    public void ToggleWindowVisible(string windowName)
    {
        if (IsWindowVisible(windowName))
        {
            HideWindowByName(windowName);
        }
        else
        {
            ShowWindowByName(windowName);
        }
    }
    public bool IsWindowVisible(string windowName)
    {
        if (m_VisibleWindow.ContainsKey(windowName))
            return true;
        else
        {
            return false;
        }
    }
    //关闭除windowName之外的所有窗口
    public void CloseExclusionWindow(string windowName)
    {
        UiConfig exclusionWndCfg = GetUiConfigByName(windowName);
        List<string> hide_windows = new List<string>();
        foreach (string name in m_VisibleWindow.Keys)
        {
            UiConfig cfg = GetUiConfigByName(name);
            if (name != windowName && cfg != null && cfg.m_Group == exclusionWndCfg.m_Group)
            {
                hide_windows.Add(name);
            }
        }
        if (hide_windows.Count > 0)
            m_ExclusionWindowStack.Push(hide_windows);
        for (int i = 0; i < hide_windows.Count; i++)
        {
            if (!string.IsNullOrEmpty(hide_windows[i]))
            {
                HideWindow(hide_windows[i]);
            }
        }
        /*
    foreach (string name in hide_windows) {
      HideWindow(name);
    }*/
        if (DFMUiRoot.InputMode == InputType.Joystick)
        {
            JoyStickInputProvider.JoyStickEnable = false;
        }
        if (WorldSystem.Instance.IsPureClientScene())
        {
            //CYGTConnector.HideCYGTSDK();
            //CYGTConnector.closeCYGTSDKController();
        }
    }
    //打开之前关闭的窗口
    public void OpenExclusionWindow(string windowName)
    {
        //
        if (m_ExclusionWindowStack.Count > 0)
            m_ExclusionWindow = m_ExclusionWindowStack.Pop();
        if (m_ExclusionWindow == null) return;
        for (int i = 0; i < m_ExclusionWindow.Count; i++)
        {
            if (!string.IsNullOrEmpty(m_ExclusionWindow[i]))
            {
                ShowWindow(m_ExclusionWindow[i]);
            }
        }
        /*
    foreach (string name in m_ExclusionWindow) {
      ShowWindow(name);
    }*/
        m_ExclusionWindow.Clear();
        if (DFMUiRoot.InputMode == InputType.Joystick && m_ExclusionWindowStack.Count == 0 && !IsAnyExclusionWindowVisble())
        {
            JoyStickInputProvider.JoyStickEnable = true;
        }
        if (m_ExclusionWindowStack.Count == 0 && WorldSystem.Instance.IsPureClientScene()
          && !IsAnyExclusionWindowVisble())
        {
            //CYGTConnector.ShowCYGTSDK();
            //CYGTConnector.closeCYGTSDKController();
        }
    }
    public void SetAllUiVisible(bool isVisible)
    {
        if (isVisible)
        {
            TouchManager.TouchEnable = true;
            OpenExclusionWindow("");
        }
        else
        {
            TouchManager.TouchEnable = false;
            CloseExclusionWindow("SceneListenUI");
        }
        IsUIVisible = isVisible;
        NicknameAndMoney(isVisible);
    }
    /*判断是否有全屏UI显示*/
    public bool IsAnyExclusionWindowVisble()
    {
        foreach (string name in m_VisibleWindow.Keys)
        {
            UiConfig uiCfg = GetUiConfigByName(name);
            if (uiCfg != null && uiCfg.m_IsExclusion)
                return true;
        }
        return false;
    }
    void NicknameAndMoney(bool vis)
    {
        if (m_RootWindow != null)
        {
            UnityEngine.Transform tf = m_RootWindow.transform.Find("DynamicWidget");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, vis);
            }
            /*tf = m_RootWindow.transform.Find("PveFightInfo(Clone)");
            if (tf != null) {
              NGUITools.SetActive(tf.gameObject, vis);
            }
            */
            UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
            if (go != null)
            {
                PveFightInfo pfi = go.GetComponent<PveFightInfo>();
                if (pfi != null)
                {
                    pfi.SetActive(vis);
                }
            }
            tf = m_RootWindow.transform.Find("ScreenScrollTip");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, vis);
            }
        }
    }
    public UnityEngine.Vector3 CalculateUiPos(float offsetL, float offsetR, float offsetT, float offsetB)
    {
        float screen_width = 0;
        float screen_height = 0;
        if (UICamera.mainCamera != null)
        {
            screen_width = UICamera.mainCamera.pixelRect.width;
            screen_height = UICamera.mainCamera.pixelRect.height;
        }
        else
        {
            screen_width = Screen.width;
            screen_height = Screen.height;
        }

        UnityEngine.Vector3 screenPos = new UnityEngine.Vector3();
        if (offsetL == -1 && offsetR == -1)
        {
            screenPos.x = screen_width / 2;
        }
        else
        {
            if (offsetL != -1)
                screenPos.x = offsetL;
            else
            {
                screenPos.x = screen_width - offsetR;
            }
        }
        if (offsetT == -1 && offsetB == -1)
        {
            screenPos.y = screen_height / 2;
        }
        else
        {
            if (offsetT != -1)
            {
                screenPos.y = screen_height - offsetT;
            }
            else
            {
                screenPos.y = offsetB;
            }
        }
        screenPos.z = 0;
        return screenPos;
    }
    static private UIManager m_Instance = new UIManager();
    static public UIManager Instance
    {
        get { return m_Instance; }
    }
    static public string GetItemProtetyStr(float data, int type)
    {
        string str = "";
        switch (type)
        {
            case (int)ArkCrossEngine.ItemAttrDataConfig.ValueType.AbsoluteValue:
                str = (data > 0.0f ? "+" : "") + UnityEngine.Mathf.FloorToInt(data);
                break;
            case (int)ArkCrossEngine.ItemAttrDataConfig.ValueType.PercentValue:
                str = (data > 0.0f ? "+" : "") + UnityEngine.Mathf.FloorToInt(data * 100) + "%";
                break;
            case (int)ArkCrossEngine.ItemAttrDataConfig.ValueType.LevelRateValue:
                str = (data > 0.0f ? "+" : "") + UnityEngine.Mathf.FloorToInt(data);
                break;
            default:
                str = "No This Item Type!";
                break;
        }
        return str;
    }
    static public float GetItemPropertyData(float data, int type)
    {
        float dataf = data;
        switch (type)
        {
            case (int)ArkCrossEngine.ItemAttrDataConfig.ValueType.AbsoluteValue:
                dataf = (float)(UnityEngine.Mathf.FloorToInt(data));
                break;
            case (int)ArkCrossEngine.ItemAttrDataConfig.ValueType.PercentValue:
                dataf = (float)(UnityEngine.Mathf.FloorToInt(data * 100) / 100.0f);
                break;
            case (int)ArkCrossEngine.ItemAttrDataConfig.ValueType.LevelRateValue:
                break;
            default:
                break;
        }
        return dataf;
    }
    static public int UIRootMinimumHeight = 640;
    static public int UIRootMaximunHeight = 768;
    static public UnityEngine.Color SkillDrectorColor = new UnityEngine.Color(255, 255, 255);
    static public List<UnityEngine.GameObject> CheckItemForDelete = new List<UnityEngine.GameObject>();
    static public float dragtime = 0.0f;
    public bool IsUIVisible = true;
    private UnityEngine.GameObject m_RootWindow = null;
    private List<string> m_ExclusionWindow = new List<string>();
    private Dictionary<string, UnityEngine.GameObject> m_IsLoadedWindow = new Dictionary<string, UnityEngine.GameObject>();
    private Dictionary<string, UnityEngine.GameObject> m_VisibleWindow = new Dictionary<string, UnityEngine.GameObject>();
    private Dictionary<string, UnityEngine.GameObject> m_UnVisibleWindow = new Dictionary<string, UnityEngine.GameObject>();
    public Dictionary<string, WindowInfo> m_WindowsInfoDic = new Dictionary<string, WindowInfo>();
    MyDictionary<int, object> uiConfigDataDic = new MyDictionary<int, object>();
    private Stack<List<string>> m_ExclusionWindowStack = new Stack<List<string>>();
}