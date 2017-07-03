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
using DigitalOpus.MB.Core;

using UnityEditor;

public class MB2_TextureBakerEditorInternal{
	//add option to exclude skinned mesh renderer and mesh renderer in filter
	//example scenes for multi material
	
	private static GUIContent insertContent = new GUIContent("+", "add a material");
	private static GUIContent deleteContent = new GUIContent("-", "delete a material");
	private static GUILayoutOption buttonWidth = GUILayout.MaxWidth(20f);

	private SerializedObject textureBaker;
	private SerializedProperty textureBakeResults, maxTilingBakeSize, doMultiMaterial, fixOutOfBoundsUVs, resultMaterial, resultMaterials, atlasPadding, resizePowerOfTwoTextures, customShaderPropNames, objsToMesh, texturePackingAlgorithm;
	
	bool resultMaterialsFoldout = true;
	bool showInstructions = false;
	bool showContainsReport = true;
	
	private static GUIContent
		createPrefabAndMaterialLabelContent = new GUIContent("Create Empty Assets For Combined Material", "Creates a material asset and a 'MB2_TextureBakeResult' asset. You should set the shader on the material. Mesh Baker uses the UnityEngine.Texture properties on the material to decide what atlases need to be created. The MB2_TextureBakeResult asset should be used in the 'Material Bake Result' field."),
		openToolsWindowLabelContent = new GUIContent("Open Tools For Adding Objects", "Use these tools to find out what can be combined, discover possible problems with meshes, and quickly add objects."),
		fixOutOfBoundsGUIContent = new GUIContent("Fix Out-Of-Bounds UVs", "If mesh has uvs outside the range 0,1 uvs will be scaled so they are in 0,1 range. Textures will have tiling baked."),
		resizePowerOfTwoGUIContent = new GUIContent("Resize Power-Of-Two Textures", "Shrinks textures so they have a clear border of width 'Atlas Padding' around them. Improves texture packing efficiency."),
		customShaderPropertyNamesGUIContent = new GUIContent("Custom UnityEngine.Shader Propert Names", "Mesh Baker has a list of common texture properties that it looks for in shaders to generate atlases. Custom shaders may have texture properties not on this list. Add them here and Meshbaker will generate atlases for them."),
		combinedMaterialsGUIContent = new GUIContent("Combined Materials", "Use the +/- buttons to add multiple combined materials. You will also need to specify which materials on the source objects map to each combined material."),
		maxTilingBakeSizeGUIContent = new GUIContent("Max Tiling Bake Size","This is the maximum size tiling textures will be baked to."),
		objectsToCombineGUIContent = new GUIContent("Objects To Be Combined","These can be prefabs or scene objects. They must be game objects with UnityEngine.Renderer components, not the parent objects. Materials on these objects will baked into the combined material(s)"),
		textureBakeResultsGUIContent = new GUIContent("Material Bake Result","This asset contains a mapping of materials to UV rectangles in the atlases. It is needed to create combined meshes or adjust meshes so they can use the combined material(s). Create it using 'Create Empty Assets For Combined Material'. Drag it to the 'Material Bake Result' field to use it."),
		texturePackingAgorithmGUIContent = new GUIContent("UnityEngine.Texture Packer", "Unity's PackTextures: Atlases are always a power of two. Can crash when trying to generate large atlases. \n\n Mesh Baker UnityEngine.Texture Packer: Atlases will be most efficient size and shape (not limited to a power of two). More robust for large atlases."),
		configMultiMatFromObjsContent = new GUIContent("Build Source To Combined Mapping From \n Objects To Be Combined", "This will group the materials on your source objects by shader and create one source to combined mapping for each shader found. For example if combining trees then all the materials with the same bark shader will be grouped togther and all the materials with the same leaf material will be grouped together. You can adjust the results afterwards.");
	
