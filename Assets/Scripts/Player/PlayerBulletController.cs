using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    public int Damage;

    void OnTriggerEnter(Collider Collider)
    {
        if (Collider.tag == "Enemy" && Collider.isTrigger != true)
        {
            Collider.GetComponent<EnemyController>().GetHit(Damage);
            Destroy(gameObject);
        }
    }
}
