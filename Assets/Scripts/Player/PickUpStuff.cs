using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpStuff : MonoBehaviour
{
    [SerializeField] PlayerSelectWeapon selectWeapon;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Weapon" && selectWeapon != null)
        {
            for (int i = 0; i < selectWeapon.playerGun.Length; i++)
            {
                if (collider.name == selectWeapon.playerGun[i].Weapon.name && selectWeapon.playerGun[i].IsHavePlayer == false)
                {
                    Debug.Log("Player pick up: " + selectWeapon.playerGun[i].Weapon.name);
                    selectWeapon.playerGun[i].IsHavePlayer = true;
                    selectWeapon.playerGun[i].Weapon.SetActive(true);
                    Destroy(collider.gameObject);
                }
                else if (collider.name == selectWeapon.playerGun[i].Weapon.name && selectWeapon.playerGun[i].IsHavePlayer == true)
                {
                    int RandomMagazine = Random.Range(30, 50);
                    Debug.Log("Player pick up: " + RandomMagazine + "ammo");
                    selectWeapon.playerGun[i].GunController.Magazine += RandomMagazine;
                    Destroy(collider.gameObject);
                }
            }
        }
    }
}
