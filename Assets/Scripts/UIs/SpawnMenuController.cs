using UnityEngine;

public class SpawnMenuController : MonoBehaviour
{
    public GameObject SpawnMenuObject;
    public PlayerController PlayerController;
    public PlayerMeleeController MleeController;
    public PlayerGunController[] GunControllers;
    private bool SpawnMenuOpen;

    void Update()
    {
        OpenSpawnMenu();
    }

    void OpenSpawnMenu()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SpawnMenuOpen = !SpawnMenuOpen;
            if (SpawnMenuOpen)
            {
                SpawnMenuObject.SetActive(true);
                PlayerController.enabled = false;
                MleeController.enabled = false;
                for (int i = 0; i < GunControllers.Length; i++)
                {
                    GunControllers[i].enabled = false;
                }
                Time.timeScale = 0;
            }
            else
            {
                SpawnMenuObject.SetActive(false);
                PlayerController.enabled = true;
                MleeController.enabled = true;
                for (int i = 0; i < GunControllers.Length; i++)
                {
                    GunControllers[i].enabled = true;
                }
                Time.timeScale = 1;
            }
        }
    }

    public void SpawnObject(GameObject GameObject)
    {
        GameObject SpawnObject = Instantiate(GameObject, PlayerController.MousePoint.position, Quaternion.identity);
        SpawnObject.name = GameObject.name;
        if (SpawnObject.tag == "Weapon")
        {
            SpawnObject.transform.rotation = Quaternion.EulerRotation(0, Random.Range(-45, 45), 90);
        }
    }
}
