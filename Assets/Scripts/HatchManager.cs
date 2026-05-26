using UnityEngine;
using UnityEngine.UI;

public class HatchManager : MonoBehaviour // Control de la escotilla
{
    [Header("Componentes de la puerta")]
    public Transform partDoor1; // Se utiliza transform porque solo se necesita mover la posición
    public Transform partDoor2;

    [Header("Configuración del movimiento de la puerta")]
    public float apertureDistance = 1.5f;
    public float moveVelocity = 3f;

    [Header("Configuración de la descompresión explosiva")] // Descompresión explosiva es la perdida de presión extrema en un ambiente
    public Transform suctionPoint;
    public float suctionForce = 7f;
    public float suctionRange = 10f;
    public TMPro.TMP_Text TextPressionSlider;

    [Header("UI al salir de la nave")]
    public GameObject DeathCanvas;
    public Transform UserCamera;
    private bool userIsDead = false;
    public Button DeathAcceptButton;
    private bool previousArduinoAccept = false;

    // Se guarda en privado la información de la posición inicial de la puerta (cerrada)
    private Vector3 ClosePositionDoor1;
    private Vector3 ClosePositionDoor2;

    // Se guarda en privado la información de la posición de la puerta (cerrada)
    private Vector3 OpenPositionDoor1;
    private Vector3 OpenPositionDoor2;

    private bool DoorIsOpen = false; // Inicialmente la puerta estará cerrada

    private bool previousArduinoButton = false;

    void Start()
    {
        // Posición actual de la escotilla (CERRADA)
        ClosePositionDoor1 = partDoor1.localPosition; // Registra la posición inicial de la puerta segun las coordenadas x y z, enfocandose especialmente en el de x
        ClosePositionDoor2 = partDoor2.localPosition;

        // Inicialmente se deja la información de la puerta abierta asi
        OpenPositionDoor1 = ClosePositionDoor1;
        OpenPositionDoor2 = ClosePositionDoor2;

        // Cambia la posición de la escotilla para que esté abierta
        OpenPositionDoor1.x = ClosePositionDoor1.x + apertureDistance; // Se suma la posición en x de la puerta cuando esta cerrada por la distancia de apertura
        OpenPositionDoor2.x = ClosePositionDoor2.x + apertureDistance;

        if (TextPressionSlider != null)
        {
            TextPressionSlider.text = suctionForce.ToString("F0"); // Esto pondrá el "7" automáticamente en la UI
        }
    }

