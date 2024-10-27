using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeController : MonoBehaviour
{
    [SerializeField]
    private LayerMask PlayerMask;
    [SerializeField]
    private Transform AttackZone;
    [SerializeField]
    private Vector3 SizeAttackZone;
    private Collider[] PlayerCollider;
    [SerializeField]
    private NavMeshAgent NavMeshAgent;
    private bool isOnAttackDistance;

    void Start()
    {

    }

    void Update()
    {
        AttackPlayer();
    }

    void AttackPlayer()
    {
        PlayerCollider = Physics.OverlapBox(AttackZone.position, SizeAttackZone, Quaternion.identity, PlayerMask);
        if (PlayerCollider.Length != 0)
        {
            isOnAttackDistance = true;
            NavMeshAgent.isStopped = true;
        }
        else
        {
            isOnAttackDistance = false;
            NavMeshAgent.isStopped = false;
        }
    }
}
