using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private InputManager _input;

    [SerializeField] private Transform playerParent;  // Parent object of the player (used for rotating the body horizontally)
    [SerializeField] private float sensitivity = 5f;
    [SerializeField] private bool isFirstPerson = true;  // A flag to check whether we're in first-person or third-person view
    private float _xRot = 0f;  // For vertical rotation (up/down)

    // Start is called before the first frame update
    void Start()
    {
        _input = InputManager.instance;
        Cursor.lockState = CursorLockMode.Locked;  // Lock the cursor in the middle of the screen
        Cursor.visible = false;  // Make sure the cursor is invisible when in locked mode
    }

    // Update is called once per frame
    void Update()
    {
        HandleLook(Time.deltaTime);  // Handle mouse look each frame
    }

    private void HandleLook(float delta)
    {
        // Only process mouse input if in first-person view
        if (isFirstPerson)
        {
            // Get mouse input for horizontal and vertical axes
            float mouseX = _input.Look.x * delta * sensitivity;
            float mouseY = _input.Look.y * delta * sensitivity;

            // Adjust the vertical rotation
            _xRot -= mouseY;
            _xRot = Mathf.Clamp(_xRot, -90f, 90f);  // Prevent excessive up/down rotation

            // Apply the local rotation to the camera
            transform.localRotation = Quaternion.Euler(_xRot, 0f, 0f);  // Up/down rotation (camera only)

            // Apply the horizontal rotation to the player (body rotation)
            playerParent.Rotate(Vector3.up * mouseX);  // Left/right rotation (player body)
        }
        else
        {
            // For third-person view, we only want to rotate the player horizontally (no up/down camera rotation)
            float mouseX = _input.Look.x * delta * sensitivity;

            // Apply only horizontal rotation to the player (body rotation)
            playerParent.Rotate(Vector3.up * mouseX);  // Left/right rotation (player body)
        }
    }

    // You can call this method to switch between first-person and third-person views externally
    public void SetFirstPerson(bool firstPerson)
    {
        isFirstPerson = firstPerson;
    }
}
