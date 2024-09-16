using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    public GameObject Bullet; //����
    public Transform StartShoot; //������ ��������
    private Vector3 TargetPoint; //����� ��������
    private Vector2 ForceShoot; //����������� ��������
    public TMP_Text WeaponName; //��� ������
    public Slider AmmoSlider; //������� �������
    public Slider MagazineSlider; //������� ��������
    public PlayerController PlayerController; //PlayerController ������
    public int Ammo; //�������
    public int MaxAmmo; //������������ ���������� ��������
    public int Magazine; //�������
    public int MaxMagazine; //������������ ���������� ���������
    private float TimeShoot; //����� ��������
    public float TimeNextShoot; //����� ���������� �������� (������� � �������)
    public float ShootForce; //�������� ����
    public float SpreadX; //������� ���� �� ��� X
    public float SpreadY; //������� ���� �� ��� Y
    public float TimeDestroyBullet; //����� ����������� ����
    
    void Update()
    {
        Shoot(); //��������
        Reload(); //�����������
        GunUi(); //UI ������
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
