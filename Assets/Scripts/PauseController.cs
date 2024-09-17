using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public GameObject PauseMenu; //���� �����
    public GameObject SettingsMenu; //���� ��������
    public TMP_Dropdown Resolution; //���������� ������
    public TMP_Dropdown GraphicsQuality; //���������� �������
    private Resolution[] AllResolution; //��� ������� ������
    public PlayerController PlayerController; //PlayerController ������
    public MleeController MleeController; //MleeController ������
    public GunController[] GunControllers; //GunController ������
    private int CurrentResolution; //������� ������ ������
    List<string> Options = new List<string>(); //
    private bool PauseOpen; //������� �� �����
    private bool SettingsOpen; //������� �� ���������
    private bool IsFullScreen; //������ �� �����

    void Start()
    {
        Resolution.ClearOptions();
        AllResolution = Screen.resolutions;
        for (int i = 0; i < AllResolution.Length; i++)
        {
            string Option = AllResolution[i].width + "x" + AllResolution[i].height + " " + AllResolution[i].refreshRate + "Hz";
            Options.Add(Option);
            if (AllResolution[i].width == Screen.currentResolution.width && AllResolution[i].height == Screen.currentResolution.height)
            {
                CurrentResolution = i;
            }
        }
        Resolution.AddOptions(Options);
        Resolution.RefreshShownValue();
        LoadSettings();
    }

    void Update()
    {
        OpenPause();
    }

    void OpenPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseOpen = !PauseOpen;
            PauseMenu.SetActive(PauseOpen);
            if (PauseOpen)
            {
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
        PauseMenu.SetActive(PauseOpen);
        PlayerController.enabled = true;
        MleeController.enabled = true;
        for (int i = 0; i < GunControllers.Length; i++)
        {
            GunControllers[i].enabled = true;
        }
        Time.timeScale = 1;
    }

    public void Settings()
    {
        SettingsOpen = !SettingsOpen;
        SettingsMenu.SetActive(SettingsOpen);
    }

    public void FullScreen()
    {
        Screen.fullScreen = IsFullScreen;
    }

    public void SetResolution(int ResolutionIndex)
    {
        Resolution resolution = AllResolution[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetGraphicsQuality(int GraphicsQualityIndex)
    {
        QualitySettings.SetQualityLevel(GraphicsQualityIndex);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionPreference", Resolution.value);
        PlayerPrefs.SetInt("FullscreenPreference", System.Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetInt("GraphicsQualityPreference", GraphicsQuality.value);
    }

    void LoadSettings()
    {
        if (PlayerPrefs.HasKey("ResolutionPreference"))
        {
            Resolution.value = PlayerPrefs.GetInt("ResolutionPreference");
        }
        else
        {
            Resolution.value = AllResolution.Length;
        }
        if (PlayerPrefs.HasKey("FullscreenPreference"))
        {
            Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        }
        else
        {
            Screen.fullScreen = true;
        }
        if (PlayerPrefs.HasKey("GraphicsQualityPreference"))
        {
            GraphicsQuality.value = PlayerPrefs.GetInt("GraphicsQualityPreference");
        }
        else
        {
            GraphicsQuality.value = 3;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
