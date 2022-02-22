using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SongScriptableObject", menuName = "ScriptableObjects/Audio/SongScriptableObject", order = 1)]
public class SongScriptableObject : ScriptableObject
{
    public string SongName;
    public bool HasTailedStart;
    public bool MainClipLoops;
    public AudioClip TailedStart;
    public AudioClip MainAudioClip;
}

