using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
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

        if (currentHealth <= 0)
        {
            Debug.Log(
            "Jugador destruido");
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
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public void ResetHealth()
    {
        currentHealth =
        maxHealth;
    }
}
