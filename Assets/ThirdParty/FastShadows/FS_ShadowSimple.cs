using UnityEngine;
using System.Collections;

public class FS_MeshKey{
	public bool isStatic;
	public Material mat;
	
	public FS_MeshKey(Material m, bool s){
		isStatic = s;
		mat = m;
	}
}

[AddComponentMenu("Fast Shadows/Simple Shadow")]
public class FS_ShadowSimple : UnityEngine.MonoBehaviour {
	[HideInInspector()] public float maxProjectionDistance = 3f;
	[HideInInspector()] public float girth = 1f;
	[HideInInspector()] public float shadowHoverHeight = .2f;
	public LayerMask layerMask = ~0;
	[HideInInspector()] public Material shadowMaterial;
	[HideInInspector()] public bool isStatic = false;
	
	[HideInInspector()] public bool useLightSource = false;
	[HideInInspector()] public UnityEngine.GameObject lightSource = null;
	[HideInInspector()] public UnityEngine.Vector3 lightDirection = new UnityEngine.Vector3(0f, -1, 0f);
	[HideInInspector()] public bool isPerspectiveProjection = false;
	[HideInInspector()] public bool doVisibilityCulling = false;
	 public UnityEngine.Vector3 shadowpostion = new UnityEngine.Vector3(0f, 0.2f, 0f);
	
	[HideInInspector()] public Rect uvs = new Rect(0f, 0f, 1f, 1f);
	
	float _girth;
	bool _isStatic;
	
	UnityEngine.Vector3 _lightDirection = UnityEngine.Vector3.zero;
	
	bool isGoodPlaneIntersect = false;
	UnityEngine.Color gizmoColor = UnityEngine.Color.white;
	
	//corner points of the generated quad
	UnityEngine.Vector3[] _corners = new UnityEngine.Vector3[4];
	
	Ray r = new Ray();
	RaycastHit rh = new RaycastHit();
	Bounds bounds = new Bounds();
	
	public UnityEngine.Vector3[] corners{
		get {return _corners;}
	}	
	
	UnityEngine.Color _color = new UnityEngine.Color(1f,1f,1f,1f);
	public UnityEngine.Color color{
		get {return _color;}
	}
	
	UnityEngine.Vector3 _normal = UnityEngine.Vector3.up;
	public UnityEngine.Vector3 normal{
		get {return _normal;}
	}
	
	// four points girth distance from shadowcaster
	UnityEngine.GameObject[] cornerGOs = new UnityEngine.GameObject[4];
	UnityEngine.GameObject shadowCaster;
	Plane shadowPlane = new Plane();
	FS_MeshKey meshKey;
	
	void Awake () {
		_isStatic = isStatic;
		if (shadowMaterial == null){
			shadowMaterial = (Material) Resources.Load("FS_ShadowMaterial");
			if (shadowMaterial == null) Debug.LogWarning("Shadow Material is not set for " + name);
		} 
		if (isStatic){
			CalculateShadowGeometry();
		}
	}


