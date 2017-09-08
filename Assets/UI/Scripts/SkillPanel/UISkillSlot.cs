using UnityEngine;
using System.Collections;
using ArkCrossEngine;
using System;
using System.Collections.Generic;
public enum SlotType
{
    SkillSetting,
    SkillStorage,
}
public class UISkillSlot : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject canUpgradeTip = null;
    public UnityEngine.GameObject upgradeEffect = null;
    public UnityEngine.GameObject upEffGO = null;
    public UILabel lblName = null;
    public UILabel lblSection = null;
    public UILabel lblUnLock = null;
    public UISprite icon = null;
    public UISprite circleSp = null;
    //锁标志
    public UISprite lockSp = null;
    public UISprite[] sectionHintSpArr = new UISprite[4];//阶数标志
    public PresetInfo m_EquipedPos = null;
    public SlotType slotType = SlotType.SkillStorage;
    public SlotPosition slotIndex = SlotPosition.SP_None;

    public bool m_IsUnlock = false;
    //
    private int m_SkillId = -1;
    private int m_skillLv = -1;
    private bool m_IsHighlight = false;
    private const int c_SectionNum = 4;
    private const string ASHHINT = "skilllevel";
    private const string LIGHTHINT = "skilllevel2";
    //可解锁提示//
    private UnityEngine.Transform trans = null;
    private const string C_RunTimeGoName = "Pointing";
    private bool m_HintHavenAdded = false;
    private float duration = 1.0f;
    /*事件list*/
    private List<object> eventList = new List<object>();
    private UnityEngine.GameObject effectClone = null; //clone特效
    void Awake()
    {
        try
        {
            trans = this.transform;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Use this for initialization
    void Start()
    {
        try
        {
            AddSubscribe();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    /*添加监听 subscribe*/
    void AddSubscribe()
    {
        object eo;
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("skill_unlock_effect", "effect", ClearEffect);
        if (null != eo)
        {
            eventList.Add(eo);
        }
        object obj = LogicSystem.EventChannelForGfx.Subscribe<int, bool>("ge_skillslot_state_change", "skill", OnStateChange);
        if (obj != null)
        {
            eventList.Add(obj);
        }
        eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (null != eo)
        {
            eventList.Add(eo);
        }

    }
    public void UnSubscribe()
    {
        try
        {
            if (null != eventList)
            {
                for (int i = 0; i < eventList.Count; i++)
                {
                    if (eventList[i] != null)
                    {
                        ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eventList[i]);
                    }
                }
                /*
	      foreach (object eo in eventList) {
	        if (eo != null) {
	          ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eo);
	        }
	      }*/
            }
        }
        catch (Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void OnStateChange(int _skillId, bool _show)
    {
        if (_skillId == SkillId)
        {
            if (canUpgradeTip != null)
            {
                NGUITools.SetActive(canUpgradeTip, _show);
            }
        }
    }

    void ClearEffect()
    {
        if (effectClone != null)
        {
            Destroy(effectClone);
            Unlock(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            if (!m_HintHavenAdded && JudgeWhetherSkillCanUnlock())
            {
                AddAnyActionHint();
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OnClick()
    {
        UISkillPanel skillPanel = NGUITools.FindInParents<UISkillPanel>(this.gameObject);
        if (skillPanel == null) return;
        if (slotType == SlotType.SkillSetting)
        {
            if (m_IsHighlight)
            {
                skillPanel.SendMsg(SkillId, slotIndex);
            }
            skillPanel.SetActionButtonState(true);
            skillPanel.SetEquipFlag(false);
        }
        else
        {
            skillPanel.SetActionButtonState(m_IsUnlock);
            skillPanel.SetEquipFlag(m_IsUnlock);
            UISkillGuide.Instance.OnUnlockSlotClick(SkillId);
        }
        if (circleSp != null) NGUITools.SetActive(circleSp.gameObject, true);
        skillPanel.SetSkillInfo(SkillId);
        skillPanel.ShowSlotHighlight(slotType);
    }
    public void InitSlot(SkillInfo info)
    {
        SkillLogicData skillCfg = SkillConfigProvider.Instance.ExtractData(SkillConfigType.SCT_SKILL, info.SkillId) as SkillLogicData;
        //m_SkillInfo = info;
        if (null != skillCfg)
        {
            SetIconAtlas(info.SkillId);
            SetIcon(skillCfg.ShowIconName);
            SetName(skillCfg.ShowName);
            SetSkillLevel(info.SkillLevel);
        }
        SkillId = info.SkillId;
        EquipedPos = info.Postions;
        Unlock(info.SkillLevel > 0);

    }
    public void SetIconAtlas(int skillId)
    {
        SkillLogicData skillCfg = SkillConfigProvider.Instance.ExtractData(SkillConfigType.SCT_SKILL, skillId) as SkillLogicData;
        //m_SkillInfo = info;
        if (null != skillCfg)
        {
            UnityEngine.GameObject goAtlas = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.GetSharedResource(skillCfg.ShowAtlasPath));
            if (goAtlas != null)
            {
                UIAtlas atlas = goAtlas.GetComponent<UIAtlas>();
                if (atlas != null && icon != null)
                {
                    icon.atlas = atlas;
                }
            }
            else
            {
                Debug.LogError("!!!Load atlas failed.");
            }
        }
    }
    public void SetIcon(string name)
    {
        if (name == null) return;
        if (icon != null)
        {
            icon.spriteName = name;
            UIButton button = this.GetComponent<UIButton>();
            if (button != null) button.normalSprite = name;
        }
        else
        {
            Debug.LogError("!! Icon did not init.");
        }
    }

    //设置等级
    public void SetSkillLevel(int level)
    {
        //在没解锁时会隐藏掉、所以这里要重新设置为可见
        ShowSection(true);
        if (lblSection != null)
        {
            lblSection.text = "Lv." + level.ToString();
        }
        for (int index = 0; index < c_SectionNum; ++index)
        {
            if (index < level && index < sectionHintSpArr.Length && sectionHintSpArr[index] != null)
            {
                sectionHintSpArr[index].spriteName = LIGHTHINT;
            }
            else
            {
                if (index < sectionHintSpArr.Length && sectionHintSpArr[index] != null)
                    sectionHintSpArr[index].spriteName = ASHHINT;
            }
        }
    }
    //设置技能名
    public void SetName(string name)
    {
        if (lblName != null)
        {
            lblName.text = name;
        }
        else
        {
            //Debug.Log("!!NameLabel did not init.");
        }
    }
    //播放解锁特效
    public void PlayUnlockEffect()
    {
        if (effectClone == null)
        {
            effectClone = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/DefenseEffect/SkillUnlockEffect"));
            effectClone = NGUITools.AddChild(gameObject, effectClone);
            effectClone.transform.localPosition = transform.Find("unlock").localPosition;
            ;
        }
    }
    public void Unlock(bool unLock)
    {
        m_IsUnlock = unLock;
        if (lockSp == null)
        {
            Debug.LogError("!!Did not init sprite:lockSprite.");
            return;
        }
        if (unLock)
        {
            DelAnyActionHint();
            if (lblSection != null) NGUITools.SetActive(lblSection.gameObject, true);
            NGUITools.SetActive(lockSp.gameObject, false);
            if (lblSection != null)
            {
                //SkillLogicData skillCfg = SkillConfigProvider.Instance.ExtractData(SkillConfigType.SCT_SKILL, SkillId) as SkillLogicData;
                SkillInfo skill_info = GetSkillInfoById(SkillId);
                SetSkillLevel(skill_info.SkillLevel);
            }
            //lockSp.spriteName = "UnLock";
        }
        else
        {
            NGUITools.SetActive(lockSp.gameObject, true);
            if (lblUnLock != null)
            {
                string CHN = StrDictionaryProvider.Instance.GetDictString(360);
                SkillLogicData skillCfg = SkillConfigProvider.Instance.ExtractData(SkillConfigType.SCT_SKILL, SkillId) as SkillLogicData;
                if (skillCfg != null) lblUnLock.text = skillCfg.ActivateLevel + CHN;
            }
            if (lblSection != null) NGUITools.SetActive(lblSection.gameObject, false);
            ShowSection(false);
        }
    }
    public void ShowSection(bool visible)
    {
        for (int index = 0; index < sectionHintSpArr.Length; ++index)
            if (sectionHintSpArr[index] != null)
                NGUITools.SetActive(sectionHintSpArr[index].gameObject, visible);
    }
    //设置可装备标志
    public void SetEquipFlag(bool equip)
    {
        //
        m_IsHighlight = equip;
    }

    //设置高亮
    public void SetHighlight(bool visible)
    {
        if (circleSp != null)
        {
            NGUITools.SetActive(circleSp.gameObject, visible);
        }
    }
    public void SetSlotIconById(int skillId)
    {
        SkillLogicData skillCfg = SkillConfigProvider.Instance.ExtractData(SkillConfigType.SCT_SKILL, skillId) as SkillLogicData;
        if (null != skillCfg)
        {
            SetIcon(skillCfg.ShowIconName);
            SetName(skillCfg.ShowName);
            SkillInfo skill_info = GetSkillInfoById(skillId);
            SetSkillLevel(skill_info.SkillLevel);
        }
        else
        {
            SetIcon("");
            //SetName("可装备");
        }
    }
    public void UpgradeSkill(int skillId, bool isUpgrade = false)
    {
        SkillLogicData skillCfg = SkillConfigProvider.Instance.ExtractData(SkillConfigType.SCT_SKILL, skillId) as SkillLogicData;
        //m_SkillInfo = info;
        if (null != skillCfg)
        {
            SetIcon(skillCfg.ShowIconName);
            SetName(skillCfg.ShowName);
            SkillInfo skill_info = GetSkillInfoById(skillId);
            SetSkillLevel(skill_info.SkillLevel);
            //播放特效
            if (isUpgrade == true && upgradeEffect != null)
            {
                UnityEngine.GameObject ef = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.NewObject(upgradeEffect));
                if (ef != null && upEffGO != null)
                {
                    ef.transform.position = upEffGO.transform.position;
                    Destroy(ef, duration);
                }
            }
        }
        SkillId = skillId;
    }
    //判断技能是否可解锁
    private bool JudgeWhetherSkillCanUnlock()
    {
        if (!m_IsUnlock && slotType != SlotType.SkillSetting)
        {
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            SkillInfo skill_info = GetSkillInfoById(SkillId);
            if (role_info != null && skill_info != null && skill_info.ConfigData != null)
            {
                if (role_info.Level >= skill_info.ConfigData.ActivateLevel)
                    return true;
            }
        }
        return false;
    }
    //添加可操作提示
    private void AddAnyActionHint()
    {
        if (!m_HintHavenAdded)
        {
            string path = UIManager.Instance.GetPathByName("Pointing");
            UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ResourceSystem.GetSharedResource(path));
            if (null != go)
            {
                go = NGUITools.AddChild(this.gameObject, go);
                UISprite sp = this.GetComponent<UISprite>();
                if (sp != null)
                    go.transform.localPosition = new UnityEngine.Vector3(sp.width / 3f, sp.height / 3f, 0);
                go.name = C_RunTimeGoName;
            }
            m_HintHavenAdded = true;
        }
    }
    //删除操作提示
    private void DelAnyActionHint()
    {
        if (trans != null)
        {
            UnityEngine.Transform child = trans.Find(C_RunTimeGoName);
            if (child != null)
            {
                NGUITools.Destroy(child.gameObject);
            }
        }
    }
    public int SkillId
    {
        get
        {
            return m_SkillId;
        }
        set
        {
            m_SkillId = value;
        }
    }

    public PresetInfo EquipedPos
    {
        get
        {
            return m_EquipedPos;
        }
        set
        {
            m_EquipedPos = value;
        }
    }
    private SkillInfo GetSkillInfoById(int skillId)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            for (int i = 0; i < role_info.SkillInfos.Count; ++i)
            {
                if (role_info.SkillInfos[i] != null && role_info.SkillInfos[i].SkillId == skillId)
                {
                    return role_info.SkillInfos[i];
                }
            }
        }
        return null;
    }
}
