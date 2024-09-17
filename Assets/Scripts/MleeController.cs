using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MleeController : MonoBehaviour
{
    public LayerMask EnemyMask; //Маска врага
    public Transform AttackZone; //Зона атаки
    public Vector3 SizeAttackZone; //Размер зоны атаки
    private Collider[] Enemys; //Враги которых игрок атакует
    public TMP_Text WeaponName; //Имя оружия
    public Slider AmmoSlider; //Слайдер патрона
    public Slider MagazineSlider; //Слайдер магазина

    void Start()
    {
        
    }

    void Update()
    {
        Attack();
        MleeUi();
    }

    void Attack()
    {
        Enemys = Physics.OverlapBox(AttackZone.position, SizeAttackZone, Quaternion.identity, EnemyMask);
        if (Input.GetKey(KeyCode.Mouse0) && Enemys.Length != 0)
        {
            for (int i = 0; i < Enemys.Length; i++)
            {
                Debug.Log("Вы атаковали " + Enemys[i].name);
                Destroy(Enemys[i].gameObject);
            }
        }
    }

    void MleeUi()
    {
        if (gameObject.activeSelf)
        {
            WeaponName.text = transform.name;
            AmmoSlider.value = 0;
            AmmoSlider.maxValue = 0;
            MagazineSlider.value = 0;
            MagazineSlider.maxValue = 0;
        }
    }
}
