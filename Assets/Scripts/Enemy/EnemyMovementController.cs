using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> patrolPoints;
    public GameObject Player;
    private Vector3 randomDirection;
    private Vector3 randomPoint;
    [SerializeField]
    private LayerMask layerMask;
    private Collider[] hitColliders;
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    private EnemyStatistics enemyStatistics;
    [SerializeField]
    private int currentPatrolIndex;
    [SerializeField]
    private float distanceDetection;
    [SerializeField]
    private float randomPointRadius;
    private float distance;

    void Update()
    {
        if (enemyStatistics.Health > 0)
        {
            Patrol();
            CheckPlayerInRange();
            FindPath();
            if (enemyStatistics.IsSleep)
            {
                navMeshAgent.isStopped = true;
            }
            else
            {
                navMeshAgent.isStopped = false;
            }
        }
    }

    void Patrol()
    {
        if (patrolPoints.Count > 1 && !enemyStatistics.IsTrigered && navMeshAgent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    void CheckPlayerInRange()
    {
        hitColliders = Physics.OverlapSphere(transform.position, distanceDetection, layerMask);
        foreach (var hitCollider in hitColliders)
        {
            if (!enemyStatistics.IsTrigered && hitCollider.tag == "Player" && Vector3.Angle(gameObject.transform.forward, hitCollider.transform.position - transform.position) < 68)
            {
                enemyStatistics.IsTrigered = true;
                Player = hitCollider.gameObject;
                break;
            }
            else if (enemyStatistics.IsTrigered)
            {
                enemyStatistics.IsSleep = false;
            }
        }
    }

    void FindRandomPointNearPlayer()
    {
        if (Vector3.Distance(Player.transform.position, randomPoint) > randomPointRadius)
        {
            randomDirection = Random.insideUnitSphere * randomPointRadius;
            randomDirection += Player.transform.position;
        }
        else
        {
            return;
        }
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(randomDirection, out navMeshHit, randomPointRadius, NavMesh.AllAreas))
        {
            randomPoint = navMeshHit.position;
        }
    }

    void FindPath()
    {
        if (Player != null && enemyStatistics.IsTrigered)
        {
            distance = Vector3.Distance(transform.position, Player.transform.position);
            if (distance >= randomPointRadius)
            {
                FindRandomPointNearPlayer();
                navMeshAgent.SetDestination(randomPoint);
                navMeshAgent.isStopped = false;
            }
            else
            {
                Vector3 Direction = Player.transform.position - transform.position;
                Direction.y = 0;
                if (Direction.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(Direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
                }
                navMeshAgent.isStopped = true;
            }
        }
    }
}
