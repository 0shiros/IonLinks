using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    
    [Header("Fullscreen")]
    [SerializeField] private Toggle fullscreenToggle;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadVolume();
        }
        else
        {
            musicSlider.value = 0.8f;
            sfxSlider.value = 0.8f;
            SetMusicVolume();
            SetSfxVolume();
        }

        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            LoadFullscreen();
        }
        else
        {
            fullscreenToggle.isOn = true;
            SetFullscreen();
        }
    }

    public void SetMusicVolume()
    {
        float musicVolume = musicSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(musicVolume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }
    
    public void SetSfxVolume()
    {
        float sfxVolume = sfxSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(sfxVolume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }
    
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SetMusicVolume();
        
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetSfxVolume();
    }

    public void SetFullscreen()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
    }

    private void LoadFullscreen()
    {
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1 ? true : false; 
        SetFullscreen();
    }
}
