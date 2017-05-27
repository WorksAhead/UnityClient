using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;

namespace XOpt
{
    public class XOCp_ResourceHolder : UnityEngine.MonoBehaviour
    {
        //把场景资源摘出来存放在数组里供逻辑使用
        static public UnityEngine.GameObject inst = null;

        //场景中的特效数组
        public UnityEngine.GameObject[] m_ParticleArray = null;

        //创建根节点
        public static void CreateRoot()
        {
            UnityEngine.GameObject or = UnityEngine.GameObject.Find("ResourceHolder");
            if (or != null)
            {
                UnityEngine.GameObject.DestroyImmediate(or);
            }

            inst = new UnityEngine.GameObject("ResourceHolder");
            inst.isStatic = true;
            inst.transform.position = UnityEngine.Vector3.zero;
            inst.AddComponent<XOCp_ResourceHolder>();
        }

        public static void SceneParticleMark()
        {
            XOCp_ResourceHolder.CreateRoot();
            XOCp_ResourceHolder.inst.GetComponent<XOCp_ResourceHolder>().BuildParticles();
        }

        public static bool IsParticleDetailed(UnityEngine.GameObject go)
        {
            if (go != null)
            {
                int dlayer = LayerMask.NameToLayer("Detail");
                if (go.layer == dlayer)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public void BuildParticles()
        {
            ParticleSystem[] ps = (ParticleSystem[])FindObjectsOfType(typeof(ParticleSystem));
            XOCp_ResourceHolder xr = XOCp_ResourceHolder.inst.GetComponent<XOCp_ResourceHolder>();
            if (xr != null)
            {
                if (ps.Length > 0)
                {
                    List<UnityEngine.GameObject> tmplst = new List<UnityEngine.GameObject>();
                    if (tmplst != null)
                    {
                        for (int i = 0; i < ps.Length; i++)
                        {
                            ParticleSystem p = ps[i];
                            if (p != null && p.gameObject != null && XOCp_ResourceHolder.IsParticleDetailed(p.gameObject))
                            {
                                tmplst.Add(p.gameObject);
                            }
                        }

                        if (tmplst.Count > 0)
                        {
                            xr.m_ParticleArray = tmplst.ToArray();
                        }
                        else
                        {
                            xr.m_ParticleArray = null;
                        }
                    }
                    else
                    {
                        xr.m_ParticleArray = null;
                    }
                }
                else
                {
                    xr.m_ParticleArray = null;
                }
            }
        }
    }
}
