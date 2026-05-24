using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 20f;

    public float maxBoostSpeed = 50f;

    public float acceleration = 20f;

    public float deceleration = 15f;

    public float rotationSpeed = 100f;

    public float boostThreshold = 0.8f;

    private float currentSpeed;

    private ArduinoSerialReader arduino;

    void Start()
    {
        currentSpeed = baseSpeed;

        arduino =
        ArduinoSerialReader.Instance;
    }

    void Update()
    {
        float speedInput;

        float steeringInput;

        bool boostInput;

        if (
        arduino != null
        &&
        arduino.IsConnected)
        {
            arduino.GetNormalizedValues(
            out speedInput,
            out steeringInput);

            boostInput =
            arduino.BoostButton;
        }
        else
        {
            speedInput =
            Input.GetAxis(
            "Vertical");

            steeringInput =
            Input.GetAxis(
            "Horizontal");

            boostInput =
            Input.GetKey(
            KeyCode.LeftShift);
        }

        BoostControl(
        boostInput);

        transform.position +=
        transform.forward *
        currentSpeed *
        Time.deltaTime;

        float yaw =
        steeringInput;

        float pitch =
        speedInput;

        transform.Rotate(
        Vector3.up *
        yaw *
        rotationSpeed *
        Time.deltaTime);

        transform.Rotate(
        Vector3.right *
        -pitch *
        rotationSpeed *
        Time.deltaTime);
    }

    void BoostControl(
    bool boosting)
    {
        float targetSpeed =
        boosting
        ?
        maxBoostSpeed
        :
        baseSpeed;

        float changeRate =
        boosting
        ?
        acceleration
        :
        deceleration;

        currentSpeed =
        Mathf.MoveTowards(
        currentSpeed,
        targetSpeed,
        changeRate *
        Time.deltaTime);
    }
    public float GetCurrentSpeed() // Método público para obtener la velocidad actual del jugador
    {
        return currentSpeed; 
    }
}