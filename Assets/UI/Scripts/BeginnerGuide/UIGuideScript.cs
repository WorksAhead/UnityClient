public class UIGuideScript : UnityEngine.MonoBehaviour
{

    public UINewbieGuideTriggerType m_TriggerType = UINewbieGuideTriggerType.T_None;
    private UnityEngine.GameObject m_GuideHand = null;
    private UIEventListener m_Listner;
    private int m_CurrentGuideId;
    private int m_FinishedId;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnEnable()
    {
        try
        {
            if (UIBeginnerGuideManager.Instance.IsBeginnerGuiderStarted && m_TriggerType != UINewbieGuideTriggerType.T_MainCity && m_TriggerType != UINewbieGuideTriggerType.T_None)
                UIBeginnerGuideManager.Instance.TriggerNewbieGuide(m_TriggerType, gameObject);
            if (m_TriggerType == UINewbieGuideTriggerType.T_PartnerSummon)
            {
                ///在副本中，UIBeiginnerGuideManager.Instance.IsBeginnerGuiderStarted==false.
                ///新手引导需要单独触发
                UIBeginnerGuideManager.Instance.TriggerNewbieGuide(m_TriggerType, gameObject);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    void OnDisable()
    {
        try
        {
            if (UIBeginnerGuideManager.Instance.IsBeginnerGuiderStarted && m_CurrentGuideId != -1)
            {
                UIBeginnerGuideManager.Instance.UnFinishNewbieGuide(m_CurrentGuideId);
                Clear();
            }
            if (m_TriggerType == UINewbieGuideTriggerType.T_PartnerSummon && m_CurrentGuideId != -1)
            {
                ///在副本中，UIBeiginnerGuideManager.Instance.IsBeginnerGuiderStarted==false一直为False
                ///新手引导需要单独触发
                UIBeginnerGuideManager.Instance.UnFinishNewbieGuide(m_CurrentGuideId);
                Clear();
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void StoreGuideInfo(UnityEngine.GameObject obj, UIEventListener listner, int currentGuideId)
    {
        Clear();
        m_GuideHand = obj;
        m_Listner = listner;
        m_CurrentGuideId = currentGuideId;
        m_FinishedId = m_CurrentGuideId;
    }
    public int GetCurrentGuideId()
    {
        if (NGUITools.GetActive(gameObject))
            return m_CurrentGuideId;
        else
        {
            return m_FinishedId;
        }
    }
    public void Clear()
    {
        if (m_GuideHand != null)
            Destroy(m_GuideHand);
        if (m_Listner != null)
            Destroy(m_Listner);
        m_CurrentGuideId = -1;
    }
}