    int count = 0;
    UnityEngine.Vector3 oldPosition ;
	public void CalculateShadowGeometry(){
        //UnityEngine.Vector3 proj;
        if (shadowMaterial == null)
        {
            return;
        }
       
		
        //if (useLightSource && lightSource == null){
        //    useLightSource = false;
        //    Debug.LogWarning("No light source object given using light direction vector.");
        //}
        //if (useLightSource){
        //    UnityEngine.Vector3 ls = transform.position - lightSource.transform.position;
        //    float mag = ls.magnitude;
        //    if (mag != 0f){
        //        lightDirection = ls / mag;
        //    } else {
        //        return; //object is on top of light source	
        //    }
        //} else if (lightDirection != _lightDirection || lightDirection == UnityEngine.Vector3.zero){ //light direction is dirty
        //    if (lightDirection == UnityEngine.Vector3.zero){
        //        Debug.LogWarning("Light Direction vector cannot be zero. assuming -y.");
        //        lightDirection = -UnityEngine.Vector3.up;
        //    }
        //    lightDirection.Normalize();	
        //    _lightDirection = lightDirection;
        //}
		
		if (shadowCaster == null || girth != _girth){
			if (shadowCaster == null){
				shadowCaster = new UnityEngine.GameObject("shadowSimple");
				cornerGOs = new UnityEngine.GameObject[4];
				for (int i = 0; i < 4; i++){
					UnityEngine.GameObject c = cornerGOs[i] = new UnityEngine.GameObject("c" + i);	
					c.transform.parent = shadowCaster.transform;
				}
				shadowCaster.transform.parent = transform;		
				shadowCaster.transform.localPosition = shadowpostion;

				shadowCaster.transform.localRotation = UnityEngine.Quaternion.identity;
				shadowCaster.transform.localScale = UnityEngine.Vector3.one;
			}
            //if (UnityEngine.Mathf.Abs(UnityEngine.Vector3.Dot(transform.forward, lightDirection)) < .9f){
            //    proj = transform.forward - UnityEngine.Vector3.Dot(transform.forward, lightDirection) * lightDirection; 
            //} else {
            //    proj = transform.up - UnityEngine.Vector3.Dot(transform.up, lightDirection) * lightDirection;
            //}
            //shadowCaster.transform.rotation = UnityEngine.Quaternion.LookRotation(proj, -lightDirection);			
			cornerGOs[0].transform.position =  shadowCaster.transform.position + girth * (shadowCaster.transform.forward - shadowCaster.transform.right);
			cornerGOs[1].transform.position =  shadowCaster.transform.position + girth * (shadowCaster.transform.forward + shadowCaster.transform.right);
			cornerGOs[2].transform.position =  shadowCaster.transform.position + girth * (-shadowCaster.transform.forward + shadowCaster.transform.right);
			cornerGOs[3].transform.position =  shadowCaster.transform.position + girth * (-shadowCaster.transform.forward - shadowCaster.transform.right);						
			_girth = girth;
		}		

        
		UnityEngine.Transform t = shadowCaster.transform;		
		
		r.origin = t.position;
		r.direction = lightDirection;
        while (count == 0)
        {
            if(oldPosition == this.transform.position)
                break;

            oldPosition.Set(this.transform.position.x, this.transform.position.y, this.transform.position.z);            

            if (maxProjectionDistance > 0f && Physics.Raycast(r, out rh, maxProjectionDistance, layerMask))
            {             
                //if (doVisibilityCulling && !isPerspectiveProjection){
                //    Plane[] camPlanes = FS_ShadowManager.Manager().getCameraFustrumPlanes();
                //    bounds.center = rh.point;
                //    bounds.size = new UnityEngine.Vector3(2f*girth,2f*girth,2f*girth);
                //    if (!GeometryUtility.TestPlanesAABB(camPlanes,bounds)){
                //        return;	
                //    }
                //}

                //// Rotate Shadowcaster 
                ////project forward or up vector onto a plane whos normal is lightDirection
                //if (UnityEngine.Mathf.Abs(UnityEngine.Vector3.Dot(transform.forward, lightDirection)) < .9f){
                //    proj = transform.forward - UnityEngine.Vector3.Dot(transform.forward, lightDirection) * lightDirection;
                //} else {
                //    proj = transform.up - UnityEngine.Vector3.Dot(transform.up, lightDirection) * lightDirection;
                //}
                //shadowCaster.transform.rotation = UnityEngine.Quaternion.Lerp(shadowCaster.transform.rotation, UnityEngine.Quaternion.LookRotation(proj, -lightDirection), .01f);			

                //float alpha;
                //float dist = rh.distance - shadowHoverHeight;
                //alpha = 1.0f - dist / maxProjectionDistance;
                //if (alpha < 0f)
                //    return;
                //alpha = UnityEngine.Mathf.Clamp01(alpha);
                //_color.a = alpha;
                //UnityEngine.Profiler.BeginSample("11111111111");
                _color.a = 1;

                _normal = rh.normal;
                UnityEngine.Vector3 hitPoint = rh.point - shadowHoverHeight * lightDirection;
                shadowPlane.SetNormalAndPosition(_normal, hitPoint);

                isGoodPlaneIntersect = true;

                float rayDist = 0f;
                //UnityEngine.Profiler.EndSample();
                //float mag = 0f;
                //if (useLightSource && isPerspectiveProjection){
                //    r.origin = lightSource.transform.position;
                //    UnityEngine.Vector3 lightSource2Corner = cornerGOs[0].transform.position - lightSource.transform.position;
                //    mag = lightSource2Corner.magnitude;
                //    r.direction = lightSource2Corner / mag;
                //    isGoodPlaneIntersect = isGoodPlaneIntersect && shadowPlane.Raycast(r, out rayDist);
                //    _corners[0] = r.origin + r.direction * rayDist;	

                //    lightSource2Corner = cornerGOs[1].transform.position - lightSource.transform.position;
                //    r.direction = lightSource2Corner / mag;
                //    isGoodPlaneIntersect = isGoodPlaneIntersect && shadowPlane.Raycast(r, out rayDist);
                //    _corners[1] = r.origin + r.direction * rayDist;

                //    lightSource2Corner = cornerGOs[2].transform.position - lightSource.transform.position;
                //    r.direction = lightSource2Corner / mag;
                //    isGoodPlaneIntersect = isGoodPlaneIntersect && shadowPlane.Raycast(r, out rayDist);
                //    _corners[2] = r.origin + r.direction * rayDist;

                //    lightSource2Corner = cornerGOs[3].transform.position - lightSource.transform.position;
                //    r.direction = lightSource2Corner / mag;
                //    isGoodPlaneIntersect = isGoodPlaneIntersect && shadowPlane.Raycast(r, out rayDist);
                //    _corners[3] = r.origin + r.direction * rayDist;
                //    if (doVisibilityCulling){
                //        Plane[] camPlanes = FS_ShadowManager.Manager().getCameraFustrumPlanes();
                //        bounds.center = rh.point;
                //        bounds.size = UnityEngine.Vector3.zero;
                //        bounds.Encapsulate(_corners[0]);
                //        bounds.Encapsulate(_corners[1]);
                //        bounds.Encapsulate(_corners[2]);
                //        bounds.Encapsulate(_corners[3]);
                //        if (!GeometryUtility.TestPlanesAABB(camPlanes,bounds)){
                //            return;	
                //        }
                //    }
                //} else {
                //UnityEngine.Profiler.BeginSample("222222");
                r.origin = cornerGOs[0].transform.position;
                isGoodPlaneIntersect = shadowPlane.Raycast(r, out rayDist);
                if (isGoodPlaneIntersect == false && rayDist == 0f)
                {
                    return;
                }
                else
                {
                    isGoodPlaneIntersect = true;
                }
                _corners[0] = r.origin + r.direction * rayDist;

                r.origin = cornerGOs[1].transform.position;
                isGoodPlaneIntersect = shadowPlane.Raycast(r, out rayDist);
                if (isGoodPlaneIntersect == false && rayDist == 0f)
                {
                    return;
                }
                else
                {
                    isGoodPlaneIntersect = true;
                }
                _corners[1] = r.origin + r.direction * rayDist;

                r.origin = cornerGOs[2].transform.position;
                isGoodPlaneIntersect = shadowPlane.Raycast(r, out rayDist);
                if (isGoodPlaneIntersect == false && rayDist == 0f)
                {
                    return;
                }
                else
                {
                    isGoodPlaneIntersect = true;
                }
                _corners[2] = r.origin + r.direction * rayDist;

                r.origin = cornerGOs[3].transform.position;
                isGoodPlaneIntersect = shadowPlane.Raycast(r, out rayDist);
                if (isGoodPlaneIntersect == false && rayDist == 0f)
                {
                    return;
                }
                else
                {
                    isGoodPlaneIntersect = true;
                }
                _corners[3] = r.origin + r.direction * rayDist;
                //}
                //UnityEngine.Profiler.EndSample();
                //UnityEngine.Profiler.BeginSample("333");
                //if (isGoodPlaneIntersect)
                //{
                //    if (meshKey == null || meshKey.mat != shadowMaterial || meshKey.isStatic != isStatic) meshKey = new FS_MeshKey(shadowMaterial, isStatic);
                //    FS_ShadowManager.Manager().registerGeometry(this, meshKey);
                //    gizmoColor = UnityEngine.Color.white;
                //}
                //else
                //{
                //    gizmoColor = UnityEngine.Color.magenta;
                //}
                //UnityEngine.Profiler.EndSample();
            }
            else
            {
                isGoodPlaneIntersect = false;
                gizmoColor = UnityEngine.Color.red;
            }
            break;
        }
       
        //count++;
        //count =  count > 1 ?  0 : count;
      

        //UnityEngine.Profiler.BeginSample("333");
        if (isGoodPlaneIntersect)
        {
            if (meshKey == null || meshKey.mat != shadowMaterial || meshKey.isStatic != isStatic)
            {                
                meshKey = new FS_MeshKey(shadowMaterial, isStatic);
            }
            FS_ShadowManager.Manager().registerGeometry(this, meshKey);
            gizmoColor = UnityEngine.Color.white;
        }
        else
        {
            gizmoColor = UnityEngine.Color.magenta;
        }
        //UnityEngine.Profiler.EndSample();
	}
	
