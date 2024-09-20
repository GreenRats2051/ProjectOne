using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Drone : DronrBase
{

    private float healTimer = 0f;
    private float healInterval = 1f;
    protected override void Interacting()
    {
        Debug.Log("Interacting");
    }


    private void Update()
    {
        if (instDrone != null)
        {
            healTimer += Time.deltaTime; 

            if (healTimer >= healInterval) 
            {
                gameObject.GetComponent<PlayerController>().Healing(1); 
                healTimer = 0f; 
            }
        }
    }
}
