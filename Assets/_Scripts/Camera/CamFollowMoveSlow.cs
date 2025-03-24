using UnityEngine;

public class ObjectMoveOnRMB : MonoBehaviour
{
    public float moveSpeed = 0.5f; // Speed at which the object moves along the Y-axis
    private float targetYPosition = 1f; // Starting Y position of the object (set to 1)
    private float currentYPosition = 1f; // Current Y position of the object
    private bool isRMBPressed = false; // Whether the RMB is held
    private bool isRMBReleased = false; // Whether the RMB has been released

    void Update()
    {
        // Check if the right mouse button (RMB) is held down
        isRMBPressed = Input.GetMouseButton(1); // RMB is button 1

        // Check if the right mouse button (RMB) has just been released
        if (Input.GetMouseButtonUp(1))
        {
            isRMBReleased = true;
        }

        // Only process movement if RMB is held
        if (isRMBPressed)
        {
            // Get mouse input for vertical movement (mouse Y)
            float mouseY = Input.GetAxis("Mouse Y"); // Positive when the mouse moves down, negative when up

            // If mouse moves up (negative value), decrease the Y position (move up)
            if (mouseY < 0)
            {
                targetYPosition = Mathf.MoveTowards(targetYPosition, 1f, moveSpeed * Time.deltaTime);
            }
            // If mouse moves down (positive value), increase the Y position (move down)
            else if (mouseY > 0)
            {
                targetYPosition = Mathf.MoveTowards(targetYPosition, 3f, moveSpeed * Time.deltaTime);
            }
        }
        // If RMB is released, slowly move the object back to its original position (Y = 1)
        else if (isRMBReleased)
        {
            targetYPosition = Mathf.MoveTowards(targetYPosition, 2f, moveSpeed * Time.deltaTime);
            if (Mathf.Abs(targetYPosition - 2f) < 0.01f) // Once it's very close to 1, reset the release flag
            {
                isRMBReleased = false;
            }
        }

        // Smoothly move the object to the target position
        currentYPosition = Mathf.Lerp(currentYPosition, targetYPosition, moveSpeed * Time.deltaTime);
        
        // Apply the new Y position to the object's transform
        transform.position = new Vector3(transform.position.x, currentYPosition, transform.position.z);
    }
}
