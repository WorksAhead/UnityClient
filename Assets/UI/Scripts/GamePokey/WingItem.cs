using UnityEngine;
using System.Collections;
using ArkCrossEngine;

public class WingItem : UnityEngine.MonoBehaviour
{

    public UISprite spKuang = null;
    public UITexture spIcon = null;
    public UILabel lblName = null;
    public UILabel lblFingtScore = null;
    public UIButton btnEquip = null;
    public UIButton btnBuy = null;
    public UnityEngine.GameObject goHasEquip = null;

    private int id;
    private int propertyId;
    private int pos;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetItemInformation(int _id, int propertyid, int fightScore, bool hasOwn)
    {
        ItemConfig item_data = ItemConfigProvider.Instance.GetDataById(_id);
        if (item_data == null)
        {
            return;
        }
        id = _id;
        propertyId = propertyid;
        pos = 7;

        if (lblName != null)
        {
            lblName.text = item_data.m_ItemName;
            UnityEngine.Color col;
            switch (item_data.m_PropertyRank)
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
            lblName.color = col;
        }
        if (spIcon != null)
        {
            UnityEngine.Texture tex = CrossObjectHelper.TryCastObject<UnityEngine.Texture>(ResourceSystem.GetSharedResource(item_data.m_ItemTrueName));
            spIcon.mainTexture = tex;
        }
        if (spKuang != null)
        {
            spKuang.spriteName = "EquipFrame" + item_data.m_PropertyRank;
        }
        if (lblFingtScore != null)
        {
            lblFingtScore.text = fightScore.ToString();
        }
        if (btnBuy != null && btnEquip != null && goHasEquip != null)
        {
            bool hasEquip = HasEquipThis(id);
            NGUITools.SetActive(btnEquip.gameObject, !hasEquip);
            NGUITools.SetActive(goHasEquip, hasEquip);
        }
        if (btnBuy != null)
        {
            NGUITools.SetActive(btnBuy.gameObject, !hasOwn);
        }
    }

    private bool HasEquipThis(int id)
    {
        RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
        if (roleInfo != null)
        {
            ItemDataInfo[] equips = roleInfo.Equips;
            if (equips != null)
            {
                foreach (ItemDataInfo info in equips)
                {
                    if (info.ItemId == id)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void OnClickEquip()
    {
        if (id > 0 && !HasEquipThis(id))
            ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_mount_equipment", "lobby", id, propertyId, pos);
    }

    public void OnClickBuy()
    {

    }

    internal void UpdateTopView()
    {
        if (btnBuy != null && btnEquip != null && goHasEquip != null)
        {
            bool hasEquip = HasEquipThis(id);
            NGUITools.SetActive(btnEquip.gameObject, !hasEquip);
            NGUITools.SetActive(goHasEquip, hasEquip);
        }

    }
}
