using Unity.VisualScripting;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public bool ForceGravity = false; // Inicialmente la gravedad estará desactivada, indicando que se encuentra en gravedad cero
    public float groundY = 0f;     // Referencia del suelo de la nave

    private bool previousArduinoGravityButton = false; // Inicialmente en el sistema de arduino, el botón de antigravedad estará desactivado

    [Header("Configuración del Planeta")]
    public float gravityValue = 9.81f; // Guardará la fuerza del planeta seleccionado. Empieza en 9.81 (Tierra)

    private GameObject[] floatingObjects; // Lista (array) en la que se ubicarán todos los objetos de la nave que van a cambiar de posición por factores alternos
    public float[] objectsVelocity; // Guarda la velocidad acumulada de cada objeto que varia según factores externos (choque entre objetos o con el usuario)

    private void Start()
    {
        GameObject floor = GameObject.FindWithTag("FloorSpaceship");
        if (floor != null)
        {
            groundY = floor.transform.position.y;
        }

        floatingObjects = GameObject.FindGameObjectsWithTag("FloatingObjects"); // Inicialmente se indica que todos los objetos con el tag FloatingObjects quedarán registrados en la lista floatingObjects
        objectsVelocity = new float [floatingObjects.Length]; // Actualiza la cantidad de objetos registrados para así guardar una velocidad para cada uno de ellos
    }

    void Update()
    {
        bool ChangeGravityInput = Input.GetKeyDown(KeyCode.G); // Si presionas la tecla g en el teclado

        // También permite activar/desactivar gravedad con Arduino
        if (ArduinoSerialReader.Instance != null && ArduinoSerialReader.Instance.IsConnected) // Si el arduino está conectado...
        {
            bool currentArduinoButton = ArduinoSerialReader.Instance.BoostButton;

            // Detecta una sola pulsación
            if (currentArduinoButton && !previousArduinoGravityButton)
            {
                ChangeGravityInput = true;
            }
            previousArduinoGravityButton = currentArduinoButton;
        }

        // Si alguna de las opciones activa el booleano...
        if (ChangeGravityInput)
        {
            ForceGravity = !ForceGravity; // La gravedad se desactiva y viceversa
            Debug.Log("Gravedad en la nave: " + (ForceGravity ? "<Color=green>ACTIVADA</Color>" : "<Color=yellow>DESACTIVADA</Color>"));
        }

        if (ForceGravity == true) // Si la fuerza de gravedad está activada (sea cual sea el planeta)...
        {
            //foreach (GameObject obj in AllObjectsInSpaceship)  // Por cada objeto en la nave...
            for (int i = 0; i < floatingObjects.Length; i++) // Por cada objeto en la nave... (se registra de esta forma para tener el valor exacto en donde queda guardada la información, así el sistema busca directamente los datos segun este valor entero)
            {
                float middleOfObject = floatingObjects[i].GetComponent<Collider>().bounds.extents.y; // para que el objeto detecte correctamente el suelo, buscará su collider y con el .bounds se sabrá la medida exacta del objeto, además con .extents se obtiene la distancia desde el centro hasta sus bordes exteriores,
                // Específicamente, da la distancia desde el centro del objeto hasta el borde de abajo (base (.y))

                if ((floatingObjects[i].transform.position.y - middleOfObject) > groundY) // El objeto SOLO cae si su altura en Y es más alta que el suelo
                    // (para que el objeto realmente se vea en el suelo, la posición en y se le resta la mitad del objeto (es como si se pasara el centro a la base del objeto))
                {
                    // VELOCIDAD ACTUAL DEL OBJETO
                    // Para ello se aplicaría la segunda ley de Newton (La aceleración de un objeto es directamente proporcional a la fuerza neta que actua sobre el e inversamente proporcional a su masa)
                    // A su vez, teniendo en cuenta la lógica de la caida libre (caso específico del Movimiento rectilineo uniformemente acelerado, objeto que acelera en linea recta ),
                    // ya que esta permite actualizar la velocidad y la posición de un objeto en ciertos intervalos de tiempo
                    // Entonces, se usaría la formula:   Vfinal = Vinicial + (Aceleración * Tiempo)
                    // En este caso la aceleración sería la gravedad del planeta en ese momento (Time.deltaTime)
                    objectsVelocity[i] += gravityValue * Time.deltaTime; // se usa +=  aumenta constantemente, permite simular fisica realistas (este += es otra forma de decir que tome el valor que ya se tiene de esta variable (velocidad inicial) y le sume lo siguiente)

                    // POSICION ACTUAL DEL OBJETO

                    // Además, para saber a que posición se va a mover el objeto con la velocidad encontrada,
                    // Se usaría la fórmula: Pfinal = Pinicial + (V * Tiempo)
                    // Esta es la versión corta de la ecuacion cuadrática larga
                    Vector3 currentPosition = floatingObjects[i].transform.position; // Se guarda la posición actual de los objetos
                    currentPosition.y -= objectsVelocity[i] * Time.deltaTime; //este += es otra forma de decir que tome el valor que ya se tiene de esta variable(posición inicial) y le reste lo siguiente
                    floatingObjects[i].transform.position = currentPosition;
                }
                else // Si la altura en Y del objeto es 0 o negativa...
                {
                    // Si por la velocidad de caída se pasa un poquito de -0.31, lo obligamos a quedarse exactamente a la altura del piso
                    objectsVelocity[i] = 0; // La velocidad vuelve a 0, al ser el suelo un límite sólido, detiene la caida en seco

                    Vector3 pos = floatingObjects[i].transform.position; // Se toma la posición actual
                    pos.y = groundY + middleOfObject; // Se ubica el objeto en el suelo (sumandole la mitad del objeto para que así el centro pase a estár en la base)
                    floatingObjects[i].transform.position = pos; // Se le da esta nueva posición al objeto
                }
            }
        }
        else // Si la fuerza de gravedad es nula, el objeto queda estático en el ambiente
        {
            for (int i = 0; i < floatingObjects.Length; i++) // Por cada objeto en la nave... (se registra de esta forma para tener el valor exacto en donde queda guardada la información, así el sistema busca directamente los datos segun este valor entero)
            {
                objectsVelocity[i] = 0f;
            }
        }
    }

    // FUNCION PARA CAMBIO DE GRAVEDAD SEGÚN EL PLANETA
    public void SelectGravity(int opcion)
    {
        switch (opcion)
        {
            case 0: ForceGravity = true; gravityValue = 9.81f; break; // PlanetEarth
            case 1: ForceGravity = true; gravityValue = 1.62f; break; // Moon
            case 2: ForceGravity = true; gravityValue = 3.71f; break; // PlanetMars
            case 3: ForceGravity = true; gravityValue = 24.79f; break; // PlanetJupiter
            case 4: ForceGravity = true; gravityValue = 10.44f; break; // PlanetSaturn
            case 5: ForceGravity = true; gravityValue = 8.87f; break; // PlanetVenus
            case 6: ForceGravity = true; gravityValue = 11.15f; break; // PlanetNeptune
            case 7: ForceGravity = true; gravityValue = 3.70f; break; //PlanetMercury
            case 8: ForceGravity = true; gravityValue = 8.69f; break; //PlanetUranus
            //case 9: ForceGravity = false; break; //SubZeroGravity
        }
        Debug.Log("Dropdown seleccionó opción: " + opcion + " | Fuerza: " + gravityValue + " | Gravedad: " + ForceGravity);
    }
}



