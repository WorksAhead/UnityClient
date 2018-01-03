public enum ItemIconType : int
{
    Task_Award = 1, //任务奖励
    Sign_in = 2, // 签到
    Store_item = 3,//商店
    Xhun_item = 4,//战魂
    Artifact = 5,//神器界面消耗品
    Scene_Star = 6,//场景星级发放item
    Scene_Award = 7,//场景随机经验和钱发放item
    Scene_First = 8,//首次通关发放item
    Scene_Award2 = 9,//场景随进物品发放item
    CombatWin = 10,//翻拍item
    Victory = 11,//胜利界面
    Equip_slot = 12,//装备格子
    FightInfo_slot = 13,//名人赛查看他人装备的格子
    Equip_List = 14,//装备界面的装备列表list
    Item_Property = 15,//物品介绍界面
    Login_Award = 16,//七天登陆奖励
    Item_Source = 17,//物品来源tip
    Partner_Strengthen = 18,//伙伴中属性强化
    Partner_Skill = 19,//伙伴技能训练
}

public class DFMItemIconUtils : UnityEngine.MonoBehaviour
{
    public int m_Money = 101; // 金币
    public int m_Diamond = 102; // 钻石
    public int m_Exp = 103; // 经验
    public int m_ResurrectionStone = 104; // 复活石
    public int m_SweptVolume = 105; // 扫荡卷
    public int m_Card = 106; // 月卡
                             // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    static private DFMItemIconUtils m_Instance = new DFMItemIconUtils();
    static public DFMItemIconUtils Instance
    {
        get
        {
            return m_Instance;
        }
    }

