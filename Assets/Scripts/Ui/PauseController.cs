using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenuObject;
    [SerializeField]
    private GameObject settingsMenuObject;
    [SerializeField]
    private GameObject achievementMenuObject;
    [SerializeField]
    private PlayerInputLisener playerInputLisener;
    private bool pauseOpen;

    void Start()
    {
        playerInputLisener.enabled = true;
        Time.timeScale = 1;
    }

    public void OpenPause()
    {
        pauseOpen = !pauseOpen;
        if (pauseOpen)
        {
            pauseMenuObject.SetActive(true);
            settingsMenuObject.SetActive(false);
            achievementMenuObject.SetActive(false);
            playerInputLisener.enabled = false;
            Time.timeScale = 0;
        }
        else
        {
            pauseMenuObject.SetActive(false);
            settingsMenuObject.SetActive(false);
            achievementMenuObject.SetActive(false);
            playerInputLisener.enabled = true;
            Time.timeScale = 1;
        }
    }

    public void Continue()
    {
        pauseOpen = false;
        pauseMenuObject.SetActive(false);
        settingsMenuObject.SetActive(false);
        achievementMenuObject.SetActive(false);
        playerInputLisener.enabled = true;
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
