using ArkCrossEngine;

public class UIItemSourceItem : UnityEngine.MonoBehaviour
{
    public UILabel lblSceneType;
    public UILabel lblSceneChapter;
    public UILabel lblSceneName;
    public UISprite spSceneType;
    private int m_SceneId = -1;
    private const string MasterIcon = "jing-ying-guan-qia-biao-xiao";
    private const string CommonIcon = "guan-jia-biao-xiao(1)";
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnClick()
    {
        //UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("SceneSelect");
        //if (go != null) {
        //  UISceneSelect sceneSelect = go.GetComponent<UISceneSelect>();
        //  if (sceneSelect != null) sceneSelect.StartChapter(m_SceneId);
        //}
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        if (role != null)
        {
            if (role.SceneInfo.ContainsKey(m_SceneId))
            {
                UIManager.Instance.HideWindowByName("ItemSourceTips");
                UIManager.Instance.HideWindowByName("ArtifactPanel");
                UnityEngine.GameObject goc = UIManager.Instance.GetWindowGoByName("SceneSelect");
                if (goc != null)
                {
                    LogicSystem.SendStoryMessage("cityplayermove", 0);//寻路
                    UISceneSelect uss = goc.GetComponent<UISceneSelect>();
                    if (uss != null)
                    {
                        uss.startChapterId = m_SceneId;
                    }
                }
            }
            else
            {
                SendScreeTipCenter(40);
            }
        }

    }
    //悬浮字中
    void SendScreeTipCenter(int id)
    {
        string chn_desc = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(id);
        ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", chn_desc, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
    }
    public void SetSourceInfo(int sceneId)
    {
        m_SceneId = sceneId;
        Data_SceneConfig sceneCfg = SceneConfigProvider.Instance.GetSceneConfigById(sceneId);
        if (sceneCfg == null)
            return;
        if (lblSceneType != null)
        {
            if (sceneCfg.m_SubType == (int)SceneSubTypeEnum.TYPE_ELITE)
            {
                string chn_des = StrDictionaryProvider.Instance.GetDictString(158);
                lblSceneType.text = chn_des;
                if (spSceneType != null) spSceneType.spriteName = MasterIcon;
            }
            else
            {
                string chn_des = StrDictionaryProvider.Instance.GetDictString(159);
                lblSceneType.text = chn_des;
                if (spSceneType != null) spSceneType.spriteName = CommonIcon;
            }
        }
        if (lblSceneName != null)
        {
            lblSceneName.text = sceneCfg.m_SceneName;
        }
    }
}
