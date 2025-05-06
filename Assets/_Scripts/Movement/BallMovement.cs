.
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallMovement : MonoBehaviour
{ 
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    public float speed = 10f;
    public Transform cam;
    public float jumpForce = 5f; // Jump force value
    private bool isGrounded;
    public object Player { get; set; }
    public CinemachineFreeLook freeLookCamera;

    private Vector3 currentVelocity; // For smoothing the movement

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x; 
        movementY = movementVector.y;
    }

    void Update()
    {
        // Ground check using raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.5f);

        // Check for jump input (assuming you want space key to jump)
        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            Jump();
        }

        // Smooth transition for camera zoom (already implemented in the original code)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 12.5f;
            freeLookCamera.m_Orbits[1].m_Radius = 3.5f;
        }
        else
        {
            speed = 10f;
            freeLookCamera.m_Orbits[1].m_Radius = 3f;
        }
    }

    private void FixedUpdate()
    {
        // Get camera forward and right directions
        Vector3 right = cam.right;
        Vector3 forward = cam.forward;

        // Ensure no vertical movement by resetting Y values
        right.y = 0;
        forward.y = 0;
        right.Normalize();
        forward.Normalize();

        // Calculate desired movement vector
        Vector3 desiredMovement = (right * movementX + forward * movementY) * speed;

        // Smooth the movement using SmoothDamp
        rb.velocity = Vector3.SmoothDamp(rb.velocity, new Vector3(desiredMovement.x, rb.velocity.y, desiredMovement.z), ref currentVelocity, 0.1f);
    }

    private void Jump()
    {
        // Apply an upward force when grounded
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
