using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Room room;
    [SerializeField]private Vector3 tpPosition;
    public bool isConnected = false;
    public bool isFinal = false;
    [SerializeField] GameObject targetRoom;
    [SerializeField] private Renderer rendererDor;

    private void Start()
    {
        GenerateRooms generator = GetComponentInParent<GenerateRooms>();
         targetRoom = generator.SetDoorEnter(room);

        if (targetRoom != null)
        {
            tpPosition = targetRoom.transform.position;
            isConnected = true;
        }
        else
        {
            isConnected = false;
        }

        if (!isConnected)
        {
            rendererDor.material.color = Color.red;
        }
    }

    public void SetState(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetConnected(bool connected)
    {
        isConnected = connected;

        if (!connected)
        {
            rendererDor.material.color = Color.red;
        }
    }

    public void SetFinal(bool final)
    {
        isFinal = final;
        if (isFinal)
        {
            rendererDor.material.color = Color.yellow; 
        }
    }

    public Vector3 GetTeleportPosition()
    {
        return tpPosition;
    }
}
