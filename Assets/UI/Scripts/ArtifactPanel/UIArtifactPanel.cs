using ArkCrossEngine;
using System;
using System.Collections.Generic;

public class UIArtifactPanel : UnityEngine.MonoBehaviour
{
    public ArkCrossEngine.GameObject upgradeEffect1 = null;
    public ArkCrossEngine.GameObject upgradeEffect2 = null;
    public ArkCrossEngine.GameObject upgradeEffect3 = null;
    public ArkCrossEngine.GameObject upgradeEffect4 = null;
    public ArkCrossEngine.GameObject upEffGO = null;
    public UIArtifactIntroduce artifactIntroduce = null;
    public UIArtifactTitle artifactTitle = null;
    public ArtifactOperation artifactOperation = null;
    public ArtifactRightInfo artifactRightInfo = null;
    private int m_CurrentArtifactId = -1;
    private const int c_ArtifactNum = 4;
    private const string AshHint = "skilllevel";
    private const string LightHint = "skilllevel2";
    private List<object> m_EventList = new List<object>();
    private List<int> canUpgradeIdList = new List<int>();
    public float effectDuration = 2.0f;
    //
    public void UnSubscribe()
    {
        try
        {
            for (int i = 0; i < m_EventList.Count; i++)
            {
                if (null != m_EventList[i]) LogicSystem.EventChannelForGfx.Unsubscribe(m_EventList[i]);
            }
            m_EventList.Clear();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Start()
    {
        try
        {
            object obj = null;
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int, int, ArkCrossEngine.Network.GeneralOperationResult>("ge_upgrade_legacy", "legacy", HandleLeveUpResult);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_item_change", "item", CheckUpgradeTip);
            if (obj != null)
                m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe<int>("artifact_click_button", "artifact", ClickButton);
            if (obj != null)
                m_EventList.Add(obj);

            InitArtifact();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //点击神器按钮
    void ClickButton(int itemID)
    {
        try
        {
            ItemDataInfo data_info = GetArtifactInfoById(itemID);
            if (null != data_info)
            {
                SetArtifactId(data_info.ItemId);
                artifactOperation.SetOperationInfo(m_CurrentArtifactId);
                if (artifactIntroduce != null)
                    artifactIntroduce.SetIntroduce(m_CurrentArtifactId, data_info.IsUnlock);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //获取神器信息
    public ItemDataInfo GetArtifactInfoById(int itemId)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            for (int i = 0; i < role_info.Legacys.Length; i++)
            {
                if (role_info.Legacys[i] != null && role_info.Legacys[i].ItemId == itemId)
                {
                    return role_info.Legacys[i];
                }
            }
        }
        return null;
    }
    void Awake()
    {
        try
        {
            CheckUpgradeTip();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        try
        {
            if (CheckUpgradeOnly())
            {
                if (canUpgradeIdList.Count > 1)
                {
                    canUpgradeIdList.Sort(IndexSort);
                }
                if (canUpgradeIdList.Count > 0)
                {
                    LocationToItem(canUpgradeIdList[0]);
                }
            }
            RefreshPanel(m_CurrentArtifactId);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private int IndexSort(int id1, int id2)
    {
        int index1 = GetArfifactIndex(id1);
        int index2 = GetArfifactIndex(id2);
        if (index1 < index2)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
    //获取神器Index
    private int GetArfifactIndex(int artifactId)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            for (int index = 0; index < role_info.Legacys.Length; ++index)
            {
                if (role_info.Legacys[index] != null && artifactId == role_info.Legacys[index].ItemId)
                    return index;
            }
        }
        return -1;
    }
    //Init 
    public void InitArtifact()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            ItemDataInfo item_info = role_info.Legacys[0];
            if (item_info != null)
            {
                SetArtifactId(item_info.ItemId);
                ClickButton(item_info.ItemId);
            }
        }
    }

    //设置当期神器id
    public void SetArtifactId(int id)
    {
        m_CurrentArtifactId = id;
    }
    //刷新整个ui
    private void RefreshPanel(int artifactId)
    {
        ClickButton(artifactId);
        if (artifactTitle != null)
            artifactTitle.UpdateTitleInfo();
        if (artifactRightInfo)
            artifactRightInfo.SetInfo();
    }
    //处理升级结果
    public void HandleLeveUpResult(int index, int artifactId, ArkCrossEngine.Network.GeneralOperationResult result)
    {
        try
        {
            if (result == ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed)
            {
                RefreshPanel(artifactId);
                CheckUpgradeTip();
                //播放特效
                if (upgradeEffect1 != null && upgradeEffect2 != null && upgradeEffect3 != null && upgradeEffect4 != null)
                {
                    ArkCrossEngine.GameObject ef = null;// = ResourceSystem.NewObject(upgradeEffect) as UnityEngine.GameObject;
                    switch (index)
                    {
                        case 1:
                            ef = ResourceSystem.NewObject(upgradeEffect1) as ArkCrossEngine.GameObject;
                            break;
                        case 2:
                            ef = ResourceSystem.NewObject(upgradeEffect2) as ArkCrossEngine.GameObject;
                            break;
                        case 3:
                            ef = ResourceSystem.NewObject(upgradeEffect3) as ArkCrossEngine.GameObject;
                            break;
                        case 4:
                            ef = ResourceSystem.NewObject(upgradeEffect4) as ArkCrossEngine.GameObject;
                            break;
                    }
                    if (ef != null && upEffGO != null)
                    {
                        ef.transform.position = upEffGO.transform.position;
                        Destroy(ef._GetImpl(), effectDuration);
                    }
                }
            }
            else
            {
                string CHN_DESC = StrDictionaryProvider.Instance.GetDictString(403);
                string CHN_CONFIRM = StrDictionaryProvider.Instance.GetDictString(4);
                LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", CHN_DESC, CHN_CONFIRM, null, null, null, false);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //根据id定位到某一项
    public void LocationToItem(int id)
    {
        //     int index = GetArfifactIndex(id);
        //     if (index >= 0 && index < spIndexHintArr.Length) {
        //       RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        //       if (role_info != null) {
        //         ItemDataInfo data_info = role_info.GetLegacyData(index);
        //         if (data_info != null) {
        //           TransGrid(-m_OffsetX);
        //           SetArtifactId(data_info.ItemId);
        //           UpdateArtifactInfo(data_info.ItemId);
        //           if (artifactIntroduce != null)
        //             artifactIntroduce.SetIntroduce(m_CurrentArtifactId, data_info.IsUnlock);
        //         }
        //       }
        //     }
    }
    //获取物品信息
    public ItemDataInfo GetItemDataInfoById(int itemId)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            for (int i = 0; i < role_info.Items.Count; i++)
            {
                if (role_info.Items[i] != null && role_info.Items[i].ItemId == itemId)
                {
                    return role_info.Items[i];
                }
            }
        }
        return null;
    }
    private void CheckUpgradeTip()
    {
        bool has = CheckUpgradeOnly();
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.GodEquip, has);
    }
    private bool CheckUpgradeOnly()
    {
        bool has = false;
        canUpgradeIdList.Clear();
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            foreach (ItemDataInfo item_info in role_info.Legacys)
            {
                if (item_info.IsUnlock)
                {//已解锁的
                    if (item_info.Level < role_info.Level)
                    {
                        LegacyLevelupConfig legacyLvUpCfg = LegacyLevelupConfigProvider.Instance.GetDataById(item_info.Level);
                        if (legacyLvUpCfg != null)
                        {
                            int legacyIndex = GetArfifactIndex(item_info.ItemId);
                            if (legacyIndex >= 0 && legacyIndex < legacyLvUpCfg.m_CostItemList.Count)
                            {
                                int costitemId = legacyLvUpCfg.m_CostItemList[legacyIndex];
                                int currentNum = 0;
                                ItemDataInfo costItemDataInfo = GetItemDataInfoById(costitemId);
                                if (costItemDataInfo != null)
                                {
                                    currentNum = costItemDataInfo.ItemNum;
                                }
                                int max = legacyLvUpCfg.m_CostNum;
                                if (currentNum >= max)
                                {//物品够
                                    has = true;
                                    canUpgradeIdList.Add(item_info.ItemId);
                                }
                            }
                        }
                    }
                }
            }
        }
        return has;
    }
}
