using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyRanged : MonoBehaviour
{
    private GameObject projectilePrefab;
    private float shootCooldown = 2f;
    private float lastShootTime;

    //protected override void Attack()
    //{
    //    if (player != null && Time.time > lastShootTime + shootCooldown)
    //    {
    //        agent.isStopped = true;
    //        Shoot();
    //        lastShootTime = Time.time;
    //    }
    //    else
    //    {
    //        agent.isStopped = false;
    //        agent.SetDestination(player.transform.position);
    //    }
    //}

    //private void Shoot()
    //{

    //    Vector3 direction = (player.transform.position - transform.position).normalized;
    //    GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    //    projectile.GetComponent<Rigidbody>().AddForce(direction * 10f, ForceMode.Impulse);
    //}

    //protected override void Patrol()
    //{
    //    if (patrolPoints.Count > 0 && !isTriggered)
    //    {
    //        if (agent.remainingDistance < 0.5f)
    //        {
    //            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
    //            agent.SetDestination(patrolPoints[currentPatrolIndex]);
    //        }
    //    }
    //}

    //protected override void Animate()
    //{
    //    animator.SetBool("IsRunning", agent.velocity.magnitude > 0.1f);
    //    animator.SetBool("Attack", isOnAttackDistance);
    //}
}