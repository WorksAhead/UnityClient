using ArkCrossEngine;
using System.Collections.Generic;
public class ArtifactOperation : UnityEngine.MonoBehaviour
{
    private int m_CostItemId = -1;
    public UITexture texCostItem = null;//消耗物品图
    public UILabel lblCostItemName = null;//消耗物品名
    private int m_CurrentArtifactId = -1;// 当前神器id
    public UIButton btnLevelUp = null;//升级按钮
    public UnityEngine.Color colorLight = UnityEngine.Color.white;
    public UnityEngine.Color colorAsh = UnityEngine.Color.grey;
    public UILabel lblCostItemCount = null;//当前剩余物品数
    public UIProgressBar uiProgressBar = null;// 进度条
    public UILabel lblCostItemInfo = null;//需要数量
    public UILabel lblLegacyLevel = null; //等级
                                          // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    // 设置操作区信息
    public void SetOperationInfo(int currentArtifactId)
    {
        m_CurrentArtifactId = currentArtifactId;
        ItemDataInfo data_info = GetArtifactInfoById(m_CurrentArtifactId);
        if (data_info != null)
        {
            LegacyLevelupConfig legacyLvUpCfg = LegacyLevelupConfigProvider.Instance.GetDataById(data_info.Level);
            if (legacyLvUpCfg != null)
            {
                int legacyIndex = GetArfifactIndex(m_CurrentArtifactId);
                if (legacyIndex >= 0 && legacyIndex < legacyLvUpCfg.m_CostItemList.Count)
                {
                    int costitemId = legacyLvUpCfg.m_CostItemList[legacyIndex];
                    int currentNum = 0;
                    ItemDataInfo costItemDataInfo = GetItemDataInfoById(costitemId);

                    if (costItemDataInfo != null)
                    {
                        currentNum = costItemDataInfo.ItemNum;
                    }
                    SetCostItem(costitemId);
                    DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Artifact, this.gameObject, costitemId, currentNum);
                    int max = legacyLvUpCfg.m_CostNum;
                    //设置按钮的状态
                    if (currentNum >= max)
                    {
                        if (btnLevelUp != null)
                        {
                            UISprite sp = btnLevelUp.GetComponent<UISprite>();
                            if (sp != null)
                                sp.color = colorLight;
                            btnLevelUp.enabled = true;
                            btnLevelUp.isEnabled = true;
                        }
                    }
                    else
                    {
                        if (btnLevelUp != null)
                        {
                            UISprite sp = btnLevelUp.GetComponent<UISprite>();
                            if (sp != null)
                                sp.color = colorAsh;
                            btnLevelUp.enabled = false;
                            btnLevelUp.isEnabled = false;
                        }
                    }
                    if (lblCostItemCount != null)
                        lblCostItemCount.text = "X " + currentNum.ToString();
                    if (uiProgressBar != null && max != 0)
                    {
                        uiProgressBar.value = (float)currentNum / max;
                    }
                    //设置等级信息及所需物品的数量
                    if (lblCostItemInfo != null)
                        lblCostItemInfo.text = currentNum + " / " + max.ToString();
                    if (lblLegacyLevel != null)
                        lblLegacyLevel.text = "Lv." + data_info.Level;

                }
            }
        }
    }

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
    //设置消耗物品
    public void SetCostItem(int itemId)
    {
        m_CostItemId = itemId;

        ItemConfig itemCfg = ItemConfigProvider.Instance.GetDataById(itemId);
        if (itemCfg != null)
        {
            if (lblCostItemName != null)
                lblCostItemName.text = itemCfg.m_ItemName;
        }
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
    //点击物品
    public void OnItemBtnClick()
    {
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("ItemSourceTips");
        if (go != null && !NGUITools.GetActive(go))
        {
            UIItemSourceTips ip = go.GetComponent<UIItemSourceTips>();
            ip.InitSourceTips(m_CostItemId);
        }
    }
    //点击去解锁自动寻路去关卡
    public void OnUnlockSearchClick()
    {
        UIManager.Instance.HideWindowByName("ArtifactPanel");
        UnityEngine.GameObject goc = UIManager.Instance.GetWindowGoByName("SceneSelect");
        if (goc != null)
        {
            LogicSystem.SendStoryMessage("cityplayermove", 0);//寻路
            UISceneSelect uss = goc.GetComponent<UISceneSelect>();
            if (uss != null)
            {
                MyDictionary<int, object> missDataDic = new MyDictionary<int, object>();
                missDataDic = MissionConfigProvider.Instance.GetData();
                foreach (MissionConfig cfg in missDataDic.Values)
                {
                    if (cfg.UnlockLegacyId == m_CurrentArtifactId)
                    {
                        uss.startChapterId = cfg.SceneId;
                    }
                }
            }
        }
    }
    //升级
    public void OnLevelUpButtonClick()
    {
        ItemDataInfo data_info = GetArtifactInfoById(m_CurrentArtifactId);
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        if (data_info != null)
        {
            LegacyLevelupConfig legacyLvUpCfg = LegacyLevelupConfigProvider.Instance.GetDataById(data_info.Level);
            if (legacyLvUpCfg != null)
            {
                int legacyIndex = GetArfifactIndex(m_CurrentArtifactId);
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

                    if (data_info.Level >= role.Level)
                    {//神器等级大于等于角色等级时
                        SendDialog(410, 4, null);
                        return;
                    }
                    if (currentNum >= max)
                    {
                        LogicSystem.PublishLogicEvent("ge_upgrade_legacy", "lobby", legacyIndex, m_CurrentArtifactId, false);
                    }
                    else
                    {
                        //物品不够、钻来凑
                        SendDialog(411, 4, null);
                        //int DiamondNum = UnityEngine.Mathf.CeilToInt(legacyLvUpCfg.m_Rate * (max - currentNum));
                        //RoleInfo role_info = LobbyClient.Instance.CurrentRole;
                        //string CHN_CONFIRM = StrDictionaryProvider.Instance.GetDictString(4);//确定
                        //string CHN_CANCEL = StrDictionaryProvider.Instance.GetDictString(9);//取消
                        //string CHN_LEVELUP = StrDictionaryProvider.Instance.GetDictString(404);//升级
                        //if (role_info != null && role_info.Gold >= DiamondNum) {
                        //  ArkCrossEngine.MyAction<int> Func = HandleDialog;
                        //  string CHN_DESC = StrDictionaryProvider.Instance.GetDictString(401);
                        //  CHN_DESC = string.Format(CHN_DESC, DiamondNum);
                        //  LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", CHN_DESC, null, CHN_LEVELUP, CHN_CANCEL, Func, false);
                        //} else {
                        //  string CHN_DESC = StrDictionaryProvider.Instance.GetDictString(402);
                        //  CHN_DESC = string.Format(CHN_DESC, DiamondNum);
                        //  LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", CHN_DESC, CHN_CONFIRM, null, null, null, false);
                        //}
                    }
                }
            }
        }
    }
    //回调
    void HandleDialog(int action)
    {
        int legacyIndex = GetArfifactIndex(m_CurrentArtifactId);
        if (action == 1)
        {
            LogicSystem.PublishLogicEvent("ge_upgrade_legacy", "lobby", legacyIndex, m_CurrentArtifactId, false);
        }
    }
    /*发送对话框*/
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
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", str, confirm, null, null, null, false);
    }
}
