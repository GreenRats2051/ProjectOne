using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneDestroyer : DronrBase
{
    [SerializeField] private int power = 55;
    ////[SerializeField] private PlayerController PlayerController;

    ////protected override void OnDestroy()
    ////{
    ////    PlayerController.SwitchToDrone();
    ////}
    //public void SpawnSetings(Player player)
    //{
    //    PlayerController = player;
    //    player.SwitchToDrone();
    //}
    private void Update()
    {
        Interacting();
    }
  
    private void Expore()
    {
        Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, 3);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].gameObject.TryGetComponent(out Player player))
            {
                player.GetHit(power);
            }
            if (hits[i].gameObject.TryGetComponent(out EnemyBase enemy))
            {
                enemy.GetHit(power);
            }
        }
    }
    protected override void Interacting()
    {
        gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Expore();
            Destroy(gameObject);
        }
    }
}
