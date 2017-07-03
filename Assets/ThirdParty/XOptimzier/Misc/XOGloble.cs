using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace XOpt
{
    public class OriginItermedia
    {
        public UnityEngine.GameObject origin = null;
        public UnityEngine.GameObject Iterm = null;
    }

    public class XOGloble
    {
        public static UnityEngine.GameObject Root = null;
        public static UnityEngine.GameObject IterM = null;
        public static UnityEngine.GameObject Output = null;
        public static UnityEngine.GameObject ResourceHolder = null;

        public static int INVALID_ID = -1;


        // m_AssetExDict = new Dictionary<int, Origin2Itermedia>();

    }
}
