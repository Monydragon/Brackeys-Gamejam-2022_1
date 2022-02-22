using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicManager : MonoBehaviour
{
    private static BackgroundMusicManager _instance;
    public static BackgroundMusicManager Instance { get { return _instance; } }

    public float AudioFadeOutTime = 1f;

    public float AudioFadeInTime = 1f;

    public float PaddingBetweenSongs = .1f;

    public SceneSongListScriptableObject LevelSongList;

    public AudioSource MainAudioSource;

    public AudioSource TailedAudioSource;

    SongScriptableObject CurrentSceneSong;

    SongScriptableObject CurrentPlayingSong;


    private void Awake()
    {
        //create this object as a singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }       
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        if (MainAudioSource == null)
        {
            Debug.LogError("BackgroundMusic: No MainAudioSource Component Set");
        }
        if(TailedAudioSource == null)
        {
            Debug.LogError("BackgroundMusic: No TailedAudioSource Component Set");
        }
    }

    //When Scene is loaded, set the scene song and start playing it
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetCurrentSceneSong(scene);
        PlayCurrentSceneSong();
    }

    //gets the scene song from the LevelSongList
    //Scene must be in build index
    private void SetCurrentSceneSong(Scene scene)
    {
        if (MainAudioSource == null) return;
        if(LevelSongList.SceneSongs.Count < scene.buildIndex)
        {
            Debug.LogError("BackgroundMusicManager: No song set for build index: " + scene.buildIndex);
            return;
        }

        CurrentSceneSong = LevelSongList.SceneSongs[scene.buildIndex];
    }

    //Fades out any currently playing song, and plays the Scene song
    public void PlayCurrentSceneSong()
    {
        //dont play song if it is already playing
        if (CurrentPlayingSong != null ? CurrentPlayingSong.SongName == CurrentSceneSong.SongName : false) return;

        //Set the currently playing song to the song to play
        CurrentPlayingSong = CurrentSceneSong;

        //stop coroutines that may be waiting
        StopAllCoroutines();

        float delayTime = 0f;
        //if something is playing fade it out
        if (MainAudioSource.isPlaying)
        {
            StartCoroutine(FadeOutSongAndStop(AudioFadeOutTime));
            delayTime += AudioFadeOutTime + PaddingBetweenSongs;
        }
        StartCoroutine(PlaySong(CurrentSceneSong, delayTime, true));
    }
    
    //Fades out any currently playing song, and plays the requested song
    public void PlayRequestedSong(SongScriptableObject songToPlay, bool bLoopSong)
    {
        //dont play song if it is already playing
        if (CurrentPlayingSong != null ? CurrentPlayingSong.SongName == songToPlay.SongName : false) return;
        
        //Set the currently playing song to the song to play
        CurrentPlayingSong = songToPlay;
        
        StopAllCoroutines();

        float delayTime = 0f;
        //if something is playing fade it out
        if (MainAudioSource.isPlaying)
        {
            StartCoroutine(FadeOutSongAndStop(AudioFadeOutTime));
            delayTime += AudioFadeOutTime + PaddingBetweenSongs;
        }

        StartCoroutine(PlaySong(songToPlay, delayTime, true));
    }

    public void PauseMusic()
    {
        MainAudioSource.Pause();
        TailedAudioSource.Pause();
    }
    public void UnpauseMusic()
    {
        MainAudioSource.UnPause();
        TailedAudioSource.UnPause();
    }
    public bool IsMusicPaused()
    {
        return MainAudioSource.isPlaying;
    }

    private IEnumerator PlaySong(SongScriptableObject songToPlay, float delay, bool bLoopSong)
    {
        
        //wait for desired time to start song
        yield return new WaitForSeconds(delay);

        //Fade in to new song
        StartCoroutine(FadeInSong(AudioFadeInTime));

        //play the main audio clip
        MainAudioSource.clip = songToPlay.MainAudioClip;
        MainAudioSource.loop = bLoopSong;
        MainAudioSource.Play();
        if (songToPlay.HasTailedStart)
        {
            //if the song has a tailed start, mute the main audio, and play the tailed
            MainAudioSource.mute = true;
            TailedAudioSource.clip = songToPlay.TailedStart;
            TailedAudioSource.loop = bLoopSong;
            TailedAudioSource.Play();

            //wait for the tailed audio to end, then unmute the main audio, and stop the tailed
            yield return new WaitForSeconds(songToPlay.TailedStart.length);
            MainAudioSource.mute = false;
            TailedAudioSource.Stop();

        }
        else
        {
            MainAudioSource.mute = false;
        }

    }
    //Fade AudioSource volume to 0 and then stop
    private IEnumerator FadeOutSongAndStop(float FadeTime)
    {
        for (float volume = 1f; volume >= 0f; volume -= Time.deltaTime / FadeTime)
        {
            MainAudioSource.volume = volume;
            TailedAudioSource.volume = volume;
            yield return null;
        }
        MainAudioSource.volume = 0f;
        TailedAudioSource.volume = 0f;
        MainAudioSource.Stop();
        TailedAudioSource.Stop();
    }

    //Fade in Audiosource volume from 0 to 1
    private IEnumerator FadeInSong(float FadeTime)
    {
        for (float volume = 0f; volume < 1f; volume += Time.deltaTime/FadeTime)
        {
            Debug.Log(volume);
            MainAudioSource.volume = volume;
            TailedAudioSource.volume = volume;
            yield return null;
        }
        MainAudioSource.volume = 1f;
        TailedAudioSource.volume = 1f;
    }
}

