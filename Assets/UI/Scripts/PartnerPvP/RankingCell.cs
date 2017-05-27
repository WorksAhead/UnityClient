using UnityEngine;
using System.Collections;
using ArkCrossEngine;
using System.Collections.Generic;

public class RankingCell : UnityEngine.MonoBehaviour
{

    public UILabel playerName;
    public UILabel playerLevel;
    public UILabel PlayerScore;
    public UISprite heroHead;

    public UISprite cup;
    public UILabel number;
    private const int arrNum = 3;
    public UISprite[] portrait = new UISprite[arrNum];
    public UISprite[] portraitFrame = new UISprite[arrNum];

    public UILabel unRank;
    public UnityEngine.Color color;
    public UnityEngine.Color color1;
    public UISprite bg;
    public UnityEngine.GameObject sprite1;
    public UnityEngine.GameObject sprite2;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //初始化item信息
    public void InitItemInfo(ArenaTargetInfo info)
    {
        if (info == null)
        {
            NGUITools.SetActive(sprite1, false);
            NGUITools.SetActive(sprite2, true);
            return;
        }
        NGUITools.SetActive(sprite2, false);
        NGUITools.SetActive(sprite1, true);
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        if (info.Guid == role.Guid)
        {
            bg.color = color;
        }
        else
        {
            bg.color = color1;
        }
        if (playerName != null)
        {
            playerName.text = info.Nickname;
        }
        if (playerLevel != null)
        {
            playerLevel.text = "Lv." + info.Level.ToString();
        }
        if (PlayerScore != null)
        {
            PlayerScore.text = info.FightingScore.ToString();
        }
        unRank.text = "";
        if (info.Rank < 4)
        {
            switch (info.Rank)
            {
                case 1:
                    cup.spriteName = "no1";
                    number.text = "NODA";
                    NGUITools.SetActive(cup.gameObject, true);
                    break;
                case 2:
                    cup.spriteName = "no2";
                    number.text = "NODB";
                    NGUITools.SetActive(cup.gameObject, true);
                    break;
                case 3:
                    cup.spriteName = "no3";
                    number.text = "NODC";
                    NGUITools.SetActive(cup.gameObject, true);
                    break;
                case -1:
                    number.text = "";
                    unRank.text = "未排名";
                    NGUITools.SetActive(cup.gameObject, false);
                    break;
            }
        }
        else
        {
            number.text = info.Rank + "ETH";
            NGUITools.SetActive(cup.gameObject, false);
        }
        if (heroHead != null)
        {
            Data_PlayerConfig cg = PlayerConfigProvider.Instance.GetPlayerConfigById(info.HeroId);
            heroHead.spriteName = cg.m_PortraitForCell;
        }
        SetPartnerPortraitActive();
        if (info.Guid == role.Guid)
        { // 自己 由于服务器发回来的列表里没有自己，所以自己要担架
            for (int i = 0; i < role.ArenaStateInfo.FightPartners.Count; i++)
            {
                PartnerInfo partnerInfo = GetPartnerInfoById(role.ArenaStateInfo.FightPartners[i]);
                Data_NpcConfig npcCfg = NpcConfigProvider.Instance.GetNpcConfigById(partnerInfo.LinkId);
                if (npcCfg != null)
                {
                    if (i <= arrNum)
                    {
                        SetPartnerIcon(portrait[i], portraitFrame[i], partnerInfo);
                        NGUITools.SetActive(portrait[i].gameObject, true);
                    }
                }
            }
        }
        else
        { // 列表其他人
            for (int i = 0; i < info.FightPartners.Count; i++)
            {
                Data_NpcConfig npcCfg = NpcConfigProvider.Instance.GetNpcConfigById(info.FightPartners[i].LinkId);
                if (npcCfg != null)
                {
                    if (i <= arrNum)
                    {
                        SetPartnerIcon(portrait[i], portraitFrame[i], info.FightPartners[i]);
                        NGUITools.SetActive(portrait[i].gameObject, true);
                    }
                }
            }
        }
    }
    //设置伙伴不可见
    void SetPartnerPortraitActive()
    {
        for (int i = 0; i < arrNum; i++)
        {
            NGUITools.SetActive(portrait[i].gameObject, false);
        }
    }
    //设置伙伴图标
    public void SetPartnerIcon(UISprite fram, UISprite icon, PartnerInfo info)
    {
        PartnerLevelUpConfig levelUpCfg = PartnerLevelUpConfigProvider.Instance.GetDataById(info.CurAdditionLevel);
        Data_NpcConfig npcCfg = NpcConfigProvider.Instance.GetNpcConfigById(info.LinkId);
        if (fram != null && levelUpCfg != null)
        {
            fram.spriteName = levelUpCfg.PartnerRankColor;
        }
        if (icon != null)
        {
            icon.spriteName = npcCfg.m_Portrait;
        }
    }
    private PartnerInfo GetPartnerInfoById(int partner_id)
    {
        RoleInfo info = LobbyClient.Instance.CurrentRole;
        if (info != null && info.PartnerStateInfo != null)
        {
            List<PartnerInfo> partnerList = info.PartnerStateInfo.GetAllPartners();
            if (partnerList != null)
            {
                for (int i = 0; i < partnerList.Count; ++i)
                {
                    if (partnerList[i] != null && partnerList[i].Id == partner_id)
                        return partnerList[i];
                }
            }
        }
        return null;
    }
    public void ClickItem()
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        SearchItem(role.ArenaStateInfo.RankList);

    }
    //找到点击的数据
    void SearchItem(List<ArenaTargetInfo> RankList)
    {
        foreach (ArenaTargetInfo info in RankList)
        {
            if (playerName.text == info.Nickname)
            {
                //LogicSystem.EventChannelForGfx.Publish("record_click_item", "record", info);
                UnityEngine.GameObject rd = UIManager.Instance.GetWindowGoByName("PPVPFighterIntro");
                if (rd != null)
                {
                    UIFighterIntro uifi = rd.GetComponent<UIFighterIntro>();
                    if (uifi != null)
                    {
                        uifi.ShowIntro(info);
                    }
                }
            }
        }
    }
}
