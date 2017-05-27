using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ArkCrossEngine;

public class UIPartnerJoin : UnityEngine.MonoBehaviour
{

    public UISprite spPortrait;
    public UILabel lblPartnerName;
    // Use this for initialization
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
            /*
            foreach (object eo in m_EventList) {
              if (eo != null) {
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eo);
              }
            }*/
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
            //     object obj = LogicSystem.EventChannelForGfx.Subscribe<int>("ge_add_partner", "ui", HandlerAddPartner);
            //     if (obj != null) m_EventList.Add(obj);
            //     obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
            //     if (obj != null) m_EventList.Add(obj);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void HandlerAddPartner(int partnerId)
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (null != role_info && role_info.PartnerStateInfo != null)
        {
            List<PartnerInfo> partners = role_info.PartnerStateInfo.GetAllPartners();
            if (null == partners) return;
            for (int index = 0; index < partners.Count; ++index)
            {
                if (partners[index] != null && partners[index].Id == partnerId)
                {
                    //找到添加伙伴信息
                    PartnerInfo info = partners[index];
                    Data_NpcConfig npcCfg = NpcConfigProvider.Instance.GetNpcConfigById(info.LinkId);
                    if (npcCfg != null)
                    {
                        if (lblPartnerName != null) lblPartnerName.text = npcCfg.m_Name;
                        if (spPortrait != null) spPortrait.spriteName = npcCfg.m_Portrait;
                    }
                    UIManager.Instance.ShowWindowByName("PartnerJoin");
                    break;
                }
            }
        }
    }
    //点击确定按钮
    public void OnConfirClick()
    {
        UIManager.Instance.HideWindowByName("PartnerJoin");
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("Partner");
        if (null == go) return;
        UIPartnerPanel script = go.GetComponent<UIPartnerPanel>();
        if (script != null) script.RefreshParnterList();
    }
}
