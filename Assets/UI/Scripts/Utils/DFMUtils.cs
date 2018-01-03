
public class DFMUtils
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void RestartGame()
    {
        UnityEngine.GameObject go = UnityEngine.GameObject.Find(ArkCrossEngine.GlobalVariables.cGameRootName);
        if (go != null)
        {
            GameLogic gameLogic = go.GetComponent<GameLogic>();
            if (gameLogic != null) gameLogic.RestartLogic();
        }
    }
    static private DFMUtils m_Instance = new DFMUtils();
    static public DFMUtils Instance
    {
        get { return m_Instance; }
    }
    // 
}
