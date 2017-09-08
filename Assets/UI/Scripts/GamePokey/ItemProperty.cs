using UnityEngine;
using System.Collections;
using ArkCrossEngine;

public class ItemProperty : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject upgradeEffect = null;
    public UnityEngine.GameObject upEffGO = null;
    public UnityEngine.GameObject effect = null;
    public UISprite spLevelUp = null;
    private float duration = 1.0f;
    // Use this for initialization
    void Start()
    {
        try
        {
            CalculateUIPosition(transform.Find("SpriteBackRight"));
            CalculateUIPosition(transform.Find("SpriteBackLeft"));
            UnityEngine.Transform tf = gameObject.transform.Find("SpriteBackLeft");
            if (tf != null)
            {
                leftLocalPos = tf.localPosition;
            }
            tf = transform.Find("SpriteBackRight");
            if (tf != null)
            {
                rightLocalPos = tf.localPosition;
            }
            transform.localPosition = new UnityEngine.Vector3(0f, -29f, 0f);
            UIManager.Instance.HideWindowByName("ItemProperty");
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

    public void ShowItemProperty(int itemId, int itemLevel)
    {
        SetItemProperty(itemId, 0, itemLevel, 0);
        ShowActionButton(false);
    }
    //用来显示与隐藏升级、装备按钮
    private void ShowActionButton(bool active)
    {
        UnityEngine.Transform ts = transform.Find("SpriteInlay");
        if (ts != null) NGUITools.SetActive(ts.gameObject, active);
        ts = transform.Find("SpriteUpdate");
        if (ts != null) NGUITools.SetActive(ts.gameObject, active);
        ts = transform.Find("SpriteBackRight/Sprite");
        if (ts != null) NGUITools.SetActive(ts.gameObject, active);
    }

    public void SetItemProperty(int itemid, int pos, int itemlevel, int propertyid, bool isUpgrade = false, bool hideBtnArea = false)
    {
        ShowActionButton(true);
        isCompareUI = false;
        ID = itemid;
        property = propertyid;
        position = pos;
        level = itemlevel;
        UnityEngine.Transform tfc = gameObject.transform.Find("SpriteBackLeft");
        if (tfc != null)
        {
            NGUITools.SetActive(tfc.gameObject, false);
        }
        tfc = transform.Find("SpriteBackRight");
        if (tfc != null)
        {
            tfc.localPosition = new UnityEngine.Vector3(0.0f, 0.0f, 0.0f);
            tfc = tfc.Find("line/Label");
            if (tfc != null)
            {
                UILabel ul = tfc.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(152);
                    ul.color = new UnityEngine.Color(1.0f, 0.52549f, 0.18039f);
                }
            }
        }
        tfc = transform.Find("SpriteBuy");
        if (tfc != null)
        {
            NGUITools.SetActive(tfc.gameObject, false);
        }
        tfc = transform.Find("SpriteSale");
        if (tfc != null)
        {
            NGUITools.SetActive(tfc.gameObject, false);
        }
        tfc = transform.Find("SpriteInlay");
        if (tfc != null)
        {
            //       UILabel ul = tfc.gameObject.GetComponent<UILabel>();
            //       if (ul != null) {
            //         ul.text = "镶嵌";
            //       }
            NGUITools.SetActive(tfc.gameObject, false);
        }
        tfc = transform.Find("SpriteUpdate/Label");
        if (tfc != null)
        {
            UILabel ul = tfc.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(121);
            }
        }
        tfc = transform.Find("SpriteUpdate/Up/money");
        if (tfc != null)
        {
            UILabel ul = tfc.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                ArkCrossEngine.ItemLevelupConfig iluc = ArkCrossEngine.ItemLevelupConfigProvider.Instance.GetDataById(level);
                if (iluc != null)
                {
                    ul.text = (iluc.m_PartsList.Count > position ? iluc.m_PartsList[position] : 0).ToString();
                }
                else
                {
                    ul.text = "0";
                }
            }
        }
        tfc = transform.Find("SpriteUpdate");
        if (tfc != null)
        {
            UIButton ub = tfc.GetComponent<UIButton>();
            if (/*us != null &&*/ ub != null)
            {
                ArkCrossEngine.RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
                if (itemlevel >= ri.Level)
                {
                    ub.isEnabled = false;
                    if (spLevelUp != null) spLevelUp.color = UnityEngine.Color.grey;
                }
                else
                {
                    ub.isEnabled = true;
                    if (spLevelUp != null) spLevelUp.color = UnityEngine.Color.white;
                }
            }
        }
        ArkCrossEngine.ItemConfig ic = ArkCrossEngine.ItemConfigProvider.Instance.GetDataById(itemid);
        tfc = transform.Find("SpriteUpdate");
        if (tfc != null && ic != null)
        {
            if (!ic.m_CanUpgrade)
            {
                NGUITools.SetActive(tfc.gameObject, false);
                //hideBtnArea = true;
            }
            else
            {
                NGUITools.SetActive(tfc.gameObject, !hideBtnArea);
            }
        }
        tfc = transform.Find("SpriteBackRight/Sprite");
        if (tfc != null)
        {
            NGUITools.SetActive(tfc.gameObject, !hideBtnArea);
        }
        SetItemHeadProperty(itemid, itemlevel, propertyid, transform.Find("SpriteBackRight"), isUpgrade);
        CompareProperty(0, 0, 0, 0, 0, 0);
        CalculateUIPosition(transform.Find("SpriteBackRight"));
    }

    public void InLay()
    {
        if (isCompareUI)
        {
            CloseItemProterty();
            RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
            if (roleInfo != null)
            {
                ItemConfig itemConfig = ItemConfigProvider.Instance.GetDataById(ID);
                if (itemConfig != null)
                {
                    if (roleInfo.Level >= itemConfig.m_WearLevel)
                    {
                        ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_mount_equipment", "lobby", ID, property, position);
                    }
                    else
                    {
                        string tip = StrDictionaryProvider.Instance.GetDictString(46);
                        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", tip, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
                    }
                }
            }
        }
    }

    public void ItemUpdate()
    {
        ArkCrossEngine.ItemLevelupConfig iluc = ArkCrossEngine.ItemLevelupConfigProvider.Instance.GetDataById(level);
        ArkCrossEngine.RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
        if (iluc != null && ri != null)
        {
            int costmoney = iluc.m_PartsList.Count > position ? iluc.m_PartsList[position] : 0;
            if (costmoney > ri.Money)
            {
                ArkCrossEngine.MyAction<int> fun = Buttonwhich;
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(122),
                ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(140), null, null, null/*fun*/, false);
            }
            else
            {
                ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_upgrade_item", "lobby", position, ID, false);
            }
        }
    }
    void Buttonwhich(int buttonid)
    {
        if (buttonid == 2)
        {
            ArkCrossEngine.ItemLevelupConfig iluc = ArkCrossEngine.ItemLevelupConfigProvider.Instance.GetDataById(level);
            ArkCrossEngine.RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
            if (iluc != null && ri != null)
            {
                int costmoney = iluc.m_PartsList.Count > position ? iluc.m_PartsList[position] : 0;
                int needmoney = costmoney - (int)(ri.Money);
                if (needmoney > 0)
                {
                    float needgold = needmoney * (iluc.m_Rate == float.Epsilon ? 1 : iluc.m_Rate);
                    if (needgold > ri.Gold)
                    {
                        ArkCrossEngine.MyAction<int> fun = Buttonwhichone;
                        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(123), "YES", null, null, fun, false);
                    }
                    else
                    {
                        ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_upgrade_item", "lobby", position, ID, true);
                    }
                }
            }
        }
    }
    void Buttonwhichone(int id)
    {
    }

    public void CloseItemProterty()
    {
        UnityEngine.Transform tf = transform.Find("SpriteUpdate");
        if (tf != null)
        {
            UIButton ub = tf.GetComponent<UIButton>();
            if (ub != null)
            {
                ub.isEnabled = true;
            }
        }
        tf = transform.Find("SpriteBackRight/Sprite");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, true);
        }
        UIManager.Instance.HideWindowByName("ItemProperty");
        //UIManager.Instance.ShowWindowByName("GamePokey");
    }

    public void SetGamePokey(UnityEngine.GameObject go)
    {
        gamepokey = go;
    }

    private void CalculateUIPosition(UnityEngine.Transform whichtf)
    {
        if (whichtf == null) return;
        UnityEngine.Transform tfd = whichtf.Find("Container/DragThing");
        if (tfd != null)
        {
            UnityEngine.Vector3 v3 = tfd.localPosition;
            tfd.localPosition = new UnityEngine.Vector3(v3.x, 0.0f, v3.z);
        }

        UnityEngine.Transform tf = whichtf.Find("Container/DragThing");
        UnityEngine.Transform tfc = tf;
        UnityEngine.Vector3 pos = new UnityEngine.Vector3();
        if (tf != null)
        {
            tf = tfc.Find("Property");
            if (tf != null)
            {
                pos = tf.localPosition;
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    pos = new UnityEngine.Vector3(pos.x, pos.y - ul.localSize.y + 20, 0.0f);
                }
            }
            tf = tfc.Find("StarRock");
            if (tf != null)
            {
                tf.localPosition = pos;
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    pos = new UnityEngine.Vector3(pos.x, pos.y - ul.localSize.y - 0, 0.0f);
                }
            }
            tf = tfc.Find("Bula");
            if (tf != null)
            {
                tf.localPosition = pos;
                //计算是否可拖动
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                }
            }
        }
    }

    private void PlayParticle(UnityEngine.Vector3 nguiPos)
    {
        if (effect != null)
        {
            UnityEngine.GameObject ef = ResourceSystem.NewObject(effect) as GameObject;
            if (ef != null)
            {
                nguiPos.Set(-0.17f, nguiPos.y, nguiPos.z);
                ef.transform.position = nguiPos;
                Destroy(ef, duration);
            }
        }
    }

    private void SetLabelProperty(ArkCrossEngine.ItemConfig itemconfig, int itemlevel, int propertyid, UnityEngine.Transform whichtf, bool isUpgrade = false)
    {
        if (itemconfig == null || whichtf == null) return;
        ArkCrossEngine.AppendAttributeConfig aac = ArkCrossEngine.AppendAttributeConfigProvider.Instance.GetDataById(propertyid);
        int level = 1;
        string str = "[ffffff]";
        float data = 0.0f;
        data = itemconfig.m_AttrData.GetAddHpMax(1.0f, level, itemlevel);
        if (UnityEngine.Mathf.Abs(data - 0) > float.Epsilon)
        {
            str += (ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(101) + UIManager.GetItemProtetyStr(data, itemconfig.m_AttrData.m_HpMaxType) + "\n");
        }
        data = itemconfig.m_AttrData.GetAddAd(1.0f, level, itemlevel);
        if (UnityEngine.Mathf.Abs(data - 0) > float.Epsilon)
        {
            str += (ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(102) + UIManager.GetItemProtetyStr(data, itemconfig.m_AttrData.m_AdType) + "\n");
        }
        data = itemconfig.m_AttrData.GetAddADp(1.0f, level, itemlevel);
        if (UnityEngine.Mathf.Abs(data - 0) > float.Epsilon)
        {
            str += (ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(103) + UIManager.GetItemProtetyStr(data, itemconfig.m_AttrData.m_ADpType) + "\n");
        }
        data = itemconfig.m_AttrData.GetAddMDp(1.0f, level, itemlevel);
        if (UnityEngine.Mathf.Abs(data - 0) > float.Epsilon)
        {
            str += (ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(104) + UIManager.GetItemProtetyStr(data, itemconfig.m_AttrData.m_MDpType) + "\n");
        }

        int proNum = str.Split('\n').Length - 1;

        switch (itemconfig.m_DamageType)
        {
            case ArkCrossEngine.ElementDamageType.DC_Fire:
                str += ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(118);
                break;
            case ArkCrossEngine.ElementDamageType.DC_Ice:
                str += ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(119);
                break;
            case ArkCrossEngine.ElementDamageType.DC_Poison:
                str += ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(120);
                break;
            case ArkCrossEngine.ElementDamageType.DC_None:
                break;
            default: break;
        }
        str += "[-]";
        UnityEngine.Transform tf = whichtf.Find("Container/DragThing/Property");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                //播放升级特效
                if (isUpgrade == true)
                {
                    for (int i = 0; i < proNum; i++)
                    {
                        PlayParticle(ul.transform.position - new UnityEngine.Vector3(0f, 0.1f * i + 0.05f, 0f));
                    }
                    //播放图标特效
                    if (upgradeEffect != null)
                    {
                        UnityEngine.GameObject ef = ResourceSystem.NewObject(upgradeEffect) as GameObject;
                        if (ef != null && upEffGO != null)
                        {
                            ef.transform.position = new UnityEngine.Vector3(upEffGO.transform.position.x, upEffGO.transform.position.y, upEffGO.transform.position.z);
                            Destroy(ef, duration);
                        }
                    }
                }
                ul.text = str;
            }
        }
        str = "";
        if (aac != null)
        {
            data = aac.GetAddCri(1.0f, level);
            if (UnityEngine.Mathf.Abs(data - 0) > float.Epsilon)
            {
                str += (ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(105) + "+" + UnityEngine.Mathf.FloorToInt(data * 100) + "%\n");
            }
            data = aac.GetAddPow(1.0f, level);
            if (UnityEngine.Mathf.Abs(data - 0) > float.Epsilon)
            {
                str += (ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(106) + "+" + UnityEngine.Mathf.FloorToInt(data * 100) + "%\n");
            }
            data = aac.GetAddBackHitPow(1.0f, level);
            if (UnityEngine.Mathf.Abs(data - 0) > float.Epsilon)
            {
                str += (ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(107) + "+" + UnityEngine.Mathf.FloorToInt(data * 100) + "%\n");
            }
            data = aac.GetAddCrackPow(1.0f, level);
            if (UnityEngine.Mathf.Abs(data - 0) > float.Epsilon)
            {
                str += (ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(108) + "+" + UnityEngine.Mathf.FloorToInt(data * 100) + "%\n");
            }
        }
        data = itemconfig.m_AttrData.GetAddFireDam(1.0f, level);
        if (UnityEngine.Mathf.Abs(data - 0) > float.Epsilon)
        {
            str += (ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(109) + UIManager.GetItemProtetyStr(data, itemconfig.m_AttrData.m_FireDamType) + "\n");
        }
        data = itemconfig.m_AttrData.GetAddIceDam(1.0f, level);
        if (UnityEngine.Mathf.Abs(data - 0) > float.Epsilon)
        {
            str += (ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(110) + UIManager.GetItemProtetyStr(data, itemconfig.m_AttrData.m_IceDamType) + "\n");
        }
        data = itemconfig.m_AttrData.GetAddPoisonDam(1.0f, level);
        if (UnityEngine.Mathf.Abs(data - 0) > float.Epsilon)
        {
            str += (ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(111) + UIManager.GetItemProtetyStr(data, itemconfig.m_AttrData.m_PoisonDamType) + "\n");
        }
        data = itemconfig.m_AttrData.GetAddFireErd(1.0f, level);
        if (UnityEngine.Mathf.Abs(data - 0) > float.Epsilon)
        {
            str += (ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(112) + UIManager.GetItemProtetyStr(data, itemconfig.m_AttrData.m_FireErdType) + "\n");
        }
        data = itemconfig.m_AttrData.GetAddIceErd(1.0f, level);
        if (UnityEngine.Mathf.Abs(data - 0) > float.Epsilon)
        {
            str += (ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(113) + UIManager.GetItemProtetyStr(data, itemconfig.m_AttrData.m_IceErdType) + "\n");
        }
        data = itemconfig.m_AttrData.GetAddPoisonErd(1.0f, level);
        if (UnityEngine.Mathf.Abs(data - 0) > float.Epsilon)
        {
            str += (ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(114) + UIManager.GetItemProtetyStr(data, itemconfig.m_AttrData.m_PoisonErdType) + "\n");
        }
        if (str != "")
        {
            str = "\n[00ff00]" + str + "[-]";
        }
        tf = whichtf.Find("Container/DragThing/StarRock");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                ul.text = str;
            }
        }
        tf = whichtf.Find("Container/DragThing/Bula");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                ul.text = itemconfig.m_Description;
            }
        }
    }
    public void Compare(int leftitem, int leftlevel, int leftpropertyid, int rightitem, int rightlevel, int rightpropertyid, int pos)
    {
        UnityEngine.Transform tf = null;
        isCompareUI = true;
        ID = rightitem;
        property = rightpropertyid;
        position = pos;
        if (leftitem == 0)
        {
            tf = gameObject.transform.Find("SpriteBackLeft");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, false);
            }
            tf = transform.Find("SpriteBackRight");
            if (tf != null)
            {
                tf.localPosition = new UnityEngine.Vector3(0.0f, 0.0f, 0.0f);
                tf = tf.Find("line/Label");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(153);
                        ul.color = UnityEngine.Color.white;
                    }
                }
            }
        }
        else
        {
            tf = gameObject.transform.Find("SpriteBackLeft");
            if (tf != null)
            {
                tf.localPosition = leftLocalPos;
                NGUITools.SetActive(tf.gameObject, true);
            }
            tf = transform.Find("SpriteBackRight");
            if (tf != null)
            {
                tf.localPosition = rightLocalPos;
                tf = tf.Find("line/Label");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(152);
                        ul.color = UnityEngine.Color.white;
                    }
                }
            }
        }
        tf = transform.Find("SpriteInlay/Label");
        if (tf != null)
        {
            UILabel ul = tf.gameObject.GetComponent<UILabel>();
            if (ul != null)
            {
                ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(116);
            }
            NGUITools.SetActive(tf.parent.gameObject, true);
        }
        //     tf = transform.Find("SpriteUpdate/Label");
        //     if (tf != null) {
        //       UILabel ul = tf.gameObject.GetComponent<UILabel>();
        //       if (ul != null) {
        //         ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(117);
        //       }
        //     }
        //     tf = transform.Find("SpriteUpdate/xiaohao");
        //     if (tf != null) {
        //       UILabel ul = tf.gameObject.GetComponent<UILabel>();
        //       if (ul != null) {
        //         ul.text = "";//ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(154);
        //       }
        //     }
        tf = transform.Find("SpriteUpdate");///Sprite
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, false);
        }
        tf = transform.Find("SpriteBuy");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, false);
        }
        tf = transform.Find("SpriteSale");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, true);
        }

        ArkCrossEngine.ItemConfig itemconfig = ArkCrossEngine.LogicSystem.GetItemDataById(rightitem);
        if (itemconfig != null)
        {
            tf = transform.Find("SpriteSale/sale/money");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = itemconfig.m_SellingPrice.ToString();
                }
            }
            tf = transform.Find("SpriteBackRight");
            DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Item_Property, tf.gameObject, rightitem);

            //tf = transform.Find("SpriteBackRight/LabelLv");
            //if (tf != null) {
            //  UILabel ul = tf.gameObject.GetComponent<UILabel>();
            //  if (ul != null) {
            //    ul.text = "Lv." + rightlevel;
            //  }
            //}

            //       tf = transform.Find("SpriteUpdate");
            //       if (tf != null) {
            //         NGUITools.SetActive(tf.gameObject, true);
            //         UIButton ub = tf.gameObject.GetComponent<UIButton>();
            //         if (ub != null) {
            //           ub.normalSprite = "db_cs";
            //         }
            //       }
            SetItemHeadProperty(leftitem, leftlevel, leftpropertyid, transform.Find("SpriteBackLeft"));
            SetItemHeadProperty(rightitem, rightlevel, rightpropertyid, transform.Find("SpriteBackRight"));
        }
        CompareProperty(leftitem, leftlevel, leftpropertyid, rightitem, rightlevel, rightpropertyid);
        CalculateUIPosition(transform.Find("SpriteBackLeft"));
        CalculateUIPosition(transform.Find("SpriteBackRight"));
    }
    private void SetItemHeadProperty(int itemid, int itemlevel, int propertyid, UnityEngine.Transform whichtf, bool isUpgrade = false)
    {
        ArkCrossEngine.ItemConfig itemconfig = ArkCrossEngine.LogicSystem.GetItemDataById(itemid);
        if (itemconfig != null)
        {
            DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Item_Property, whichtf.gameObject, itemid);
            UnityEngine.Transform tf = whichtf.Find("LabelLv");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = "Lv." + itemlevel;
                }
            }
            tf = whichtf.Find("InlayLv");
            if (tf != null)
            {
                UILabel lbl = tf.gameObject.GetComponent<UILabel>();
                if (lbl != null)
                {
                    lbl.text = StrDictionaryProvider.Instance.Format(45, itemconfig.m_WearLevel);
                    RoleInfo role = LobbyClient.Instance.CurrentRole;
                    if (role != null)
                    {
                        if (role.Level >= itemconfig.m_WearLevel)
                        {//可穿戴
                            lbl.color = new UnityEngine.Color(0f, 251 / 255f, 75 / 255f);
                        }
                        else
                        {//不可穿戴
                            lbl.color = UnityEngine.Color.red;
                        }
                    }
                }
            }
            SetLabelProperty(itemconfig, itemlevel, propertyid, whichtf, isUpgrade);
        }
    }
    private void CompareProperty(int leftitem, int leftitemlevel, int leftpropertyid, int rightitem, int rightitemlevel, int rightpropertyid)
    {
        if (isCompareUI)
        {
            int level = 1;
            float dataL = 0.0f;
            float dataR = 0.0f;
            string str = "";
            ArkCrossEngine.ItemConfig itemconfigL = ArkCrossEngine.LogicSystem.GetItemDataById(leftitem);
            ArkCrossEngine.ItemConfig itemconfigR = ArkCrossEngine.LogicSystem.GetItemDataById(rightitem);
            if (itemconfigL != null && itemconfigR != null)
            {
                ArkCrossEngine.AppendAttributeConfig aacL = ArkCrossEngine.AppendAttributeConfigProvider.Instance.GetDataById(leftpropertyid);
                ArkCrossEngine.AppendAttributeConfig aacR = ArkCrossEngine.AppendAttributeConfigProvider.Instance.GetDataById(rightpropertyid);
                dataL = UIManager.GetItemPropertyData(itemconfigL.m_AttrData.GetAddHpMax(1.0f, level, leftitemlevel), itemconfigR.m_AttrData.m_HpMaxType);
                dataR = UIManager.GetItemPropertyData(itemconfigR.m_AttrData.GetAddHpMax(1.0f, level, leftitemlevel), itemconfigR.m_AttrData.m_HpMaxType);
                if (UnityEngine.Mathf.Abs(dataR - dataL) > float.Epsilon)
                {
                    str += (((dataR - dataL) > 0.0f ? "[00ffea]" : "[ff0000]") + ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(101) + UIManager.GetItemProtetyStr((dataR - dataL), itemconfigR.m_AttrData.m_HpMaxType) + "[-]\n");
                }
                dataL = UIManager.GetItemPropertyData(itemconfigL.m_AttrData.GetAddAd(1.0f, level, leftitemlevel), itemconfigR.m_AttrData.m_AdType);
                dataR = UIManager.GetItemPropertyData(itemconfigR.m_AttrData.GetAddAd(1.0f, level, leftitemlevel), itemconfigR.m_AttrData.m_AdType);
                if (UnityEngine.Mathf.Abs(dataR - dataL) > float.Epsilon)
                {
                    str += (((dataR - dataL) > 0.0f ? "[00ffea]" : "[ff0000]") + ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(102) + UIManager.GetItemProtetyStr((dataR - dataL), itemconfigR.m_AttrData.m_AdType) + "[-]\n");
                }
                dataL = UIManager.GetItemPropertyData(itemconfigL.m_AttrData.GetAddADp(1.0f, level, leftitemlevel), itemconfigR.m_AttrData.m_ADpType);
                dataR = UIManager.GetItemPropertyData(itemconfigR.m_AttrData.GetAddADp(1.0f, level, leftitemlevel), itemconfigR.m_AttrData.m_ADpType);
                if (UnityEngine.Mathf.Abs(dataR - dataL) > float.Epsilon)
                {
                    str += (((dataR - dataL) > 0.0f ? "[00ffea]" : "[ff0000]") + ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(103) + UIManager.GetItemProtetyStr((dataR - dataL), itemconfigR.m_AttrData.m_ADpType) + "[-]\n");
                }
                dataL = UIManager.GetItemPropertyData(itemconfigL.m_AttrData.GetAddMDp(1.0f, level, leftitemlevel), itemconfigR.m_AttrData.m_MDpType);
                dataR = UIManager.GetItemPropertyData(itemconfigR.m_AttrData.GetAddMDp(1.0f, level, leftitemlevel), itemconfigR.m_AttrData.m_MDpType);
                if (UnityEngine.Mathf.Abs(dataR - dataL) > float.Epsilon)
                {
                    str += (((dataR - dataL) > 0.0f ? "[00ffea]" : "[ff0000]") + ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(104) + UIManager.GetItemProtetyStr((dataR - dataL), itemconfigR.m_AttrData.m_MDpType) + "[-]\n");
                }
                dataL = (aacL == null ? 0.0f : UnityEngine.Mathf.FloorToInt(aacL.GetAddCri(1.0f, level) * 100) /*/ 100.0f*/);
                dataR = (aacR == null ? 0.0f : UnityEngine.Mathf.FloorToInt(aacR.GetAddCri(1.0f, level) * 100) /*/ 100.0f*/);
                if (UnityEngine.Mathf.Abs(dataR - dataL) > float.Epsilon)
                {
                    str += (((dataR - dataL) > 0.0f ? "[00ffea]" : "[ff0000]") + ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(105) + ((dataR - dataL) > 0.0f ? "+" : "") + UnityEngine.Mathf.FloorToInt((dataR - dataL) /** 100*/) + "%[-]\n");
                }
                dataL = (aacL == null ? 0.0f : UnityEngine.Mathf.FloorToInt(aacL.GetAddPow(1.0f, level) * 100) /*/ 100.0f*/);
                dataR = (aacR == null ? 0.0f : UnityEngine.Mathf.FloorToInt(aacR.GetAddPow(1.0f, level) * 100) /*/ 100.0f*/);
                if (UnityEngine.Mathf.Abs(dataR - dataL) > float.Epsilon)
                {
                    str += (((dataR - dataL) > 0.0f ? "[00ffea]" : "[ff0000]") + ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(106) + ((dataR - dataL) > 0.0f ? "+" : "") + UnityEngine.Mathf.FloorToInt((dataR - dataL) /** 100*/) + "%[-]\n");
                }
                dataL = (aacL == null ? 0.0f : UnityEngine.Mathf.FloorToInt(aacL.GetAddBackHitPow(1.0f, level) * 100) /*/ 100.0f*/);
                dataR = (aacR == null ? 0.0f : UnityEngine.Mathf.FloorToInt(aacR.GetAddBackHitPow(1.0f, level) * 100) /*/ 100.0f*/);
                if (UnityEngine.Mathf.Abs(dataR - dataL) > float.Epsilon)
                {
                    str += (((dataR - dataL) > 0.0f ? "[00ffea]" : "[ff0000]") + ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(107) + ((dataR - dataL) > 0.0f ? "+" : "") + UnityEngine.Mathf.FloorToInt((dataR - dataL) /** 100*/) + "%[-]\n");
                }
                dataL = (aacL == null ? 0.0f : UnityEngine.Mathf.FloorToInt(aacL.GetAddCrackPow(1.0f, level) * 100) /*/ 100.0f*/);
                dataR = (aacR == null ? 0.0f : UnityEngine.Mathf.FloorToInt(aacR.GetAddCrackPow(1.0f, level) * 100) /*/ 100.0f*/);
                if (UnityEngine.Mathf.Abs(dataR - dataL) > float.Epsilon)
                {
                    str += (((dataR - dataL) > 0.0f ? "[00ffea]" : "[ff0000]") + ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(108) + ((dataR - dataL) > 0.0f ? "+" : "") + UnityEngine.Mathf.FloorToInt((dataR - dataL) /** 100*/) + "%[-]\n");
                }
                dataL = UIManager.GetItemPropertyData(itemconfigL.m_AttrData.GetAddFireDam(1.0f, level), itemconfigR.m_AttrData.m_FireDamType);
                dataR = UIManager.GetItemPropertyData(itemconfigR.m_AttrData.GetAddFireDam(1.0f, level), itemconfigR.m_AttrData.m_FireDamType);
                if (UnityEngine.Mathf.Abs(dataR - dataL) > float.Epsilon)
                {
                    str += (((dataR - dataL) > 0.0f ? "[00ffea]" : "[ff0000]") + ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(109) + UIManager.GetItemProtetyStr((dataR - dataL), itemconfigR.m_AttrData.m_FireDamType) + "[-]\n");
                }
                dataL = UIManager.GetItemPropertyData(itemconfigL.m_AttrData.GetAddIceDam(1.0f, level), itemconfigR.m_AttrData.m_IceDamType);
                dataR = UIManager.GetItemPropertyData(itemconfigR.m_AttrData.GetAddIceDam(1.0f, level), itemconfigR.m_AttrData.m_IceDamType);
                if (UnityEngine.Mathf.Abs(dataR - dataL) > float.Epsilon)
                {
                    str += (((dataR - dataL) > 0.0f ? "[00ffea]" : "[ff0000]") + ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(110) + UIManager.GetItemProtetyStr((dataR - dataL), itemconfigR.m_AttrData.m_IceDamType) + "[-]\n");
                }
                dataL = UIManager.GetItemPropertyData(itemconfigL.m_AttrData.GetAddPoisonDam(1.0f, level), itemconfigR.m_AttrData.m_PoisonDamType);
                dataR = UIManager.GetItemPropertyData(itemconfigR.m_AttrData.GetAddPoisonDam(1.0f, level), itemconfigR.m_AttrData.m_PoisonDamType);
                if (UnityEngine.Mathf.Abs(dataR - dataL) > float.Epsilon)
                {
                    str += (((dataR - dataL) > 0.0f ? "[00ffea]" : "[ff0000]") + ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(111) + UIManager.GetItemProtetyStr((dataR - dataL), itemconfigR.m_AttrData.m_PoisonDamType) + "[-]\n");
                }
                dataL = UIManager.GetItemPropertyData(itemconfigL.m_AttrData.GetAddFireErd(1.0f, level), itemconfigR.m_AttrData.m_FireErdType);
                dataR = UIManager.GetItemPropertyData(itemconfigR.m_AttrData.GetAddFireErd(1.0f, level), itemconfigR.m_AttrData.m_FireErdType);
                if (UnityEngine.Mathf.Abs(dataR - dataL) > float.Epsilon)
                {
                    str += (((dataR - dataL) > 0.0f ? "[00ffea]" : "[ff0000]") + ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(112) + UIManager.GetItemProtetyStr((dataR - dataL), itemconfigR.m_AttrData.m_FireErdType) + "[-]\n");
                }
                dataL = UIManager.GetItemPropertyData(itemconfigL.m_AttrData.GetAddIceErd(1.0f, level), itemconfigR.m_AttrData.m_IceErdType);
                dataR = UIManager.GetItemPropertyData(itemconfigR.m_AttrData.GetAddIceErd(1.0f, level), itemconfigR.m_AttrData.m_IceErdType);
                if (UnityEngine.Mathf.Abs(dataR - dataL) > float.Epsilon)
                {
                    str += (((dataR - dataL) > 0.0f ? "[00ffea]" : "[ff0000]") + ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(113) + UIManager.GetItemProtetyStr((dataR - dataL), itemconfigR.m_AttrData.m_IceErdType) + "[-]\n");
                }
                dataL = UIManager.GetItemPropertyData(itemconfigL.m_AttrData.GetAddPoisonErd(1.0f, level), itemconfigR.m_AttrData.m_PoisonErdType);
                dataR = UIManager.GetItemPropertyData(itemconfigL.m_AttrData.GetAddPoisonErd(1.0f, level), itemconfigR.m_AttrData.m_PoisonErdType);
                if (UnityEngine.Mathf.Abs(dataR - dataL) > float.Epsilon)
                {
                    str += (((dataR - dataL) > 0.0f ? "[00ffea]" : "[ff0000]") + ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(114) + UIManager.GetItemProtetyStr((dataR - dataL), itemconfigR.m_AttrData.m_PoisonErdType) + "[-]\n");
                }
            }
            if (str.Length != 0)
            {
                str = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(100) + "\n" + str;
            }
            str += itemconfigR.m_Description;
            UnityEngine.Transform tfr = transform.Find("SpriteBackRight/Container/DragThing/Bula");
            if (tfr != null)
            {
                UILabel ul = tfr.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = str;
                }
            }
        }
        //     else {
        //       UnityEngine.Transform tfr = transform.Find("SpriteBackRight/Container/DragThing/Bula");
        //       if (tfr != null) {
        //         UILabel ul = tfr.gameObject.GetComponent<UILabel>();
        //         if (ul != null) {
        //           ul.text = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(115);
        //         }
        //       }
        //     }
    }
    public void ExchangeGoodsCompare(string sprite, int exchangedid, int leftitem, int leftlevel, int leftpropertyid, int rightitem, int rightlevel, int rightpropertyid, int pos, int currency)
    {
        Compare(leftitem, leftlevel, leftpropertyid, rightitem, rightlevel, rightpropertyid, pos);
        signforExchange = true;
        exchangedId = exchangedid;
        exchangeCurrency = currency;
        SetExchangeGoods(sprite, currency, true, ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(161));
    }
    public void ExchangeGoodsSetItemProperty(string sprite, int exchangedid, int itemid, int pos, int itemlevel, int propertyid, int currency, bool isUpgrade = false)
    {
        SetItemProperty(itemid, pos, itemlevel, propertyid, isUpgrade);
        signforExchange = true;
        exchangedId = exchangedid;
        exchangeCurrency = currency;
        SetExchangeGoods(sprite, currency, true, ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(162), true);
    }
    private void SetExchangeGoods(string sprite, int currency, bool sign, string str, bool canSearch = false)
    {
        if (sign)
        {
            UnityEngine.Transform tf = transform.Find("SpriteBuy");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, true);
            }
            RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
            if (ri != null)
            {
                tf = transform.Find("SpriteBuy/money");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        ul.text = currency.ToString();
                        int nowcurrency = 0;
                        StoreConfig sc = ArkCrossEngine.StoreConfigProvider.Instance.GetDataById(exchangedId);
                        if (ri != null && sc != null)
                        {
                            if (sc.m_Currency == ItemConfigProvider.Instance.GetGoldId())
                            {
                                nowcurrency = ri.Money;
                            }
                            else if (sc.m_Currency == ItemConfigProvider.Instance.GetDiamondId())
                            {
                                nowcurrency = ri.Gold;
                            }
                            else
                            {
                                nowcurrency = ri.ExchangeCurrency;
                            }
                        }
                        if (currency > nowcurrency)
                        {
                            ul.color = UnityEngine.Color.red;
                        }
                        else
                        {
                            ul.color = UnityEngine.Color.white;
                        }
                    }
                }
            }
            tf = transform.Find("SpriteBuy/SpriteCurrency");
            if (tf != null)
            {
                UISprite us = tf.gameObject.GetComponent<UISprite>();
                if (us != null)
                {
                    us.spriteName = sprite;
                }
            }
            tf = transform.Find("SpriteInlay");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, false);
            }
            tf = transform.Find("SpriteUpdate");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, false);
            }
            tf = transform.Find("SpriteSale");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, false);
            }
            tf = transform.Find("SpriteBackRight/line/Label");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    ul.text = str;
                    ul.color = UnityEngine.Color.white;
                }
            }
        }
        else
        {
            UnityEngine.Transform tf = transform.Find("SpriteInlay");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, true);
            }
        }
    }
    public void Sale()
    {
        int[] sell = new int[] { ID };
        int[] propertarray = new int[] { property };
        ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_discard_item", "lobby", sell, propertarray);
        CloseItemProterty();
        //       UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("TaskAward");
        //       if (go != null) {
        //         TaskAward ta = go.GetComponent<TaskAward>();
        //         ArkCrossEngine.ItemConfig ic = ArkCrossEngine.ItemConfigProvider.Instance.GetDataById(ID);
        //         if (ta != null && ic != null) {
        //           ta.SetSellGain("X " + ic.m_SellingPrice, ic.m_SellGainGoldProb == float.Epsilon ? null : ("X " + ic.m_SellGainGoldProb * 100 + "%"));
        //         }
        //         UIManager.Instance.ShowWindowByName("TaskAward");
        //       }
    }
    public void Buy()
    {
        RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
        StoreConfig sc = ArkCrossEngine.StoreConfigProvider.Instance.GetDataById(exchangedId);
        if (ri != null && sc != null)
        {
            int nowcurrency = 0;
            if (sc.m_Currency == ItemConfigProvider.Instance.GetGoldId())
            {
                nowcurrency = ri.Money;
            }
            else if (sc.m_Currency == ItemConfigProvider.Instance.GetDiamondId())
            {
                nowcurrency = ri.Gold;
            }
            else
            {
                nowcurrency = ri.ExchangeCurrency;
            }
            if (nowcurrency < exchangeCurrency)
            {
                GfxSystem.PublishGfxEvent("ge_screen_tip", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(163), UIScreenTipPosEnum.AlignCenter, UnityEngine.Vector3.zero);
            }
            else
            {
                GfxSystem.PublishGfxEvent("ge_ui_connect_hint", "ui", false, true);
                ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_exchange_goods", "lobby", exchangedId, false);
            }
            UIManager.Instance.HideWindowByName("ItemProperty");
            signforExchange = false;
        }
    }
    public void CheckOperation()
    {
        int i = 0;
        if (!isCompareUI && !signforExchange)
        {
            RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
            if (ri != null)
            {
                if (ri.Level == level)
                {
                    i = 170;
                }
            }
        }
        else
        {
            i = 171;
        }
        if (i != 0)
        {
            GfxSystem.PublishGfxEvent("ge_screen_tip", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(i), UIScreenTipPosEnum.AlignCenter, UnityEngine.Vector3.zero);
        }
    }
    private int exchangedId = 0;
    private int exchangeCurrency = 0;
    private bool isCompareUI = false;
    private bool signforExchange = false;
    private int ID = 0;
    private int property = 0;
    private int position = 0;
    private int level = 0;
    private UnityEngine.GameObject gamepokey = null;
    private UnityEngine.Vector3 leftLocalPos = new UnityEngine.Vector3();
    private UnityEngine.Vector3 rightLocalPos = new UnityEngine.Vector3();

}
