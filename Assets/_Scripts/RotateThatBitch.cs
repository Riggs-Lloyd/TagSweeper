using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateThatBitch : MonoBehaviour
{
    public float rotationSpeed = 20f; // Speed of rotation (degrees per second)

    void Update()
    {
        // Rotate the object around the Y-axis by the specified speed
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
