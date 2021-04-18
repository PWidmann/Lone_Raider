using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform cameraTarget;
    [SerializeField] float smoothTime = 0.5f;

    private Vector3 velocity;
    private Vector3 correctedCamTarget;

    void LateUpdate()
    {
        correctedCamTarget = cameraTarget.position;
        correctedCamTarget.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, correctedCamTarget, ref velocity, smoothTime);
    }
}
