using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 10f;

    public float rotationSpeed = 3f;

    public float detectionDistance = 40f;

    public float avoidDistance = 12f;

    public float patrolRadius = 20f;

    public float patrolChangeTime = 4f;

    private Transform player;

    private Vector3 patrolTarget;

    private float patrolTimer;

    private bool chasingPlayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        PickRandomPoint();
    }

    void Update()
    {
        if (player == null)
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        chasingPlayer =
            distance <= detectionDistance;

        if (chasingPlayer)
        {
            ChasePlayer(distance);
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolChangeTime)
        {
            patrolTimer = 0;

            PickRandomPoint();
        }

        MoveTowards(patrolTarget);
    }

    void PickRandomPoint()
    {
        patrolTarget =
            transform.position +
            Random.insideUnitSphere *
            patrolRadius;
    }

    void ChasePlayer(float distance)
    {
        Vector3 target;

        if (distance < avoidDistance)
        {
            target =
                transform.position -
                (player.position -
                transform.position)
                .normalized * 15f;
        }
        else
        {
            target = player.position;
        }

        MoveTowards(target);
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 direction =
            (target -
            transform.position)
            .normalized;

        Quaternion lookRotation =
            Quaternion.LookRotation(direction);

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                rotationSpeed *
                Time.deltaTime
            );

        transform.position +=
            transform.forward *
            moveSpeed *
            Time.deltaTime;
    }
}