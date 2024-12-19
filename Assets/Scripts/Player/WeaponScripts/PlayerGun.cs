using UnityEngine;
using UnityEngine.UI;

public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    private Transform startShoot;
    [SerializeField]
    private Image currentIconWeapon;
    [SerializeField]
    private PlayerGunSettings gunSettings;
    public int Ammo;
    public int Magazine;
    private int needAmmo;
    private float timeShoot;

    void Start()
    {
        timeShoot = gunSettings.TimeNextShoot;
    }

    void Update()
    {
        timeShoot += Time.deltaTime;
        currentIconWeapon.fillAmount = (float)Ammo / gunSettings.MaxAmmo;
    }

    public void Shoot(PlayerMovement playerMovementController)
    {
        if (Ammo != 0 && timeShoot >= gunSettings.TimeNextShoot)
        {
            timeShoot = 0;
            Ammo--;
            Vector3 TargetPoint = new Vector3(playerMovementController.MousePoint.position.x, playerMovementController.MousePoint.position.y + 1.4f, playerMovementController.MousePoint.position.z);
            Vector3 DirWithoutSpread = TargetPoint - startShoot.position;
            Vector2 ForceShoot = new Vector2(Random.Range(-gunSettings.SpreadX, gunSettings.SpreadX), Random.Range(-gunSettings.SpreadY, gunSettings.SpreadY));
            Vector3 DirWithSpread = DirWithoutSpread + new Vector3(ForceShoot.x, ForceShoot.y, 0);
            DirWithSpread = DirWithoutSpread + new Vector3(ForceShoot.x, ForceShoot.y, 0);
            GameObject CurrentBulletObject = Instantiate(gunSettings.Bullet, startShoot.position, startShoot.rotation);
            CurrentBulletObject.transform.forward = DirWithSpread.normalized;
            CurrentBulletObject.GetComponent<Rigidbody>().AddForce(DirWithSpread.normalized * gunSettings.ShootForce, ForceMode.Impulse);
            CurrentBulletObject.AddComponent<PlayerBullet>();
            CurrentBulletObject.GetComponent<PlayerBullet>().Damage = gunSettings.Damage;
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
