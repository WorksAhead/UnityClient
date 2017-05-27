using UnityEngine;
using System.Collections;
using System;
namespace XOpt
{
    public class XOUtil
    {
        public static string Path(UnityEngine.GameObject obj)
        {
            string path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }

        public static void Display(UnityEngine.GameObject obj, bool tog)
        {
            return;
            if (obj != null)
            {
                UnityEngine.Renderer rd = obj.GetComponent<UnityEngine.Renderer>();
                if (rd != null)
                {
                    rd.enabled = tog;
                }
            }
        }
    }
}
