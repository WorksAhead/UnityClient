using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;
#endif
using System.IO;

public static class XCCYouProcess
{
#if UNITY_EDITOR
    public static void OnProcess(XCProject project, string projPath, string channelName)
    {
        project.ApplyMod(Path.Combine(UnityEngine.Application.dataPath, "Editor/XUPorter/Mods/CYMGSDK.projmods"));
        EditorPlist(projPath, channelName);
        EditorCode(projPath);
    }
    private static void EditorPlist(string filePath, string channelName)
    {
        XCPlist list = new XCPlist(filePath);
        string PlistAdd = @"  
            <key>CFBundleURLTypes</key>
            <array>
              <dict>
                <key>CFBundleURLSchemes</key>
                  <array>
                    <string>cy1407921103977</string>
                  </array>
                </dict>            
            </array>";
        list.AddKey(PlistAdd);
        list.Save();
    }

    private static void EditorCode(string filePath)
    {
        //读取UnityAppController.mm文件
        XClass UnityAppController = new XClass(filePath + "/Classes/UnityAppController.mm");
        //添加代码解决键盘语音输入问题
        UnityAppController.WriteAbove("if(_didResignActive)", "\tEAGLContextSetCurrentAutoRestore autoRestore(_mainDisplay->surface.context);\n\t\t");
        //添加MBI初始化代码
        UnityAppController.WriteBelow("#include \"Unity/GlesHelper.h\"", "#import \"Cylib.h\"");
        string appKey = "1407921103977";
        string channelID = "1010802002";
        string sentence = string.Format("[Cylib onStart:@\"{0}\" withChannelID:@\"{1}\"];", appKey, channelID);
        UnityAppController.WriteAbove("return NO;", sentence);
    }
#endif
}
