using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Transform firePoint; // El punto desde donde se disparará la bala

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
    }

    void Update()
    {
        bool shootInput = 
        Input.GetKeyDown( // Detecta si se presiona la barra espaciadora para disparar
        KeyCode.Space);

        if (
        arduino != null // Verifica si el Arduino está conectado
        &&
        arduino.IsConnected)
        {
            bool currentFire = // Lee el estado del botón de disparo desde el Arduino
            arduino.FireButton; // Asegúrate de que "FireButton" sea el nombre correcto de la propiedad en tu clase ArduinoSerialReader

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
    }
}