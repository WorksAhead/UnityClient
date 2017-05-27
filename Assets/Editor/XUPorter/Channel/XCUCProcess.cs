using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;
#endif
using System.IO;

public static class XCUCProcess
{
#if UNITY_EDITOR
    public static void OnProcess(XCProject project, string projPath, string channelName)
    {

    }
    private static void EditorPlist(string filePath, string channelName)
    {

    }
    private static void EditorCode(string filePath)
    {
    }
#endif
}
