using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class BombExpo : MonoBehaviour
{
    
    public GameObject[] tiles = new GameObject[8];
    
  
    
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void checkAdjacent()
    {
        //check each directions (N,NE,E...) and see if that tiles isBombTrue is true or not if yes the add to number val

        for (int i = 0; i < 9; i++)
        {
           if (tiles == true)
            {
                
            }   
        }
    } 
}
