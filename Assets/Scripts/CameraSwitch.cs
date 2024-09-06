using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera thirdPersonCamera;
    public CinemachineVirtualCamera framingCamera;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)) 
        {

            framingCamera.Priority = 20;
            thirdPersonCamera.Priority = 10;
        }
        else
        {

            thirdPersonCamera.Priority = 20;
            framingCamera.Priority = 10;
        }
    }
}