#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.IO;
using System;

using UnityEditor;
namespace XOpt
{
	[CustomEditor(typeof(XOCp_MarkIntermediate))]
	[ExecuteInEditMode]

	public class XOEd_MarkIntermediate : Editor
	{
		//在这里方法中就可以绘制面板。
	    public override void OnInspectorGUI()
		{
			//得到Test对象
		    XOCp_MarkIntermediate hdl = (XOCp_MarkIntermediate) target;
			EditorGUILayout.BeginVertical();

			hdl.goOrigin = EditorGUILayout.ObjectField(hdl.goOrigin, typeof(UnityEngine.GameObject), true) as UnityEngine.GameObject;

			// hdl.bShowOrigin = EditorGUILayout.Toggle("Show Origin", hdl.bShowOrigin);
			// XOCp_OptimizerHandler omi = XOGloble.Root.GetComponent<XOCp_OptimizerHandler>();

			// if(hdl.bShowOrigin == true)
			// {
			// 	if(omi.p2O.ContainsKey(hdl.OriginInstID) == true)
			// 	{
			// 		OriginItermedia datainfo = omi.p2O[hdl.OriginInstID];
			// 		if(datainfo != null)
			// 	    {
			// 	    	if(datainfo.origin != null)
			// 	    	{
			// 	    		XOUtil.Display(datainfo.origin, true);
			// 	    	}

			// 	    	if(datainfo.Iterm != null)
			// 	    	{
			// 	    		XOUtil.Display(datainfo.Iterm, false);
			// 	    	}
			// 	    }
			// 	}
			// }
			// else
			// {
			// 	if(omi.p2O.ContainsKey(hdl.OriginInstID) == true)
			// 	{
			// 		OriginItermedia datainfo = omi.p2O[hdl.OriginInstID];
			// 		if(datainfo != null)
			// 	    {
			// 	    	if(datainfo.origin != null)
			// 	    	{
			// 	    		XOUtil.Display(datainfo.origin, false);
			// 	    	}

			// 	    	if(datainfo.Iterm != null)
			// 	    	{
			// 	    		XOUtil.Display(datainfo.Iterm, true);
			// 	    	}
			// 	    }
			// 	}

			// }
		    EditorGUILayout.EndVertical();
		}
	}
}
#endif