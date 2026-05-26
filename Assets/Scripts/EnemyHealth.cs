using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;

    public GameObject explosionPrefab;

    int currentHealth;

    void Start()
    {
        currentHealth =
        maxHealth;
    }

    public void TakeDamage(
    int damage)
    {
        currentHealth -=
        damage;

        if (currentHealth <= 0)
        {
            Explode();

            if (
                GameManager.Instance
                != null)
            {
                GameManager.Instance
                .EnemyKilled();
            }

            Destroy(
            gameObject);
        }
    }

    void Explode()
    {
        if (
        explosionPrefab != null)
        {
            GameObject explosion =
            Instantiate(
            explosionPrefab,
            transform.position,
            Quaternion.identity);

            Destroy(
            explosion,
            2f);
        }
    }
}