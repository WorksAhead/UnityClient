#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace XOpt
{
	[ExecuteInEditMode]
	public class XOEd_MainFrame : ScriptableWizard 
	{
		private static XOEd_MainFrame _myWindow;


		// GUI look
		private static UnityEngine.Vector2 scrollPos;
		private static GUIStyle headerStyle = new GUIStyle();
		private static GUIStyle footerStyle = new GUIStyle();
		private static int blockWidth = 300;
		
		//GUI Config
		public static bool bViewTextureSplit = true;
		public static bool bViewCompressMesh = true;


		public List<string> pathlist; //todo make this UnityEngine.Renderer

	    [MenuItem ("Window/SceneParticleMark", false, 50)]
   		public static void SceneParticleMark ()
		{
			XOCp_ResourceHolder.SceneParticleMark();
		}


	    [MenuItem ("Window/ScenesCombine/Release/Sort", false, 50)]
   		public static void Sort ()
		{
			BackUpScene();
		}

	    [MenuItem ("Window/ScenesCombine/Release/CombineMaterial", false, 50)]
   		public static void Comb ()
		{
			CombineTexture();
		}

	    [MenuItem ("Window/ScenesCombine/Release/CombineMesh", false, 50)]
   		public static void CombineMeshes ()
		{
		   	UnityEngine.GameObject[] gos = Selection.gameObjects;
		  	if(gos != null)
	    	{
	    		XOTextureCombine.CombineMeshes(gos);
	    	}
		}

	    [MenuItem ("Window/ScenesCombine/Release/CombineGameObject", false, 50)]
   		public static void CombineGameObjects ()
		{
		   	UnityEngine.GameObject[] gos = Selection.gameObjects;
		  	if(gos != null)
	    	{
	    		XOTextureCombine.CombineGameObjects(gos);
	    	}
		}


	    [MenuItem ("Window/ScenesCombine/Release/TryCombine", false, 50)]
   		public static void TryComb ()
		{
	    	UnityEngine.GameObject[] gos = Selection.gameObjects;
	    	if(gos != null)
	    	{
	    		XOTextureCombine.TryCombine(gos);
	    	}
		}

	    [MenuItem ("Window/ScenesCombine/Debug/Combine", false, 50)]
	    public static void CombineTexture()
	    {
	    	UnityEngine.GameObject[] gos = Selection.gameObjects;
	    	if(gos != null)
	    	{
	    		XOTextureCombine.Combine(gos);
	    	}
	    }

	    [MenuItem ("Window/ScenesCombine/Debug/BackUpScene", false, 50)]
		public static void BackUpScene ()
		{
			CreateOptRoot();

			XOCp_OptimizerHandler xoh = XOGloble.Root.GetComponent<XOCp_OptimizerHandler>();
			xoh.olst.Clear();
			xoh.nlst.Clear();

			UnityEngine.Renderer[] rs = (UnityEngine.Renderer[]) FindObjectsOfType(typeof(UnityEngine.Renderer));
			for (int i = 0; i < rs.Length; i++)
			{
				UnityEngine.Renderer r = rs[i];
				if (r is MeshRenderer)
				{
					if (r.GetComponent<TextMesh>() != null)
					{
						continue; //don't add TextMeshes
					}
					if(r.GetComponent<XOCp_MarkIgnore>() != null)
					{
						continue;
					}
					if(r.gameObject.isStatic == true && r.gameObject.activeInHierarchy == true)
					{
			            if(xoh != null)
			            {
			               xoh.olst.Add(r.gameObject);
			            }

						if(XOMeshSplit.CanSplit(r.gameObject) == true)
						{
							XOMeshSplit.ExtractMaterialMeshes(r.gameObject);
						}
						else
						{
							UnityEngine.GameObject backobj = NewIterM(r.gameObject);
							xoh.nlst.Add(backobj);
							AddObjDic(r.gameObject, backobj);
						}

						xoh.bDisplayOrigin 	= false;
						xoh.bDisplayNew 	= true;
						r.gameObject.SetActive(false);
					}
				}
			}

            if(xoh != null)
            {
               xoh.CreateNodes();
            }
		}

		public static UnityEngine.GameObject NewIterM(UnityEngine.GameObject origin)
		{
			UnityEngine.GameObject backobj 				= UnityEngine.GameObject.Instantiate(origin, origin.transform.position, origin.transform.rotation) as UnityEngine.GameObject;
			backobj.transform.parent 		= XOGloble.IterM.transform;
			backobj.transform.localScale 	= origin.transform.lossyScale;
			backobj.name 					= XOUtil.Path(origin);
			XOCp_MarkIntermediate mi 		= backobj.AddComponent<XOCp_MarkIntermediate>();
			mi.OriginInstID 				= origin.GetInstanceID();
			mi.goOrigin						= origin;
			backobj.SetActive(true);

			XOCp_OptimizerHandler xoh = XOGloble.Root.GetComponent<XOCp_OptimizerHandler>();
            if(xoh != null)
            {
               xoh.AddObjLstByShaderMaterial(backobj);
            }

    		XOUtil.Display(backobj, false);
			return backobj;
		}

		public static void AddObjDic(UnityEngine.GameObject origin, UnityEngine.GameObject newone)
		{
			XOCp_OptimizerHandler oph = XOGloble.Root.GetComponent<XOCp_OptimizerHandler>();
			if(oph != null)
			{
				Dictionary<int, OriginItermedia> p2O = oph.p2O;
				if(p2O != null)
				{
					int instid = origin.GetInstanceID();
					if(p2O.ContainsKey(instid) == false)
					{
						OriginItermedia objdata = new OriginItermedia();
						objdata.origin = origin;
						objdata.Iterm = newone;
						p2O.Add(instid, objdata);
					}
				}
			}
		}

		public static void CreateOptRoot()
		{
			UnityEngine.GameObject or = UnityEngine.GameObject.Find("OptimzerRoot");
			if(or != null)
			{
				UnityEngine.GameObject.DestroyImmediate(or);
			}

			XOGloble.Root = new UnityEngine.GameObject("OptimzerRoot");
			XOGloble.Root.isStatic = true;
			XOGloble.Root.transform.position = UnityEngine.Vector3.zero;
			XOGloble.Root.AddComponent<XOCp_OptimizerHandler>();

			XOGloble.IterM = new UnityEngine.GameObject("Itermediate");
			XOGloble.IterM.isStatic = true;
			XOGloble.IterM.transform.parent = XOGloble.Root.transform;
			XOGloble.IterM.transform.position = UnityEngine.Vector3.zero;

			XOGloble.Output = new UnityEngine.GameObject("Output");
			XOGloble.Output.isStatic = true;
			XOGloble.Output.transform.parent = XOGloble.Root.transform;
			XOGloble.Output.transform.position = UnityEngine.Vector3.zero;
		}
	}
};
#endif