using UnityEngine;

public class PlayerUseSmokeGrenade : MonoBehaviour
{
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private GameObject smokeGrenade;

    public void Spawn(Vector3 targetPosition)
    {
        GameObject spawnSmoke = Instantiate(smokeGrenade, startPoint.position, Quaternion.identity);
        PlayerSmokeGrenade playerSmokeGrenade = spawnSmoke.GetComponent<PlayerSmokeGrenade>();
        playerSmokeGrenade.ThrowToTarget(spawnSmoke, targetPosition, startPoint.position);
    }
}
