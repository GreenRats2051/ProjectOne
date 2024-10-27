using TMPro;
using UnityEngine;

public class PlayerMeleeController : MonoBehaviour
{
    [SerializeField]
    private LayerMask EnemyMask;
    [SerializeField]
    private Transform AttackZone;
    [SerializeField]
    private Vector3 SizeAttackZone;
    [SerializeField]
    private TMP_Text AmmoAndMagazineText;
    private Collider[] EnemysCollider;

    void Update()
    {
        Attack();
        MleeUi();
    }

    void Attack()
    {
        EnemysCollider = Physics.OverlapBox(AttackZone.position, SizeAttackZone, Quaternion.identity, EnemyMask);
        if (Input.GetKey(KeyCode.Mouse0) && EnemysCollider.Length != 0)
        {
            for (int i = 0; i < EnemysCollider.Length; i++)
            {
                Destroy(EnemysCollider[i].gameObject);
            }
        }
    }

    void MleeUi()
    {
        if (gameObject.activeSelf)
        {
            AmmoAndMagazineText.text = "None";
        }
    }
}
