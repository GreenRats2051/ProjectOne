using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DronrBase : MonoBehaviour
{
    [SerializeField]
    private int droneLivetime;    
    [SerializeField]
    private int droneCoolDownTime;
    [SerializeField]
    private GameObject prefubDrone;
    [SerializeField]
    private GameObject target;
    private bool _isActiv;
    GameObject instDrone;
    protected abstract void Interacting();
    private LisenerActiveButton _listener;
    private bool _isdroneReady;

    protected virtual void Start()
    {
        if(TryGetComponent<LisenerActiveButton>(out LisenerActiveButton lisener))
        {
            _listener = lisener;
            _listener.OnDroneActivated += CreateDrone;
            _listener.OnDroneDeactivated += DestroyDrone;
        }
    }

    protected virtual void OnDestroy()
    {
        if (_listener != null)
        {
            _listener.OnDroneActivated -= CreateDrone;
            _listener.OnDroneDeactivated -= DestroyDrone;
        }
    }

    void CreateDrone()
    {
        if (_isdroneReady)
        {
            instDrone = Instantiate(prefubDrone, target.transform.position + new Vector3(1, 0, 0), target.transform.rotation);
            instDrone.transform.parent = target.transform;
            DestroyAfterLifetime();
        }

    }
    private void DestroyDrone()
    {
        if (instDrone != null)
        {
            Destroy(instDrone);
        }
        StartCoroutine(Cooldown());
    }

    private IEnumerator DestroyAfterLifetime()
    {
        _isdroneReady = false;
        yield return new WaitForSeconds(droneLivetime);
        DestroyDrone();
    }
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(droneCoolDownTime);
        _isdroneReady = true;
    }
}
