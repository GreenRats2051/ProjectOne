using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Room room;
    private Vector3 tpPosition;
    private void Start()
    {
        tpPosition = GetComponentInParent<GenerateRooms>().SetDoorEnter(room).transform.position;
    }
}
