using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;

    public int amount = 4;

    public Vector2 xRange;

    public Vector2 yRange;

    public float zMin = 40;

    public float zMax = 200;

    void Start()
    {
        SpawnMeteorites();
    }

    void SpawnMeteorites()
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
            meteorPrefab,
            pos,
            Quaternion.identity);
        }
    }
}
