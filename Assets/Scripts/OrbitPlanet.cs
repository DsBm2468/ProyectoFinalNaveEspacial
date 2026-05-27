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

                Vector3 planetGlobalPos = p.Planet.position;

                if (p.Planet.parent != null)
                {
                    planetGlobalPos = p.Planet.parent.TransformPoint(p.Planet.localPosition);
                }

                float currentDistance = Vector3.Distance(Spaceship.position, planetGlobalPos); // Registra la distancia entre la nave y el planeta actual
                if (currentDistance <= p.GravityRadioPlanet) // Si la nave esta dentro del rango de gravedad del planeta...
                {
                    // La nave se dirige al centro del planeta, dando la simlación de acercarse al planeta debido a su gravedad
                    // Para ello se resta la posición del planeta por la posicion actual de la nave
                    Vector3 directionToCenter = (planetGlobalPos - Spaceship.position).normalized;

                    // La nave se dirige al centro del planeta, dando la simlación de acercarse al planeta debido a su gravedad
                    // Se mueven las coordenadas de la nave en la escena hacia el centro del planeta, teniendo en cuenta la fuerza de atracción correspondiente a cada cuerpo celeste
                    //Spaceship.Translate(directionToCenter * p.ForceAtrraction * Time.deltaTime, Space.World);
                    Spaceship.position += directionToCenter * p.ForceAtrraction * Time.deltaTime;
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
