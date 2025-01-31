using UnityEngine;

namespace _Scipts
{
    public class LockCameraRotation : MonoBehaviour
    {
        private float rotationSpeed = 5f;
        private float rotationX = 0f;
        private float rotationY = 0f;
        
        public float minRotationY = -60f;
        public float maxRotationY = 60f;

        private void Update()
        {
            // Only allow rotation if the right mouse button is held down
            if (Input.GetMouseButton(1)) // Right-click is held
            {
                
            } else
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                // Update rotation values
                rotationX += mouseX * rotationSpeed;
                rotationY -= mouseY * rotationSpeed;

                // Clamp vertical rotation to avoid flipping the camera upside down
                rotationY = Mathf.Clamp(rotationY, minRotationY, maxRotationY);

                // Apply the rotation
                transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);
            }
        }
    }
}
