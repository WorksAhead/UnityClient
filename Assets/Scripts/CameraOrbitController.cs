using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbitController : MonoBehaviour {

    public Transform targetTransform;      // Camera target object transform, Camera will look at that target.
    public float startingDistance = 10f;   // Distance the camera starts from target object.
    public float maxDistance = 20f;        // Max distance the camera can be from target object.
    public float minDistance = 3f;         // Min distance the camera can be from target object.
    public float zoomSpeed = 20f;          // The speed the camera zooms in and out.
    public float targetHeight = 1.0f;      // The amount from the target object pivot the camera should look at.
    public float camRotationSpeed = 150;   // The speed at which the camera rotates.
    public float camXAngle = 45.0f;        // The camera x euler angle.

    private float mCamYAngle = 0.0f;       // The camera y euler angle.
    public Transform cameraTransform;      // The camera's transform.
    private float mMinCameraAngle = 0.0f;  // The min angle on the camera's x axis.
    private float mMaxCameraAngle = 90.0f; // The max angle on the camera's x axis.

    public bool rotateLeft = false;
    public bool rotateRight = false;
    public bool rotateUp = false;
    public bool rotateDown = false;
    public bool zoomIn = false;
    public bool zoomOut = false;

    private Vector3 mOriginalRotation;
    private Vector3 mOriginalPosition;

    // Use this for initialization
    public bool Init()
    {
        if (targetTransform == null)
        {
            return false;
        }

        if (cameraTransform == null)
        {
            return false;
        }

        // Get camera game object's transform.
        cameraTransform.position = targetTransform.position;
        Vector3 angles = cameraTransform.eulerAngles;
        // Set default y angle.
        mCamYAngle = angles.y;

        mOriginalRotation = angles;
        Quaternion rotation = Quaternion.Euler(camXAngle, mCamYAngle, 0);
        Vector3 trm = rotation * Vector3.forward * startingDistance + new Vector3(0, -1 * targetHeight, 0);
        mOriginalPosition = targetTransform.position - trm;

        return true;
    }
	
	// Update is called once per frame
	public void DoUpdate() {
        if (rotateLeft)
        {
            cameraTransform.RotateAround(targetTransform.transform.position, new Vector3(0, 1, 0), camRotationSpeed * Time.deltaTime);
            mCamYAngle = cameraTransform.eulerAngles.y;
        }
        else if (rotateRight)
        {
            cameraTransform.RotateAround(targetTransform.transform.position, new Vector3(0, 1, 0), -camRotationSpeed * Time.deltaTime);
            mCamYAngle = cameraTransform.eulerAngles.y;
        }

        if (rotateUp)
        {
            camXAngle += camRotationSpeed * Time.deltaTime;
            if (camXAngle > mMaxCameraAngle)
            {
                camXAngle = mMaxCameraAngle;
            }
        }
        else if (rotateDown)
        {
            camXAngle += -camRotationSpeed * Time.deltaTime;
            if (camXAngle < mMinCameraAngle)
            {
                camXAngle = mMinCameraAngle;
            }
        }

        if (zoomIn)
        {
            startingDistance -= Time.deltaTime * zoomSpeed;
            if (startingDistance < minDistance)
            {
                startingDistance = minDistance;
            }
        }
        else if (zoomOut)
        {
            startingDistance += Time.deltaTime * zoomSpeed;
            if (startingDistance > maxDistance)
            {
                startingDistance = maxDistance;
            }
        }

        // Set camera angles.
        Quaternion rotation = Quaternion.Euler(camXAngle, mCamYAngle, 0);
        cameraTransform.rotation = rotation;
        // Position Camera.
        Vector3 trm = rotation * Vector3.forward * startingDistance + new Vector3(0, -1 * targetHeight, 0);
        Vector3 position = targetTransform.position - trm;
        cameraTransform.position = position;
    }
}
