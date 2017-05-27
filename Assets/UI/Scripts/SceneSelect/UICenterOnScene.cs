//
using UnityEngine;

/// <summary>
/// Ever wanted to be able to auto-center on an object within a draggable panel?
/// Attach this script to the container that has the objects to center on as its children.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Center Scroll View on Child")]
public class UICenterOnScene : UnityEngine.MonoBehaviour
{
    /// <summary>
    /// The strength of the spring.
    /// </summary>

    public float springStrength = 8f;

    /// <summary>
    /// If set to something above zero, it will be possible to move to the next page after dragging past the specified threshold.
    /// </summary>

    public float nextPageThreshold = 0f;

    /// <summary>
    /// Callback to be triggered when the centering operation completes.
    /// </summary>

    public SpringPanel.OnFinished onFinished;

    UIScrollView mScrollView;
    UnityEngine.GameObject mCenteredObject;

    /// <summary>
    /// Game object that the draggable panel is currently centered on.
    /// </summary>

    public UnityEngine.GameObject centeredObject
    {
        get
        {
            return mCenteredObject;
        }
    }
    /* Recenter()函数改在Start中调用
     * 注释掉OnEnable中的调用
     * OnEnable中调用会因为UICamera.currentTouch.totalDelta不为0而出现Bug
     */
    void Start()
    {
        try
        {
            Recenter();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    //
    //void OnEnable() { Recenter(); }
    void OnDragFinished() { if (enabled) Recenter(); }

    /// <summary>
    /// Ensure that the threshold is always positive.
    /// </summary>

    void OnValidate()
    {
        nextPageThreshold = UnityEngine.Mathf.Abs(nextPageThreshold);
    }

    /// <summary>
    /// Recenter the draggable list on the center-most child.
    /// </summary>

    public void Recenter()
    {
        if (mScrollView == null)
        {
            mScrollView = NGUITools.FindInParents<UIScrollView>(gameObject);

            if (mScrollView == null)
            {
                Debug.LogWarning(GetType() + " requires " + typeof(UIScrollView) + " on a parent object in order to work", this);
                enabled = false;
                return;
            }
            else
            {
                mScrollView.onDragFinished = OnDragFinished;

                if (mScrollView.horizontalScrollBar != null)
                    mScrollView.horizontalScrollBar.onDragFinished = OnDragFinished;

                if (mScrollView.verticalScrollBar != null)
                    mScrollView.verticalScrollBar.onDragFinished = OnDragFinished;
            }
        }
        if (mScrollView.panel == null) return;

        // Calculate the panel's center in world coordinates
        UnityEngine.Vector3[] corners = mScrollView.panel.worldCorners;
        UnityEngine.Vector3 panelCenter = (corners[2] + corners[0]) * 0.5f;
        // Offset this value by the momentum
        UnityEngine.Vector3 pickingPoint = panelCenter - mScrollView.currentMomentum * (mScrollView.momentumAmount * 0.1f);
        mScrollView.currentMomentum = UnityEngine.Vector3.zero;

        float min = float.MaxValue;
        UnityEngine.Transform closest = null;
        UnityEngine.Transform trans = transform;
        int index = 0;

        // Determine the closest child
        for (int i = 0, imax = trans.childCount; i < imax; ++i)
        {
            UnityEngine.Transform t = trans.GetChild(i);
            float sqrDist = UnityEngine.Vector3.SqrMagnitude(t.position - pickingPoint);
            if (sqrDist < min)
            {
                min = sqrDist;
                closest = t;
                index = i;
            }
        }

        // If we have a touch in progress and the next page threshold set
        if (nextPageThreshold > 0f && UICamera.currentTouch != null)
        {
            // If we're still on the same object
            if (mCenteredObject != null && mCenteredObject.transform == trans.GetChild(index))
            {
                UnityEngine.Vector2 totalDelta = UICamera.currentTouch.totalDelta;

                float delta = 0f;

                switch (mScrollView.movement)
                {
                    case UIScrollView.Movement.Horizontal:
                        {
                            delta = totalDelta.x;
                            break;
                        }
                    case UIScrollView.Movement.Vertical:
                        {
                            delta = totalDelta.y;
                            break;
                        }
                    default:
                        {
                            delta = totalDelta.magnitude;
                            break;
                        }
                }
                if (delta > nextPageThreshold)
                {
                    // Next page
                    if (index > 0)
                        closest = trans.GetChild(index - 1);
                }
                else if (delta < -nextPageThreshold)
                {
                    // Previous page
                    if (index < trans.childCount - 1)
                        closest = trans.GetChild(index + 1);
                }
            }
        }

        CenterOn(closest, panelCenter);
    }

    /// <summary>
    /// Center the panel on the specified target.
    /// </summary>

    void CenterOn(UnityEngine.Transform target, UnityEngine.Vector3 panelCenter)
    {
        if (target != null && mScrollView != null && mScrollView.panel != null)
        {
            UnityEngine.Transform panelTrans = mScrollView.panel.cachedTransform;
            mCenteredObject = target.gameObject;
            //通知翻页
            UICurrentChapter currentChapter = mCenteredObject.GetComponent<UICurrentChapter>();
            UISceneSelect sceneSelect = NGUITools.FindInParents<UISceneSelect>(this.gameObject);
            if (sceneSelect != null && currentChapter != null)
            {
                sceneSelect.SetCurrentChapter(currentChapter.m_ChapterId);
            }
            // Figure out the difference between the chosen child and the panel's center in local coordinates
            UnityEngine.Vector3 cp = panelTrans.InverseTransformPoint(target.position);
            UnityEngine.Vector3 cc = panelTrans.InverseTransformPoint(panelCenter);
            UnityEngine.Vector3 localOffset = cp - cc;

            // Offset shouldn't occur if blocked
            if (!mScrollView.canMoveHorizontally) localOffset.x = 0f;
            if (!mScrollView.canMoveVertically) localOffset.y = 0f;
            localOffset.z = 0f;

            // Spring the panel to this calculated position
            SpringPanel.Begin(mScrollView.panel.cachedGameObject,
              panelTrans.localPosition - localOffset, springStrength).onFinished = onFinished;
        }
        else mCenteredObject = null;
    }

    /// <summary>
    /// Center the panel on the specified target.
    /// </summary>

    public void CenterOn(UnityEngine.Transform target)
    {
        if (mScrollView != null && mScrollView.panel != null)
        {
            UnityEngine.Vector3[] corners = mScrollView.panel.worldCorners;
            UnityEngine.Vector3 panelCenter = (corners[2] + corners[0]) * 0.5f;
            CenterOn(target, panelCenter);
        }
    }
}