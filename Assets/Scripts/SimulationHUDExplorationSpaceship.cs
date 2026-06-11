using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationHUDExplorationSpaceship : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text statusText;
    public GameObject PanelSimulations;

    [Header("Parámetros físicos de referencia")]
    // public Vector3 gravityEarth = new Vector3(0, -9.81f, 0);
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
            PanelSimulations.SetActive(false); // El panel inicialmente no será visible hasta que sea solicitado con el botón de P
        }

        if (statusText != null)
        {
            statusText.gameObject.SetActive(true); // La información de statusText Desde el inicio estará activada
        }

        GameObject floor = GameObject.FindWithTag("FloorSpaceship"); // Se indica el objeto que tiene el tag de piso
        if (floor != null) groundY = floor.transform.position.y;
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

        //if (UIisVisible) // Si el panel es visible...
        //{
            DoCalculationsForSimulations(); // Se accederá a los calculos para ajustar las simulaciones
        //}
    }

    void DoCalculationsForSimulations()
    {
        //Rigidbody[] AllObjectsInSpaceship = Object.FindObjectsByType<Rigidbody>((FindObjectsSortMode.None)); // Indica que todos los objetos que tengan rigidbody los tenga en cuenta (pero no necesariamente en orden)
        GameObject[] AllObjectsInSpaceship = GameObject.FindGameObjectsWithTag("FloatingObjects"); // Indica que todos los objetos que tengan el tag de FloatingObjects los tenga en cuenta (pero no necesariamente en orden)
        
        int quantityObjects = AllObjectsInSpaceship.Length; // Se guarda la cantidad de objetos en escena en una variable

        float kinetic = 0f; // Energía de movimiento, inicialmente estará en 0, al momento que se detecte que la puerta fue abierta, la velocidad de la descompresión hará que la energía cinética suba
        float potential = 0f; // Energia de la altura, este valor se ve reflejado segun donde estén ubicados los objetos cuando la gravedad esté o no activada, cuando los objetos estén en el piso el valor será 0
        
        simulationTime += Time.deltaTime;
        //if (GravityScript != null && GravityScript.ForceGravity == true) // Si el script de la gravedad está activado y la fuerza tambien...
        //{
        //    simulationTime += Time.deltaTime; // Entonces el tiempo empezará a transcurrir
        //}

        for (int i = 0; i < AllObjectsInSpaceship.Length; i++) // Por cada objeto en la nave... (se registra de esta forma para tener el valor exacto en donde queda guardada la información, así el sistema busca directamente los datos segun este valor entero)
        {
            GameObject obj = AllObjectsInSpaceship[i]; // Se guardan los objetos en una variable para su facil acceso

           // Se determina la masa del objeto según su volumen
           // SE DEJA ESTA INFORMACIÓN EN ESTE SCRIPT YA QUE SU OBJETIVO ES MOSTRAR LA ENERGIA CINETICA Y POTENCIAL DE LA NAVE, ESTOS VALORES DEPENDEN DE LA MASA
            float mass = obj.transform.localScale.x * obj.transform.localScale.y * obj.transform.localScale.z * 2.0f;
            // la escala del objeto en los ejes x y z se multiplican entre si para tener el volumen relativo (que da 1)
            // Para hayar masa la formula es M = Volumen * Densidad,
            // en este caso se usa el valor utilizado (2.0f) equivale al peso aproximado de los materiales
            // que generalmente se utilizan en objetos encontrados en las naves
            // (aleaciones de aluminio, titnio, acero inoxidable y polimeros reforzados con fibra de carbono)
            // CABE ACLARAR QUE AUNQUE LOS VALORES SE VEN PEQUEÑOS, EL SISTEMA LOS DETECTA COMO SERIAN EN LA VIDA REAL (2.0f representa 2000kg/m3)

            // Energía Cinética: 0.5 * masa * velocidad al cuadrado (Es la energía que genera un cuerpo cuando está ganando velocidad)
            float velocidadObjeto = GravityScript.objectsVelocity[i];
            float v2 = velocidadObjeto * velocidadObjeto; // Se obtiene ls velocidad al cuadrado
            kinetic += 0.5f * mass * v2;

            // Energía Potencial: masa * gravedad * altura (Es la energía que tiene un objeto guardado solo por estar elevado)
            // Es la energía que un objrto  tiene almacenada debido a su posición (altura) dentro de un campo gravitatorio (en este caso es energía potencial gravitatoria)
            float gMag = GravityScript != null ? GravityScript.gravityValue : 9.81f; // Es la magnitud de la gravedad, indica la aceleración que ejerce el planeta seleccionado
            float h = Mathf.Max(0f, obj.transform.position.y - groundY); // h es la altura relativa (distancia vertical entre el objeto y el piso de la nave)
            potential += mass * gMag * h;  
        }
        
        // Finalmente, se aplica el principio de conservación de energía dando la energía mecánica total
        // De esta forma se muestra como la pérdida de altura equivale a la ganancia de velocidad
        // Fórmula Emecánica = Ecinetina + Epotencial
        float total = kinetic + potential;

        // IDENTIFICACIÓN DE GRAVEDAD PRESENTE EN LA NAVE
        string currentEntorn = "Desconocido"; // En caso de haber fallas, el entorno aparecería como desconocido
        if (GravityScript.ForceGravity == false) // Si la fuerza de grvedad está desáctivada... 
        {
            currentEntorn = "<color=yellow>Gravedad Cero</color>"; // El entorno aparecerá como gravedad cero
        } 
        else // Si no...
        {
            float currentG = GravityScript.gravityValue; // Se indicará en esta variable el valor de la gravedad del script
            if (Mathf.Approximately(currentG, 9.81f)) currentEntorn = "Planeta Tierra"; // Si el valor es aproximadamente (...) el entorno aparecerá como (...)
            else if (Mathf.Approximately(currentG, 1.62f)) currentEntorn = "La Luna";
            else if (Mathf.Approximately(currentG, 3.71f)) currentEntorn = "Planeta Marte";
            else if (Mathf.Approximately(currentG, 24.79f)) currentEntorn = "Planeta Júpiter";
            else if (Mathf.Approximately(currentG, 10.44f)) currentEntorn = "Planeta Saturno";
            else if (Mathf.Approximately(currentG, 8.87f)) currentEntorn = "Planeta Venus";
            else if (Mathf.Approximately(currentG, 11.15f)) currentEntorn = "Planeta Neptuno";
            else if (Mathf.Approximately(currentG, 3.70f)) currentEntorn = "Planeta Mercurio";
            else if (Mathf.Approximately(currentG, 8.69f)) currentEntorn = "Planeta Urano";
        }

        // IDENTIFICACIÓN DEL ESTADO DE LA COMPUERTA
        string currentStateDoor = "No detectada"; // En caso de haber fallas, el entorno aparecería como No detectada
        float currentSuction = 0f;

        if (HatchScript != null) 
        {
            currentSuction = HatchScript.suctionForce; // Detecta el valor actual de presión/succión directamente del script/slider del panel

            if (HatchScript.DoorIsOpen == true) // Si la puerta está abierta...
            {
                currentStateDoor = "<color=red>ABIERTA!! (PELIGRO DE DESCOMPRESIÓN EXTREMA)</color>";
            }
            else
            {
                currentStateDoor = "<color=green>CERRADA (ÁREA SEGURA)</color>";
                currentSuction = 0f;
            }
        }

        if (statusText != null)
        {
            statusText.text = $"<b>SISTEMA DE TELEMETRÍA ESPACIAL</b>\n" +
                              $"Tiempo de Simulación: {simulationTime:F2}s\n" +
                              $"Objetos en la nave: {quantityObjects}\n" +
                              $"Entorno Actual: {currentEntorn}\n" +
                              $"Gravedad Activa: {(GravityScript.ForceGravity ? GravityScript.gravityValue.ToString("F2") : "0.00")} m/s²\n" +
                              $"Estado de la compuerta: {currentStateDoor}\n" +
                              $"Nivel de succión: {currentSuction:F0} m/s²\n" +
                              $"-------------------------------------------------\n" +
                              $"Energía Cinética: {kinetic:F2} J\n" +
                              $"Energía Potencial: {potential:F2} J\n" +
                              $"Energía Mecánica Total: {total:F2} J";
        }
    }
}



