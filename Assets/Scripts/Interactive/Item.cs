using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] bool isHealth;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<PlayerStatistics>())
        {
            if (isHealth)
            {
                collider.GetComponent<PlayerStatistics>().Healing(10);
            }
            else
            {
                collider.GetComponent<PlayerStatistics>().Armoring();
            }
            Destroy(gameObject);
        }
    }
}
