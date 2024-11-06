using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeController : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerMask;
    [SerializeField]
    private Transform attackZone;
    [SerializeField]
    private Vector3 sizeAttackZone;
    private Collider[] playerCollider;
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    void Update()
    {
        playerCollider = Physics.OverlapBox(attackZone.position, sizeAttackZone, Quaternion.identity, playerMask);
        if (playerCollider.Length != 0)
        {
            navMeshAgent.isStopped = true;
        }
        else
        {
            navMeshAgent.isStopped = false;
        }
    }
}
