using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Transform firePoint;

    private ArduinoSerialReader arduino;

    private bool previousFireState;

    void Start()
    {
        arduino =
        ArduinoSerialReader.Instance; 
    }

    void Update()
    {
        bool shootInput =
        Input.GetKeyDown(
        KeyCode.Space);

        if (
        arduino != null // chequear si la instancia de ArduinoSerialReader está disponible
        &&
        arduino.IsConnected) // chequear si el arduino está conectado antes de leer su estado
        {
            bool currentFire = // leer el estado del botón de disparo desde el Arduino
            arduino.FireButton;

            if (
            currentFire
            &&
            !previousFireState)
            {
                shootInput = true;
            }

            previousFireState =
            currentFire;
        }

        if (shootInput)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(
        bulletPrefab,
        firePoint.position,
        firePoint.rotation);
    }
}