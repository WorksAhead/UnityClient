using ArkCrossEngine;
using System.Collections.Generic;
public class ArtifactRightInfo : UnityEngine.MonoBehaviour
{
    private const int c_ButtonNumber = 7;
    public UnityEngine.GameObject[] buttons = new UnityEngine.GameObject[c_ButtonNumber];
    public UnityEngine.GameObject[] lines = new UnityEngine.GameObject[c_ButtonNumber];
    public UnityEngine.GameObject[] tipsBtn = new UnityEngine.GameObject[c_ButtonNumber];
    public UILabel lblLegacyAttr = null;//总属性
    private int m_CurrentArtifactId = -1;// 当前神器id
                                         // Use this for initialization
    void Start()
    {
        try
        {
            InitButtonClick();
            SetInfo();
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
    // 设置神器信息
    public void SetInfo()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            UISprite us;
            for (int i = 0; i < buttons.Length; i++)
            {
                us = buttons[i].GetComponent<UISprite>();
                if (us != null)
                {
                    if (i < role_info.Legacys.Length)
                    {
                        if (role_info.Legacys[i].IsUnlock)
                        {
                            us.spriteName = "shenqi-" + (i + 1);
                        }
                        else
                        {
                            us.spriteName = "shenqihui-" + (i + 1);
                        }
                    }
                    else
                    {
                        us.spriteName = "di";
                    }
                }
                buttons[i].GetComponent<UIButton>().normalSprite = us.spriteName;
            }
            LineShow();
            showAllAtrr();
        }
    }

