using System;
using TMPro;
using UnityEngine;

public class PlayerSelectWeaponController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text Ammo;
    [SerializeField]
    private Gun[] playerGun;
    public int weaponSwitch;
    private int currentWeaponValue
    {
        get => currentWeapon;
        set
        {
            currentWeapon = value;
            if (playerGun[currentWeapon].GunController == null)
            {
                Ammo.text = "None";
            }
            else
            {
                Ammo.text = playerGun[currentWeapon].GunController.Ammo + "/" + playerGun[currentWeapon].GunController.Magazine;
            }
        }
    }
    private int currentWeapon;

    public void SelectWeapon(float mouseScrole)
    {
        currentWeaponValue = weaponSwitch;
        if (mouseScrole > 0)
        {
            if (weaponSwitch >= playerGun.Length - 1)
            {
                weaponSwitch = 0;
            }
            else
            {
                weaponSwitch++;
            }
        }
        else if (mouseScrole < 0)
        {
            if (weaponSwitch <= 0)
            {
                weaponSwitch = playerGun.Length - 1;
            }
            else
            {
                weaponSwitch--;
            }
        }
        if (currentWeaponValue != weaponSwitch)
        {
            for (int i = 0; i < playerGun.Length; i++)
            {
                if (i == weaponSwitch && playerGun[i].IsHavePlayer == true)
                {
                    playerGun[i].Weapon.SetActive(true);
                }
                else
                {
                    playerGun[i].Weapon.SetActive(false);
                }
            }
        }
    }
}

[Serializable]
public class Gun
{
    public GameObject Weapon;
    public PlayerGunController GunController;
    public bool IsHavePlayer;
}
