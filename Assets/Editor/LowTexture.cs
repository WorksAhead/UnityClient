using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class LowTexture
{
    static bool run = false;
    static string strPlatform = "Android";
    static bool both = false;
    static int beginIndex = UnityEngine.Application.dataPath.Length - 6;

    [MenuItem("Window/lowTexture/android")]
    public static void doLowTextrueAndroid()
    {
        both = false;
        run = true;
        Debug.Log("lowTexture begin");
        UnityEngine.Object[] SelectedAsset = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        foreach (UnityEngine.Object obj in SelectedAsset)
        {
            string sourcePath = AssetDatabase.GetAssetPath(obj);
            string fullPath = UnityEngine.Application.dataPath + sourcePath.Substring(6);
            doLowTextrue(fullPath);
        }
        EditorUtility.ClearProgressBar();
        Debug.Log("lowTexture ok");
    }

    [MenuItem("Window/lowTexture/androidandpc")]
    public static void doLowTextrueAndroidAndPC()
    {
        both = true;
        run = true;
        Debug.Log("lowTexture begin");
        UnityEngine.Object[] SelectedAsset = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        foreach (UnityEngine.Object obj in SelectedAsset)
        {
            string sourcePath = AssetDatabase.GetAssetPath(obj);
            string fullPath = UnityEngine.Application.dataPath + sourcePath.Substring(6);
            doLowTextrue(fullPath);
        }
        EditorUtility.ClearProgressBar();
        Debug.Log("lowTexture ok");
    }



    enum RetType
    {
        Error = -1,
        Ok = 0,
        Ignore = 1,
    }

    public static void doLowTextrue(string path)
    {
        run = true;
        Debug.Log("lowTexture begin:" + path);
        if (File.Exists(path))
        {
            FileInfo fi = new FileInfo(path);
            handleFile(fi);
        }
        else
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            enumResource(dir);
        }
        EditorUtility.ClearProgressBar();
        Debug.Log("lowTexture ok");
    }
    public static void doScriptLowTextrue()
    {
        string[] dirList = new string[] {
      "Scenes",
      "Brush",
      "Character/Monster",
      "Character/Npc",
      "Content",
      "Effect",
      "Resources",
    };

        both = false;
        run = true;
        Debug.Log("lowTexture begin");
        foreach (string curDir in dirList)
        {
            string curDirAbs = UnityEngine.Application.dataPath + "/" + curDir;
            //Debug.Log("doLowTextrue curDirAbs:" + curDirAbs);
            doLowTextrue(curDirAbs);
        }
        Debug.Log("lowTexture ok");
    }
    static void enumResource(DirectoryInfo dir)
    {
        FileInfo[] fi = dir.GetFiles();
        DirectoryInfo[] di = dir.GetDirectories();
        for (int i = 0; i < fi.Length && run; i++)
        {
            handleFile(fi[i]);
        }
        for (int i = 0; i < di.Length && run; i++)
        {
            enumResource(di[i]);
        }
    }

    static void handleFile(FileInfo fi)
    {
        //fn should begin at Assets\,and has ext
        string fp = fi.FullName.Substring(beginIndex);
        run = !EditorUtility.DisplayCancelableProgressBar("lowTexture", fp, 0);
        RetType ret = RetType.Ok;
        switch (fi.Extension.ToLower())
        {
            case ".png":
            case ".jpg":
            case ".tga":
            case ".exr":
                ret = handleTexture(fp);
                break;
            case ".prefab":
                ret = handlePrefab(fp);
                break;
        };
        if (ret == RetType.Error) Debug.LogError("low quality failed:" + fp);
    }

    static RetType handleTexture(string assetPath)
    {
        Texture2D t = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D)) as Texture2D;
        if (null == t)
        {
            return RetType.Error;
        }
        TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        //已经缩放到0.5/
        if (textureImporter.userData == "low")
        {
            Debug.Log("!!Ignore already == low");
            return RetType.Ignore;
        }
        //平台差异设置/
        int maxTextSize = 0;
        TextureImporterFormat tf = new TextureImporterFormat();
        bool ret = textureImporter.GetPlatformTextureSettings(strPlatform, out maxTextSize, out tf);

        int max = 0;
        if (!ret)
        {
            //用default设置
            max = textureImporter.maxTextureSize;
            tf = textureImporter.textureFormat;
        }
        else
        {
            max = t.width > t.height ? t.width : t.height;
        }

        //图片大小缩小1/2/
        if (max == 0)
        {
            Debug.Log("!!Error max==0");
            return RetType.Error;
        }
        if (max <= 32 || maxTextSize <= 32)
        {
            Debug.Log("!!Ignore max<=32 || maxTextSize<=32");
            return RetType.Ignore;
        }
        int a = maxTextSize;
        while (a >= max) a = a >> 1;
        if (both)
        {
            textureImporter.SetPlatformTextureSettings("Android", a, tf);
            textureImporter.SetPlatformTextureSettings("Standalone", a, tf);
        }
        else
        {
            textureImporter.SetPlatformTextureSettings("Android", a, tf);
        }
        textureImporter.userData = "low";
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        return RetType.Ok;
    }

    static RetType handlePrefab(string assetPath)
    {
        UIAtlas atlas = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UIAtlas)) as UIAtlas;
        if (null == atlas) return RetType.Ignore;
        if (null == atlas.spriteMaterial || null == atlas.spriteMaterial.mainTexture) return RetType.Error;
        string path = AssetDatabase.GetAssetPath(atlas.spriteMaterial.mainTexture);
        //缩放关联的texture/
        RetType ret = handleTexture(path);
        // NGUI update issue
        //if (atlas.scale != 1f) return RetType.Ignore;
        //atlas.scale = 0.5f;
        throw new System.Exception("NGUI update issue, fix me.");
        //缩放图集坐标/
        BetterList<string> sl = atlas.GetListOfSprites();
        if (sl == null) return RetType.Error;
        foreach (string sn in sl)
        {
            UISpriteData sd = atlas.GetSprite(sn);
            sd.x /= 2;
            sd.y /= 2;
            sd.width /= 2;
            sd.height /= 2;

            sd.borderBottom /= 2;
            sd.borderLeft /= 2;
            sd.borderRight /= 2;
            sd.borderTop /= 2;

            sd.paddingTop /= 2;
            sd.paddingBottom /= 2;
            sd.paddingLeft /= 2;
            sd.paddingRight /= 2;
        }
        atlas.pixelSize *= 2;
        atlas.MarkAsChanged();
        return ret;
    }
}
