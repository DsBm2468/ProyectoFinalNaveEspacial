using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool Paused;
    private bool previousPause;

    ArduinoSerialReader arduino;
    void Start()
    {
        arduino =
        ArduinoSerialReader.Instance;
    }

    void Update()
    {
      
    }
}
