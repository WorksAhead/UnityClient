// EasyTouch v2.0 (September 2012)
// EasyTouch library is copyright (c) of Hedgehog Team
// Please send feedback or bug reports to the.hedgehog.team@gmail.com
using UnityEngine;
using System.Collections;


// This is the class that simulate touches with the mouse.
// Internal use only, DO NOT USE IT
public class EasyTouchInput{
	
	#region private members
	private UnityEngine.Vector2[] oldMousePosition = new UnityEngine.Vector2[2];
	private int[] tapCount = new int[2];
	private float[] startActionTime = new float[2];
	private float[] deltaTime = new float[2];
	private float[] tapeTime = new float[2];
	
	// Complexe 2 fingers simulation
	private bool bComplex=false;
	private UnityEngine.Vector2 deltaFingerPosition;
	private UnityEngine.Vector2 oldFinger2Position;
	private UnityEngine.Vector2 complexCenter;
	#endregion
	
	#region Public methods
	// Return the number of touch
	public int TouchCount(){
	
		#if ((UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR) 
			return getTouchCount(true);
		#else
			return getTouchCount(false);
		#endif
		
	}
	
	private int getTouchCount(bool realTouch){
		
		int count=0;
		
		if (realTouch || EasyTouch.instance.enableRemote){
			count = Input.touchCount;
		}
		else{
			if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)){
				count=1;
				if (Input.GetKey(UnityEngine.KeyCode.LeftAlt) || Input.GetKey(EasyTouch.instance.twistKey)|| Input.GetKey(UnityEngine.KeyCode.LeftControl) ||Input.GetKey(EasyTouch.instance.swipeKey ))
					count=2;
				if (Input.GetKeyUp(UnityEngine.KeyCode.LeftAlt)|| Input.GetKeyUp(EasyTouch.instance.twistKey)|| Input.GetKeyUp(UnityEngine.KeyCode.LeftControl)|| Input.GetKeyUp(EasyTouch.instance.swipeKey))
					count=2;
			}		
		}
		
		return count;
	}
		
	// return in Finger structure all informations on an touch
	public Finger GetMouseTouch(int fingerIndex,Finger myFinger){
		
		Finger finger;
		
		if (myFinger!=null){
			finger = myFinger;
		}
		else{
			finger = new Finger();
			finger.gesture = EasyTouch.GestureType.None;
		}
		
		
		if (fingerIndex==1 && (Input.GetKeyUp(UnityEngine.KeyCode.LeftAlt)|| Input.GetKeyUp(EasyTouch.instance.twistKey)|| Input.GetKeyUp(UnityEngine.KeyCode.LeftControl)|| Input.GetKeyUp(EasyTouch.instance.swipeKey))){
			
			finger.fingerIndex = fingerIndex;
			finger.position = oldFinger2Position; 
			finger.deltaPosition = finger.position - oldFinger2Position;
			finger.tapCount = tapCount[fingerIndex];
			finger.deltaTime = Time.realtimeSinceStartup-deltaTime[fingerIndex];
			finger.phase = TouchPhase.Ended;
			
			return finger;			
		}
			
		if (Input.GetMouseButton(0)){
			
			finger.fingerIndex = fingerIndex;
			finger.position = GetPointerPosition(fingerIndex);
			
			if (Time.realtimeSinceStartup-tapeTime[fingerIndex]>0.5){
				tapCount[fingerIndex]=0;
			}
			
			if (Input.GetMouseButtonDown(0) || (fingerIndex==1 && (Input.GetKeyDown(UnityEngine.KeyCode.LeftAlt)|| Input.GetKeyDown(EasyTouch.instance.twistKey)|| Input.GetKeyDown(UnityEngine.KeyCode.LeftControl)|| Input.GetKeyDown(EasyTouch.instance.swipeKey)))){
				
				// Began						
				finger.position = GetPointerPosition(fingerIndex);
				finger.deltaPosition = UnityEngine.Vector2.zero;
				tapCount[fingerIndex]=tapCount[fingerIndex]+1;
				finger.tapCount = tapCount[fingerIndex];
				startActionTime[fingerIndex] = Time.realtimeSinceStartup;
				deltaTime[fingerIndex] = startActionTime[fingerIndex];
				finger.deltaTime = 0;
				finger.phase = TouchPhase.Began;
				
				
				if (fingerIndex==1){
					oldFinger2Position = finger.position;
				}
				else{
					oldMousePosition[fingerIndex] = finger.position;
				}

				if (tapCount[fingerIndex]==1){
					tapeTime[fingerIndex] = Time.realtimeSinceStartup;
				}

				
				return finger;
			}	
     

       		finger.deltaPosition = finger.position - oldMousePosition[fingerIndex];
       		
       		finger.tapCount = tapCount[fingerIndex];
       		finger.deltaTime = Time.realtimeSinceStartup-deltaTime[fingerIndex];
			if (finger.deltaPosition.sqrMagnitude <1){
				finger.phase = TouchPhase.Stationary;
			}
			else{
				finger.phase = TouchPhase.Moved;
			}

			oldMousePosition[fingerIndex] = finger.position;
			deltaTime[fingerIndex] = Time.realtimeSinceStartup;
        			
			return finger;
		}
		
		else if (Input.GetMouseButtonUp(0)){
			finger.fingerIndex = fingerIndex;
			finger.position = GetPointerPosition(fingerIndex);
			finger.deltaPosition = finger.position - oldMousePosition[fingerIndex];
			finger.tapCount = tapCount[fingerIndex];
			finger.deltaTime = Time.realtimeSinceStartup-deltaTime[fingerIndex];
			finger.phase = TouchPhase.Ended;
			oldMousePosition[fingerIndex] = finger.position;
			
			return finger;
		}
			
		
		return null;
	}

	// Get the position of the simulate second finger
	public UnityEngine.Vector2 GetSecondFingerPosition(){
		
		UnityEngine.Vector2 pos = new UnityEngine.Vector2(-1,-1);
		
		if ((Input.GetKey(UnityEngine.KeyCode.LeftAlt)|| Input.GetKey(EasyTouch.instance.twistKey)) && (Input.GetKey(UnityEngine.KeyCode.LeftControl)|| Input.GetKey(EasyTouch.instance.swipeKey))){
			if (!bComplex){
				bComplex=true;
				deltaFingerPosition = (UnityEngine.Vector2)Input.mousePosition - oldFinger2Position;
			}
			pos = GetComplex2finger();
			return pos;
		}
		else if (Input.GetKey(UnityEngine.KeyCode.LeftAlt)|| Input.GetKey(EasyTouch.instance.twistKey) ){		
			pos =  GetPinchTwist2Finger();
			bComplex = false;
			return pos;
		}else if (Input.GetKey(UnityEngine.KeyCode.LeftControl)|| Input.GetKey(EasyTouch.instance.swipeKey) ){	

			pos =GetComplex2finger();
			bComplex = false;
			return pos;
		}
		
		return pos;		
	}
	#endregion
	
	#region Private methods
	// Get the postion of simulate finger
	private UnityEngine.Vector2 GetPointerPosition(int index){
	
		UnityEngine.Vector2 pos;
		
		if (index==0){
			pos= Input.mousePosition;
			return pos;
		}
		else{
			return GetSecondFingerPosition();
			
		}
	}
	
	// Simulate for a twist or pinc
	private UnityEngine.Vector2 GetPinchTwist2Finger(){

		UnityEngine.Vector2 position;
		
		if (complexCenter==UnityEngine.Vector2.zero){
			position.x = (Screen.width/2.0f) - (Input.mousePosition.x - (Screen.width/2.0f)) ;
			position.y = (Screen.height/2.0f) - (Input.mousePosition.y - (Screen.height/2.0f));
		}
		else{
			position.x = (complexCenter.x) - (Input.mousePosition.x - (complexCenter.x)) ;
			position.y = (complexCenter.y) - (Input.mousePosition.y - (complexCenter.y));
		}
		oldFinger2Position = position;
		
		return position;
	}
	
	// complexe Alt + Ctr
	private UnityEngine.Vector2 GetComplex2finger(){
	
		UnityEngine.Vector2 position;
		
		position.x = Input.mousePosition.x - deltaFingerPosition.x;
		position.y = Input.mousePosition.y - deltaFingerPosition.y;
		
		complexCenter = new UnityEngine.Vector2((Input.mousePosition.x+position.x)/2f, (Input.mousePosition.y+position.y)/2f);
		oldFinger2Position = position;
		
		return position;
	}
	#endregion
}


