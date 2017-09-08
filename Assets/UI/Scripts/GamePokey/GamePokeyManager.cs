using UnityEngine;
using ArkCrossEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GamePokeyManager : UnityEngine.MonoBehaviour
{
    public UIGridForDFM gridEquip = null;
    public UIGridForDFM gridWingTop = null;
    public UIGridForDFM gridWingBotton = null;
    public UIGridForDFM gridChip = null;
    private bool isFirstInitItems = true;
    private List<int> wingIdList = new List<int>();
    private List<ChipItem> chipItemList = new List<ChipItem>();

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
            //GamePokey
            if (eventlist != null) { eventlist.Clear(); }
            if (changeitemDic != null) { changeitemDic.Clear(); }
            object eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int[], int[], int[]>("ge_add_item", "bag", AddItem);
            if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int[], int[], int[], ArkCrossEngine.Network.GeneralOperationResult>("ge_delete_item", "bag", DeleteItemInCheck);
            if (eo != null) { eventlist.Add(eo); }
            //     eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, int>("ge_delete_item", "equipment", DeleteInEquipment);
            //     if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (eo != null) eventlist.Add(eo);
            //HeroEquipment
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, int, int, int, ArkCrossEngine.Network.GeneralOperationResult>("ge_fiton_equipment", "equipment", HeroPutOnEquipment);
            if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, int, int, int, ArkCrossEngine.Network.GeneralOperationResult>("ge_upgrade_item", "equipment", UpgradeItem);
            if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<ArkCrossEngine.CharacterProperty, ArkCrossEngine.Network.GeneralOperationResult>("ge_request_role_property", "property", HeroUpdateProperty);
            if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_update_role_dynamic_property", "ui", UpdateDynamicProperty);
            if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<List<NewEquipInfo>>("ge_new_equip", "equipment", NewEquipment);
            if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_entrance_gold_change", "ui", CheckHasUpgrade);
            if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("ge_user_levelup", "property", CheckHasUpgradeLvUp);
            if (eo != null) { eventlist.Add(eo); }
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<ArkCrossEngine.Network.GeneralOperationResult>("ge_compound_equip", "compound", CompoundResult);
            if (eo != null) { eventlist.Add(eo); }

            //RequestGamePokey
            try
            {
                ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_request_items", "ui");
            }
            catch (System.Exception ex)
            {
                ArkCrossEngine.LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
            SetRoleStaticProperty();
            RecordSomeWidget();
            SetRoleDynamicProperty();
            InitEquipmentTransform();
            UIManager.Instance.HideWindowByName("GamePokey");

            //money
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (signForInitPos)
            {
                SetInitPosition();
                signForInitPos = false;
            }
            SetRoleDynamicProperty();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void CheckHasUpgradeLvUp(int lv)
    {
        CheckHasUpgrade();
    }

    private void CheckHasUpgrade()
    {
        bool has = false;
        ArkCrossEngine.RoleInfo roleInfo = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
        for (var i = 0; i < CanUpgradeCount; i++)
        {
            bool showTip = false;
            if (equiparry[i].hasEquip && equiparry[i].level < roleInfo.Level)
            {
                ArkCrossEngine.ItemLevelupConfig config = ArkCrossEngine.ItemLevelupConfigProvider.Instance.GetDataById(equiparry[i].level);
                if (config != null)
                {
                    float needMoney = config.m_PartsList.Count > i ? config.m_PartsList[i] : 0;
                    if (roleInfo.Money >= needMoney)
                    {
                        has = true;
                        showTip = true;
                    }
                }
            }
            ShowEquipUpgradeTip(i, showTip);
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.Equip_Upgrade, has);
    }

    private void ShowEquipUpgradeTip(int pos, bool showTip)
    {
        UnityEngine.Transform tf = transform.Find("HeroEquip/Equipment/Slot" + pos + "/UpgradeTip");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, showTip);
        }
    }

    void SetRoleStaticProperty()
    {
        ArkCrossEngine.RoleInfo player = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
        if (player != null)
        {
            UnityEngine.Transform tf = transform.Find("Head/name");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = player.Nickname;
                }
            }
            tf = transform.Find("chartexture");
            if (tf != null)
            {
                UISprite us = tf.gameObject.GetComponent<UISprite>();
                if (us != null)
                {
                    if (player.HeroId == 1)
                    {
                        us.spriteName = "fa-shi";
                    }
                    else
                    {
                        us.spriteName = "ci-ke";
                    }
                }
            }
            tf = transform.Find("Head/headPic");
            if (tf != null)
            {
                UISprite us = tf.gameObject.GetComponent<UISprite>();
                if (us != null)
                {
                    if (player.HeroId == 1)
                    {
                        us.spriteName = "jianshi";
                    }
                    else
                    {
                        us.spriteName = "cike";
                    }
                }
            }
        }
    }

    void RecordSomeWidget()
    {
        UnityEngine.Transform tf = transform.Find("Head/level");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                labellv = ul;
            }
        }
        tf = transform.Find("HeroEquip/Title/LabelSword");
        if (tf != null)
        {
            fightlabellocalpos = tf.position;
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                labellvfight = ul;
            }
        }
        tf = transform.Find("HeroEquip/Title/LabelSome");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                labelexp = ul;
            }
        }
        tf = transform.Find("HeroEquip/Title/ProgressBarBack");
        if (tf != null)
        {
            UIProgressBar upb = tf.gameObject.GetComponent<UIProgressBar>();
            if (upb != null)
            {
                progressupb = upb;
            }
        }
        tf = transform.Find("Money/Label");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                money = ul;
            }
        }
        tf = transform.Find("Diamond/Label");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                diamond = ul;
            }
        }
        tf = transform.Find("HeroEquip/Title/LabelLove");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                labelhpmax = ul;
            }
        }
    }

    void SetRoleDynamicProperty()
    {
        ArkCrossEngine.RoleInfo player = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
        if (player != null)
        {
            if (labellv != null)
            {
                labellv.text = "" + player.Level;
            }
            RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
            if (ri != null)
            {
                UserInfo ui = ri.GetPlayerSelfInfo();
                if (ui != null)
                {
                    CharacterProperty cp = ui.GetActualProperty();
                    if (cp != null)
                    {
                        if (labellvfight != null)
                        {
                            labellvfight.text = cp.AttackBase.ToString();
                        }
                    }
                }
            }

            if (money != null)
            {
                money.text = player.Money.ToString();
            }
            if (diamond != null)
            {
                diamond.text = player.Gold.ToString();
            }
            int needexp = 0;
            ArkCrossEngine.PlayerLevelupExpConfig plec = ArkCrossEngine.PlayerConfigProvider.Instance.GetPlayerLevelupExpConfigById(player.HeroId);
            if (plec != null)
            {
                needexp = plec.m_ConsumeExp;
            }

            UpdateEx(player.Level, player.Exp);

            if (labelhpmax != null)
            {
                ArkCrossEngine.UserInfo ui = player.GetPlayerSelfInfo();
                if (ui != null)
                {
                    ArkCrossEngine.CharacterProperty cp = ui.GetActualProperty();
                    if (cp != null)
                    {
                        labelhpmax.text = cp.HpMax.ToString();
                    }
                }
            }
        }
    }

    //更新经验值
    public void UpdateEx(int level, int exp)
    {
        int curent = 0, max = 0;
        int baseExp = 0;
        if (level == 1)
        {
            baseExp = 0;
        }
        else
        {
            PlayerLevelupExpConfig expCfg = PlayerConfigProvider.Instance.GetPlayerLevelupExpConfigById(level - 1);
            if (expCfg != null)
                baseExp = expCfg.m_ConsumeExp;
        }
        PlayerLevelupExpConfig expCfgHith = PlayerConfigProvider.Instance.GetPlayerLevelupExpConfigById(level);
        if (expCfgHith != null)
        {
            max = expCfgHith.m_ConsumeExp - baseExp;
        }
        curent = exp - baseExp;
        if (progressupb != null && max != 0)
        {
            progressupb.value = curent / (float)max;
        }
        if (labelexp != null && max != 0)
        {
            labelexp.text = curent.ToString() + "/" + max;
        }
    }

    void UpgradeItem(int pos, int id, int itemlevel, int item_random_property, ArkCrossEngine.Network.GeneralOperationResult result)
    {
        try
        {
            if (ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed == result)
            {
                HeroPutOnEquipment(id, pos, itemlevel, item_random_property, ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed);
                UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("ItemProperty");
                if (go != null)
                {
                    ItemProperty ip = go.GetComponent<ItemProperty>();
                    if (ip != null)
                    {
                        ip.SetItemProperty(id, pos, itemlevel, item_random_property, true);
                    }
                }
            }
            else
            {
                //ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", result.ToString(), "understand", null, null, null, false);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void SetInitPosition()
    {
        if (gridEquip != null)
        {
            UIGridForDFM ug = gridEquip.gameObject.GetComponent<UIGridForDFM>();
            if (ug != null)
            {
                ug.repositionNow = true;
            }
        }
        UnityEngine.Transform tf = transform.Find("PokeyContainer/EquipContainer/Control_SimpleVerticalScrollBar");
        if (tf != null)
        {
            UIScrollBar usb = tf.gameObject.GetComponent<UIScrollBar>();
            if (usb != null)
            {
                usb.value = 0;
            }
        }
    }

    public void AddItem(int[] item, int[] item_num, int[] item_append_property)
    {
        try
        {
            if (isFirstInitItems)
            {
                wingIdList.Clear();
                RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
                if (roleInfo != null)
                {
                    ItemDataInfo[] equips = roleInfo.Equips;
                    if (equips != null)
                    {
                        foreach (ItemDataInfo info in equips)
                        {
                            ItemConfig config = ItemConfigProvider.Instance.GetDataById(info.ItemId);
                            if (config != null)
                            {
                                if (config.m_WearParts == 7)
                                {
                                    //wingIdList.Add(info.ItemId);// todo 暂时注释
                                    //AddWingGo(config, info.ItemId, info.RandomProperty, true);
                                }
                            }
                        }
                    }
                }
                isFirstInitItems = false;
            }
            int count = item.Length;
            for (int i = 0; i < count; ++i)
            {
                int itemcell = item[i];
                int itemNum = item_num[i];
                ArkCrossEngine.ItemConfig item_data = ArkCrossEngine.ItemConfigProvider.Instance.GetDataById(itemcell);
                if (null != item_data)
                {
                    if (item_data.m_CanWear)
                    {
                        //if (item_data.m_WearParts == 7) {//翅膀// todo 暂时注释
                        //  if (wingIdList.IndexOf(itemcell) == -1) {
                        //    wingIdList.Add(itemcell);
                        //    AddWingGo(item_data, itemcell, item_append_property[i], true);
                        //  }
                        //} else {
                        AddEquipGo(item_data, itemcell, item_append_property[i]);
                        //}
                    }
                    else if (item_data.m_CompoundItemId.Count > 0 && item_data.m_CompoundItemId[0] > 0)
                    {//可合成
                        ChipItem ci = null;
                        foreach (ChipItem citem in chipItemList)
                        {
                            if (citem.Id() == itemcell)
                            {
                                ci = citem;
                            }
                        }
                        if (ci == null)
                        {
                            AddChipGo(itemcell, item_append_property[i], itemNum);
                        }
                        else
                        {
                            //update
                            ci.UpdateView(itemNum);
                        }
                        CheckHasCompound();
                    }
                }
            }
            if (gridEquip != null)
            {
                UIGridForDFM ug = gridEquip.gameObject.GetComponent<UIGridForDFM>();
                if (ug != null)
                {
                    ug.sortRepositionForDF = true;
                }
            }
            if (gridWingTop != null)
            {
                UIGridForDFM ug = gridWingTop.gameObject.GetComponent<UIGridForDFM>();
                if (ug != null)
                {
                    ug.sortRepositionForDF = true;
                }
            }
            if (gridChip != null)
            {
                UIGridForDFM ug = gridChip.gameObject.GetComponent<UIGridForDFM>();
                if (ug != null)
                {
                    ug.sortRepositionForDF = true;
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void AddChipGo(int itemId, int property, int itemNum)
    {
        UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/GamePokey/ChipCell"));
        go = NGUITools.AddChild(gridChip.gameObject, go);
        if (go != null)
        {
            // geng xin item
            ChipItem ci = go.GetComponent<ChipItem>();
            if (ci != null)
            {
                //int fs = (int)GetItemFightScore(item_data, append_property, 1);
                ci.SetItemInformation(itemId, property, itemNum);
                chipItemList.Add(ci);
            }
            ////记录物品id
            //ItemClick ic = go.GetComponent<ItemClick>();
            //if (ic != null) {
            //  ic.ID = itemId;
            //  //ic.PropertyId = append_property;
            //}
            //添加物品后更改控件名，便于后续工作
            go.transform.name = "Item" + itemcount++;
        }
    }

    private void AddEquipGo(ItemConfig item_data, int itemcell, int p)
    {
        UnityEngine.GameObject go = ArkCrossEngine.ResourceSystem.GetSharedResource("UI/GamePokey/ItemCell") as UnityEngine.GameObject;
        go = NGUITools.AddChild(gridEquip.gameObject, go);
        if (go != null)
        {
            //更改itemcell上的物品信息
            SetItemInformation(go, itemcell, p);
            //记录物品id
            ItemClick ic = go.GetComponent<ItemClick>();
            if (ic != null)
            {
                ic.ID = itemcell;
                ic.PropertyId = p;
            }
            //添加物品后更改控件名，便于后续工作
            go.transform.name = "Item" + itemcount++;
        }
    }

    private void AddWingGo(ItemConfig item_data, int itemId, int append_property, bool hasOwn)
    {
        UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/GamePokey/WingCell"));
        if (gridWingTop != null)
        {
            if (go != null)
            {
                go = NGUITools.AddChild(gridWingTop.gameObject, go);
                // geng xin item
                WingItem wi = go.GetComponent<WingItem>();
                if (wi != null)
                {
                    int fs = (int)GetItemFightScore(item_data, append_property, 1);
                    wi.SetItemInformation(itemId, append_property, fs, true);
                }
                //记录物品id
                ItemClick ic = go.GetComponent<ItemClick>();
                if (ic != null)
                {
                    ic.ID = itemId;
                    ic.PropertyId = append_property;
                }
                //添加物品后更改控件名，便于后续工作
                go.transform.name = "Item" + itemcount++;
            }
        }
    }

    private void CompoundResult(ArkCrossEngine.Network.GeneralOperationResult result)
    {
        if (result == ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed)
        {
            RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
            if (roleInfo != null)
            {
                for (int i = chipItemList.Count - 1; i >= 0; i--)
                {
                    ItemDataInfo itemData = roleInfo.GetItemData(chipItemList[i].Id(), chipItemList[i].Property());
                    if (itemData == null || itemData.ItemNum <= 0)
                    {
                        NGUITools.DestroyImmediate(chipItemList[i].gameObject);
                        chipItemList.RemoveAt(i);
                    }
                    else
                    {
                        chipItemList[i].UpdateView(itemData.ItemNum);
                    }
                }
            }
            CheckHasCompound();
        }
        else
        {
            Debug.Log(result);
        }
    }

    private void CheckHasCompound()
    {
        bool has = false;
        RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
        if (roleInfo != null)
        {
            for (int i = chipItemList.Count - 1; i >= 0; i--)
            {
                if (chipItemList[i].CanCompound)
                {
                    has = true;
                    break;
                }
            }
        }
        GamePokeyButtonEvent gbe = transform.GetComponent<GamePokeyButtonEvent>();
        if (gbe != null)
        {
            gbe.CanCompound = has;
        }
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_systemnewtip_state_change", "ui", SystemNewTipType.Equip_Tip, has);
    }

    public void DeleteItemInCheck(int[] item, int[] item_property_id, int[] item_num, ArkCrossEngine.Network.GeneralOperationResult result)
    {
        try
        {
            if (ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed == result)
            {
                UnityEngine.Transform tfc = null;
                for (int j = 0; j < item.Length; ++j)
                {
                    ItemConfig config = ItemConfigProvider.Instance.GetDataById(item[j]);
                    if (gridEquip != null)
                    {
                        for (int i = 0; i < gridEquip.transform.childCount; ++i)
                        {
                            tfc = gridEquip.transform.GetChild(i);
                            if (tfc != null)
                            {
                                ItemClick ic = tfc.gameObject.GetComponent<ItemClick>();
                                if (ic != null && ic.ID == item[j] && ic.PropertyId == item_property_id[j])
                                {
                                    NGUITools.DestroyImmediate(tfc.gameObject);
                                    break;
                                }
                            }
                        }
                    }
                    if (gridWingTop != null)
                    {
                        if (config != null && config.m_WearParts == 7)
                        {//翅膀
                            for (int k = 0; k < gridWingTop.transform.childCount; ++k)
                            {
                                tfc = gridWingTop.transform.GetChild(k);
                                WingItem witem = tfc.GetComponent<WingItem>();
                                if (witem != null)
                                {
                                    witem.UpdateTopView();
                                }
                            }
                        }
                    }

                }
                if (gridEquip != null)
                {
                    UIGridForDFM ug = gridEquip.gameObject.GetComponent<UIGridForDFM>();
                    if (ug != null)
                    {
                        ug.repositionNow = true;
                    }
                }
            }
            else
            {
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", result.ToString(), "YES", null, null, null, false);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void ArrangedItem()
    {
        if (gridEquip != null)
        {
            UIGridForDFM ug = gridEquip.gameObject.GetComponent<UIGridForDFM>();
            if (ug != null)
            {
                //排序，规则未制定
                ug.sortRepositionForDF = true;
            }
        }
    }

    private void SetItemInformation(UnityEngine.GameObject go, int id, int propertyid)
    {
        ArkCrossEngine.ItemConfig itemconfig = ArkCrossEngine.LogicSystem.GetItemDataById(id);
        if (itemconfig == null) return;

        if (go != null)
        {
            DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Equip_List, go, id);
            UnityEngine.Transform tf;
            RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
            ItemDataInfo[] equips = null;
            if (roleInfo != null)
            {
                equips = roleInfo.Equips;
            }
            int _fightScore = (int)GetItemFightScore(itemconfig, propertyid, 1);
            int _equipFingScore = 0;
            if (equips != null)
            {
                foreach (ItemDataInfo info in equips)
                {
                    ItemConfig equipConfig = LogicSystem.GetItemDataById(info.ItemId);
                    if (equipConfig != null)
                    {
                        if (equipConfig.m_WearParts == itemconfig.m_WearParts)
                        {
                            _equipFingScore = (int)GetItemFightScore(equipConfig, info.RandomProperty, info.Level);
                            _fightScore = (int)GetItemFightScore(itemconfig, propertyid, info.Level);
                        }
                    }
                }
            }
            tf = go.transform.Find("Up");
            if (tf != null)
            {
                UISprite sp = tf.GetComponent<UISprite>();
                if (sp != null)
                {
                    sp.spriteName = _fightScore >= _equipFingScore ? "Up" : "down";
                }
            }
            tf = go.transform.Find("Up/value");
            if (tf != null)
            {
                UILabel label = tf.GetComponent<UILabel>();
                if (label != null)
                {
                    label.text = Math.Abs(_fightScore - _equipFingScore).ToString();
                    label.color = _fightScore >= _equipFingScore ? new UnityEngine.Color(0, 251 / 255f, 75 / 255f) : new UnityEngine.Color(1, 0, 0);
                }
            }

            ItemClick script = go.GetComponent<ItemClick>();
            if (script != null)
            {
                script.fightScoreChange = _fightScore - _equipFingScore;
            }

            tf = go.transform.Find("LabelOccupation");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = itemconfig.m_ItemType;
                }
            }
        }
    }

    private float GetItemFightScore(int itemid, int propertyid, int itemlevel)
    {
        ArkCrossEngine.ItemConfig itemconfig = ArkCrossEngine.LogicSystem.GetItemDataById(itemid);
        if (itemconfig != null)
        {
            return GetItemFightScore(itemconfig, propertyid, itemlevel);
        }
        return 0f;
    }

    private float GetItemFightScore(ArkCrossEngine.ItemConfig itemconfig, int propertyid, int itemlevel)
    {
        ArkCrossEngine.RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
        CharacterProperty cp = ri.GetPlayerSelfInfo().GetActualProperty();
        if (itemconfig != null && ri != null && cp != null)
        {
            ArkCrossEngine.AppendAttributeConfig aac = ArkCrossEngine.AppendAttributeConfigProvider.Instance.GetDataById(propertyid);
            if (aac == null)
            {
                return (ArkCrossEngine.AttributeScoreConfigProvider.Instance.CalcAttributeScore(
                  itemconfig.m_AttrData.GetAddHpMax(cp.HpMax, ri.Level, itemlevel), itemconfig.m_AttrData.GetAddEpMax(cp.EnergyMax, ri.Level, itemlevel),
                  itemconfig.m_AttrData.GetAddAd(cp.AttackBase, ri.Level, itemlevel), itemconfig.m_AttrData.GetAddADp(cp.ADefenceBase, ri.Level, itemlevel),
                  itemconfig.m_AttrData.GetAddMDp(cp.MDefenceBase, ri.Level, itemlevel), itemconfig.m_AttrData.GetAddCri(cp.Critical, ri.Level),
                  itemconfig.m_AttrData.GetAddPow(cp.CriticalPow, ri.Level), itemconfig.m_AttrData.GetAddBackHitPow(cp.CriticalBackHitPow, ri.Level),
                  itemconfig.m_AttrData.GetAddCrackPow(cp.CriticalCrackPow, ri.Level), itemconfig.m_AttrData.GetAddFireDam(cp.FireDamage, ri.Level),
                  itemconfig.m_AttrData.GetAddIceDam(cp.IceDamage, ri.Level), itemconfig.m_AttrData.GetAddPoisonDam(cp.PoisonDamage, 1),
                  itemconfig.m_AttrData.GetAddFireErd(cp.FireERD, ri.Level), itemconfig.m_AttrData.GetAddIceErd(cp.IceERD, ri.Level),
                  itemconfig.m_AttrData.GetAddPoisonErd(cp.PoisonERD, ri.Level)));
            }
            else
            {
                return (ArkCrossEngine.AttributeScoreConfigProvider.Instance.CalcAttributeScore(
                 itemconfig.m_AttrData.GetAddHpMax(cp.HpMax, ri.Level, itemlevel) + aac.GetAddHpMax(cp.HpMax, ri.Level),
                 itemconfig.m_AttrData.GetAddEpMax(cp.EnergyMax, ri.Level, itemlevel) + aac.GetAddEpMax(cp.EnergyMax, ri.Level),
                 itemconfig.m_AttrData.GetAddAd(cp.AttackBase, ri.Level, itemlevel) + aac.GetAddAd(cp.AttackBase, ri.Level),
                 itemconfig.m_AttrData.GetAddADp(cp.ADefenceBase, ri.Level, itemlevel) + aac.GetAddADp(cp.ADefenceBase, itemlevel),
                 itemconfig.m_AttrData.GetAddMDp(cp.MDefenceBase, ri.Level, itemlevel) + aac.GetAddMDp(cp.MDefenceBase, ri.Level),
                 itemconfig.m_AttrData.GetAddCri(cp.Critical, ri.Level) + aac.GetAddCri(cp.Critical, ri.Level),
                 itemconfig.m_AttrData.GetAddPow(cp.CriticalPow, ri.Level) + aac.GetAddPow(cp.CriticalPow, ri.Level),
                 itemconfig.m_AttrData.GetAddBackHitPow(cp.CriticalBackHitPow, ri.Level) + aac.GetAddBackHitPow(cp.CriticalBackHitPow, ri.Level),
                 itemconfig.m_AttrData.GetAddCrackPow(cp.CriticalCrackPow, ri.Level) + aac.GetAddCrackPow(cp.CriticalCrackPow, ri.Level),
                 itemconfig.m_AttrData.GetAddFireDam(cp.FireDamage, ri.Level) + aac.GetAddFireDam(cp.FireDamage, ri.Level),
                 itemconfig.m_AttrData.GetAddIceDam(cp.IceDamage, ri.Level) + aac.GetAddIceDam(cp.IceDamage, ri.Level),
                 itemconfig.m_AttrData.GetAddPoisonDam(cp.PoisonDamage, ri.Level) + aac.GetAddPoisonDam(cp.PoisonDamage, ri.Level),
                 itemconfig.m_AttrData.GetAddFireErd(cp.FireERD, ri.Level) + aac.GetAddFireDam(cp.FireERD, ri.Level),
                 itemconfig.m_AttrData.GetAddIceErd(cp.IceERD, ri.Level) + aac.GetAddIceErd(cp.IceERD, ri.Level),
                 itemconfig.m_AttrData.GetAddPoisonErd(cp.PoisonERD, ri.Level) + aac.GetAddPoisonErd(cp.PoisonERD, ri.Level)));
            }
        }
        return 0f;
    }

    private void UpdateDynamicProperty()
    {
        try
        {
            SetRoleDynamicProperty();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void HeroPutOnEquipment(int id, int pos, int itemLevel, int itemRandomProperty, ArkCrossEngine.Network.GeneralOperationResult result)
    {
        try
        {
            if (ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed == result)
            {
                if (pos >= 0 && pos < 8)
                {
                    equiparry[pos].SetEquipmentInfo(id, itemLevel, itemRandomProperty);
                    UpdateRelateEquipFight(pos);
                    CheckHasUpgrade();
                }
            }
            else
            {
                if (ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_Position == result)
                {
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(151),
                    ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(140), null, null, null, false);
                }
                else if (ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_LevelError == result)
                {
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(173),
                    ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(140), null, null, null, false);
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void UpdateRelateEquipFight(int pos)
    {
        if (gridEquip != null)
        {
            int num = gridEquip.transform.childCount;
            for (int i = 0; i < num; i++)
            {
                UnityEngine.Transform tfItem = gridEquip.transform.GetChild(i);
                ItemClick ic = tfItem.GetComponent<ItemClick>();
                if (ic != null && ic.ID > 0)
                {
                    //int fs = (int)GetItemFightScore(ic.ID, ic.PropertyId, 1);
                    ItemConfig config = ItemConfigProvider.Instance.GetDataById(ic.ID);
                    if (config != null && config.m_WearParts == pos)
                    {
                        int packFs = (int)GetItemFightScore(ic.ID, ic.PropertyId, equiparry[pos].level);
                        int equipFs = (int)GetItemFightScore(equiparry[pos].id, equiparry[pos].propertyid, equiparry[pos].level);
                        UnityEngine.Transform tf = tfItem.Find("Up");
                        if (tf != null)
                        {
                            UISprite sp = tf.GetComponent<UISprite>();
                            if (sp != null)
                            {
                                sp.spriteName = packFs >= equipFs ? "Up" : "down";
                            }
                        }
                        tf = tfItem.Find("Up/value");
                        if (tf != null)
                        {
                            UILabel label = tf.GetComponent<UILabel>();
                            if (label != null)
                            {
                                label.text = Math.Abs(packFs - equipFs).ToString();
                                label.color = packFs >= equipFs ? new UnityEngine.Color(0, 251 / 255f, 75 / 255f) : new UnityEngine.Color(1, 0, 0);
                            }
                        }
                    }
                }
            }
        }
    }

    private void HeroUpdateProperty(ArkCrossEngine.CharacterProperty info, ArkCrossEngine.Network.GeneralOperationResult result)
    {
        try
        {
            if (result == ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed)
            {
                UnityEngine.Transform tfroot = transform;
                if (tfroot != null)
                {
                    UnityEngine.Transform tf = tfroot.Find("RoleInfo/DragThing/Right0");
                    if (tf != null)
                    {
                        UILabel ul = tf.gameObject.GetComponent<UILabel>();
                        if (ul != null)
                        {
                            string str = "[ffffff]";
                            if (info != null)
                            {
                                str += info.AttackBase + "\n";
                                str += FormatDecimal(info.Critical * 100) + "%\n";
                                str += FormatDecimal(info.CriticalPow * 100) + "%\n";
                                str += FormatDecimal(info.CriticalBackHitPow * 100) + "%\n";
                                str += FormatDecimal(info.CriticalCrackPow * 100) + "%\n";
                                str += FormatDecimal(info.FireDamage) + "\n";
                                str += FormatDecimal(info.IceDamage) + "\n";
                                str += FormatDecimal(info.PoisonDamage) + "[-]";
                            }
                            ul.text = str;
                        }
                    }
                    tf = tfroot.Find("RoleInfo/DragThing/Right1");
                    if (tf != null)
                    {
                        UILabel ul = tf.gameObject.GetComponent<UILabel>();
                        if (ul != null)
                        {
                            string str = "[ffffff]";
                            if (info != null)
                            {
                                str += info.HpMax + "\n";
                                str += info.ADefenceBase + "\n";
                                str += info.MDefenceBase + "\n";
                                str += FormatDecimal(info.FireERD) + "\n";
                                str += FormatDecimal(info.IceERD) + "\n";
                                str += FormatDecimal(info.PoisonERD) + "[-]";
                            }
                            ul.text = str;
                        }
                    }
                    tf = tfroot.Find("RoleInfo/DragThing/Right2");
                    if (tf != null)
                    {
                        UILabel ul = tf.gameObject.GetComponent<UILabel>();
                        if (ul != null)
                        {
                            string str = "[ffffff]";
                            if (info != null)
                            {
                                str += "+" + FormatDecimal(info.MoveSpeed * 100) + "%\n";
                                str += FormatDecimal(info.HpRecover) + "/5S\n";
                                str += FormatDecimal(info.EnergyRecover) + "/5S\n";
                                str += "+" + /*info.EnergyRecover * 10*/0 + "%\n";
                            }
                            ul.text = str;
                        }
                    }
                }
            }
            else
            {
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", result.ToString(), "understand", null, null, null, false);
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private float FormatDecimal(float f)
    {
        string temp = f.ToString("f1");//精确到1位（四舍五入）
        return float.Parse(temp);//转为float 可去掉.0的情况
    }

    private void InitEquipmentTransform()
    {
        UnityEngine.Transform tf = transform.Find("HeroEquip/Equipment");
        if (tf != null)
        {
            for (int i = 0; i < 8; ++i)
            {
                UnityEngine.Transform tf1 = tf.Find("Slot" + i);
                equiparry[i].equipSlot = tf1.gameObject;
            }
        }
    }
    private void NewEquipment(List<NewEquipInfo> neil)
    {
        try
        {
            if (neil != null && neil.Count > 0 && changeitemDic != null)
            {
                ArkCrossEngine.ItemConfig ic = null;
                for (int i = 0; i < neil.Count; i++)
                {
                    if (neil[i] != null)
                    {
                        ic = ArkCrossEngine.ItemConfigProvider.Instance.GetDataById(neil[i].ItemId);
                        if (ic != null)
                        {
                            if (changeitemDic.ContainsKey(ic.m_WearParts))
                            {
                                ChangeNewEquip cne = changeitemDic[ic.m_WearParts];
                                if (cne != null)
                                {
                                    float score = GetItemFightScore(neil[i].ItemId, neil[i].ItemRandomProperty, cne.needlevel);
                                    if (score > cne.fightscore)
                                    {
                                        changeitemDic[ic.m_WearParts] = new ChangeNewEquip(neil[i].ItemId, neil[i].ItemRandomProperty, score, cne.needlevel);
                                    }
                                }
                            }
                            else if (GetEquipmentInfo(ic.m_WearParts) != null)
                            {
                                EquipmentInfo ei = GetEquipmentInfo(ic.m_WearParts);
                                float score0 = GetItemFightScore(ei.id, ei.propertyid, ei.level);
                                float score1 = GetItemFightScore(neil[i].ItemId, neil[i].ItemRandomProperty, ei.level);
                                if (score0 < score1)
                                {
                                    changeitemDic.Add(ic.m_WearParts, new ChangeNewEquip(neil[i].ItemId, neil[i].ItemRandomProperty, score1, ei.level));
                                }
                            }
                            else
                            {
                                float score2 = GetItemFightScore(neil[i].ItemId, neil[i].ItemRandomProperty, 1);
                                changeitemDic.Add(ic.m_WearParts, new ChangeNewEquip(neil[i].ItemId, neil[i].ItemRandomProperty, score2, 1));
                            }
                        }
                    }
                }
                /*
        foreach (NewEquipInfo nei in neil) {
          if (nei != null) {
            ic = ArkCrossEngine.ItemConfigProvider.Instance.GetDataById(nei.ItemId);
            if (ic != null) {
              if (changeitemDic.ContainsKey(ic.m_WearParts)) {
                ChangeNewEquip cne = changeitemDic[ic.m_WearParts];
                if (cne != null) {
                  float score = GetItemFightScore(nei.ItemId, nei.ItemRandomProperty, cne.needlevel);
                  if (score > cne.fightscore) {
                    changeitemDic[ic.m_WearParts] = new ChangeNewEquip(nei.ItemId, nei.ItemRandomProperty, score, cne.needlevel);
                  }
                }
              } else if (GetEquipmentInfo(ic.m_WearParts) != null) {
                EquipmentInfo ei = GetEquipmentInfo(ic.m_WearParts);
                float score0 = GetItemFightScore(ei.id, ei.propertyid, ei.level);
                float score1 = GetItemFightScore(nei.ItemId, nei.ItemRandomProperty, ei.level);
                if (score0 < score1) {
                  changeitemDic.Add(ic.m_WearParts, new ChangeNewEquip(nei.ItemId, nei.ItemRandomProperty, score1, ei.level));
                }
              } else {
                float score2 = GetItemFightScore(nei.ItemId, nei.ItemRandomProperty, 1);
                changeitemDic.Add(ic.m_WearParts, new ChangeNewEquip(nei.ItemId, nei.ItemRandomProperty, score2, 1));
              }
            }
          }
        }*/
                foreach (ChangeNewEquip cne in changeitemDic.Values)
                {
                    if (cne != null)
                    {
                        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("DynamicEquipment");
                        if (go != null)
                        {
                            DynamicEquipment de = go.GetComponent<DynamicEquipment>();
                            if (de != null)
                            {
                                de.SetEquipment(new ChangeNewEquip(cne.id, cne.propertyid, 0, 0));
                                break;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private bool signForInitPos = true;
    private long itemcount = 99999;

    private UILabel money = null;
    private UILabel labellv = null;
    private UILabel diamond = null;
    private UILabel labellvfight = null;
    private UILabel labelexp = null;
    private UILabel labelhpmax = null;
    private UIProgressBar progressupb = null;
    private UnityEngine.Vector3 fightlabellocalpos;
    private const int CanUpgradeCount = 6;
    static private EquipmentInfo[] equiparry = { new EquipmentInfo(), new EquipmentInfo(),
                                               new EquipmentInfo(), new EquipmentInfo(),
                                               new EquipmentInfo(), new EquipmentInfo(),
                                               new EquipmentInfo(), new EquipmentInfo() };
    static public Dictionary<int, ChangeNewEquip> changeitemDic = new Dictionary<int, ChangeNewEquip>();
    static public EquipmentInfo GetEquipmentInfo(int pos)
    {
        if (pos >= 0 && pos < 8)
        {
            return equiparry[pos];
        }
        else
        {
            return null;
        }
    }

    static public UnityEngine.Texture GetTextureByPicName(string picturename)
    {
        return CrossObjectHelper.TryCastObject<UnityEngine.Texture>(ArkCrossEngine.ResourceSystem.GetSharedResource(picturename));
    }
}
public class EquipmentInfo
{
    public void SetEquipmentInfo(int itemid, int itemlevel, int itempro, int type = 0)
    {
        hasEquip = itemid > 0 ? true : false;
        id = itemid;
        level = itemlevel;
        propertyid = itempro;
        if (type == 0)
        {
            DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Equip_slot, equipSlot, id);
        }
        else if (type == 1)
        {
            DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.FightInfo_slot, equipSlot, id);
        }
    }
    public void SetFrameAndIcon(string frame)
    {
        UISprite us = equipSlot.GetComponent<UISprite>();
        if (us != null)
            us.spriteName = frame;
    }
    public bool hasEquip = false;
    public int id = 0;
    public int level = 0;
    public int propertyid = 0;
    public UnityEngine.GameObject equipSlot;
    //public UITexture Itemtexture = null;
    private UISprite Framesprite = null;
}
public class ChangeNewEquip
{
    public ChangeNewEquip(int itemid, int itempro, float score, int level)
    {
        id = itemid;
        propertyid = itempro;
        fightscore = score;
        needlevel = level;
    }
    public int id = 0;
    public int propertyid = 0;
    public float fightscore = 0f;
    public int needlevel = 0;
}
