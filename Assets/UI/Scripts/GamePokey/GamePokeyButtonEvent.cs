using UnityEngine;
using ArkCrossEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePokeyButtonEvent : UnityEngine.MonoBehaviour
{
    private List<UnityEngine.GameObject> containerList = new List<UnityEngine.GameObject>();
    public UnityEngine.GameObject roleInfoContainer = null;
    public UnityEngine.GameObject wingContainer = null;
    public UnityEngine.GameObject xhunContainer = null;
    public UnityEngine.GameObject pockeyContainer = null;
    public UnityEngine.GameObject fashionContainer = null;

    public UIToggle tgChip = null;

    private bool m_CanCompound = false;

    public bool CanCompound
    {
        set
        {
            m_CanCompound = value;
            UpdateToggleChipTip();
        }
        get
        {
            return m_CanCompound;
        }
    }

    // Use this for initialization
    void Start()
    {
        try
        {
            for (int i = 0; i < 9; ++i)
            {
                UnityEngine.Transform tf = transform.Find("HeroEquip/Equipment/Slot" + i);
                if (tf != null)
                {
                    UIEventListener.Get(tf.gameObject).onClick = SlotButtonClick;
                }
            }
            containerList.Add(roleInfoContainer);
            containerList.Add(wingContainer);
            containerList.Add(xhunContainer);
            containerList.Add(pockeyContainer);
            containerList.Add(fashionContainer);
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

    void OnEnable()
    {
        try
        {
            ShowContainer(pockeyContainer);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void SlotButtonClick(UnityEngine.GameObject go)
    {
        //     UnityEngine.Transform tf = transform.Find("RoleInfo");
        //     if (tf != null) {
        //       if (NGUITools.GetActive(tf.gameObject)) {
        //         return;
        //       }
        //     }

        if (go == null) return;
        int pos = 0;
        switch (go.transform.name)
        {
            case "Slot0":
                pos = 0;
                break;
            case "Slot1":
                pos = 1;
                break;
            case "Slot2":
                pos = 2;
                break;
            case "Slot3":
                pos = 3;
                break;
            case "Slot4":
                pos = 4;
                break;
            case "Slot5":
                pos = 5;
                break;
            case "Slot6"://时装
                pos = 6;
                //ShowContainer(fashionContainer);
                return;
            case "Slot7"://翅膀
                pos = 7;
                break;
            //ShowContainer(wingContainer);
            //return;
            case "Slot8"://Xhun
                pos = 8;
                break;
            //RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
            //if (roleInfo != null) {
            //  LevelLock config = LevelLockProvider.Instance.GetDataById(9);
            //  if (config != null) {
            //    if (config.m_Level <= roleInfo.Level) {//等级开放
            //      ShowContainer(xhunContainer);            
            //    }
            //  }
            //}
            //return;
            default:
                return;
        }
        EquipmentInfo ei = GamePokeyManager.GetEquipmentInfo(pos);
        if (ei != null && ei.id != 0)
        {
            UnityEngine.GameObject ipgo = UIManager.Instance.GetWindowGoByName("ItemProperty");
            if (ipgo != null && !NGUITools.GetActive(ipgo))
            {
                ItemProperty ip = ipgo.GetComponent<ItemProperty>();
                ItemConfig config = ItemConfigProvider.Instance.GetDataById(ei.id);
                ip.SetItemProperty(ei.id, pos, ei.level, ei.propertyid, false, !config.m_CanUpgrade);
                UIManager.Instance.HideWindowByName("EntrancePanel");
                UIManager.Instance.ShowWindowByName("ItemProperty");
            }
        }
    }

    private void ShowContainer(UnityEngine.GameObject container)
    {
        if (container != null)
        {
            NGUITools.SetActive(container, true);
        }
        for (int i = 0; i < containerList.Count; i++)
        {
            if (containerList[i] != null && containerList[i] != container)
            {
                NGUITools.SetActive(containerList[i], false);
            }
        }
    }

    public void ArrangeButton()
    {
        //Debug.Log("ArrangeButton");

        GamePokeyManager gpm = transform.gameObject.GetComponent<GamePokeyManager>();
        if (gpm != null)
        {
            gpm.ArrangedItem();
        }
    }

    public void CloseXhunView()
    {
        UnityEngine.Transform tf = gameObject.transform.Find("Xhunview");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, false);
        }
    }

    public void BulksaleButton()
    {
        if (UIManager.CheckItemForDelete == null) return;
        List<int> li = new List<int>();
        li.Clear();
        List<int> propertys = new List<int>();
        propertys.Clear();
        int count = UIManager.CheckItemForDelete.Count;
        for (int i = 0; i < count; ++i)
        {
            UnityEngine.GameObject go = UIManager.CheckItemForDelete[i];
            if (go != null)
            {
                ItemClick ic = go.GetComponent<ItemClick>();
                if (ic != null)
                {
                    li.Add(ic.ID);
                    propertys.Add(ic.PropertyId);
                }
            }
        }
        int[] sell = li.ToArray();
        int[] property = propertys.ToArray();
        GfxSystem.EventChannelForLogic.Publish("ge_discard_item", "lobby", sell, property);
    }
    public void ReturnButton()
    {
        UIManager.Instance.HideWindowByName("ItemProperty");
        UIManager.Instance.HideWindowByName("GamePokey");
        UIManager.Instance.ShowWindowByName("EntrancePanel");
    }
    public void DetailProperty()
    {
        if (roleInfoContainer != null)
        {
            if (NGUITools.GetActive(roleInfoContainer))
            {
                ShowContainer(pockeyContainer);
            }
            else
            {
                ShowContainer(roleInfoContainer);
                ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_request_role_property", "ui");
            }
        }
        //UnityEngine.Transform tf = transform.Find("RoleInfo");
        //UnityEngine.Transform tf1 = transform.Find("PokeyContainer");
        //if (tf != null) {
        //  if (NGUITools.GetActive(tf.gameObject)) {
        //    NGUITools.SetActive(tf.gameObject, false);
        //    if (tf1 != null) {
        //      NGUITools.SetActive(tf1.gameObject, true);
        //    }
        //  } else {
        //    NGUITools.SetActive(tf.gameObject, true);
        //    ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_request_role_property", "ui");
        //    if (tf1 != null) {
        //      NGUITools.SetActive(tf1.gameObject, false);
        //    }
        //  }
        //}
    }
    public void OnCilckClose()
    {
        ShowContainer(pockeyContainer);
    }
    public void BuyGold()
    {
        UIManager.Instance.ShowWindowByName("GoldBuy");
    }

    public void OnToggleChipChange()
    {
        UpdateToggleChipTip();
    }

    private void UpdateToggleChipTip()
    {
        if (tgChip != null)
        {
            UnityEngine.Transform tfTip = tgChip.transform.Find("Tip");
            if (tfTip != null)
            {
                NGUITools.SetActive(tfTip.gameObject, tgChip.value == false && CanCompound ? true : false);
            }
        }
    }
}
