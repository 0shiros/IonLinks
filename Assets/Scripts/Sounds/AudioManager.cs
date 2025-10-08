using System;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class AudioClips
{
    public string nameClip;
    public AudioClip audioClip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {get; private set;}
    
    [Header("---Audio Source---")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("---Audio Clips---")]
    [SerializeField] private AudioClips[] audioClips;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        PlayMusic("Music");
        PlaySFX("SFX");
    }
    
    public void PlayMusic(string clipName)
    {
        AudioClips music = System.Array.Find(audioClips, x => x.nameClip == clipName);
        if (music != null)
        {
            musicSource.PlayOneShot(music.audioClip);
        }
        else
        {
            Debug.LogWarning($"No Music audio found with name {clipName}");
        }
    }

    public void PlaySFX(string clipName)
    {
        AudioClips sfx = System.Array.Find(audioClips, x => x.nameClip == clipName);
        if (sfx != null)
        {
            sfxSource.PlayOneShot(sfx.audioClip);
        }
        else
        {
            Debug.LogWarning($"No SFX audio found with name {clipName}");
        }
    }
}

