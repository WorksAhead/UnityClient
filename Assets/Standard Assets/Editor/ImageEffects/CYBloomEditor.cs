using System;
using UnityEditor;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [CustomEditor (typeof(CYBloom))]
    class CYBloomEditor : Editor
    {
        SerializedObject serObj;

        SerializedProperty bloomIntensity;
        SerializedProperty bloomThreshold;
        SerializedProperty brightOffset;
        SerializedProperty HDRBrightLevel;
        SerializedProperty HDRBloomMul;

        void OnEnable () {
            serObj = new SerializedObject (target);

            bloomIntensity = serObj.FindProperty("bloomIntensity");
            bloomThreshold = serObj.FindProperty("bloomThreshold");
            brightOffset = serObj.FindProperty("brightOffset");
            HDRBrightLevel = serObj.FindProperty("HDRBrightLevel");
            HDRBloomMul = serObj.FindProperty("HDRBloomMul");
        }


        public override void OnInspectorGUI ()
        {
            serObj.Update();


            EditorGUILayout.PropertyField (bloomIntensity, new GUIContent("Intensity"));
            bloomThreshold.floatValue = EditorGUILayout.Slider ("Threshold", bloomThreshold.floatValue, -0.05f, 10.0f);
            EditorGUILayout.PropertyField(brightOffset, new GUIContent("Bright Offset"));
            EditorGUILayout.PropertyField(HDRBrightLevel, new GUIContent("HDR Bright Level"));
            EditorGUILayout.PropertyField(HDRBloomMul, new GUIContent("HDR Bloom Mul"));

            serObj.ApplyModifiedProperties();
        }
    }
}
