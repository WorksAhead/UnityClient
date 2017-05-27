using UnityEngine;
using System.Collections;

public class WindowInfo
{
    public string windowName = "";
    public string windowPath = "";
    public UnityEngine.Vector3 windowPos = new UnityEngine.Vector3();
    public UILoadType windowType = UILoadType.DontLoad;
    public bool isCenter = false;
    public int sceneId = 0;
}