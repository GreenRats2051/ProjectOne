using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame(int numberLevel)
    {
        SceneManager.LoadScene(numberLevel);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
