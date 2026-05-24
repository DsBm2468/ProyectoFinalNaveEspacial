using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;

    int currentHealth;

    HealthUI ui;

    void Start()
    {
        currentHealth =
        maxHealth;

        ui =
        FindFirstObjectByType<
        HealthUI>();

        UpdateUI();
    }

    public void TakeDamage(
    int damage)
    {
        currentHealth -=
        damage;

        currentHealth =
        Mathf.Clamp(
        currentHealth,
        0,
        maxHealth);

        UpdateUI();

        if (currentHealth <= 0)
        {
            GameOverManager
            .Instance
            .Show();
        }
    }

    public void Heal(
    int amount)
    {
        currentHealth +=
        amount;

        currentHealth =
        Mathf.Clamp(
        currentHealth,
        0,
        maxHealth);

        UpdateUI();
    }

    public void ResetHealth()
    {
        currentHealth =
        maxHealth;

        UpdateUI();
    }

    void UpdateUI()
    {
        if (ui != null)
        {
            ui.UpdateHealth(
            currentHealth,
            maxHealth);
        }
    }
}