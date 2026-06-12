using UnityEngine;

public class OrbitPlanet : MonoBehaviour
{
    public enum Planetype { Mercurio, Venus, Tierra, Marte, Jupiter, Saturno, Urano, Neptuno, Luna }
   
    [System.Serializable]
    public class GravitationalProperties
    {
        public string PlanetName;
        public Transform Planet;
        public Planetype PSelectioned;
        public float GravityRadioPlanet;
        public float ForceAtrraction;
    }

    [Header("Objetos de la Simulación")]
    public Transform Spaceship;

    [Header("Lista de Cuerpos Celestes")]
    public GravitationalProperties[] celestialBodies; // Se crea una lista en el que se encuentra la informacion de los cuerpos celestes

    void Start()
    {
        foreach (GravitationalProperties p in celestialBodies) // Por cada planeta de la lista...
        {
            // Inicialmente, los planetas contarán con su respectiva rotación y orbita
            switch (p.PSelectioned)
            {
                case Planetype.Mercurio:
                    p.GravityRadioPlanet = 60f;
                    p.ForceAtrraction = 15f;
                    break;
                case Planetype.Venus:
                    p.GravityRadioPlanet = 90f;
                    p.ForceAtrraction = 20f;
                    break;
                case Planetype.Tierra:
                    p.GravityRadioPlanet = 100f;
                    p.ForceAtrraction = 25f;
                    break;
                case Planetype.Marte:
                    p.GravityRadioPlanet = 70f;
                    p.ForceAtrraction = 15f;
                    break;
                case Planetype.Jupiter:
                    p.GravityRadioPlanet = 250f;
                    p.ForceAtrraction = 50f;
                    break;
                case Planetype.Saturno:
                    p.GravityRadioPlanet = 200f;
                    p.ForceAtrraction = 40f;
                    break;
                case Planetype.Urano:
                    p.GravityRadioPlanet = 140f;
                    p.ForceAtrraction = 30f;
                    break;
                case Planetype.Neptuno:
                    p.GravityRadioPlanet = 130f;
                    p.ForceAtrraction = 30f;
                    break;
                case Planetype.Luna:
                    p.GravityRadioPlanet = 40f;
                    p.ForceAtrraction = 10f;
                    break;
            }
        }
    }

