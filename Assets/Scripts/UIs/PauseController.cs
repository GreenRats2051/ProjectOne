using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject PauseMenuObject;
    public GameObject SettingsMenuObject;
    public GameObject AchievementMenuObject;
    public PlayerController PlayerController;
    public PlayerMeleeController MleeController;
    public PlayerGunController[] GunControllers;
    private bool PauseOpen;

    void Update()
    {
        OpenPause();
    }

    void OpenPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseOpen = !PauseOpen;
            if (PauseOpen)
            {
                PauseMenuObject.SetActive(true);
                SettingsMenuObject.SetActive(false);
                AchievementMenuObject.SetActive(false);
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
                PauseMenuObject.SetActive(false);
                SettingsMenuObject.SetActive(false);
                AchievementMenuObject.SetActive(false);
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

    public void Continue()
    {
        PauseOpen = false;
        PauseMenuObject.SetActive(false);
        SettingsMenuObject.SetActive(false);
        AchievementMenuObject.SetActive(false);
        PlayerController.enabled = true;
        MleeController.enabled = true;
        for (int i = 0; i < GunControllers.Length; i++)
        {
            GunControllers[i].enabled = true;
        }
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
