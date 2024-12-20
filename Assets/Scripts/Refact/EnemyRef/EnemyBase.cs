using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyBase : MonoBehaviour
{
    public Action GoToSleep;
    public int Power => _power;

    [Header("Base Settings")]
    [SerializeField] protected int attackRadius = 10;
    [SerializeField] protected int _power = 1;
    [SerializeField] protected int Health = 3;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected bool _isTriggered = false;
    [SerializeField] protected bool _isSleep = false;
    [SerializeField] protected bool _isDead = false;
    [SerializeField] protected LayerMask layer;
    [SerializeField] private float stoppingDistance = 1.5f;

    private List<GameObject> _players = new List<GameObject>();
    private List<GameObject> _supports = new List<GameObject>();
    protected GameObject target;
    private Collider[] hitColliders;

    protected abstract void AttackPlayer();
    protected abstract void Patrol();
    protected abstract void Animate();

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance; // Настраиваем остановку перед целью
        _players = ListPlayer.Inst._players;
        _supports = ListSupport.Inst._supports;
    }

    protected void Update()
    {
        Animate();

        if (_isDead)
        {
            agent.isStopped = true;
            return;
        }

        if (_isSleep)
        {
            agent.isStopped = true;
            FindPlayerInRange();
        }
        else
        {
            agent.isStopped = false;
            Patrol();

            if (_isTriggered)
            {
                FollowTarget();
            }
            else
            {
                FindPlayerInRange();
            }
        }
    }

    private void FindPlayerInRange()
    {
        _isTriggered = false;
        target = null;

        foreach (var player in _players)
        {
            if (IsPlayerInRange(player))
            {
                target = player;
                _isTriggered = true;
                _isSleep = false;

                // Активируем других врагов в радиусе
                ActivateNearbyEnemies();
                return;
            }
        }
    }

    private bool IsPlayerInRange(GameObject player)
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        Vector3 directionToPlayer = player.transform.position - transform.position;

        return distance < attackRadius && Vector3.Angle(transform.forward, directionToPlayer) < 90;
    }

    private void FollowTarget()
    {
        if (target == null || Vector3.Distance(target.transform.position, transform.position) > attackRadius)
        {
            _isTriggered = false;
            return;
        }

        if (Vector3.Distance(transform.position, target.transform.position) > agent.stoppingDistance)
        {
            agent.SetDestination(target.transform.position);
        }
    }

    private void ActivateNearbyEnemies()
    {
        hitColliders = Physics.OverlapSphere(transform.position, attackRadius, layer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out EnemyBase enemy) && enemy != this)
            {
                enemy._isSleep = false;
            }
        }
    }

    public void GetHit(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            _isDead = true;
            agent.isStopped = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        if (agent != null && agent.hasPath)
        {
            Gizmos.color = Color.green;
            NavMeshPath path = agent.path;
            Vector3 previousCorner = transform.position;

            foreach (Vector3 corner in path.corners)
            {
                Gizmos.DrawLine(previousCorner, corner);
                previousCorner = corner;
            }
        }
    }
}
