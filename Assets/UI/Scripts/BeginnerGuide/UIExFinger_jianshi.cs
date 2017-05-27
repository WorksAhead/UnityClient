public class UIExFinger_jianshi : UnityEngine.MonoBehaviour
{

    public void OnTweenFingerFinished()
    {
        UIManager.Instance.HideWindowByName("ExFinger_jianshi");
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
        if (go != null)
        {
            SkillBar skillBar = go.GetComponent<SkillBar>();
            if (skillBar != null) skillBar.SetLockFrame(false);
        }
    }
}