    // 物品item赋值 type哪里显示的物品， item为要显示资源框， itemid物品id， itemcount要显示的物品数量，-1默认不显示数量
    public void SetItemInfo(ItemIconType type, UnityEngine.GameObject item, int itemId, int itemcount = -1)
    {
        if (item == null || itemId == 0)
        {
            return;
        }
        int nameType = 0; // 0:普通 1：s装备 ， 2：碎片
        UITexture icon = null;
        UISprite frame = null;
        UILabel num = null;
        UILabel lName = null;
        switch (type)
        {
            case ItemIconType.Task_Award:
                TaskAwardItem(item, out icon, out frame, out num);
                break;
            case ItemIconType.Sign_in:
                SignInItem(item, out icon, out frame, out num);
                nameType = 1;
                break;
            case ItemIconType.Store_item:
                StoreItem(item, out icon, out frame, out num);
                nameType = 1;
                break;
            case ItemIconType.Xhun_item:
                XhunItem(item, out icon, out frame, out num);
                break;
            case ItemIconType.Artifact:
                ArtifactItem(item, out icon, out frame, out num);
                break;
            case ItemIconType.Scene_Star:
                SceneStrarItem(item, out icon, out frame);
                break;
            case ItemIconType.Scene_Award:
                SceneAwardItem(item, out icon, out frame, out num);
                break;
            case ItemIconType.Scene_Award2:
                SceneAwardItem2(item, out icon, out frame, out num);
                break;
            case ItemIconType.Scene_First:
                SceneFirstItem(item, out icon, out frame);
                break;
            case ItemIconType.CombatWin:
                CombatWinItem(item, out icon, out frame, out num, out lName);
                break;
            case ItemIconType.Victory:
                VictorPanel(item, out icon, out frame, out lName, out num);
                break;
            case ItemIconType.Equip_slot:
                EquipSlot(item, out icon, out frame);
                nameType = 1;
                break;
            case ItemIconType.FightInfo_slot:
                nameType = 1;
                FightInfoSlot(item, out icon, out frame);
                break;
            case ItemIconType.Equip_List:
                nameType = 1;
                EquipListItem(item, out icon, out frame, out lName);
                break;
            case ItemIconType.Item_Property:
                ItemPropertySlot(item, out icon, out frame, out lName);
                break;
            case ItemIconType.Login_Award:
                LoginAwardSlot(item, out icon, out frame, out num);
                break;
            case ItemIconType.Item_Source:
                ItemSource(item, out icon, out frame, out lName);
                break;
            case ItemIconType.Partner_Strengthen:
                nameType = 1;
                PartnerStrengthen(item, out icon, out frame);
                break;
            case ItemIconType.Partner_Skill:
                nameType = 1;
                PartnerSkill(item, out icon, out frame, out lName);
                break;
        }
        ArkCrossEngine.ItemConfig ic = ArkCrossEngine.ItemConfigProvider.Instance.GetDataById(itemId);
        if (ic != null)
        {
            UnityEngine.Texture utt = GamePokeyManager.GetTextureByPicName(ic.m_ItemTrueName);
            if (utt != null && icon != null)
            {
                icon.mainTexture = utt;
            }
            if (frame != null)
            {
                if (ic.m_ShowType == 0)
                {
                    if (nameType == 0)
                    {
                        frame.spriteName = "EquipFrame" + ic.m_PropertyRank;
                    }
                    else if (nameType == 1)
                    {
                        frame.spriteName = "SEquipFrame" + ic.m_PropertyRank;
                    }
                }
                else if (ic.m_ShowType == 1)
                {
                    frame.spriteName = "SFrame" + ic.m_PropertyRank;
                }
            }
            if (num != null)
            {
                if (itemcount > -1)
                {
                    if (type == ItemIconType.Store_item)
                    {
                        num.text = "" + (itemcount > 1 ? itemcount.ToString() : "");
                    }
                    else
                    {
                        num.text = "X" + itemcount;
                    }
                }
                else
                {
                    num.text = "";
                }
            }
            if (lName != null)
            {
                lName.text = ic.m_ItemName;
                UnityEngine.Color col = new UnityEngine.Color();
                switch (ic.m_PropertyRank)
                {
                    case 1:
                        col = new UnityEngine.Color(1.0f, 1.0f, 1.0f);
                        break;
                    case 2:
                        col = new UnityEngine.Color(0x00 / 255f, 0xfb / 255f, 0x4a / 255f);
                        break;
                    case 3:
                        col = new UnityEngine.Color(0x41 / 255f, 0xc0 / 255f, 0xff / 255f);
                        break;
                    case 4:
                        col = new UnityEngine.Color(0xff / 255f, 0x00 / 255f, 0xff / 255f);
                        break;
                    case 5:
                        col = new UnityEngine.Color(0xff / 255f, 0xa3 / 255f, 0x00 / 255f);
                        break;
                    default:
                        col = new UnityEngine.Color(1.0f, 1.0f, 1.0f);
                        break;
                }
                lName.color = col;
            }
        }
    }
    //任务奖励item
    void TaskAwardItem(UnityEngine.GameObject item, out UITexture icon, out UISprite frame, out UILabel num)
    {
        UITexture ut = item.GetComponent<UITexture>();
        icon = ut;

        UnityEngine.Transform tf = item.transform.Find("Frame");
        UISprite us = tf.gameObject.GetComponent<UISprite>();
        frame = us;

        tf = item.transform.Find("Label");
        UILabel ul = tf.gameObject.GetComponent<UILabel>();
        num = ul;
    }
    // 签到item
    void SignInItem(UnityEngine.GameObject item, out UITexture icon, out UISprite frame, out UILabel num)
    {
        UnityEngine.Transform tf = item.transform.Find("IconKuang/Icon");
        UITexture ut = tf.gameObject.GetComponent<UITexture>();
        icon = ut;

        tf = item.transform.Find("IconKuang");
        UISprite us = tf.gameObject.GetComponent<UISprite>();
        frame = us;

        tf = item.transform.Find("namber");
        UILabel ul = tf.gameObject.GetComponent<UILabel>();
        num = ul;
    }
    // 商店item
    void StoreItem(UnityEngine.GameObject item, out UITexture icon, out UISprite frame, out UILabel num)
    {
        UnityEngine.Transform tf = item.transform.Find("kuang/Texture");
        UITexture ut = tf.gameObject.GetComponent<UITexture>();
        icon = ut;

        tf = item.transform.Find("kuang");
        UISprite us = tf.gameObject.GetComponent<UISprite>();
        frame = us;

        tf = item.transform.Find("kuang/Limit");
        UILabel ul = tf.gameObject.GetComponent<UILabel>();
        num = ul;
    }
    // 战魂item
    void XhunItem(UnityEngine.GameObject item, out UITexture icon, out UISprite frame, out UILabel num)
    {
        UnityEngine.Transform tf = item.transform.Find("Sprite/Texture");
        UITexture ut = tf.gameObject.GetComponent<UITexture>();
        icon = ut;

        tf = item.transform.Find("Sprite");
        UISprite us = tf.gameObject.GetComponent<UISprite>();
        frame = us;

        tf = item.transform.Find("LabelNum");
        UILabel ul = tf.gameObject.GetComponent<UILabel>();
        num = ul;
    }
    // 神器消耗的物品item
    void ArtifactItem(UnityEngine.GameObject item, out UITexture icon, out UISprite frame, out UILabel num)
    {
        UnityEngine.Transform tf = item.transform.Find("xiaohao/Texture");
        UITexture ut = tf.gameObject.GetComponent<UITexture>();
        icon = ut;

        tf = item.transform.Find("xiaohao");
        UISprite us = tf.gameObject.GetComponent<UISprite>();
        frame = us;

        tf = item.transform.Find("xiaohao/lv_iconNum");
        UILabel ul = tf.gameObject.GetComponent<UILabel>();
        num = ul;
    }
    // 场景介绍 星级发放item
    void SceneStrarItem(UnityEngine.GameObject item, out UITexture icon, out UISprite frame)
    {
        UnityEngine.Transform tf = item.transform.Find("item");
        UITexture ut = tf.gameObject.GetComponent<UITexture>();
        icon = ut;

        UISprite us = item.gameObject.GetComponent<UISprite>();
        frame = us;
    }
    // 场景介绍 随机奖励item
    void SceneAwardItem(UnityEngine.GameObject item, out UITexture icon, out UISprite frame, out UILabel num)
    {
        UnityEngine.Transform tf = item.transform.Find("Sprite");
        UITexture ut = tf.gameObject.GetComponent<UITexture>();
        icon = ut;

        UISprite us = item.gameObject.GetComponent<UISprite>();
        frame = us;

        tf = item.transform.Find("Label");
        UILabel ul = tf.gameObject.GetComponent<UILabel>();
        num = ul;
    }
    // 场景介绍 随机奖励物品item
    void SceneAwardItem2(UnityEngine.GameObject item, out UITexture icon, out UISprite frame, out UILabel num)
    {

        UITexture ut = item.gameObject.GetComponent<UITexture>();
        icon = ut;

        UnityEngine.Transform tf = item.transform.Find("kuang");
        UISprite us = tf.gameObject.GetComponent<UISprite>();
        frame = us;

        tf = item.transform.Find("Label");
        UILabel ul = tf.gameObject.GetComponent<UILabel>();
        num = ul;
    }
    // 场景介绍 首次通关奖励item
    void SceneFirstItem(UnityEngine.GameObject item, out UITexture icon, out UISprite frame)
    {
        UnityEngine.Transform tf = item.transform.Find("kuang/Texture");
        UITexture ut = tf.gameObject.GetComponent<UITexture>();
        icon = ut;

        tf = item.transform.Find("kuang");
        UISprite us = tf.gameObject.GetComponent<UISprite>();
        frame = us;

    }
    // 场景介绍 首次通关奖励item
    void CombatWinItem(UnityEngine.GameObject item, out UITexture icon, out UISprite frame, out UILabel num, out UILabel lName)
    {
        UnityEngine.Transform tf = item.transform.Find("Texture");
        UITexture ut = tf.gameObject.GetComponent<UITexture>();
        NGUITools.SetActive(tf.gameObject, true);
        icon = ut;

        tf = item.transform.Find("Sprite");
        NGUITools.SetActive(tf.gameObject, true);
        UISprite us = tf.gameObject.GetComponent<UISprite>();
        frame = us;

        tf = item.transform.Find("Label");
        NGUITools.SetActive(tf.gameObject, true);
        UILabel ul = tf.gameObject.GetComponent<UILabel>();
        num = ul;

        tf = item.transform.Find("name");
        NGUITools.SetActive(tf.gameObject, true);
        UILabel uiName = tf.gameObject.GetComponent<UILabel>();
        lName = uiName;
    }
    // 胜利界面item
    void VictorPanel(UnityEngine.GameObject item, out UITexture icon, out UISprite frame, out UILabel lName, out UILabel num)
    {
        NGUITools.SetActive(item, true);
        UITexture ut = item.gameObject.GetComponent<UITexture>();
        icon = ut;

        UnityEngine.Transform tf = item.transform.Find("kuang");
        UISprite us = tf.gameObject.GetComponent<UISprite>();
        frame = us;

        tf = item.transform.Find("name");
        UILabel ul = tf.gameObject.GetComponent<UILabel>();
        lName = ul;

        tf = item.transform.Find("lblnum");
        UILabel ulnum = tf.gameObject.GetComponent<UILabel>();
        num = ulnum;
    }
    // 装备格子item
    void EquipSlot(UnityEngine.GameObject item, out UITexture icon, out UISprite frame)
    {
        UnityEngine.Transform tf = item.transform.Find("icon");
        UITexture ut = tf.gameObject.GetComponent<UITexture>();
        icon = ut;

        tf = item.transform.Find("Sprite");
        UISprite us = tf.gameObject.GetComponent<UISprite>();
        frame = us;

    }
    // 名人战中的装备
    void FightInfoSlot(UnityEngine.GameObject item, out UITexture icon, out UISprite frame)
    {
        UnityEngine.Transform tf = item.transform.Find("Texture");
        UITexture ut = tf.gameObject.GetComponent<UITexture>();
        icon = ut;

        UISprite us = item.gameObject.GetComponent<UISprite>();
        frame = us;
    }
    // 装备内的装备列表item
    void EquipListItem(UnityEngine.GameObject item, out UITexture icon, out UISprite frame, out UILabel lName)
    {
        UnityEngine.Transform tf = item.transform.Find("Icon");
        UITexture ut = tf.gameObject.GetComponent<UITexture>();
        icon = ut;

        tf = item.transform.Find("IconKuang");
        UISprite us = tf.gameObject.GetComponent<UISprite>();
        frame = us;

        tf = item.transform.Find("LabelName");
        UILabel ul = tf.gameObject.GetComponent<UILabel>();
        lName = ul;
    }
    // 装备介绍与对比界面的格子
    void ItemPropertySlot(UnityEngine.GameObject item, out UITexture icon, out UISprite frame, out UILabel lName)
    {
        UnityEngine.Transform tf = item.transform.Find("Texture");
        UITexture ut = tf.gameObject.GetComponent<UITexture>();
        icon = ut;

        tf = item.transform.Find("TextureFrame");
        UISprite us = tf.gameObject.GetComponent<UISprite>();
        frame = us;

        tf = item.transform.Find("LabelName");
        UILabel ul = tf.gameObject.GetComponent<UILabel>();
        lName = ul;
    }
    // 装备介绍与对比界面的格子
    void LoginAwardSlot(UnityEngine.GameObject item, out UITexture icon, out UISprite frame, out UILabel num)
    {
        UnityEngine.Transform tf = item.transform.Find("Item/IconKuang/Icon");
        UITexture ut = tf.gameObject.GetComponent<UITexture>();
        icon = ut;

        tf = item.transform.Find("Item/IconKuang");
        UISprite us = tf.gameObject.GetComponent<UISprite>();
        frame = us;

        tf = item.transform.Find("Item/item_num");
        UILabel ul = tf.gameObject.GetComponent<UILabel>();
        num = ul;
    }

