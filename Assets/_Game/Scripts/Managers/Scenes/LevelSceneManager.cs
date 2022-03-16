using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSceneManager
{
    public void LoadLevel(string levelName, bool fadeTransition = true)
    {
        SceneManager.LoadScene(levelName);

        if (fadeTransition)
        {
            // Trigger transition end
            EventManager.LoadingTransition(false);
        }
    }
}
