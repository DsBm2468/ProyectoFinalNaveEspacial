using UnityEngine;

public class ShipInteriorMovement : MonoBehaviour
{
    public float moveSpeed = 2f; // Velocidad de movimiento del usuario
    public float rotationSpeed = 80f; //  Velocidad de rotación de la cámara
    //public float mouseSensibility = 2f; // Sensibilidad del mouse al mover la cabeza
    
    public bool invertHorizontal;
    public bool invertVertical;

    //private CharacterController controller; // Se encarga de mover personajes (en este caso en primera persona) sin depender de fisicas complejas de masa y gravedad de un rigidbody
    //private float rotationX = 0f; // Guarda cuánto has mirado arriba o abajo para ponerle un límite.
    //private float verticalVelocity = 0f;
    Rigidbody rb;

    ArduinoSerialReader arduino; // Lector de arduino

    void Start()
    {
        arduino =
        ArduinoSerialReader.Instance;

    }

    void Update()
    {
        float horizontal;

        float vertical;

        if (
        arduino != null
        &&
        arduino.IsConnected)
        {
            arduino.GetNormalizedValues(
            out horizontal,
            out vertical);
        }
        else
        {
            horizontal =
            Input.GetAxis(
            "Horizontal");

            vertical =
            Input.GetAxis(
            "Vertical");
        }

        if (invertHorizontal)
        {
            horizontal *= -1f;
        }

        if (invertVertical)
        {
            vertical *= -1f;
        }

        transform.Rotate(
        Vector3.up *
        horizontal *
        rotationSpeed *
        Time.deltaTime);

        transform.position +=
        transform.forward *
        vertical *
        moveSpeed *
        Time.deltaTime;
    }
}
