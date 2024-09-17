using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MleeController : MonoBehaviour
{
    public LayerMask EnemyMask; //����� �����
    public Transform AttackZone; //���� �����
    public Vector3 SizeAttackZone; //������ ���� �����
    private Collider[] Enemys; //����� ������� ����� �������
    public TMP_Text WeaponName; //��� ������
    public Slider AmmoSlider; //������� �������
    public Slider MagazineSlider; //������� ��������

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
                Debug.Log("�� ��������� " + Enemys[i].name);
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
