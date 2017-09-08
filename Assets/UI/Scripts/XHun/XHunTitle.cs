using UnityEngine;
using System.Collections;
using ArkCrossEngine;

public class XHunTitle : UnityEngine.MonoBehaviour
{

    public UILabel lblDiamond = null;
    public UILabel lblMoneyCoin = null;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        try
        {
            RoleInfo info = LobbyClient.Instance.CurrentRole;
            if (null != info)
            {
                if (lblDiamond != null)
                    lblDiamond.text = info.Gold.ToString();
                if (lblMoneyCoin != null)
                    lblMoneyCoin.text = info.Money.ToString();
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void OnHideButtonClick()
    {
        UIManager.Instance.HideWindowByName("XHun");
    }
    public void OnBuyCoinClick()
    {
        UIManager.Instance.ShowWindowByName("GoldBuy");
    }

    public void OnBuyDiamondClick()
    {

    }
    public void UpdateTitleInfo()
    {
        OnEnable();
    }
}
