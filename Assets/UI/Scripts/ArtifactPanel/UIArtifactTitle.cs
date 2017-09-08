using ArkCrossEngine;

public class UIArtifactTitle : UnityEngine.MonoBehaviour
{
    public UILabel lblDiamond = null;
    public UILabel lblMoneyCoin = null;
    public UILabel lblPlayerFight = null;
    public UILabel lblPlayerLevel = null;
    public UISprite spPortrait = null;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
                if (lblPlayerFight != null)
                    lblPlayerFight.text = info.FightingScore.ToString();
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OnEnable()
    {
        try
        {
            RoleInfo info = LobbyClient.Instance.CurrentRole;
            if (null != info)
            {
                if (lblDiamond != null) lblDiamond.text = info.Gold.ToString();
                if (lblMoneyCoin != null) lblMoneyCoin.text = info.Money.ToString();
                if (lblPlayerFight != null) lblPlayerFight.text = info.FightingScore.ToString();
                if (lblPlayerLevel != null) lblPlayerLevel.text = info.Level.ToString();
                UserInfo user_info = info.GetPlayerSelfInfo();
                if (user_info != null)
                {
                    Data_PlayerConfig playerData = PlayerConfigProvider.Instance.GetPlayerConfigById(info.HeroId);
                    if (playerData != null)
                    {
                        if (spPortrait != null) spPortrait.spriteName = playerData.m_Portrait;
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void OnHideButtonClick()
    {
        UIManager.Instance.HideWindowByName("ArtifactPanel");
    }
    public void OnBuyCoinClick()
    {
        UIManager.Instance.ShowWindowByName("GoldBuy");
    }
    public void OnBuyDiamondClick()
    {
        //todo 钻石购买
    }
    public void UpdateTitleInfo()
    {
        OnEnable();
    }
}
