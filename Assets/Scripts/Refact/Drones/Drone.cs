using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Drone : DronrBase
{

    private float healTimer = 0f;
   [SerializeField] private float healInterval = 0.5f;
    protected override void Interacting()
    {
        
    }


    private void Update()
    {
        if (instDrone != null)
        {
            healTimer += Time.deltaTime; 

            if (healTimer >= healInterval) 
            {
                gameObject.GetComponent<Player>().Healing(1); 
                healTimer = 0f; 
            }
        }
    }
}