//CODIGO FUNCIONAL USADO EN EL EXAMEN DE REALIDAD MIXTA
//using UnityEngine;

//public class Gravity : MonoBehaviour
//{
//    public bool ForceGravity = true; // Inicialmente la gravedad estará activada debido a que se encuentra dentro de la nave.

//    private bool previousArduinoGravityButton = false;

//    [Header("Configuración del Planeta")]
//    public float gravityValue = 9.81f; // Guardará la fuerza del planeta seleccionado. Empieza en 9.81 (Tierra)

//    void Start()
//    {
//        ApplyPhysics();
//    }

//    void Update()
//    {
//        bool ChangeGravityInput = Input.GetKeyDown(KeyCode.G); // Sigue funcionando teclado

//        // También permite activar/desactivar gravedad con Arduino
//        if (
//        ArduinoSerialReader.Instance != null
//        &&
//        ArduinoSerialReader.Instance.IsConnected)
//        {
//            bool currentArduinoButton =
//            ArduinoSerialReader.Instance.BoostButton;

//            // Detecta una sola pulsación
//            if (
//            currentArduinoButton
//            &&
//            !previousArduinoGravityButton)
//            {
//                ChangeGravityInput =
//                true;
//            }

//            previousArduinoGravityButton =
//            currentArduinoButton;
//        }

