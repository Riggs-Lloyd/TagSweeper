using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Singleton pattern for accessing from other scripts
    public static InputManager instance;

    private Controls _controls;

    public Canvas deathScreen;  // Reference to death screen UI

    // Input Actions and related properties
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }

    public InputAction FireAction { get; private set; }
    public InputAction AimAction { get; private set; }
    public InputAction ReloadAction { get; private set; }
    public InputAction SprayAction { get; private set; }
    public InputAction RunAction { get; private set; }
    public InputAction DamageTestingAction { get; private set; }

    public bool FireDown { get; private set; }
    public bool AimDown { get; private set; }
    public bool ReloadDown { get; private set; }
    public bool SprayDown { get; private set; }
    public bool RunningTrue { get; private set; }
    public bool NotRunning { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        _controls = new Controls();
        _controls.Enable();

        // Expose Full InputActions
        FireAction = _controls.Locomotion.Shoot;
        AimAction = _controls.Locomotion.Aim;
        ReloadAction = _controls.Locomotion.Reload;
        SprayAction = _controls.Locomotion.Spray;
        RunAction = _controls.Locomotion.Run;
        DamageTestingAction = _controls.Locomotion.DamageTesting;
    }

    private void Start()
    {
        // Capture input for firing and aiming
        _controls.Locomotion.Shoot.performed += context => FireDown = true;
        _controls.Locomotion.Shoot.canceled += context => FireDown = false;

        _controls.Locomotion.Aim.performed += context => AimDown = true;
        _controls.Locomotion.Aim.canceled += context => AimDown = false;

        _controls.Locomotion.Reload.performed += context => ReloadDown = true;
        _controls.Locomotion.Reload.canceled += context => ReloadDown = false;
        

        // Debugging log to check the status of input actions
        Debug.Log("InputManager Initialized");
    }

    private void Update()
    {
        // Capture input for values that constantly change
        Move = _controls.Locomotion.Move.ReadValue<Vector2>();
        Look = _controls.Locomotion.Look.ReadValue<Vector2>();
    }

    // Disable all input actions except those needed for the death screen
    public void DisableForDeathScreen()
    {
        if (deathScreen.isActiveAndEnabled)
        {
            Debug.Log("Disabling all actions except Look for death screen");

            // Disable movement and combat actions
            _controls.Locomotion.Move.Disable();
            _controls.Locomotion.Shoot.Disable();
            _controls.Locomotion.Aim.Disable();
            _controls.Locomotion.Reload.Disable();
            _controls.Locomotion.Spray.Disable();
            _controls.Locomotion.Run.Disable();

            // Enable only looking (if needed for UI interaction)
            _controls.Locomotion.Look.Enable();
        }
        else
        {
            Debug.Log("Death screen is not active. Input actions remain enabled.");
        }
    }

    // Re-enable all input actions when respawned or when not in the death screen
    public void EnableForNormalGameplay()
    {
        // Re-enable movement and combat actions
        Debug.Log("Re-enabling all actions for normal gameplay");

        _controls.Locomotion.Move.Enable();
        _controls.Locomotion.Shoot.Enable();
        _controls.Locomotion.Aim.Enable();
        _controls.Locomotion.Reload.Enable();
        _controls.Locomotion.Spray.Enable();
        _controls.Locomotion.Run.Enable();

        // Optionally, disable Look input when not needed
        _controls.Locomotion.Look.Disable();
    }
}
