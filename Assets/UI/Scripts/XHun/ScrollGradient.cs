using UnityEngine;
using System.Collections;

public class ScrollGradient : UnityEngine.MonoBehaviour
{
    UIScrollView mScrollView;
    UnityEngine.GameObject mCenteredObject;

    // Use this for initialization
    void Start()
    {
        try
        {
            mScrollView = NGUITools.FindInParents<UIScrollView>(gameObject);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    UnityEngine.Vector3 PostionConvert(UnityEngine.Vector3 orign)
    {
        UnityEngine.Vector3 newPos = orign;
        return newPos;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (mScrollView.panel == null)
                return;

            // Calculate the panel's center in world coordinates
            UnityEngine.Vector3[] corners = mScrollView.panel.worldCorners;
            UnityEngine.Vector3 panelCenter = (corners[2] + corners[0]) * 0.5f;

            // Offset this value by the momentum
            //UnityEngine.Vector3 pickingPoint = panelCenter - mScrollView.currentMomentum * (mScrollView.momentumAmount * 0.1f);
            mScrollView.currentMomentum = UnityEngine.Vector3.zero;

            UnityEngine.Transform trans = transform;

            float length = UnityEngine.Vector3.SqrMagnitude(corners[2] - corners[0]) / 2;

            // Determine the closest child
            for (int i = 0, imax = trans.childCount; i < imax; ++i)
            {
                UnityEngine.Transform t = trans.GetChild(i);
                float sqrDist = UnityEngine.Vector3.SqrMagnitude(t.position - panelCenter);

                if (sqrDist > length)
                    sqrDist = length;

                if (sqrDist < length)
                {
                    float factor = sqrDist / length;
                    factor = 1.2f - UnityEngine.Mathf.Sqrt(factor) * 0.4f;
                    t.transform.localScale = new UnityEngine.Vector3(factor, factor, factor);

                    float alphaFactor = UnityEngine.Mathf.Pow((1 - sqrDist / length), 3);
                    //t.transform.gameObject.GetComponent<>
                    UISprite[] sprites = t.transform.gameObject.GetComponentsInChildren<UISprite>();
                    foreach (UISprite child in sprites)
                    {
                        if (child.gameObject.name == "GreenFrame")
                        {
                            child.alpha = UnityEngine.Mathf.Pow(alphaFactor, 5);//绿框接近透明
                        }
                        else
                        {
                            child.alpha = alphaFactor;
                        }
                    }
                }

            }
            mScrollView.panel.SetDirty();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}
