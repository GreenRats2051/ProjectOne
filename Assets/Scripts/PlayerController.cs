using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera MainCamera; //Êàìåðà èãðîêà
    public LayerMask WeaponMask; //Ìàñêà îðóæèÿ
    public Transform PlayerModel; //Ìîäåëü èãðîêà
    public Transform MousePoint; //Òî÷êà ìûøè äëÿ âèäèìîñòè
    public Vector3 Point; //Òî÷êà ìûøè
    private Vector2 InputAction; //Íàæàòèå ââåðõ, âíèç, âëåâî, âïðàî
    private Collider[] Weapons; //Áëèæàéøåå îðóæèå
    public Rigidbody Rigidbody; //Rigidbody èãðîêà
    public Weapon[] PlayerWeapons; //Îðóæèå èãðîêà
    public float Speed; //Ñêîðîñòü èãðîêà
    public float RadiusCheckWeapon; //Ðàäèóñ ïîäíÿòèÿ îðóæèÿ

    void Start()
    {
        
    }

    void Update()
    {
        Movement(); //Ïåðåäâèæåíèå
        WeaponPickUp(); //Ïîäíÿòèå îðóæèÿ
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
                    Debug.Log("Îðóæèå " + PlayerWeapons[i].PlayerWeaponModel.name + " òåïåðü " + PlayerWeapons[i].IsHavyPlayer);
                    Destroy(Weapons[0]);
                }
                else if (Weapons[0].name == PlayerWeapons[i].PlayerWeaponModel.name && PlayerWeapons[i].IsHavyPlayer == true)
                {
                    Debug.Log("Äîáàâëåíû ïàòðîíû ê " + PlayerWeapons[i].PlayerWeaponModel.name);
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
    public bool IsHavyPlayer;
}
