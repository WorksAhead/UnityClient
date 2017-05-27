using UnityEngine;
using System.Collections;
using ArkCrossEngine;
using System.Collections.Generic;

public class UIRuleIntro : UnityEngine.MonoBehaviour
{

    public UnityEngine.GameObject goRuleAwardItem = null;//prefab
    public UnityEngine.GameObject goAwardParent = null;
    public UILabel lblRule = null;
    public UILabel lblRankTxt = null;
    public UILabel lblMyRank = null;
    public UnityEngine.GameObject goCurrentAward = null;
    public UnityEngine.GameObject goTxtRule = null;
    public UnityEngine.GameObject goAwardRule = null;
    public float yChange = 88;
    public UILabel lblCurrDiamond = null;
    public UILabel lblCurrGold = null;
    public UILabel lblCurrTxt = null;

    private UnityEngine.Vector3 defaultTxtRuleVec;
    private UnityEngine.Vector3 defaultAwardRuleVec;
    private int nameIndex = 900;

    private List<ArenaPrizeConfig> m_configList = new List<ArenaPrizeConfig>();
    private ArenaPrizeConfig m_awardConfig = null;
    private bool hasStart = false;
    void Start()
    {
        try
        {
            if (hasStart == false)
            {
                if (goTxtRule != null)
                {
                    defaultTxtRuleVec = goTxtRule.transform.localPosition;
                }
                if (goAwardRule != null)
                {
                    defaultAwardRuleVec = goAwardRule.transform.localPosition;
                }
            }
            if (lblRule != null)
            {
                lblRule.text = StrDictionaryProvider.Instance.GetDictString(1116).Replace("\\n", "\n");
            }
            hasStart = true;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowIntro()
    {
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        if (role == null)
        {
            return;
        }
        if (hasStart == false)
        {
            Start();
        }
        nameIndex = 900;
        int myRank = role.ArenaStateInfo.Rank;
        m_awardConfig = GetAwardConfig(myRank);
        UpdateLabel(myRank);
        UpdatePosition(myRank);
        UpdateCurrentAward(myRank);
        UpdateAwardRule(myRank);

        UIManager.Instance.ShowWindowByName("PPVPRuleIntro");
    }

    private ArenaPrizeConfig GetAwardConfig(int rank)
    {
        ArenaPrizeConfig config = null;
        MyDictionary<int, object> dic = ArenaConfigProvider.Instance.PrizeConfig.GetData();
        m_configList.Clear();
        foreach (object o in dic.Values)
        {
            ArenaPrizeConfig apc = (ArenaPrizeConfig)o;
            m_configList.Add(apc);
        }
        m_configList.Sort(SortConfigList);
        for (int i = 0; i < m_configList.Count; i++)
        {
            if (rank < m_configList[i].FitBegin)
            {
                break;
            }
            config = m_configList[i];
        }
        return config;
    }

    private int SortConfigList(ArenaPrizeConfig a, ArenaPrizeConfig b)
    {
        if (a.FitBegin < b.FitBegin)
        {
            return -1;
        }
        else if (a.FitBegin == b.FitBegin)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    private void UpdateLabel(int rank)
    {
        if (lblMyRank != null)
        {
            if (rank == -1)
            {//暂无排名
                lblRankTxt.text = StrDictionaryProvider.Instance.GetDictString(1101);//"当前暂无排名";
                lblMyRank.text = "";
            }
            else
            {
                lblRankTxt.text = StrDictionaryProvider.Instance.GetDictString(1102);//"你的当前排名：";
                lblMyRank.text = rank.ToString();
            }
        }
    }

    private void UpdatePosition(int rank)
    {
        if (rank == -1)
        {//暂无排名
            if (goCurrentAward != null)
            {
                NGUITools.SetActive(goCurrentAward, false);
            }
            if (goTxtRule != null)
            {
                goTxtRule.transform.localPosition = new UnityEngine.Vector3(defaultTxtRuleVec.x, defaultTxtRuleVec.y + yChange, defaultTxtRuleVec.z);
            }
            if (goAwardRule != null)
            {
                goAwardRule.transform.localPosition = new UnityEngine.Vector3(defaultAwardRuleVec.x, defaultAwardRuleVec.y + yChange, defaultAwardRuleVec.z);
            }
        }
        else
        {
            if (goCurrentAward != null)
            {
                NGUITools.SetActive(goCurrentAward, true);
            }
            if (goTxtRule != null)
            {
                goTxtRule.transform.localPosition = defaultTxtRuleVec;
            }
            if (goAwardRule != null)
            {
                goAwardRule.transform.localPosition = defaultAwardRuleVec;
            }
        }
    }

    private void UpdateCurrentAward(int rank)
    {
        if (rank == -1)
        {
            return;
        }

        if (lblCurrDiamond != null)
        {
            lblCurrDiamond.text = m_awardConfig.Gold.ToString();
        }
        if (lblCurrGold != null)
        {
            lblCurrGold.text = m_awardConfig.Money.ToString();
        }
        if (lblCurrTxt != null)
        {
            string str = "";
            if (m_awardConfig.FitEnd - m_awardConfig.FitBegin == 1)
            {
                str = StrDictionaryProvider.Instance.Format(1103, m_awardConfig.FitBegin);//第{0}名
            }
            else
            {
                str = StrDictionaryProvider.Instance.Format(1104, m_awardConfig.FitBegin, m_awardConfig.FitEnd - 1);//第{0}至{1}名
            }
            lblCurrTxt.text = StrDictionaryProvider.Instance.Format(1100, str);
        }
    }

    private void UpdateAwardRule(int rank)
    {
        if (goRuleAwardItem == null || goAwardParent == null)
            return;
        int oldDataNum = goAwardParent.transform.childCount;
        for (int i = oldDataNum - 1; i >= 0; i--)
        {
            DestroyImmediate(goAwardParent.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < 5; i++)
        {//前5档必显示
            UnityEngine.GameObject item = NGUITools.AddChild(goAwardParent, goRuleAwardItem);
            UpdateAwardItem(item, m_configList[i]);
            item.name = nameIndex++ + "";
        }
        int currentIndex = m_configList.IndexOf(m_awardConfig);
        int maxIndex = m_configList.Count - 1;
        if (rank == -1)
        {
            currentIndex = maxIndex + 1;//表的长度+1
        }
        int frontNum = 2;//前2
        int behindNum = 1;//后1
        for (int i = frontNum + 1; i >= -behindNum; i--)
        {
            if (currentIndex - i > 5 && currentIndex - i <= maxIndex)
            {
                UnityEngine.GameObject item = NGUITools.AddChild(goAwardParent, goRuleAwardItem);
                UpdateAwardItem(item, m_configList[currentIndex - i], i == frontNum + 1);
                item.name = nameIndex++ + "";
            }
        }
        UIGrid grid = goAwardParent.GetComponent<UIGrid>();
        if (grid != null)
        {
            //grid.sorted = true;
            grid.repositionNow = true;
        }
    }

    private void UpdateAwardItem(UnityEngine.GameObject item, ArenaPrizeConfig config, bool showDot = false)
    {
        if (item == null)
            return;
        UnityEngine.Transform tf = null;
        //显示省略号
        if (showDot == true)
        {
            tf = item.transform.Find("Diamond");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, !showDot);
            }
            tf = item.transform.Find("money");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, !showDot);
            }
            tf = item.transform.Find("Label");
            if (tf != null)
            {
                UILabel lblDot = tf.GetComponent<UILabel>();
                lblDot.text = "... ...";
            }
            return;
        }
        //正常显示
        tf = item.transform.Find("Label");
        if (tf != null)
        {
            UILabel lblRank = tf.GetComponent<UILabel>();
            if (lblRank != null)
            {
                if (config.FitEnd - config.FitBegin == 1)
                {
                    lblRank.text = StrDictionaryProvider.Instance.Format(1103, config.FitBegin);//第{0}名
                }
                else
                {
                    lblRank.text = StrDictionaryProvider.Instance.Format(1104, config.FitBegin, config.FitEnd - 1);//第{0}至{1}名
                }
            }
        }
        tf = item.transform.Find("Diamond/Label");
        if (tf != null)
        {
            UILabel lblDiamond = tf.GetComponent<UILabel>();
            if (lblDiamond != null)
            {
                lblDiamond.text = config.Gold.ToString();
            }
        }
        tf = item.transform.Find("money/Label");
        if (tf != null)
        {
            UILabel lblGold = tf.GetComponent<UILabel>();
            if (lblGold != null)
            {
                lblGold.text = config.Money.ToString();
            }
        }
    }

    public void OnClickClose()
    {
        UIManager.Instance.HideWindowByName("PPVPRuleIntro");
    }
}
