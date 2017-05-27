using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CustomGestureEditor : EditorWindow 
{
  public const float GestureAreaSize = 400;
  public const float ToolbarHeight = 50;
  public const float MinSampleDistance = 5.0f;
  CustomGestureTemplate template;

  bool recording = false;
  List<UnityEngine.Vector2> points;

  public static CustomGestureEditor Open(CustomGestureTemplate template)
  {
    CustomGestureEditor window = EditorWindow.GetWindow<CustomGestureEditor>(true, "Gesture Editor: " + template.name);
    window.maxSize = new UnityEngine.Vector2(GestureAreaSize, GestureAreaSize + ToolbarHeight + 22);
    window.minSize = window.maxSize;
    window.Init(template);
    window.Focus();

    return window;
  }

  void Init(CustomGestureTemplate template)
  {
    this.template = template;
    points = new List<UnityEngine.Vector2>();
  }

  void OnGUI()
  {
    DrawGestureView();

    GUILayout.FlexibleSpace();

    DrawToolbar();

    GUILayout.Space(50);

    if (Event.current.type == EventType.MouseDown)
      OnMouseDown(Event.current.mousePosition);
    else if (Event.current.type == EventType.MouseDrag)
      OnMouseDrag(Event.current.mousePosition);
    else if (Event.current.type == EventType.MouseUp)
      OnMouseUp(Event.current.mousePosition);
  }

  void OnMouseDown(UnityEngine.Vector2 pos)
  {
    recording = true;
    points.Clear();
    AddPoint(pos);
  }

  void OnMouseDrag(UnityEngine.Vector2 pos)
  {
    if (recording) {
      if (points.Count > 0) {
        UnityEngine.Vector2 lastPos = points[points.Count - 1];

        if (UnityEngine.Vector2.SqrMagnitude(pos - lastPos) < (MinSampleDistance * MinSampleDistance))
          return;
      }

      AddPoint(pos);

      HandleUtility.Repaint();
    }
  }

  void OnMouseUp(UnityEngine.Vector2 pos)
  {
    recording = false;
  }

  void AddPoint(UnityEngine.Vector2 p)
  {
    points.Add(p);
  }

  void DrawTemplate()
  {
    float size = 0.95f * GestureAreaSize;
    Rect canvasRect = new Rect(0.5f * (GestureAreaSize - size), 0.5f * (GestureAreaSize - size), size, size);

    UnityEngine.Vector2 center = canvasRect.center;
    float scale = 0.95f * size;

    for (int i = 1; i < template.PointCount; ++i) {
      UnityEngine.Vector2 p1 = template.GetPosition(i - 1);
      UnityEngine.Vector2 p2 = template.GetPosition(i);

      p1.y = -p1.y;
      p2.y = -p2.y;

      p1 = center + scale * p1;
      p2 = center + scale * p2;

      Handles.DrawLine(p1, p2);
    }
  }

  void DrawNewPoints()
  {
    if (points.Count > 1) {
      Handles.color = UnityEngine.Color.yellow;

      for (int i = 1; i < points.Count; ++i) {
        UnityEngine.Vector2 p1 = points[i - 1];
        UnityEngine.Vector2 p2 = points[i];

        Handles.CircleCap(0, p1, UnityEngine.Quaternion.identity, 2.0f);
        Handles.DrawLine(p1, p2);
      }
    }
  }

  void DrawGestureView()
  {
    GUILayoutUtility.GetRect(GestureAreaSize, GestureAreaSize, GestureAreaSize, GestureAreaSize);

    Handles.color = UnityEngine.Color.white;
    DrawTemplate();

    Handles.color = UnityEngine.Color.yellow;
    DrawNewPoints();
  }

  void DrawToolbar()
  {
    GUILayout.BeginHorizontal(GUILayout.Height(ToolbarHeight));
    GUILayout.Space(5);

    GUI.enabled = points.Count > 1;

    if (ToolbarButton("Clear"))
      Clear();

    if (ToolbarButton("Apply"))
      Apply();

    GUI.enabled = true;

    GUILayout.Space(5);
    GUILayout.EndHorizontal();
  }

  void Clear()
  {
    points.Clear();
  }

  void Apply()
  {
    template.BeginPoints();

    for (int i = 0; i < points.Count; ++i) {
      UnityEngine.Vector2 p = points[i];
      p.y = -p.y; // must flip
      template.AddPoint(0, p);
    }

    template.EndPoints();

    Clear();
  }

  public bool ToolbarButton(string text)
  {
    return GUILayout.Button(text, GUILayout.ExpandHeight(true));
  }
}
