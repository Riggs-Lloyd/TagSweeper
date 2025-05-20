using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float enemyHealth = 100f;
    public float chaseSpeed = 3.5f;    // Speed at which the enemy chases the player
    public float detectionRange = 10f; // Range within which the enemy will detect and chase the player
    public float stopChaseDistance = 12f; // Distance at which the enemy stops chasing
    public float damageAmount = 10f;  // How much damage the enemy will deal to the player
    public float damageCooldown = 1f; // Time in seconds between consecutive damage to prevent spamming

    private Transform player;          // Reference to the player's transform
    private bool isChasing = false;    // Is the enemy chasing the player?
    private bool canDamagePlayer = true; // To handle the damage cooldown

    [Header("Health Settings")]
    public float maxHealth = 100f;  // Maximum health of the enemy
    private float currentHealth;

    private CharacterController characterController;  // Reference to the CharacterController component

    private void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;  // Assuming the player is tagged as "Player"
        characterController = GetComponent<CharacterController>(); // Get the CharacterController attached to the enemy
    }

    private void Update()
    {
        // Check the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the player is within detection range and start chasing
        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }
        else if (distanceToPlayer > stopChaseDistance)
        {
            // Stop chasing if the player is out of stopChaseDistance
            isChasing = false;
        }

        // Chase the player if the enemy is in chasing mode
        if (isChasing)
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        // Calculate direction towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        characterController.Move(direction * chaseSpeed * Time.deltaTime);
    }

    // Use OnControllerColliderHit instead of OnCollisionEnter
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the hit object is the player
        if (hit.gameObject.CompareTag("Player") && canDamagePlayer)
        {
            Debug.Log("Enemy is attempting to damage the player!");
            PlayerHealth playerHealth = hit.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);  // Apply damage
                StartCoroutine(DamageCooldown());  // Apply cooldown to prevent spamming damage
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;  // Reduce health by the damage amount
        if (currentHealth <= 0f)
        {
            Die();  // Call Die method when health reaches zero
        }
    }

    // Method to handle enemy death
    private void Die()
    {
        // You can add death animations or particle effects here.
        Debug.Log(gameObject.name + " died!");

        // Destroy the enemy object (or disable it, depending on your needs)
        Destroy(gameObject);
    }

    // Optional: A method to get the current health, in case you want to display it
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    private IEnumerator DamageCooldown()
    {
        // Prevent spamming damage
        canDamagePlayer = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamagePlayer = true;
    }
}
