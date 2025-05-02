using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputManager _input;
    private CharacterController _controller;

    
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    private bool IsRunning;
    [Header("Stamina System")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina;
    [SerializeField] private float staminaDrainRate = 25f;  // per second
    [SerializeField] private float staminaRegenRate = 15f;  // per second
    [SerializeField] private bool canRun = true;

    private new GunController Guns;
    // Start is called before the first frame update
    void Start()
    {
        _input = InputManager.instance;
        _controller = GetComponent<CharacterController>();
        _input.RunAction.performed += RunningChecker;
        _input.RunAction.canceled += NotRunningRn;
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement(Time.deltaTime);
        HandleStamina(Time.deltaTime);
    }

    private void HandleMovement(float delta)
    {
        // create a movement vector that is RELATIVE TO THE DIRECTION THE PLAYER IS FACING
        Vector3 moveDir = (_input.Move.x * transform.right) + (_input.Move.y * transform.forward);
        // tell the controller to move
        float currentSpeed = (IsRunning && canRun) ? 10f : 5f;
        _controller.Move(moveDir * (currentSpeed * delta));
        
    }

    private void HandleStamina(float delta)
    {
        bool isTryingToSprint = IsRunning && _input.Move != Vector2.zero;
        if (isTryingToSprint && canRun)
        {
            currentStamina -= staminaDrainRate * delta;
            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                canRun = false;
            }
        }
        else
        {
            currentStamina += staminaRegenRate * delta;
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }

            if (currentStamina > 10f)
            {
                canRun = true;
            }
        }
    }

    private void RunningChecker(InputAction.CallbackContext obj)
    {
        IsRunning = true;

    }

    private void NotRunningRn(InputAction.CallbackContext obj)
    {
        IsRunning = false;
    }
    
}