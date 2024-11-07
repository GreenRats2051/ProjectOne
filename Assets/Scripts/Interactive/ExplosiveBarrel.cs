using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField]
    private float explosionRadius; // Радиус взрыва
    [SerializeField]
    private float explosionForce; // Сила взрыва
    [SerializeField]
    private GameObject explosionEffect; // Эффект взрыва
    [SerializeField]
    private float destroyDelay; // Задержка перед уничтожением бочки

    void OnColliderEnter(Collider Collider)
    {
        if (Collider.tag == "Bullet")
        {
            Explode();
        }
    }

    void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
        Destroy(gameObject, destroyDelay);
    }
}
