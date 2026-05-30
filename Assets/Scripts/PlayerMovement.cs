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

    public bool invertHorizontal;

    public bool invertVertical;

    public float rotationAcceleration = 200f;

    public float rotationDamping = 2f;

    float currentYawSpeed;

    float currentPitchSpeed;

    private ArduinoSerialReader arduino;

    AudioSource audioSource;

    public AudioClip boostSound;

    bool playingBoost;

    void Start()
    {
        currentSpeed = baseSpeed;

        arduino =
        ArduinoSerialReader.Instance;

        audioSource = GetComponent<AudioSource>();
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

        if (invertHorizontal)
        {
            yaw *= -1f;
        }

        if (invertVertical)
        {
            pitch *= -1f;
        }

        // Acelera la rotación
        currentYawSpeed +=
        yaw *
        rotationAcceleration *
        Time.deltaTime;

        currentPitchSpeed +=
        pitch *
        rotationAcceleration *
        Time.deltaTime;

        // Aplica fricción espacial suave
        currentYawSpeed =
        Mathf.Lerp(
        currentYawSpeed,
        0,
        rotationDamping *
        Time.deltaTime);

        currentPitchSpeed =
        Mathf.Lerp(
        currentPitchSpeed,
        0,
        rotationDamping *
        Time.deltaTime);

        // Rota usando la velocidad acumulada
        transform.Rotate(
        Vector3.up *
        currentYawSpeed *
        Time.deltaTime);

        transform.Rotate(
        Vector3.right *
        -currentPitchSpeed *
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

        if (
            boosting
            &&
            !playingBoost)
        {
            audioSource.clip =
            boostSound;

            audioSource.loop =
            true;

            audioSource.Play();

            playingBoost =
            true;
        }

        if (
        !boosting
        &&
        playingBoost)
        {
            audioSource.Stop();

            playingBoost =
            false;
        }
    }
    public float GetCurrentSpeed() // Método público para obtener la velocidad actual del jugador
    {
        return currentSpeed; 
    }
}