    void Update()
    {
        if (Spaceship != null && celestialBodies != null) // Si hay en la escena la nave y los cuerpos celestes... 
        {
            foreach (GravitationalProperties p in celestialBodies) // Por cada planeta de la lista...
            {
                if (p.Planet == null) continue;

                Vector3 planetGlobalPos = p.Planet.position; // Se detecta la posicion de cada planeta

                if (p.Planet.parent != null)
                {
                    planetGlobalPos = p.Planet.parent.TransformPoint(p.Planet.localPosition);
                }

                // Para dar la fuerza gravitatoria de cada cuerpo celete se aplica la ley de gravitación universal de Newton (Establece que todo objeto
                // en el universo que posee masa atrae a cualquier otro objeto con masa mediante una fuerza directamente proporcional al producto de sus masas e inversamente
                // proporcional al cuadrado de la distancia que los separa.)

                // Siendo u fórmula F = G ((m1*m2)/r^2)
                // G = Constante de gravitación universal     m1 y m2 = masa del cuerpo 1 y 2     r^2 = distancia entre cuerpos

                // Para hacer la simulación, debido a que aplicar esta fórmula a cada cuerpo celeste del sistema solar ocuparía mucho procesador en un bucle foreach
                // se adaptó la Ley de Newton mediante un modelo de "Esfera de Influencia"(SOI) (Es la región esférica alrededor de un cuerpo celeste
                // donde su atracción gravitatoria es dominante sobre la de otros cuerpos más masivos, como el Sol. Util en la astrodinámica para calcular trayectorias y orbitas)
                // para eso se aplican 3 etapas: 

                float currentDistance = Vector3.Distance(Spaceship.position, planetGlobalPos); // Primero se registra la distancia entre la nave y el planeta actual

                if (currentDistance <= p.GravityRadioPlanet) // Si la nave esta dentro del rango de gravedad del planeta...
                {
                    // La nave se dirige al centro del planeta, dando la simlación de acercarse al planeta debido a su gravedad
                    // Para ello sigue el paso 2. se resta la posición del planeta por la posicion actual de la nave
                    Vector3 directionToCenter = (planetGlobalPos - Spaceship.position).normalized;

                    // Finalmente, Se mueven las coordenadas de la nave en la escena hacia el centro del planeta, teniendo en cuenta la fuerza de atracción correspondiente a cada cuerpo celeste
                    Spaceship.position += directionToCenter * p.ForceAtrraction * Time.deltaTime; // se usa += para que el valor permite percibir una atracción constante, suave y fluida
                    //Spaceship.Translate(directionToCenter * p.ForceAtrraction * Time.deltaTime, Space.World);
                    break; // Se aplica este break para que no se aplique esta fuerza constantemente, solo cuando se encuentra la nave en su área
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (celestialBodies == null) return;
        foreach (GravitationalProperties p in celestialBodies)
        {
            if (p.Planet != null)
            {
                Gizmos.color = Color.red;
                Vector3 planetWorldPos = p.Planet.position;
                if (p.Planet.parent != null)
                {
                    planetWorldPos = p.Planet.parent.TransformPoint(p.Planet.localPosition);
                }
                Gizmos.DrawWireSphere(planetWorldPos, p.GravityRadioPlanet);
            }
        }
    }
}

// INTENTO POR HACER QUE SEA MODIFICABLE LA FUERZA GRAVITATORIA DE LOS CUERPOS CELESTES
//using Unity.Collections.LowLevel.Unsafe;
//using UnityEngine;

//public class OrbitPlanet : MonoBehaviour
//{
//    public enum Planetype { Sol, Mercurio, Venus, Tierra, Marte, Jupiter, Saturno, Urano, Neptuno, Luna }

//    [System.Serializable]
//    public class GravitationalProperties
//    {
//        public string PlanetName;
//        public Transform Planet;
//        public Planetype PSelectioned;
//        public float GravityRadioPlanet;
//        public float ForceAtraction;

//        //[Header("Collision de cuerpos celestes cuando la nave se choca con ellos")]
//        //public float CollisionRadius;
//    }

//    [Header("Objetos de la Simulación")]
//    public Transform Spaceship;

//    //[Header("UI de Fin de la simulación")]
//    //public GameObject GameOverCanvas;

//    [Header("Lista de Cuerpos Celestes")]
//    public GravitationalProperties[] celestialBodies; // Se crea una lista en el que se encuentra la informacion de los cuerpos celestes

//    void Start()
//    {
//        // Si el Canvas de Game Over está activo, se desactiva
//        //if (GameOverCanvas != null) GameOverCanvas.SetActive(false);

//        foreach (GravitationalProperties p in celestialBodies) // Por cada planeta de la lista...
//        {
//            // Inicialmente, los planetas contarán con su respectiva rotación y orbita
//            switch (p.PSelectioned)
//            {
//                case Planetype.Sol:
//                    p.GravityRadioPlanet = 30f;
//                    p.ForceAtraction = 80f;
//                    break;
//                case Planetype.Mercurio:
//                    p.GravityRadioPlanet = 60f;
//                    p.ForceAtraction = 15f;
//                    break;
//                case Planetype.Venus:
//                    p.GravityRadioPlanet = 90f;
//                    p.ForceAtraction = 20f;
//                    break;
//                case Planetype.Tierra:
//                    p.GravityRadioPlanet = 100f;
//                    p.ForceAtraction = 25f;
//                    break;
//                case Planetype.Marte:
//                    p.GravityRadioPlanet = 70f;
//                    p.ForceAtraction = 15f;
//                    break;
//                case Planetype.Jupiter:
//                    p.GravityRadioPlanet = 250f;
//                    p.ForceAtraction = 50f;
//                    break;
//                case Planetype.Saturno:
//                    p.GravityRadioPlanet = 200f;
//                    p.ForceAtraction = 40f;
//                    break;
//                case Planetype.Urano:
//                    p.GravityRadioPlanet = 140f;
//                    p.ForceAtraction = 30f;
//                    break;
//                case Planetype.Neptuno:
//                    p.GravityRadioPlanet = 130f;
//                    p.ForceAtraction = 30f;
//                    break;
//                case Planetype.Luna:
//                    p.GravityRadioPlanet = 40f;
//                    p.ForceAtraction = 10f;
//                    break;
//            }
//        }
//    }

//    void Update()
//    {
//        if (Spaceship != null && celestialBodies != null) // Si hay en la escena la nave y los cuerpos celestes... 
//        {
//            foreach (GravitationalProperties p in celestialBodies) // Por cada planeta de la lista...
//            {
//                if (p.Planet == null) continue;

//                Vector3 planetGlobalPos = p.Planet.position; // Se detecta la posicion de cada planeta

//                if (p.Planet.parent != null)
//                {
//                    planetGlobalPos = p.Planet.parent.TransformPoint(p.Planet.localPosition);
//                }

//                // Para dar la fuerza gravitatoria de cada cuerpo celete se aplica la ley de gravitación universal de Newton (Establece que todo objeto
//                // en el universo que posee masa atrae a cualquier otro objeto con masa mediante una fuerza directamente proporcional al producto de sus masas e inversamente
//                // proporcional al cuadrado de la distancia que los separa.)

//                // Siendo u fórmula F = G ((m1*m2)/r^2)
//                // G = Constante de gravitación universal     m1 y m2 = masa del cuerpo 1 y 2     r^2 = distancia entre cuerpos

//                // Para hacer la simulación, debido a que aplicar esta fórmula a cada cuerpo celeste del sistema solar ocuparía mucho procesador en un bucle foreach
//                // se adaptó la Ley de Newton mediante un modelo de "Esfera de Influencia"(SOI) (Es la región esférica alrededor de un cuerpo celeste
//                // donde su atracción gravitatoria es dominante sobre la de otros cuerpos más masivos, como el Sol. Util en la astrodinámica para calcular trayectorias y orbitas)
//                // para eso se aplican 3 etapas: 

//                float currentDistance = Vector3.Distance(Spaceship.position, planetGlobalPos); // Primero se registra la distancia entre la nave y el planeta actual

//                if (currentDistance <= p.GravityRadioPlanet) // Si la nave esta dentro del rango de gravedad del planeta...
//                {
//                    // La nave se dirige al centro del planeta, dando la simulación de acercarse al planeta debido a su gravedad
//                    // Para ello sigue el paso 2. se resta la posición del planeta por la posicion actual de la nave
//                    Vector3 directionToCenter = (planetGlobalPos - Spaceship.position).normalized;

//                    // Finalmente, Se mueven las coordenadas de la nave en la escena hacia el centro del planeta, teniendo en cuenta la fuerza de atracción correspondiente a cada cuerpo celeste
//                    Spaceship.position += directionToCenter * p.ForceAtraction * Time.deltaTime; // se usa += para que el valor permite percibir una atracción constante, suave y fluida
//                    //Spaceship.Translate(directionToCenter * p.ForceAtrraction * Time.deltaTime, Space.World);
//                    break; // Se aplica este break para que no se aplique esta fuerza constantemente, solo cuando se encuentra la nave en su área
//                }
//            }
//        }
//    }

//    //private void ActiveGameOver(Collider other)
//    //{
//    //    if (other.CompareTag("Player") || other.transform == Spaceship)
//    //    {
//    //        if (GameOverCanvas != null)
//    //        {
//    //            GameOverCanvas.SetActive(true); // Muestra el cartel de "Nave destruida / Game Over"
//    //            Time.timeScale = 0f; // Pausa el juego por completo para denotar el fin de la simulación
//    //        }
//    //        Debug.LogWarning("¡LA NAVE SE HA ESTRELLADO! GAME OVER.");
//    //    }
//    //}

//    void OnDrawGizmos()
//    {
//        if (celestialBodies == null) return;
//        foreach (GravitationalProperties p in celestialBodies)
//        {
//            if (p.Planet != null)
//            {
//                Gizmos.color = Color.red;
//                Vector3 planetWorldPos = p.Planet.position;
//                if (p.Planet.parent != null)
//                {
//                    planetWorldPos = p.Planet.parent.TransformPoint(p.Planet.localPosition);
//                }
//                Gizmos.DrawWireSphere(planetWorldPos, p.GravityRadioPlanet);
//            }
//        }
//    }

//    //public void ChangeGravityForce(float option)
//    //{
//    //    if (celestialBodies == null) return;

//    //    foreach (GravitationalProperties p in celestialBodies)
//    //    {
//    //        if (p.PSelectioned.ToString().ToLower() == PlanetsBehavior.SelectedPlanetGlobal.ToString().ToLower())
//    //        {
//    //            p.ForceAtraction = option;
//    //            break;
//    //        }
//    //    }
//    //}
//}
