using UnityEngine;

public class DeveloperMenu : MonoBehaviour
{
    [SerializeField]
    private Vector3 coordinates;
    [SerializeField]
    private GameObject developerMenu;
    [SerializeField]
    private PlayerInputLisener playerInputLisener;
    private bool isOpen;

    public void OpenDeveloperMenu()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            playerInputLisener.ActivateScripts(false);
            developerMenu.SetActive(true);
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else
        {
            playerInputLisener.ActivateScripts(true);
            developerMenu.SetActive(false);
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }

    public void CoordinatesX(string X)
    {
        coordinates.x = float.Parse(X);
    }

    public void CoordinatesY(string Y)
    {
        coordinates.y = float.Parse(Y);
    }

    public void CoordinatesZ(string Z)
    {
        coordinates.z = float.Parse(Z);
    }

    public void SpawnObject(GameObject gameObject)
    {
        Instantiate(gameObject, coordinates, Quaternion.identity);
    }
}