	[MenuItem("GameObject/Create Other/Mesh Baker/Material Baker")]
	public static void CreateNewTextureBaker(){
		MB2_TextureBaker[] mbs = (MB2_TextureBaker[]) Editor.FindObjectsOfType(typeof(MB2_TextureBaker));
    	Regex regex = new Regex(@"(\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
		int largest = 0;
		try{
			for (int i = 0; i < mbs.Length; i++){
				Match match = regex.Match(mbs[i].name);
				if (match.Success){
					int val = Convert.ToInt32(match.Groups[1].Value);
					if (val >= largest)
						largest = val + 1;
				}
			}
		} catch(Exception e){
			if (e == null) e = null; //Do nothing supress compiler warning
		}
		UnityEngine.GameObject nmb = new UnityEngine.GameObject("MaterialBaker" + largest);
		nmb.transform.position = UnityEngine.Vector3.zero;
		MB2_TextureBaker tb = nmb.AddComponent<MB2_TextureBaker>();
		tb.texturePackingAlgorithm = MB2_PackingAlgorithmEnum.MeshBakerTexturePacker;
	}

	void _init(MB2_TextureBaker target) {
		textureBaker = new SerializedObject(target);
		doMultiMaterial = textureBaker.FindProperty("doMultiMaterial");
		fixOutOfBoundsUVs = textureBaker.FindProperty("fixOutOfBoundsUVs");
		resultMaterial = textureBaker.FindProperty("resultMaterial");
		resultMaterials = textureBaker.FindProperty("resultMaterials");
		atlasPadding = textureBaker.FindProperty("atlasPadding");
		resizePowerOfTwoTextures = textureBaker.FindProperty("resizePowerOfTwoTextures");
		customShaderPropNames = textureBaker.FindProperty("customShaderPropNames");
		objsToMesh = textureBaker.FindProperty("objsToMesh");
		maxTilingBakeSize = textureBaker.FindProperty("maxTilingBakeSize");
		textureBakeResults = textureBaker.FindProperty("textureBakeResults");
		texturePackingAlgorithm = textureBaker.FindProperty("texturePackingAlgorithm");
	}	
	
	public void DrawGUI(MB2_TextureBaker mom){
		if (textureBaker == null){
			_init(mom);
		}
		
		textureBaker.Update();

		showInstructions = EditorGUILayout.Foldout(showInstructions,"Instructions:");
		if (showInstructions){
			EditorGUILayout.HelpBox("1. Add scene objects or prefabs to combine. For best results these should use the same shader as result material.\n\n" +
									"2. Create Empty Assets For Combined Material(s)\n\n" +
									"3. Check that shader on result material(s) are correct.\n\n" +
									"4. Bake materials into combined material(s).\n\n" +
									"5. Look at warnings/errors in console. Decide if action needs to be taken.\n\n" +
									"6. You are now ready to build combined meshs or adjust meshes to use the combined material(s).", UnityEditor.MessageType.None);
			
		}				

		EditorGUILayout.Separator();		
		EditorGUILayout.LabelField("Objects To Be Combined",EditorStyles.boldLabel);	
		if (GUILayout.Button(openToolsWindowLabelContent)){
			MB_MeshBakerEditorWindow mmWin = (MB_MeshBakerEditorWindow) EditorWindow.GetWindow(typeof(MB_MeshBakerEditorWindow));
			mmWin.target = (MB2_MeshBakerRoot) mom;
		}	
		EditorGUILayout.PropertyField(objsToMesh,objectsToCombineGUIContent, true);		
		
		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Output",EditorStyles.boldLabel);
		if (GUILayout.Button(createPrefabAndMaterialLabelContent)){
			string newPrefabPath = EditorUtility.SaveFilePanelInProject("Asset name", "", "asset", "Enter a name for the baked texture results");
			if (newPrefabPath != null){
				MB2_TextureBakerEditor.CreateCombinedMaterialAssets(mom, newPrefabPath);
			}
		}	
		EditorGUILayout.PropertyField(textureBakeResults, textureBakeResultsGUIContent);
		if (textureBakeResults.objectReferenceValue != null){
			showContainsReport = EditorGUILayout.Foldout(showContainsReport, "Shaders & Materials Contained");
			if (showContainsReport){
				EditorGUILayout.HelpBox(((MB2_TextureBakeResults)textureBakeResults.objectReferenceValue).GetDescription(), MessageType.Info);	
			}
		}
		EditorGUILayout.PropertyField(doMultiMaterial,new GUIContent("Multiple Combined Materials"));		
		
		if (mom.doMultiMaterial){
			EditorGUILayout.LabelField("Source Material To Combined Mapping",EditorStyles.boldLabel);
			if (GUILayout.Button(configMultiMatFromObjsContent)){
				ConfigureMutiMaterialsFromObjsToCombine(mom);	
			}
			EditorGUILayout.BeginHorizontal();
			resultMaterialsFoldout = EditorGUILayout.Foldout(resultMaterialsFoldout, combinedMaterialsGUIContent);
			
			if(GUILayout.Button(insertContent, EditorStyles.miniButtonLeft, buttonWidth)){
				if (resultMaterials.arraySize == 0){
					mom.resultMaterials = new MB_MultiMaterial[1];	
				} else {
					resultMaterials.InsertArrayElementAtIndex(resultMaterials.arraySize-1);
				}
			}
			if(GUILayout.Button(deleteContent, EditorStyles.miniButtonRight, buttonWidth)){
				resultMaterials.DeleteArrayElementAtIndex(resultMaterials.arraySize-1);
			}			
			EditorGUILayout.EndHorizontal();
			if (resultMaterialsFoldout){
				for(int i = 0; i < resultMaterials.arraySize; i++){
					EditorGUILayout.Separator();
					string s = "";
					if (i < mom.resultMaterials.Length && mom.resultMaterials[i] != null && mom.resultMaterials[i].combinedMaterial != null) s = mom.resultMaterials[i].combinedMaterial.shader.ToString();
					EditorGUILayout.LabelField("---------- submesh:" + i + " " + s,EditorStyles.boldLabel);
					EditorGUILayout.Separator();
					SerializedProperty resMat = resultMaterials.GetArrayElementAtIndex(i);
					EditorGUILayout.PropertyField(resMat.FindPropertyRelative("combinedMaterial"));
					SerializedProperty sourceMats = resMat.FindPropertyRelative("sourceMaterials");
					EditorGUILayout.PropertyField(sourceMats,true);
				}
			}
			
		} else {			
			EditorGUILayout.PropertyField(resultMaterial,new GUIContent("Combined Mesh Material"));
		}				
		
		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Material Bake Options",EditorStyles.boldLabel);		
		EditorGUILayout.PropertyField(atlasPadding,new GUIContent("Atlas Padding"));
		EditorGUILayout.PropertyField(resizePowerOfTwoTextures, resizePowerOfTwoGUIContent);
		EditorGUILayout.PropertyField(customShaderPropNames,customShaderPropertyNamesGUIContent,true);
		EditorGUILayout.PropertyField(maxTilingBakeSize, maxTilingBakeSizeGUIContent);
		EditorGUILayout.PropertyField(fixOutOfBoundsUVs,fixOutOfBoundsGUIContent);		
		EditorGUILayout.PropertyField(texturePackingAlgorithm, texturePackingAgorithmGUIContent);
		
		EditorGUILayout.Separator();				
		if (GUILayout.Button("Bake Materials Into Combined Material")){
			mom.CreateAndSaveAtlases(updateProgressBar, System.IO.File.WriteAllBytes);
			EditorUtility.ClearProgressBar();
			EditorUtility.SetDirty(mom.textureBakeResults);
		}
		textureBaker.ApplyModifiedProperties();		
		textureBaker.SetIsDifferentCacheDirty();
	}
		
	public void updateProgressBar(string msg, float progress){
		EditorUtility.DisplayProgressBar("Combining Meshes", msg, progress);
	}
	
	public void ConfigureMutiMaterialsFromObjsToCombine(MB2_TextureBaker mom){
		if (mom.objsToMesh.Count == 0){
			Debug.LogError("You need to add some objects to combine before building the multi material list.");
			return;
		}
		if (resultMaterials.arraySize > 0){
			Debug.LogError("You already have some source to combined material mappings configured. You must remove these before doing this operation.");
			return;			
		}
		if (mom.textureBakeResults == null){
			Debug.LogError("Material Bake Result asset must be set before using this operation.");
			return;
		}
		Dictionary<UnityEngine.Shader,List<Material>> shader2Material_map = new Dictionary<UnityEngine.Shader, List<Material>>();
		bool hasOutOfBoundsUVs = false;
		for (int i = 0; i < mom.objsToMesh.Count; i++){
			UnityEngine.GameObject go = mom.objsToMesh[i];
			if (go == null) {
				Debug.LogError("Null object in list of objects to combine at position " + i);
				return;
			}
			UnityEngine.Renderer r = go.GetComponent<UnityEngine.Renderer>();
			if (r == null || (!(r is MeshRenderer) && !(r is SkinnedMeshRenderer))){
				Debug.LogError("UnityEngine.GameObject at position " + i + " in list of objects to combine did not have a renderer");
				return;
			}
			
			Mesh m = MB_Utility.GetMesh(go);
			Rect dummy = new Rect();
			if (MB_Utility.hasOutOfBoundsUVs(m, ref dummy, -1)) hasOutOfBoundsUVs = true;	
			
			for (int j = 0; j < r.sharedMaterials.Length; j++){
				if (r.sharedMaterials[j] == null) continue;
				List<Material> matsThatUseShader = null;;
				if (!shader2Material_map.TryGetValue(r.sharedMaterials[j].shader, out matsThatUseShader)){
					matsThatUseShader = new List<Material>();
					shader2Material_map.Add(r.sharedMaterials[j].shader, matsThatUseShader);
				}
				if (!matsThatUseShader.Contains(r.sharedMaterials[j])) matsThatUseShader.Add(r.sharedMaterials[j]);
			}
		}
		if (hasOutOfBoundsUVs){
			mom.fixOutOfBoundsUVs = true;
			Debug.LogWarning("Some game objects have out-of-bounds UVs. Setting the fix-out-of-bounds UVs flag");
		}
		if (shader2Material_map.Count == 0) Debug.LogError("Found no materials in list of objects to combine");
		mom.resultMaterials = new MB_MultiMaterial[shader2Material_map.Count];
		string pth = AssetDatabase.GetAssetPath(mom.textureBakeResults);
		string baseName = Path.GetFileNameWithoutExtension(pth);
		string folderPath = pth.Substring(0,pth.Length - baseName.Length - 6);		
		int k = 0;
		foreach(UnityEngine.Shader sh in shader2Material_map.Keys){ 
			List<Material> matsThatUse = shader2Material_map[sh];
			MB_MultiMaterial mm = mom.resultMaterials[k] = new MB_MultiMaterial();
			mm.sourceMaterials = matsThatUse;
			string matName = folderPath +  baseName + "-mat" + k + ".mat";
			Material newMat = new Material(UnityEngine.Shader.Find("Diffuse"));
			if (matsThatUse.Count > 0 && matsThatUse[0] != null){
				MB2_TextureBaker.ConfigureNewMaterialToMatchOld(newMat, matsThatUse[0]);
			}
			AssetDatabase.CreateAsset(newMat, matName);
			mm.combinedMaterial = (Material) AssetDatabase.LoadAssetAtPath(matName,typeof(Material));
			k++;
		}
		textureBaker.UpdateIfDirtyOrScript();
	}
}
