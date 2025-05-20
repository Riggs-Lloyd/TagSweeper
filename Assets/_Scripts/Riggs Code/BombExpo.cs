using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class BombExpo : MonoBehaviour
{

     public UnityEvent uEvent;
     public GameObject TriggerObject;

     public void OnTriggerEnter(Collider Collider)
     {
          if(Collider.gameObject == TriggerObject)
          uEvent.Invoke();
     }
}
