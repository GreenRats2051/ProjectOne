using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field:SerializeField] public Transform playerTransform {  get; private set; }
    [field:SerializeField] public Rigidbody playerRb {  get; private set; }
    [field: SerializeField] public Transform playerCurse {  get; private set; }
    private void Awake()
    {

        ListPlayer.Inst.addPlayer(gameObject);
    }
  
    
}
