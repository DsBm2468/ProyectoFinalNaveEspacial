using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public Vector2 xRange;

    public Vector2 yRange;

    public int minEnemies = 10;

    public int maxEnemies = 25;

    void Start()
    {
        SpawnWave();
    }

    void SpawnWave()
    {
        int amount =
        Random.Range(
        minEnemies,
        maxEnemies + 1);

        for (
        int i = 0;
        i < amount;
        i++)
        {
            Vector3 pos =
            new Vector3(
            Random.Range(
            xRange.x,
            xRange.y),

            Random.Range(
            yRange.x,
            yRange.y),

            Random.Range(
            40,
            120));

            Instantiate(
            enemyPrefab,
            pos,
            Quaternion.identity);

            GameManager.Instance
            .EnemySpawned();
        }
    }
}