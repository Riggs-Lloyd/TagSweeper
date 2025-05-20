using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    
    public GameObject[] tiles = new GameObject[2];
    private int n = 1;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(tiles[n].name + " is at index " + n);

    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
