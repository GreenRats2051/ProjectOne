using UnityEngine;
using UnityEngine.AI;

public class AIController
{
    private float randomPointRadius = 5f;
    private Vector3 randomPoint;

    public void FindPath(GameObject enemyAgent, Transform player, bool isTriggered, NavMeshAgent agent)
    {
        if (isTriggered)
        {
            float distanceToPlayer = Vector3.Distance(enemyAgent.transform.position, player.position);

            if (distanceToPlayer > agent.stoppingDistance)
            {
                agent.SetDestination(player.position);
            }
        }
        else
        {
            FindRandomPointNearPlayer(player, agent);
        }
    }

    private void FindRandomPointNearPlayer(Transform player, NavMeshAgent agent)
    {
        Vector3 randomDirection = Random.insideUnitSphere * randomPointRadius;
        randomDirection += player.position;
        randomDirection.y = 0;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, randomPointRadius, NavMesh.AllAreas))
        {
            randomPoint = hit.position;
            agent.SetDestination(randomPoint);
        }
    }
}
