using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class SupportBase : MonoBehaviour
{
    [SerializeField]
    private Transform _anchorPoint;

    //[SerializeField]
    //private Transform _anchorPointDefault;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private float _maxdistance;

    [SerializeField]
    private float _rayDistance;

    [SerializeField]
    private float _speed;

    //[SerializeField]
    //private LayerMask _wals;

    [SerializeField]
    private float DistanceToAim = 0.15f;

    protected Animator _animator;
    protected NavMeshAgent _agent;
    private RaycastHit _hitInfo;
    private Quaternion lastRotation;
    Quaternion lookRotation;
    public bool _behindCheck;
    public bool _wasLeft = false;
    public bool _givingAmmo = false;
    
    protected virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        lastRotation = transform.rotation;
        ListSupport.Inst.addSupport(gameObject);
    }

    protected virtual void Update()
    {
        if ((_player.playerRb.velocity.sqrMagnitude > 0.1f|| (transform.rotation != lastRotation) )&& !_wasLeft && !_givingAmmo) 
        {
            _agent.SetDestination(_anchorPoint.position);
            _animator.SetBool("isMove", true);
            Vector3 directionToCursor = (_player.playerCurse.position - transform.position).normalized;
            if (_behindCheck)
            {
                 lookRotation = Quaternion.LookRotation(-directionToCursor);
            }
            else
            {
                 lookRotation = Quaternion.LookRotation(directionToCursor);
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            _agent.isStopped = _agent.remainingDistance < DistanceToAim;

            //if (CheckForObstacles())
            //{
            //    Vector3 direction = (_anchorPoint.position - _hitInfo.point).normalized;
            //    float distanceToMove = (_maxdistance - _hitInfo.distance);

            //    if (distanceToMove > 0)
            //    {
            //        _anchorPoint.position += direction * distanceToMove * _speed * Time.deltaTime;
            //    }
            //}
            //else
            //{
            //    _anchorPoint.position = Vector3.Lerp(_anchorPoint.position, _anchorPointDefault.position, Time.deltaTime * _speed);
            //}
        }
        if (_givingAmmo)
        {
            _agent.updateRotation = true;
            _agent.SetDestination(_player.playerTransform.position);
            _agent.isStopped = _agent.remainingDistance < DistanceToAim;
            if(_agent.remainingDistance < DistanceToAim)
            {
                GiveToPlayer();
                _givingAmmo = false;
                _agent.updateRotation = false;
            }
        }
        else
        {
            _animator.SetBool("isMove", false);
        }

    }
    public void GiveToPlayer()
    {

    }
    public void SetOrder(bool lefted,bool behind)
    {
        _wasLeft = lefted;
        _behindCheck = behind;
    }
    public void AssignAnchor(Transform anchor)
    {
        _anchorPoint = anchor;
        //_anchorPointDefault = Instantiate(anchor);
        
        
    }
    //private bool CheckForObstacles()
    //{
    //    return Physics.Raycast(transform.position, transform.forward, out _hitInfo, _maxdistance, _wals) ||
    //           Physics.Raycast(transform.position, -transform.forward, out _hitInfo, _maxdistance, _wals) ||
    //           Physics.Raycast(transform.position, transform.right, out _hitInfo, _maxdistance, _wals) ||
    //           Physics.Raycast(transform.position, -transform.right, out _hitInfo, _maxdistance, _wals);
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * _maxdistance);
        Gizmos.DrawRay(transform.position, -transform.forward * _maxdistance);
        Gizmos.DrawRay(transform.position, transform.right * _maxdistance);
        Gizmos.DrawRay(transform.position, -transform.right * _maxdistance);
    }
}