    void Update()
    {
        bool OpenDoorInput = Input.GetKeyDown(KeyCode.Space); // Sigue funcionando teclado

        // Si Arduino está conectado también permitirá abrir/cerrar la puerta
        if (
        ArduinoSerialReader.Instance != null
        &&
        ArduinoSerialReader.Instance.IsConnected)
        {
            bool currentArduinoButton =
            ArduinoSerialReader.Instance.FireButton;

            // Solo detecta el momento de pulsar, no mantener presionado
            if (
            currentArduinoButton
            &&
            !previousArduinoButton)
            {
                OpenDoorInput = true;
            }

            previousArduinoButton =
            currentArduinoButton;
        }

        // Si cualquiera de los dos (teclado o Arduino) lo activa
        if (OpenDoorInput)
        {
            DoorIsOpen = !DoorIsOpen; // Cuando se presione cambiará el estado de la puerta
        }

        // Entonces se corrobora ese estado en la puerta, según el resultado se utiliará el valor guardado en las variables correspondientes
        Vector3 destinationDoor1 = DoorIsOpen ? OpenPositionDoor1 : ClosePositionDoor1;
        Vector3 destinationDoor2 = DoorIsOpen ? OpenPositionDoor2 : ClosePositionDoor2;

        // Según la información anterior, el desplazamiento de la puerta de forma horizontal (eje x), será presentado de forma agradable
        partDoor1.localPosition = Vector3.Lerp(partDoor1.localPosition, destinationDoor1, Time.deltaTime * moveVelocity); // .Lerp da un punto intermedio entre la posición inicial y la final, al usarlo da un movimiento más fluido
        // Se usa Time.deltaTime * moveVelocity ya que indica a que velocidad da esos pasos intermedios hacia la posición de destino indicado
        partDoor2.localPosition = Vector3.Lerp(partDoor2.localPosition, destinationDoor2, Time.deltaTime * moveVelocity);

        
        // SIMULACIÓN DE DESCOMPRENSIÓN EXPLOSIVA

        if (DoorIsOpen == true && suctionPoint != null) // Si la puerta está abierta y el punto de succión existe...
        {
            GameObject[] AllObjectsInSpaceship = GameObject.FindGameObjectsWithTag("FloatingObjects"); // Indica que todos los objetos que tengan el tag de FloatingObjects los tenga en cuenta (pero no necesariamente en orden)

            foreach (GameObject obj in AllObjectsInSpaceship) // Por cada objeto en la nave...
            {
                float distance = Vector3.Distance(obj.transform.position, suctionPoint.position); // Calculará la distancia entre el objeto y el agujero de la puerta, teniendo en cuenta pa posición de ambos elementos
                
                if (distance <= suctionRange) // Si la distancia entre el objeto y el agujero de la puerta está dentro del rango de succión...
                {
                    Vector3 directionToSpace = (suctionPoint.position - obj.transform.position).normalized; // Se calcula la dirección de la presión hacia el exterior de la nave (hacia la puerta abierta)
                    // Restando la posición del punto de succión con la posición del objeto

                    float intensityForce = (1f - (distance / suctionRange)) * suctionForce; // Si el objeto está más cerca del agujero de succión (la puerta abierta de la nave), la presión jala con más fuerza
                    // se divide la distancia actual del objeto con el rang de succión, esto se hace para volver la información a porcentaje decimal puro de más facil lectura (entre 0.0 y 1.0)
                    // el 1f (f de float) se refiere al 100% de la fuera disponible, por eso debe restarse al inicio
                    // al tener estos valores, se le multiplica la fuerza de succión, así el sistema sabe que porcentaje de fuerza le corresponde al objeto según su posición

                    obj.transform.Translate(directionToSpace * intensityForce * Time.deltaTime, Space.World); // Se cambia la posición de los objetos hacia el exterior de la nave
                    // el obj se reposiciona por medio de la multiplicación entre la  dirección de la presión hacia el exterior de la nave por la intensidad de esta fuerza,
                    // usando el *Time.deltaTime para que el objeto se mueva de forma suave y en tiempo real hacia Space.World
                    // (que mueve el objeto en linea recta hacia la puerta usando los ejes del mundo real sin importar la rotacion del objeto)

                    obj.transform.Rotate(Vector3.up * (intensityForce * 3f) * Time.deltaTime); // Para que la simulación se vea más real, mientras el objeto es arrastrado, el objeto rodará en el aire de forma caotica simulando como actuaria en un ambiente de presión extrema
                    // Vector3.up es el eje vertical (y) que indica que el objeto debe girar sobre sí mismo
                    // A esto se le multiplica la velocidad de giro (que se obtiene multiplicando la intensidad de la fuerza por una variable que funciona como amplificador de señal, permitiendole al sistema presentar la rotacion con la fuerza suficiente para girar de forma visible en el simulador, este valor puede variar)

                    if (obj.transform == UserCamera && distance < 1.5f) // Si el objeto que se está moviendo es la cámara y esta ubicada a menos de 1.5f de distancia del punto de succión...
                    {
                        userIsDead = true;
                        ShowDeathUI();
                    }
                }
            }
        }
        // Si apareció el canvas de muerte
        if (
        userIsDead
        &&
        DeathCanvas.activeSelf)
        {
            bool acceptInput =
            Input.GetKeyDown(
            KeyCode.Return);

            if (
            ArduinoSerialReader.Instance != null
            &&
            ArduinoSerialReader.Instance.IsConnected)
            {
                bool currentButton =
                ArduinoSerialReader.Instance.BoostButton;

                if (
                currentButton
                &&
                !previousArduinoAccept)
                {
                    acceptInput =
                    true;
                }

                previousArduinoAccept =
                currentButton;
            }

            if (
            acceptInput
            &&
            DeathAcceptButton != null)
            {
                DeathAcceptButton
                .onClick
                .Invoke();
            }
        }
    }

    void ShowDeathUI() 
    {
        if (DeathCanvas != null)
        {
            DeathCanvas.SetActive(true);
        }
    }

    // FUNCION PARA CAMBIO DE PRESIÓN
    //public void ChangePression(float valuePression)
    //{
    //    suctionForce = valuePression;

    //    if (TextPressionSlider != null)
    //    {
    //        TextPressionSlider.text = valuePression.ToString("F0"); // Muestra el número sin decimales
    //    }
    //    Debug.Log("Nueva fuerza de succión aplicada: " + suctionForce);
    //}

    //public void UpPression()
    //{
    //    suctionForce += 5f; // Sube de 5 en 5
    //    if (suctionForce > 35f) suctionForce = 35f; // Límite máximo

    //    if (TextPressionSlider != null)
    //    {
    //        TextPressionSlider.text = suctionForce.ToString("F0");
    //    }
    //}

    //public void DownPression()
    //{
    //    suctionForce -= 5f; // Baja de 5 en 5
    //    if (suctionForce < 5f) suctionForce = 5f; // Límite mínimo

    //    if (TextPressionSlider != null)
    //    {
    //        TextPressionSlider.text = suctionForce.ToString("F0");
    //    }
    //}
}