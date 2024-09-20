using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    [SerializeField]
    private float _walkSoundRadius = 7;
    Collider[] hits;
    void Update()
    {
        if (gameObject.GetComponent<LisenerActiveButton>().Iscrouch || gameObject.transform.position.magnitude < 0.2f)
        {
            hits = Physics.OverlapSphere(gameObject.transform.position, 1f);
        }
        else
        {
            hits = Physics.OverlapSphere(gameObject.transform.position, _walkSoundRadius);
        }

        foreach(Collider hit in hits)
        {
            if(hit.gameObject.layer == 9|| hit.gameObject.layer == 11)
            {
                if(hit.TryGetComponent<EnemyBase>(out EnemyBase enemy))
                {
                    enemy.IsSleep = false;
                    enemy.Player = gameObject;
                    enemy.IsTrigered = true;
                }
                //if (hit.TryGetComponent<EnemyRange>(out EnemyRange enemyRange))
                //{
                //    enemyRange.IsSleep = false;
                //}
            }
        }
    }
}
