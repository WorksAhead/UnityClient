using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ArkCrossEngine;

public class UIPartnerPvpVictory : UnityEngine.MonoBehaviour
{

    public UISprite spHeadL = null;
    public UISprite spHeadR = null;
    public UILabel lblNameL = null;
    public UILabel lblNameR = null;
    public UISprite win = null;
    public UISprite lose = null;
    public List<UnityEngine.GameObject> leftItemList = new List<UnityEngine.GameObject>();
    public List<UnityEngine.GameObject> rightItemList = new List<UnityEngine.GameObject>();

    private int m_MaxDamage = 0;
    private List<object> m_EventList = new List<object>();

    public void UnSubscribe()
    {
        try
        {
            for (int i = 0; i < m_EventList.Count; i++)
            {
                if (m_EventList[i] != null)
                {
                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(m_EventList[i]);
                }
            }
            m_EventList.Clear();
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void Awake()
    {
        try
        {
            object obj = null;
            obj = LogicSystem.EventChannelForGfx.Subscribe<ChallengeInfo>("ge_partnerpvp_result", "ui", InitVictoryPanel);
            if (obj != null)
                m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null)
                m_EventList.Add(obj);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void Start()
    {

    }

    void OnEnable()
    {
        try
        {
            //结束画面开始前结束普通攻击
            UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
            if (go != null)
            {
                SkillBar sBar = go.GetComponent<SkillBar>();
                if (sBar != null)
                {
                    sBar.StopAttack();
                    if (ArkCrossEngine.LogicSystem.PlayerSelf != null)
                        GfxModule.Skill.GfxSkillSystem.Instance.StopAttack(ArkCrossEngine.LogicSystem.PlayerSelf);
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void Update()
    {

    }


    private void InitVictoryPanel(ChallengeInfo cInfo)
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        if (role == null)
            return;

        UIManager.Instance.ShowWindowByName("PPVPVictoryPanel");

        ChallengeEntityInfo myCInfo = cInfo.Target.Guid == role.Guid ? cInfo.Target : cInfo.Challenger;
        ChallengeEntityInfo theirCInfo = cInfo.Target.Guid == role.Guid ? cInfo.Challenger : cInfo.Target;

        bool isWin = cInfo.Target.Guid == role.Guid ? cInfo.IsChallengerSuccess ? false : true : cInfo.IsChallengerSuccess ? true : false;
        if (spHeadL != null)
        {
            spHeadL.spriteName = GetHeadSpName(myCInfo.HeroId);
        }
        if (spHeadR != null)
        {
            spHeadR.spriteName = GetHeadSpName(theirCInfo.HeroId);
        }
        if (lblNameL != null)
        {
            lblNameL.text = myCInfo.NickName;
        }
        if (lblNameR != null)
        {
            lblNameR.text = theirCInfo.NickName;
        }
        if (isWin)
        {
            if (win != null)
            {
                NGUITools.SetActive(win.gameObject, true);
                NGUITools.SetActive(lose.gameObject, false);
            }
        }
        else
        {
            if (lose != null)
            {
                NGUITools.SetActive(win.gameObject, false);
                NGUITools.SetActive(lose.gameObject, true);
            }
        }

        m_MaxDamage = GetMaxDamage(cInfo);
        for (int i = 0; i < leftItemList.Count; i++)
        {
            if (i == 0)
            {//主角
                UpdateItem(leftItemList[i], myCInfo.HeroId, myCInfo.UserDamage, true);
            }
            else
            {
                if (myCInfo.PartnerDamage != null && myCInfo.PartnerDamage.Count >= i)
                {
                    UpdateItem(leftItemList[i], myCInfo.PartnerDamage[i - 1].OwnerId, myCInfo.PartnerDamage[i - 1].Damage, false);
                    NGUITools.SetActive(leftItemList[i], true);
                }
                else
                {
                    NGUITools.SetActive(leftItemList[i], false);
                }
            }
        }

        for (int i = 0; i < rightItemList.Count; i++)
        {
            if (i == 0)
            {//主角
                UpdateItem(rightItemList[i], theirCInfo.HeroId, theirCInfo.UserDamage, true);
            }
            else
            {
                if (theirCInfo.PartnerDamage != null && theirCInfo.PartnerDamage.Count >= i)
                {
                    UpdateItem(rightItemList[i], theirCInfo.PartnerDamage[i - 1].OwnerId, theirCInfo.PartnerDamage[i - 1].Damage, false);
                    NGUITools.SetActive(rightItemList[i], true);
                }
                else
                {
                    NGUITools.SetActive(rightItemList[i], false);
                }
            }
        }
    }

    private void UpdateItem(UnityEngine.GameObject go, int id, int damage, bool isHero)
    {
        //伤害值，及对应对比进度条
        UnityEngine.Transform tf = go.transform.Find("value");
        if (tf != null)
        {
            UIProgressBar progress = tf.GetComponent<UIProgressBar>();
            if (progress != null)
            {
                progress.value = 0;
            }
            tf = tf.Find("Label");
            if (tf != null)
            {
                UILabel lblValue = tf.GetComponent<UILabel>();
                if (lblValue != null)
                {
                    lblValue.text = damage.ToString();
                }
            }
        }
        //头像
        tf = go.transform.Find("Sprite");
        if (tf != null)
        {
            UISprite sprite = tf.GetComponent<UISprite>();
            if (sprite != null)
            {
                string spName = "";
                if (isHero)
                {
                    spName = GetHeadSpName(id);
                }
                else
                {
                    PartnerConfig pCfg = PartnerConfigProvider.Instance.GetDataById(id);
                    if (pCfg != null)
                    {
                        Data_NpcConfig npcCfg = NpcConfigProvider.Instance.GetNpcConfigById(pCfg.LinkId);
                        if (npcCfg != null)
                        {
                            spName = npcCfg.m_Portrait;
                        }
                    }
                }
                sprite.spriteName = spName;
            }
        }
        //彩框
        UISprite colorFrame = go.GetComponent<UISprite>();
        if (colorFrame != null)
        {
            //colorFrame.spriteName = "";
        }
    }

    private int GetMaxDamage(ChallengeInfo cInfo)
    {
        List<int> damageList = new List<int>();
        damageList.Add(cInfo.Challenger.UserDamage);
        damageList.Add(cInfo.Target.UserDamage);
        if (cInfo.Challenger.PartnerDamage != null)
        {
            foreach (DamageInfo pd in cInfo.Challenger.PartnerDamage)
            {
                damageList.Add(pd.Damage);
            }
        }
        if (cInfo.Target.PartnerDamage != null)
        {
            foreach (DamageInfo pd in cInfo.Target.PartnerDamage)
            {
                damageList.Add(pd.Damage);
            }
        }
        damageList.Sort();
        return damageList[damageList.Count - 1];
    }

    private string GetHeadSpName(int heroId)
    {
        Data_PlayerConfig cg = PlayerConfigProvider.Instance.GetPlayerConfigById(heroId);
        if (cg != null)
        {
            return cg.m_Portrait;
        }
        return "";
    }

    public void AutoClose()
    {
        GfxSystem.EventChannelForLogic.Publish("ge_return_maincity", "lobby");
    }
}
