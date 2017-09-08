using UnityEngine;
using ArkCrossEngine;

public class UiPortalObject : UnityEngine.MonoBehaviour
{
    public string WindowName = "";
    public bool IsStopped = true;
    public void SetVisible(int visible)
    {
        UnityEngine.Renderer[] renderers = gameObject.GetComponentsInChildren<UnityEngine.Renderer>();
        for (int i = 0; i < renderers.Length; ++i)
        {
            if (visible == 0)
            {
                renderers[i].enabled = false;
            }
            else
            {
                renderers[i].enabled = true;
            }
        }
    }
    public void PlayParticle()
    {
        UnityEngine.ParticleSystem[] pss = gameObject.GetComponentsInChildren<UnityEngine.ParticleSystem>();
        for (int i = 0; i < pss.Length; ++i)
        {
            pss[i].Play();
        }
        IsStopped = false;
    }
    public void StopParticle()
    {
        UnityEngine.ParticleSystem[] pss = gameObject.GetComponentsInChildren<UnityEngine.ParticleSystem>();
        for (int i = 0; i < pss.Length; ++i)
        {
            pss[i].Stop();
        }
        IsStopped = true;
    }
    public void PlaySound(int index)
    {
        UnityEngine.AudioSource[] audios = gameObject.GetComponents<UnityEngine.AudioSource>();
        if (null != audios && index >= 0 && index < audios.Length)
        {
            audios[index].Play();
        }
    }
    public void StopSound(int index)
    {
        UnityEngine.AudioSource[] audios = gameObject.GetComponents<UnityEngine.AudioSource>();
        if (null != audios && index >= 0 && index < audios.Length)
        {
            audios[index].Stop();
        }
    }

    private void OnTriggerEnter(UnityEngine.Collider collider)
    {
        try
        {
            if (!IsStopped)
            {
                UnityEngine.GameObject obj = collider.gameObject;
                if (null != obj)
                {
                    if (obj == LogicSystem.PlayerSelf)
                    {
                        UIManager.Instance.ShowWindowByName(WindowName);
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void OnTriggerExit(UnityEngine.Collider collider)
    {
        try
        {
            //Debug.Log("ontriggerexit");
        }
        catch (System.Exception ex)
        {
            LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}
