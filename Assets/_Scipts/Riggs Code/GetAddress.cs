using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class GetAddress : MonoBehaviour
{
    static Transform Row1;
    Transform buildingnumbers;

    private void Awake()
    {
        throw new NotImplementedException();
    }

    static Transform[] children = Row1.gameObject.GetComponentsInChildren<Transform>();
    private int randomIndex = Random.Range(0, children.Length);
    Transform randomChild;

    public GetAddress()
    {
        randomChild = children[randomIndex];
    }

    void Start()
    {
        
        Row1 = transform.Find("Row1");
        buildingnumbers = Row1.transform.Find(randomChild.name);
        if (buildingnumbers != null)
        {
            Debug.Log(buildingnumbers.name);
        }
        else
        {
            Debug.Log("No Building Numbers Found");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
