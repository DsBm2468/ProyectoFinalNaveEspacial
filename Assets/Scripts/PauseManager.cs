using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    bool paused;

    bool previousPause;

    bool previousNavigate;

    bool previousAccept;

    int selectedOption;

    ArduinoSerialReader arduino;

    public GameObject pausePanel;

    public TMP_Text continueText;

    public TMP_Text exitText;

    void Start()
    {
        arduino =
        ArduinoSerialReader.Instance;

        pausePanel.SetActive(
        false);

        UpdateVisual();
    }

    void Update()
    {
        HandlePause();

        if (!paused)
            return;

        HandleMenuInput();
    }

    void HandlePause()
    {
        bool pauseInput =
        Input.GetKeyDown(
        KeyCode.Escape);

        if (
        arduino != null
        &&
        arduino.IsConnected)
        {
            bool currentPause =
            arduino.PauseButton;

            if (
            currentPause
            &&
            !previousPause)
            {
                pauseInput =
                true;
            }

            previousPause =
            currentPause;
        }

        if (pauseInput)
        {
            TogglePause();
        }
    }

    void HandleMenuInput()
    {
        bool navigate = 
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
            bool currentNavigate =
            arduino.FireButton; // Asumiendo que el botón de disparo se usa para navegar

            bool currentAccept =
            arduino.BoostButton; // Asumiendo que el botón de boost se usa para aceptar

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
            selectedOption++;

            if (selectedOption > 1)
            {
                selectedOption = 0;
            }

            UpdateVisual();
        }

        if (accept)
        {
            ExecuteOption();
        }
    }

    void ExecuteOption()
    {
        if (selectedOption == 0)
        {
            TogglePause();
        }

        else
        {
            Time.timeScale =
            1;

            SceneManager.LoadScene(
            "MainScreen");
        }
    }

    void TogglePause()
    {
        paused =
        !paused;

        Time.timeScale =
        paused
        ?
        0
        :
        1;

        pausePanel.SetActive(
        paused);

        selectedOption =
        0;

        UpdateVisual();
    }

    void UpdateVisual()
    {
        continueText.text =
        selectedOption == 0
        ?
        "> CONTINUAR"
        :
        "CONTINUAR";

        exitText.text =
        selectedOption == 1
        ?
        "> SALIR"
        :
        "SALIR";
    }
}