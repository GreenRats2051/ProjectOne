using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Camera MainCamera; //������ ������
    public LayerMask WeaponMask; //����� ������
    public Transform PlayerModel; //������ ������
    public Transform MousePoint; //����� ���� ��� ���������
    public Vector3 Point; //����� ����
    private Vector2 InputAction; //������� �����, ����, �����, �����
    private Collider[] Weapons; //��������� ������
    public Slider ArmorSlider;
    public Slider HealthSlider;
    public Rigidbody Rigidbody; //Rigidbody ������
    public Weapon[] PlayerWeapons; //������ ������
    public int Armor; //����� ������
    public int MaxArmor; //������������ ����� ������
    public int Health; //�������� ������
    public int MaxHealth; //������������ �������� ������
    public float Speed; //�������� ������
    public float RadiusCheckWeapon; //������ �������� ������

    void Start()
    {
        ArmorSlider.maxValue = MaxArmor;
        HealthSlider.maxValue = MaxHealth;
    }

    void Update()
    {
        Movement(); //������������
        WeaponPickUp(); //�������� ������
    }

    void Movement()
    {
        InputAction.x = Input.GetAxis("Horizontal");
        InputAction.y = Input.GetAxis("Vertical");
        Rigidbody.velocity = new Vector3(InputAction.x * Speed, Rigidbody.velocity.y, InputAction.y * Speed);
        RaycastHit RaycastHit;
        Ray RayMainCamera = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(RayMainCamera, out RaycastHit, Mathf.Infinity))
        {
            Point = new Vector3(RaycastHit.point.x, 1, RaycastHit.point.z);
        }
        MousePoint.position = Point;
        PlayerModel.LookAt(new Vector3(Point.x, transform.position.y, Point.z));
    }

    void WeaponPickUp()
    {
        Weapons = Physics.OverlapSphere(transform.position, RadiusCheckWeapon, WeaponMask);
        if (Weapons.Length != 0)
        {
            for (int i = 0; i < PlayerWeapons.Length; i++)
            {
                if (Weapons[0].name == PlayerWeapons[i].PlayerWeaponModel.name && PlayerWeapons[i].IsHavyPlayer == false)
                {
                    PlayerWeapons[i].IsHavyPlayer = true;
                    PlayerWeapons[i].PlayerWeaponModel.SetActive(true);
                    Debug.Log("������ " + PlayerWeapons[i].PlayerWeaponModel.name + " ������ " + PlayerWeapons[i].IsHavyPlayer);
                    Destroy(Weapons[0]);
                }
                else if (Weapons[0].name == PlayerWeapons[i].PlayerWeaponModel.name && PlayerWeapons[i].GunController.Magazine != PlayerWeapons[i].GunController.MaxMagazine && PlayerWeapons[i].IsHavyPlayer == true)
                {
                    int RandomMagazine = UnityEngine.Random.Range(1, 3);
                    PlayerWeapons[i].GunController.Magazine += RandomMagazine;
                    Debug.Log("��������� " + RandomMagazine + " ��������� � " + PlayerWeapons[i].PlayerWeaponModel.name);
                    Destroy(Weapons[0]);
                }
                else if (Weapons[0].name != PlayerWeapons[i].PlayerWeaponModel.name)
                {
                    PlayerWeapons[i].PlayerWeaponModel.SetActive(false);
                }
            }
        }
    }
}

[Serializable]
public class Weapon
{
    public GameObject PlayerWeaponModel;
    public GunController GunController;
    public bool IsHavyPlayer;
}
