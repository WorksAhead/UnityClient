using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class UISummonPartner : UnityEngine.MonoBehaviour
{
    private int unlockLevel = 1;
    private int m_TriggerSceneId = -1;
    public UIProgressBar barHp;
    public UnityEngine.GameObject goParternContainer;
    public UnityEngine.GameObject goPartnerSummon;
    public UnityEngine.GameObject goPartnerSummonHpBar;
    public UISprite spCountDown;
    public UISprite spPartnerPortrait = null;
    public UISprite spPortraitForHp;//伙伴血条时的头像
    public UISprite spRankColor;//伙伴等级颜色
    private bool m_IsStartCd = false;
    private float m_StartSummonTime = 0;
    private float m_LiveDuration = 0f;
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
            ArkCrossEngine.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Awake()
    {
        try
        {
            if (goPartnerSummonHpBar != null) NGUITools.SetActive(goPartnerSummonHpBar, false);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    // Use this for initialization
    void Start()
    {
        try
        {
            object obj = LogicSystem.EventChannelForGfx.Subscribe<bool>("ge_partner_summon_result", "ui", HandlerPartnerSummonResult);
            if (obj != null) m_EventList.Add(obj);
            obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            if (obj != null) m_EventList.Add(obj);
            InitPartnerSummon();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (m_IsStartCd)
            {
                UpdatePartnerCdAndHpBar();
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void InitPartnerSummon()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null && role_info.PartnerStateInfo != null)
        {
            PartnerInfo info = role_info.PartnerStateInfo.GetActivePartner();
            if (info != null)
            {
                PartnerLevelUpConfig levelUpCfg = PartnerLevelUpConfigProvider.Instance.GetDataById(info.CurAdditionLevel);
                if (levelUpCfg != null) ResetSlotColor(levelUpCfg.PartnerRankColor);
                Data_NpcConfig npcCfg = NpcConfigProvider.Instance.GetNpcConfigById(info.LinkId);
                if (npcCfg != null && spPartnerPortrait != null)
                {
                    spPartnerPortrait.spriteName = npcCfg.m_Portrait;
                    if (spPortraitForHp != null) spPortraitForHp.spriteName = npcCfg.m_Portrait;
                    UIButton btnComp = spPartnerPortrait.GetComponent<UIButton>();
                    if (btnComp != null) btnComp.normalSprite = npcCfg.m_Portrait;
                }
            }
            else
            {
                //没有出战伙伴，隐藏召唤按钮
                if (goParternContainer != null) NGUITools.SetActive(goParternContainer, false);
            }
        }
        if (UIDataCache.Instance.IsPvPScene() || UIDataCache.Instance.IsArenaPvPScene())
        {
            //PVP需要隐藏出战按钮
            if (goParternContainer != null) NGUITools.SetActive(goParternContainer, false);
        }
    }
    //召唤队友
    public void OnSummonPartnerClick()
    {
        if (!m_IsStartCd)
        {
            LogicSystem.PublishLogicEvent("ge_summon_partner", "partner");
        }
        else
        {
            string CHN = StrDictionaryProvider.Instance.GetDictString(704);
            LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", CHN, UIScreenTipPosEnum.AlignBottom, UnityEngine.Vector3.zero);
        }
    }
    private void GetStartTime()
    {

    }
    /// <summary>
    /// 先更新伙伴血量、伙伴血量为0时开始走伙伴CD
    /// </summary>
    private void UpdatePartnerCdAndHpBar()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;

        if (null != role_info && null != role_info.PartnerStateInfo)
        {
            PartnerInfo partner_info = role_info.PartnerStateInfo.GetActivePartner();
            if (partner_info != null)
            {
                long cd = partner_info.CoolDown;
                float partnerHpPercent = role_info.PartnerStateInfo.ActivePartnerHpPercent;
                if (barHp != null) barHp.value = partnerHpPercent;
                if (partnerHpPercent <= 0f)
                {
                    //走CD
                    ShowPartnerPortrait(true);
                    if (m_LiveDuration == 0f)
                        m_LiveDuration = UnityEngine.Time.time * 1000 - m_StartSummonTime * 1000;
                    long deltaTime = (long)(UnityEngine.Time.time * 1000 - m_StartSummonTime * 1000);
                    if (cd <= m_LiveDuration)
                    {
                        if (spCountDown != null) spCountDown.fillAmount = 0f;
                        m_IsStartCd = false;
                    }
                    else
                    {
                        if (spCountDown != null) spCountDown.fillAmount = 1 - (deltaTime - m_LiveDuration) / (float)(cd - m_LiveDuration);
                        if (spCountDown != null && spCountDown.fillAmount <= 0f)
                        {
                            m_IsStartCd = false;
                        }
                    }
                }
            }
        }
    }

    private void HandlerPartnerSummonResult(bool successed)
    {
        if (successed)
        {
            ShowPartnerPortrait(false);
            m_LiveDuration = 0f;
            m_IsStartCd = true;
            m_StartSummonTime = UnityEngine.Time.time;
            if (spCountDown != null) spCountDown.fillAmount = 0f;
        }
        else
        {
            string CHN = StrDictionaryProvider.Instance.GetDictString(704);
            LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", CHN, UIScreenTipPosEnum.AlignBottom, UnityEngine.Vector3.zero);
        }
    }
    //判断是否完成改关
    private bool IsSceneFinished(int sceneId)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            return role_info.SceneInfo.ContainsKey(sceneId);
        }
        return false;
    }
    private void ShowPartnerPortrait(bool active)
    {
        if (goPartnerSummon != null) NGUITools.SetActive(goPartnerSummon, active);
        if (goPartnerSummonHpBar != null) NGUITools.SetActive(goPartnerSummonHpBar, !active);
    }
    //重置伙伴边框颜色
    private void ResetSlotColor(string rankColor = "SEquipFrame1")
    {
        if (spRankColor != null)
        {
            spRankColor.spriteName = rankColor;
            if (goPartnerSummon == null) return;
            UIButton btn = goPartnerSummon.GetComponent<UIButton>();
            if (btn != null) btn.normalSprite = rankColor;
        }
    }
}
