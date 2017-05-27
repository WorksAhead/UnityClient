using ArkCrossEngine;

public class StageClear : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        try
        {
            m_Particle = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("Hero_FX/1_Swordman/ui_FX_01"));

            UnityEngine.Transform trans = this.transform.FindChild("stage");
            if (trans == null)
                return;
            UnityEngine.GameObject go = trans.gameObject;
            if (null != go)
            {
                UnityEngine.Vector3 pos = go.transform.position * 0.3f;
                PlayParticle("stage", pos);
                pos = go.transform.position * 1.3f;
                PlayParticle("stage", pos);
            }
            trans = this.transform.FindChild("clear");
            if (trans == null)
                return;
            go = trans.gameObject;
            if (null != go)
            {
                UnityEngine.Vector3 pos = go.transform.position * 0.3f;
                PlayParticle("clear", pos);
                pos = go.transform.position * 1.3f;
                PlayParticle("clear", pos);
            }
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
            if (m_IsStageFinished && m_IsClearFinished)
            {
                timeDelta -= RealTime.deltaTime;
                if (timeDelta <= 0)
                {

                    StartTween();
                    m_IsStageFinished = false;
                    m_IsClearFinished = false;
                    timeDelta = 0.5f;
                }
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void StartTween()
    {
        UnityEngine.Transform trans = this.transform.FindChild("stage");
        if (trans == null)
            return;
        UnityEngine.GameObject go = trans.gameObject;
        if (null == go)
            return;
        StartTweenPos(go);
        TweenAlpha.Begin(go, 0.2f, 0f);

        trans = this.transform.FindChild("clear");
        if (null == trans)
            return;
        go = trans.gameObject;
        if (null == go)
            return;
        StartTweenPos(go);
        TweenAlpha.Begin(go, 0.2f, 0f);
    }
    private void StartTweenPos(UnityEngine.GameObject go)
    {
        if (null == go)
            return;
        TweenPosition tweenPos = go.GetComponent<TweenPosition>();
        if (tweenPos != null)
            Destroy(tweenPos);
        tweenPos = go.AddComponent<TweenPosition>();
        tweenPos.from = go.transform.localPosition;
        tweenPos.to = new UnityEngine.Vector3(0, 0, 0);
        tweenPos.duration = 0.2f;
    }
    public void OnStageAnimFinished()
    {
        UnityEngine.Transform trans = this.transform.FindChild("stage");
        if (trans == null)
            return;
        UnityEngine.GameObject go = trans.gameObject;
        if (null == go)
            return;
        UnityEngine.Vector3 pos = go.transform.position;
        PlayParticle("stage", pos);
        UISprite sp = go.GetComponent<UISprite>();
        if (null == sp)
            return;
        sp.spriteName = m_ClearType;
        m_IsStageFinished = true;
    }
    public void OnClearAnimFinished()
    {
        UnityEngine.Transform trans = this.transform.FindChild("clear");
        if (null == trans)
            return;
        UnityEngine.GameObject go = trans.gameObject;
        if (null == go)
            return;
        UnityEngine.Vector3 pos = go.transform.position;
        PlayParticle("clear", pos);
        UISprite sp = go.GetComponent<UISprite>();
        if (null == sp)
            return;
        sp.spriteName = "clear";
        m_IsClearFinished = true;
    }
    private void PlayParticle(string father, UnityEngine.Vector3 nguiPos)
    {
        UnityEngine.Transform trans = this.transform.FindChild(father);
        if (null == trans)
            return;
        UnityEngine.GameObject fatherGo = trans.gameObject;
        if (null == fatherGo)
            return;

        if (null != m_Particle)
        {
            UnityEngine.Vector3 curPos = UICamera.mainCamera.WorldToScreenPoint(nguiPos);
            curPos = UnityEngine.Camera.main.ScreenToWorldPoint(new UnityEngine.Vector3(curPos.x, curPos.y, 5));
            UnityEngine.GameObject effect = UnityEngine.GameObject.Instantiate(m_Particle, curPos, UnityEngine.Quaternion.identity) as UnityEngine.GameObject;
            if (null != effect)
                effect.transform.position = curPos;
            Destroy(effect, duration);

        }

    }
    public void SetClearType(string type)
    {
        m_ClearType = type.ToLower();
    }
    private string m_ClearType = "stage";
    private UnityEngine.GameObject m_Particle = null;
    private bool m_IsStageFinished = false;
    private bool m_IsClearFinished = false;
    public float duration = 1.0f;
    public float timeDelta = 0.5f;
}
