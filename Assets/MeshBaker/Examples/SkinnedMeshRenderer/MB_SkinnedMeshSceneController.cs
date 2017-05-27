using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MB_SkinnedMeshSceneController : UnityEngine.MonoBehaviour {	
	public UnityEngine.GameObject swordPrefab;
	public UnityEngine.GameObject hatPrefab;
	public UnityEngine.GameObject glassesPrefab;
	public UnityEngine.GameObject workerPrefab;
	
	public UnityEngine.GameObject targetCharacter;
	
	public MB2_MeshBaker skinnedMeshBaker;
	
	UnityEngine.GameObject swordInstance;
	UnityEngine.GameObject glassesInstance;
	UnityEngine.GameObject hatInstance;
	
	void Start () {
		    //To demonstrate lets add a character to the combined mesh
			UnityEngine.GameObject worker1 = (UnityEngine.GameObject) Instantiate(workerPrefab);
			worker1.transform.position = new UnityEngine.Vector3(1.31f, 0.985f, -0.25f);
			worker1.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		    //IMPORTANT set the culling type to something other than renderer. Animations may not play
		    //if animation.cullingType is left on BasedOnRenderers. This appears to be a bug in Unity
		    //the animation gets confused about the bounds if the skinnedMeshRenderer is changed
		    worker1.GetComponent<Animation>().cullingType = AnimationCullingType.AlwaysAnimate; //IMPORTANT
			worker1.GetComponent<Animation>().Play("run");
				
		    //create an array with everything we want to add
		    //It is important to add the gameObject with the UnityEngine.Renderer/mesh attached
			UnityEngine.GameObject[] objsToAdd = new UnityEngine.GameObject[1] {worker1.GetComponentInChildren<SkinnedMeshRenderer>().gameObject};
					    
		    //add the objects. This will disable the renderers on the source objects
			skinnedMeshBaker.AddDeleteGameObjects(objsToAdd,null);
		    //apply the changes to the mesh
			skinnedMeshBaker.Apply();
	}
	
	void OnGUI () {
		if (GUILayout.Button ("Add/Remove Sword")) {
			if (swordInstance == null){
				UnityEngine.Transform hand = SearchHierarchyForBone(targetCharacter.transform,"RightHandAttachPoint");
				swordInstance = (UnityEngine.GameObject) Instantiate(swordPrefab);
				swordInstance.transform.parent = hand;
				swordInstance.transform.localPosition = UnityEngine.Vector3.zero;
				swordInstance.transform.localRotation = UnityEngine.Quaternion.identity;
				swordInstance.transform.localScale = UnityEngine.Vector3.one;
				UnityEngine.GameObject[] objsToAdd = new UnityEngine.GameObject[1] {swordInstance.GetComponentInChildren<MeshRenderer>().gameObject};
				skinnedMeshBaker.AddDeleteGameObjects(objsToAdd,null);
				skinnedMeshBaker.Apply();
			} else if (skinnedMeshBaker.CombinedMeshContains(swordInstance.GetComponentInChildren<MeshRenderer>().gameObject)) {
				UnityEngine.GameObject[] objsToDelete = new UnityEngine.GameObject[1] {swordInstance.GetComponentInChildren<MeshRenderer>().gameObject};
				skinnedMeshBaker.AddDeleteGameObjects(null,objsToDelete);
				skinnedMeshBaker.Apply();
				Destroy(swordInstance);
				swordInstance = null;
			}
		}
		if (GUILayout.Button ("Add/Remove Hat")) {
			if (hatInstance == null){
				UnityEngine.Transform hand = SearchHierarchyForBone(targetCharacter.transform,"HeadAttachPoint");
				hatInstance = (UnityEngine.GameObject) Instantiate(hatPrefab);
				hatInstance.transform.parent = hand;
				hatInstance.transform.localPosition = UnityEngine.Vector3.zero;
				hatInstance.transform.localRotation = UnityEngine.Quaternion.identity;
				hatInstance.transform.localScale = UnityEngine.Vector3.one;
				UnityEngine.GameObject[] objsToAdd = new UnityEngine.GameObject[1] {hatInstance.GetComponentInChildren<MeshRenderer>().gameObject};			
				skinnedMeshBaker.AddDeleteGameObjects(objsToAdd,null);
				skinnedMeshBaker.Apply();
			} else if (skinnedMeshBaker.CombinedMeshContains(hatInstance.GetComponentInChildren<MeshRenderer>().gameObject)) {
				UnityEngine.GameObject[] objsToDelete = new UnityEngine.GameObject[1] {hatInstance.GetComponentInChildren<MeshRenderer>().gameObject};
				skinnedMeshBaker.AddDeleteGameObjects(null,objsToDelete);
				skinnedMeshBaker.Apply();
				Destroy(hatInstance);
				hatInstance = null;				
			}
		}
		if (GUILayout.Button ("Add/Remove Glasses")) {
			if (glassesInstance == null){
				UnityEngine.Transform hand = SearchHierarchyForBone(targetCharacter.transform,"NoseAttachPoint");
				glassesInstance = (UnityEngine.GameObject) Instantiate(glassesPrefab);
				glassesInstance.transform.parent = hand;
				glassesInstance.transform.localPosition = UnityEngine.Vector3.zero;
				glassesInstance.transform.localRotation = UnityEngine.Quaternion.identity;
				glassesInstance.transform.localScale = UnityEngine.Vector3.one;
				UnityEngine.GameObject[] objsToAdd = new UnityEngine.GameObject[1] {glassesInstance.GetComponentInChildren<MeshRenderer>().gameObject};			
				skinnedMeshBaker.AddDeleteGameObjects(objsToAdd,null);
				skinnedMeshBaker.Apply();
			} else if (skinnedMeshBaker.CombinedMeshContains(glassesInstance.GetComponentInChildren<MeshRenderer>().gameObject)) {
				UnityEngine.GameObject[] objsToDelete = new UnityEngine.GameObject[1] {glassesInstance.GetComponentInChildren<MeshRenderer>().gameObject};
				skinnedMeshBaker.AddDeleteGameObjects(null,objsToDelete);
				skinnedMeshBaker.Apply();
				Destroy(glassesInstance);
				glassesInstance = null;				
			}
		}		
	}
	
	public UnityEngine.Transform SearchHierarchyForBone(UnityEngine.Transform current, string name)   
	{
	    if (current.name.Equals( name ))
	        return current;
	
	    for (int i = 0; i < current.childCount; ++i)
	    {
	        UnityEngine.Transform found = SearchHierarchyForBone(current.GetChild(i), name);
	
	        if (found != null)
	            return found;
	    }
	    return null;
	}
}
