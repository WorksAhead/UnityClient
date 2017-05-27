using UnityEngine;
using System.Collections;
using System;
using ArkCrossEngine;

public class BottomInfoContainer : UnityEngine.MonoBehaviour
{

    public UIProgressBar progressHp = null;
    public UILabel labelHp = null;
    public UIProgressBar progressDamage = null;
    public UILabel labelDamage = null;
    public UIProgressBar progressArmor = null;
    public UILabel labelArmor = null;
    public UIProgressBar progressMdef = null;
    public UILabel labelMdef = null;

    public UILabel labelDes1 = null;
    public UILabel labelDes2 = null;
    public UILabel labelDes3 = null;
    public UILabel labelDes4 = null;

    private ArrayList lvList = null;
    private ArrayList desLabelList = null;

    // Use this for initialization
    void OnEnable()
    {
        try
        {
            //if (lvList == null) {
            //  lvList = new ArrayList();
            //  lvList.Add(3);
            //  lvList.Add(5);
            //  lvList.Add(7);
            //  lvList.Add(9);
            //}

            if (labelDes1 != null && labelDes2 != null && labelDes3 != null && labelDes4 != null && desLabelList == null)
            {
                desLabelList = new ArrayList();
                desLabelList.Add(labelDes1);
                desLabelList.Add(labelDes2);
                desLabelList.Add(labelDes3);
                desLabelList.Add(labelDes4);
            }
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

    public void UpdateView(ItemDataInfo itemInfo)
    {
        int lvIndex = 0;
        for (int i = 0; i < lvList.Count; i++)
        {
            lvIndex = i;
            if (itemInfo.Level < Convert.ToInt32(lvList[i]))
            {
                break;
            }
            if (i == lvList.Count - 1)
            {
                lvIndex = lvList.Count;
            }
        }

        if (desLabelList != null)
        {
            for (int i = 0; i < desLabelList.Count; i++)
            {
                UILabel label = (UILabel)desLabelList[i];

                UnityEngine.Transform tfDesc = label.transform.Find("LabelDesc");
                if (tfDesc != null)
                {
                    UILabel labelDesc = tfDesc.GetComponent<UILabel>();
                    if (labelDesc != null)
                    {
                        if (i < lvIndex)
                        {
                            label.color = new UnityEngine.Color(1f, 0.5255f, 0.1804f);//亮
                            labelDesc.color = new UnityEngine.Color(1f, 1f, 1f);
                        }
                        else
                        {
                            label.color = new UnityEngine.Color(0.3882f, 0.3882f, 0.3882f);//灰
                            labelDesc.color = new UnityEngine.Color(0.3882f, 0.3882f, 0.3882f);
                        }
                    }
                }
            }
        }
        XSoulLevelConfig config = XSoulLevelConfigProvider.Instance.GetDataById(itemInfo.ItemId);
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        UserInfo userInfo = role_info.GetPlayerSelfInfo();

        //设置进度条，数值
        ItemConfig itemConfig = ItemConfigProvider.Instance.GetDataById(itemInfo.ItemId);
        float current = itemConfig.m_AttrData.GetAddHpMax(0, userInfo.GetLevel(), itemInfo.Level - 1);
        float max = itemConfig.m_AttrData.GetAddHpMax(0, userInfo.GetLevel(), config.m_MaxLevel - 1);
        SetProgressValue(progressHp, labelHp, current, max);
        current = itemConfig.m_AttrData.GetAddAd(0, userInfo.GetLevel(), itemInfo.Level - 1);
        max = itemConfig.m_AttrData.GetAddAd(0, userInfo.GetLevel(), config.m_MaxLevel - 1);
        SetProgressValue(progressDamage, labelDamage, current, max);
        current = itemConfig.m_AttrData.GetAddADp(0, userInfo.GetLevel(), itemInfo.Level - 1);
        max = itemConfig.m_AttrData.GetAddADp(0, userInfo.GetLevel(), config.m_MaxLevel - 1);
        SetProgressValue(progressArmor, labelArmor, current, max);
        current = itemConfig.m_AttrData.GetAddMDp(0, userInfo.GetLevel(), itemInfo.Level - 1);
        max = itemConfig.m_AttrData.GetAddMDp(0, userInfo.GetLevel(), config.m_MaxLevel - 1);
        SetProgressValue(progressMdef, labelMdef, current, max);
    }

    private void SetProgressValue(UIProgressBar progress, UILabel label, float current, float max)
    {
        if (progress != null)
        {
            bool view = max <= 0 ? false : true;
            NGUITools.SetActive(progress.transform.parent.gameObject, view);

            progress.value = (float)current / max;
            NGUITools.SetActive(progress.gameObject, true);
        }
        if (label != null)
        {
            label.text = ((int)current).ToString();
        }
    }

    public void InitLabels(ItemDataInfo itemInfo)
    {
        if (lvList == null)
        {
            lvList = new ArrayList();
        }
        lvList.Clear();

        if (itemInfo != null)
        {
            ItemConfig itemConfig = ItemConfigProvider.Instance.GetDataById(itemInfo.ItemId);
            int[] dat = itemConfig.m_ActiveBuffOnLevel;
            if (desLabelList != null)
            {
                for (int i = 0; i < desLabelList.Count; i++)
                {
                    UILabel labelLv = (UILabel)desLabelList[i];

                    UnityEngine.Transform tfDesc = labelLv.transform.Find("LabelDesc");
                    if (tfDesc != null)
                    {
                        UILabel labelDesc = tfDesc.GetComponent<UILabel>();
                        if (labelDesc != null)
                        {
                            if (2 * i < dat.Length)
                            {
                                lvList.Add(dat[i * 2]);
                                labelLv.text = "Lv." + dat[i * 2];
                                ImpactLogicData impactLogicData = SkillConfigProvider.Instance.ExtractData(SkillConfigType.SCT_IMPACT, dat[i * 2 + 1]) as ImpactLogicData;
                                if (impactLogicData == null)
                                    labelDesc.text = "";
                                else
                                    labelDesc.text = impactLogicData.ImpactDescription;
                            }
                            else
                            {
                                labelLv.text = "";
                                labelDesc.text = "";
                            }
                        }
                    }
                }
            }
        }
    }

}
