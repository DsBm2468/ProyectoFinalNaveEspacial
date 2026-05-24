using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    public GameObject panel;

    public TMP_Text retryText;

    public TMP_Text exitText;

    int selected;

    bool gameOver;

    bool previousFire;

    bool previousBoost;

    ArduinoSerialReader arduino;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        arduino =
        ArduinoSerialReader.Instance;

        if (panel != null)
        {
            panel.SetActive(false);
        }

        UpdateVisual();
    }

    void Update()
    {
        if (!gameOver)
            return;

        bool move =
        Input.GetKeyDown(
        KeyCode.Space);

        bool accept =
        Input.GetKeyDown(
        KeyCode.LeftShift);

        if (
        arduino != null
        &&
        arduino.IsConnected)
        {
            bool currentFire =
            arduino.FireButton;

            bool currentBoost = arduino.BoostButton; // Asegúrate de que el nombre del botón sea correcto

            move = currentFire // Botón de disparo para navegar
            &&
            !previousFire;

            accept = currentBoost // Botón de boost para aceptar
            &&
            !previousBoost;

            previousFire =
            currentFire;

            previousBoost =
            currentBoost;
        }

        if (move)
        {
            selected =
            (selected + 1)
            % 2;

            UpdateVisual();
        }

        if (accept)
        {
            Execute();
        }
    }

    void Execute()
    {
        Time.timeScale = 1;

        if (selected == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            SceneManager.LoadScene(
            "PantallaInicio");
        }
    }

    void UpdateVisual()
    {
        if (
        retryText == null
        ||
        exitText == null)
            return;

        retryText.text =
        selected == 0
        ?
        "> REINTENTAR"
        :
        "REINTENTAR";

        exitText.text =
        selected == 1
        ?
        "> SALIR"
        :
        "SALIR";
    }

    public void Show()
    {
        gameOver =
        true;

        Time.timeScale =
        0;

        if (panel != null)
        {
            panel.SetActive(
            true);
        }

        selected =
        0;

        UpdateVisual();
    }
}