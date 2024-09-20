using System.Collections.Generic;
using System.Collections;

using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(LineRenderer))]
public class EnemyRange : EnemyBase
{
    [Header("Range Settings")]
    [SerializeField]
    private int RangeDistance;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject bulletStartPoint;
    [SerializeField] private List<Vector3> patrolPoints = new List<Vector3>();
    [SerializeField] private Animator animator;
    [SerializeField]
    private float speed = 10;
    private int currentPatrolIndex = -1;
    private bool isOnAttackDistance;
    private bool _isShooting;
    AnimatorStateInfo stateInfo;
    LineRenderer Line;
    Vector3 directionToPlayerN;
    Coroutine _coroutine;

    protected override void AttackPlayer()
    {
        if (player != null)
        {
            if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= RangeDistance)
            {
                isOnAttackDistance = true;
                gameObject.transform.LookAt(player.transform.position);
                
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
        animator.SetBool("Dead", _dead);
        if ((isOnAttackDistance|| _isShooting)&&!_dead)
        {
            animator.SetBool("Dead", _dead);
            if (Line == null)
            {
                Line = GetComponent<LineRenderer>();
                Line.startWidth = 0.1f; 
                Line.endWidth = 0.1f;  
                Line.positionCount = 2; 
            }
            if (stateInfo.IsName("Dead"))
            {
                animator.SetBool("Dead", false);
            }

            directionToPlayerN = (player.transform.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(bulletStartPoint.transform.position, player.transform.position);
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayerN); 
            float rotationSpeed = 0.1f;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            RaycastHit hit;
            if (Physics.Raycast(bulletStartPoint.transform.position, directionToPlayerN, out hit, distanceToPlayer))
            {
                Line.SetPosition(0, bulletStartPoint.transform.position); 
                Line.SetPosition(1, hit.point);
                if(_coroutine == null)
                {
                    _coroutine = StartCoroutine(ShootPrepare());
                }

            }
            Line.enabled = true;
        }
        else
        {
            if (Line != null)
            {
                Line.enabled = false;
            }
        }
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        animator.SetBool("IsRunning", agent.velocity.magnitude > 0.1f);
        //animator.SetBool("Attack", isOnAttackDistance);
        animator.SetBool("Dead", _dead);
        if (stateInfo.IsName("Melee"))
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

    }
    void Shoot()
    {
       GameObject instBullet = Instantiate(bullet, bulletStartPoint.transform.position, gameObject.transform.rotation);
        instBullet.GetComponent<Rigidbody>().AddForce(directionToPlayerN*speed,ForceMode.Impulse);
    }

    protected override void Start()
    {
        base.Start();
    }
    IEnumerator ShootPrepare()
    {

            yield return new WaitForSeconds(2);
            Shoot();
            _coroutine = null;
        
    }
}