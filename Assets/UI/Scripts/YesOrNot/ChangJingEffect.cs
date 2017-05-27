using UnityEngine;
using System.Collections;

public class ChangJingEffect : UnityEngine.MonoBehaviour
{
    public enum OpenSceneType : int
    {
        T_Boss = 1,             // boss出场
        T_Checkpoint = 2,       // 关卡开始
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //打开类型
    public void OpenEffectType(OpenSceneType type, string name, int chapter = 0, int section = 0)
    {
        switch (type)
        {
            case OpenSceneType.T_Boss:
                ShowBossEffect(name);
                break;
            case OpenSceneType.T_Checkpoint:
                ShowSceneBeginEffect(name, chapter, section);
                break;
        }
    }
    //boss 显示  
    void ShowBossEffect(string name)
    {
        UnityEngine.Transform tf = transform.Find("name");
        NGUITools.SetActive(tf.gameObject, false);
        tf = transform.Find("Boss");
        NGUITools.SetActive(tf.gameObject, true);

        tf = tf.transform.Find("bossName/Label");
        if (null != tf)
        {
            UILabel uiLable = tf.gameObject.GetComponent<UILabel>();
            if (null != uiLable)
            {
                uiLable.text = name;
            }
        }
    }
    //关卡刚刚开始特效
    void ShowSceneBeginEffect(string name, int chapter, int section)
    {
        UnityEngine.Transform tf = transform.Find("Boss");
        NGUITools.SetActive(tf.gameObject, false);
        tf = transform.Find("name");
        NGUITools.SetActive(tf.gameObject, true);

        UnityEngine.Transform labelTf = tf.Find("name/Label");
        if (null != labelTf)
        {
            UILabel uiLable = labelTf.gameObject.GetComponent<UILabel>();
            if (null != uiLable)
            {
                uiLable.text = name;
            }
        }
        UnityEngine.Transform usTF = tf.Find("number/number");
        if (usTF != null)
        {
            UISprite us = usTF.gameObject.GetComponent<UISprite>();
            if (us != null)
            {
                us.spriteName = "d" + section;
            }
        }
    }
    //销毁特效
    public void DestroyDefenseEffect()
    {
        UnityEngine.Transform tf = transform.Find("Boss");
        NGUITools.SetActive(tf.gameObject, false);
        tf = transform.Find("name");
        NGUITools.SetActive(tf.gameObject, false);
        Destroy(this.gameObject);
    }
}
