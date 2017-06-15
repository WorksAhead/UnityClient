using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkCrossEngine
{
    public class CrossObjectHelper
    {
        public static T TryCastObject<T>(UnityEngine.Object obj) where T : UnityEngine.Object
        {
            return (T)obj;
        }

        public static UnityEngine.Object TryConstructCrossObject(UnityEngine.Object obj)
        {
            return obj;
        }
    }
}
