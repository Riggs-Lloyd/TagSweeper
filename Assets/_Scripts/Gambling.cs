using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gambling : MonoBehaviour
{
    public TextMeshPro resultText;  // Assign your TextMesh in the inspector
    public float interactionRange = 3f; // Range within which player can interact
    public Transform player; // Assign the player object in the inspector
    private int rolls = 1;

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactionRange && Input.GetKeyDown(KeyCode.X))
        {
            int roll = Random.Range(1, 1000); // Rolls between 1 and 999
            if (roll == 999)
            {
                resultText.text = "Win";
            }
            else
            {
                resultText.text = "Loss " + roll;
                Debug.Log("Rolled");
            }
        }
    }
}
