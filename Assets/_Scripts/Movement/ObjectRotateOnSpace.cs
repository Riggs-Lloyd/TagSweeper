using UnityEngine;

public class ObjectRotateOnSpace : MonoBehaviour
{
	public float rotationSpeed = 10f; // Speed of the rotation (degrees per second)
	private bool isRotating = false; // Whether the object is rotating

	void Update()
	{
		// Check if the Spacebar is pressed
		if (Input.GetKeyDown(KeyCode.Space) && !isRotating)
		{
			// Start rotating the object
			StartCoroutine(RotateObject());
		}
	}

	private System.Collections.IEnumerator RotateObject()
	{
		isRotating = true;

		float totalRotation = 0f; // Total rotation applied
		while (totalRotation < 360f)
		{
			float rotationThisFrame = rotationSpeed * Time.deltaTime; // Calculate rotation for this frame
			transform.Rotate(0f, 0f, -rotationThisFrame); // Rotate the object around the Z-axis

			totalRotation += rotationThisFrame; // Keep track of the total rotation

			// Wait for the next frame
			yield return null;
		}

		// Ensure the object ends up exactly at 360 degrees
		transform.Rotate(0f, 0f, -360f - totalRotation);

		isRotating = false; // Done rotating
	}
}