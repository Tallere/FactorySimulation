using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class cFurnaceSlot : cInventorySlot, IDropHandler
{
    public enum SlotType
    {
        Smeltable,
        Fuel,
        Output
    }

    public SlotType slotType;

    public override void OnDrop(PointerEventData eventData)
    {
        cInventoryItem inventoryItem = eventData.pointerDrag.GetComponent<cInventoryItem>();

        if (inventoryItem != null)
        {
            bool accepted = false;

            if (slotType == SlotType.Smeltable && inventoryItem.item.smeltable)
            {
                accepted = true;
            }
            else if (slotType == SlotType.Fuel && inventoryItem.item.type == ItemType.Fuel)
            {
                accepted = true;
            }

            if (accepted && transform.childCount == 0)
            {
                inventoryItem.parentAfterDrag = transform;
                base.OnDrop(eventData);
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
        }
        else
        {
            cInventoryItem inventoryItem = GetComponentInChildren<cInventoryItem>();
            if (inventoryItem.item == newItem)
            {
                inventoryItem.count += amount;
                inventoryItem.RefreshCount();
            }
        }
    }
}