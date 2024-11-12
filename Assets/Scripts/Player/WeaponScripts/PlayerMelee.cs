using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField]
    private LayerMask enemyMask;
    [SerializeField]
    private Transform attackZone;
    private Collider[] enemysCollider;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float attackRadius;
    [SerializeField]
    private float timeAttack;
    [SerializeField]
    private float timeNextAttack;

    void Update()
    {
        enemysCollider = Physics.OverlapSphere(attackZone.position, attackRadius, enemyMask);
        if (timeAttack < timeNextAttack)
        {
            timeAttack += Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (timeAttack >= timeNextAttack)
        {
            Debug.Log("You attack");
            if (enemysCollider.Length != 0)
            {
                for (int i = 0; i < enemysCollider.Length; i++)
                {
                    enemysCollider[i].GetComponent<EnemyStatistics>().GetHit(damage);
                }
            }
            timeAttack = 0;
        }
    }
}
