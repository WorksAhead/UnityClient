using UnityEngine;
using System.Collections;
using ArkCrossEngine;
using System.Collections.Generic;
/*
 * name：UIDataCache；
 * function: ui数据存储类 此类为单例；
 * author: 李齐；
 * */
public class UIDataCache
{

    static private UIDataCache m_Instance = new UIDataCache();
    static public UIDataCache Instance
    {
        get
        {
            return m_Instance;
        }
    }
    public void Init(UnityEngine.GameObject rootWindow)
    {
        LogicSystem.EventChannelForGfx.Subscribe<int>("ge_change_sceneId_ui", "ui_data", ChangeSceneId);
        LogicSystem.EventChannelForGfx.Subscribe<string, string, string, string, ArkCrossEngine.MyAction<int>, bool>("ge_show_dialog", "ui", HandleDialogMsg);
    }
    //判断当前场景是否为多人PVE
    public bool IsMultPveScene()
    {
        Data_SceneConfig cfg = SceneConfigProvider.Instance.GetSceneConfigById(curSceneId);
        if (cfg != null)
        {
            return (cfg.m_Type == (int)SceneTypeEnum.TYPE_MULTI_PVE);
        }
        return false;
    }
    public bool IsPvPScene()
    {
        Data_SceneConfig cfg = SceneConfigProvider.Instance.GetSceneConfigById(curSceneId);
        if (cfg != null)
        {
            return (cfg.m_Type == (int)SceneTypeEnum.TYPE_PVP);
        }
        return false;
    }
    public bool IsArenaPvPScene()
    {
        Data_SceneConfig cfg = SceneConfigProvider.Instance.GetSceneConfigById(curSceneId);
        if (cfg != null)
        {
            return cfg.m_SubType == (int)SceneSubTypeEnum.TYPE_PVAP;
        }
        return false;
    }
    public bool IsMainciytScene()
    {
        Data_SceneConfig cfg = SceneConfigProvider.Instance.GetSceneConfigById(curSceneId);
        if (cfg != null)
        {
            return cfg.m_Type == (int)SceneTypeEnum.TYPE_PURE_CLIENT_SCENE;
        }
        return false;
    }
    public bool IsServerSelectScene()
    {
        Data_SceneConfig cfg = SceneConfigProvider.Instance.GetSceneConfigById(curSceneId);
        if (cfg != null)
        {
            return cfg.m_Type == (int)SceneTypeEnum.TYPE_SERVER_SELECT;
        }
        else
        {
            //第一次登陆时，无法判断该类型
            return true;
        }
    }
    void ChangeSceneId(int sceneId)
    {
        curSceneId = sceneId;
    }
    private float time;

    public int curUnlockSceneId = 1011; //当前解锁 的普通关卡id
    public int curMasterUnlockSceneId = 2011;//当前解锁 的精英关卡id
    public bool justLogin = true;//刚刚登陆
    public int curSceneId;  //当前进入地图的id
    public int lastSceneId; //切换之前的场景Id
    public int curRank = -1;//名人赛当前排名
    public float curPlayerFightingScore = float.MinValue;//玩家当前战力
    public UnityEngine.Vector3 MainSceneCameraPos = new UnityEngine.Vector3(48.6937f, 155.9267f, 2.950846f);//用于保存登录场景时场景相机的数据
    public UnityEngine.Vector3 MainSceneCameralocalEulerAngles = new UnityEngine.Vector3(359.1326f, 0.0f, 0.0f);//和登录场景相机数据保持一致
    public bool m_IsSceneCameraInit = false;
    public bool isLoadingEnd = false;//技能是否laoding结束
    public bool needShowMarsLoading = false;//是否需要播放pvp前置动画
    public SceneSubTypeEnum prevSceneType = SceneSubTypeEnum.TYPE_UNKNOWN;//上一场景类型
    public bool masterRecord = true;// 是否需要名人战历史记录查询
                                    /// <summary>
                                    /// //ui打开时的缓存
                                    /// </summary>
    public int openRankingTable = 0; // 0:战神赛， 1：排行榜
                                     //打开扫荡ui使用参数
    public int wipeSceneId = -1; // 扫荡场景id
    public int wipeTimes = -1; // 扫荡次数
                               //打开扫荡神器ui使用参数
    public int artifactId = -1; //神器id
                                //打开动态好友ui使用参数
    public GfxUserInfo dynamicUserInfo = null; // 动态好友信息
                                               //打开Verifaction ui 使用参数
    public UIVerification.OpenType uiVerifactType = UIVerification.OpenType.Verification; // 0:打开激活码， 1：打开添加好友
                                                                                          //打开taskaward ui使用参数
    public int taskAwardUIType = -1; // 0:uiactivityAward 1:任务奖励打开时2:      3:试炼打开界面
    public List<int> taskAwardItems = null;// 任务奖励item
    public List<int> taskAwardNums = null;//任务奖励数量
    public TaskCompleteType taskComplleteType = TaskCompleteType.T_common; // 任务完成类型
    public int missionconfigID = -1;// 任务id
    public int treasureMapMoney = -1; // 试炼金钱，用于任务
    public int treasureCurrentClickIndex = -1; // 试炼金钱，用于任务当前点击
    public List<MailInfo> maillist = null;//邮件列表
    public int vigor = 0;//角色当前的体力
    private void HandleDialogMsg(string message, string button0, string button1, string button2, ArkCrossEngine.MyAction<int> dofunction, bool islogic)
    {
        UIManager.Instance.ShowWindowByName("Dialog");
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("Dialog");
        if (go != null)
        {
            Dialog dialogScript = go.GetComponent<Dialog>();
            if (dialogScript != null) dialogScript.ManageDialog(message, button0, button1, button2, dofunction, islogic);
        }
    }

}
