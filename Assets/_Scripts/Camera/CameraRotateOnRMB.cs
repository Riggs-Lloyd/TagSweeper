using UnityEngine;
using Cinemachine;

public class CameraRotateOnRMB : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine virtual camera
    public Transform target; // The target the camera is rotating around
    public float rotationSpeed = 5f; // Speed of rotation
    public float verticalRotationLimit = 80f; // Limit for vertical rotation (degrees)

    private bool isRMBPressed = false; // Whether the RMB is pressed
    private float currentVerticalRotation = 0f; // Current vertical rotation value
    private Transform cameraTransform; // To store the transform of the camera for easier reference
    private CinemachineOrbitalTransposer orbitalTransposer; // Reference to the orbital transposer
    

    private void Start()
    {
        // Cache the transform of the virtual camera for convenience
        cameraTransform = virtualCamera.transform;

        // Get the orbital transposer (which controls camera positioning relative to target)
        orbitalTransposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private void Update()
    {
        // Check if the right mouse button (RMB) is held down
        isRMBPressed = Input.GetMouseButton(1); // RMB is button 1

        // If RMB is held, allow rotation around the target
        if (isRMBPressed)
        {
            // Get mouse input for rotating the camera
            float mouseX = Input.GetAxis("Mouse X"); // Mouse movement on X-axis (horizontal)
            float mouseY = Input.GetAxis("Mouse Y"); // Mouse movement on Y-axis (vertical)

            // Rotate the target based on the horizontal mouse movement (for yaw)
            if (target != null)
            {
                // Horizontal rotation around the Y-axis (yaw)
                target.Rotate(Vector3.up * mouseX * rotationSpeed, Space.World);

                // Adjust the vertical rotation with a limit to prevent over-rotation
                currentVerticalRotation -= mouseY * rotationSpeed;
                currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, -verticalRotationLimit, verticalRotationLimit);

                // Apply the vertical rotation by rotating the FollowOffset around the X-axis (pitch)
                if (orbitalTransposer != null)
                {
                    // Modify the FollowOffset's pitch (rotation along X-axis)
                    orbitalTransposer.m_FollowOffset = Quaternion.Euler(currentVerticalRotation, orbitalTransposer.m_FollowOffset.y, 0) * orbitalTransposer.m_FollowOffset;
                }
            }
        }
    }
}

