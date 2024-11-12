using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int Damage;

    void OnTriggerEnter(Collider Collider)
    {
        if (Collider.isTrigger != true)
        {
            if (Collider.tag == "Enemy")
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
