using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneSongListScriptableObject", menuName = "ScriptableObjects/Audio/SceneSongListScriptableObject", order = 1)]
public class SceneSongListScriptableObject : ScriptableObject
{

    public List<SongScriptableObject> SceneSongs;
}
