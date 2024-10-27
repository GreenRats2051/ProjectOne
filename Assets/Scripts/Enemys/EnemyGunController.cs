using UnityEngine;

public class EnemyGunController : MonoBehaviour
{
    [SerializeField]
    private Transform StartShoot;
    [SerializeField]
    LineRenderer LineRenderer;
    [SerializeField]
    private EnemyController EnemController;
    [SerializeField]
    private EnemyGunSettings GunSettings;
    private float TimeShoot;

    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        TimeShoot += Time.deltaTime;
        if (TimeShoot >= GunSettings.TimeNextShoot && EnemController.Player != null)
        {
            TimeShoot = 0;
            Vector3 TargetPoint = new Vector3(EnemController.Player.transform.position.x, StartShoot.position.y, EnemController.Player.transform.position.z);
            Vector3 DirWithoutSpread = TargetPoint - StartShoot.position;
            Vector2 ForceShoot = new Vector2(Random.Range(-GunSettings.SpreadX, GunSettings.SpreadX), Random.Range(-GunSettings.SpreadY, GunSettings.SpreadY));
            Vector3 DirWithSpread = DirWithoutSpread + new Vector3(ForceShoot.x, ForceShoot.y, 0);
            DirWithSpread = DirWithoutSpread + new Vector3(ForceShoot.x, ForceShoot.y, 0);
            GameObject CurrentBulletObject = Instantiate(GunSettings.Bullet, StartShoot.position, StartShoot.rotation);
            CurrentBulletObject.transform.forward = DirWithSpread.normalized;
            CurrentBulletObject.GetComponent<Rigidbody>().AddForce(DirWithSpread.normalized * GunSettings.ShootForce, ForceMode.Impulse);
            CurrentBulletObject.GetComponent<EnemyBulletController>().Damage = GunSettings.Damage;
            Destroy(CurrentBulletObject, GunSettings.TimeDestroyBullet);
        }
        RaycastHit RaycastHit;
        Ray Ray = new Ray(StartShoot.position, StartShoot.forward);
        if (Physics.Raycast(Ray, out RaycastHit, 100f))
        {
            LineRenderer.SetPosition(1, new Vector3(0, 0, RaycastHit.point.z));
        }
    }
}
