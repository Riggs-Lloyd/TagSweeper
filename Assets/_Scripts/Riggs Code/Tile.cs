using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
	public int isBomb;
	public bool isBombTrue;
	
	public GameObject[] tiles = new GameObject[3];
	public int counter;

	// Start is called before the first frame update

	void Start()
	{

		isBomb = Random.Range(1, 11);

		if (isBomb <= 2)
		{
			isBombTrue = true;
		}
		else
		{
			isBombTrue = false;
		}
		

		for (int i = 0; i < tiles.Length; i++)
		{
			if (this.gameObject.Tile.isBombTrue == true)
				
				//Is checking its own isBombTrue not its neighbors
				
			{
				counter += 1;S
			}
			else
			{
				Debug.Log("Woof");
			}
		}


		// Update is called once per frame
	void Update()
	{
		if (gameObject.CompareTag("Tagged") == true && isBombTrue == true)
		{
			
			transform.Find("BOOM " +  gameObject.name).gameObject.SetActive(true);
			
		}
		else
		{
			return;
		}

		void CheckAdjacent()
		{
			//check each directions (N,NE,E...) and see if that tiles isBombTrue is true or not if yes the add to number val
        
			
			}

		} 
		
	}

	
}
