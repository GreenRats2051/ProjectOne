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
    public Slider ArmorSlider; //������� �����
    public Slider HealthSlider; //������� ��������
    public Rigidbody Rigidbody; //Rigidbody ������
    public Weapon[] PlayerWeapons; //������ ������
    public int Armor; //����� ������
    public int MaxArmor; //������������ ����� ������
    public int Health; //�������� ������
    public int MaxHealth; //������������ �������� ������
    private int WeaponSwitch; //
    private int CurrentWeapon; //������� ��������� ������
    private float MouseScroll; //������������� ������ ����
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
        SelectWeapons(); //����� ������
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
                if (Weapons[0].name == PlayerWeapons[i].PlayerWeaponModel.name && PlayerWeapons[i].IsHavePlayer == false)
                {
                    PlayerWeapons[i].IsHavePlayer = true;
                    PlayerWeapons[i].PlayerWeaponModel.SetActive(true);
                    Debug.Log("������ " + PlayerWeapons[i].PlayerWeaponModel.name + " ������ " + PlayerWeapons[i].IsHavePlayer);
                    Destroy(Weapons[0]);
                }
                else if (Weapons[0].name == PlayerWeapons[i].PlayerWeaponModel.name && PlayerWeapons[i].GunController.Magazine != PlayerWeapons[i].GunController.MaxMagazine && PlayerWeapons[i].IsHavePlayer == true)
                {
                    int RandomMagazine = UnityEngine.Random.Range(1, 3);
                    PlayerWeapons[i].GunController.Magazine += RandomMagazine;
                    Debug.Log("��������� " + RandomMagazine + " ��������� � " + PlayerWeapons[i].PlayerWeaponModel.name);
                    Destroy(Weapons[0]);
                }
                else
                {
                    PlayerWeapons[i].PlayerWeaponModel.SetActive(false);
                }
            }
        }
    }

    //void SelectWeapons()
    //{
    //    MouseScroll = Input.GetAxis("Mouse ScrollWheel");
    //    CurrentWeapon = WeaponSwitch;
    //    if (MouseScroll > 0)
    //    {
    //        if (WeaponSwitch >= PlayerWeapons.Length - 1)
    //        {
    //            WeaponSwitch = 0;
    //        }
    //        else
    //        {
    //            WeaponSwitch++;
    //        }
    //    }
    //    else if (MouseScroll < 0)
    //    {
    //        if (WeaponSwitch <= 0)
    //        {
    //            WeaponSwitch = PlayerWeapons.Length - 1;
    //        }
    //        else
    //        {
    //            WeaponSwitch--;
    //        }
    //    }
    //    if (CurrentWeapon != WeaponSwitch)
    //    {
    //        for (int i = 0; i < PlayerWeapons.Length; i++)
    //        {
    //            if (i == WeaponSwitch)
    //            {
    //                PlayerWeapons[i].PlayerWeaponModel.SetActive(true);
    //            }
    //            else
    //            {
    //                PlayerWeapons[i].PlayerWeaponModel.SetActive(false);
    //            }
    //        }
    //    }
    //}
    void SelectWeapons()
    {
        MouseScroll = Input.GetAxis("Mouse ScrollWheel");
        int initialWeaponSwitch = WeaponSwitch; 
        bool weaponSelected = false;
        if (MouseScroll > 0)
        {
            do
            {
                PlayerWeapons[WeaponSwitch].PlayerWeaponModel.SetActive(false);
                WeaponSwitch = (WeaponSwitch + 1) % PlayerWeapons.Length;
                if (PlayerWeapons[WeaponSwitch].IsHavePlayer)
                {
                    weaponSelected = true;
                }
                if (WeaponSwitch == initialWeaponSwitch)
                    break;

            } while (!weaponSelected);
            WeaponCheck();
        }
        else if (MouseScroll < 0)
        {
            do
            {
                PlayerWeapons[WeaponSwitch].PlayerWeaponModel.SetActive(false);
                WeaponSwitch = (WeaponSwitch - 1 + PlayerWeapons.Length) % PlayerWeapons.Length;
                if (PlayerWeapons[WeaponSwitch].IsHavePlayer)
                {
                    weaponSelected = true;
                }
                WeaponSwitchReset();
                if (WeaponSwitch == initialWeaponSwitch)
                    break;

            } while (!weaponSelected);
            WeaponCheck();
        }

    
        void WeaponSwitchReset()
        {
                if (WeaponSwitch < 0)
                {
                    WeaponSwitch = PlayerWeapons.Length - 1;

                }

        }
        void WeaponCheck()
        {

            if (weaponSelected)
            {
                PlayerWeapons[WeaponSwitch].PlayerWeaponModel.SetActive(true);
            }
        }

    }
}


[Serializable]
public class Weapon
{
    public GameObject PlayerWeaponModel;
    public GunController GunController;
    public bool IsHavePlayer;
}
