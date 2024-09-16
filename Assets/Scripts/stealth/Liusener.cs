using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liusener : MonoBehaviour
{

    public bool Iscrouch => _iscrouch;
    Crouch _crouch;
    [SerializeField]
    bool _iscrouch ;
    private void Start()
    {
        _crouch = new();
    }
    void Update()
    {
        _iscrouch = Input.GetKey(KeyCode.Space);

        if (_iscrouch)
        {
           // _crouch.InvokeCrouch(gameObject);
        }
    }
}
