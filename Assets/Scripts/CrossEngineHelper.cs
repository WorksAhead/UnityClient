using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkCrossEngine
{
    public class CrossObjectHelper
    {
        public static T TryCastObject<T>(ArkCrossEngine.Object obj) where T : UnityEngine.Object
        {
            if (obj == null)
            {
                return null;
            }

            return obj.GetImpl<T>();
        }

        public static ArkCrossEngine.Object TryConstructCrossObject(UnityEngine.Object obj)
        {
            if (obj == null)
            {
                return null;
            }

            return ArkCrossEngine.ObjectFactory.Create<ArkCrossEngine.GameObject>(obj);
        }
    }
}
