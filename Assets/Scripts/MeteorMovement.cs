using UnityEngine;

public class MeteorMovement : MonoBehaviour
{
    public float moveSpeed = 15f;

    public float rotateSpeed = 60f;

    Vector3 moveDirection;

    Vector3 rotateDirection;

    void Start()
    {
        moveDirection =
        Random.onUnitSphere;

        rotateDirection =
        Random.insideUnitSphere;
    }

    void Update()
    {
        transform.position +=
        moveDirection *
        moveSpeed *
        Time.deltaTime;

        transform.Rotate(
        rotateDirection *
        rotateSpeed *
        Time.deltaTime);

        if (
        transform.position.magnitude
        > 300)
        {
            moveDirection =
            -transform.position.normalized;
        }
    }
}