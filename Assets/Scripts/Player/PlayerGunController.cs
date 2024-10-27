using TMPro;
using UnityEngine;

public class PlayerGunController : MonoBehaviour
{
    [SerializeField]
    private Transform StartShoot;
    [SerializeField]
    private TMP_Text AmmoAndMagazineText;
    [SerializeField]
    private PlayerController PlayerController;
    [SerializeField]
    private PlayerGunSettings GunSettings;
    public int Ammo;
    public int Magazine;
    private int NeedAmmo;
    private float TimeShoot;

    void Update()
    {
        Shoot();
        Reload();
        GunUi();
    }

    void Shoot()
    {
        TimeShoot += Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0) && Ammo != 0 && TimeShoot >= GunSettings.TimeNextShoot)
        {
            TimeShoot = 0;
            Ammo--;
            Vector3 TargetPoint = new Vector3(PlayerController.MousePoint.position.x, StartShoot.position.y, PlayerController.MousePoint.position.z);
            Vector3 DirWithoutSpread = TargetPoint - StartShoot.position;
            Vector2 ForceShoot = new Vector2(Random.Range(-GunSettings.SpreadX, GunSettings.SpreadX), Random.Range(-GunSettings.SpreadY, GunSettings.SpreadY));
            Vector3 DirWithSpread = DirWithoutSpread + new Vector3(ForceShoot.x, ForceShoot.y, 0);
            DirWithSpread = DirWithoutSpread + new Vector3(ForceShoot.x, ForceShoot.y, 0);
            GameObject CurrentBulletObject = Instantiate(GunSettings.Bullet, StartShoot.position, StartShoot.rotation);
            CurrentBulletObject.transform.forward = DirWithSpread.normalized;
            CurrentBulletObject.GetComponent<Rigidbody>().AddForce(DirWithSpread.normalized * GunSettings.ShootForce, ForceMode.Impulse);
            CurrentBulletObject.GetComponent<PlayerBulletController>().Damage = GunSettings.Damage;
            Destroy(CurrentBulletObject, GunSettings.TimeDestroyBullet);
        }
    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && Magazine != 0)
        {
            NeedAmmo = GunSettings.MaxAmmo - Ammo;
            Magazine -= NeedAmmo;
            Ammo += NeedAmmo;
            Ammo = GunSettings.MaxAmmo;
        }
    }

    void GunUi()
    {
        if (gameObject.activeSelf)
        {
            AmmoAndMagazineText.text = Ammo + "/" + Magazine;
        }
    }
}
