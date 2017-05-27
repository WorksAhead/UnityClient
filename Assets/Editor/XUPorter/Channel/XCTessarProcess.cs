using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;
#endif
using System.IO;
using System.Text;

public static class XCTessarProcess
{
    public static void OnProcess(XCProject project, string projPath, string channelName)
    {
        project.ApplyMod(Path.Combine(UnityEngine.Application.dataPath, "Editor/XUPorter/Mods/Tessar.projmods"));
        EditorPlist(projPath, channelName);
        EditorCode(projPath);
    }
    private static void EditorPlist(string filePath, string channelName)
    {
        if (!string.IsNullOrEmpty(channelName))
        {
            XCPlist list = new XCPlist(filePath);
            string PlistAdd = @"  
            <key>DFMBundleChannel</key>
            <string>" + channelName + @"</string>";
            list.AddKey(PlistAdd);
            list.Save();
        }
    }
    private static void EditorCode(string filePath)
    {
        XClass UnityAppController = new XClass(filePath + "/Classes/UnityAppController.mm");

        StringBuilder sbRegisterCode = new StringBuilder();
        sbRegisterCode.AppendLine("NSDictionary *appInfo = [[NSBundle mainBundle] infoDictionary];");
        sbRegisterCode.AppendLine("NSString *appVersion = [NSString stringWithFormat:@\"%@ (%@)\", appInfo[@\"CFBundleShortVersionString\"], appInfo[@\"CFBundleVersion\"]];");
        sbRegisterCode.AppendLine("NSString *appChannel = [NSString stringWithFormat:@\"%@\", appInfo[@\"DFMBundleChannel\"]];");
        sbRegisterCode.AppendLine("[TessarMobileSDK proj:@\"dashfire\" channel:appChannel version:appVersion];");

        string strRegisterPosingCode = "	[self performSelector:@selector(startUnity:) withObject:application afterDelay:0];";
        UnityAppController.WriteBelow(strRegisterPosingCode, sbRegisterCode.ToString());

        string sbIncludeCode = "#import \"TessarMobileSDK.h\"";
        string strIncludePosingCode = "#include \"Unity/GlesHelper.h\"";
        UnityAppController.WriteBelow(strIncludePosingCode, sbIncludeCode);
    }
}
