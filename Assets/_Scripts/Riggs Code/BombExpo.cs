using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class BombExpo : MonoBehaviour
{
    
    public GameObject[] tiles = new GameObject[3];
    

    private void Start()
    {
        checkAdjacent();
    }

    private void Update()
    {
        
    }
   

    private void checkAdjacent()
    {
        //check each directions (N,NE,E...) and see if that tiles isBombTrue is true or not if yes the add to number val
        

       // foreach (bool bombVal in tiles)
      //  {
      //      GetComponent<Tile>().isBombTrue = n;
       //     if (n == true)
         //   {
       //         Debug.Log("Meow");
        //    }
      //  }

    } 
    
    
    
}
