using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager :
MonoBehaviour
{
    public TMP_Text playText;

    public TMP_Text exploreText;

    public TMP_Text exitText;

    int selected;

    bool previousNavigate;

    bool previousAccept;

    ArduinoSerialReader arduino;

    void Start()
    {
        arduino =
        ArduinoSerialReader.Instance;

        UpdateVisual();
    }

    void Update()
    {
        bool navigate =
        Input.GetKeyDown(
        KeyCode.Space);

        bool accept =
        Input.GetKeyDown(
        KeyCode.Return);

        if (
        arduino != null
        &&
        arduino.IsConnected)
        {
            bool currentNavigate =
            arduino.FireButton;

            bool currentAccept =
            arduino.BoostButton;

            navigate =
            currentNavigate
            &&
            !previousNavigate;

            accept =
            currentAccept
            &&
            !previousAccept;

            previousNavigate =
            currentNavigate;

            previousAccept =
            currentAccept;
        }

        if (navigate)
        {
            selected++;

            if (selected > 2)
            {
                selected = 0;
            }

            UpdateVisual();
        }

        if (accept)
        {
            Execute();
        }
    }

    void Execute()
    {
        if (selected == 0)
        {
            SceneManager.LoadScene(
            "ProyectoNaveFinal");
        }

        else if (selected == 1)
        {
            SceneManager.LoadScene(
            "ExplorationSpaceship");
        }

        else
        {
            Application.Quit();
        }
    }

    void UpdateVisual()
    {
        playText.text =
        selected == 0
        ?
        "> Navega por el espacio"
        :
        "Navega por el espacio";

        exploreText.text =
        selected == 1
        ?
        "> Explora la nave"
        :
        "Explora la nave";

        exitText.text =
        selected == 2
        ?
        "> Salir"
        :
        "Salir";
    }
}