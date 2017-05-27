using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;
#endif
using System.IO;

public static class XCGTProcess
{
    public static void OnProcess(XCProject project, string projPath, string channelName)
    {
        //CYGT
        //project.ApplyMod(Path.Combine(UnityEngine.Application.dataPath, "Editor/XUPorter/Mods/CYGTSDK.projmods"));
        //EditorPlist(projPath, channelName);
        //EditorCode(projPath);
    }
    private static void EditorPlist(string filePath, string channelName)
    {

    }

    private static void EditorCode(string filePath)
    {
    }
}
