using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour
{
    [Header("States")]

    [SerializeField]
    private bool _patrol, _attack;
    [SerializeField]
    private bool _isTrigered = false;
    [Space]
    
    
    [Header("Patrol Points")]
    [SerializeField]
    List<Vector3> points = new List<Vector3>();
    private int currentPointIndex = -1;

    [Header("restrictions")]
    [SerializeField]
    private int _attackRadius, _MeleeDictance;


    
    private NavMeshAgent agent;
    private GameObject player;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, _attackRadius);

            foreach (Collider collider in hitColliders)
            {
                if (collider.transform.tag == "Player")
                {
                    if (Vector3.Angle(gameObject.transform.forward, collider.transform.position - transform.position) < 90)   
                    {
                        player = collider.transform.gameObject;
                        _attack = true;
                        break;
                    }
                    else
                    {
                        _attack = false;
                    }
                }
                else
                {
                    _attack = false;
                }
            }
        
        Attacking();
        Patrol();

    }
    
    private void Attacking()
    {
        if (_attack)
        {

            agent.SetDestination(player.transform.position);
            if (!agent.pathPending && agent.remainingDistance < _MeleeDictance)
            {
                agent.ResetPath();
            }
        }
    }
    private void Patrol()
    {
        if (_patrol&&!_attack)
        {
            if (points.Count > 0)
            {
                agent.SetDestination(points[0]);
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    currentPointIndex = (currentPointIndex + 1) % points.Count;
                    agent.SetDestination(points[currentPointIndex]);
                }

            }

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }
}
