using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GridInfo : MonoBehaviour
{
    [CanBeNull]
    public ArrayList row = new ArrayList();
    
    //---
    
    public GameObject[,] rows = new GameObject[2,2];
    private int k = 0;
    [SerializeField] private int Row;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("AH");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
