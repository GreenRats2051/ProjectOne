using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public GameObject Player;
    private Vector3 RandomDirection;
    private Vector3 RandomPoint;
    [SerializeField]
    private LayerMask LayerMask;
    private Collider[] HitColliders;
    [SerializeField]
    private NavMeshAgent NavMeshAgent;
    [SerializeField]
    private List<Transform> PatrolPoints = new List<Transform>();
    public int Health;
    [SerializeField]
    private int CurrentPatrolIndex;
    [SerializeField]
    private float DistanceDetection;
    [SerializeField]
    private float RandomPointRadius;
    private float Distance;
    [SerializeField]
    private bool IsTrigered;
    [SerializeField]
    private bool IsSleep;

    void Start()
    {

    }

    void Update()
    {
        if (Health >= 0)
        {
            Patrol();
            CheckPlayerInRange();
            FindPath();
            if (IsSleep)
            {
                NavMeshAgent.isStopped = true;
            }
            else
            {
                NavMeshAgent.isStopped = false;
            }
        }
    }

    void Patrol()
    {
        if (PatrolPoints.Count > 1 && !IsTrigered && NavMeshAgent.remainingDistance < 0.5f)
        {
            CurrentPatrolIndex = (CurrentPatrolIndex + 1) % PatrolPoints.Count;
            NavMeshAgent.SetDestination(PatrolPoints[CurrentPatrolIndex].position);
        }
    }
    void CheckPlayerInRange()
    {
        HitColliders = Physics.OverlapSphere(transform.position, DistanceDetection, LayerMask);
        foreach (var hitCollider in HitColliders)
        {
            if (!IsTrigered && hitCollider.tag == "Player" && Vector3.Angle(gameObject.transform.forward, hitCollider.transform.position - transform.position) < 90)
            {
                IsTrigered = true;
                Player = hitCollider.gameObject;
                break;
            }
            else if (IsTrigered)
            {
                if (hitCollider.TryGetComponent<EnemyMeleeController>(out EnemyMeleeController enemyMelee))
                {
                    IsSleep = false;

                }
                if (hitCollider.TryGetComponent<EnemyGunController>(out EnemyGunController enemyRange))
                {
                    IsSleep = false;
                }
            }
        }
    }

    void FindRandomPointNearPlayer()
    {

        if (Vector3.Distance(Player.transform.position, RandomPoint) > RandomPointRadius)
        {
            RandomDirection = Random.insideUnitSphere * RandomPointRadius;
            RandomDirection += Player.transform.position;
        }
        else
        {
            return;
        }
        NavMeshHit NavMeshHit;
        if (NavMesh.SamplePosition(RandomDirection, out NavMeshHit, RandomPointRadius, NavMesh.AllAreas))
        {
            RandomPoint = NavMeshHit.position;
        }
    }

    void FindPath()
    {
        if (Player != null && IsTrigered)
        {
            Distance = Vector3.Distance(transform.position, Player.transform.position);
            if (Distance >= RandomPointRadius)
            {
                FindRandomPointNearPlayer();
                NavMeshAgent.SetDestination(RandomPoint);
                NavMeshAgent.isStopped = false;
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
                NavMeshAgent.isStopped = true;
            }
        }
    }

    public void GetHit(int Damage)
    {
        Health -= Damage;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
