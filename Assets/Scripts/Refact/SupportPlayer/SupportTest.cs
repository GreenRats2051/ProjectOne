using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportShooter : SupportBase
{
    private bool isShooting;
    private FieldOfView field;
    private Transform targetLock;
    private int maxdistance;
    private LayerMask Tm8;
    private LayerMask Enemy;
    protected override void Start()
    {
        base.Start();
        field = GetComponent<FieldOfView>();
    }
    protected override void Update()
    {
        base.Update();
        if (isShooting && field.VisibleEnemies.Count > 0)
        {
            targetLock = field.VisibleEnemies[0];
            foreach (var enemy in field.VisibleEnemies)
            {
                if (Vector3.Distance(enemy.position, gameObject.transform.position) < Vector3.Distance(targetLock.position, gameObject.transform.position))
                {
                    targetLock = enemy;
                }
            }
            transform.LookAt(targetLock);
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, transform.forward, maxdistance, Tm8) && Physics.Raycast(transform.position, transform.forward, out hit, maxdistance, Enemy))
            {

            }
        }
    }
}
