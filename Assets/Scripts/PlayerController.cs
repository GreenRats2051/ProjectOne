using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Camera MainCamera; //Камера игрока
    public Transform PlayerModel; //Модель игрока
    public Transform MousePoint; //Точка мыши для видимости
    private Vector2 InputAction; //Нажатие вверх, вниз, влево, впрао
    public Slider ArmorSlider; //Слайдер брони
    public Slider HealthSlider; //Слайдер здоровья
    public Rigidbody Rigidbody; //Rigidbody игрока
    public Weapon[] PlayerWeapons; //Оружие игрока
    public int Armor; //Броня игрока
    public int MaxArmor; //Максимальная броня игрока
    public int Health; //Здоровье игрока
    public int MaxHealth; //Максимальное здоровье игрока
    private int WeaponSwitch; //
    private int CurrentWeapon; //Текущее выбранное оружие
    private float MouseScroll; //Прокручивание колеса мыши
    public float Speed; //Скорость игрока

    void Start()
    {
        ArmorSlider.maxValue = MaxArmor;
        HealthSlider.maxValue = MaxHealth;
    }

    void Update()
    {
        Movement(); //Передвижение
        SelectWeapons(); //Смена оружия
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
            MousePoint.position = new Vector3(RaycastHit.point.x, 1, RaycastHit.point.z);
        }
        PlayerModel.LookAt(new Vector3(MousePoint.position.x, transform.position.y, MousePoint.position.z));
    }

    void SelectWeapons()
    {
        MouseScroll = Input.GetAxis("Mouse ScrollWheel");
        CurrentWeapon = WeaponSwitch;
        if (MouseScroll > 0)
        {
            if (WeaponSwitch >= PlayerWeapons.Length - 1)
            {
                WeaponSwitch = 0;
            }
            else
            {
                WeaponSwitch++;
            }
        }
        else if (MouseScroll < 0)
        {
            if (WeaponSwitch <= 0)
            {
                WeaponSwitch = PlayerWeapons.Length - 1;
            }
            else
            {
                WeaponSwitch--;
            }
        }
        if (CurrentWeapon != WeaponSwitch)
        {
            for (int i = 0; i < PlayerWeapons.Length; i++)
            {
                if (i == WeaponSwitch)
                {
                    PlayerWeapons[i].PlayerWeaponModel.SetActive(true);
                }
                else
                {
                    PlayerWeapons[i].PlayerWeaponModel.SetActive(false);
                }
            }
        }
    }

    void OnTriggerEnter(Collider Collider)
    {
        if (Collider.tag == "Weapon")
        {
            for (int i = 0; i < PlayerWeapons.Length; i++)
            {
                if (Collider.name == PlayerWeapons[i].PlayerWeaponModel.name && PlayerWeapons[i].IsHavyPlayer == false)
                {
                    PlayerWeapons[i].IsHavyPlayer = true;
                    PlayerWeapons[i].PlayerWeaponModel.SetActive(true);
                    Debug.Log("Оружие " + PlayerWeapons[i].PlayerWeaponModel.name + " теперь " + PlayerWeapons[i].IsHavyPlayer);
                    Destroy(Collider.gameObject);
                }
                else if (Collider.name == PlayerWeapons[i].PlayerWeaponModel.name && PlayerWeapons[i].GunController.Magazine != PlayerWeapons[i].GunController.MaxMagazine && PlayerWeapons[i].IsHavyPlayer == true)
                {
                    int RandomMagazine = UnityEngine.Random.Range(1, 3);
                    PlayerWeapons[i].GunController.Magazine += RandomMagazine;
                    Debug.Log("Добавлено " + RandomMagazine + " магазинов к " + PlayerWeapons[i].PlayerWeaponModel.name);
                    Destroy(Collider.gameObject);
                }
                else
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
