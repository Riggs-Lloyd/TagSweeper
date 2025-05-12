using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public Canvas deathScreen; // still using Canvas type

    private void Start()
    {
        currentHealth = maxHealth;

        if (deathScreen != null)
            deathScreen.gameObject.SetActive(false);

        // Subscribe to damage input
        InputManager.instance.DamageTestingAction.performed += ctx => TakeDamage(20f);
    }

    private void OnDestroy()
    {
        // Clean up
        InputManager.instance.DamageTestingAction.performed -= ctx => TakeDamage(20f);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took damage. Health now: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died.");

        // Disable movement
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
            playerController.DisableMovement();

        // Show death screen
        if (deathScreen != null)
            deathScreen.gameObject.SetActive(true);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        if (deathScreen != null)
            deathScreen.gameObject.SetActive(false);
    }
}