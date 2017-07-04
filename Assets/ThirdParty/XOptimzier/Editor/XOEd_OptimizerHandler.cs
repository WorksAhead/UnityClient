#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEditor;

namespace XOpt
{
	[CustomEditor(typeof(XOCp_OptimizerHandler))]

	public class XOEd_OptimizerHandler : Editor
	{
		[ExecuteInEditMode]
		//在这里方法中就可以绘制面板。
	    public override void OnInspectorGUI()
		{
			//得到Test对象
		    XOCp_OptimizerHandler hdl = (XOCp_OptimizerHandler) target;


			// serializedObject.Update();
			// EditorGUILayout.PropertyField(serializedObject.FindProperty("integers"), true);
			// serializedObject.ApplyModifiedProperties();



			EditorGUILayout.BeginVertical();
			
			if(GUILayout.Button("DisplayOrigin"))
			{
				if(hdl.bDisplayOrigin == true)
				{
					hdl.bDisplayOrigin = false;
		            XOCp_OptimizerHandler xoh = hdl.Root.GetComponent<XOCp_OptimizerHandler>();
	                if(xoh != null)
	                {
   						foreach(UnityEngine.GameObject go in xoh.olst)
   						{
   							go.SetActive(false);
   						}
	                }
				}
				else if(hdl.bDisplayOrigin == false)
				{
					hdl.bDisplayOrigin = true;
	                XOCp_OptimizerHandler xoh = hdl.Root.GetComponent<XOCp_OptimizerHandler>();
	                if(xoh != null)
	                {
   						foreach(UnityEngine.GameObject go in xoh.olst)
   						{
   							go.SetActive(true);
   						}
	                }
				}
			}

			if(GUILayout.Button("DeleteOrigin"))
			{
	            XOCp_OptimizerHandler xoh = hdl.Root.GetComponent<XOCp_OptimizerHandler>();
                if(xoh != null)
                {
					foreach(UnityEngine.GameObject go in xoh.olst)
  					{
   						UnityEngine.GameObject.DestroyImmediate(go);
   					}
	            }
			}


			if(GUILayout.Button("DisplayNew"))
			{
				if(hdl.bDisplayNew == true)
				{
					hdl.bDisplayNew = false;
	                XOCp_OptimizerHandler xoh = hdl.Root.GetComponent<XOCp_OptimizerHandler>();
	                if(xoh != null)
	                {
   						foreach(UnityEngine.GameObject go in xoh.nlst)
   						{
   							go.SetActive(false);
   						}
	                }
				}
				else if(hdl.bDisplayNew == false)
				{
					hdl.bDisplayNew = true;
	                XOCp_OptimizerHandler xoh = hdl.Root.GetComponent<XOCp_OptimizerHandler>();
	                if(xoh != null)
	                {
   						foreach(UnityEngine.GameObject go in xoh.nlst)
   						{
   							go.SetActive(true);
   						}
	                }
				}
			}

		    EditorGUILayout.EndVertical();
		}
	}
}
#endif