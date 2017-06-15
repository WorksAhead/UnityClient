using System.Collections.Generic;
using ArkCrossEngine;

public class NewbieGuideManager : UnityEngine.MonoBehaviour
{
    private object eo = null;
    void Start()
    {
        try
        {
            eo = ArkCrossEngine.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void UnSubscribe()
    {
        try
        {
            if (eo != null)
            {
                ArkCrossEngine.LogicSystem.EventChannelForGfx.Unsubscribe(eo);
            }
            GuidEnd();
            if (ngm != null)
            {
                NGUITools.DestroyImmediate(ngm);
            }
        }
        catch (System.Exception ex)
        {
            ArkCrossEngine.LogicSystem.LogicLog("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void SetMySelf(NewbieGuideManager myself, UnityEngine.Transform tf)
    {
        ngm = myself;
        uiRootTF = tf;
    }

    public void DoInitGuid(List<int> list)
    {
        if (list == null) return;
        int count = list.Count;
        for (int i = 0; i < count; ++i)
        {
            int id = list[i];
            ArkCrossEngine.NewbieGuideConfig ng = ArkCrossEngine.NewbieGuideProvider.Instance.GetDataById(id);
            if (ng != null && uiRootTF != null)
            {
                switch (ng.m_Type)
                {
                    case 1:
                        {
                            UnityEngine.Transform tf = uiRootTF.Find(ng.m_TargetChildPath);
                            if (tf != null)
                            {
                                UnityEngine.GameObject go = ResourceSystem.GetSharedResource(ng.m_GuideUiPath) as UnityEngine.GameObject;
                                if (null != go)
                                {
                                    go = NGUITools.AddChild(tf.gameObject, go);
                                    if (go != null)
                                    {
                                        go.transform.rotation = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(ng.m_RotateThree[0], ng.m_RotateThree[1], ng.m_RotateThree[2]));
                                        go.transform.localPosition = new UnityEngine.Vector3(ng.m_LocalPosition[0], ng.m_LocalPosition[1], ng.m_LocalPosition[2]);
                                        go.transform.localScale = new UnityEngine.Vector3(ng.m_Scale[0], ng.m_Scale[1], ng.m_Scale[2]);
                                        NGUITools.SetActive(go, ng.m_Visible);
                                        GOInfo gi = new GOInfo(id, tf.gameObject, go);
                                        idFindGo.Add(id, gi);
                                        GoFindid.Add(tf.gameObject, id);
                                        idList.Add(id);
                                    }
                                }
                                tf.gameObject.AddComponent<UIEventListener>();
                                UIEventListener.Get(tf.gameObject).onClick += ResponseEvent;
                            }
                        }
                        break;
                    case 2:
                        {
                            UnityEngine.Transform tf = uiRootTF.Find(ng.m_TargetChildPath);
                            if (tf != null)
                            {
                                GoFindIsCloseid.Add(tf.gameObject, id);
                                tf.gameObject.AddComponent<UIEventListener>();
                                UIEventListener.Get(tf.gameObject).onClick += CloseEvent;
                            }
                        }
                        break;
                    case 3:
                        {
                            UnityEngine.Transform tf = uiRootTF.Find(ng.m_TargetChildPath);
                            if (tf != null)
                            {
                                NGUITools.SetActive(tf.gameObject, ng.m_Visible);
                            }
                        }
                        break;
                    case 4:
                        {
                            UnityEngine.Transform tf = uiRootTF.Find(ng.m_TargetChildPath);
                            if (tf != null)
                            {
                                if (ng.m_ChildNumber >= 0 && ng.m_ChildNumber < tf.childCount)
                                {
                                    tf = tf.GetChild(ng.m_ChildNumber);
                                    if (tf != null)
                                    {
                                        UnityEngine.GameObject go = ResourceSystem.GetSharedResource(ng.m_GuideUiPath) as UnityEngine.GameObject;
                                        if (null != go)
                                        {
                                            go = NGUITools.AddChild(tf.gameObject, go);
                                            if (go != null)
                                            {
                                                go.transform.rotation = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(ng.m_RotateThree[0], ng.m_RotateThree[1], ng.m_RotateThree[2]));
                                                go.transform.localPosition = new UnityEngine.Vector3(ng.m_LocalPosition[0], ng.m_LocalPosition[1], ng.m_LocalPosition[2]);
                                                NGUITools.SetActive(go, ng.m_Visible);
                                                GOInfo gi = new GOInfo(id, tf.gameObject, go);
                                                idFindGo.Add(id, gi);
                                                GoFindid.Add(tf.gameObject, id);
                                                idList.Add(id);
                                            }
                                        }
                                        tf.gameObject.AddComponent<UIEventListener>();
                                        UIEventListener.Get(tf.gameObject).onClick += ResponseEvent;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }
    }

    private void ResponseEvent(UnityEngine.GameObject go)
    {
        if (go != null)
        {
            if (GoFindid.ContainsKey(go))
            {
                int id = GoFindid[go];
                if (id == idList[idList.Count - 1])
                {
                    //告知结束系列任务
                    GuidEnd();
                    ArkCrossEngine.GfxSystem.EventChannelForLogic.Publish("ge_newbie_end", "lobby");
                    NGUITools.DestroyImmediate(ngm);
                }
                else
                {
                    MakeMeAndFrontMeClickAndShowNext(id);
                }
            }
        }
    }
    private void MakeMeAndFrontMeClickAndShowNext(int id)
    {
        int count = idList.Count;
        bool equalid = false;
        for (int j = 0; j < count; ++j)
        {
            int i = idList[j];
            if (equalid)
            {
                if (idFindGo.ContainsKey(i))
                {
                    GOInfo gi = idFindGo[i];
                    if (gi != null)
                    {
                        ArkCrossEngine.NewbieGuideConfig ng = ArkCrossEngine.NewbieGuideProvider.Instance.GetDataById(gi.id);
                        if (ng != null)
                        {
                            NGUITools.SetActive(gi.son, ng.m_Visible);
                        }
                    }
                }
                continue;
            }
            if (i != id)
            {
                if (idFindGo.ContainsKey(i))
                {
                    GOInfo gi = idFindGo[i];
                    if (gi != null)
                    {
                        NGUITools.SetActive(gi.son, false);
                    }
                }
            }
            else
            {
                if (idFindGo.ContainsKey(i))
                {
                    GOInfo gi = idFindGo[i];
                    if (gi != null)
                    {
                        NGUITools.SetActive(gi.son, false);
                    }
                }
                if (j + 1 < count)
                {
                    int m = idList[j + 1];
                    if (idFindGo.ContainsKey(m))
                    {
                        GOInfo gi = idFindGo[m];
                        if (gi != null)
                        {
                            NGUITools.SetActive(gi.son, true);
                        }
                    }
                    ++j;
                    equalid = true;
                }
            }
        }
    }
    private void GuidEnd()
    {
        foreach (int id in idFindGo.Keys)
        {
            GOInfo gi = idFindGo[id];
            if (gi != null)
            {
                if (gi.myself != null)
                {
                    UIEventListener.Get(gi.myself).onClick -= ResponseEvent;
                    NGUITools.DestroyImmediate(gi.son);
                }
            }
        }
        idList.Clear();
        idFindGo.Clear();
        GoFindid.Clear();

        foreach (UnityEngine.GameObject go in GoFindIsCloseid.Keys)
        {
            UIEventListener.Get(go).onClick -= CloseEvent;
        }
        GoFindIsCloseid.Clear();
    }
    private void CloseEvent(UnityEngine.GameObject go)
    {
        if (go != null && GoFindIsCloseid.ContainsKey(go))
        {
            int id = GoFindIsCloseid[go];
            ArkCrossEngine.NewbieGuideConfig ng = ArkCrossEngine.NewbieGuideProvider.Instance.GetDataById(id);
            if (ng != null)
            {
                id = ng.m_PreviousGuideId;
                bool sign = false;
                foreach (int lid in idList)
                {
                    if (sign)
                    {
                        ng = ArkCrossEngine.NewbieGuideProvider.Instance.GetDataById(lid);
                        if (ng != null)
                        {
                            if (idFindGo.ContainsKey(lid))
                            {
                                GOInfo gi = idFindGo[lid];
                                if (gi != null && gi.son != null)
                                {
                                    NGUITools.SetActive(gi.son, ng.m_Visible);
                                }
                            }
                        }
                    }
                    if (lid == id)
                    {
                        if (idFindGo.ContainsKey(id))
                        {
                            GOInfo gi = idFindGo[id];
                            if (gi != null && gi.son != null)
                            {
                                NGUITools.SetActive(gi.son, true);
                            }
                        }
                        sign = true;
                    }
                }
            }
        }
    }
    private List<int> idList = new List<int>();
    private Dictionary<int, GOInfo> idFindGo = new Dictionary<int, GOInfo>();
    private Dictionary<UnityEngine.GameObject, int> GoFindid = new Dictionary<UnityEngine.GameObject, int>();
    private Dictionary<UnityEngine.GameObject, int> GoFindIsCloseid = new Dictionary<UnityEngine.GameObject, int>();
    private UnityEngine.Transform uiRootTF = null;

    private NewbieGuideManager ngm = null;
}
class GOInfo
{
    public GOInfo(int gid, UnityEngine.GameObject mygo, UnityEngine.GameObject go)
    {
        id = gid;
        myself = mygo;
        son = go;
    }
    public int id;
    public UnityEngine.GameObject myself = null;
    public UnityEngine.GameObject son = null;
}