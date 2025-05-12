using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
	public int isBomb;
	public bool isBombTrue;

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
	}
}
