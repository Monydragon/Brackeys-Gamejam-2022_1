using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [SerializeField] private Scrollbar _masterScrollbar;
    [SerializeField] private Scrollbar _bgmScrollbar;
    [SerializeField] private Scrollbar _sfxScrollbar;

    private GameSystems _systems;

    public readonly string KEY_MASTER_SOUND = "MasterVolume";
    public readonly string KEY_BGM_SOUND = "MusicVolume";
    public readonly string KEY_SFX_SOUND = "SFXVolume";

    public void Setup(GameSystems systems)
    {
        _systems = systems;

        // Set default values
        _masterScrollbar.value = PlayerPrefs.GetFloat(KEY_MASTER_SOUND, 1.0f);
        _bgmScrollbar.value = PlayerPrefs.GetFloat(KEY_BGM_SOUND, 1.0f);
        _sfxScrollbar.value = PlayerPrefs.GetFloat(KEY_SFX_SOUND, 1.0f);
    }

    public void OnBackClicked()
    {
        StartCoroutine(_systems.StateManager.NavigateToState(null, true));
    }

    public void OnMasterSoundChanged(Single value)
    {
        PlayerPrefs.SetFloat(KEY_MASTER_SOUND, value);
        PlayerPrefs.Save();
        AudioManager.Instance.SetVolumePercentage(AudioManager.AudioChannel.Master, value);
    }
    public void OnSFXSoundChanged(Single value)
    {
        PlayerPrefs.SetFloat(KEY_SFX_SOUND, value);
        PlayerPrefs.Save();
        AudioManager.Instance.SetVolumePercentage(AudioManager.AudioChannel.SFX, value);
    }
    public void OnBGMSoundChanged(Single value)
    {
        PlayerPrefs.SetFloat(KEY_BGM_SOUND, value);
        PlayerPrefs.Save();
        AudioManager.Instance.SetVolumePercentage(AudioManager.AudioChannel.Music, value);
    }
}
