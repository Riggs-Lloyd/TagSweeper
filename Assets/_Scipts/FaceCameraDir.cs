using UnityEngine;

public class ObjectFaceCameraDirection : MonoBehaviour
{
	public Camera cameraToFollow; // Reference to the camera
	public bool keepYAxisRotation = true; // Whether to only apply rotation on the Y-axis
	public Vector3 rotationOffset; // Rotation offset (editable in Inspector)

	void Update()
	{
		if (cameraToFollow != null)
		{
			// Get the camera's forward direction, but ignore the Y-axis (we only want to rotate around Y-axis)
			Vector3 cameraForward = new Vector3(cameraToFollow.transform.forward.x, 0f, cameraToFollow.transform.forward.z);
            
			// Calculate the rotation to make the object face the camera
			Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

			// If you only want to rotate the object on the Y-axis (ignore pitch and roll)
			if (keepYAxisRotation)
			{
				targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
			}

			// Apply the rotation offset to the target rotation
			targetRotation *= Quaternion.Euler(rotationOffset);

			// Apply the rotation to the object
			transform.rotation = targetRotation;
		}
	}
}