using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace XOpt
{
    public class XOCp_OptimizerHandler : UnityEngine.MonoBehaviour
    {
        [HideInInspector]
        public Dictionary<int, OriginItermedia> p2O = new Dictionary<int, OriginItermedia>();
        public List<UnityEngine.GameObject> olst = new List<UnityEngine.GameObject>();
        public List<UnityEngine.GameObject> nlst = new List<UnityEngine.GameObject>();

        [HideInInspector]
        public Dictionary<string, Dictionary<string, List<UnityEngine.GameObject>>> s2o = new Dictionary<string, Dictionary<string, List<UnityEngine.GameObject>>>();

        public bool bDisplayOrigin = false;
        public bool bDisplayNew = true;


        public UnityEngine.GameObject Root = XOGloble.Root;
        public UnityEngine.GameObject IterM = XOGloble.IterM;
        public UnityEngine.GameObject Output = XOGloble.Output;


        public void AddObjLstByShaderMaterial(UnityEngine.GameObject newobj)
        {
            if (s2o != null)
            {
                MeshRenderer lMeshRenderer = newobj.GetComponent<MeshRenderer>();
                if (lMeshRenderer)
                {
                    string shadername = lMeshRenderer.sharedMaterial.shader.name;
                    string materialname = lMeshRenderer.sharedMaterial.name;

                    if (s2o.ContainsKey(shadername) == true)
                    {
                        if (s2o[shadername].ContainsKey(materialname) == true)
                        {
                            s2o[shadername][materialname].Add(newobj);
                        }
                        else
                        {
                            List<UnityEngine.GameObject> newlist = new List<UnityEngine.GameObject>();
                            newlist.Add(newobj);
                            s2o[shadername].Add(materialname, newlist);
                        }
                    }
                    else
                    {
                        List<UnityEngine.GameObject> newlist = new List<UnityEngine.GameObject>();
                        newlist.Add(newobj);

                        Dictionary<string, List<UnityEngine.GameObject>> newdic = new Dictionary<string, List<UnityEngine.GameObject>>();
                        newdic.Add(materialname, newlist);

                        s2o.Add(shadername, newdic);
                    }
                }
            }
        }

        public bool CanMerge(UnityEngine.GameObject go)
        {
            bool bret = true;

            MeshFilter lMeshFilter = go.GetComponent<MeshFilter>();
            if (lMeshFilter != null)
            {
                Mesh lMesh = lMeshFilter.sharedMesh;
                UnityEngine.Vector2[] _uvOffset = lMesh.uv;

                for (int uv = 0; uv < _uvOffset.Length; uv++)
                {
                    //Debug.Log("[uv].x = " + _uvOffset[uv].x + " [uv].y = " + _uvOffset[uv].y);
                    if (_uvOffset[uv].x > 1.0f)
                    {
                        bret = false;
                        break;
                    }
                    if (_uvOffset[uv].x < 0.0f)
                    {
                        bret = false;
                        break;
                    }
                    if (_uvOffset[uv].y > 1.0f)
                    {
                        bret = false;
                        break;
                    }
                    if (_uvOffset[uv].y < 0.0f)
                    {
                        bret = false;
                        break;
                    }
                }
                return bret;
            }
            return false;
        }

        public void CreateNodes()
        {
            foreach (string shaderkey in s2o.Keys)
            {
                UnityEngine.GameObject newshaderroot = new UnityEngine.GameObject(shaderkey);
                newshaderroot.isStatic = true;
                UnityEngine.GameObject newshaderroot2 = new UnityEngine.GameObject(shaderkey);
                newshaderroot2.isStatic = true;

                if (newshaderroot != null)
                {
                    newshaderroot.transform.position = UnityEngine.Vector3.zero;
                    newshaderroot.transform.rotation = UnityEngine.Quaternion.identity;
                    newshaderroot.transform.localScale = UnityEngine.Vector3.one;
                    newshaderroot.transform.parent = XOGloble.Output.transform;

                    newshaderroot2.transform.position = UnityEngine.Vector3.zero;
                    newshaderroot2.transform.rotation = UnityEngine.Quaternion.identity;
                    newshaderroot2.transform.localScale = UnityEngine.Vector3.one;
                    newshaderroot2.transform.parent = XOGloble.IterM.transform;

                    int height = 0;
                    int width = 0;
                    Texture2D _maintxt = null;

                    foreach (string materialkey in s2o[shaderkey].Keys)
                    {
                        List<UnityEngine.GameObject> tlst = s2o[shaderkey][materialkey];
                        if (tlst != null)
                        {
                            UnityEngine.GameObject go = tlst[0];
                            if (go != null)
                            {
                                MeshRenderer lMergedMeshRenderer = go.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
                                _maintxt = lMergedMeshRenderer.sharedMaterial.mainTexture as Texture2D;
                                if (_maintxt != null)
                                {
                                    height = _maintxt.height;
                                    width = _maintxt.width;
                                }
                            }
                        }

                        string nodename = height.ToString() + "X" + width.ToString() + "-" + materialkey;
                        UnityEngine.GameObject newmaterialroot = new UnityEngine.GameObject(nodename);
                        newmaterialroot.isStatic = true;
                        UnityEngine.GameObject newmaterialroot2 = new UnityEngine.GameObject(nodename);
                        newmaterialroot2.isStatic = true;

                        if (newmaterialroot != null)
                        {
                            newmaterialroot.transform.position = UnityEngine.Vector3.zero;
                            newmaterialroot.transform.rotation = UnityEngine.Quaternion.identity;
                            newmaterialroot.transform.localScale = UnityEngine.Vector3.one;
                            newmaterialroot.transform.parent = newshaderroot.transform;
                            XOCp_MaterialRoot comp = newmaterialroot.AddComponent<XOCp_MaterialRoot>();
                            if (comp != null)
                            {
                                comp.maintxt = _maintxt;
                            }

                            newmaterialroot2.transform.position = UnityEngine.Vector3.zero;
                            newmaterialroot2.transform.rotation = UnityEngine.Quaternion.identity;
                            newmaterialroot2.transform.localScale = UnityEngine.Vector3.one;
                            newmaterialroot2.transform.parent = newshaderroot2.transform;

                            List<UnityEngine.GameObject> lst = s2o[shaderkey][materialkey];
                            foreach (UnityEngine.GameObject go in lst)
                            {
                                //if(CanMerge(go))
                                //{
                                go.transform.parent = newmaterialroot.transform;
                                //}
                                //else
                                //{
                                // 	go.transform.parent = newmaterialroot2.transform;
                                //}
                            }
                        }
                    }
                }
            }

            ClearUnusedNodes(XOGloble.IterM);
            ClearUnusedNodes(XOGloble.Output);
        }

        public void ClearUnusedNodes(UnityEngine.GameObject goroot)
        {
            List<UnityEngine.GameObject> DelShaderObjLst = new List<UnityEngine.GameObject>();

            for (int i = 0; i < goroot.transform.childCount; i++)
            {
                UnityEngine.Transform shadertrans = goroot.transform.GetChild(i);
                if (shadertrans.childCount > 0)
                {
                    List<UnityEngine.GameObject> DelMaterialObjLst = new List<UnityEngine.GameObject>();
                    for (int j = 0; j < shadertrans.childCount; j++)
                    {
                        UnityEngine.Transform materialtrans = shadertrans.GetChild(j);
                        if (materialtrans.childCount == 0)
                        {
                            DelMaterialObjLst.Add(materialtrans.gameObject);
                        }
                    }

                    foreach (UnityEngine.GameObject go in DelMaterialObjLst)
                    {
                        go.transform.parent = null;
                        UnityEngine.GameObject.DestroyImmediate(go);
                    }

                    DelMaterialObjLst.Clear();
                }

                if (shadertrans.childCount == 0)
                {
                    DelShaderObjLst.Add(shadertrans.gameObject);
                }
            }

            foreach (UnityEngine.GameObject go in DelShaderObjLst)
            {
                go.transform.parent = null;
                UnityEngine.GameObject.DestroyImmediate(go);
            }
            DelShaderObjLst.Clear();
        }


        public bool bShowOrigin = true;

    }
}