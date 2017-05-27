using UnityEngine;
using System.Collections;
using ArkCrossEngine;

public class UITreasurePlayerInfo : UnityEngine.MonoBehaviour
{


    public UIProgressBar hpBar = null;
    public UIProgressBar mpBar = null;
    public UIProgressBar staminaBar = null;//体力
    public UISprite spPortrait = null;
    public UILabel lblLevel = null;
    public UILabel lblHp = null;
    public UILabel lblMp = null;
    public UILabel lblMoney = null;
    public UILabel lblDiamond = null;
    public UILabel lblStamina = null;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            if (role_info != null)
            {
                if (lblDiamond != null) lblDiamond.text = role_info.Gold.ToString();
                if (lblMoney != null) lblMoney.text = role_info.Money.ToString();
                if (lblStamina != null)
                {
                    lblStamina.text = role_info.CurStamina + "/" + 120;
                    if (staminaBar != null) staminaBar.value = role_info.CurStamina / (float)120;
                }
                ExpeditionPlayerInfo ep_info = role_info.GetExpeditionInfo();
                UserInfo user_info = role_info.GetPlayerSelfInfo();
                if (ep_info != null && user_info != null)
                {
                    if (hpBar != null)
                    {
                        hpBar.value = ep_info.Hp / 1000f;
                        int hpMax = user_info.GetActualProperty().HpMax;
                        int curhp = (int)((float)ep_info.Hp / 1000f * hpMax);
                        string hpstr = string.Format("{0}/{1}", curhp > hpMax ? hpMax : curhp, hpMax);
                        if (lblHp != null) lblHp.text = hpstr;
                    }
                    if (mpBar != null)
                    {
                        mpBar.value = ep_info.Mp / 1000f;
                    }

                    int mpMax = user_info.GetActualProperty().EnergyMax;
                    int curMp = (int)((float)ep_info.Mp / 1000f * mpMax);
                    string mpStr = string.Format("{0}/{1}", curMp > mpMax ? mpMax : curMp, mpMax);
                    if (lblMp != null) lblMp.text = mpStr;
                    if (lblLevel != null)
                    {
                        lblLevel.text = role_info.Level.ToString();
                    }
                    if (spPortrait != null)
                    {
                        Data_PlayerConfig playerData = PlayerConfigProvider.Instance.GetPlayerConfigById(role_info.HeroId);
                        if (playerData != null)
                        {
                            spPortrait.spriteName = playerData.m_Portrait;
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OnEnable()
    {

    }
    public void ResetPlayerInfo()
    {
        OnEnable();
    }
    //购买金币
    public void OnBuyMoney()
    {
        UIManager.Instance.ShowWindowByName("GoldBuy");
    }
    //购买体力
    public void OnBuyStamina()
    {
        UIManager.Instance.ShowWindowByName("TiliBuy");
    }
}
