using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public GameObject prefub;
    public float throwHeight = 10f;
    public float gravity = 9.81f;

    public void spawn(Vector3 targetPosition, Vector3 startPoint)
    {
        GameObject spawnSmoke = Instantiate(prefub, startPoint, Quaternion.identity);
        Smoke smokeScript = spawnSmoke.GetComponent<Smoke>();
        Rigidbody rb = spawnSmoke.GetComponentInChildren<Rigidbody>();
        ParticleSystem particleSystem = spawnSmoke.GetComponent<ParticleSystem>();
        smokeScript.ThrowToTarget(targetPosition, rb, startPoint, particleSystem);
    }

    private void OnParticleCollision(GameObject other)
    {

        if (other.gameObject.layer == 9)
        {
            other.GetComponent<EnemyBase>().IsSleep = true;
            other.GetComponent<EnemyBase>().IsTrigered = true;
            other.GetComponent<EnemyBase>().Player = null;
        }
    }

    void ThrowToTarget(Vector3 targetPosition, Rigidbody rb, Vector3 transformPoint,ParticleSystem particleSystem)
    {
        Vector3 direction = targetPosition - transformPoint;
        Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);
        float time = Mathf.Sqrt(2 * throwHeight / gravity) + Mathf.Sqrt(2 * (throwHeight - (transformPoint.y - targetPosition.y)) / gravity);
        Vector3 velocityXZ = directionXZ / time;
        float velocityY = gravity * time / 2;
        Vector3 initialVelocity = velocityXZ + Vector3.up * velocityY;
        rb.velocity = initialVelocity;
        StartCoroutine(ActiveAndDeactive(time,  particleSystem,rb));
    }

    IEnumerator ActiveAndDeactive(float time,ParticleSystem particleSystem, Rigidbody rb)
    {
        yield return new WaitForSeconds(time);
        particleSystem.transform.position = rb.position;
        particleSystem.Play();
            yield return new WaitForSeconds(particleSystem.main.duration);
        
        Destroy(gameObject);
    }
}