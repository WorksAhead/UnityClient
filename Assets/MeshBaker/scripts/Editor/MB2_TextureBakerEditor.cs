//----------------------------------------------
//            MeshBaker
// Copyright © 2011-2012 Ian Deane
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEditor;

[CustomEditor(typeof(MB2_TextureBaker))]
public class MB2_TextureBakerEditor : Editor {
	
	MB2_TextureBakerEditorInternal tbe = new MB2_TextureBakerEditorInternal();
	
	public override void OnInspectorGUI(){
		tbe.DrawGUI((MB2_TextureBaker) target);	
	}

	public static void CreateCombinedMaterialAssets(MB2_TextureBaker target, string pth){
		MB2_TextureBaker mom = (MB2_TextureBaker) target;
		string baseName = Path.GetFileNameWithoutExtension(pth);
		if (baseName == null || baseName.Length == 0) return;
		string folderPath = pth.Substring(0,pth.Length - baseName.Length - 6);
		
		List<string> matNames = new List<string>();
		if (mom.doMultiMaterial){
			for (int i = 0; i < mom.resultMaterials.Length; i++){
				matNames.Add( folderPath +  baseName + "-mat" + i + ".mat" );
				AssetDatabase.CreateAsset(new Material(UnityEngine.Shader.Find("Diffuse")), matNames[i]);
				mom.resultMaterials[i].combinedMaterial = (Material) AssetDatabase.LoadAssetAtPath(matNames[i],typeof(Material));
			}
		}else{
			matNames.Add( folderPath +  baseName + "-mat.mat" );
			Material newMat = new Material(UnityEngine.Shader.Find("Diffuse"));
			if (mom.objsToMesh.Count > 0 && mom.objsToMesh[0] != null){
				UnityEngine.Renderer r = mom.objsToMesh[0].GetComponent<UnityEngine.Renderer>();
				if (r.sharedMaterial != null){
					newMat.shader = r.sharedMaterial.shader;					
					MB2_TextureBaker.ConfigureNewMaterialToMatchOld(newMat,r.sharedMaterial);
				}
			} else {
				Debug.Log("If you add objects to be combined before creating the Combined Material Assets. Then Mesh Baker will create a result material that is a duplicate of the material on the first object to be combined. This saves time configuring the shader.");	
			}
			AssetDatabase.CreateAsset(newMat, matNames[0]);
			mom.resultMaterial = (Material) AssetDatabase.LoadAssetAtPath(matNames[0],typeof(Material));
		}
		//create the MB2_TextureBakeResults
		AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<MB2_TextureBakeResults>(),pth);
		mom.textureBakeResults = (MB2_TextureBakeResults) AssetDatabase.LoadAssetAtPath(pth, typeof(MB2_TextureBakeResults));
		AssetDatabase.Refresh();
	}	
}