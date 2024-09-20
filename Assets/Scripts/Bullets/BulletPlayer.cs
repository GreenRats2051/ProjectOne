using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    [SerializeField]
    private int bulletPower = 3;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9|| collision.gameObject.layer == 11)
        {
            collision.gameObject.GetComponent<EnemyBase>().GetHit(bulletPower);
        }
        Destroy(gameObject);
    }
}
