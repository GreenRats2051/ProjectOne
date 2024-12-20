using System.Collections.Generic;
using UnityEngine;

public class GenerateRooms : MonoBehaviour
{
    [SerializeField] private GameObject mainSpawnPoint;
    [SerializeField] private GameObject leftSpawnPoint;
    [SerializeField] private GameObject rightSpawnPoint;
    [SerializeField] private GameObject centerSpawnPoint;
    [Header("Get points")]
    [SerializeField] private GameObject mainReturnPoint;
    [SerializeField] private GameObject leftReturnPoint;
    [SerializeField] private GameObject rightReturnPoint;
    [SerializeField] private GameObject centerReturnPoint;

    [Header("Room Settings")]
    [SerializeField] private GameObject roomPrefabT;
    [SerializeField] private GameObject roomPrefabF;
    [SerializeField] private GameObject roomPrefabThree;
    [SerializeField] private float spacingX;
    [SerializeField] private float spacingY;
    [SerializeField] private float spacingZ;
    [SerializeField] private bool canSpawn = true;
    [SerializeField] private int maxRooms = 20;
    private GameObject finalRoom;

    private List<Vector3> occupiedPositions = new List<Vector3>();

    [SerializeField] private Room currentRoomPosition;
    private static bool finalRoomSet = false;
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
    public void GetList(List<Vector3> listget)
    {
        occupiedPositions = listget;
    }
    public void GetFinishRoom(GameObject obj)
    {
        finalRoom = obj;
    }

    private void SpawnRoom(Room roomType)
    {
        lock (_lockObject)
        {
            if (roomCount >= maxRooms - 1)
            {
                if (!finalRoomSet)
                {
                    finalRoom = gameObject;
                    SetFinalRoom();
                }
                return;
            }


            floorNew = (Floors)Random.Range(0, 3);
            GameObject prefab = null;
            switch (floorNew)
            {
                case Floors.Forward:
                    prefab = roomPrefabF;
                    break;
                case Floors.T_Variant:
                    prefab = roomPrefabT;
                    break;
                case Floors.ThreeWays:
                    prefab = roomPrefabThree;
                    break;
            }

            Vector3 roomOffset = GetRoomOffset(roomType);

            Vector3 spawnPosition = transform.position + roomOffset;

            if (occupiedPositions.Contains(spawnPosition))
            {
                return;
            }

            GameObject newRoom = Instantiate(
                prefab,
                spawnPosition,
                Quaternion.identity
            );

            occupiedPositions.Add(spawnPosition);

            var newRoomScript = newRoom.GetComponent<GenerateRooms>();
            newRoomScript.GetList(occupiedPositions);
            newRoomScript.SetRoomPosition(roomType, floorNew);

            LinkRooms(newRoomScript, roomType);
            roomCount++;
  

        }
    }
    

    public void SetFinalRoom()
    {
        lock (_lockObject)
        {
            if (finalRoomSet) return; 
            finalRoomSet = true;              }

        foreach (Door door in GetComponentsInChildren<Door>())
        {
            door.SetFinal(true); 
        }
    }

    private void LinkRooms(GenerateRooms newRoom, Room roomType)
    {
        switch (roomType)
        {
            case Room.Right:
                rightReturnPoint = newRoom.mainSpawnPoint;
                newRoom.mainReturnPoint = rightSpawnPoint;
                break;

            case Room.Left:
                leftReturnPoint = newRoom.mainSpawnPoint;
                newRoom.mainReturnPoint = leftSpawnPoint;
                break;

            case Room.Center:
                centerReturnPoint = newRoom.mainSpawnPoint;
                newRoom.mainReturnPoint = centerSpawnPoint;
                break;
        }
    }

    public GameObject SetDoorEnter(Room room)
    {
        switch (room)
        {
            case Room.Right:
                return rightReturnPoint;
            case Room.Left:
                return leftReturnPoint;
            case Room.Center:
                return centerReturnPoint;
            case Room.Return:
                return mainReturnPoint;
            default:
                return null;
        }
        
    }

    private Vector3 GetRoomOffset(Room roomType)
    {
        float offsetZ = 0;

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