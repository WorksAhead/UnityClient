#if UNITY_EDITOR

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class ReferenceFinder
{
    public static List<Object> FindRefrencesTo(Object obj, bool autoSelection = true)
    {
        var referenceBy = new List<Object>();
        //var allObjects = Object.FindObjectsOfType<GameObject>();
        var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        for (int i = 0; i < allObjects.Length; ++i)
        {
            var go = allObjects[i];
            if (PrefabUtility.GetPrefabType(go) == PrefabType.PrefabInstance)
            {
                if (PrefabUtility.GetPrefabParent(go) == obj)
                {
                    Debug.Log(string.Format("referenced by {0}, {1}", go.name, go.GetType()), go);
                    referenceBy.Add(go);
                }
            }

            var components = go.GetComponents<Component>();
            for (int j = 0; j < components.Length; ++j)
            {
                var c = components[j];
                if (c == null)
                {
                    continue;
                }

                var so = new SerializedObject(c);
                var sp = so.GetIterator();
                while (sp.NextVisible(true))
                {
                    if (sp.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (sp.objectReferenceValue == obj)
                        {
                            Debug.Log(string.Format("referenced by {0}, {1}", c.name, c.GetType()), c);
                            referenceBy.Add(c.gameObject);
                        }
                    }
                }
            }
        }

        if (autoSelection)
        {
            if (referenceBy.Any())
            {
                Selection.objects = referenceBy.ToArray();
            }
            else
            {
                Debug.Log("no references in scene");
            }
        }

        return referenceBy;
    }
}
#endif