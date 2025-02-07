using UnityEngine;
using Cinemachine;

public class CameraRotateOnRMB : MonoBehaviour
{
	public CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine virtual camera
	public Transform target; // The target the camera is rotating around
	public float rotationSpeed = 5f; // Speed of rotation
	private bool isRMBPressed = false; // Whether the RMB is pressed

	private void Update()
	{
		// Check if the right mouse button (RMB) is held down
		isRMBPressed = Input.GetMouseButton(1); // RMB is button 1

		// If RMB is held, allow rotation around the target
		if (isRMBPressed)
		{
			// Get mouse input for rotating the camera
			float mouseX = Input.GetAxis("Mouse X"); // Mouse movement on X-axis

			// Rotate the target based on the mouse movement
			if (target != null)
			{
				target.Rotate(Vector3.up * mouseX * rotationSpeed, Space.World);
			}
		}
	}
}