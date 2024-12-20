using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Door door; 
    private bool isPlayerInRange = false;
    private GameObject player;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private string scene;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInRange = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInRange = false;
            player = null;
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.G)) 
        {
            if (door != null && door.isConnected) 
            {
                TeleportPlayer(); 
            }
            if (door.isFinal)
            {
                SceneManager.LoadScene(scene);
            }
        }
    }

    private void TeleportPlayer()
    {
        Vector3 teleportPosition = door.GetTeleportPosition();

        if (player != null)
        {
            player.transform.position = teleportPosition; 
        }
    }
}
