using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;

public class GUIHelp
{
    public static void OpenInMacFileBrowser(string path)
    {
        bool openInsidesOfFolder = false;

        // try mac
        string macPath = path.Replace("\\", "/"); // mac finder doesn't like backward slashes

        if (Directory.Exists(macPath)) // if path requested is a folder, automatically open insides of that folder
        {
            openInsidesOfFolder = true;
        }

        //Debug.Log("macPath: " + macPath);
        //Debug.Log("openInsidesOfFolder: " + openInsidesOfFolder);

        if (!macPath.StartsWith("\""))
        {
            macPath = "\"" + macPath;
        }
        if (!macPath.EndsWith("\""))
        {
            macPath = macPath + "\"";
        }
        string arguments = (openInsidesOfFolder ? "" : "-R ") + macPath;
        //Debug.Log("arguments: " + arguments);
        try
        {
            System.Diagnostics.Process.Start("open", arguments);
        }
        catch (System.ComponentModel.Win32Exception e)
        {
            // tried to open mac finder in windows
            // just silently skip error
            // we currently have no platform define for the current OS we are in, so we resort to this
            e.HelpLink = ""; // do anything with this variable to silence warning about not using it
        }
    }

    public static void OpenInWinFileBrowser(string path)
    {
        bool openInsidesOfFolder = false;

        // try windows
        string winPath = path.Replace("/", "\\"); // windows explorer doesn't like forward slashes

        if (Directory.Exists(winPath)) // if path requested is a folder, automatically open insides of that folder
        {
            openInsidesOfFolder = true;
        }
        try
        {
            System.Diagnostics.Process.Start("explorer.exe", (openInsidesOfFolder ? "/root," : "/select,") + winPath);
        }
        catch (System.ComponentModel.Win32Exception e)
        {
            // tried to open win explorer in mac
            // just silently skip error
            // we currently have no platform define for the current OS we are in, so we resort to this
            e.HelpLink = ""; // do anything with this variable to silence warning about not using it
        }
    }

    public static void OpenInFileBrowser(string path)
    {
        OpenInWinFileBrowser(path);
        OpenInMacFileBrowser(path);
    }
    public static Texture2D CopyTextures(Texture2D aBaseTexture)
    {
        int aWidth = aBaseTexture.width;
        int aHeight = aBaseTexture.height;

        Texture2D aReturnTexture = new Texture2D(aWidth, aHeight, TextureFormat.RGBA32, false);

        UnityEngine.Color[] aBaseTexturePixels = aBaseTexture.GetPixels();
        aReturnTexture.SetPixels(aBaseTexturePixels);
        aReturnTexture.Apply(false);

        return aReturnTexture;
    }
    public static void SetLightmapReadable()
    {
        if (LightmapSettings.lightmaps != null)
        {
            int count = LightmapSettings.lightmaps.Length;
            for (int index = 0; index < count; index++)
            {
                Texture2D tex = LightmapSettings.lightmaps[index].lightmapColor;
                string path = AssetDatabase.GetAssetPath(tex);
                TextureImporter importor = (TextureImporter)AssetImporter.GetAtPath(path);
                importor.isReadable = true;
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
        }
    }
    public static void ShowHeader(string label, float width)
    {
        GUIStyle headerStyle = new GUIStyle();
        headerStyle.alignment = TextAnchor.MiddleLeft;
        headerStyle.fontSize = 14;
        headerStyle.normal.textColor = UnityEngine.Color.gray;
        headerStyle.fontStyle = FontStyle.Bold;
        GUILayout.Box(label, headerStyle, GUILayout.Width(width));
    }
    public static bool IsSameSide(UnityEngine.Vector3 A, UnityEngine.Vector3 B, UnityEngine.Vector3 C, UnityEngine.Vector3 P)
    {
        UnityEngine.Vector3 AB = B - A;
        UnityEngine.Vector3 AC = C - A;
        UnityEngine.Vector3 AP = P - A;

        UnityEngine.Vector3 v1 = UnityEngine.Vector3.Cross(AB, AC);
        UnityEngine.Vector3 v2 = UnityEngine.Vector3.Cross(AB, AP);
        return UnityEngine.Vector3.Dot(v1, v2) >= 0;
    }
    public static bool IsPointInTriangle(UnityEngine.Vector3 P, UnityEngine.Vector3 A, UnityEngine.Vector3 B, UnityEngine.Vector3 C)
    {
        return IsSameSide(A, B, C, P) &&
            IsSameSide(B, C, A, P) &&
            IsSameSide(C, A, B, P);
    }
    public static bool IsPointInTriangle(UnityEngine.Vector2 P, UnityEngine.Vector2 A, UnityEngine.Vector2 B, UnityEngine.Vector2 C)
    {
        UnityEngine.Vector3 P2D = new UnityEngine.Vector3(P.x, P.y, 0);
        UnityEngine.Vector3 A2D = new UnityEngine.Vector3(A.x, A.y, 0);
        UnityEngine.Vector3 B2D = new UnityEngine.Vector3(B.x, B.y, 0);
        UnityEngine.Vector3 C2D = new UnityEngine.Vector3(C.x, C.y, 0);
        return IsSameSide(A2D, B2D, C2D, P2D) &&
            IsSameSide(B2D, C2D, A2D, P2D) &&
            IsSameSide(C2D, A2D, B2D, P2D);
    }
    public static bool IsRectEqual(Rect lhs, Rect rhs)
    {
        if (lhs == rhs)
        {
            return true;
        }
        if (UnityEngine.Mathf.Abs(lhs.xMin - rhs.xMin) < float.Epsilon
          && UnityEngine.Mathf.Abs(lhs.yMin - rhs.yMin) < float.Epsilon
          && UnityEngine.Mathf.Abs(lhs.xMax - rhs.xMax) < float.Epsilon
          && UnityEngine.Mathf.Abs(lhs.yMax - rhs.yMax) < float.Epsilon)
        {
            return true;
        }
        return false;
    }
}
