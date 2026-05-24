using UnityEngine;

public class PlayerBoundary : MonoBehaviour
{
    public float turnSpeed = 3f;

    private bool returning;

    private Vector3 returnDirection;

    void Update()
    {
        if (returning)
        {
            Quaternion targetRotation =
            Quaternion.LookRotation(
            returnDirection);

            transform.rotation =
            Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            turnSpeed *
            Time.deltaTime);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boundary"))
        {
            returnDirection =
            -transform.position.normalized;

            returning = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary"))
        {
            returning = false;
        }
    }
}
