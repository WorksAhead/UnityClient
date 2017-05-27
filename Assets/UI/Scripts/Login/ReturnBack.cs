public class ReturnBack : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPress()
    {
        /*ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_change_scene", "game", m_SceneId);*/
        NGUITools.DestroyImmediate(this.gameObject);
    }

    public void BackToBegin()
    {
        /*ArkCrossEngine.LogicSystem.PublishLogicEvent("ge_change_scene", "game", m_SceneId);*/
    }
    public int m_SceneId = 6;
}
