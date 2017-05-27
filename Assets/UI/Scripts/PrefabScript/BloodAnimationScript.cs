using UnityEngine;

public class BloodAnimationScript : UnityEngine.MonoBehaviour
{
    private bool bActive = false;
    private ActiveAnimation m_Anim = null;
    private UnityEngine.Vector3 outPos = new UnityEngine.Vector3(-1000.0f, -1000.0f, -1000.0f);
    private float fTimeRecycle = 0.0f;

    public UILabel m_Label = null;
    public UnityEngine.Transform m_Transform = null;

    // Use this for initialization
    void Awake()
    {
        InitState();
    }

    void Update()
    {
        if (fTimeRecycle > 0.0f)
        {
            float time = Time.time;
            if (fTimeRecycle <= time)
            {
                fTimeRecycle = 0.0f;
                AnimaionFinish();
            }
        }
    }

    public void AnimaionFinish()
    {
        if (m_Anim != null)
        {
            m_Anim.StopAllCoroutines();
            EventDelegate.Remove(m_Anim.onFinished, AnimaionFinish);
        }
        InitState();
    }

    public void InitState()
    {
        bActive = false;
        //fTimeRecycle = 0.0f;
        m_Transform.localPosition = outPos;
    }

    public void SetTimeRecycle(float timeRecycle = 0.0f)
    {
        if (timeRecycle <= 0.0f)
        {
            timeRecycle = 1.0f;
        }
        fTimeRecycle = timeRecycle + Time.time;
    }

    public void PlayAnimation()
    {
        if (m_Transform != null)
        {
            if (fTimeRecycle <= 0.0f)
            {
                SetTimeRecycle();
            }
            UnityEngine.Transform tf = m_Transform.Find("Label");
            if (tf != null)
            {
                m_Anim = null;
                UITweener[] t = tf.GetComponentsInChildren<UITweener>();
                if (t.Length > 0)
                {
                    bActive = true;
                    for (int i = 0; i < t.Length; ++i)
                    {
                        t[i].ResetToBeginning();
                        t[i].PlayForward();
                    }
                }
                else if (t.Length == 0)
                {
                    Animation a = tf.GetComponent<Animation>();
                    if (a != null)
                    {
                        bActive = true;
                        m_Anim = ActiveAnimation.Play(a, a.clip.name, AnimationOrTween.Direction.Forward,
                                             AnimationOrTween.EnableCondition.EnableThenPlay, AnimationOrTween.DisableCondition.DisableAfterForward);
                        EventDelegate.Add(m_Anim.onFinished, AnimaionFinish, true);
                    }
                }
            }
        }
    }

    public void StopAnimation()
    {
        if (m_Transform != null)
        {
            UnityEngine.Transform tf = m_Transform.Find("Label");
            if (tf != null)
            {
                m_Anim = null;
                UITweener[] t = tf.GetComponentsInChildren<UITweener>();
                if (t.Length == 0)
                {
                    Animation a = tf.GetComponent<Animation>();
                    if (a != null)
                    {
                        m_Anim = ActiveAnimation.Play(a, a.clip.name, AnimationOrTween.Direction.Forward,
                                                      AnimationOrTween.EnableCondition.EnableThenPlay, AnimationOrTween.DisableCondition.DisableAfterForward);
                        EventDelegate.Add(m_Anim.onFinished, AnimaionFinish, true);
                        m_Anim.StopAllCoroutines();
                    }
                }
            }
            InitState();
        }
    }
    public bool IsActive()
    {
        return bActive;
    }
    public void SetText(string text)
    {
        if (m_Label != null && !string.IsNullOrEmpty(text))
        {
            m_Label.text = text;
        }
    }
    public void SetTextColor(UnityEngine.Color color)
    {
        if (m_Label != null)
        {
            m_Label.color = color;
        }
    }
}
