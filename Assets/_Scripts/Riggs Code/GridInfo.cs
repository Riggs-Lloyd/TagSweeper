using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfo : MonoBehaviour
{
    public GameObject[] rows = new GameObject[2];
    private int k = 0;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(rows[k].name + " is at index " + k);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
