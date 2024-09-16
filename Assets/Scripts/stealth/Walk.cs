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
        if (gameObject.GetComponent<Liusener>().Iscrouch)
        {
            hits = Physics.OverlapSphere(gameObject.transform.position, 1f);
        }
        else
        {
            hits = Physics.OverlapSphere(gameObject.transform.position, _walkSoundRadius);
        }

        foreach(Collider hit in hits)
        {
            if(hit.gameObject.layer == 9)
            {
                if(hit.TryGetComponent<EnemyMelee>(out EnemyMelee enemyMelee))
                {
                    enemyMelee.IsSleep = false;
                }
                if (hit.TryGetComponent<EnemyRange>(out EnemyRange enemyRange))
                {
                    enemyRange.IsSleep = false;
                }
            }
        }
    }
}
