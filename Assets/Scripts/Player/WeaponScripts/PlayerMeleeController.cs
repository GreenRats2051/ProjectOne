using UnityEngine;

public class PlayerMeleeController : MonoBehaviour
{
    [SerializeField]
    private LayerMask enemyMask;
    [SerializeField]
    private Transform attackZone;
    [SerializeField]
    private Vector3 sizeAttackZone;
    private Collider[] enemysCollider;
    public int Damage;

    void Update()
    {
        enemysCollider = Physics.OverlapBox(attackZone.position, sizeAttackZone, Quaternion.identity, enemyMask);
    }

    public void Attack()
    {
        if (enemysCollider.Length != 0)
        {
            for (int i = 0; i < enemysCollider.Length; i++)
            {
                enemysCollider[i].GetComponent<EnemyStatistics>().GetHit(Damage);
            }
        }
    }
}
