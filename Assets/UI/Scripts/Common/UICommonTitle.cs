using ArkCrossEngine;

public class UICommonTitle : UnityEngine.MonoBehaviour
{
    public UISprite spPortrait = null;
    public UILabel lblLevel = null;
    public UILabel lblFighting = null;
    public UILabel lblDiamond = null;
    public UILabel lblMoneyCoin = null;
    public string WindowName = "";
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            OnEnable();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OnEnable()
    {
        RoleInfo info = LobbyClient.Instance.CurrentRole;
        if (null != info)
        {
            if (lblLevel != null) lblLevel.text = "Lv." + info.Level.ToString();
            if (lblFighting != null) lblFighting.text = ((int)info.FightingScore).ToString();
            if (lblDiamond != null) lblDiamond.text = info.Gold.ToString();
            if (lblMoneyCoin != null) lblMoneyCoin.text = info.Money.ToString();
            Data_PlayerConfig playerData = PlayerConfigProvider.Instance.GetPlayerConfigById(info.HeroId);
            if (playerData != null)
            {
                if (spPortrait != null) spPortrait.spriteName = playerData.m_Portrait;
            }
        }
    }
    public void OnHideButtonClick()
    {
        UIManager.Instance.HideWindowByName(WindowName);
    }
    public void OnBuyCoinClick()
    {
        UIManager.Instance.ShowWindowByName("GoldBuy");
    }
}
