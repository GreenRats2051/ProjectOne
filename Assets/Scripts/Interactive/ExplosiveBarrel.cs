using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionEffect;
    [SerializeField]
    private int health;
    [SerializeField]
    private float explosionRadius;
    [SerializeField]
    private float explosionForce;
    [SerializeField]
    private float destroyDelay;

    public void GetHit(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rigidbody = nearbyObject.GetComponent<Rigidbody>();
            EnemyStatistics enemyStatistics = nearbyObject.GetComponent<EnemyStatistics>();
            PlayerStatistics playerStatistics = nearbyObject.GetComponent<PlayerStatistics>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
            if (enemyStatistics != null)
            {
                enemyStatistics.GetHit(999);
            }
            if (playerStatistics != null)
            {
                playerStatistics.GetHit(5);
            }
        }
        Destroy(gameObject, destroyDelay);
    }
}
