using UnityEngine;
using System.Collections;
using System;

namespace XOpt
{
    public class XOCp_MarkIntermediate : UnityEngine.MonoBehaviour
    {
        //中间根节点
        //什么都不用做
        public int OriginInstID = XOGloble.INVALID_ID;

        //是否显示原
        public bool bShowOrigin = true;

        //源
        public UnityEngine.GameObject goOrigin = null;
    }
};
