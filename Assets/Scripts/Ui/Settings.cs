using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown resolutionButton;
    [SerializeField]
    private Toggle fullScreenButtton;
    [SerializeField]
    private TMP_Dropdown graphicsQualityButton;
    private Resolution[] allResolution;
    private int currentResolution;
    private List<string> options = new List<string>();

    void Start()
    {
        resolutionButton.ClearOptions();
        allResolution = Screen.resolutions;
        for (int i = 0; i < allResolution.Length; i++)
        {
            string Option = allResolution[i].width + "x" + allResolution[i].height;
            options.Add(Option);
            if (allResolution[i].width == Screen.currentResolution.width && allResolution[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }
        resolutionButton.AddOptions(options);
        resolutionButton.RefreshShownValue();
        LoadSettings();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = allResolution[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetGraphicsQuality(int graphicsQualityIndex)
    {
        QualitySettings.SetQualityLevel(graphicsQualityIndex);
    }

    public void DefualtSettings()
    {
        resolutionButton.value = allResolution.Length;
        Screen.fullScreen = true;
        fullScreenButtton.isOn = true;
        graphicsQualityButton.value = 2;

    }

    public void AcceptSettings()
    {
        PlayerPrefs.SetInt("ResolutionPreference", resolutionButton.value);
        PlayerPrefs.SetInt("FullscreenPreference", System.Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetInt("GraphicsQualityPreference", graphicsQualityButton.value);
    }

    void LoadSettings()
    {
        if (PlayerPrefs.HasKey("ResolutionPreference"))
        {
            resolutionButton.value = PlayerPrefs.GetInt("ResolutionPreference");
        }
        else
        {
            resolutionButton.value = allResolution.Length;
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
            graphicsQualityButton.value = PlayerPrefs.GetInt("GraphicsQualityPreference");
        }
        else
        {
            graphicsQualityButton.value = 2;
        }
    }
}
