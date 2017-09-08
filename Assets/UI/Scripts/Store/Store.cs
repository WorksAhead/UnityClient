using System;
using System.Collections.Generic;
using ArkCrossEngine;

public class Store : UnityEngine.MonoBehaviour
{
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
            if (eventlist != null) { eventlist.Clear(); }
            object eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (eo != null) eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, bool, ArkCrossEngine.Network.GeneralOperationResult, int, int>("ge_exchange_goods", "store", ManageExchangeGoods);
            if (eo != null) eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, List<int>, List<int>>("ge_sync_exchanges", "store", SyncExchangeData);
            if (eo != null) eventlist.Add(eo);
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, int>("ge_refresh_exchange_num", "store",
            (int currency, int num) => { if (currencyId == currency) refreshNum = num; if (currency == 0) refreshNum = 0; });
            if (eo != null) eventlist.Add(eo);

            UnityEngine.Transform tf = transform.Find("sp_heikuang/ScrollView/Grid");
            if (tf != null)
            {
                gridGo = tf.gameObject;
            }
            tf = transform.Find("suipian/Label");
            if (tf != null)
            {
                UILabel ul = tf.gameObject.GetComponent<UILabel>();
                if (ul != null)
                {
                    currencyLabel = ul;
                    RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
                    if (ri != null)
                    {
                        currencyLabel.text = ri.ExchangeCurrency.ToString();
                    }
                }
            }
            //ManageExchangeGoods(true, ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed, 0, 0);
            //ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_sync_exchanges", "lobby",200001);
            UIManager.Instance.HideWindowByName("Store");
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
            RoleInfo ri = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
            if (ri != null)
            {
                int nowcurrency = 0;
                if (currencyId == ItemConfigProvider.Instance.GetGoldId())
                {
                    nowcurrency = ri.Money;
                }
                else if (currencyId == ItemConfigProvider.Instance.GetDiamondId())
                {
                    nowcurrency = ri.Gold;
                }
                else
                {
                    nowcurrency = ri.ExchangeCurrency;
                }
                if (nowcurrency != lastcurrency)
                {
                    lastcurrency = nowcurrency;
                    foreach (storeInfo si in storeDic.Values)
                    {
                        ArkCrossEngine.StoreConfig sc = ArkCrossEngine.StoreConfigProvider.Instance.GetDataById(si.ID);
                        if (sc != null)
                        {
                            SetGameObjectInfo(si.go, sc, si.CanBuyTime);
                        }
                    }
                    if (currencyLabel != null)
                    {
                        currencyLabel.text = lastcurrency.ToString();
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void SyncExchangeData(int currency, List<int> ids, List<int> nums)
    {
        if (currency != currencyId) return;
        if (ids != null && nums != null && ids.Count == nums.Count)
        {
            int count = ids.Count;
            for (int i = 0; i < count; ++i)
            {
                AddItem(ids[i], nums[i]);
            }
        }
    }
    void ManageExchangeGoods(int currency, bool refresh, ArkCrossEngine.Network.GeneralOperationResult result, int exchangeid, int exchangenum)
    {
        try
        {
            GfxSystem.PublishGfxEvent("ge_ui_connect_hint", "ui", false, false);
            if (currency != currencyId && currency != 0) return;
            if (refresh)
            {
                if (result == ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed)
                {
                    DataDictionaryMgr<StoreConfig> storedata = ArkCrossEngine.StoreConfigProvider.Instance.StoreDictionaryMgr;
                    if (storedata != null)
                    {
                        MyDictionary<int, object> storedic = storedata.GetData();
                        if (storedic != null)
                        {
                            StoreConfig sc = null;
                            foreach (KeyValuePair<int, object> pair in storedic)
                            {
                                sc = pair.Value as StoreConfig;
                                if (sc != null)
                                {
                                    if (sc.m_Currency == currencyId || (currency == 0 && sc.m_Currency == currencyId))
                                    {
                                        AddItem(pair.Key, 0);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (result == ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_CostError)
                {
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(123),
                    ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(140), null, null, null, false);
                }
            }
            else
            {
                if (result == ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed)
                {
                    AddItem(exchangeid, exchangenum);
                    ArkCrossEngine.StoreConfig sc = ArkCrossEngine.StoreConfigProvider.Instance.GetDataById(exchangeid);
                    if (sc != null)
                    {
                        ArkCrossEngine.ItemConfig ic = ArkCrossEngine.ItemConfigProvider.Instance.GetDataById(sc.m_ItemId);
                        if (ic != null)
                        {
                            GfxSystem.PublishGfxEvent("ge_screen_tip", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(1005)
                                                      + ic.m_ItemName + "X" + sc.m_ItemNum, UIScreenTipPosEnum.AlignCenter, UnityEngine.Vector3.zero);
                        }
                    }
                }
                else
                {
                    //提示
                    int sign = 1003;
                    switch (result)
                    {
                        case ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_CostError:
                            sign = 1000;
                            break;
                        case ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_Overflow:
                            sign = 1001;
                            break;
                        case ArkCrossEngine.Network.GeneralOperationResult.LC_Failure_LevelError:
                            sign = 1002;
                            break;
                        default:
                            sign = 1003;
                            break;
                    }
                    GfxSystem.PublishGfxEvent("ge_screen_tip", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(sign), UIScreenTipPosEnum.AlignCenter, UnityEngine.Vector3.zero);
                }
            }
            UIGrid ug = gridGo.GetComponent<UIGrid>();
            if (ug != null)
            {
                ug.repositionNow = true;
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void AddItem(int storeid, int buytimes)
    {
        StoreConfig sc = ArkCrossEngine.StoreConfigProvider.Instance.GetDataById(storeid);
        if (sc != null)
        {
            if (storeDic.ContainsKey(storeid))
            {
                storeInfo si = storeDic[storeid];
                if (si != null)
                {
                    SetGameObjectInfo(si.go, sc, sc.m_HaveDayLimit ? sc.m_DayLimit - buytimes : 1);
                    si.ChangeTimes(sc.m_HaveDayLimit ? sc.m_DayLimit - buytimes : 1);
                }
            }
            else
            {
                UnityEngine.GameObject go = GetAGameObject();
                NGUITools.SetActive(go, true);
                if (go != null)
                {
                    go.transform.name = string.Format("{0:D5}", storeid);
                    UIEventListener.Get(go).onClick = StoreItemClick;
                    SetGameObjectInfo(go, sc, sc.m_HaveDayLimit ? sc.m_DayLimit - buytimes : 1);
                    storeDic.Add(storeid, new storeInfo(sc.m_HaveDayLimit ? sc.m_DayLimit - buytimes : 1, storeid, go));
                }
            }
        }
    }
    UnityEngine.GameObject GetAGameObject()
    {
        if (gridGo != null)
        {
            int count = gridGo.transform.childCount;
            for (int i = 0; i < count; ++i)
            {
                UnityEngine.Transform tf = gridGo.transform.GetChild(i);
                if (tf != null)
                {
                    if (!CheckAlreadyUsed(tf.gameObject))
                    {
                        return tf.gameObject;
                    }
                }
            }
            UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/Store/Item"));
            go = NGUITools.AddChild(gridGo, go);
            if (go != null)
            {
                return go;
            }
        }
        return null;
    }
    bool CheckAlreadyUsed(UnityEngine.GameObject go)
    {
        foreach (storeInfo si in storeDic.Values)
        {
            if (go == si.go)
            {
                return true;
            }
        }
        return false;
    }
    void UnVisibleUnUsed()
    {
        if (gridGo != null)
        {
            int count = gridGo.transform.childCount;
            for (int i = 0; i < count; ++i)
            {
                UnityEngine.Transform tf = gridGo.transform.GetChild(i);
                if (tf != null)
                {
                    bool sign = false;
                    foreach (storeInfo si in storeDic.Values)
                    {
                        if (si.go == tf.gameObject)
                        {
                            sign = true;
                            break;
                        }
                    }
                    if (sign == false)
                    {
                        NGUITools.SetActive(tf.gameObject, false);
                    }
                }
            }
        }
    }
    void SetGameObjectInfo(UnityEngine.GameObject goc, StoreConfig sc, int remaintimes)
    {
        if (goc != null && sc != null)
        {
            ItemConfig ic = ArkCrossEngine.ItemConfigProvider.Instance.GetDataById(sc.m_ItemId);
            if (ic != null)
            {
                UnityEngine.Transform tf = goc.transform.Find("name");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        ul.text = ic.m_ItemName;
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
                        ul.color = col;
                    }
                }
                //if (sc.m_HaveDayLimit) {
                DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Store_item, goc, sc.m_ItemId, sc.m_ItemNum);
                //         } else {
                //           DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Store_item, goc, sc.m_ItemId);
                //         }

                //        tf = goc.transform.Find("kuang");
                //        if (tf != null) {
                //          UISprite us = tf.gameObject.GetComponent<UISprite>();
                //          if (us != null) {
                //            us.spriteName = "SEquipFrame" + ic.m_PropertyRank;
                //          }
                //        }
                //        tf = tf.Find("UnityEngine.Texture");
                //        if (tf != null) {
                //          UITexture ut = tf.gameObject.GetComponent<UITexture>();
                //          if (ut != null) {
                //            UnityEngine.Texture tt = GamePokeyManager.GetTextureByPicName(ic.m_ItemTrueName);
                //            if (tt != null) {
                //              ut.mainTexture = tt;
                //            }
                //          }
                //        }
                ////         tf = goc.transform.Find("kuang/number");
                ////         if (tf != null) {
                ////           UILabel ul = tf.gameObject.GetComponent<UILabel>();
                ////           if (ul != null) {
                ////             ul.text = sc.m_ItemNum.ToString();
                ////           }
                ////         }
                //        tf = goc.transform.Find("kuang/Limit");
                //        if (tf != null) {
                //          UILabel ul = tf.gameObject.GetComponent<UILabel>();
                //          if (ul != null) {
                //if (sc.m_HaveDayLimit) {
                //  ul.text = "" + remaintimes/* + "/" + sc.m_DayLimit*/;
                //} else {
                //  ul.text = "" /*+ ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(1004)*/;
                //}
                //          }
                //        }
                tf = goc.transform.Find("Price/Label");
                if (tf != null)
                {
                    UILabel ul = tf.gameObject.GetComponent<UILabel>();
                    if (ul != null)
                    {
                        int price = 0;
                        if (sc.m_HaveDayLimit)
                        {
                            if (sc.m_Price.Count > (sc.m_DayLimit - remaintimes))
                            {
                                price = sc.m_Price[sc.m_DayLimit - remaintimes];
                            }
                            if (sc.m_Price.Count == (sc.m_DayLimit - remaintimes))
                            {
                                price = sc.m_Price[sc.m_Price.Count - 1];
                            }
                        }
                        else
                        {
                            price = sc.m_Price[0];
                        }
                        ul.text = price.ToString();
                        if (lastcurrency < price)
                        {
                            ul.color = UnityEngine.Color.red;
                        }
                        else
                        {
                            ul.color = new UnityEngine.Color(131 / 255f, 67 / 255f, 0f);
                        }
                    }
                }
                tf = goc.transform.Find("SellOut");
                if (tf != null)
                {
                    if (sc.m_HaveDayLimit && remaintimes <= 0)
                    {
                        NGUITools.SetActive(tf.gameObject, true);
                    }
                    else
                    {
                        NGUITools.SetActive(tf.gameObject, false);
                    }
                }
                tf = goc.transform.Find("Matte");
                if (tf != null)
                {
                    if (sc.m_HaveDayLimit && remaintimes <= 0)
                    {
                        NGUITools.SetActive(tf.gameObject, true);
                    }
                    else
                    {
                        NGUITools.SetActive(tf.gameObject, false);
                    }
                }
                tf = goc.transform.Find("Price/Currency");
                if (tf != null)
                {
                    UISprite us = tf.gameObject.GetComponent<UISprite>();
                    if (us != null)
                    {
                        us.spriteName = currencySprite;
                    }
                }
            }
        }
    }
    private void StoreItemClick(UnityEngine.GameObject goc)
    {
        foreach (storeInfo si in storeDic.Values)
        {
            if (si != null)
            {
                if (si.go == goc)
                {
                    if (si.CanBuyTime == 0)
                    {
                        //已卖完
                    }
                    else
                    {
                        //需要兑换
                        ArkCrossEngine.StoreConfig sc = ArkCrossEngine.StoreConfigProvider.Instance.GetDataById(si.ID);
                        if (sc != null)
                        {
                            ArkCrossEngine.ItemConfig itemconfig = ArkCrossEngine.ItemConfigProvider.Instance.GetDataById(sc.m_ItemId);
                            if (itemconfig != null)
                            {
                                if (itemconfig.m_CanWear)
                                {
                                    EquipmentInfo ei = GamePokeyManager.GetEquipmentInfo(itemconfig.m_WearParts);
                                    if (ei != null)
                                    {
                                        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("ItemProperty");
                                        if (go != null && !NGUITools.GetActive(go))
                                        {
                                            ItemProperty ip = go.GetComponent<ItemProperty>();
                                            if (ip != null)
                                            {
                                                ip.ExchangeGoodsCompare(currencySprite, si.ID, ei.id, ei.level, ei.propertyid, sc.m_ItemId, ei.level, ei.propertyid, itemconfig.m_WearParts, si.GetCurrency());
                                                UIManager.Instance.ShowWindowByName("ItemProperty");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("ItemProperty");
                                    if (go != null && !NGUITools.GetActive(go))
                                    {
                                        ItemProperty ip = go.GetComponent<ItemProperty>();
                                        if (ip != null)
                                        {
                                            ip.ExchangeGoodsSetItemProperty(currencySprite, si.ID, sc.m_ItemId, 0, 0, 0, si.GetCurrency());
                                            UIManager.Instance.ShowWindowByName("ItemProperty");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public void CloseStore()
    {
        UIManager.Instance.HideWindowByName("Store");
    }
    public void RequestRefresh()
    {
        ArkCrossEngine.MyAction<int> fun = Yes;
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_show_dialog", "ui", ArkCrossEngine.StrDictionaryProvider.Instance.Format(1006, (refreshNum + 1) * 50), null,
        ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(140), ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(157), fun, false);
    }
    private void Yes(int id)
    {
        if (id == 1)
        {
            ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_refresh_exchanges", "lobby", currencyId);
            GfxSystem.PublishGfxEvent("ge_ui_connect_hint", "ui", false, true);
        }
    }
    public void ChangeStore(int currencyid, string spritename)
    {
        if (currencyid != currencyId)
        {
            GfxSystem.PublishGfxEvent("ge_ui_connect_hint", "ui", false, true);
            UnityEngine.Transform tf = transform.Find("suipian/Seacher");
            if (tf != null)
            {
                UIButton ub = tf.gameObject.GetComponent<UIButton>();
                if (ub != null)
                {
                    ub.normalSprite = spritename;
                }
            }
            currencyId = currencyid;
            currencySprite = spritename;
            storeDic.Clear();
            ManageExchangeGoods(currencyid, true, ArkCrossEngine.Network.GeneralOperationResult.LC_Succeed, 0, 0);
            UnVisibleUnUsed();
            ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_sync_exchanges", "lobby", currencyid);
        }
        ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_sync_exchanges", "lobby", 0);
    }
    public void CurrencyButton()
    {
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_item_come_from", "ui", currencyId);
    }
    private int currencyId = 0;
    private string currencySprite = null;
    private int refreshNum = 0;
    private UILabel currencyLabel = null;
    private int lastcurrency = 0;
    private UnityEngine.GameObject gridGo = null;
    private Dictionary<int, storeInfo> storeDic = new Dictionary<int, storeInfo>();
}
class storeInfo
{
    public int CanBuyTime;
    public UnityEngine.GameObject go;
    public int ID;
    public storeInfo(int time, int id, UnityEngine.GameObject goc)
    {
        CanBuyTime = time;
        go = goc;
        ID = id;
    }
    public int GetCurrency()
    {
        ArkCrossEngine.StoreConfig sc = ArkCrossEngine.StoreConfigProvider.Instance.GetDataById(ID);
        if (sc != null)
        {
            if (sc.m_HaveDayLimit)
            {
                if (sc.m_Price.Count > sc.m_DayLimit - CanBuyTime)
                {
                    return sc.m_Price[sc.m_DayLimit - CanBuyTime];
                }
            }
            else
            {
                return sc.m_Price[0];
            }
        }
        return 0;
    }
    public void ChangeTimes(int time)
    {
        if (time < 0)
        {
            CanBuyTime = 0;
        }
        else
        {
            CanBuyTime = time;
        }
    }
}