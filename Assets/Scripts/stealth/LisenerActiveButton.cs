using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LisenerActiveButton : MonoBehaviour
{
    public event Action OnDroneActivated;
    public event Action OnDroneDeactivated;
    private bool _isActivateDrone;
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
        bool newIsActivateDrone = Input.GetKey(KeyCode.Q);


        if (newIsActivateDrone != _isActivateDrone)
        {
            _isActivateDrone = newIsActivateDrone;

            if (_isActivateDrone)
                OnDroneActivated?.Invoke();
            else
                OnDroneDeactivated?.Invoke();
        }
    }

}
