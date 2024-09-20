using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    public GameObject Bullet; //Пуля
    public Transform StartShoot; //Начала выстрела
    private Vector3 TargetPoint; //Точка выстрела
    private Vector2 ForceShoot; //Направление выстрела
    public TMP_Text WeaponName; //Имя оружия
    public Slider AmmoSlider; //Слайдер патрона
    public Slider MagazineSlider; //Слайдер магазина
    public PlayerController PlayerController; //PlayerController игрока
    public int Ammo; //Патроны
    public int MaxAmmo; //Максимальное количество патронов
    public int Magazine; //Магазин
    public int MaxMagazine; //Максимальное количество магазинов
    private float TimeShoot; //Время выстрела
    public float TimeNextShoot; //Время следующего выстрела (выстрел в секунду)
    public float ShootForce; //Скорость пули
    public float SpreadX; //Разброс пуль по оси X
    public float SpreadY; //Разброс пуль по оси Y
    public float TimeDestroyBullet; //Время уничтожения пули
    
    void Update()
    {
        Shoot(); //Стрельба
        Reload(); //Перезарядка
        GunUi(); //UI оружия
    }

    void Shoot()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Ammo != 0 && Time.time >= TimeShoot)
        {
            TimeShoot = Time.time + 1 / TimeNextShoot;
            Ammo--;
            AmmoSlider.value = Ammo;
            TargetPoint = new Vector3(PlayerController.MousePoint.position.x, PlayerController.MousePoint.position.y + 1, PlayerController.MousePoint.position.z);
            Vector3 DirWithoutSpread = TargetPoint - StartShoot.position;
            ForceShoot.x = Random.Range(-SpreadX, SpreadX);
            ForceShoot.y = Random.Range(-SpreadY, SpreadY);
            Vector3 DirWithSpread = DirWithoutSpread + new Vector3(ForceShoot.x, ForceShoot.y, 0);
            ForceShoot.x = Random.Range(-SpreadX, SpreadX);
            ForceShoot.y = Random.Range(-SpreadY, SpreadY);
            DirWithSpread = DirWithoutSpread + new Vector3(ForceShoot.x, ForceShoot.y, 0);
            GameObject Current_Bullet = Instantiate(Bullet, StartShoot.position, Quaternion.identity);
            Current_Bullet.transform.forward = DirWithSpread.normalized;
            Current_Bullet.GetComponent<Rigidbody>().AddForce(DirWithSpread.normalized * ShootForce, ForceMode.Impulse);
            Destroy(Current_Bullet, TimeDestroyBullet);
        }
    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && Magazine != 0)
        {
            Magazine--;
            MagazineSlider.value = Magazine;
            Ammo = MaxAmmo;
            AmmoSlider.value = Ammo;
        }
    }

    void GunUi()
    {
        if (gameObject.activeSelf)
        {
            WeaponName.text = transform.name;
            AmmoSlider.maxValue = MaxAmmo;
            AmmoSlider.value = Ammo;
            MagazineSlider.maxValue = MaxMagazine;
            MagazineSlider.value = Magazine;
        }
    }
}
