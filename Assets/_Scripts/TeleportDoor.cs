using UnityEngine;

public class MovePlayerOnTrigger : MonoBehaviour
{
    // Reference to the GameObject that the player will move to.
    public GameObject targetObject;

    // When the player enters the trigger area.
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger zone.
        if (other.CompareTag("Player"))
        {
            // Move player to the target GameObject's position.
            other.transform.position = targetObject.transform.position;
        }
    }
}


