using UnityEngine;

public class PlayerGunController : MonoBehaviour
{
    [SerializeField]
    private Transform startShoot;
    [SerializeField]
    private PlayerGunSettings gunSettings;
    public int Ammo;
    public int Magazine;
    private int needAmmo;
    [SerializeField]
    private float timeShoot;

    void Start()
    {
        timeShoot = gunSettings.TimeNextShoot;
    }

    void Update()
    {
        timeShoot += Time.deltaTime;
    }

    public void Shoot(PlayerMovementController playerMovementController)
    {
        if (Ammo != 0 && timeShoot >= gunSettings.TimeNextShoot)
        {
            timeShoot = 0;
            Ammo--;
            Vector3 TargetPoint = new Vector3(playerMovementController.MousePoint.position.x, startShoot.position.y, playerMovementController.MousePoint.position.z);
            Vector3 DirWithoutSpread = TargetPoint - startShoot.position;
            Vector2 ForceShoot = new Vector2(Random.Range(-gunSettings.SpreadX, gunSettings.SpreadX), Random.Range(-gunSettings.SpreadY, gunSettings.SpreadY));
            Vector3 DirWithSpread = DirWithoutSpread + new Vector3(ForceShoot.x, ForceShoot.y, 0);
            DirWithSpread = DirWithoutSpread + new Vector3(ForceShoot.x, ForceShoot.y, 0);
            GameObject CurrentBulletObject = Instantiate(gunSettings.Bullet, startShoot.position, startShoot.rotation);
            CurrentBulletObject.transform.forward = DirWithSpread.normalized;
            CurrentBulletObject.GetComponent<Rigidbody>().AddForce(DirWithSpread.normalized * gunSettings.ShootForce, ForceMode.Impulse);
            CurrentBulletObject.AddComponent<PlayerBulletController>();
            CurrentBulletObject.GetComponent<PlayerBulletController>().Damage = gunSettings.Damage;
            Destroy(CurrentBulletObject, gunSettings.TimeDestroyBullet);
        }
    }

    public void Reload()
    {
        if (Magazine != 0)
        {
            needAmmo = gunSettings.MaxAmmo - Ammo;
            Magazine -= needAmmo;
            Ammo += needAmmo;
            Ammo = gunSettings.MaxAmmo;
        }
    }
}
