using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int Damage;
    [SerializeField] private LayerMask LayerMask;
    void OnTriggerEnter(Collider Collider)
    {
        if (Collider.isTrigger != true)
        {
            if (Collider.GetComponent<EnemyStatistics>())
            {
                Collider.GetComponent<EnemyStatistics>().GetHit(Damage);
            }
            if (Collider.tag == "Explode")
            {
                Collider.GetComponent<ExplosiveBarrel>().GetHit(Damage);
            }
            Destroy(gameObject);
        }
    }
}
