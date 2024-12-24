using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class EnemyRange : EnemyBase
{
    [Header("Range Settings")]
    [SerializeField]
    private int rangeDistance;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject bulletStartPoint;
    [SerializeField]
    private List<Vector3> patrolPoints = new List<Vector3>();
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float bulletSpeed = 10f;

    private int currentPatrolIndex = -1;
    private bool isOnAttackDistance;
    private bool isShooting;
    private Coroutine shootingCoroutine;

    private LineRenderer lineRenderer;
    private AnimatorStateInfo stateInfo;
    private Vector3 directionToPlayer;

    protected override void Start()
    {
        base.Start();
        InitializeLineRenderer();
    }

    private void InitializeLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    protected override void AttackPlayer()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        isOnAttackDistance = distanceToTarget <= rangeDistance;

        agent.isStopped = isOnAttackDistance;
        if (isOnAttackDistance)
        {
            FaceTarget();
            PrepareShooting();
        }
    }

    private void FaceTarget()
    {
        directionToPlayer = (target.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void PrepareShooting()
    {
        if (isShooting) return;

        float distanceToPlayer = Vector3.Distance(bulletStartPoint.transform.position, target.transform.position);
        RaycastHit hit;

        if (Physics.Raycast(bulletStartPoint.transform.position, directionToPlayer, out hit, distanceToPlayer))
        {
            lineRenderer.SetPosition(0, bulletStartPoint.transform.position);
            lineRenderer.SetPosition(1, hit.point);
            lineRenderer.enabled = true;

            if (shootingCoroutine == null)
                shootingCoroutine = StartCoroutine(ShootingRoutine());
        }
    }

    private IEnumerator ShootingRoutine()
    {
        isShooting = true;
        yield return new WaitForSeconds(2);
        Shoot();
        isShooting = false;
        shootingCoroutine = null;
    }

    private void Shoot()
    {
        GameObject instantiatedBullet = Instantiate(bullet, bulletStartPoint.transform.position, transform.rotation);
        Rigidbody bulletRigidbody = instantiatedBullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(directionToPlayer * bulletSpeed, ForceMode.Impulse);
    }

    protected override void Patrol()
    {
        if (patrolPoints.Count <= 1 || isOnAttackDistance || _isTrigered) return;

        if (agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            agent.SetDestination(patrolPoints[currentPatrolIndex]);
        }
    }

    protected override void Animate()
    {
        UpdateAnimatorStates();
        ManageLineRenderer();
    }

    private void UpdateAnimatorStates()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        animator.SetBool("Dead", _dead);
        animator.SetBool("IsRunning", agent.velocity.magnitude > 0.1f);

        if (stateInfo.IsName("Melee"))
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        else
        {
            agent.isStopped = false;
        }
    }

    private void ManageLineRenderer()
    {
        if (isOnAttackDistance && !isShooting && !_dead)
        {
            if (shootingCoroutine == null)
                lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
