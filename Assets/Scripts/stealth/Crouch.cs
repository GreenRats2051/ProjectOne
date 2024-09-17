using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch 
{
    public void InvokeCrouch(GameObject gameObject)
    {
        gameObject.GetComponent<PlayerController>().Speed /= 2;
    }
}
