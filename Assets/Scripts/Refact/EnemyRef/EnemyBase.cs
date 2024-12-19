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
    [SerializeField] protected int attackRadius; 
    [SerializeField] protected int _power = 1; 
    [SerializeField] protected int Health=3; 
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected bool _isTrigered= false;
    [SerializeField] protected bool  _isSleep= false;
    [SerializeField] protected bool _dead= false;
    [SerializeField] protected LayerMask layer ;
    [SerializeField] private float minDistanceToTarget = 1.5f;

    private List<GameObject> _players = new List<GameObject>();
    private List<GameObject> supports = new List<GameObject>();
    protected GameObject target ;
    private AIController _agent;
    private Collider[] hitColliders;
    protected abstract void AttackPlayer(); 
    protected abstract void Patrol();  
    protected abstract void Animate(); 

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _agent = new();
        _players = ListPlayer.Inst._players;
        supports = ListSupport.Inst._supports;
    }
    protected void Update()
    {
        Animate();
        if (!_dead)
        {
            if (_isSleep)
            {
                agent.isStopped = true;
                FindPlayerInRange();
            }
            else
            {
                agent.isStopped = false;
                Patrol();
                AttackPlayer();
                if (_isTrigered)
                {
                    CheckPlayerInRange();
                }
                else if (!_isTrigered)
                {
                    FindPlayerInRange();
                }
            }

        }
    }

    private void CheckPlayerInRange()
    {
        target = null; 
        _isTrigered = false; 

        foreach (var player in _players)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            Vector3 directionToPlayer = player.transform.position - transform.position;

            
            if (distance < attackRadius && Vector3.Angle(transform.forward, directionToPlayer) < 90)
            {
                target = player; 
                _isTrigered = true; 
                return; 
            }
        }
    }

    private bool FindPlayerInRange()
    {
        bool find = false;
        foreach (var player in _players)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < attackRadius
                && Vector3.Angle(gameObject.transform.forward, player.transform.position - transform.position) < 90)
            {
                target = player;
                _isTrigered = true;
                find = true;
                _isSleep = false;
                hitColliders = Physics.OverlapSphere(transform.position, attackRadius);
                foreach (var hitCollider in hitColliders)
                {

                    if ((layer.value & (1 << hitCollider.gameObject.layer)) != 0)
                    {
                        if (hitCollider.TryGetComponent(out EnemyBase enemy))
                        {
                            enemy._isSleep = false;
                        }
                    }
                    _agent.FindPath(gameObject, target.transform, _isTrigered, agent);
                }
            }

        }
        return find;
    }

    /*protected void CheckPlayerInRange()
{
   hitColliders = Physics.OverlapSphere(transform.position, attackRadius);
   foreach (var hitCollider in hitColliders)
   {
       if (!_isTrigered)
       {
           if (hitCollider.CompareTag("Player"))
           {
               if (Vector3.Angle(gameObject.transform.forward, hitCollider.transform.position - transform.position) < 90)
               {
                   player = GetComponent<Collider>().transform.gameObject;
                   _isTrigered = true;
                   player = hitCollider.gameObject;
                   break;
               }
           }
       }
       else
       {
           if (hitCollider.gameObject.layer == 9 || hitCollider.gameObject.layer == 11)
           {
               if (hitCollider.TryGetComponent<EnemyMelee>(out EnemyMelee enemyMelee))
               {
                   enemyMelee.IsSleep = false;

               }
               if (hitCollider.TryGetComponent<EnemyRange>(out EnemyRange enemyRange))
               {
                   enemyRange.IsSleep = false;
               }
           }
       }

   }


}*/
    public void GetHit(int damage)
    {
        Health -= damage;
        if (Health == 0)
        {
            _dead = true;
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