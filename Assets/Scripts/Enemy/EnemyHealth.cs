using UnityEngine;
using System.Collections; // Required for IEnumerator and coroutines

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f; // Kesehatan musuh
    private Animator anim; // Reference to the Animator component
    private Renderer enemyRenderer; // Reference to the Renderer component for changing color
    private Color originalColor; // Store the original color of the enemy
    public Color hitColor = Color.red; // The color to turn to when hit
    public float hitEffectDuration = 0.2f; // Duration of the red effect

    private void Awake()
    {
        anim = GetComponent<Animator>(); // Get the Animator component attached to the enemy
        enemyRenderer = GetComponent<Renderer>(); // Get the Renderer component to change color
        originalColor = enemyRenderer.material.color; // Store the original color of the enemy
    }

    // Method untuk menerima damage
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} menerima {damage} damage, sisa kesehatan: {health}");

        // Make the enemy turn red when hit
        if (enemyRenderer != null)
        {
            StopAllCoroutines(); // Stop any existing color change coroutines
            StartCoroutine(ChangeColorOnHit()); // Start the red color effect
        }

        // Trigger the damage animation
        if (anim != null)
        {
            anim.SetTrigger("hurt"); // Assuming "damage" is the trigger for the damage animation
        }

        if (health <= 0f)
        {
            Die();
        }
    }

    // Method to handle the death of the enemy
    private void Die()
    {
        Debug.Log($"{gameObject.name} mati!");

        // Trigger the death animation
        if (anim != null)
        {
            anim.SetTrigger("die"); // Assuming "die" is the trigger for the death animation
        }

        // Optionally, you can destroy the game object after a delay to allow the death animation to play
        Destroy(gameObject, 2f); // Destroy after 2 seconds to allow the death animation to play
    }

    // Coroutine to change the enemy's color to red when hit
    private IEnumerator ChangeColorOnHit()
    {
        // Change the color to red
        enemyRenderer.material.color = hitColor;

        // Wait for the duration of the hit effect
        yield return new WaitForSeconds(hitEffectDuration);

        // Restore the original color
        enemyRenderer.material.color = originalColor;
    }

    // Detect collision with an object that can deal damage (e.g., player, bullet, etc.)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the enemy is hit by a damage-dealing object (e.g., player or projectile)
        if (collision.gameObject.CompareTag("Player")) // Change "Player" to the tag of your damage source
        {
            float damage = 20f; // Set the damage value (adjust based on your game's needs)
            TakeDamage(damage); // Apply damage to the enemy
        }
    }

    // Optionally, you can use OnTriggerEnter2D if using triggers instead of colliders
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) // Change "Player" to the tag of your damage source
        {
            float damage = 20f; // Set the damage value (adjust based on your game's needs)
            TakeDamage(damage); // Apply damage to the enemy
        }
    }
}
