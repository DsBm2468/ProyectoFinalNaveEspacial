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

        FindUI();

        if (
        pausePanel != null)
        {
            pausePanel.SetActive(
            false);
        }

        UpdateVisual();
    }

    void FindUI()
    {
        if (
        pausePanel == null)
        {
            pausePanel =
            GameObject.Find(
            "PausePanel");
        }

        if (
        continueText == null)
        {
            continueText =
            GameObject.Find(
            "ContinueText")
            ?.GetComponent<
            TMP_Text>();
        }

        if (
        exitText == null)
        {
            exitText =
            GameObject.Find(
            "ExitText")
            ?.GetComponent<
            TMP_Text>();
        }
    }

    void Update()
    {
        if (
        pausePanel == null
        ||
        continueText == null
        ||
        exitText == null)
        {
            FindUI();
        }

        HandlePause();

        if (
        !paused)
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

        if (
        pauseInput)
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

        if (
        navigate)
        {
            selectedOption =
            (selectedOption + 1)
            % 2;

            UpdateVisual();
        }

        if (
        accept)
        {
            ExecuteOption();
        }
    }

    void ExecuteOption()
    {
        if (
        selectedOption == 0)
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

        if (
        pausePanel != null)
        {
            pausePanel.SetActive(
            paused);
        }

        selectedOption =
        0;

        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (
        continueText == null
        ||
        exitText == null)
            return;

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