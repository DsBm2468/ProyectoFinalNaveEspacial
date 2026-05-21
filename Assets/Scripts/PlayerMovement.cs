using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 20f;

    public float maxBoostSpeed = 45f;

    public float acceleration = 20f;

    public float deceleration = 15f;

    public float rotationSpeed = 100f;

    private float currentSpeed;

    void Start()
    {
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        BoostControl();

        transform.position +=
        transform.forward *
        currentSpeed *
        Time.deltaTime;

        float yaw =
        Input.GetAxis("Horizontal");

        float pitch =
        Input.GetAxis("Vertical");

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

    void BoostControl()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed =
            Mathf.MoveTowards(
            currentSpeed,
            maxBoostSpeed,
            acceleration *
            Time.deltaTime);
        }
        else
        {
            currentSpeed =
            Mathf.MoveTowards(
            currentSpeed,
            baseSpeed,
            deceleration *
            Time.deltaTime);
        }
    }
}