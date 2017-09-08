//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// UIDragDropItem is a base script for your own Drag & Drop operations.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Drag and Drop Item")]
public class UIDragDropItemForDFM : UnityEngine.MonoBehaviour
{
    public enum Restriction
    {
        None,
        Horizontal,
        Vertical,
        PressAndHold,
    }

    /// <summary>
    /// What kind of restriction is applied to the drag & drop logic before dragging is made possible.
    /// </summary>

    public Restriction restriction = Restriction.None;

    /// <summary>
    /// Whether a copy of the item will be dragged instead of the item itself.
    /// </summary>

    public bool cloneOnDrag = false;

    #region Common functionality

    protected UnityEngine.Transform mTrans;
    protected UnityEngine.Transform mParent;
    protected UnityEngine.Collider mCollider;
    protected UIRoot mRoot;
    protected UIGridForDFM mGrid;
    protected UITable mTable;
    protected int mTouchID = int.MinValue;
    protected float mPressTime = 0f;
    protected UIDragScrollView mDragScrollView = null;
    public OnRealse onRealse;
    public delegate void OnRealse(UnityEngine.GameObject go);
    /// <summary>
    /// Cache the transform.
    /// </summary>

    protected virtual void Start()
    {
        try
        {
            mTrans = transform;
            mCollider = GetComponent<Collider>();
            mDragScrollView = GetComponent<UIDragScrollView>();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogErrorFromGfx("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void StartDrag()
    {
        //UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
        UICamera.currentTouch.pressed = gameObject;
        UICamera.currentTouch.dragged = gameObject;
        Start();
        OnDragDropStart();
    }

    /// <summary>
    /// Record the time the item was pressed on.
    /// </summary>

    void OnPress(bool isPressed) { if (isPressed) mPressTime = RealTime.time; }

    /// <summary>
    /// Start the dragging operation.
    /// </summary>

    void OnDragStart()
    {
        if (!enabled || mTouchID != int.MinValue) return;

        // If we have a restriction, check to see if its condition has been met first
        if (restriction != Restriction.None)
        {
            if (restriction == Restriction.Horizontal)
            {
                UnityEngine.Vector2 delta = UICamera.currentTouch.totalDelta;
                if (UnityEngine.Mathf.Abs(delta.x) < UnityEngine.Mathf.Abs(delta.y)) return;
            }
            else if (restriction == Restriction.Vertical)
            {
                UnityEngine.Vector2 delta = UICamera.currentTouch.totalDelta;
                if (UnityEngine.Mathf.Abs(delta.x) > UnityEngine.Mathf.Abs(delta.y)) return;
            }
            else if (restriction == Restriction.PressAndHold)
            {
                if (mPressTime + 1f > RealTime.time) return;
            }
        }

        if (cloneOnDrag)
        {
            UnityEngine.GameObject clone = NGUITools.AddChild(transform.parent.gameObject, gameObject);
            clone.transform.localPosition = transform.localPosition;
            clone.transform.localRotation = transform.localRotation;
            clone.transform.localScale = transform.localScale;

            UIButtonColor bc = clone.GetComponent<UIButtonColor>();
            if (bc != null) bc.defaultColor = GetComponent<UIButtonColor>().defaultColor;

            UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);

            UICamera.currentTouch.pressed = clone;
            UICamera.currentTouch.dragged = clone;

            UIDragDropItemForDFM item = clone.GetComponent<UIDragDropItemForDFM>();
            item.Start();
            item.OnDragDropStart();
        }
        else OnDragDropStart();
    }

    /// <summary>
    /// Perform the dragging.
    /// </summary>

    void OnDrag(UnityEngine.Vector2 delta)
    {
        if (!enabled || mTouchID != UICamera.currentTouchID) return;
        OnDragDropMove((UnityEngine.Vector3)delta * mRoot.pixelSizeAdjustment);
    }

    /// <summary>
    /// Notification sent when the drag event has ended.
    /// </summary>

    void OnDragEnd()
    {
        if (!enabled || mTouchID != UICamera.currentTouchID) return;
        OnDragDropRelease(UICamera.hoveredObject);
    }

    #endregion

    /// <summary>
    /// Perform any logic related to starting the drag & drop operation.
    /// </summary>
    private UnityEngine.Vector3 mypos;
    protected virtual void OnDragDropStart()
    {
        //if (UnityEngine.Time.realtimeSinceStartup - UIManager.dragtime < 0.5f) { return; }
        // Automatically disable the scroll view
        if (mDragScrollView != null) mDragScrollView.enabled = false;

        // Disable the collider so that it doesn't intercept events
        if (mCollider != null) mCollider.enabled = false;
        mypos = mTrans.transform.localPosition;
        mTouchID = UICamera.currentTouchID;
        mParent = mTrans.parent;
        mRoot = NGUITools.FindInParents<UIRoot>(mParent);

        // Re-parent the item
        if (UIDragDropRoot.root != null)
            mTrans.parent = UIDragDropRoot.root;

        UnityEngine.Vector3 pos = mTrans.localPosition;
        pos.z = 0f;
        mTrans.localPosition = pos;

        // Notify the widgets that the parent has changed
        NGUITools.MarkParentAsChanged(gameObject);
    }

    /// <summary>
    /// Adjust the dragged object's position.
    /// </summary>

    protected virtual void OnDragDropMove(UnityEngine.Vector3 delta)
    {
        mTrans.localPosition += delta;
    }

    /// <summary>
    /// Drop the item onto the specified object.
    /// </summary>

    protected virtual void OnDragDropRelease(UnityEngine.GameObject surface)
    {
        UIManager.dragtime = UnityEngine.Time.realtimeSinceStartup;
        if (!cloneOnDrag)
        {

            NGUITools.Destroy(gameObject);

            if (onRealse != null)
            {
                onRealse(surface);
            }
        }
        else
            NGUITools.Destroy(gameObject);
    }
}
