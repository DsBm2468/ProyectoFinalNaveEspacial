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

//using UnityEngine;

//public class Gravity : MonoBehaviour
//{
//    bool ForceGravity = true; // Inicialmente la gravedad estará activada debido a que se encuentra dentro de la nave.
    
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.G)) // Si presionas l tecla g (este comando es temporal)
//        {
//            ForceGravity = !ForceGravity; // La gravedad se desactiva y viceversa

//            Rigidbody[] AllObjectsInSpaceship = Object.FindObjectsByType<Rigidbody>((FindObjectsSortMode.None)); // Indica que todos los objetos que tengan rigidbody los tenga en cuenta (pero no necesariamente en orden)

//            foreach (Rigidbody rb in AllObjectsInSpaceship)
//            {
//                rb.useGravity = ForceGravity; // Para que el rigidbody tenga en cuenta el factor de useGravity con la fuerza de gravedad de la tecla g

//                if (!ForceGravity) // Si la fuerza de gravedad es nula...
//                {
//                    rb.linearVelocity = Vector3.zero; // La velocidad de movimiento del objeto se detendrá, manteniendolo estatico
//                    // Indica la velocidad de movimiento, 
//                    // linearVelocity es la propiedad que controla la velocidad de movimiento en el espacio de un objeto con físicas o Rigidbody. Define la cantidad de unidades del mundo que el objeto se desplaza por segundo en los ejes X, Y y Z.
//                    rb.angularVelocity = Vector3.zero; // La velocidad de giro del objeto se detendrá, manteniendolo estatico
//                    // angularVelocity es la propiedad que controla o devuelve la velocidad de rotación de un objeto físico. Representa la rapidez con la que el objeto gira sobre sus propios ejes.

//                    rb.linearDamping = 0f; // Quitamos la resistencia del aire para que el objeto flote infinitamente al ser tocado
//                    rb.angularDamping = 0f; // Quitamos la resistencia de giro para que no deje de dar vueltas en el vacío

//                    //Vector3 randomDirection = Random.onUnitSphere; // Da una dirección al azar en un vector 3D
//                    //rb.AddForce(randomDirection * 0.5f, ForceMode.Impulse); // Se le otorga un sutil impulso hacia la dirección aleatoria, simulando la gravedad 0 (para ello usa una especie de salto inicial para no aplicar la fuerza de golpe)
//                } 
//                else // Si vuelve la gravedad...
//                {
//                    rb.linearDamping = 0.05f; // Se restaura la resistencia y presión del aire.
//                    // linearDamping es una propiedad del componente Rigidbody o Rigidbody2D que simula la resistencia o fricción del aire 
//                    rb.angularDamping = 0.05f; // Evita que los objetos al tocar el piso resbalen
//                    // angularDamping simula la resistencia del aire o fricción rotacional. Se encarga de desacelerar progresivamente la rotación de un objeto cuando está girando
//                }
//            }
//            Debug.Log("Gravedad en la nave: " + (ForceGravity ? "<Color=green>ACTIVADA</Color>" : "<Color=yellow>DESACTIVADA</Color>"));
//        }
//    }
//}
