using System;
using ArkCrossEngine;
public class NotificationLogic : UnityEngine.MonoBehaviour
{
    #region Singleton
    private static NotificationLogic s_Instance = null;
    public static NotificationLogic Instance
    {
        get
        {
            if (s_Instance == null)
            {
                UnityEngine.GameObject gameObjectRoot = UnityEngine.GameObject.Find(ArkCrossEngine.GlobalVariables.cGameRootName);
                if (gameObjectRoot != null)
                {
                    s_Instance = gameObjectRoot.GetComponent<NotificationLogic>();
                    if (s_Instance == null)
                    {
                        s_Instance = gameObjectRoot.AddComponent<NotificationLogic>();
                    }
                }
            }
            return s_Instance;
        }
    }
    #endregion
    public void Init()
    {
#if UNITY_ANDROID
  
#endif
        CleanNotification();
    }
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void OnApplicationPause(bool paused)
    {
        try
        {
            if (paused)
            {
                NotifyAllMessage();
            }
            else
            {
                CleanNotification();
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OnApplicationQuit()
    {
        try
        {
            NotifyAllMessage();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void CleanNotification()
    {

    }
    private void NotifyAllMessage()
    {
        DataDictionaryMgr<Data_NotificationConfig> m_NotificationConfigMgr = NotificationConfigProvider.Instance.NotificationConfigMgr;
        foreach (Data_NotificationConfig config in m_NotificationConfigMgr.GetData().Values)
        {
            NotifyMessage(config.m_Id, config.m_Title, config.m_Content, config.m_Date, config.m_Interval);
        }
    }
    private static void NotifyMessage(int id, string title, string content, DateTime fireDate, string interval)
    {
        try
        {

        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private static DateTime GetDateNow(DateTime fireDate)
    {
        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;
        System.DateTime newDate = new System.DateTime(year, month, day, fireDate.Hour, fireDate.Minute, 0);
        return newDate;
    }


}