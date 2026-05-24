using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
    public GameObject healthPrefab;

    public int amount = 3;

    public Vector2 xRange;

    public Vector2 yRange;

    public float zMin = 40;

    public float zMax = 150;

    void Start()
    {
        SpawnHealth();
    }

    void SpawnHealth()
    {
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
            zMin,
            zMax));

            Instantiate(
            healthPrefab,
            pos,
            Quaternion.identity);
        }
    }
}