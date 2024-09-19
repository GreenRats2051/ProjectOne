using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LisenerActiveButton : MonoBehaviour
{
    public event Action OnDroneActivated;
    public event Action OnDroneDeactivated;
    public event Action OnSmokeDrop;
    public bool Iscrouch => _iscrouch;


    private bool _isActivateDrone;
    private bool _isChange;
    private bool _isDrop;
    private Crouch _crouch;
    [SerializeField]
    private bool _iscrouch ;
    [SerializeField]
    Smoke _smoke;
    [SerializeField]
    PlayerController _playerController;
    private void Start()
    {
        _crouch = new();
    }
    void Update()
    {
        _iscrouch = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.G))
        {
            _smoke.spawn(_playerController.MousePoint.position, (gameObject.transform.position + new Vector3(0, 2, 0)));
        }

        if (_iscrouch)
        {
           // _crouch.InvokeCrouch(gameObject);
        }




        if (Input.GetKeyDown(KeyCode.Q))
        {
            _isActivateDrone = !_isActivateDrone;
        }
        if (_isChange!= _isActivateDrone)
        {
            if (_isActivateDrone)
                OnDroneActivated?.Invoke();
            else
                OnDroneDeactivated?.Invoke();
            _isChange = _isActivateDrone;
        }

        
    }

}
