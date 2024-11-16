using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenuObject;
    [SerializeField]
    private GameObject settingsMenuObject;
    [SerializeField]
    private GameObject achievementMenuObject;
    [SerializeField]
    private PlayerInputLisener playerInputLisener;
    private bool isOpen;

    void Start()
    {
        playerInputLisener.enabled = true;
        Time.timeScale = 1;
    }

    public void OpenPause()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            playerInputLisener.ActivateScripts(false);
            pauseMenuObject.SetActive(true);
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else
        {
            playerInputLisener.ActivateScripts(true);
            pauseMenuObject.SetActive(false);
            Cursor.visible = false;
            Time.timeScale = 1;
        }
        settingsMenuObject.SetActive(false);
        achievementMenuObject.SetActive(false);
    }

    public void Return()
    {
        isOpen = false;
        pauseMenuObject.SetActive(false);
        settingsMenuObject.SetActive(false);
        achievementMenuObject.SetActive(false);
        playerInputLisener.ActivateScripts(true);
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitInMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
