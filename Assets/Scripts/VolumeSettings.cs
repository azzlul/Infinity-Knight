using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
   public AudioMixer audioMixer;
   public Slider musicSlider;
   public Slider audioSlider;

    private void Start()
    {
        if(PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetAudioVolume();
            SetMusicVolume();
        }
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetAudioVolume()
    {
        float volume = audioSlider.value;
        audioMixer.SetFloat("audio", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("audioVolume", volume);
    }
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        audioSlider.value = PlayerPrefs.GetFloat("audioVolume");
    }
}
