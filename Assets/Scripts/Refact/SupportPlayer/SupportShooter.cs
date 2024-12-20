using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SupportShooter : SupportBase
{
    [SerializeField] bool rilfe;
    [SerializeField] bool pistol;
    [SerializeField] GameObject rilfeObj;
    [SerializeField] GameObject pistolObj;
    private bool isShooting;
    private FieldOfView field;
    private Transform targetLock;
    private int maxdistance;
    private LayerMask Tm8;
    private LayerMask Enemy;




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
    private bool _dead;
    private Coroutine shootingCoroutine;

    private LineRenderer lineRenderer;
    private AnimatorStateInfo stateInfo;
    private Vector3 directionToPlayer;


    protected override void Start()
    {
        _animator = GetComponent<Animator>();
        base.Start();
        if (rilfe&&pistol)
        {
            return;
        }
        else
        {
            _animator.SetBool("Pistol", pistol);
            _animator.SetBool("Rifle", rilfe);
            rilfeObj.SetActive(rilfe);
            pistolObj.SetActive(pistol);
        }
        field = GetComponentInChildren<FieldOfView>();
        InitializeLineRenderer();
    }
    protected override void Update()
    {

        base.Update();
        if (isShooting && field.VisibleEnemies.Count > 0)
        {
            targetLock = field.VisibleEnemies[0];
            foreach (var enemy in field.VisibleEnemies)
            {
                if (Vector3.Distance(enemy.position, gameObject.transform.position) < Vector3.Distance(targetLock.position, gameObject.transform.position))
                {
                    targetLock = enemy;
                }
            }
            transform.LookAt(targetLock);
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, transform.forward, maxdistance, Tm8) &&
            Physics.Raycast(transform.position, transform.forward, out hit, maxdistance, Enemy))
            {
                Shoot();
            }
        }
    }
  

    private void InitializeLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    protected void AttackPlayer()
    {
        if (targetLock == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, targetLock.transform.position);
        isOnAttackDistance = distanceToTarget <= maxdistance;

        _agent.isStopped = isOnAttackDistance;
        if (isOnAttackDistance)
        {
            FaceTarget();
            PrepareShooting();
        }
    }

    private void FaceTarget()
    {
        directionToPlayer = (targetLock.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void PrepareShooting()
    {
        if (isShooting) return;

        float distanceToPlayer = Vector3.Distance(bulletStartPoint.transform.position, targetLock.transform.position);
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



    protected  void Animate()
    {
        UpdateAnimatorStates();
        ManageLineRenderer();
    }

    private void UpdateAnimatorStates()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        animator.SetTrigger("Dead");
        animator.SetBool("isMove", _agent.velocity.magnitude > 0.1f);
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
