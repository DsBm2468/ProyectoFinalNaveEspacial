using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int enemiesAlive;

    public int enemiesKilled;

    public string nextScene;

    public string returnScene;

    public PlayerHealth playerHealth;



    void Awake()
    {
        if (
        Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(
            gameObject);
        }
        else
        {
            Destroy(
            gameObject);
        }
    }

    public void EnemySpawned()
    {
        enemiesAlive++;
    }

    public void EnemyKilled()
    {
        enemiesAlive--;

        enemiesKilled++;

        if (
        enemiesAlive <= 0)
        {
            Invoke(
            nameof(
            SectorCleared),
            2f);
        }
    }

    public GameObject clearMessage;

    void SectorCleared()
    {
        clearMessage
        .SetActive(true);

        Invoke(
        nameof(
        ChangeScene),
        3f);
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(
        "ExplorationSpaceship");
    }

    public void ResetPlayer()
    {
        if (
        playerHealth != null)
        {
            playerHealth.ResetHealth();
        }
    }
}