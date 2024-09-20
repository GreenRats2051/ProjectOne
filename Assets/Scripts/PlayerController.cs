using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Camera MainCamera; 
    public Transform PlayerModel;
    public Transform MousePoint; 
    private Vector2 InputAction; 
    public Slider ArmorSlider; 
    public Slider HealthSlider; 
    public Rigidbody Rigidbody; 
    public Weapon[] PlayerWeapons; 
    public int Armor; 
    public int MaxArmor; 
    public int Health; 
    public int MaxHealth; 
    private int WeaponSwitch; 
    private int CurrentWeapon; 
    private float MouseScroll; 
    public float Speed; 


    void Start()
    {
        ArmorSlider.maxValue = MaxArmor;
        HealthSlider.maxValue = MaxHealth;
    }

    void Update()
    {
        Movement(); 
        SelectWeapons();
        ChangeSliderPlayerLive();
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

    void OnTriggerEnter(Collider Collider)
    {
        if (Collider.tag == "Weapon")
        {
            for (int i = 0; i < PlayerWeapons.Length; i++)
            {
                if (Collider.name == PlayerWeapons[i].PlayerWeaponModel.name && PlayerWeapons[i].IsHavePlayer == false)
                {
                    PlayerWeapons[i].IsHavePlayer = true;
                    PlayerWeapons[i].PlayerWeaponModel.SetActive(true);
                    Destroy(Collider.gameObject);
                }
                else if (Collider.name == PlayerWeapons[i].PlayerWeaponModel.name && PlayerWeapons[i].GunController.Magazine != PlayerWeapons[i].GunController.MaxMagazine && PlayerWeapons[i].IsHavePlayer == true)
                {
                    int RandomMagazine = UnityEngine.Random.Range(1, 3);
                    PlayerWeapons[i].GunController.Magazine += RandomMagazine;
                    Destroy(Collider.gameObject);
                }
                else
                {
                    PlayerWeapons[i].PlayerWeaponModel.SetActive(false);
                }
            }
        }
    }
    public void Healing(int healvalue)
    {
        if(Health + healvalue < MaxHealth)
        {
            Health += healvalue;
        }        


    }
    public void ChangeSliderPlayerLive()
    {
        if(ArmorSlider.value!= Armor)
        {
            ArmorSlider.value = Armor;
        }
        if (HealthSlider.value != Health)
        {
            HealthSlider.value = Health;
        }
    }
    public void GetHit(int damage)
    {
        if (Armor != 0&&damage<Armor)
        {
            Armor -= damage;
        }
        else if(Armor != 0)
        {
            damage -= Armor;
            Armor = 0;
            Health -= damage;
        }
        else
        {
            Health -= damage;
        }

    }
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
