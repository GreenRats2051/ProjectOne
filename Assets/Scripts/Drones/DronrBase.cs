using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    Slider drone;
    protected virtual void Start()
    {
        drone.maxValue = droneLivetime;
        drone.value = drone.maxValue;
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
            drone.maxValue = droneLivetime;
            drone.value = drone.maxValue;
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
             drone.value = 0;
            Destroy(instDrone);
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator DestroyAfterLifetime()
    {
        for (int i = 0; i < droneLivetime; i++)
        {
            drone.value -= 1;
            yield return new WaitForSeconds(1);
        }
        DestroyDrone();
    }

    private IEnumerator Cooldown()
    {
        drone.maxValue = droneCoolDownTime;
        drone.value = 0;
        _isdroneReady = false;
        for(int i =0; i < droneCoolDownTime; i++)
        {
            drone.value += 1;
            yield return new WaitForSeconds(1);
        }

        
        _isdroneReady = true; 
    }
}