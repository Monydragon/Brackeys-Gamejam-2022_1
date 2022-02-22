using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTestController : MonoBehaviour
{
    public SongScriptableObject RequestableSong;

    public float volumeChangeRate = .1f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            BackgroundMusicManager.Instance.PauseMusic();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            BackgroundMusicManager.Instance.UnpauseMusic();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            BackgroundMusicManager.Instance.PlayRequestedSong(RequestableSong, true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            BackgroundMusicManager.Instance.PlayCurrentSceneSong();
        }
        if (Input.GetKeyDown(KeyCode.A))// change mas
        {
            AudioManager.Instance.SetVolumePercentage(AudioManager.AudioChannel.Master,AudioManager.Instance.GetVolumePercentage(AudioManager.AudioChannel.Master) - volumeChangeRate);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            AudioManager.Instance.SetVolumePercentage(AudioManager.AudioChannel.Master, AudioManager.Instance.GetVolumePercentage(AudioManager.AudioChannel.Master) + volumeChangeRate);
        }
    }
}