// PRIMER INTENTO
//using UnityEngine;
//using TMPro;

//public class SimulationHUDExplorationSpaceship : MonoBehaviour
//{
//    [Header("UI")]
//    public TMP_Text statusText;
//    public GameObject PanelSimulations;

//    [Header("Parámetros físicos de referencia")]
//    public Vector3 gravityEarth = new Vector3(0, -9.81f, 0);
//    public float groundY = 0f;     // Referencia del suelo de la nave

//    [Header("Conexión con los scripts de las simulaciones")] // Valores guardados en el panel de simulación de gravedad y  descompresión extrema
//    public Gravity GravityScript;
//    public HatchManager HatchScript;

//    [Header("Cronómetro interno de la simulación")]
//    private float simulationTime = 0f;
//    private bool UIisVisible = false;

//    void Start()
//    {
//        if (PanelSimulations != null)
//        {
//            PanelSimulations.SetActive(false); // El panel inicialmente no será visible hasta que sea solicitado con el botón de tab
//        }
//    }

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.P)) // Si presionas la tecla P...
//        {
//            UIisVisible = !UIisVisible; // La UI será visible y viceversa

//            if (PanelSimulations != null) // Si existe el panel de ajuste de simulaciones...
//            {
//                PanelSimulations.SetActive(UIisVisible); // Entonces se verá en pantalla dependiendo del estado del boolean UIisVisible
//            }
//        }

