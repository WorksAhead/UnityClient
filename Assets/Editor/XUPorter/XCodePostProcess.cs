using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;
#endif
using System.IO;

public static class XCodePostProcess
{

#if UNITY_EDITOR
    private static string s_ChannelName = "cyou";

    [PostProcessBuild(999)]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target != BuildTarget.iOS)
        {
            Debug.LogWarning("Target is not iPhone. XCodePostProcess will not run");
            return;
        }

        //�õ�xcode���̵�·��
        string path = Path.GetFullPath(pathToBuiltProject);

        // Create a new project object from build target
        XCProject project = new XCProject(pathToBuiltProject);
        XCTessarProcess.OnProcess(project, path, s_ChannelName);
        XCGTProcess.OnProcess(project, path, s_ChannelName);
        switch (s_ChannelName)
        {
            case "uc":
                XCUCProcess.OnProcess(project, path, s_ChannelName);
                break;
            default:
                XCCYouProcess.OnProcess(project, path, s_ChannelName);
                break;
        }
        project.Save();
    }
    public static void RegisterChannelName(string channel)
    {
        s_ChannelName = channel;
    }
#endif
}
