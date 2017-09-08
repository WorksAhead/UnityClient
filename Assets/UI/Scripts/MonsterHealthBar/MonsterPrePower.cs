public class MonsterPrePower : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        try
        {
            progress = this.GetComponent<UIProgressBar>();
            progress.value = 0;
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            UpdatePos();
            if (Duration > 0)
            {
                if (progress != null)
                    progress.value += RealTime.deltaTime / Duration;

                if (progress.value >= 1)
                {
                    {
                        NGUITools.SetActive(this.gameObject, false);
                        Destroy(this.gameObject);
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void UpdatePos()
    {
        UnityEngine.Vector3 pos = Position;
        if (UnityEngine.Camera.main != null)
            pos = UnityEngine.Camera.main.WorldToScreenPoint(pos);
        pos.z = 0;
        UnityEngine.Vector3 nguiPos = UnityEngine.Vector3.zero;
        if (UICamera.mainCamera != null)
        {
            nguiPos = UICamera.mainCamera.ScreenToWorldPoint(pos);
        }
        if (this.transform != null)
        {
            this.transform.position = nguiPos;
        }
    }


    public float Duration
    {
        get
        {
            return m_Duration;
        }
        set
        {
            m_Duration = value;
        }
    }

    public int PowerId
    {
        get { return m_PowerId; }
        set
        {
            m_PowerId = value;
        }
    }
    public UnityEngine.Vector3 Position
    {
        get { return m_Pos; }
        set
        {
            m_Pos = value;
        }
    }

    private UIProgressBar progress = null;
    private float m_Duration = 1f;
    private int m_PowerId = -1;
    private UnityEngine.Vector3 m_Pos = new UnityEngine.Vector3();

}
