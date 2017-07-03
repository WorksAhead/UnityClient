using UnityEngine;
using UnityEditor;
using System.Collections;

public class MB_InspectorFunctions : UnityEngine.MonoBehaviour {
	public static void DrawArrayField(SerializedObject serializedObject,string property){
	    serializedObject.Update();
	    EditorGUIUtility.LookLikeInspector();
	
	    SerializedProperty myIterator = serializedObject.FindProperty(property);
	    while (true){
	        Rect myRect = GUILayoutUtility.GetRect(0f, 16f);
	        bool showChildren = EditorGUI.PropertyField(myRect, myIterator);
			if (!myIterator.NextVisible(showChildren)){
				break;	
			}
		}
	    serializedObject.ApplyModifiedProperties();		
	}
}
