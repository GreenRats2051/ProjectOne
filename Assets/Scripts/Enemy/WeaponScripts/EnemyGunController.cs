using UnityEngine;

public class EnemyGunController : MonoBehaviour
{
    [SerializeField]
    private Transform startShoot;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private EnemyMovementController enemyMovementController;
    [SerializeField]
    private EnemyGunSettings gunSettings;
    private float timeShoot;

    void Update()
    {
        timeShoot += Time.deltaTime;
        if (enemyMovementController.Player != null && timeShoot >= gunSettings.TimeNextShoot)
        {
            timeShoot = 0;
            Vector3 TargetPoint = new Vector3(enemyMovementController.Player.transform.position.x, startShoot.position.y, enemyMovementController.Player.transform.position.z);
            Vector3 DirWithoutSpread = TargetPoint - startShoot.position;
            Vector2 ForceShoot = new Vector2(Random.Range(-gunSettings.SpreadX, gunSettings.SpreadX), Random.Range(-gunSettings.SpreadY, gunSettings.SpreadY));
            Vector3 DirWithSpread = DirWithoutSpread + new Vector3(ForceShoot.x, ForceShoot.y, 0);
            DirWithSpread = DirWithoutSpread + new Vector3(ForceShoot.x, ForceShoot.y, 0);
            GameObject CurrentBulletObject = Instantiate(gunSettings.Bullet, startShoot.position, startShoot.rotation);
            CurrentBulletObject.transform.forward = DirWithSpread.normalized;
            CurrentBulletObject.GetComponent<Rigidbody>().AddForce(DirWithSpread.normalized * gunSettings.ShootForce, ForceMode.Impulse);
            CurrentBulletObject.AddComponent<EnemyBulletController>();
            CurrentBulletObject.GetComponent<EnemyBulletController>().Damage = gunSettings.Damage;
            Destroy(CurrentBulletObject, gunSettings.TimeDestroyBullet);
        }
        RaycastHit RaycastHit;
        Ray Ray = new Ray(startShoot.position, startShoot.forward);
        if (Physics.Raycast(Ray, out RaycastHit, 100f))
        {
            lineRenderer.SetPosition(1, new Vector3(0, 0, RaycastHit.point.z));
        }
    }
}
