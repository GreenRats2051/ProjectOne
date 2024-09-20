using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class LisenerActiveButton : MonoBehaviour
{
    public event Action OnDroneActivated;
    public event Action OnDroneDeactivated;
    public event Action OnSmokeDrop;
    public bool Iscrouch => _iscrouch;

    private int smokesValue = 10;// убрать в контролер потом
    private int DefultValue = 10;// убрать в контролер потом
    [SerializeField]
    private Slider smokesSlider ;// убрать в контролер потом


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

        if (Input.GetKeyDown(KeyCode.G)&& smokesValue!=0)
        {
            smokesValue--;
            smokesSlider.value -= 1;
            _smoke.spawn(_playerController.MousePoint.position, (gameObject.transform.position + new Vector3(0, 2, 0)));
        }

        if (_iscrouch)
        {
            _crouch.InvokeCrouch(gameObject,2);
        }
        else
        {
            _crouch.InvokeCrouch(gameObject,4);
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
