using UnityEngine;
using System.Collections;
using ArkCrossEngine;

public class ReliveCtrl
{


    public void Init(UnityEngine.GameObject father)
    {
        m_FatherGo = father;
    }
    public void ShowReliveUi()
    {
        UnityEngine.GameObject go = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource("UI/Relive"));
        if (null != go)
        {
            go = NGUITools.AddChild(m_FatherGo, go);
        }
    }

    static private ReliveCtrl m_Instance = new ReliveCtrl();
    static public ReliveCtrl Instance
    {
        get { return m_Instance; }
    }
    private UnityEngine.GameObject m_FatherGo = null;

}
