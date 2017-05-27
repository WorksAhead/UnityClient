using UnityEngine;
using System.Collections;

public class UIGuideHandScript : UnityEngine.MonoBehaviour
{

    public UISprite spSkillIcon;
    private bool m_NeedLoop = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTweenHandFinished()
    {
        if (m_NeedLoop)
        {
            UITweener[] tweeners = this.GetComponentsInChildren<UITweener>();
            for (int index = 0; index < tweeners.Length; ++index)
            {
                if (tweeners[index] != null)
                {
                    tweeners[index].ResetToBeginning();
                    tweeners[index].PlayForward();
                }
            }
        }
    }
    //设置是否需要循环播放
    public void EnableLoop(bool enable)
    {
        m_NeedLoop = enable;
    }
    //
    public void SkillStyle(UIAtlas atlas, string spriteName)
    {
        EnableLoop(false);
        if (spSkillIcon != null)
        {
            NGUITools.SetActive(spSkillIcon.gameObject, true);
            spSkillIcon.atlas = atlas;
            spSkillIcon.spriteName = spriteName;
        }
    }
}
