using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
	public int isBomb;
	public bool isBombTrue;
	public GameObject[] tiles = new GameObject[8];
	public int counter;

	// Start is called before the first frame update

	private void Awake()
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

	void Start()
	{


		for (int i = 0; i < tiles.Length; i++)
		{
			if (tiles[i].GetComponent<Tile>().isBombTrue)
				//Is checking its own isBombTrue not its neighbors
			{
				counter += 1;
			}
			else
			{
				counter += 0;
			}
		}

	}

	

	// Update is called once per frame
	void Update()
	{
		if (gameObject.CompareTag("Tagged") == true && isBombTrue == true)
		{
			
			transform.Find("BOOM 0:0").gameObject.SetActive(true);
			
		}
		else
		{
			return;
		}
	}
	}
