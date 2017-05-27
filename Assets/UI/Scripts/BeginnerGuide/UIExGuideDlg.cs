using UnityEngine;
using System.Collections;
using ArkCrossEngine;
public class UIExGuideDlg : UnityEngine.MonoBehaviour
{
    public UILabel lblTeachWords;
    public float duration_cike_finger = 1f;
    public float ExGuideDlgTweenAlphaDelay_cike = 5f;
    public float ExGuideDlgTweenAlphaDelay_jianshi = 5f;
    private UnityEngine.GameObject m_RumtimeGuideHand;
    public UnityEngine.Vector3[] CikeFingerPos = new UnityEngine.Vector3[3];
    void Update()
    {
    }
    public void OnTweenFinished()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (role_info != null)
        {
            if (role_info.HeroId == (int)UIHeroType.Jianshi)
            {
                UIManager.Instance.LoadWindowByName("ExFinger_jianshi", UICamera.mainCamera);
            }
            if (role_info.HeroId == (int)UIHeroType.Cike)
            {
                StartCoroutine(ShowFinger_cike());
            }
        }
    }

    public void SetTeachWords(string str)
    {
        if (lblTeachWords != null) lblTeachWords.text = str;
    }
    public IEnumerator ShowFinger_cike()
    {
        SetLockFrame(false);
        if (m_RumtimeGuideHand == null)
            m_RumtimeGuideHand = UIManager.Instance.LoadWindowByName("GuideHand", UICamera.mainCamera);
        if (m_RumtimeGuideHand != null)
        {
            for (int index = 0; index < CikeFingerPos.Length; ++index)
            {
                UnityEngine.Vector3 screenPos = new UnityEngine.Vector3();
                screenPos.z = 0f;
                screenPos.x = CikeFingerPos[index].x * Screen.width;
                screenPos.y = CikeFingerPos[index].y * Screen.height;
                UnityEngine.Vector3 worldPos = UICamera.mainCamera.ScreenToWorldPoint(screenPos);
                m_RumtimeGuideHand.transform.position = worldPos;
                SetLockFrame(false);
                yield return new WaitForSeconds(duration_cike_finger);
            }
        }
        UIManager.Instance.HideWindowByName("GuideHand");
        yield return new WaitForSeconds(0f);
    }
    private void SetLockFrame(bool enable)
    {
        UnityEngine.GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
        if (go != null)
        {
            SkillBar skillBar = go.GetComponent<SkillBar>();
            if (skillBar != null) skillBar.SetLockFrame(enable);
        }
    }
    public void SetTweenAlphaDelay(UIHeroType hero)
    {
        TweenAlpha alpha = this.GetComponent<TweenAlpha>();
        if (alpha != null)
        {
            if (hero == UIHeroType.Cike)
                alpha.delay = ExGuideDlgTweenAlphaDelay_cike;
            if (hero == UIHeroType.Jianshi)
                alpha.delay = ExGuideDlgTweenAlphaDelay_jianshi;

        }
    }
}
