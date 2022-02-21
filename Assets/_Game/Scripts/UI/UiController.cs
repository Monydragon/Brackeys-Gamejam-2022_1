using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiController : MonoBehaviour
{
    public GameObject inventoryContainer;
    public GameObject itemSlotPrefab;
    public List<GameObject> inventoryItems;
    public TMP_Text inventoryFullText;
    public float inventoryFullTextTime;

    private void OnEnable()
    {
        EventManager.onInventoryChanged += UpdateInventoryUI;
    }

    public void UpdateInventoryUI(InventoryObject _inventory)
    {
        inventoryItems.Clear();
        foreach (Transform child in inventoryContainer.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < _inventory.container.Count; i++)
        {
            var _item = _inventory.container[i];

            var itemSlot = Instantiate(itemSlotPrefab, inventoryContainer.transform);
            itemSlot.name = _item.item.name;
            itemSlot.GetComponent<Image>().sprite = _item.item.icon;
            itemSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = _item.item.name;
            itemSlot.transform.GetChild(1).GetComponent<TMP_Text>().text = _item.item.description;
            itemSlot.transform.GetChild(2).GetComponent<TMP_Text>().text = _item.amount.ToString();
            itemSlot.GetComponent<ItemSlotUse>().item = _item.item;
            inventoryItems.Add(itemSlot);
            if(_inventory.currentSize >= _inventory.maxSize)
            {
                StartCoroutine(ShowInventoryFullText());
            }

        }
    }

    private void OnDisable()
    {
        EventManager.onInventoryChanged -= UpdateInventoryUI;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ShowInventoryFullText()
    {
        inventoryFullText.gameObject.SetActive(true);
        yield return new WaitForSeconds(inventoryFullTextTime);
        inventoryFullText.gameObject.SetActive(false);
    }
}
