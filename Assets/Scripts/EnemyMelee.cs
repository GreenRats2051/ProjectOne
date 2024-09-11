using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMelee : MonoBehaviour
{
    [SerializeField]
    private Animator Animator;
    [SerializeField]
    private NavMeshAgent agent;
    internal static bool IsOnAttackDistance;

    private AnimatorStateInfo stateInfo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // удар
        }
    }

    void Update()
    {
        stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        if (agent.velocity.magnitude > 0.1f)
        {
            Animator.SetBool("IsRunning", true);
        }
        else
        {
            Animator.SetBool("IsRunning", false);
        }

        if (IsOnAttackDistance)
        {
            Animator.SetBool("Attack", true);
            agent.isStopped = true; 
        }
        else
        {
            Animator.SetBool("Attack", false);
            agent.isStopped = false; 
        }
    }

    private void LateUpdate()
    {
        if (stateInfo.IsName("Melee"))
        {
            agent.isStopped = true; 
        }
        else
        {
            agent.isStopped = false;
        }
    }
}