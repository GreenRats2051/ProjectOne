using System.Collections.Generic;
using UnityEngine;

public class GenerateRooms : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private GameObject mainSpawnPoint;
    [SerializeField] private GameObject leftSpawnPoint;
    [SerializeField] private GameObject rightSpawnPoint;
    [SerializeField] private GameObject centerSpawnPoint;

    [Header("Return Points")]
    [SerializeField] private GameObject mainSpawnReturnPoint;
    [SerializeField] private GameObject leftSpawnReturnPoint;
    [SerializeField] private GameObject rightSpawnReturnPoint;
    [SerializeField] private GameObject centerSpawnReturnPoint;


    [Header("Room Settings")]
    [SerializeField] private GameObject roomPrefabT;
    [SerializeField] private GameObject roomPrefabF;
    [SerializeField] private GameObject roomPrefabThree;
    [SerializeField] private float spacingX;
    [SerializeField] private float spacingY;
    [SerializeField] private float spacingZ;
    [SerializeField] private bool canSpawn = true;
    [SerializeField] private int maxRooms = 20;

    private List<Vector3> occupiedPositions = new List<Vector3>();

    [SerializeField] private Room currentRoomPosition;
    private static int roomCount = 0;
    [SerializeField] private Floors floor;
    [SerializeField] private Floors floorNew;

    private static readonly object _lockObject = new object();

    private void Start()
    {


        if (canSpawn)
        {
            GenerateInitialRooms();
        }
    }

    private void GenerateInitialRooms()
    {
        switch (floor)
        {
            case Floors.Forward:
                SpawnRoom(Room.Center);
                break;
            case Floors.T_Variant:
                SpawnRoom(Room.Right);
                SpawnRoom(Room.Left);
                break;
            case Floors.ThreeWays:
                SpawnRoom(Room.Center);
                SpawnRoom(Room.Right);
                SpawnRoom(Room.Left);
                break;
        }
    }

    private void SpawnRoom(Room roomType)
    {
        lock (_lockObject)
        {
            if (roomCount >= maxRooms) return;
            floorNew = (Floors)Random.Range(0, 3);
            GameObject prefub = null;
            switch (floorNew)
            {
                case Floors.Forward:
                    prefub = roomPrefabF;
                    break;
                case Floors.T_Variant:
                    prefub = roomPrefabT;
                    break;
                case Floors.ThreeWays:
                    prefub = roomPrefabThree;
                    break;
            }

            Vector3 roomOffset = GetRoomOffset(roomType);

            Vector3 spawnPosition = transform.position + roomOffset;

            if (occupiedPositions.Contains(spawnPosition))
            {
                return;
            }

            GameObject newRoom = Instantiate(
                prefub,
                spawnPosition,
                Quaternion.identity
            );

            occupiedPositions.Add(spawnPosition);

            var newRoomScript = newRoom.GetComponent<GenerateRooms>();
            newRoomScript.SetRoomPosition(roomType, floorNew);

            LinkRooms(newRoomScript, roomType);
            roomCount++;
        }
    }


    private void LinkRooms(GenerateRooms newRoom, Room roomType)
    {
        switch (roomType)
        {
            case Room.Right:
                rightSpawnPoint = newRoom.mainSpawnPoint;
                newRoom.mainSpawnReturnPoint = rightSpawnReturnPoint;
                break;

            case Room.Left:
                leftSpawnPoint = newRoom.mainSpawnPoint;
                newRoom.mainSpawnReturnPoint = leftSpawnReturnPoint;
                break;

            case Room.Center:
                centerSpawnPoint = newRoom.mainSpawnPoint;
                newRoom.mainSpawnReturnPoint = centerSpawnReturnPoint;
                break;
        }
    }
    public GameObject SetDoorEnter(Room room)
    {

        switch (room)
        {
            case Room.Right:
                return rightSpawnPoint;
            case Room.Left:
                return leftSpawnPoint;
            case Room.Center:
                return centerSpawnPoint;
            default:
                return null;
        }
    }
    private Vector3 GetRoomOffset(Room roomType)
    {
        float  offsetZ = 0;

        switch (roomType)
        {
            case Room.Right:
                offsetZ = -1;
                break;

            case Room.Left:
                offsetZ = 1;
                break;

            case Room.Center:
                break;
        }

        return new Vector3(spacingX, 0, spacingZ * offsetZ);
    }

    public void SetRoomPosition(Room room, Floors floors)
    {
        currentRoomPosition = room;
        floor = floors;
    }

    public GameObject GetSpawnPoint(Room room)
    {
        return room switch
        {
            Room.Right => rightSpawnPoint,
            Room.Left => leftSpawnPoint,
            Room.Center => centerSpawnPoint,
            _ => null
        };
    }
}