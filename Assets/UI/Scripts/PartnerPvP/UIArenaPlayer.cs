using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;
public class UIArenaPlayer : UnityEngine.MonoBehaviour
{
    public UISprite spPortrait;
    public UILabel lblLevel;
    public UILabel lblName;
    public UILabel lblFighting;
    public UILabel lblRank;//排名
    public UIButton btnChallenge;
    private ArenaTargetInfo m_PlayerInfo = null;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //初始化玩家信息
    public void InitPlayerInfo(ArenaTargetInfo playerInfo)
    {
        if (null == playerInfo) return;
        m_PlayerInfo = playerInfo;
        if (lblLevel != null) lblLevel.text = playerInfo.Level.ToString();
        if (lblName != null) lblName.text = playerInfo.Nickname;
        if (lblRank != null) lblRank.text = playerInfo.Rank.ToString();
        if (lblFighting != null) lblFighting.text = playerInfo.FightingScore.ToString();
        //还差个战力
        Data_PlayerConfig playerCfg = PlayerConfigProvider.Instance.GetPlayerConfigById(playerInfo.HeroId);
        if (playerCfg != null && spPortrait != null)
        {
            spPortrait.spriteName = playerCfg.m_Portrait;
        }
    }
    public void OnChallengeClick()
    {
        LogicSystem.PublishLogicEvent("start_challenge", "arena", m_PlayerInfo.Guid);
        //     UIPartnerPvpRightInfo right_info = NGUITools.FindInParents<UIPartnerPvpRightInfo>(gameObject);
        //     if (right_info != null) {
        //       right_info.RefreshMatchPlayerInfo(true);
        //     }
    }
    public void OnClick()
    {
        //点击查看玩家信息按钮
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("PPVPFighterIntro");
        if (go != null)
        {
            UIFighterIntro fighterIntro = go.GetComponent<UIFighterIntro>();
            if (fighterIntro != null) fighterIntro.ShowIntro(m_PlayerInfo);
        }
    }
    public void EnableChallengeButton(bool enable)
    {
        if (btnChallenge == null) return;
        if (enable)
            btnChallenge.defaultColor = UnityEngine.Color.white;
        else
            btnChallenge.defaultColor = btnChallenge.disabledColor;
        btnChallenge.isEnabled = enable;
    }
}
