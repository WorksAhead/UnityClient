public class FadeControler : UnityEngine.MonoBehaviour
{
    //public float m_DarkPercent;
    public void SetBlackPercent(float dark_percent)
    {
        if (screen_bloom_ != null)
        {
            screen_bloom_.m_Color = UnityEngine.Color.Lerp(UnityEngine.Color.white, UnityEngine.Color.black, dark_percent);
        }
    }

    /*void Update ()
    {
      if (m_LastDarkPercent != m_DarkPercent) {
        SetDarkPercent(m_DarkPercent);
        m_LastDarkPercent = m_DarkPercent;
      }
    }*/

    void Start()
    {
        try
        {
            screen_bloom_ = gameObject.GetComponent<ScreenBloom>();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogFromGfx("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private ScreenBloom screen_bloom_;
    //private float m_LastDarkPercent = 0;
}
