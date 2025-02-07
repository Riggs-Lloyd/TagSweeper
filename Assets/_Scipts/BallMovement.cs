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
 public object Player { get; set; }

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
 private void FixedUpdate() 
    {
       
       Vector3 right = cam.right;
       Vector3 forward = cam.forward;
       
       right.y = 0;  // Ignore the Y axis to prevent vertical movement
       forward.y = 0;  // Ignore the Y axis to prevent vertical movement
       right.Normalize();
       forward.Normalize();
       
       Vector3 movement = (right * movementX + forward * movementY) * speed;
        
       rb.AddForce(movement, ForceMode.Force);
    }
}