    // 初始化按钮点击
    void InitButtonClick()
    {
        for (int index = 0; index < buttons.Length; ++index)
        {
            if (buttons[index] != null)
                UIEventListener.Get(buttons[index]).onClick = this.OnButtonClick;
        }
        for (int index = 0; index < tipsBtn.Length; ++index)
        {
            if (tipsBtn[index] != null)
                UIEventListener.Get(tipsBtn[index]).onPress = this.OnTipsPress;
        }
    }
    public void OnTipsPress(UnityEngine.GameObject go, bool isPress)
    {
        if (isPress)
        {
            int index = SearchTipsBtn(go);
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            LegacyComplexAttrConifg legacycomplex = LegacyComplexAttrConifgProvider.Instance.GetDataById(index + 1);
            ItemConfig itemCfg = ItemConfigProvider.Instance.GetDataById(legacycomplex.Property);
            if (itemCfg != null)
            {
                //获取附加属性
                List<int> appendAttrList = itemCfg.m_AttachedProperty;
                if (appendAttrList == null)
                    return;
                //默认神器的附加属性只有一个
                int appendId = -1;
                if (appendAttrList.Count > 0)
                    appendId = appendAttrList[0];
                AppendAttributeConfig cfg = AppendAttributeConfigProvider.Instance.GetDataById(appendId);
                if (lblLegacyAttr != null)
                {
                    lblLegacyAttr.text = GetAppendAttr(cfg, index).ToString();
                }
            }
        }
        else
        {
            showAllAtrr();
        }
    }
    public void OnButtonClick(UnityEngine.GameObject go)
    {
        if (go == null)
            return;
        int index = SearchButton(go);
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null && index < role_info.Legacys.Length)
        {
            m_CurrentArtifactId = role_info.Legacys[index].ItemId;
            LogicSystem.EventChannelForGfx.Publish("artifact_click_button", "artifact", m_CurrentArtifactId);
            SetButtonBg(go);
        }
    }
    //设置
    void SetButtonBg(UnityEngine.GameObject go)
    {
        for (int i = 0; i < buttons.Length; ++i)
        {
            UnityEngine.Transform tf = buttons[i].transform.Find("bg");
            if (tf != null)
            {
                if (go == buttons[i])
                {
                    NGUITools.SetActive(tf.gameObject, true);
                }
                else
                {
                    NGUITools.SetActive(tf.gameObject, false);
                }
            }
        }
    }
    int SearchButton(UnityEngine.GameObject go)
    {
        int index = -1;
        for (int i = 0; i < buttons.Length; ++i)
        {
            if (go == buttons[i])
            {
                index = i;
                break;
            }
        }
        return index;
    }
    int SearchTipsBtn(UnityEngine.GameObject go)
    {
        int index = -1;
        for (int i = 0; i < tipsBtn.Length; ++i)
        {
            if (go == tipsBtn[i])
            {
                index = i;
                break;
            }
        }
        return index;
    }
    // 连线显示
    void LineShow()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            NGUITools.SetActive(lines[i].gameObject, false);
        }
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            if (role_info.Legacys[0].IsUnlock && role_info.Legacys[1].IsUnlock)
            {
                NGUITools.SetActive(lines[0].gameObject, true);
            }
            if (role_info.Legacys[1].IsUnlock && role_info.Legacys[2].IsUnlock)
            {
                NGUITools.SetActive(lines[1].gameObject, true);
            }
            if (role_info.Legacys[2].IsUnlock && role_info.Legacys[3].IsUnlock)
            {
                NGUITools.SetActive(lines[2].gameObject, true);
            }
            //todo 加神器
        }
    }
    //显示属性和
    void showAllAtrr()
    {
        int hp = 0;
        int damage = 0;
        int mp = 0;
        int armor = 0;
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        UserInfo userInfo = role_info.GetPlayerSelfInfo();
        for (int i = 0; i < role_info.Legacys.Length; i++)
        {
            ItemConfig itemCfg = ItemConfigProvider.Instance.GetDataById(role_info.Legacys[i].ItemId);
            int itemLevel = role_info.Legacys[i].Level;
            if (role_info.Legacys[i].IsUnlock)
            {
                hp += (int)itemCfg.m_AttrData.GetAddHpMax(0, userInfo.GetLevel(), itemLevel);
                damage += (int)itemCfg.m_AttrData.GetAddAd(0, userInfo.GetLevel(), itemLevel);
                mp += (int)itemCfg.m_AttrData.GetAddMDp(0, userInfo.GetLevel(), itemLevel);
                armor += (int)itemCfg.m_AttrData.GetAddADp(0, userInfo.GetLevel(), itemLevel);
            }
        }
        lblLegacyAttr.text = GetStringDictionaryKey(101) + hp + "\n"
                                + GetStringDictionaryKey(102) + damage + "\n"
                                + GetStringDictionaryKey(103) + mp + "\n"
                                + GetStringDictionaryKey(104) + armor + "\n";
    }
    string GetStringDictionaryKey(int key)
    {
        string chn_desc = "";
        chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(key);
        return chn_desc;
    }
    /// <summary>
    /// 参数index 代表第几个神器、已确定其属性，目前只能写死了！！
    /// <param name="index"> </param>
    //获得附加属性
    public string GetAppendAttr(AppendAttributeConfig cfg, int index)
    {
        if (cfg == null)
            return "";
        float ret =
                 cfg.GetAddHpMax(1.0f, 1) + cfg.GetAddEpMax(1.0f, 1) +
                 cfg.GetAddAd(1.0f, 1) + cfg.GetAddADp(1.0f, 1) +
                 cfg.GetAddMDp(1.0f, 1) + cfg.GetAddCri(1.0f, 1) +
                 cfg.GetAddPow(1.0f, 1) + cfg.GetAddBackHitPow(1.0f, 1) +
                 cfg.GetAddCrackPow(1.0f, 1) + cfg.GetAddFireDam(1.0f, 1) +
                 cfg.GetAddIceDam(1.0f, 1) + cfg.GetAddPoisonDam(1.0f, 1) +
                 cfg.GetAddFireDam(1.0f, 1) + cfg.GetAddIceErd(1.0f, 1) +
                 cfg.GetAddPoisonErd(1.0f, 1) + cfg.GetAddEpRecover1(1.0f, 1)
                 + cfg.GetAddHpRecover1(1f, 1) + cfg.GetAddAd2(1.0f, 1)
                 + cfg.GetAddHpMax2(1f, 1);
        string str = "";
        switch (index)
        {
            case 0:
                str = StrDictionaryProvider.Instance.GetDictString(405);
                str = str + ret;
                break;
            case 1:
                str = StrDictionaryProvider.Instance.GetDictString(406);
                str = str + ret;
                break;
            case 2:
                str = StrDictionaryProvider.Instance.GetDictString(407);
                str = str + (int)(ret * 100) + "%";
                break;
            case 3:
                str = StrDictionaryProvider.Instance.GetDictString(408);
                str = str + (int)(ret * 100) + "%";
                break;
        }
        return str;
    }
    //获取神器信息

}
