using System.Collections;
using UnityEngine;

public class PlayerSmokeGrenade : MonoBehaviour
{
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private GameObject prefub;
    [SerializeField]
    private float throwHeight;
    [SerializeField]
    private float gravity;

    public void Spawn(Vector3 targetPosition)
    {
        GameObject spawnSmoke = Instantiate(prefub, startPoint.position, Quaternion.identity);
        PlayerSmokeGrenade smokeScript = spawnSmoke.GetComponent<PlayerSmokeGrenade>();
        Rigidbody rigidbody = spawnSmoke.GetComponentInChildren<Rigidbody>();
        ParticleSystem particleSystem = spawnSmoke.GetComponent<ParticleSystem>();
        smokeScript.ThrowToTarget(targetPosition, startPoint.position, rigidbody, particleSystem);
    }

    void ThrowToTarget(Vector3 targetPosition, Vector3 transformPoint, Rigidbody rigidbody, ParticleSystem particleSystem)
    {
        Vector3 direction = targetPosition - transformPoint;
        Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);
        float time = Mathf.Sqrt(2 * throwHeight / gravity) + Mathf.Sqrt(2 * (throwHeight - (transformPoint.y - targetPosition.y)) / gravity);
        Vector3 velocityXZ = directionXZ / time;
        float velocityY = gravity * time / 2;
        Vector3 initialVelocity = velocityXZ + Vector3.up * velocityY;
        rigidbody.velocity = initialVelocity;
        StartCoroutine(ActiveAndDeactive(rigidbody, particleSystem, time));
    }

    IEnumerator ActiveAndDeactive(Rigidbody rigidbody, ParticleSystem particleSystem, float time)
    {
        yield return new WaitForSeconds(time);
        particleSystem.transform.position = rigidbody.position;
        particleSystem.Play();
        yield return new WaitForSeconds(particleSystem.main.duration);
        Destroy(gameObject);
    }

    void OnParticleCollision(GameObject gameObject)
    {

        if (gameObject.tag == "Enemy")
        {
            gameObject.GetComponent<EnemyMovementController>().Player = null;
            gameObject.GetComponent<EnemyStatistics>().IsSleep = true;
            gameObject.GetComponent<EnemyStatistics>().IsTrigered = false;
        }
    }
}
