using UnityEngine;

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
    public float suctionForce = 15f;
    public float suctionRange = 10f;

    // Se guarda en privado la información de la posición inicial de la puerta (cerrada)
    private Vector3 ClosePositionDoor1;
    private Vector3 ClosePositionDoor2;

    // Se guarda en privado la información de la posición de la puerta (cerrada)
    private Vector3 OpenPositionDoor1;
    private Vector3 OpenPositionDoor2;

    private bool DoorIsOpen = false; // Inicialmente la puerta estará cerrada

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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoorIsOpen = !DoorIsOpen; // Cuando se presione barra espaciadora, cambiará el estado de la puerta
        }

        // Entonces se corrobora ese estado en la puerta, según el resultado se utiliará el valor guardado en las variables correspondientes
        Vector3 destinationDoor1 = DoorIsOpen ? OpenPositionDoor1 : ClosePositionDoor1;
        Vector3 destinationDoor2 = DoorIsOpen ? OpenPositionDoor2 : ClosePositionDoor2;

        // Según la información anterior, el desplazamiento de la puerta de forma horizontal (eje x), será presentado de forma agradable
        partDoor1.localPosition = Vector3.Lerp(partDoor1.localPosition, destinationDoor1, Time.deltaTime * moveVelocity); // .Lerp da un punto intermedio entre la posición inicial y la final, al usarlo da un movimiento más fluido
        // Se usa Time.deltaTime * moveVelocity ya que indica a que velocidad da esos pasos intermedios hacia la posición de destino indicado
        partDoor2.localPosition = Vector3.Lerp(partDoor2.localPosition, destinationDoor2, Time.deltaTime * moveVelocity);


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

                    //float intensityForce = (); // Si el objeto está más cerca del agujero de succión (la puerta abierta de la nave), la presión jala con más fuerza
                }
            }
        }
    }
}
