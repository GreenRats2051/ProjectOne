using System.Collections;
using UnityEngine;

public class PlayerSmokeGrenade : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rigidbody;
    [SerializeField]
    private ParticleSystem particleSystem;
    [SerializeField]
    private float throwHeight;
    [SerializeField]
    private float gravity;

    public void ThrowToTarget(GameObject spawnSmoke, Vector3 targetPosition, Vector3 transformPoint)
    {
        Vector3 direction = targetPosition - transformPoint;
        Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);
        float time = Mathf.Sqrt(2 * throwHeight / gravity) + Mathf.Sqrt(2 * (throwHeight - (transformPoint.y - targetPosition.y)) / gravity);
        Vector3 velocityXZ = directionXZ / time;
        float velocityY = gravity * time / 2;
        Vector3 initialVelocity = velocityXZ + Vector3.up * velocityY;
        rigidbody.velocity = initialVelocity;
        StartCoroutine(ActiveAndDeactive(spawnSmoke, time));
    }

    IEnumerator ActiveAndDeactive(GameObject spawnSmoke, float time)
    {
        yield return new WaitForSeconds(time + 0.28f);
        ParticleSystem smoke = Instantiate(particleSystem, transform.position, Quaternion.identity);
        smoke.Play();
        yield return new WaitForSeconds(particleSystem.main.duration);
        Destroy(spawnSmoke);
        Destroy(smoke, 20);
    }

    void OnParticleCollision(GameObject gameObject)
    {
        if (gameObject.GetComponent<EnemyStatistics>() != null)
        {
            gameObject.GetComponent<EnemyMovement>().Player = null;
            gameObject.GetComponent<EnemyStatistics>().IsSleep = true;
            gameObject.GetComponent<EnemyStatistics>().IsTrigered = true;
        }
    }
}
