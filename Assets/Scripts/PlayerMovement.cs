using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 20f;

    public float maxBoostSpeed = 45f;

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
        if (arduino == null)
            return;

        arduino.GetNormalizedValues(
        out float speedInput,
        out float steeringInput);

        BoostControl(speedInput);

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

    void BoostControl(float speedInput)
    {
        bool boosting =
        Mathf.Abs(speedInput)
        >= boostThreshold;

        float targetSpeed =
        boosting
        ? maxBoostSpeed
        : baseSpeed;

        float changeRate =
        boosting
        ? acceleration
        : deceleration;

        currentSpeed =
        Mathf.MoveTowards(
        currentSpeed,
        targetSpeed,
        changeRate *
        Time.deltaTime);
    }
}