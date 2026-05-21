using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float spawnRate = 3f;

    public int maxEnemies = 2;

    public Vector2 xRange;

    public Vector2 yRange;

    private int currentEnemies;

    void Start()
    {
        InvokeRepeating(
            "SpawnEnemy",
            1f,
            spawnRate
        );
    }

    void SpawnEnemy()
    {
        if (currentEnemies >= maxEnemies)
            return;

        Vector3 spawnPos =
        new Vector3(
            Random.Range(xRange.x, xRange.y),
            Random.Range(yRange.x, yRange.y),
            50f
        );

        Instantiate(
            enemyPrefab,
            spawnPos,
            Quaternion.identity
        );

        currentEnemies++;
    }
}