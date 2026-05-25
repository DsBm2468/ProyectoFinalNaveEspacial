using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Transform firePoint;

    public AudioClip shootSound;

    AudioSource audioSource;

    PlayerMovement player;

    ArduinoSerialReader arduino;

    bool previousFire;

    void Start()
    {
        player =
        GetComponent<
        PlayerMovement>();

        arduino =
        ArduinoSerialReader
        .Instance;

        audioSource =
        GetComponent<
        AudioSource>();
    }

    void Update()
    {
        bool shootInput =
        Input.GetKeyDown(
        KeyCode.Space);

        if (
        arduino != null
        &&
        arduino.IsConnected)
        {
            bool currentFire =
            arduino.FireButton;

            if (
            currentFire
            &&
            !previousFire)
            {
                shootInput =
                true;
            }

            previousFire =
            currentFire;
        }

        if (shootInput)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet =
        Instantiate(
        bulletPrefab,
        firePoint.position,
        firePoint.rotation);

        Bullet bulletScript =
        bullet.GetComponent<
        Bullet>();

        if (
        bulletScript != null)
        {
            bulletScript.inheritedSpeed =
            player.GetCurrentSpeed();
        }

        if (
        shootSound
        != null)
        {
            audioSource.PlayOneShot(
            shootSound);
        }
    }
}