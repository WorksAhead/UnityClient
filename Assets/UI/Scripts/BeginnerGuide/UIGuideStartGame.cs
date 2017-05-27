public class UIGuideStartGame : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //
        if (JoyStickInputProvider.JoyStickEnable)
        {
            JoyStickInputProvider.JoyStickEnable = false;
        }
    }
    void OnClick()
    {
        UIManager.Instance.HideWindowByName("GuideStartGame");
        //UIManager.Instance.ShowWindowByName("HeroPanel");
        JoyStickInputProvider.JoyStickEnable = false;
        //UIManager.Instance.HideWindowByName("SkillBar");
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
        if (go != null)
        {
            HeroPanel heroView = go.GetComponent<HeroPanel>();
            if (heroView != null) heroView.SetActive(true);
            SkillBar sBar = go.GetComponent<SkillBar>();
            if (sBar != null) sBar.SetActive(false);
        }
        if (UIBeginnerGuide.Instance.onGuideGameStart != null)
        {
            UIBeginnerGuide.Instance.onGuideGameStart();
        }
    }
}
