using UnityEngine;

public class UIPlaySoundInvoke : UnityEngine.MonoBehaviour
{

    public UnityEngine.AudioClip audioClip;
    public float delay = 0f;
#if UNITY_3_5
	public float volume = 1f;
	public float pitch = 1f;
#else
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0f, 2f)]
    public float pitch = 1f;
#endif
    void Start()
    {
        if (delay > 0f)
        {
            Invoke("Play", delay);
        }
        else
        {
            Play();
        }
    }

    public void Play()
    {
        NGUITools.PlaySound(audioClip, volume, pitch);
    }
}
