using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SimulationHUDSpace : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text statusText;
    public GameObject PanelSimulations;
    public TMP_Dropdown planetDropdown;
    public Slider orbitSlider;
    public Slider rotationSlider;

    //[Header("Configuración")]
    //public Vector3 gravityRef = new Vector3(0, -9.81f, 0);
    //public float groundY = 0f;     // referencia para energía potencial

    [Header("Conexión con los scripts de las simulaciones")] // Valores guardados en el panel de simulación de orbita del planeta seleccionado, la fuerza gravitatoria del mismo y la fuerza de los potenciadores
    public PlanetsBehavior SelectedPlanet;
    public OrbitPlanet OrbitPlanetScript;

    [Header("Cronómetro interno de la simulación")]
    private float simulationTime = 0f;
    private bool UIisVisible = false;

    private PlanetsBehavior.Planet lastCheckedPlanet;

    void Start()
    {
        if (PanelSimulations != null)
        {
            PanelSimulations.SetActive(false); // El panel inicialmente no será visible hasta que sea solicitado con el botón de P
        }

        if (statusText != null)
        {
            statusText.gameObject.SetActive(true); // La información de statusText Desde el inicio estará activada
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // Si presionas R...
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);// Reinicia la escena
        }

        if (Input.GetKeyDown(KeyCode.P)) // Si presionas la tecla P...
        {
            UIisVisible = !UIisVisible; // La UI será visible y viceversa

            if (PanelSimulations != null) // Si existe el panel de ajuste de simulaciones...
            {
                PanelSimulations.SetActive(UIisVisible); // Entonces se verá en pantalla dependiendo del estado del boolean UIisVisible
            }
        }

        PlanetsBehavior[] AllCelestialBodies = Object.FindObjectsByType<PlanetsBehavior>(FindObjectsSortMode.None);
        foreach (PlanetsBehavior CB in AllCelestialBodies)
        {
            if (CB.planetSelectioned == PlanetsBehavior.SelectedPlanetGlobal)
            {
                SelectedPlanet = CB; // Vinculamos el cuerpo celeste enfocado para los cálculos
                break;
            }
        }

        DoCalculationsForSimulations(); // Se accederá a los calculos para ajustar las simulaciones
    }

    void DoCalculationsForSimulations()
    {
        simulationTime += Time.deltaTime;

        // IDENTIFICACIÓN DE ELEMENTOS DEL SISTEMA SOLAR (CON SU RESPECTIVA ROTACION EN ORBITA Y ROTACION SOBRE SI MISMOS)

        // Valores que aparecerán por defecto (Estos no alterarán por ahora la simulación) CelestialBody es CB
        string CBName = "Desconocido"; // En caso de haber fallas
        float CBOrbit = 0f;
        float CBRotation = 0f;
        float CBMass = 0;
        float CBKinetic = 0f; // Energía de movimiento

        if (SelectedPlanet != null)
        {
            // Se registra la información relevante del cuerpo celeste
            CBName = SelectedPlanet.planetSelectioned.ToString(); // Nombre
            CBOrbit = SelectedPlanet.OrbitalSpeed; // Velocidad en órbita (Alrededor del sol)
            CBRotation = SelectedPlanet.RotationalSpeedHimself; //Velocidad de rotación alrededor de si mismo

            CBMass = SelectedPlanet.transform.localScale.x * SelectedPlanet.transform.localScale.y * SelectedPlanet.transform.localScale.z * 2.0f;
            // la escala del objeto en los ejes x y z se multiplican entre si para tener el volumen relativo (que da 1)
            // Para hayar masa la formula es M = Volumen * Densidad,
            // en este caso se usa el valor utilizado (2.0f) equivale a la densidad constante estándar de los cuerpos celestes
            // es un promedio intermedio entre un planeta de roca y uno de gas
            // Generalmente un planeta rocoso como la tierra o merecurio oscila en 5000 kg/m^3, y en planetas gaseosos como Júpiter ronda los 1300 kg/m^3
            // CABE ACLARAR QUE AUNQUE LOS VALORES SE VEN PEQUEÑOS, EL SISTEMA LOS DETECTA COMO SERIAN EN LA VIDA REAL (2.0f representa 2000kg/m3)

            // Energía Cinética: 0.5 * masa * velocidad al cuadrado (Es la energía que genera un cuerpo cuando está ganando velocidad)
            float v2 = CBOrbit * CBOrbit; // Se obtiene la velocidad al cuadrado
            CBKinetic += 0.5f * CBMass * v2;
        }

        if (statusText != null)
        {
            statusText.text = $"<b>SISTEMA DE TELEMETRÍA ESPACIAL</b>\n" +
                              $"Tiempo de Simulación: {simulationTime:F2}s\n" +
                              $"Cuerpo celeste enfocado: {CBName}\n" +
                              $"Masa del cuerpo: {CBMass:F2} kg\n" +
                              $"Velocidad de órbita: {CBOrbit:F1} km/h\n" +
                              $"Velocidad de rotación propia: {CBRotation:f1} km/h\n" +
                              $"-------------------------------------------------\n" +
                              $"Energía Cinética: {CBKinetic:F2} J\n" +
                              $"Energía Potencial: {0.00:F2} J (Sin gravedad de superficie)\n" +
                              $"Energía Mecánica Total: {CBKinetic:F2} J";
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
