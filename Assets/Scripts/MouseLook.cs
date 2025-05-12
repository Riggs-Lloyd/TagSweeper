using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private InputManager _input;

    [SerializeField] private Transform playerParent;
    [SerializeField] private float sensitivity = 5;

    private float _xRot;

    // Start is called before the first frame update
    void Start()
    {
        _input = InputManager.instance;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandleLook(Time.deltaTime);
    }

    private void HandleLook(float delta)
    {
        // create the true values that we want to apply
        float mouseX = _input.Look.x * delta * sensitivity;
        float mouseY = _input.Look.y * delta * sensitivity;

        // avoid inverting look up/down
        _xRot -= mouseY;

        // clamp X rotation to avoid breaking neck
        _xRot = Mathf.Clamp(_xRot, -90, 90);

        // apply rotation to appropriate things.
        transform.localRotation = Quaternion.Euler(_xRot, 0, 0);

        playerParent.Rotate(Vector3.up, mouseX);
    }
}