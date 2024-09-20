using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch 
{
    public void InvokeCrouch(GameObject gameObject,int value)
    {
        gameObject.GetComponent<PlayerController>().Speed = value;
    }
}
