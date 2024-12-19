using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DroneHeal : DronrBase
{

    private float healTimer = 0f;
   [SerializeField] private float healInterval = 0.5f;
    private void Update()
    {
        if (instDrone != null)
        {
            healTimer += Time.deltaTime; 

            if (healTimer >= healInterval) 
            {
                gameObject.GetComponent<PlayerStatistics>().Healing(1); 
                healTimer = 0f; 
            }
        }
    }
}
