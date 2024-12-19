using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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

    private bool _isdroneReady = true;
    [SerializeField]
    private Slider drone;
    [SerializeField]
    private bool droneTrack = true;

    protected virtual void Start()
    {
        drone.maxValue = droneLivetime;
        drone.value = drone.maxValue;
        
    }

    protected virtual void ActionDrone(){}
    internal virtual void ActionDrone(float valueF1,float valueF2, bool boolValue1 )
    {
        gameObject.transform.position += new Vector3(valueF1, 0, valueF2) * Time.deltaTime;
    }
    public bool IsDroneAllive()
    {
        return instDrone != null;
    }
    public void CreateDrone()
    {

        if (_isdroneReady)
        {
            drone.maxValue = droneLivetime;
            drone.value = drone.maxValue;
            instDrone = Instantiate(prefubDrone, target.transform.position + new Vector3(1, 1, 0), target.transform.rotation);
            StartCoroutine(DestroyAfterLifetime());
        }
        if (droneTrack)
        {
            instDrone.transform.parent = target.transform;
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
    private void DestroyDrone()
    {
        if (instDrone != null)
        {
            drone.value = 0;
            Destroy(instDrone);
            StartCoroutine(Cooldown());
        }
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