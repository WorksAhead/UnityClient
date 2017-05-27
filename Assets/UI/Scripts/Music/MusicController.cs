namespace ArkCrossEngine
{
    public class MusicController : UnityEngine.MonoBehaviour
    {
        void Start()
        {
            try
            {
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int>("ge_init_backgroud_music", "music", InitBackgroudMusic);
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_play_backgroud_music", "music", PlayBackgroudMusic);
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_stop_backgroud_music", "music", StopBackgroudMusic);
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe<int, int>("ge_pause_or_resume_backgroud_music", "music", PauseOrResumeBackgroudMusic);
            }
            catch (System.Exception ex)
            {
                ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void InitBackgroudMusic(int scene_id)
        {
            Data_SceneConfig dsc = SceneConfigProvider.Instance.GetSceneConfigById(scene_id);
            if (null != dsc)
            {
                UnityEngine.GameObject go = UnityEngine.GameObject.Find("Audio");
                if (null != go)
                {
                    UnityEngine.AudioSource[] audio_source = go.GetComponents<UnityEngine.AudioSource>();
                    if (null == audio_source)
                        return;
                    for (int i = 0; i < audio_source.Length; i++)
                    {
                        if (null != audio_source[i])
                        {
                            if (audio_source[i].isPlaying)
                                continue;
                            string name = "";
                            float volume = 1.0f;
                            if (0 == i)
                            {
                                name = dsc.m_BkMusic;
                                volume = dsc.m_BkMusicVolume;
                            }
                            else
                            {
                                name = dsc.m_StoryMusic;
                                volume = dsc.m_StoryMusicVolume;
                            }
                            UnityEngine.AudioClip clip = (UnityEngine.AudioClip)UnityEngine.Resources.Load(name) as UnityEngine.AudioClip;
                            if (null != clip)
                            {
                                audio_source[i].clip = clip;
                                audio_source[i].volume = dsc.m_BkMusicVolume * 0.0f; // tmp disable backgroud music
                            }
                        }
                    }
                }
            }
        }
        private void PlayBackgroudMusic()
        {
            int scene_id = UIDataCache.Instance.curSceneId;
            Data_SceneConfig data = SceneConfigProvider.Instance.GetSceneConfigById(scene_id);
            if (null != data && !(1 == data.m_Type && 0 == data.m_SubType)
              && !(1 == data.m_Type && 5 == data.m_SubType)
              && !(3 == data.m_Type && 0 == data.m_SubType))
            {
                PauseOrResumeBackgroudMusic(0, 1);
            }
            PauseOrResumeBackgroudMusic(0, 0);
        }
        private void PauseOrResumeBackgroudMusic(int is_pause, int index)
        {
            UnityEngine.GameObject go = UnityEngine.GameObject.Find("Audio");
            if (null != go)
            {
                UnityEngine.AudioSource[] audio_source = go.GetComponents<UnityEngine.AudioSource>();
                if (null == audio_source)
                    return;
                for (int i = 0; i < audio_source.Length; i++)
                {
                    if (i == index && null != audio_source[i])
                    {
                        if (is_pause > 0)
                        {
                            if (audio_source[i].isPlaying)
                                audio_source[i].Pause();
                        }
                        else
                        {
                            if (!audio_source[i].isPlaying)
                                audio_source[i].Play();
                        }
                    }
                }
            }
        }
        private void StopBackgroudMusic()
        {
            UnityEngine.GameObject go = UnityEngine.GameObject.Find("Audio");
            if (null != go)
            {
                UnityEngine.AudioSource[] audio_source = go.GetComponents<UnityEngine.AudioSource>();
                if (null == audio_source)
                    return;
                for (int i = 0; i < audio_source.Length; i++)
                {
                    if (null != audio_source[i])
                    {
                        if (audio_source[i].isPlaying)
                            audio_source[i].Stop();
                    }
                }
            }
        }
    }
}
