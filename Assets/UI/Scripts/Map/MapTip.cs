using ArkCrossEngine;

public class MapTip : UnityEngine.MonoBehaviour
{

    public UILabel lblName = null;
    public UILabel lblDes = null;
    public UILabel lblOpen1 = null;
    public UILabel lblOpen2 = null;
    public UILabel lblLvLimit = null;
    public UIButton btnGo = null;

    private int m_sceneId = -1;
    private bool m_hasOpen = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void UpdateView(int sceneId)
    {
        MainCityConfig config = MainCityConfigProvider.Instance.GetDataById(sceneId);
        if (config != null)
        {
            m_sceneId = sceneId;

            if (lblName != null)
            {
                lblName.text = config.m_Name;
            }
            if (lblDes != null)
            {
                lblDes.text = config.m_Discribe;
            }
            if (lblOpen1 != null)
            {
                lblOpen1.text = config.m_ChapterName1;
            }
            if (lblOpen2 != null)
            {
                lblOpen2.text = config.m_ChapterName2;
            }
            if (lblLvLimit != null)
            {
                lblLvLimit.text = "Lv." + config.m_Level;
            }
            if (btnGo != null)
            {
                RoleInfo role = LobbyClient.Instance.CurrentRole;
                if (role != null)
                {
                    m_hasOpen = false;
                    if (role.Level >= config.m_Level)
                    {//已开放
                        m_hasOpen = true;
                    }
                    lblLvLimit.color = m_hasOpen ? new UnityEngine.Color(0, 251 / 255f, 75 / 255f) : new UnityEngine.Color(1, 0, 0);
                }
            }
        }
    }

    public void OnClickGo()
    {
        if (m_sceneId != -1 && m_hasOpen)
        {
            if (WorldSystem.Instance.GetCurSceneId() == m_sceneId)
            {
                string chn_desc = StrDictionaryProvider.Instance.GetDictString(1304);
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
            }
            else
            {
                LogicSystem.PublishLogicEvent("ge_change_scene", "game", m_sceneId);
            }
        }
        else
        {
            string chn_desc = StrDictionaryProvider.Instance.GetDictString(31);
            LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
        }
    }
}
