using UnityEngine;
using UnityEngine.UI; // Required for UI Text/Image
using TMPro;

public class PlayerStat : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100; // Maximum health points
    [SerializeField] private TextMeshProUGUI healthText; // UI Text to display health

    private int currentHealth; // Current health of the player

    void Start()
    {
        // Initialize health to max health when the game starts
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }

    // Method to take damage
    public void TakeDamage(int damageAmount)
    {
        // Reduce health
        currentHealth -= damageAmount;

        // Update health display
        UpdateHealthDisplay();

        // Check if player is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to update health display
    private void UpdateHealthDisplay()
    {
        // Ensure health text is not null before updating
        if (healthText != null)
        {
            healthText.text = $"HP: {Mathf.Max(0, currentHealth)}/{maxHealth}";
        }
    }

    // Method to handle player death
    private void Die()
    {
        // Optional visual/audio effects can be added here
        Debug.Log("Player has died!");

        // Remove the player from the game
        Destroy(gameObject);
    }

    // Optional: Method to heal the player
    public void Heal(int healAmount)
    {
        // Increase health, but don't exceed max health
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        UpdateHealthDisplay();
    }

    // Optional: Getter for current health
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
