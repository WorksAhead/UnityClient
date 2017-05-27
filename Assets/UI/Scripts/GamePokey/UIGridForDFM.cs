//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// All children added to the game object with this script will be repositioned to be on a grid of specified dimensions.
/// If you want the cells to automatically set their scale based on the dimensions of their content, take a look at UITable.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Grid")]
public class UIGridForDFM : UIWidgetContainer
{
    public delegate void OnReposition();

    public enum Arrangement
    {
        Horizontal,
        Vertical,
    }

    /// <summary>
    /// Type of arrangement -- vertical or horizontal.
    /// </summary>

    public Arrangement arrangement = Arrangement.Horizontal;

    /// <summary>
    /// Maximum children per line.
    /// If the arrangement is horizontal, this denotes the number of columns.
    /// If the arrangement is vertical, this stands for the number of rows.
    /// </summary>

    public int maxPerLine = 0;

    /// <summary>
    /// The width of each of the cells.
    /// </summary>

    public float cellWidth = 200f;

    /// <summary>
    /// The height of each of the cells.
    /// </summary>

    public float cellHeight = 200f;

    /// <summary>
    /// Whether the grid will smoothly animate its children into the correct place.
    /// </summary>

    public bool animateSmoothly = false;

    /// <summary>
    /// Whether the children will be sorted alphabetically prior to repositioning.
    /// </summary>

    public bool sorted = false;

    /// <summary>
    /// Whether to ignore the disabled children or to treat them as being present.
    /// </summary>

    public bool hideInactive = true;

    /// <summary>
    /// Whether the parent container will be notified of the grid's changes.
    /// </summary>

    public bool keepWithinPanel = false;

    /// <summary>
    /// Callback triggered when the grid repositions its contents.
    /// </summary>

    public OnReposition onReposition;

    /// <summary>
    /// Reposition the children on the next Update().
    /// </summary>

    public bool repositionNow { set { if (value) { mReposition = true; enabled = true; } } }
    public bool sortRepositionForDF { set { if (value) { DFReposition = true; mReposition = true; enabled = true; } } }

    bool mReposition = false;
    UIPanel mPanel;
    bool mInitDone = false;
    bool DFReposition = false;

    protected virtual void Init()
    {
        mInitDone = true;
        mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
    }

    protected virtual void Start()
    {
        if (!mInitDone) Init();
        bool smooth = animateSmoothly;
        animateSmoothly = false;
        Reposition();
        animateSmoothly = smooth;
        enabled = false;
    }

    protected virtual void Update()
    {
        if (mReposition) Reposition();
        enabled = false;
    }
    static protected int SortByName(UnityEngine.Transform a, UnityEngine.Transform b)
    {
        int i;
        int j;
        if (int.TryParse(a.name.Remove(0, 4), out i) && int.TryParse(b.name.Remove(0, 4), out j))
        {
            return i - j;
        }
        else
        {
            return 0;
        }
    }

    static protected int SortByDFM(UnityEngine.Transform a, UnityEngine.Transform b)
    {/* return string.Compare(a.name, b.name);*/
        int ranka = 0;
        int rankb = 0;
        if (a != null && b != null)
        {
            ItemClick ic = a.gameObject.GetComponent<ItemClick>();
            if (ic != null)
            {
                ArkCrossEngine.ItemConfig itemconfig = ArkCrossEngine.LogicSystem.GetItemDataById(ic.ID);
                if (itemconfig != null)
                {
                    //ranka = itemconfig.m_PropertyRank;
                    ranka = ic.fightScoreChange;
                }
            }
            ic = b.gameObject.GetComponent<ItemClick>();
            if (ic != null)
            {
                ArkCrossEngine.ItemConfig itemconfig = ArkCrossEngine.LogicSystem.GetItemDataById(ic.ID);
                if (itemconfig != null)
                {
                    //rankb = itemconfig.m_PropertyRank;
                    rankb = ic.fightScoreChange;
                }
            }
        }
        return rankb - ranka;
    }

    /// <summary>
    /// Want your own custom sorting logic? Override this function.
    /// </summary>

    protected virtual void DFMSort(List<UnityEngine.Transform> list) { list.Sort(SortByDFM); }
    protected virtual void Sort(List<UnityEngine.Transform> list) { list.Sort(SortByName); }

    /// <summary>
    /// Recalculate the position of all elements within the grid, sorting them alphabetically if necessary.
    /// </summary>

    [ContextMenu("Execute")]
    public virtual void Reposition()
    {
        if (UnityEngine.Application.isPlaying && !mInitDone && NGUITools.GetActive(this))
        {
            mReposition = true;
            return;
        }

        if (!mInitDone) Init();

        mReposition = false;
        UnityEngine.Transform myTrans = transform;
        bool noonesort = true;
        int x = 0;
        int y = 0;

        if (DFReposition)
        {
            DFReposition = false;
            noonesort = false;
            List<UnityEngine.Transform> list = new List<UnityEngine.Transform>();

            for (int i = 0; i < myTrans.childCount; ++i)
            {
                UnityEngine.Transform t = myTrans.GetChild(i);
                if (t && (!hideInactive || NGUITools.GetActive(t.gameObject))) list.Add(t);
            }
            DFMSort(list);

            for (int i = 0, imax = list.Count; i < imax; ++i)
            {
                UnityEngine.Transform t = list[i];
                t.name = "Item" + i;

                if (!NGUITools.GetActive(t.gameObject) && hideInactive) continue;

                float depth = t.localPosition.z;
                UnityEngine.Vector3 pos = (arrangement == Arrangement.Horizontal) ?
                    new UnityEngine.Vector3(cellWidth * x, -cellHeight * y, depth) :
                    new UnityEngine.Vector3(cellWidth * y, -cellHeight * x, depth);

                if (animateSmoothly && UnityEngine.Application.isPlaying)
                {
                    SpringPosition.Begin(t.gameObject, pos, 15f).updateScrollView = true;
                }
                else t.localPosition = pos;

                if (++x >= maxPerLine && maxPerLine > 0)
                {
                    x = 0;
                    ++y;
                }
            }
        }
        if (sorted && noonesort)
        {
            List<UnityEngine.Transform> list = new List<UnityEngine.Transform>();

            for (int i = 0; i < myTrans.childCount; ++i)
            {
                UnityEngine.Transform t = myTrans.GetChild(i);
                if (t && (!hideInactive || NGUITools.GetActive(t.gameObject))) list.Add(t);
            }
            Sort(list);

            for (int i = 0, imax = list.Count; i < imax; ++i)
            {
                UnityEngine.Transform t = list[i];

                if (!NGUITools.GetActive(t.gameObject) && hideInactive) continue;

                float depth = t.localPosition.z;
                UnityEngine.Vector3 pos = (arrangement == Arrangement.Horizontal) ?
                  new UnityEngine.Vector3(cellWidth * x, -cellHeight * y, depth) :
                  new UnityEngine.Vector3(cellWidth * y, -cellHeight * x, depth);

                if (animateSmoothly && UnityEngine.Application.isPlaying)
                {
                    SpringPosition.Begin(t.gameObject, pos, 15f).updateScrollView = true;
                }
                else t.localPosition = pos;

                if (++x >= maxPerLine && maxPerLine > 0)
                {
                    x = 0;
                    ++y;
                }
            }
        }

        if (keepWithinPanel && mPanel != null)
            mPanel.ConstrainTargetToBounds(myTrans, true);

        if (onReposition != null)
            onReposition();
    }
}
