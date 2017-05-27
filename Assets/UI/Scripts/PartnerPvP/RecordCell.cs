using UnityEngine;
using System.Collections;
using ArkCrossEngine;
using System.Collections.Generic;
public class RecordCell : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject win;
    public UnityEngine.GameObject fail;
    public UILabel playerName;
    public UILabel playerLevel;
    public UILabel PlayerScore;
    public UILabel failNum;
    public UILabel winLabel;
    public UISprite heroHead;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*点击数据*/
    public void OnClickData()
    {
        LogicSystem.EventChannelForGfx.Publish("click_record_item", "record", this.gameObject);
    }
    //初始化item信息
    public void InitItemInfo(ChallengeInfo info)
    {
        int maxRank = 8000;
        ArenaBaseConfig bc = ArenaConfigProvider.Instance.GetBaseConfigById(1);
        if (null != bc)
        {
            maxRank = bc.MaxRank + 1;
        }
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        ChallengeEntityInfo challengeInfo;
        if (role.Guid == info.Challenger.Guid)
        { // 自己是挑战者
            challengeInfo = info.Target;
            int rankUp = 0;
            if (info.IsChallengerSuccess)
            {// 挑战别人赢了
                if (info.Target.Rank < 0)
                { // 别人未上榜 ， 名次不变
                    rankUp = 0;
                }
                else if (info.Challenger.Rank == -1 && info.Target.Rank > 0)
                { //自己未上榜 别人在榜上
                    rankUp = maxRank - info.Target.Rank;
                }
                else
                { // 都在榜上
                    if (info.Challenger.Rank - info.Target.Rank < 0)
                    {  // 名次高于他人  ，此时名次不变
                        rankUp = 0;
                    }
                    else
                    {//自己在榜上 ，且名次低于他人， 改变名次此时要改变名次 
                        rankUp = info.Challenger.Rank - info.Target.Rank;
                    }
                }
                winLabel.text = rankUp.ToString();
                NGUITools.SetActive(win, true);
                NGUITools.SetActive(fail, false);
            }
            else
            {// 挑战他人失败
                if (winLabel != null)
                {
                    failNum.text = "0";
                }
                NGUITools.SetActive(win, false);
                NGUITools.SetActive(fail, true);
            }
        }
        else
        { // 自己是被挑战者
            challengeInfo = info.Challenger;
            if (info.IsChallengerSuccess)
            { // 挑战者赢了 (我输了)， 
                NGUITools.SetActive(win, false);
                NGUITools.SetActive(fail, true);
                if (winLabel != null)
                {
                    int rankUp = 0;
                    if (info.Target.Rank == -1)
                    {// 我未上榜
                        rankUp = 0;
                    }
                    else if (info.Challenger.Rank == -1)
                    { // 他未上榜，那么我将是未上榜的人了 下降自己的名次
                        rankUp = info.Target.Rank + 1;
                    }
                    else if (info.Target.Rank - info.Challenger.Rank > 0)
                    { // 都在榜上，我名次低于他 名次不变
                        rankUp = 0;
                    }
                    else
                    { // 我名次高于他
                        rankUp = info.Challenger.Rank - info.Target.Rank;
                    }
                    failNum.text = rankUp.ToString();
                }
            }
            else
            { // 我赢了（挑战的人输了） 名次不变
                NGUITools.SetActive(win, true);
                NGUITools.SetActive(fail, false);
                if (winLabel != null)
                {
                    winLabel.text = "0";
                }
            }
        }
        if (playerName != null)
        {
            playerName.text = challengeInfo.NickName;
        }
        if (playerLevel != null)
        {
            playerLevel.text = "Lv." + challengeInfo.Level.ToString();
        }
        if (PlayerScore != null)
        {
            PlayerScore.text = challengeInfo.FightScore.ToString();
        }
        if (heroHead != null)
        {
            Data_PlayerConfig cg = PlayerConfigProvider.Instance.GetPlayerConfigById(challengeInfo.HeroId);
            heroHead.spriteName = cg.m_PortraitForCell;
        }
    }
}
