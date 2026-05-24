using UnityEngine;

public class MeteorDamage : MonoBehaviour
{
    public int damage = 10;

    void OnTriggerEnter(
    Collider other)
    {
        PlayerHealth player =
        other.GetComponent<
        PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(
            damage);
        }
    }
}