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
            lifeTime
        );
    }

    void Update()
    {
        transform.Translate(
            Vector3.forward *
            speed *
            Time.deltaTime
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy =
            other.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(
                    damage
                );
            }

            Destroy(gameObject);
        }
    }
}