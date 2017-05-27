using UnityEngine;
using System.Collections;

public class MB_ExampleMover : UnityEngine.MonoBehaviour {
	
	public int axis = 0;
	
	void Update () {
		UnityEngine.Vector3 v1 = new UnityEngine.Vector3(5f,5f,5f);
		v1[axis] *= UnityEngine.Mathf.Sin(Time.time);
		transform.position = v1;
	}
}
