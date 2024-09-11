using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class cAssemblerSlot : MonoBehaviour, IDropHandler
{
    public cItem item;
    public int count;

    public void OnDrop(PointerEventData eventData)
    {
        cInventoryItem inventoryItem = eventData.pointerDrag.GetComponent<cInventoryItem>();

        if (inventoryItem != null)
        {
            if (transform.childCount == 0 || (item != null && item == inventoryItem.item))
            {
                inventoryItem.parentAfterDrag = transform;
                inventoryItem.transform.SetParent(transform);
                inventoryItem.transform.localPosition = Vector3.zero;
                UpdateSlotState();
            }
            else
            {
                inventoryItem.transform.SetParent(inventoryItem.parentAfterDrag);
                inventoryItem.transform.localPosition = Vector3.zero;
            }
        }
    }

    public void AddItem(cItem newItem, int amount)
    {
        if (transform.childCount == 0)
        {
            GameObject newItemObject = Instantiate(cInventory.Instance.inventoryItemPrefab, transform);
            cInventoryItem inventoryItem = newItemObject.GetComponent<cInventoryItem>();
            inventoryItem.InitializeItem(newItem);
            inventoryItem.count = amount;
            inventoryItem.RefreshCount();
            UpdateSlotState();
        }
        else
        {
            cInventoryItem inventoryItem = GetComponentInChildren<cInventoryItem>();
            if (inventoryItem.item == newItem)
            {
                inventoryItem.count += amount;
                inventoryItem.RefreshCount();
                UpdateSlotState();
            }
        }
    }

    public void RemoveItem(int amount)
    {
        cInventoryItem inventoryItem = GetComponentInChildren<cInventoryItem>();
        if (inventoryItem != null)
        {
            inventoryItem.count -= amount;
            if (inventoryItem.count <= 0)
            {
                Destroy(inventoryItem.gameObject);
                UpdateSlotState();
            }
            else
            {
                inventoryItem.RefreshCount();
                UpdateSlotState();
            }
        }
    }

    private void UpdateSlotState()
    {
        cInventoryItem inventoryItem = GetComponentInChildren<cInventoryItem>();
        if (inventoryItem != null)
        {
            item = inventoryItem.item;
            count = inventoryItem.count;
        }
        else
        {
            item = null;
            count = 0;
        }
    }


    public bool IsEmpty()
    {
        return transform.childCount == 0;
    }
}
