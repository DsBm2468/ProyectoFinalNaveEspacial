using UnityEngine;

public class ShipInteriorMovement : MonoBehaviour
{
    public float moveSpeed = 2f;

    public float rotationSpeed = 80f;

    public bool invertHorizontal;

    public bool invertVertical;

    Rigidbody rb;

    ArduinoSerialReader arduino;

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
