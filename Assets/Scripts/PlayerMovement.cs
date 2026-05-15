using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 20f;

    public float rotationSpeed = 100f;

    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime; // mueve hacia adelante

        float yaw = Input.GetAxis("Horizontal"); //Rotacion horizontal

        float pitch = Input.GetAxis("Vertical"); //Rotacion vertical

        transform.Rotate(
            Vector3.up * yaw * rotationSpeed * Time.deltaTime // Girar izquierda/derecha
        );

        transform.Rotate(
            Vector3.right * -pitch * rotationSpeed * Time.deltaTime // Girar arriba/abajo
        );
    }
}