using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnButton : MonoBehaviour
{
    public PlayerHealth BobThePlayer;
    public PlayerController controller;

    public void OnRespawnButtonPressed()
    {
        // Reset health and re-enable player
        BobThePlayer.ResetHealth();

        if (controller != null)
        {
            controller.Respawn();
        }

        Debug.Log("Player respawned.");
    }
}

