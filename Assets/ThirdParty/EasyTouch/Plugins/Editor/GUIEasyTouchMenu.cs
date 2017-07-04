using UnityEngine;
using UnityEditor;
using System.Collections;

public class GUIEasyTouchMenu : Editor {

	[MenuItem ("Hedgehog Team/EasyTouch/Add EasyTouch for C#")]
	static void  AddEasyTouch4C(){
		GUIEasyTouchMenu.AddEasyTouch(true);
	}

	[MenuItem ("Hedgehog Team/EasyTouch/Add EasyTouch for javascript")]
	static void  AddEasyTouch4Java(){
		GUIEasyTouchMenu.AddEasyTouch(false);
	}
	
	[MenuItem ("Hedgehog Team/EasyTouch/Extensions/Adding a new joystick")]
	static void  AddJoystick(){
		
		EasyTouch  easyt =(EasyTouch) UnityEngine.GameObject.FindObjectOfType(typeof(EasyTouch));
		if (easyt==null){
			easyt = GUIEasyTouchMenu.AddEasyTouch(true).GetComponent<EasyTouch>();	
		}
	
		if (easyt.useBroadcastMessage){
				easyt.isExtension = true;	
		}
		
		UnityEngine.GameObject joy = new UnityEngine.GameObject("New joystick");
		joy.AddComponent<EasyJoystick>();
		 Selection.activeGameObject = joy;
	}
	
	[MenuItem ("Hedgehog Team/EasyTouch/Extensions/Adding a new button")]
	static void  AddButton(){
		
		EasyTouch  easyt =(EasyTouch) UnityEngine.GameObject.FindObjectOfType(typeof(EasyTouch));
		if (easyt==null){
			easyt = GUIEasyTouchMenu.AddEasyTouch(true).GetComponent<EasyTouch>();	
		}
	
		if (easyt.useBroadcastMessage){
			easyt.isExtension = true;	
		}

		
		UnityEngine.GameObject butt = new UnityEngine.GameObject("New button");
		butt.AddComponent<EasyButton>();
		Selection.activeGameObject = butt;
	}	
	
	
	static UnityEngine.GameObject AddEasyTouch(bool c){
	
		if (UnityEngine.GameObject.FindObjectOfType(typeof(EasyTouch))==null){
			UnityEngine.GameObject easyTouch = new UnityEngine.GameObject("EasyTouch");
			EasyTouch easy = easyTouch.AddComponent<EasyTouch>();
			
			if (c){
				easy.useBroadcastMessage = false;		
			}
			else{
				easy.useBroadcastMessage = true;
			}
			return easyTouch;
		}
		else{
			EditorUtility.DisplayDialog("Warning","EasyTouch is already exist in your scene","OK");
			return null;
		}
		
	}
}
