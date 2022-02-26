using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeartSlot : MonoBehaviour
{
    [SerializeField] private Sprite _emptyHeartSprite;
    [SerializeField] private Sprite _halfHeartSprite;
    [SerializeField] private Sprite _fullHeartSprite;

    [SerializeField] private Image _heartImage;

    /// <summary>
    /// Sets the heart sprite for the correct portion of health
    /// </summary>
    /// <param name="portion">The int value for the portion of heart to fill - 0 = empty, 1 = half, 2 = full</param>
    public void SetHeartPortion(int portion)
    {
        Sprite sprite = null;
        switch (portion)
        {
            case 0:
                sprite = _emptyHeartSprite;
                break;
            case 1:
                sprite = _halfHeartSprite;
                break;
            case 2:
                sprite = _fullHeartSprite;
                break;
            default:
                sprite = _emptyHeartSprite;
                Debug.LogErrorFormat("Unsupported portion sent to heart container: {0}", portion);
                break;
        }
        
        if(_heartImage.sprite != sprite)
        {
            _heartImage.sprite = sprite;
        }
        SetVisibilty(true);
    }

    public void SetVisibilty(bool enabled)
    {
        if (_heartImage.gameObject.activeSelf != enabled)
        {
            _heartImage.gameObject.SetActive(enabled);
        }
    }
}
