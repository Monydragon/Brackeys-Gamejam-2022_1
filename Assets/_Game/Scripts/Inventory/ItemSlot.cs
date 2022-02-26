using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemDescText;
    [SerializeField] private TextMeshProUGUI _itemAmountText;

    private ItemObject _item;

    public void UseItem()
    {
        if(_item != null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _item.Use(player);
        }
        else
        {
            Debug.LogWarning($"Item is NULL");
        }
    }

    public void ClearItem()
    {
        _itemImage.enabled = false;
        _itemImage.sprite = null;
        _itemNameText.text = string.Empty;
        _itemDescText.text = string.Empty;
        _itemAmountText.text = string.Empty;
        _itemImage.gameObject.SetActive(false);
    }

    public void SetItem(ItemObject item, int amount)
    {
        _item = item;
        _itemImage.sprite = _item.icon;
        _itemNameText.text = _item.name;
        _itemDescText.text = _item.description;
        _itemAmountText.text = amount.ToString();
        _itemImage.gameObject.SetActive(true);
    }

    public void UpdateAmount(int amount)
    {
        if(amount == 0)
        {
            ClearItem();
        }
        else
        {
            _itemAmountText.text = amount.ToString();
        }
    }
}
