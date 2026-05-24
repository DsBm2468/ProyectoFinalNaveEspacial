using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 30f;

    public float lifeTime = 3f;

    public int damage = 1;

    void Start()
    {
        Destroy(
        gameObject,
        lifeTime);
    }

    void Update()
    {
        transform.Translate(
        Vector3.forward *
        speed *
        Time.deltaTime);
    }

    void OnTriggerEnter(
Collider other)
    {
        Debug.Log(
        "Golpee: "
        + other.name);

        EnemyHealth enemy =
        other.GetComponentInParent<
        EnemyHealth>();

        if (enemy != null)
        {
            Debug.Log(
            "Aqui ENEMYHEALTH");

            enemy.TakeDamage(
            damage);

            Destroy(
            gameObject);
        }
        else
        {
            Debug.Log(
            "NO ENCONTRE ENEMYHEALTH");
        }
    }
}