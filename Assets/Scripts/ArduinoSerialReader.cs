using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class ArduinoSerialReader : MonoBehaviour
{
    public static ArduinoSerialReader Instance
    {
        get;
        private set;
    }

    [Header("Serial Configuration")]

    [SerializeField]
    private string portName = "COM5";

    [SerializeField]
    private int baudRate = 9600;

    [SerializeField]
    private int readTimeoutMs = 100;

    [SerializeField]
    private bool autoConnectOnStart = true;

    public int RawHorizontal
    {
        get;
        private set;
    }

    public int RawVertical
    {
        get;
        private set;
    }

    public bool FireButton
    {
        get;
        private set;
    }

    public bool BoostButton
    {
        get;
        private set;
    }

    public bool PauseButton
    {
        get;
        private set;
    }

    public bool IsConnected
    {
        get;
        private set;
    }

    private SerialPort serialPort;

    private Thread readThread;

    private volatile bool running;

    private readonly object dataLock =
    new object();

    void Awake()
    {
        if (
        Instance != null
        &&
        Instance != this)
        {
            Destroy(gameObject);

            return;
        }

        Instance = this;

        DontDestroyOnLoad(
        gameObject);
    }

    void Start()
    {
        if (autoConnectOnStart)
        {
            Connect();
        }
    }

    public void Connect()
    {
        if (IsConnected)
        {
            return;
        }

        try
        {
            serialPort =
            new SerialPort(
            portName,
            baudRate);

            serialPort.ReadTimeout =
            readTimeoutMs;

            serialPort.NewLine =
            "\n";

            serialPort.Open();

            running = true;

            readThread =
            new Thread(ReadLoop);

            readThread.IsBackground =
            true;

            readThread.Start();

            IsConnected = true;

            Debug.Log(
            "Arduino conectado");
        }
        catch (System.Exception e)
        {
            Debug.LogError(
            e.Message);
        }
    }

    void ReadLoop()
    {
        while (running)
        {
            try
            {
                string line =
                serialPort.ReadLine();

                ParseLine(line);
            }
            catch (System.TimeoutException)
            {

            }
            catch (System.Exception e)
            {
                Debug.LogWarning(
                e.Message);
            }
        }
    }

    void ParseLine(string line)
    {
        if (
        string.IsNullOrWhiteSpace(
        line))
        {
            return;
        }

        string[] parts =
        line.Trim().Split(',');

        if (parts.Length != 5)
        {
            return;
        }

        if (
        int.TryParse(
        parts[0],
        out int horizontal)

        &&

        int.TryParse(
        parts[1],
        out int vertical)

        &&

        int.TryParse(
        parts[2],
        out int fire)

        &&

        int.TryParse(
        parts[3],
        out int boost)

        &&

        int.TryParse(
        parts[4],
        out int pause))
        {
            lock (dataLock)
            {
                RawHorizontal =
                horizontal;

                RawVertical =
                vertical;

                FireButton =
                fire == 1;

                BoostButton =
                boost == 1;

                PauseButton =
                pause == 1;
            }
        }
    }

    public void GetNormalizedValues(
    out float horizontal,
    out float vertical)
    {
        lock (dataLock)
        {
            horizontal =
            (RawHorizontal - 512)
            / 512f;

            vertical =
            (RawVertical - 512)
            / 512f;
        }

        horizontal =
        Mathf.Clamp(
        horizontal,
        -1f,
        1f);

        vertical =
        Mathf.Clamp(
        vertical,
        -1f,
        1f);
    }

    public void Disconnect()
    {
        running = false;

        if (
        readThread != null
        &&
        readThread.IsAlive)
        {
            readThread.Join(
            500);
        }

        if (
        serialPort != null
        &&
        serialPort.IsOpen)
        {
            serialPort.Close();
        }

        IsConnected = false;
    }

    void OnApplicationQuit()
    {
        Disconnect();
    }

    void OnDestroy()
    {
        Disconnect();
    }
}