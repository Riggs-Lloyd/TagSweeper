using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLayout : MonoBehaviour
{
    private int width;
    private int height;
    private int[,] address;


    void Start()
    {
     //   transform.GetChild(3).gameObject
        {
       //     Debug.Log(0:0);
        }
    }
    public BlockLayout(int width, int height)
    {
        this.width = width;
        this.height = height;
        
        address = new int[this.width, this.height];

        for (int x = 0; x < address.GetLength(0); x++)
        {
            for (int y = 0; y < address.GetLength(1); y++)
            {
                
            }
        }
    }
    
  
    //ok ok sooo lets think this through ermmmmmmmmmmmmmmmmm ok what if we have a component that attaches to a block
    //that when the spray paint is applied to that block it calculates.
    
    //SOOO block system right  then it has to randomly assign bombs based on the amount given and it then edits those addresses.
}
