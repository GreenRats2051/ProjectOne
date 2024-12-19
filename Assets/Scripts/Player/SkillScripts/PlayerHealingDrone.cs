using UnityEngine;
using UnityEngine.AI;

public class PlayerHealingDrone : MonoBehaviour
{
    public GameObject player;
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    private float followDistance;
    [SerializeField]
    private float speedFly;
    [SerializeField]
    private float flightHeight;
    private float healTimer;
    [SerializeField]
    private float healInterval;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.transform.position - player.transform.forward * followDistance;
            targetPosition.y = flightHeight;
            navMeshAgent.SetDestination(targetPosition);
            float wave = Mathf.Sin(Time.time * speedFly) * 0.2f;
            transform.position = new Vector3(transform.position.x, flightHeight + wave, transform.position.z);
            healTimer += Time.deltaTime;
            if (healTimer >= healInterval && player.GetComponent<PlayerStatistics>().healthValue > 0)
            {
                player.GetComponent<PlayerStatistics>().Healing(1);
                healTimer = 0f;
            }
        }
    }
}
