using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MleeController : MonoBehaviour
{
    public LayerMask EnemyMask; //Маска врага
    public Transform AttackZone; //Зона атаки
    public Vector3 SizeAttackZone; //Размер зоны атаки
    private Collider[] Enemys; //Враги которых игрок атакует

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
                Debug.Log("Вы атаковали " + Enemys[i].name);
                Destroy(Enemys[i].gameObject);
            }
        }
    }
}
