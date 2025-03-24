using UnityEngine;
using Cinemachine;

public class FreeLookOverride : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera; // Reference to the CinemachineFreeLook Camera
    private bool isRMBPressed = false; // Track if RMB is being held down

    // Sensitivity and smoothing controls
    public float rotationSpeedX = 0.05f; // Speed of horizontal rotation (lower value for slower movement)
    public float rotationSpeedY = 0.05f; // Speed of vertical rotation (lower value for slower movement)
    public float smoothingFactor = 0.1f; // Smoothing for mouse movement (increase for smoother movement)

    // Cap the velocity of the camera's movement
    public float maxRotationVelocity = 100f; // Maximum rotation velocity

    private float targetXRotation = 0f; // Target for horizontal rotation
    private float targetYRotation = 0f; // Target for vertical rotation
    private float currentXRotation = 0f; // Current horizontal rotation (for smooth transition)
    private float currentYRotation = 0f; // Current vertical rotation (for smooth transition)

    void Start()
    {
        if (freeLookCamera != null)
        {
            // Disable the default input provider in Cinemachine FreeLook
            freeLookCamera.m_XAxis.m_InputAxisName = "";
            freeLookCamera.m_YAxis.m_InputAxisName = "";
        }
        else
        {
            Debug.LogError("CinemachineFreeLook camera is not assigned!");
        }
    }

    void Update()
    {
        if (freeLookCamera != null)
        {
            // Check if the right mouse button (RMB) is being held down
            isRMBPressed = Input.GetMouseButton(1); // Right mouse button is 1

            // Debug log to confirm RMB status
            if (isRMBPressed)
            {
                Debug.Log("Right Mouse Button (RMB) is pressed.");
            }
            else
            {
                Debug.Log("Right Mouse Button (RMB) is released.");
            }

            if (isRMBPressed)
            {
                // Get mouse movement for X and Y axes
                float mouseX = Input.GetAxis("Mouse X"); // Mouse movement on X-axis
                float mouseY = Input.GetAxis("Mouse Y"); // Mouse movement on Y-axis

                // Update target rotations based on mouse input and scaling it down
                targetXRotation += mouseX * rotationSpeedX;
                targetYRotation -= mouseY * rotationSpeedY;

                // Clamp the vertical (Y) rotation to prevent flipping the camera
                targetYRotation = Mathf.Clamp(targetYRotation, -80f, 80f);

                // Cap the camera's velocity to avoid it rotating too fast
                targetXRotation = Mathf.Clamp(targetXRotation, -maxRotationVelocity, maxRotationVelocity);
                targetYRotation = Mathf.Clamp(targetYRotation, -maxRotationVelocity, maxRotationVelocity);
            }

            // Smoothly transition the current rotation to the target rotation
            currentXRotation = Mathf.Lerp(currentXRotation, targetXRotation, smoothingFactor);
            currentYRotation = Mathf.Lerp(currentYRotation, targetYRotation, smoothingFactor);

            // Apply the smoothed rotation to the Cinemachine FreeLook camera's X and Y axes
            freeLookCamera.m_XAxis.Value = currentXRotation;
            freeLookCamera.m_YAxis.Value = currentYRotation;
        }
    }
}
