using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
namespace ArkCrossEngine
{
    public class CoroutineInsManager : UnityEngine.MonoBehaviour
    {
        #region Singleton
        private static CoroutineInsManager s_Instance = null;
        public static CoroutineInsManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    UnityEngine.GameObject gameObjectRoot = UnityEngine.GameObject.Find(ArkCrossEngine.GlobalVariables.cGameRootName);
                    if (gameObjectRoot != null)
                    {
                        s_Instance = gameObjectRoot.GetComponent<CoroutineInsManager>();
                        if (s_Instance == null)
                        {
                            s_Instance = gameObjectRoot.AddComponent<CoroutineInsManager>();
                        }
                    }
                }
                return s_Instance;
            }
        }
        #endregion
    }
}
