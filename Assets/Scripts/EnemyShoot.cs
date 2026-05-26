using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Transform firePoint;

    public float fireRate = 1.5f;

    public float shootDistance = 30f;

    private Transform player;

    private float nextFire;

    void Start()
    {
        player =
        GameObject.FindGameObjectWithTag("Player")
        .transform;
    }

    void Update()
    {
        if (player == null)
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (distance <= shootDistance)
        {
            if (Time.time > nextFire)
            {
                nextFire =
                    Time.time +
                    fireRate;

                Shoot();
            }
        }
    }

    void Shoot()
    {
        Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );
    }
}