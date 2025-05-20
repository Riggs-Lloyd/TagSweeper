using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public Canvas deathScreen; // Canvas type for the death screen

    private bool isDead = false;

    // For proper unsubscription
    private System.Action<InputAction.CallbackContext> _damageTestHandler;

    private void Start()
    {
        currentHealth = maxHealth;

        if (deathScreen != null)
            deathScreen.gameObject.SetActive(false);

        // Subscribe to damage test action
        _damageTestHandler = ctx => TakeDamage(20f);
        InputManager.instance.DamageTestingAction.performed += _damageTestHandler;
    }

    private void OnDestroy()
    {
        if (_damageTestHandler != null)
            InputManager.instance.DamageTestingAction.performed -= _damageTestHandler;
    }

    // Method to handle taking damage
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth);
        Debug.Log("Player took damage. Health now: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // Method to handle death
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player Died.");

        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
            playerController.DisableMovement();

        if (deathScreen != null)
            deathScreen.gameObject.SetActive(true);
    }

    // Reset the player's health
    public void ResetHealth()
    {
        isDead = false;
        currentHealth = maxHealth;

        if (deathScreen != null)
            deathScreen.gameObject.SetActive(false);
    }

    // Method to get the current health
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}