//        // Si cualquiera de los dos lo activa
//        if (ChangeGravityInput)
//        {
//            ForceGravity =
//            !ForceGravity;

//            ApplyPhysics();
//        }
//    }

//    void ApplyPhysics()
//    {
//        Physics.gravity = new Vector3(0, -gravityValue, 0); // Aplicamos el valor del planeta a la gravedad global de Unity (Negativo para que caiga al piso)

//        Rigidbody[] AllObjectsInSpaceship = Object.FindObjectsByType<Rigidbody>((FindObjectsSortMode.None)); // Indica que todos los objetos que tengan rigidbody los tenga en cuenta (pero no necesariamente en orden)

//        foreach (Rigidbody rb in AllObjectsInSpaceship)
//        {
//            rb.WakeUp();

//            rb.useGravity = ForceGravity; // Para que el rigidbody tenga en cuenta el factor de useGravity con la fuerza de gravedad de la tecla g

//            if (!ForceGravity) // Si la fuerza de gravedad es nula...
//            {
//                Vector3 randomDirection = Random.onUnitSphere; // Da una dirección al azar en un vector 3D
//                rb.AddForce(randomDirection * 0.1f, ForceMode.Impulse); // Se le otorga un sutil impulso hacia la dirección aleatoria, simulando la gravedad 0 (para ello usa una especie de salto inicial para no aplicar la fuerza de golpe)
//            }
//            else // Si vuelve la gravedad...
//            {
//                rb.linearDamping = 0.05f; // Se restaura la resistencia y presión del aire.
//                                          // linearDamping es una propiedad del componente Rigidbody o Rigidbody2D que simula la resistencia o fricción del aire 
//                rb.angularDamping = 0.05f; // Evita que los objetos al tocar el piso resbalen
//                                           // angularDamping simula la resistencia del aire o fricción rotacional. Se encarga de desacelerar progresivamente la rotación de un objeto cuando está girando
//            }
//        }
//        Debug.Log("Gravedad en la nave: " + (ForceGravity ? "<Color=green>ACTIVADA</Color>" : "<Color=yellow>DESACTIVADA</Color>"));
//    }
//    // FUNCION PARA CAMBIO DE GRAVEDAD SEGÚN EL PLANETA
//    public void SelectGravity(int opcion)
//    {
//        switch (opcion)
//        {
//            case 0: ForceGravity = true; gravityValue = 9.81f; break; // PlanetEarth
//            case 1: ForceGravity = true; gravityValue = 1.62f; break; // Moon
//            case 2: ForceGravity = true; gravityValue = 3.71f; break; // PlanetMars
//            case 3: ForceGravity = true; gravityValue = 24.79f; break; // PlanetJupiter
//            case 4: ForceGravity = true; gravityValue = 10.44f; break; // PlanetSaturn
//            case 5: ForceGravity = true; gravityValue = 8.87f; break; // PlanetVenus
//            case 6: ForceGravity = true; gravityValue = 11.15f; break; // PlanetNeptune
//            case 7: ForceGravity = true; gravityValue = 3.70f; break; //PlanetMercury
//            case 8: ForceGravity = true; gravityValue = 8.69f; break; //PlanetUranus
//            case 9: ForceGravity = false; break; //SubZeroGravity
//        }
//        Debug.Log("Dropdown seleccionó opción: " + opcion + " | Fuerza: " + gravityValue + " | Gravedad: " + ForceGravity);
//        ApplyPhysics();
//    }
//}



