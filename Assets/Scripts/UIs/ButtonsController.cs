using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour
{
    public TMP_Dropdown ResolutionButton;
    public Toggle FullScreenButtton;
    public TMP_Dropdown GraphicsQualityButton;
    private Resolution[] AllResolution;
    private int CurrentResolution;
    private List<string> Options = new List<string>();

    void Start()
    {
        ResolutionButton.ClearOptions();
        AllResolution = Screen.resolutions;
        for (int i = 0; i < AllResolution.Length; i++)
        {
            string Option = AllResolution[i].width + "x" + AllResolution[i].height + " " + AllResolution[i].refreshRateRatio + "Hz";
            Options.Add(Option);
            if (AllResolution[i].width == Screen.currentResolution.width && AllResolution[i].height == Screen.currentResolution.height)
            {
                CurrentResolution = i;
            }
        }
        ResolutionButton.AddOptions(Options);
        ResolutionButton.RefreshShownValue();
        LoadSettings();
    }

    public void SetResolution(int ResolutionIndex)
    {
        Resolution resolution = AllResolution[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void FullScreen(bool IsFullScreen)
    {
        Screen.fullScreen = IsFullScreen;
    }

    public void SetGraphicsQuality(int GraphicsQualityIndex)
    {
        QualitySettings.SetQualityLevel(GraphicsQualityIndex);
    }

    public void DefualtSettings()
    {
        ResolutionButton.value = AllResolution.Length;
        Screen.fullScreen = true;
        FullScreenButtton.isOn = true;
        GraphicsQualityButton.value = 2;

    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionPreference", ResolutionButton.value);
        PlayerPrefs.SetInt("FullscreenPreference", System.Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetInt("GraphicsQualityPreference", GraphicsQualityButton.value);
    }

    void LoadSettings()
    {
        if (PlayerPrefs.HasKey("ResolutionPreference"))
        {
            ResolutionButton.value = PlayerPrefs.GetInt("ResolutionPreference");
        }
        else
        {
            ResolutionButton.value = AllResolution.Length;
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
            GraphicsQualityButton.value = PlayerPrefs.GetInt("GraphicsQualityPreference");
        }
        else
        {
            GraphicsQualityButton.value = 2;
        }
    }
}
