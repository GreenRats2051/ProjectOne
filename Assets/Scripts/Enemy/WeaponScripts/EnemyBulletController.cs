using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public int Damage;

    void OnTriggerEnter(Collider Collider)
    {
        if (Collider.tag == "Player" && Collider.isTrigger != true)
        {
            Collider.GetComponent<PlayerStatistics>().GetHit(Damage);
            Destroy(gameObject);
        }
    }
}