// CUARTO INTENTO
//using System.Xml.Serialization;
//using UnityEngine;

//public class Gravity : MonoBehaviour
//{
//    public bool ForceGravity = true; // Inicialmente la gravedad estará activada debido a que se encuentra dentro de la nave.
//    public float groundY = -0.31f;     // Referencia del suelo de la nave

//    [Header("Configuración del Planeta")]
//    public float gravityValue = 9.81f; // Guardará la fuerza del planeta seleccionado. Empieza en 9.81 (Tierra)

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.G)) // Si presionas l tecla g (este comando es temporal)
//        {
//            ForceGravity = !ForceGravity; // La gravedad se desactiva y viceversa

//            Debug.Log("Gravedad en la nave: " + (ForceGravity ? "<Color=green>ACTIVADA</Color>" : "<Color=yellow>DESACTIVADA</Color>"));
//        }

//        if (ForceGravity == true) // Si la fuerza de gravedad está activada (sea cual sea el planeta)...
//        {
//            GameObject[] AllObjectsInSpaceship = GameObject.FindGameObjectsWithTag("FloatingObjects"); // Indica que todos los objetos que tengan el tag de FloatingObjects los tenga en cuenta (pero no necesariamente en orden)

//            foreach (GameObject obj in AllObjectsInSpaceship)  // Por cada objeto en la nave...
//            {
//                if (obj.name == "Main Camera") // Si el objeto tiene como nombre Main Camera...
//                {
//                    continue; // No 'lo tendrá en cuenta para que no cambie la altura de la cámara (que es la vista del usuario)
//                }

//                if (obj.transform.position.y > groundY) // El objeto  SOLO cae si su altura en Y es más alta que el suelo
//                {
//                    obj.transform.Translate(Vector3.down * gravityValue * Time.deltaTime, Space.World); // El objeto cae por la gravedad del planeta
//                    // el objeto se traslada hacia abajo con la fuerza de gravedad actual (indicada por el planeta seleccionado) en tiempo real, aplicadas a todo el escenario
//                }
//                else
//                {
//                    // Si por la velocidad de caída se pasa un poquito de -0.31, lo obligamos a quedarse exactamente a la altura del piso
//                    Vector3 pos = obj.transform.position;
//                    pos.y = groundY;
//                    obj.transform.position = pos;
//                }
//            } 
//        }// Si la fuerza de gravedad es nula, el objeto queda estático en el ambiente
//    }

//    // FUNCION PARA CAMBIO DE GRAVEDAD SEGÚN EL PLANETA
//    public void SelectGravity(int opcion)
//    {
//        switch (opcion)
//        {
//            case 0: ForceGravity = true; gravityValue = 9.81f; break; // PlanetEarth
//            case 1: ForceGravity = true; gravityValue = 1.62f; break; // Moon
//            case 2: ForceGravity = true; gravityValue = 3.71f; break; // PlanetMars
//            case 3: ForceGravity = true; gravityValue = 24.79f; break; // PlanetJupiter
//            case 4: ForceGravity = true; gravityValue = 10.44f; break; // PlanetSaturn
//            case 5: ForceGravity = true; gravityValue = 8.87f; break; // PlanetVenus
//            case 6: ForceGravity = true; gravityValue = 11.15f; break; // PlanetNeptune
//            case 7: ForceGravity = true; gravityValue = 3.70f; break; //PlanetMercury
//            case 8: ForceGravity = true; gravityValue = 8.69f; break; //PlanetUranus
//            case 9: ForceGravity = false; break; //SubZeroGravity
//        }
//        Debug.Log("Dropdown seleccionó opción: " + opcion + " | Fuerza: " + gravityValue + " | Gravedad: " + ForceGravity);
//    }
//}


