using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    //enum of the audio channels available, can add more
    public enum AudioChannel { Master, SFX, Music } //...

    //struct for storing variables about audio channels and exposing to the editor
    [System.Serializable]
    public struct AudioChannelInfo
    {
        public AudioChannel Channel;
        public AudioMixerGroup MixerGroup;
        public string VolumePropertyName;
    }

    //Serializable list of Audio Channel Info, to be fed into dictionary
    public List<AudioChannelInfo> AudioChannelList= new List<AudioChannelInfo>();

    //Dictionary relating the enum of the channel to the string value of the volume setting inside the mixer and PlayerPrefs
    Dictionary<AudioChannel, AudioChannelInfo> AudioChannelInfoDictionary = new Dictionary<AudioChannel, AudioChannelInfo>();

    public AudioMixer MasterMixer;

    private void Awake()
    {
        //create this object as a singleton that persists between levels
        if (_instance != null && _instance != this)
        {
            Destroy(_instance);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        //copy list into Dictionary
        foreach (AudioChannelInfo audioChannelInfo in AudioChannelList)
        {
            AudioChannelInfoDictionary[audioChannelInfo.Channel] = audioChannelInfo;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //Set each audio channels volume percentage to the value in PlayerPrefs, or default to 100%
        foreach(KeyValuePair<AudioChannel,AudioChannelInfo> kvp in AudioChannelInfoDictionary)
        {
            SetVolumePercentage(kvp.Key, PlayerPrefs.GetFloat(kvp.Value.VolumePropertyName, 1f), false);
        }
    }

    /**
     *Set Volume
     *Sets the volume of the audio channel
     *@param audioChannelVolumeToGet The channel to set the volume of
     *@param newVolumePercentage the percentage to set the volume to (Clamped 0f to 1f)
     *@param bSetPlayerPrefFlag flag to set new volume percentage to PlayerPrefs
     *@return float volume percentage between 0f and 1f
     **/
    public void SetVolumePercentage(AudioChannel audioChannelVolumeToSet, float newVolumePercentage, bool bSetPlayerPrefFlag = false)
    {
        newVolumePercentage = Mathf.Clamp(newVolumePercentage, 0f, 1f);
        float decibel = LinearToDecibel(newVolumePercentage);
        string channelVolumePropertyName = AudioChannelInfoDictionary[audioChannelVolumeToSet].VolumePropertyName;
        MasterMixer.SetFloat(channelVolumePropertyName, decibel);
        if (bSetPlayerPrefFlag)
        {
            PlayerPrefs.SetFloat(channelVolumePropertyName, decibel);
        }
    }

     /**
      *Get Volume
      *gets the volume of the audio channel
      *@param audioChannelVolumeToGet The channel to get the volume of
      *
      *@return float volume percentage between 0f and 1f
      **/
    public float GetVolumePercentage(AudioChannel audioChannelVolumeToGet)
    {
        float outVolume; //out parameter
        string channelVolumePropertyName = AudioChannelInfoDictionary[audioChannelVolumeToGet].VolumePropertyName;
        MasterMixer.GetFloat(channelVolumePropertyName, out outVolume);
        return DecibelToLinear(outVolume);
    }

    private float LinearToDecibel(float linear)
    {
        float dB;

        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;

        return dB;
    }

    private float DecibelToLinear(float dB)
    {
        float linear = Mathf.Pow(10.0f, dB / 20.0f);

        return linear;
    }
}
