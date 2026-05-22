using UnityEngine;

public class Gravity : MonoBehaviour
{
    bool ForceGravity = true; // Inicialmente la gravedad estará activada debido a que se encuentra dentro de la nave.
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // Si presionas l tecla g (este comando es temporal)
        {
            ForceGravity = !ForceGravity; // La gravedad se desactiva y viceversa

            Rigidbody[] AllObjectsInSpaceship = Object.FindObjectsByType<Rigidbody>((FindObjectsSortMode.None)); // Indica que todos los objetos que tengan rigidbody los tenga en cuenta (pero no necesariamente en orden)

            foreach (Rigidbody rb in AllObjectsInSpaceship)
            {
                rb.useGravity = ForceGravity; // Para que el rigidbody tenga en cuenta el factor de useGravity con la fuerza de gravedad de la tecla g

                if (!ForceGravity) // Si la fuerza de gravedad es nula...
                {
                    Vector3 randomDirection = Random.onUnitSphere; // Da una dirección al azar en un vector 3D
                    rb.AddForce(randomDirection * 0.5f, ForceMode.Impulse); // Se le otorga un sutil impulso hacia la dirección aleatoria, simulando la gravedad 0 (para ello usa una especie de salto inicial para no aplicar la fuerza de golpe)
                }
            }
            Debug.Log("Gravedad en la nave: " + (ForceGravity ? "<Color=green>ACTIVADA</Color>" : "<Color=yellow>DESACTIVADA</Color>"));
        }
    }
}
