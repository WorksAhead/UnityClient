using UnityEngine;
using ArkCrossEngine;
public enum GetNewThingsType : int
{
    T_Skill = 0, //技能
    T_Function = 1,//新功能
    T_Partner = 2,
}
public class GetNewThings : UnityEngine.MonoBehaviour
{
    public UISprite skillIcon;
    public UISprite partnerIcon;
    public UISprite fuctionIcon;
    public UnityEngine.GameObject fuctionPanel;
    public UILabel fuctionLabel;
    public UILabel label;
    public UnityEngine.GameObject tweencontain;
    public UnityEngine.GameObject tweenEffect;
    private string functionName = "";
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    // 初始化
    public void InitPanel(NewThings newThings)
    {
        tweencontain.transform.GetComponent<TweenTransform>().to = newThings.tf;
        tweenEffect.transform.GetComponent<TweenTransform>().to = newThings.tf;
        tweenEffect.transform.GetComponent<TweenTransform>().from = newThings.tf;
        switch (newThings.type)
        {
            case GetNewThingsType.T_Skill:
                NewSkill(newThings.id);
                NGUITools.SetActive(partnerIcon.gameObject, false);
                NGUITools.SetActive(skillIcon.gameObject, true);
                NGUITools.SetActive(fuctionPanel.gameObject, false);

                break;
            case GetNewThingsType.T_Partner:
                NewPartner(newThings.id);
                NGUITools.SetActive(skillIcon.gameObject, false);
                NGUITools.SetActive(partnerIcon.gameObject, true);
                NGUITools.SetActive(fuctionPanel.gameObject, false);

                break;
            case GetNewThingsType.T_Function:
                NewFunction(newThings.name, newThings.id);
                NGUITools.SetActive(skillIcon.gameObject, false);
                NGUITools.SetActive(partnerIcon.gameObject, false);
                NGUITools.SetActive(fuctionPanel.gameObject, true);

                break;
        }
        Invoke("OnTweenIcon", 1.7f);
    }
    public void OnTweenCenterFinished()
    {
        if (functionName != "")
            LogicSystem.EventChannelForGfx.Publish("ge_trigger_newbie_guide", "ui");
        Destroy(this.gameObject);
    }
    void OnTweenIcon()
    {
        LogicSystem.EventChannelForGfx.Publish("get_new_function_effect", "ui_effect", functionName);
    }
    //技能
    void NewSkill(int skillId)
    {
        SkillLogicData skillCfg = SkillConfigProvider.Instance.ExtractData(SkillConfigType.SCT_SKILL, skillId) as SkillLogicData;
        //m_SkillInfo = info;
        if (null != skillCfg)
        {
            SetIcon(skillCfg.ShowIconName, skillIcon);
            label.text = StrTools(751, skillCfg.ShowName);
        }
    }
    //伙伴
    void NewPartner(int linkId)
    {
        LogicSystem.EventChannelForGfx.Publish("ge_refresh_partner", "ui");
        Data_NpcConfig npcCfg = NpcConfigProvider.Instance.GetNpcConfigById(linkId);
        if (npcCfg != null)
        {
            SetIcon(npcCfg.m_Portrait, partnerIcon);
            label.text = label.text = StrTools(750, npcCfg.m_Name);
        }
    }
    string StrTools(int id, string insert_name = "")
    {
        string chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(id);
        if (insert_name != "")
        {
            chn_desc = string.Format(chn_desc, insert_name);
        }
        return chn_desc;
    }
    //新功能
    void NewFunction(string btn, int id)
    {
        LevelLock info = LevelLockProvider.Instance.GetDataById(id);
        functionName = btn;
        string labelStr = info.m_Note;
        string picName = "";
        label.text = StrTools(752, info.m_Note);
        switch (btn)
        {
            case "Entrance-Pve":
                picName = "fb";
                break;
            case "Entrance-Mail":
                picName = "youjian";
                break;
            case "Entrance-Trial":
                picName = "huodong";
                break;
            case "Entrance-Match":
                picName = "tz";
                break;
            case "Entrance-Friend":
                picName = "friends";
                break;
            case "Entrance-Equipment":
                picName = "zhuangbei";
                break;
            case "Entrance-Skill":
                picName = "jneng";
                break;
            case "Entrance-Partner":
                picName = "chuzhan";
                break;
            case "Entrance-XHun":
                picName = "Xhun";
                break;
            case "Entrance-GodEquip":
                picName = "shenqi";
                break;
        }
        SetIcon(picName, fuctionIcon);
        fuctionLabel.text = labelStr;
    }
    void SetIcon(string name, UISprite icon)
    {
        if (name == null)
            return;
        if (icon != null)
        {
            icon.spriteName = name;
        }
        else
        {
            Debug.LogError("!! Icon did not init.");
        }
    }
}
