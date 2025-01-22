using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static InputManager inputManager;
    private Controls _controls;
    private float speed = 10f;
    
    
    
    private void Awake()
    {
        if (inputManager == null)
        {
            inputManager = this;
        }
        else
        {
            Destroy(this);
        }

        _controls = new Controls();
        _controls.Enable();

    }

    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime);



    }
}
