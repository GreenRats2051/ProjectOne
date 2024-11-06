using UnityEngine;

public class PlayerBulletController : MonoBehaviour
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
            Destroy(gameObject);
        }
    }
}
