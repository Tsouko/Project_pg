using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("Iframes")]
    [SerializeField] private float iframesDuration;
    [SerializeField] private int numberOfflashes;
    private SpriteRenderer spriteRend;

    // Optionally, a reference to the GameManager or UI manager to show game over
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverUI; // Assign a Game Over UI object in the inspector

    private bool canAttack = true; // Flag for controlling whether the player can attack

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        if (dead) return; // Prevent taking damage after death

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invulnerability());
        }
        else
        {
            if (!dead)
            {
                Die();
            }
        }
    }

    public void AddHealth(float _value)
    {
        if (dead) return; // Prevent adding health after death

        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        canAttack = false; // Prevent attacking while in invulnerability
        for (int i = 0; i < numberOfflashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f); // Flash red
            yield return new WaitForSeconds(iframesDuration / (numberOfflashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iframesDuration / (numberOfflashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        canAttack = true; // Allow attacking again after invulnerability ends
    }

    private void Die()
    {
        dead = true;
        anim.SetTrigger("die"); // Play death animation
        GetComponent<PlayerMovement>().enabled = false; // Disable player movement

        // Show game over UI (if assigned)
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // Additional death logic can be added here (e.g., disabling player input, playing sound, etc.)
    }

    // Method to check if the player can attack
    public bool CanAttack()
    {
        return !dead && canAttack; // Player can attack if not dead and invulnerability is over
    }
}
