using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneDestroyer : DronrBase
{
    [SerializeField]
    private GameObject explosionEffect;
    [SerializeField]
    private float explosionRadius;
    [SerializeField]
    private float explosionForce;
    [SerializeField]
    private float destroyDelay;

    private void Update()
    {
        ActionDrone();
    }

    void Explode()
    {
        Instantiate(explosionEffect, instDrone.transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(instDrone.transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rigidbody = nearbyObject.GetComponent<Rigidbody>();
            EnemyStatistics enemyStatistics = nearbyObject.GetComponent<EnemyStatistics>();
            PlayerStatistics playerStatistics = nearbyObject.GetComponent<PlayerStatistics>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
            if (enemyStatistics != null)
            {
                enemyStatistics.GetHit(999);
            }
            if (playerStatistics != null)
            {
                playerStatistics.GetHit(5);
            }
        }
        Destroy(instDrone, destroyDelay);
    }
    internal override void ActionDrone(float valueH, float valueV,bool boolValue)
    {
        base.ActionDrone(valueH, valueV,boolValue);
        if (boolValue)
        {
            Explode();
            Destroy(instDrone);
        }
    }
}
