using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //удар
        }
    }
}
public class EnemyMeleeAnimation : StateMachineBehaviour
{
    Animator animator;
    private void Awake()
    {
        
    }
}
