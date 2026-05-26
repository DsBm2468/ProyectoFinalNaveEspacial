using UnityEngine;
using TMPro;

public class SimulationHUDExplorationSpaceship : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text statusText;
    public GameObject PanelSimulations;

    [Header("Parámetros físicos de referencia")]
    public Vector3 gravityEarth = new Vector3(0, -9.81f, 0);
    public float groundY = 0f;     // Referencia del suelo de la nave

    [Header("Conexión con los scripts de las simulaciones")] // Valores guardados en el panel de simulación de gravedad y  descompresión extrema
    public Gravity GravityScript;
    public HatchManager HatchScript;

    [Header("Cronómetro interno de la simulación")]
    private float simulationTime = 0f;
    private bool UIisVisible = false;

    void Start()
    {
        if (PanelSimulations != null)
        {
            PanelSimulations.SetActive(false); // El panel inicialmente no será visible hasta que sea solicitado con el botón de tab
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Si presionas la tecla P...
        {
            UIisVisible = !UIisVisible; // La UI será visible y viceversa

            if (PanelSimulations != null) // Si existe el panel de ajuste de simulaciones...
            {
                PanelSimulations.SetActive(UIisVisible); // Entonces se verá en pantalla dependiendo del estado del boolean UIisVisible
            }
        }

        if (UIisVisible) // Si el panel es visible...
        {
            DoCalculationsForSimulations(); // Se accederá a los calculos para ajustar las simulaciones
        }
    }

    void DoCalculationsForSimulations()
    {
        Rigidbody[] AllObjectsInSpaceship = Object.FindObjectsByType<Rigidbody>((FindObjectsSortMode.None)); // Indica que todos los objetos que tengan rigidbody los tenga en cuenta (pero no necesariamente en orden)
        //GameObject[] AllObjectsInSpaceship = GameObject.FindGameObjectsWithTag("FloatingObjects"); // Indica que todos los objetos que tengan el tag de FloatingObjects los tenga en cuenta (pero no necesariamente en orden)
        
        int quantityObjects = AllObjectsInSpaceship.Length; // Se guarda la cantidad de objetos en escena en una variable

        float kinetic = 0f; // Energía de movimiento, inicialmente estará en 0, al momento que se detecte que la puerta fue abierta, la velocidad de la descompresión hará que la energía cinética suba
        float potential = 0f; // Energia de la altura, este valor se ve reflejado segun donde estén ubicados los objetos cuando la gravedad esté o no activada, cuando los objetos estén en el piso el valor será 0

        if (GravityScript != null && GravityScript.ForceGravity == true) // Si el script de la gravedad está activado y la fuerza tambien...
        {
            simulationTime += Time.deltaTime; // Entonces el tiempo empezará a transcurrir
        }

        foreach (Rigidbody rb in AllObjectsInSpaceship) // Por cada objeto en la nave...
        {
            float mass = rb.mass; // Tendrán un mismo valor de peso

            if (rb.GetComponent<Camera>() != null) // Si el objeto tiene el componente de cámara...
            {
                mass = 70f; // La masa será la de un astronauta (masa promedio según los estándares de la NASA)
            }
            else // Si no, entonces la masa del objeto dependerá del volumen de este
            {
                mass = rb.transform.localScale.x * rb.transform.localScale.y * rb.transform.localScale.z * 2.0f;
                // la escala del objeto en los ejes x y z se multiplican entre si para tener el volumen relativo (que da 1)
                // Para hayar masa la formula es M = Volumen * Densidad,
                // en este caso se usa el valor utilizado (2.0f) equivale al peso aproximado de los materiales
                // que generalmente se utilizan en objetos encontrados en las naves
                // (aleaciones de aluminio, titnio, acero inoxidable y polimeros reforzados con fibra de carbono)
                // CABE ACLARAR QUE AUNQUE LOS VALORES SE VEN PEQUEÑOS, EL SISTEMA LOS DETECTA COMO SERIAN EN LA VIDA REAL (2.0f representa 2000kg/m3)
            }

            // Energía Cinética: 0.5 * masa * velocidad al cuadrado
            float v2 = rb.linearVelocity.sqrMagnitude;
            kinetic += 0.5f * mass * v2;

            // Energía Potencial: masa * gravedad * altura
            float gMag = GravityScript != null ? GravityScript.gravityValue : 9.81f;
            float h = Mathf.Max(0f, rb.transform.position.y - groundY);
            potential += mass * gMag * h;

            float total = kinetic + potential;
        }
    }
}



//private void OnEnable()
//{
//    if (SimulationManager.Instance != null)
//        SimulationManager.Instance.OnSimulationStep += Refresh;
//}

//private void OnDisable()
//{
//    if (SimulationManager.Instance != null)
//        SimulationManager.Instance.OnSimulationStep -= Refresh;
//}

//private void Refresh(float dt)
//{
//    var sim = SimulationManager.Instance;
//    int count = ParticleWorld.All.Count;

//    // Energías acumuladas del sistema completo
//    float kinetic = 0f;
//    float potential = 0f;
//    float gMag = gravityRef.magnitude;

//    foreach (var p in ParticleWorld.All)
//    {
//        float v2 = p.Velocity.sqrMagnitude;
//        kinetic += 0.5f * p.Mass * v2;

//        float h = p.Position.y - groundY;
//        potential += p.Mass * gMag * h;
//    }

//    float total = kinetic + potential;
//    string state = sim.isPaused ? "PAUSED" : "RUNNING";

//    statusText.text =
//        $"[ {state} ]\n" +
//        $"t = {sim.SimulationTime:F2} s   step #{sim.StepCount}\n" +
//        $"Δt = {sim.updateTime * 1000f:F1} ms   timeScale = {sim.timeScale:F2}x\n" +
//        $"\n" +
//        $"Partículas: {count}\n" +
//        $"Ek = {kinetic:F1} J\n" +
//        $"Ep = {potential:F1} J\n" +
//        $"E  = {total:F1} J";
//}
