//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using ArkCrossEngine;

/// <summary>
/// UIDragDropItem is a base script for your own Drag & Drop operations.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Drag and Drop Item")]
public class UIDragDropForEquip : UnityEngine.MonoBehaviour
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
    protected UnityEngine.Collider mCollider;
    protected int mTouchID = int.MinValue;
    protected float mPressTime = 0f;
    protected UIDragScrollView mDragScrollView = null;
    private int m_pos = -1;

    /// <summary>
    /// Cache the transform.
    /// </summary>

    protected virtual void Start()
    {
        try
        {
            mTrans = transform;
            mCollider = GetComponent<UnityEngine.Collider>();
            mDragScrollView = GetComponent<UIDragScrollView>();
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
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

        UnityEngine.GameObject gp = UIManager.Instance.GetWindowGoByName("GamePokey");
        if (gp != null)
        {
            UnityEngine.Transform root = UIDragDropRoot.root;
            if (root != null)
            {
                UnityEngine.GameObject item = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/GamePokey/ItemDrag"));
                if (item != null)
                {
                    item = NGUITools.AddChild(root.gameObject, item);
                    UnityEngine.Vector2 v2 = UICamera.currentTouch.pos;
                    UnityEngine.Vector3 v3 = UICamera.mainCamera.ScreenToWorldPoint(new UnityEngine.Vector3(v2.x, v2.y, 0));
                    item.transform.position = v3;//UICamera.mainCamera.transform.InverseTransformPoint(v2.x, v2.y, 0);
                    ItemClick ic = GetComponent<ItemClick>();
                    if (ic != null)
                    {
                        DFMItemIconUtils.Instance.SetItemInfo(ItemIconType.Equip_slot, item, ic.ID);
                        ArkCrossEngine.ItemConfig config = ArkCrossEngine.ItemConfigProvider.Instance.GetDataById(ic.ID);
                        if (config != null)
                        {
                            m_pos = config.m_WearParts;
                        }
                        ChangeSlotState(true);
                    }
                    UIDragScrollView dragScrollView = item.GetComponent<UIDragScrollView>();
                    if (dragScrollView != null)
                    {
                        UIScrollView scroll = transform.parent.parent.GetComponent<UIScrollView>();
                        dragScrollView.scrollView = scroll;
                    }
                    UIDragDropItemForDFM drag = item.GetComponent<UIDragDropItemForDFM>();
                    if (drag != null)
                    {
                        if (mDragScrollView != null)
                            mDragScrollView.enabled = false;

                        if (mCollider != null)
                            mCollider.enabled = false;
                        drag.onRealse = OnDragDropRelease;
                        drag.StartDrag();
                    }
                }
            }
        }
    }

    #endregion

    private void ChangeSlotState(bool show)
    {
        if (m_pos != -1)
        {
            EquipmentInfo info = GamePokeyManager.GetEquipmentInfo(m_pos);
            if (info != null)
            {
                UnityEngine.Transform frame = info.equipSlot.transform.Find("Frame");
                if (frame != null)
                {
                    NGUITools.SetActive(frame.gameObject, show);
                }
            }
        }
    }

    protected virtual void OnDragDropRelease(UnityEngine.GameObject surface)
    {
        ChangeSlotState(false);
        UIDragDropContainer container = surface ? NGUITools.FindInParents<UIDragDropContainer>(surface) : null;
        if (container != null)
        {
            if (container.reparentTarget.name == "Equipment")
            {
                
                ItemClick ic = mTrans.gameObject.GetComponent<ItemClick>();
                if (ic != null)
                {
                    if (surface != null)
                    {
                        ArkCrossEngine.RoleInfo roleInfo = ArkCrossEngine.LobbyClient.Instance.CurrentRole;
                        if (roleInfo != null)
                        {
                            ArkCrossEngine.ItemConfig itemConfig = ArkCrossEngine.ItemConfigProvider.Instance.GetDataById(ic.ID);
                            if (itemConfig != null)
                            {
                                if (itemConfig.m_WearLevel > roleInfo.Level)
                                {
                                    string tip = ArkCrossEngine.StrDictionaryProvider.Instance.GetDictString(46);
                                    ArkCrossEngine.LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", tip, UIScreenTipPosEnum.AlignCenter, new UnityEngine.Vector3(0f, 0f, 0f));
                                }
                                else
                                {
                                    int slotid = 0;
                                    string str = surface.transform.name;
                                    if (str != null)
                                    {
                                        char[] ch = str.ToCharArray();
                                        if (ch != null && ch.Length >= 5)
                                        {
                                            if (System.Int32.TryParse(ch[4].ToString(), out slotid))
                                            {
                                                EquipmentInfo ei = GamePokeyManager.GetEquipmentInfo(slotid);
                                                if (ei != null)
                                                {
                                                    ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_mount_equipment", "lobby", ic.ID, ic.PropertyId, slotid);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (mDragScrollView != null)
            mDragScrollView.enabled = true;

        if (mCollider != null)
            mCollider.enabled = true;

    }
}