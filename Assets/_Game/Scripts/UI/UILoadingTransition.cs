using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UILoadingTransition : MonoBehaviour
{
    [SerializeField] private Image _image;

    public static readonly float TRANSITION_TIME = 0.3f;

    private void OnEnable()
    {
        EventManager.onLoadingTransition += EventManager_onLoadingTransition;
    }

    private void OnDisable()
    {
        EventManager.onLoadingTransition -= EventManager_onLoadingTransition;
    }

    private void EventManager_onLoadingTransition(bool show)
    {
        if (show)
        {
            Debug.LogWarning("Transitioning to showing");
            _image.enabled = true;
            _image.DOColor(new Color(0, 0, 0, 1), TRANSITION_TIME).OnComplete(() => { Debug.LogWarning("Transition to show complete"); });
        }
        else
        {
            Debug.LogWarning("Transitioning to hiding");

            _image.DOColor(new Color(0, 0, 0, 0), TRANSITION_TIME).OnComplete(() => { _image.enabled = false; Debug.LogWarning("Turning off transition"); });
        }
    }
}
