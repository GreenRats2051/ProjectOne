
using UnityEngine;
using UnityEngine.AI;

public class AIController
{

    private float randomPointRadius = 5f;  
    Vector3 randomDirection;
    private Vector3 randomPoint;  
    public void FindPath(GameObject enemyAgent,Transform player,bool _isTrigered,NavMeshAgent agent)
    {
            float distanceToPlayer = Vector3.Distance(enemyAgent.transform.position, player.position);
            if (distanceToPlayer > randomPointRadius )   
            {
                
                FindRandomPointNearPlayer(player);           
                agent.SetDestination(randomPoint);     
            }
            else
            {
                agent.SetDestination(player.position);
            }
    }
    void FindRandomPointNearPlayer( Transform player)
    {

        if (Vector3.Distance(player.position, randomPoint) > randomPointRadius) // Еслиточка все еще в радиусе новуюд не создаем
        {
            randomDirection = Random.insideUnitSphere * randomPointRadius;
            randomDirection += player.position;  
        }
        else
        {
            return;
        }
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, randomPointRadius, NavMesh.AllAreas))
        {
            randomPoint = hit.position;  // Если точка достижима, запоминаем её
        }
    }

}