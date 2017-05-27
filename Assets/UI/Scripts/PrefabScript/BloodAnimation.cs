using UnityEngine;
using System.Collections;

public class BloodAnimation : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AnimaionFinish()
    {
        ArkCrossEngine.GameObject _gameobject = ArkCrossEngine.ObjectFactory.Create<ArkCrossEngine.GameObject>(gameObject);//new ArkCrossEngine.GameObject(gameObject);
        ArkCrossEngine.ResourceSystem.RecycleObject(_gameobject);
        if (anim != null)
        {
            EventDelegate.Remove(anim.onFinished, AnimaionFinish);
        }
    }

    public void PlayAnimation()
    {
        UnityEngine.Transform tf = transform.Find("Label");
        if (tf != null)
        {
            Animation a = tf.GetComponent<Animation>();
            UITweener[] t = tf.GetComponentsInChildren<UITweener>();
            if (a != null && t.Length == 0)
            {
                anim = ActiveAnimation.Play(a, a.clip.name, AnimationOrTween.Direction.Forward,
                  AnimationOrTween.EnableCondition.EnableThenPlay, AnimationOrTween.DisableCondition.DisableAfterForward);
                EventDelegate.Add(anim.onFinished, AnimaionFinish, true);
            }
            else
            {
                anim = null;
                int count = t.Length;
                if (t != null && count > 0)
                {
                    for (int i = 0; i < count; ++i)
                    {
                        t[i].ResetToBeginning();
                        t[i].PlayForward();
                    }
                }
            }
        }
    }
    private ActiveAnimation anim = null;
}
