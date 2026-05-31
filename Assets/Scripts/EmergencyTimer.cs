using System;
using UnityEngine;

public class EmergencyTimer : MonoBehaviour
{
    public float timeToReturnToPilotArea = 30f; // Indica el tiempo en que nuevamente se diriga al área de pilotaje
    public bool emergencyIsOver = false; // Indica si la emergencia ya ha pasado o no, inicialmente es false
    private bool previousArduinoAccept = false;

    [Header("Elementos del canvas para mostrar la alerta")]
    public GameObject TextAlert1;
    public GameObject TextAlert2;
    public GameObject ButtonReturnPilotScene;

    private void Start()
    {
        if (TextAlert1 != null) TextAlert1.SetActive(false); // Hace que los elementos del canvasAlert no aparezcan inicialmente sino cuando pase el tiempo indicado
        if (TextAlert2 != null) TextAlert2.SetActive(false);
        if (ButtonReturnPilotScene != null) ButtonReturnPilotScene.SetActive(false);
    }
    void Update()
    {
        if (emergencyIsOver == false) // Si la emergencia no ha pasado...
        {
            timeToReturnToPilotArea = timeToReturnToPilotArea - Time.deltaTime; // Se va restando el tiempo que transcurre el usuario en la nave desde que ingresó a la escena
            Debug.Log("Tiempo restante: " + timeToReturnToPilotArea);

            if (timeToReturnToPilotArea <= 0) // El tiempo para volver al area de pilotaje acabó...
            {
                emergencyIsOver = true; // Se indica que la emergencia ya paso
                TurnOnAlert(); // Además, se da la indicación para mostrar en pantalla la alerta
            }
        }

        // Si la alerta ya apareció
        if (
        emergencyIsOver
        &&
        ButtonReturnPilotScene.activeSelf)
        {
            bool acceptInput =
            Input.GetKeyDown(
            KeyCode.Return); // Enter también sirve

            // Si Arduino existe
            if (
            ArduinoSerialReader.Instance != null
            &&
            ArduinoSerialReader.Instance.IsConnected)
            {
                bool currentButton =
                ArduinoSerialReader.Instance.BoostButton;

                // Detecta una sola pulsación
                if (
                currentButton
                &&
                !previousArduinoAccept)
                {
                    acceptInput = true;
                }

                previousArduinoAccept =
                currentButton;
            }

            if (acceptInput)
            {
                ButtonReturnPilotScene
                .GetComponent<UnityEngine.UI.Button>()
                .onClick
                .Invoke();
            }
        }
    }

    void TurnOnAlert()
    {
        Debug.LogWarning("¡ALERTA!");
        TextAlert1.SetActive(true); // Aparecen los elementos del CanvasAlert en pantalla
        TextAlert2.SetActive(true);
        ButtonReturnPilotScene.SetActive(true);

    }
}