	void Update(){
		if (_isStatic != isStatic){
			if (isStatic == true){ //becoming static
				meshKey = new FS_MeshKey(shadowMaterial, isStatic);
				CalculateShadowGeometry();
				FS_ShadowManager.Manager().RecalculateStaticGeometry(null, meshKey);	
				
			} else { //becoming dynamic
				FS_MeshKey oldMeshKey = meshKey;
				meshKey = new FS_MeshKey(shadowMaterial, isStatic);
				FS_ShadowManager.Manager().RecalculateStaticGeometry(this, oldMeshKey);	
			}
			_isStatic = isStatic;
		}
		if (!isStatic){
            UnityEngine.Profiling.Profiler.BeginSample("CalculateShadowGeometry");
			CalculateShadowGeometry();
            UnityEngine.Profiling.Profiler.EndSample();
		}
	}
	
	void OnDrawGizmos(){
		if (shadowCaster != null){
			Gizmos.color = UnityEngine.Color.yellow;
			Gizmos.DrawRay(shadowCaster.transform.position, shadowCaster.transform.up);
			Gizmos.DrawRay(shadowCaster.transform.position, shadowCaster.transform.forward);
			Gizmos.DrawRay(shadowCaster.transform.position, shadowCaster.transform.right);
			Gizmos.color = UnityEngine.Color.blue;
			Gizmos.DrawRay(shadowCaster.transform.position, transform.forward);
			Gizmos.color = gizmoColor;
			if (isGoodPlaneIntersect){
				Gizmos.DrawLine(cornerGOs[0].transform.position, corners[0]);
				Gizmos.DrawLine(cornerGOs[1].transform.position, corners[1]);
				Gizmos.DrawLine(cornerGOs[2].transform.position, corners[2]);
				Gizmos.DrawLine(cornerGOs[3].transform.position, corners[3]);
				
				Gizmos.DrawLine(cornerGOs[0].transform.position, cornerGOs[1].transform.position);
				Gizmos.DrawLine(cornerGOs[1].transform.position, cornerGOs[2].transform.position);
				Gizmos.DrawLine(cornerGOs[2].transform.position, cornerGOs[3].transform.position);
				Gizmos.DrawLine(cornerGOs[3].transform.position, cornerGOs[0].transform.position);
				
				Gizmos.DrawLine(corners[0], corners[1]);
				Gizmos.DrawLine(corners[1], corners[2]);
				Gizmos.DrawLine(corners[2], corners[3]);
				Gizmos.DrawLine(corners[3], corners[0]);
				
				//draw the bounds
				//Gizmos.color = UnityEngine.Color.blue;
				//Gizmos.DrawWireCube(bounds.center,bounds.size);
			}
		}
	}
}