// TERCER INTENTO
//using System.Xml.Serialization;
//using UnityEngine;

//public class Gravity : MonoBehaviour
//{
//    public bool ForceGravity = true; // Inicialmente la gravedad estará activada debido a que se encuentra dentro de la nave.
//    public float groundY = 0f;     // Referencia del suelo de la nave

//    [Header("Configuración del Planeta")]
//    public float gravityValue = 9.81f; // Guardará la fuerza del planeta seleccionado. Empieza en 9.81 (Tierra)

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.G)) // Si presionas l tecla g (este comando es temporal)
//        {
//            ForceGravity = !ForceGravity; // La gravedad se desactiva y viceversa
//            // Si la fuerza de gravedad es nula, el objeto queda estático en el ambiente
//            Debug.Log("Gravedad en la nave: " + (ForceGravity ? "<Color=green>ACTIVADA</Color>" : "<Color=yellow>DESACTIVADA</Color>"));

//        }
//        if (ForceGravity == true) // Si la fuerza de gravedad está activada (sea cual sea el planeta)...
//        {
//            GameObject[] AllObjectsInSpaceship = GameObject.FindGameObjectsWithTag("FloatingObjects"); // Indica que todos los objetos que tengan el tag de FloatingObjects los tenga en cuenta (pero no necesariamente en orden)
//            foreach (GameObject obj in AllObjectsInSpaceship)  // Por cada objeto en la nave...
//            {
//                obj.transform.Translate(Vector3.down * gravityValue * Time.deltaTime, Space.World); // El objeto cae por la gravedad del planeta
//                                                                                                    // el objeto se traslada hacia abajo con la fuerza de gravedad actual (indicada por el planeta seleccionado) en tiempo real, aplicadas a todo el escenario
//            }
//        }
//    }

//    // FUNCION PARA CAMBIO DE GRAVEDAD SEGÚN EL PLANETA
//    public void SelectGravity(int opcion)
//    {
//        switch (opcion)
//        {
//            case 0: ForceGravity = true; gravityValue = 9.81f; break; // PlanetEarth
//            case 1: ForceGravity = true; gravityValue = 1.62f; break; // Moon
//            case 2: ForceGravity = true; gravityValue = 3.71f; break; // PlanetMars
//            case 3: ForceGravity = true; gravityValue = 24.79f; break; // PlanetJupiter
//            case 4: ForceGravity = true; gravityValue = 10.44f; break; // PlanetSaturn
//            case 5: ForceGravity = true; gravityValue = 8.87f; break; // PlanetVenus
//            case 6: ForceGravity = true; gravityValue = 11.15f; break; // PlanetNeptune
//            case 7: ForceGravity = true; gravityValue = 3.70f; break; //PlanetMercury
//            case 8: ForceGravity = true; gravityValue = 8.69f; break; //PlanetUranus
//            case 9: ForceGravity = false; break; //SubZeroGravity
//        }
//    }
//}



// SEGUNDO INTENTO
//using UnityEngine;

//public class Gravity : MonoBehaviour
//{
//    public bool ForceGravity = true; // Inicialmente la gravedad estará activada debido a que se encuentra dentro de la nave.

//    [Header("Configuración del Planeta")]
//    public float gravityValue = 9.81f; // Guardará la fuerza del planeta seleccionado. Empieza en 9.81 (Tierra)

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



//PRIMER INTENTO
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
//                    Vector3 randomDirection = Random.onUnitSphere; // Da una dirección al azar en un vector 3D
//                    rb.AddForce(randomDirection * 0.5f, ForceMode.Impulse); // Se le otorga un sutil impulso hacia la dirección aleatoria, simulando la gravedad 0 (para ello usa una especie de salto inicial para no aplicar la fuerza de golpe)
//                }
//            }
//            Debug.Log("Gravedad en la nave: " + (ForceGravity ? "<Color=green>ACTIVADA</Color>" : "<Color=yellow>DESACTIVADA</Color>"));
//        }
//    }
//}