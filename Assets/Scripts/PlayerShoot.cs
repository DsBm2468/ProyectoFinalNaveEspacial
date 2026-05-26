using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Transform firePoint;

    PlayerMovement player;

    void Start()
    {
        player =
        GetComponent<
        PlayerMovement>();
    }

    void Update()
    {
        if (
        Input.GetKeyDown(
        KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = 
        Instantiate(
        bulletPrefab,
        firePoint.position, // Spawnea la bala en la posición del firePoint
        firePoint.rotation); // Rota la bala para que apunte en la dirección del firePoint

        Bullet bulletScript = 
        bullet.GetComponent<
        Bullet>();

        if (
        bulletScript != null)
        {
            bulletScript.inheritedSpeed =
            player.GetCurrentSpeed(); // Pasa la velocidad actual del jugador a la bala para que esta herede esa velocidad
        }
    }
}