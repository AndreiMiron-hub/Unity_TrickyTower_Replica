using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject MainMenuHolder;
    public GameObject OptionsMenuHolder;

    public Slider[] VolumeSliders;
    public Toggle[] ResolutionToggles;

    public int[] ScreenWidths;
    public int activeScreenResIndex;

    private void Start()
    {
        activeScreenResIndex = PlayerPrefs.GetInt("ScreenResIndex", activeScreenResIndex);
        bool isFullscreen = PlayerPrefs.GetInt("FullScreen") == 1 ? true : false;

        VolumeSliders[0].value = AudioManager.Instance.MasterVolumePercent;
        VolumeSliders[1].value = AudioManager.Instance.MusicVolumePercent;
        VolumeSliders[2].value = AudioManager.Instance.SfxVolumePercent;

        for(int i = 0; i < ResolutionToggles.Length; i++)
        {
            ResolutionToggles[i].isOn = i == activeScreenResIndex;
        }

        SetFullScreen(isFullscreen);
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OptionsMenu()
    {
        MainMenuHolder.SetActive(false);
        OptionsMenuHolder.SetActive(true);
    }

    public void BackToMainMenu()
    {
        MainMenuHolder.SetActive(true);
        OptionsMenuHolder.SetActive(false);
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.Instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }

    public void SetSfxVolume(float value)
    {
        AudioManager.Instance.SetVolume(value, AudioManager.AudioChannel.Sfx);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.Instance.SetVolume(value, AudioManager.AudioChannel.Music);
    }

    public void SetScreenResolution(int i)
    {
        if (ResolutionToggles[i].isOn)
        {
            float aspectRatio = 16 / 9f;
            Screen.SetResolution(ScreenWidths[i], (int)(ScreenWidths[i] / aspectRatio), false);
            activeScreenResIndex = i;
            PlayerPrefs.SetInt("ScreenResIndex", activeScreenResIndex);
            PlayerPrefs.Save();
        }
    }

    public void SetFullScreen(bool isFullscreen)
    {
        for (int i = 0; i < ResolutionToggles.Length; i++)
        {
            ResolutionToggles[i].interactable = !isFullscreen;
        }

        if (isFullscreen)
        {
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
        else
        {
            SetScreenResolution(activeScreenResIndex);
        }

        PlayerPrefs.SetInt("FullScreen", ((isFullscreen) ? 1 : 0));
        PlayerPrefs.Save();
    }
}
