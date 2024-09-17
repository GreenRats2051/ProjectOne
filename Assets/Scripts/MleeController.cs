using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MleeController : MonoBehaviour
{
    public LayerMask EnemyMask; //����� �����
    public Transform AttackZone; //���� �����
    public Vector3 SizeAttackZone; //������ ���� �����
    private Collider[] Enemys; //����� ������� ����� �������

    void Start()
    {
        
    }

    void Update()
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
}
