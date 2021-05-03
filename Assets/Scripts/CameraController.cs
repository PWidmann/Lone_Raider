using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [SerializeField] Transform cameraTarget;
    [SerializeField] float smoothTime = 0.5f;

    private Vector3 velocity;
    private Vector3 correctedCamTarget;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void LateUpdate()
    {
        if (cameraTarget)
        {
            correctedCamTarget = cameraTarget.position;
            correctedCamTarget.z = transform.position.z;
            transform.position = Vector3.SmoothDamp(transform.position, correctedCamTarget, ref velocity, smoothTime);
        }
    }

    public void SetCameraTarget(GameObject targetObject)
    {
        cameraTarget = targetObject.transform;
    }
}
