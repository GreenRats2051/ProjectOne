using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAttack : MonoBehaviour
{
    [SerializeField]
    EnemyMelee EnemyMelee;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other);
            other.GetComponent<PlayerController>().GetHit(EnemyMelee.Power);
        }
    }
}
