using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
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
    [SerializeField]
    private int damage;
    private float timeAttack;
    [SerializeField]
    private float timeNextAttack;

    void Update()
    {
        playerCollider = Physics.OverlapBox(attackZone.position, sizeAttackZone, Quaternion.identity, playerMask);
        if (timeAttack < timeNextAttack)
        {
            timeAttack += Time.deltaTime;
        }
        if (playerCollider.Length != 0 && timeAttack >= timeNextAttack)
        {
            Debug.Log("Enemy attack");
            navMeshAgent.isStopped = true;
            for (int i = 0; i < playerCollider.Length; i++)
            {
                playerCollider[i].GetComponent<PlayerStatistics>().GetHit(damage);
            }
            timeAttack = 0;
        }
        else
        {
            navMeshAgent.isStopped = false;
        }
    }
}
