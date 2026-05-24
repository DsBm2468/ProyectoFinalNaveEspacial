using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 20;

    void OnTriggerEnter(
    Collider other)
    {
        PlayerHealth player =
        other.GetComponent<
        PlayerHealth>();

        if (player != null)
        {
            player.Heal(
            healAmount);

            Destroy(
            gameObject);
        }
    }
}