using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class UIRecordData : UnityEngine.MonoBehaviour
{

    public UISprite spHeadL = null;
    public UISprite spHeadR = null;
    public UILabel lblNameL = null;
    public UILabel lblNameR = null;
    public UISprite spWinOrLose = null;

    public List<UnityEngine.GameObject> leftItemList = new List<UnityEngine.GameObject>();
    public List<UnityEngine.GameObject> rightItemList = new List<UnityEngine.GameObject>();

    private int m_MaxDamage = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowRecord(ChallengeInfo cInfo)
    {

        RoleInfo role = LobbyClient.Instance.CurrentRole;
        if (role == null)
            return;

        UIManager.Instance.ShowWindowByName("PPVPRecordData");

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

        if (spWinOrLose != null)
        {
            spWinOrLose.spriteName = isWin ? "win" : "lose";
            int num = spWinOrLose.transform.childCount;
            for (int i = 0; i < num; i++)
            {
                NGUITools.SetActive(spWinOrLose.transform.GetChild(i).gameObject, isWin);
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

    private void UpdateItem(UnityEngine.GameObject go, int id, int damage, bool isHero)
    {
        //伤害值，及对应对比进度条
        UnityEngine.Transform tf = go.transform.Find("value");
        if (tf != null)
        {
            UIProgressBar progress = tf.GetComponent<UIProgressBar>();
            if (progress != null)
            {
                progress.value = (float)damage / m_MaxDamage;
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

    private string GetHeadSpName(int heroId)
    {
        Data_PlayerConfig cg = PlayerConfigProvider.Instance.GetPlayerConfigById(heroId);
        if (cg != null)
        {
            return cg.m_Portrait;
        }
        return "";
    }

    public void OnClickClose()
    {
        UIManager.Instance.HideWindowByName("PPVPRecordData");
    }
}
