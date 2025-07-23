
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] string musicVolumeParam = "MusicVolume";
    [SerializeField] string sfxVolumeParam = "SFXVolume";
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    void Awake()
    {
        LoadMusicVolume();
        LoadSFXVolume();
    }

    void OnEnable()
    {
        SetMusicVolume();
        SetSFXVolume();
    }

    void Start()
    {
        SetMusicVolume();
        SetSFXVolume();
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        float dbVolume = volume > 0.0001f ? Mathf.Log10(volume) * 20f : -80f;
        audioMixer.SetFloat(musicVolumeParam, dbVolume);
        PlayerPrefs.SetFloat(musicVolumeParam, volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        float dbVolume = volume > 0.0001f ? Mathf.Log10(volume) * 20f : -80f;
        audioMixer.SetFloat(sfxVolumeParam, dbVolume);
        PlayerPrefs.SetFloat(sfxVolumeParam, volume);
        PlayerPrefs.Save();
    }


    public void LoadMusicVolume()
    {

        float volume = PlayerPrefs.GetFloat(musicVolumeParam, 1f);
        musicSlider.value = volume;
    }

    public void LoadSFXVolume()
    {

        float volume = PlayerPrefs.GetFloat(sfxVolumeParam, 1f);
        sfxSlider.value = volume;

    }
}