using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 locationOffset;
    public Vector3 rotationOffset;

    private void FixedUpdate()
    {
        Vector3 desiredPostion = target.position + target.rotation * locationOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPostion, smoothSpeed);
        transform.position = smoothedPosition;


    }
}
