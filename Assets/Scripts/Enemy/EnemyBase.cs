using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected int attackRadius; 
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected GameObject player;
    [SerializeField] protected bool _isTrigered= false;
    [SerializeField] protected bool _isSleep= false;
    public bool IsSleep { get => _isSleep; set => _isSleep = value; }
    private AIController _agent;
    private Collider[] hitColliders;
    protected abstract void AttackPlayer(); 
    protected abstract void Patrol();  
    protected abstract void Animate(); 

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _agent = new();
    }

    protected virtual void Update()
    {
        Animate();
        if (_isSleep)
        {
            return;
        }
        CheckPlayerInRange();
        Patrol();
        AttackPlayer();
        if(player!=null&&_isTrigered)
            _agent.FindPath(gameObject,player.transform,_isTrigered,agent);
    }
    protected void CheckPlayerInRange()
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
                if (hitCollider.gameObject.layer == 9)
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


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        // Отрисовка пути NavMesh агента
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