//        if (UIisVisible) // Si el panel es visible...
//        {
//            DoCalculationsForSimulations(); // Se accederá a los calculos para ajustar las simulaciones
//        }
//    }

//    void DoCalculationsForSimulations()
//    {
//        Rigidbody[] AllObjectsInSpaceship = Object.FindObjectsByType<Rigidbody>((FindObjectsSortMode.None)); // Indica que todos los objetos que tengan rigidbody los tenga en cuenta (pero no necesariamente en orden)
//                                                                                                             //GameObject[] AllObjectsInSpaceship = GameObject.FindGameObjectsWithTag("FloatingObjects"); // Indica que todos los objetos que tengan el tag de FloatingObjects los tenga en cuenta (pero no necesariamente en orden)

//        int quantityObjects = AllObjectsInSpaceship.Length; // Se guarda la cantidad de objetos en escena en una variable

//        float kinetic = 0f; // Energía de movimiento, inicialmente estará en 0, al momento que se detecte que la puerta fue abierta, la velocidad de la descompresión hará que la energía cinética suba
//        float potential = 0f; // Energia de la altura, este valor se ve reflejado segun donde estén ubicados los objetos cuando la gravedad esté o no activada, cuando los objetos estén en el piso el valor será 0

//        if (GravityScript != null && GravityScript.ForceGravity == true) // Si el script de la gravedad está activado y la fuerza tambien...
//        {
//            simulationTime += Time.deltaTime; // Entonces el tiempo empezará a transcurrir
//        }

//        foreach (Rigidbody rb in AllObjectsInSpaceship) // Por cada objeto en la nave...
//        {
//            float mass = rb.mass; // Tendrán un mismo valor de peso

//            if (rb.GetComponent<Camera>() != null) // Si el objeto tiene el componente de cámara...
//            {
//                mass = 70f; // La masa será la de un astronauta (masa promedio según los estándares de la NASA)
//            }
//            else // Si no, entonces la masa del objeto dependerá del volumen de este
//            {
//                mass = rb.transform.localScale.x * rb.transform.localScale.y * rb.transform.localScale.z * 2.0f;
//                // la escala del objeto en los ejes x y z se multiplican entre si para tener el volumen relativo (que da 1)
//                // Para hayar masa la formula es M = Volumen * Densidad,
//                // en este caso se usa el valor utilizado (2.0f) equivale al peso aproximado de los materiales
//                // que generalmente se utilizan en objetos encontrados en las naves
//                // (aleaciones de aluminio, titnio, acero inoxidable y polimeros reforzados con fibra de carbono)
//                // CABE ACLARAR QUE AUNQUE LOS VALORES SE VEN PEQUEÑOS, EL SISTEMA LOS DETECTA COMO SERIAN EN LA VIDA REAL (2.0f representa 2000kg/m3)
//            }

//            // Energía Cinética: 0.5 * masa * velocidad al cuadrado
//            float v2 = rb.linearVelocity.sqrMagnitude;
//            kinetic += 0.5f * mass * v2;

//            // Energía Potencial: masa * gravedad * altura
//            float gMag = GravityScript != null ? GravityScript.gravityValue : 9.81f;
//            float h = Mathf.Max(0f, rb.transform.position.y - groundY);
//            potential += mass * gMag * h;

//            float total = kinetic + potential;
//        }
//    }
//}


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
