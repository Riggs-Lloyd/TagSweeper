
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class BallMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private Vector3 currentVelocity;

    [Header("Movement Settings")]
    public float speed = 10f;
    public float sprintSpeed = 12.5f;

    [Header("Jump Settings")]
    public float jumpForce = 5f;
    private bool isGrounded = false;
    private bool jumpRequested = false;
    private bool jumpUsed = false;

    [Header("Camera")]
    public Transform cam;
    public CinemachineFreeLook freeLookCamera;

    [Header("Rotation")]
    public float rotationSpeed = 90f;
    public Transform myTargetTransform;
    private bool isRotating = false;

    private bool jumpPressed = false;  // Track spacebar press for jump
    private bool canRotate = false;    // Track if we are allowed to rotate (after jump)
    private float lastJumpTime = 0f;   // Time of the jump for second press timing control
    public float secondPressWindow = 1.5f;  // Time window for second press (in seconds)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    // Input System Movement Callback
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // Input System Jump Callback (Handles both jump and mid-air rotation)
    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (isGrounded && !jumpUsed)
            {
                // First press (on ground): Jump
                jumpRequested = true;
                jumpUsed = true; // Mark that we've jumped
                jumpPressed = true; // Set jumpPressed for the first jump
                lastJumpTime = Time.time; // Record the time of the jump press
                canRotate = false; // Prevent rotation immediately after jump
                Debug.Log("Jump pressed on the ground!");
            }
            else if (Input.GetKey(KeyCode.Space) && !jumpPressed && Time.time - lastJumpTime <= secondPressWindow)
            {
                // Second press (mid-air): Trigger rotation
                Debug.Log("Second space pressed mid-air!");
                StartCoroutine(RotateObject(myTargetTransform));
                jumpPressed = false; // Reset after rotation to prevent continuous rotations
            }
            else
            {
                Debug.Log("Space pressed, but conditions not met for rotation.");
            }
        }
    }

    void Update()
    {
        // Sprint camera adjustment
        if (Keyboard.current.leftShiftKey.isPressed)
        {
            speed = sprintSpeed;
            if (freeLookCamera != null)
                freeLookCamera.m_Orbits[1].m_Radius = 3.5f;
        }
        else
        {
            speed = 10f;
            if (freeLookCamera != null)
                freeLookCamera.m_Orbits[1].m_Radius = 3f;
        }

        // Reset canRotate flag if the player is grounded
        if (isGrounded)
        {
            canRotate = true; // Allow rotation after jump when grounded
        }
    }

    void FixedUpdate()
    {
        // Camera-relative movement
        Vector3 right = cam.right;
        Vector3 forward = cam.forward;
        right.y = 0;
        forward.y = 0;
        right.Normalize();
        forward.Normalize();

        Vector3 desiredMovement = (right * movementX + forward * movementY) * speed;
        rb.velocity = Vector3.SmoothDamp(rb.velocity, new Vector3(desiredMovement.x, rb.velocity.y, desiredMovement.z), ref currentVelocity, 0.1f);

        // Jump
        if (jumpRequested)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequested = false;
            isGrounded = false; // Prevent double jump until grounded
        }
    }

    // Rotate object coroutine
    private IEnumerator RotateObject(Transform target)
    {
        isRotating = true;

        float totalRotation = 0f;
        while (totalRotation < 360f)
        {
            float rotationThisFrame = rotationSpeed * Time.deltaTime;
            target.Rotate(0f, 0f, -rotationThisFrame);
            totalRotation += rotationThisFrame;
            yield return null;
        }

        float overshoot = totalRotation - 360f;
        target.Rotate(0f, 0f, overshoot);

        isRotating = false;
    }

    // Ground check
    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Angle(contact.normal, Vector3.up) < 45f)
            {
                isGrounded = true;
                jumpUsed = false; // Reset jump usage
                jumpPressed = false; // Reset jumpPressed when grounded
                //Debug.Log("Grounded, jumpUsed reset.");
                return;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        Debug.Log("Exiting ground.");
    }
}
