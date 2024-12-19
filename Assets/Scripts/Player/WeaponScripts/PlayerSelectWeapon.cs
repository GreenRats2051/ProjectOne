using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectWeapon : MonoBehaviour
{
    [SerializeField]
    private TMP_Text Ammo;
    [SerializeField]
    private TMP_Text Magazine;
    [SerializeField]
    private Image currentIconWeapon;
    public Gun[] playerGun;
    public int weaponSwitch;
    private int currentWeaponValue
    {
        get => currentWeapon;
        set
        {
            currentWeapon = value;
            if (playerGun[currentWeapon].GunController == null || playerGun[currentWeapon].IsHavePlayer == false)
            {
                currentIconWeapon.sprite = null;
                currentIconWeapon.color = new Color(255, 255, 255, 0);
                Ammo.text = "";
                Magazine.text = "";
            }
            else if (playerGun[currentWeapon].GunController != null && playerGun[currentWeapon].IsHavePlayer == true)
            {
                currentIconWeapon.sprite = playerGun[currentWeapon].iconWeapon;
                currentIconWeapon.color = new Color(255, 255, 255, 255);
                Ammo.text = playerGun[currentWeapon].GunController.Ammo.ToString();
                Magazine.text = playerGun[currentWeapon].GunController.Magazine.ToString();
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
    public Sprite iconWeapon;
    public PlayerGun GunController;
    public bool IsHavePlayer;
}
