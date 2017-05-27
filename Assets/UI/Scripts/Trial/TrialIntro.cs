using UnityEngine;
using System.Collections;

public class TrialIntro : UnityEngine.MonoBehaviour
{

    public UnityEngine.GameObject trialJinBi = null;
    public UnityEngine.GameObject trialShiLian = null;

    private TrialIntroType m_type = TrialIntroType.None;

    void Awake()
    {
        try
        {
            if (trialShiLian != null)
            {
                TrialShiLian shilian = trialShiLian.GetComponent<TrialShiLian>();
                if (shilian != null)
                {
                    shilian.InitNotification();
                }
                TrialJinBi jinbi = trialJinBi.GetComponent<TrialJinBi>();
                if (jinbi != null)
                {
                    jinbi.InitNotification();
                }
            }
            //UIManager.Instance.HideWindowByName("TrialIntro");
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        try
        {
            //CYGTConnector.ShowCYGTSDK();
            if (trialJinBi == null || trialShiLian == null)
                return;
            NGUITools.SetActive(trialJinBi, false);
            NGUITools.SetActive(trialShiLian, false);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void SetIntroType(TrialIntroType type)
    {
        if (trialJinBi == null || trialShiLian == null || type == TrialIntroType.None)
            return;

        m_type = type;

        switch (m_type)
        {
            case TrialIntroType.JinBi:
                NGUITools.SetActive(trialJinBi, true);
                break;
            case TrialIntroType.ShiLian:
                NGUITools.SetActive(trialShiLian, true);
                break;
        }
    }

    public void OnClickClose()
    {
        UIManager.Instance.HideWindowByName("TrialIntro");
    }
}
