using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;

    [SerializeField] AudioMixer masterMixer;

    List<Resolution> allResolutions;
    Resolution currentResolution;
    
    // Start is called before the first frame update
    void Start()
    {
        // create list of resolutions
        allResolutions = new List<Resolution>();
        allResolutions.Add(new Resolution(640, 480));
        allResolutions.Add(new Resolution(1024, 1024));
        allResolutions.Add(new Resolution(1280, 720));
        allResolutions.Add(new Resolution(1366, 768));
        allResolutions.Add(new Resolution(1920, 1080));

        // add resolutions to dropdown
        FillResolutions();

        // update UI to reflect current settings
        RefreshUI();
    }

    void RefreshUI() // changes UI elements to reflect current settings
    {
        // set resolution dropdown based on current resolution
        currentResolution = new Resolution(Screen.width, Screen.height);

        //Debug.Log("on refresh, resolution is: " + currentResolution.ToString());

        int resIndex = -1;
        foreach(Resolution r in allResolutions)
        {
            if (currentResolution.x == r.x && currentResolution.y == r.y)
            {
                resIndex = allResolutions.IndexOf(r);
            }
        }

        if (resIndex != -1)
        {
            //Debug.Log("found res in the list. index = " + resIndex);
            resolutionDropdown.value = resIndex;
        }
        else
        {
            //Debug.Log("res not in list");
            resolutionDropdown.value = 0;
        }

        // set fullscreen checkbox
        bool isFullscreen = Screen.fullScreen;
        fullscreenToggle.isOn = isFullscreen;

        // set BGM slider based on music volume
        float bgmVol = 0;
        masterMixer.GetFloat("musicVol", out bgmVol);
        bgmSlider.value = bgmVol;

        // set SFX slider based on music volume
        float sfxVol = 0;
        masterMixer.GetFloat("sfxVol", out sfxVol);
        sfxSlider.value = sfxVol;
    }

    public void FillResolutions() // add all resolutions to dropdown list
    {
        List<string> resText = new List<string>();

        foreach(Resolution r in allResolutions)
        {
            resText.Add(r.ToString());
        }

        resolutionDropdown.AddOptions(resText);
    }

    public void UpdateWindow() // change screen resolution and fullscreen setting; called when values are changed in the menu
    {
        List<string> resText = new List<string>();

        foreach (Resolution r in allResolutions)
        {
            resText.Add(r.ToString());
        }

        int index = resolutionDropdown.value;

        currentResolution = allResolutions[index];

        Screen.SetResolution(currentResolution.x, currentResolution.y, fullscreenToggle.isOn);
        Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void UpdateSFXVolume(float val) // change volume of sound effects, called when slider is adjusted
    {
        masterMixer.SetFloat("sfxVol", val);
    }

    public void UpdateBGMVolume(float val) // change volume of music, called when slider is adjusted
    {
        masterMixer.SetFloat("musicVol", val);
    }

    public void Cancel() // cancel settings menu, called when back button is clicked
    {
        gameObject.SetActive(false);
    }
}

class Resolution
{
    public int x;
    public int y;

    public Resolution(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return x + " x " + y;
    }
}