using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DronrBase : MonoBehaviour
{
    [SerializeField]
    private int droneLivetime = 4; 
    [SerializeField]
    private int droneCoolDownTime = 4; 
    [SerializeField]
    private GameObject prefubDrone;
    [SerializeField]
    private GameObject target;
    protected GameObject instDrone;
    protected abstract void Interacting();
    private LisenerActiveButton _listener;
    private bool _isdroneReady = true;

    protected virtual void Start()
    {
        if (TryGetComponent(out LisenerActiveButton lisener))
        {
            _listener = lisener;
            _listener.OnDroneActivated += Interacting;
            _listener.OnDroneActivated += CreateDrone;
            _listener.OnDroneDeactivated += DestroyDrone;
        }
    }

    protected virtual void OnDestroy()
    {
        if (_listener != null)
        {
            _listener.OnDroneActivated -= Interacting;
            _listener.OnDroneActivated -= CreateDrone;
            _listener.OnDroneDeactivated -= DestroyDrone;
        }
    }

    void CreateDrone()
    {
        if (_isdroneReady)
        {
            Debug.Log("Creating drone...");
            instDrone = Instantiate(prefubDrone, target.transform.position + new Vector3(1, 1, 0), target.transform.rotation);
            instDrone.transform.parent = target.transform;
            StartCoroutine(DestroyAfterLifetime());
        }

    }

    private void DestroyDrone()
    {
        if (instDrone != null)
        {

            Destroy(instDrone);
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(droneLivetime);
        DestroyDrone();
    }

    private IEnumerator Cooldown()
    {
        _isdroneReady = false;
        yield return new WaitForSeconds(droneCoolDownTime);
        _isdroneReady = true; 
    }
}