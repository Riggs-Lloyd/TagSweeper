using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // make a singleton
    public static InputManager instance;

    private Controls _controls;

    // make properties for all our controls
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }

    public InputAction FireAction { get; private set; }
    public InputAction AimAction { get; private set; }
    public InputAction ReloadAction { get; private set; }
    public InputAction SprayAction { get; private set; }
    public InputAction RunAction { get; private set; }

    public bool FireDown { get; private set; }
    public bool AimDown { get; private set; }
    
    public bool ReloadDown { get; private set; }
    public bool SprayDown { get; private set; }
    public bool RunningTrue { get; private set; }
    public bool NotRunning { get; private set; }
    
    

    public void Awake()
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

    }


    void Start()
    {
        // Capture input for firing and aiming
        // event += parameters => *code*
        _controls.Locomotion.Shoot.performed += context => FireDown = true;
        _controls.Locomotion.Shoot.canceled += context => FireDown = false;

        _controls.Locomotion.Aim.performed += context => AimDown = true;
        _controls.Locomotion.Aim.canceled += context => AimDown = false;

        _controls.Locomotion.Reload.performed += context => ReloadDown = true;
        _controls.Locomotion.Reload.canceled += context => ReloadDown = false;
    }


    void Update()
    {
        // Capture input for values that constantly change
        Move = _controls.Locomotion.Move.ReadValue<Vector2>();
        Look = _controls.Locomotion.Look.ReadValue<Vector2>();
    }
}