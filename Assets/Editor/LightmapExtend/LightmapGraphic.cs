using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class LightmapGraphic
{
    public static void PaintTextureTriangles(List<TriangleInfo> triangles, Rect textureRect, UnityEngine.Color color)
    {
        UnityEngine.Vector2 leftTop = new UnityEngine.Vector2(textureRect.xMin, textureRect.yMin);
        for (int i = 0; i < triangles.Count; i++)
        {
            UnityEngine.Vector2 pointA = triangles[i].TexOffs[0] + leftTop;
            UnityEngine.Vector2 pointB = triangles[i].TexOffs[1] + leftTop;
            UnityEngine.Vector2 pointC = triangles[i].TexOffs[2] + leftTop;
            GUIGraphic.DrawLine(pointA, pointB, color);
            GUIGraphic.DrawLine(pointA, pointC, color);
            GUIGraphic.DrawLine(pointB, pointC, color);
        }
        /*
    foreach (TriangleInfo triInfo in triangles) {
      UnityEngine.Vector2 pointA = triInfo.TexOffs[0] + leftTop;
      UnityEngine.Vector2 pointB = triInfo.TexOffs[1] + leftTop;
      UnityEngine.Vector2 pointC = triInfo.TexOffs[2] + leftTop;
      GUIGraphic.DrawLine(pointA, pointB, color);
      GUIGraphic.DrawLine(pointA, pointC, color);
      GUIGraphic.DrawLine(pointB, pointC, color);
    }*/
    }
    public static void PaintTextureTriangle(TriangleInfo triInfo, Rect textureRect, UnityEngine.Color color)
    {
        if (triInfo == null || triInfo.Vertexs == null || triInfo.Vertexs.Count != 3)
        {
            return;
        }
        UnityEngine.Vector2 leftTop = new UnityEngine.Vector2(textureRect.xMin, textureRect.yMin);
        UnityEngine.Vector2 OffMin = new UnityEngine.Vector2(0.5f, 0.5f);
        for (int index = 0; index < 1; index++)
        {
            UnityEngine.Vector2 Off = OffMin * index;
            UnityEngine.Vector2 pointA = triInfo.TexOffs[0] + leftTop + Off;
            UnityEngine.Vector2 pointB = triInfo.TexOffs[1] + leftTop + Off;
            UnityEngine.Vector2 pointC = triInfo.TexOffs[2] + leftTop + Off;
            GUIGraphic.DrawLine(pointA, pointB, color);
            GUIGraphic.DrawLine(pointA, pointC, color);
            GUIGraphic.DrawLine(pointB, pointC, color);
        }
    }
    public static void PaintSceneTriangle(TriangleInfo triInfo)
    {
        if (triInfo == null || triInfo.Vertexs == null || triInfo.Vertexs.Count != 3)
        {
            return;
        }
        List<UnityEngine.Vector3> vertexs = triInfo.Vertexs;
        UnityEngine.Transform transform = triInfo.Renderer.transform;
        UnityEngine.Vector3 pointA = transform.TransformPoint(vertexs[0]);
        UnityEngine.Vector3 pointB = transform.TransformPoint(vertexs[1]);
        UnityEngine.Vector3 pointC = transform.TransformPoint(vertexs[2]);
        UnityEngine.Color oldColor = Handles.color;
        Handles.color = UnityEngine.Color.red;
        Handles.DrawLine(pointA, pointB);
        Handles.DrawLine(pointA, pointC);
        Handles.DrawLine(pointB, pointC);
        Handles.color = oldColor;
    }

}