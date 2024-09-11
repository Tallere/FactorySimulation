using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class cInventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;

    private void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        image.color = selectedColor;
    }

    public void Deselect()
    {
        image.color = notSelectedColor;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            cInventoryItem inventoryItem = eventData.pointerDrag.GetComponent<cInventoryItem>();
            inventoryItem.parentAfterDrag = transform;

            inventoryItem.transform.SetParent(transform);
            inventoryItem.transform.localPosition = Vector3.zero;

            int slotIndex = Array.IndexOf(cInventory.Instance.inventorySlots, this);

            if (slotIndex < 7)
            {
                cInventory.Instance.ChangeSelectedSlot(slotIndex);

                if (cInventory.Instance.selectedSlot == slotIndex)
                {
                    cInventory.Instance.UpdateSelectedItem();
                }
            }
        }
    }
}