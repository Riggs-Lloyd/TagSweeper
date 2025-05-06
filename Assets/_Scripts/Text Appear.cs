using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAppear : MonoBehaviour
{
	public float interactionRange = 3f;
	public Transform player;

	public GameObject textbox; // Changed from 'object' to 'GameObject'

	void Start()
	{
		textbox.SetActive(false); // to hide
	}

	void Update()
	{
		float distance = Vector3.Distance(player.position, transform.position);
		if (distance <= interactionRange && Input.GetKeyDown(KeyCode.X))
		{
			textbox.SetActive(true); // to show
			

		}
	}
}