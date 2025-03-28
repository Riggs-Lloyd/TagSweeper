using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallMovement : MonoBehaviour
{ 
   
   private Rigidbody rb;
 private float movementX;
 private float movementY;
 public float speed = 10;
 public Transform cam;
 void Start()
    {
       rb = GetComponent<Rigidbody>();

       cam = GetComponent<Camera>().transform;
    }
 void OnMove(InputValue movementValue)
    {
       Vector2 movementVector = movementValue.Get<Vector2>();
       
        movementX = movementVector.x; 
        movementY = movementVector.y;

       
    }
 private void FixedUpdate() 
    {
       Vector2 movement = (cam.right) + (cam.forward);
        
        rb.AddForce(movement * speed);
    }
}