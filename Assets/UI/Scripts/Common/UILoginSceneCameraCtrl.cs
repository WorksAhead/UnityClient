using UnityEngine;

public class UILoginSceneCameraCtrl : UnityEngine.MonoBehaviour
{
    public string clipName = "Take 001";
    private Animation m_Anim;
    private bool m_IsFinished = false;
    // Use this for initialization
    void Start()
    {
        //m_Anim = this.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(!m_IsFinished)
        //JudgeAnimationFinished();
    }
    public void SetAnimationFinished()
    {
        Animation anim = this.GetComponent<Animation>();
        if (anim != null)
        {
            AnimationClip clip = anim.GetClip(clipName);
            GetComponent<Animation>()[clipName].time = clip.length;
            GetComponent<Animation>()[clipName].enabled = true;
            // Sample animations now.
            // 取样动画。
            GetComponent<Animation>().Sample();
            GetComponent<Animation>()[clipName].enabled = false;
            m_IsFinished = true;
        }
    }
    public void JudgeAnimationFinished()
    {
        if (m_Anim != null && !m_Anim.isPlaying)
        {
            //结束
            UnityEngine.GameObject goLogin = UIManager.Instance.GetWindowGoByName("LoginPrefab");
            if (goLogin != null)
            {
                Login login = goLogin.GetComponent<Login>();
                if (login != null) StartCoroutine(login.PlayLogoAnimation());
            }
            m_IsFinished = true;
        }
    }
}
