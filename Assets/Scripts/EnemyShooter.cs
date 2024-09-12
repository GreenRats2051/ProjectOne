using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyShooter : MonoBehaviour
{
    [SerializeField]
    private Animator Animator;
    [SerializeField]
    private NavMeshAgent agent;
    internal static bool IsOnAttackDistance;

    private AnimatorStateInfo stateInfo;
    private Coroutine _preperForshoot;
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
            if (GetComponent<EnemyStatsShooter>().Player != null)
            {
                gameObject.transform.LookAt(GetComponent<EnemyStatsShooter>().Player.transform);
            }
            if (_preperForshoot == null)
            {
                _preperForshoot = StartCoroutine(PreperForshoot());
            }
            Animator.SetBool("Attack", true);
            agent.isStopped = true; 
        }
        else
        {
            Animator.SetBool("Attack", false);
            agent.isStopped = false; 
        }
    }
    IEnumerator PreperForshoot()
    {
        yield return new WaitForSeconds(2);
        _preperForshoot = null;
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