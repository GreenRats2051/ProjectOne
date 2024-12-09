using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field:SerializeField] public Transform playerTransform {  get; private set; }
    [field:SerializeField] public Rigidbody playerRb {  get; private set; }
    [field: SerializeField] public Transform playerCurse {  get; private set; }
    [field: SerializeField] public int hp {  get; private set; }
    [field: SerializeField] public int hpMax {  get; private set; }
    [field: SerializeField] public bool dead {  get; private set; }
    private void Awake()
    {

        ListPlayer.Inst.addPlayer(this.gameObject);
    }
    public void GetHit(int damage)
    {
        hp -= damage;
        if (hp == 0)
        {
            dead = true;
        }
    }
    public void Healing(int addhp)
    {
        if (hp <= hpMax)
        {
            hp += addhp;
        }

    }
    
}
