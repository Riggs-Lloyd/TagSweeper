using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputManager _input;
    private CharacterController _controller;
    private PlayerHealth _playerHealth;

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

    private bool isDead = false;

    [Header("Death and Respawn")]
    [SerializeField] private Canvas deathScreen; // Canvas UI for death screen
    private float respawnTime = 3f;  // Time to respawn after death

    private new GunController Guns;

    // Start is called before the first frame update
    void Start()
    {
        _input = InputManager.instance;
        _controller = GetComponent<CharacterController>();
        _playerHealth = GetComponent<PlayerHealth>();  // Get the PlayerHealth component
        
        // Subscribe to running actions
        _input.RunAction.performed += RunningChecker;
        _input.RunAction.canceled += NotRunningRn;
        
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return; // Skip movement if dead

        HandleMovement(Time.deltaTime);
        HandleStamina(Time.deltaTime);
    }

    private void HandleMovement(float delta)
    {
        // Only move if not dead
        if (isDead) return;

        // Create a movement vector relative to the direction the player is facing
        Vector3 moveDir = (_input.Move.x * transform.right) + (_input.Move.y * transform.forward);
        // Tell the controller to move based on the current speed (running or walking)
        float currentSpeed = (IsRunning && canRun) ? runSpeed : walkSpeed;
        _controller.Move(moveDir * (currentSpeed * delta));
    }

    private void HandleStamina(float delta)
    {
        if (isDead) return; // Don't handle stamina if dead

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

    // Disable movement and show the death screen
    public void DisableMovement()
    {
        isDead = true;

        // Disable the CharacterController to stop movement
        if (_controller != null)
        {
            _controller.enabled = false;
            _input.DisableForDeathScreen();
        }

        // Show the death screen if assigned
        if (deathScreen != null)
        {
            deathScreen.gameObject.SetActive(true);
        }
    }

    // Enable movement and hide the death screen
    public void Respawn()
    {
        isDead = false;
        currentStamina = maxStamina;  // Reset stamina

        // Hide the death screen
        if (deathScreen != null)
        {
            deathScreen.gameObject.SetActive(false);
        }

        // Re-enable movement by enabling the CharacterController
        if (_controller != null)
        {
            _controller.enabled = true;
        }
    }

    // Call this method when the player takes damage and reaches zero health
    private void Die()
    {
        DisableMovement(); // Disable movement and show death screen
        // Optional: Play death animation, sound, etc.
    }

    // Check if player is trying to sprint
    private void RunningChecker(InputAction.CallbackContext obj)
    {
        IsRunning = true;
    }

    // Check when the player is not trying to sprint
    private void NotRunningRn(InputAction.CallbackContext obj)
    {
        IsRunning = false;
    }

    // Example of taking damage (use this method with health system logic)
    public void TakeDamage(float damage)
    {
        if (isDead) return; // Skip damage if already dead

        // Call the PlayerHealth script to decrease health
        if (_playerHealth != null)
        {
            _playerHealth.TakeDamage(damage); // This handles the health system

            // If health is 0, call Die() to disable movement
            if (_playerHealth.GetCurrentHealth() <= 0)
            {
                Die();
            }
        }
    }
}

