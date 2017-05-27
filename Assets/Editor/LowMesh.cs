using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class LowMesh
{
    static bool run = false;
    static string strPlatform = "Android";
    static int beginIndex = UnityEngine.Application.dataPath.Length - 6;

    [MenuItem("Window/LowMesh/android/LowSelected")]
    public static void doLowMeshAndroid()
    {
        run = true;
        Debug.Log("LowMesh begin");
        UnityEngine.Object[] SelectedAsset = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        foreach (UnityEngine.Object obj in SelectedAsset)
        {
            string sourcePath = AssetDatabase.GetAssetPath(obj);
            string fullPath = UnityEngine.Application.dataPath + sourcePath.Substring(6);
            doLowMesh(fullPath, ModelImporterMeshCompression.Low);
        }
        EditorUtility.ClearProgressBar();
        Debug.Log("LowMesh ok");
    }

    [MenuItem("Window/LowMesh/android/LowAll")]
    public static void doLowAllMeshAndroid()
    {
        run = true;
        Debug.Log("LowMesh begin");
        doScriptLowMesh(ModelImporterMeshCompression.Low);
        Debug.Log("LowMesh ok");
    }

    enum RetType
    {
        Error = -1,
        Ok = 0,
        Ignore = 1,
    }

    public static void doLowMesh(string path, ModelImporterMeshCompression compression)
    {
        run = true;
        Debug.Log("LowMesh begin:" + path);
        if (File.Exists(path))
        {
            FileInfo fi = new FileInfo(path);
            handleFile(fi, compression);
        }
        else
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            enumResource(dir, compression);
        }
        EditorUtility.ClearProgressBar();
        Debug.Log("LowMesh ok");
    }
    public static void doScriptLowMesh(ModelImporterMeshCompression compression)
    {
        string[] dirList = new string[] {
      "Character",
      "Content",
      "Effect",
      "Scenes",
      "FastShadows",
      "Standard Assets",
    };

        run = true;
        Debug.Log("LowMesh begin");
        foreach (string curDir in dirList)
        {
            string curDirAbs = UnityEngine.Application.dataPath + "/" + curDir;
            //Debug.Log("doLowMesh curDirAbs:" + curDirAbs);
            doLowMesh(curDirAbs, compression);
        }
        Debug.Log("LowMesh ok");
    }
    static void enumResource(DirectoryInfo dir, ModelImporterMeshCompression compression)
    {
        FileInfo[] fi = dir.GetFiles();
        DirectoryInfo[] di = dir.GetDirectories();
        for (int i = 0; i < fi.Length && run; i++)
        {
            handleFile(fi[i], compression);
        }
        for (int i = 0; i < di.Length && run; i++)
        {
            enumResource(di[i], compression);
        }
    }

    static void handleFile(FileInfo fi, ModelImporterMeshCompression compression)
    {
        Debug.Log("LowMesh handleFile begin");
        //fn should begin at Assets\,and has ext
        string fp = fi.FullName.Substring(beginIndex);
        run = !EditorUtility.DisplayCancelableProgressBar("lowMesh", fp, 0);
        RetType ret = RetType.Ok;

        Debug.Log("LowMesh handleFile ToLower = " + fi.Extension.ToLower());
        switch (fi.Extension.ToLower())
        {
            case ".fbx":
                ret = handleMesh(fp, compression);
                break;
        };
        Debug.Log("LowMesh handleFile end");
        if (ret == RetType.Error) Debug.LogError("low quality failed:" + fp);
    }

    static RetType handleMesh(string assetPath, ModelImporterMeshCompression compression)
    {
        Debug.Log("LowMesh handleMesh begin");
        ModelImporter meshImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;
        if (null == meshImporter)
        {
            Debug.Log("LowMesh handleMesh Error");
            return RetType.Error;
        }
        meshImporter.meshCompression = compression;
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        Debug.Log("LowMesh handleMesh Ok");
        return RetType.Ok;
    }
}
