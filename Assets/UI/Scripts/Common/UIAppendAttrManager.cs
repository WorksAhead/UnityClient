/*
 *  附加属性类
 *  用于装备、伙伴等需要显示附加属性的界面
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;
public enum UIAppendAttrEnum
{
    AddHpMax1 = 600,
    AddAd1 = 601,
    AddCri1,
    AddPow1 = 603,
    AddBackHitPow1 = 604,
    AddHpMax2 = 605,
    AddAd2 = 606,
    AddAdp2 = 607,
    AddMdp2 = 608,
    AddFireDam1 = 609,
    AddFireErd1 = 610,
}
public enum UIAttrShowType
{
    Absolute,
    Percent
}
public class UIAppendAttrManager
{
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public List<string> GetAppendProperty(int property_id)
    {
        ArkCrossEngine.AppendAttributeConfig aacfg = ArkCrossEngine.AppendAttributeConfigProvider.Instance.GetDataById(property_id);
        if (aacfg == null)
        {
            Debug.Log("Get append attr failed. append propertyID:" + property_id);
            return null;
        }
        float data = 0.0f;
        int level = 1;
        List<string> attrList = new List<string>();
        //属性获取根据AppendAtt配表的顺序依次添加
        data = aacfg.GetAddHpMax1(1.0f, level);
        FormatAbsoluteAppendAttr(data, UIAppendAttrEnum.AddHpMax1, attrList);

        data = aacfg.GetAddAd1(1.0f, level);
        FormatAbsoluteAppendAttr(data, UIAppendAttrEnum.AddAd1, attrList);

        data = aacfg.GetAddCri1(1.0f, level);
        FormatAbsoluteAppendAttr(data, UIAppendAttrEnum.AddCri1, attrList);

        data = aacfg.GetAddPow1(1.0f, level);
        FormatAbsoluteAppendAttr(data, UIAppendAttrEnum.AddPow1, attrList);

        data = aacfg.GetAddBackHitPow1(1.0f, level);
        FormatAbsoluteAppendAttr(data, UIAppendAttrEnum.AddBackHitPow1, attrList);

        data = aacfg.GetAddHpMax2(1.0f, level);
        FormatPercentAppendAttr(data, UIAppendAttrEnum.AddHpMax2, attrList);

        data = aacfg.GetAddAd2(1.0f, level);
        FormatPercentAppendAttr(data, UIAppendAttrEnum.AddAd2, attrList);

        data = aacfg.GetAddADp2(1.0f, level);
        FormatPercentAppendAttr(data, UIAppendAttrEnum.AddAdp2, attrList);

        data = aacfg.GetAddMDp2(1.0f, level);
        FormatPercentAppendAttr(data, UIAppendAttrEnum.AddMdp2, attrList);

        data = aacfg.GetAddFireDam1(1.0f, level);
        FormatAbsoluteAppendAttr(data, UIAppendAttrEnum.AddFireDam1, attrList);

        data = aacfg.GetAddFireErd1(1.0f, level);
        FormatAbsoluteAppendAttr(data, UIAppendAttrEnum.AddFireErd1, attrList);
        return attrList;
    }

    //绝对值形式显示
    private void FormatAbsoluteAppendAttr(float value, UIAppendAttrEnum dict_id, List<string> attrList)
    {
        if (UnityEngine.Mathf.Abs(value - 0) > float.Epsilon)
        {
            string strAttr = (ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString((int)dict_id) + "+" + UnityEngine.Mathf.FloorToInt(value));
            if (attrList != null) attrList.Add(strAttr);
        }
    }
    //百分比形式显示
    private void FormatPercentAppendAttr(float value, UIAppendAttrEnum dict_id, List<string> attrList)
    {
        if (UnityEngine.Mathf.Abs(value - 0) > float.Epsilon)
        {
            string strAttr = (ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString((int)dict_id) + "+" + (value * 100) + "%");
            if (attrList != null) attrList.Add(strAttr);
        }
    }
    static private UIAppendAttrManager m_Instance = new UIAppendAttrManager();
    static public UIAppendAttrManager Instance
    {
        get
        {
            return m_Instance;
        }
    }
}
