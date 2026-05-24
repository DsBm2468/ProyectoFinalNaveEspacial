using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 30f;

    public float inheritedSpeed;

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
        transform.position +=
        transform.forward *
        (speed + inheritedSpeed) *
        Time.deltaTime;
    }

    void OnTriggerEnter(
    Collider other)
    {
        EnemyHealth enemy =
        other.GetComponentInParent<
        EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(
            damage);

            Destroy(
            gameObject);
        }
    }
}