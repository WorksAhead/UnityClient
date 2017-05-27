using UnityEngine;
using System.Collections;
using ArkCrossEngine;
public class StageClearEffect : UnityEngine.MonoBehaviour
{

    public UnityEngine.GameObject minClearEffect = null;
    public UnityEngine.GameObject maxClearEffect = null;
    public UnityEngine.AudioClip audio1;//大特效一段声音
    public UnityEngine.AudioClip audio2;//大特效2段声音
    public UnityEngine.AudioClip audio0;// 小特效声音
                                        // Use this for initialization
    void Start()
    {
        try
        {
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    //打开类型
    public void OpenEffectType(int type)
    {
        switch (type)
        {
            case 0:
                ShowMinEffect();
                break;
            case 1:
                ShowMaxEffect();
                break;
        }
    }
    //小 
    void ShowMinEffect()
    {
        UnityEngine.Transform tf = transform.Find("StageClear");
        NGUITools.SetActive(tf.gameObject, false);
        tf = transform.Find("little_clear");
        NGUITools.SetActive(tf.gameObject, true);
        NGUITools.SetActive(minClearEffect, true);
        NGUITools.SetActive(maxClearEffect, false);
        NGUITools.PlaySound(audio0, 1f, 1f);
    }
    //大
    void ShowMaxEffect()
    {
        UnityEngine.Transform tf = transform.Find("little_clear");
        NGUITools.SetActive(tf.gameObject, false);

        tf = transform.Find("StageClear");
        NGUITools.SetActive(tf.gameObject, true);

        NGUITools.SetActive(minClearEffect, false);
        NGUITools.SetActive(maxClearEffect, true);
        NGUITools.PlaySound(audio1, 1f, 1f);
        Invoke("PlaySoundAudio2", 1f);
    }
    void PlaySoundAudio2()
    {
        NGUITools.PlaySound(audio2, 1f, 1f);
    }
    //销毁特效
    public void DestroyDefenseEffect()
    {
        UnityEngine.Transform tf = transform.Find("little_clear");
        NGUITools.SetActive(tf.gameObject, false);
        tf = transform.Find("StageClear");
        NGUITools.SetActive(tf.gameObject, false);
        Destroy(this.gameObject);
        NGUITools.SetActive(minClearEffect, false);
        NGUITools.SetActive(maxClearEffect, false);
    }
}