    //装备来源
    private void ItemSource(UnityEngine.GameObject item, out UITexture icon, out UISprite frame, out UILabel lName)
    {
        UnityEngine.Transform tf = item.transform.Find("Item/icon/Texture");
        UITexture ut = tf.gameObject.GetComponent<UITexture>();
        icon = ut;

        tf = item.transform.Find("Item/icon");
        UISprite us = tf.gameObject.GetComponent<UISprite>();
        frame = us;

        tf = item.transform.Find("Item/lblName");
        UILabel ul = tf.gameObject.GetComponent<UILabel>();
        lName = ul;
    }
    //伙伴属性强化
    private void PartnerStrengthen(UnityEngine.GameObject item, out UITexture icon, out UISprite frame)
    {
        UnityEngine.Transform tf = item.transform.Find("CostItem/ItemSprite/item-icon");
        UITexture ut = tf.gameObject.GetComponent<UITexture>();
        icon = ut;

        tf = item.transform.Find("CostItem/ItemSprite");
        UISprite us = tf.gameObject.GetComponent<UISprite>();
        frame = us;

        //tf = item.transform.Find("CostItem/03ItemName");
        //UILabel ul = tf.gameObject.GetComponent<UILabel>();
        //lName = ul;
    }
    //伙伴技能训练
    private void PartnerSkill(UnityEngine.GameObject item, out UITexture icon, out UISprite frame, out UILabel lName)
    {
        UnityEngine.Transform tf = item.transform.Find("LiftMaterial/item/Texture");
        UITexture ut = tf.gameObject.GetComponent<UITexture>();
        icon = ut;

        tf = item.transform.Find("LiftMaterial/item");
        UISprite us = tf.gameObject.GetComponent<UISprite>();
        frame = us;

        tf = item.transform.Find("LiftMaterial/item/name");
        UILabel ul = tf.gameObject.GetComponent<UILabel>();
        lName = ul;
    }
}
