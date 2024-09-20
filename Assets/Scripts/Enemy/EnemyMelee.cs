using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : EnemyBase
{
    [Header("Melee Settings")]
    [SerializeField]
    private int meleeDistance;
    [SerializeField] private List<Vector3> patrolPoints = new List<Vector3>(); 
    [SerializeField] private Animator animator; 
    private int currentPatrolIndex = -1;
    private bool isOnAttackDistance;
    AnimatorStateInfo stateInfo;
    protected override void AttackPlayer()
    {
        if (player != null)
        {
            if ( Vector3.Distance(gameObject.transform.position,player.transform.position) <= meleeDistance)
            {
                isOnAttackDistance = true;
                agent.isStopped = true;
            }
            else
            {
                isOnAttackDistance = false;
                agent.isStopped = false;
            }
        }
    }


    protected override void Patrol()
    {
        if (patrolPoints.Count > 1 && !_isTrigered)
        {
            if (agent.remainingDistance < 0.5f)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
                agent.SetDestination(patrolPoints[currentPatrolIndex]);
            }
        }
    }


    protected override void Animate()
    {

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);


        if (!stateInfo.IsName("Dead"))
        {
            animator.SetBool("IsRunning", agent.velocity.magnitude > 0.1f);
            animator.SetBool("Attack", isOnAttackDistance);
            animator.SetBool("Sleep", _isSleep);
        }
        animator.SetBool("Dead", _dead);
        if (stateInfo.IsName("Melee")|| stateInfo.IsName("Standing To Crouched"))
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
        if (stateInfo.IsName("Dead"))
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
    }


    protected override void Start()
    {
        base.Start();
    }
}