using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class cInventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public TextMeshProUGUI countText;

    [HideInInspector] public cItem item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    private CanvasGroup canvasGroup;
    private Canvas parentCanvas;
    private static Canvas dragCanvas;

    private void Awake()
    {
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        parentCanvas = GetComponentInParent<Canvas>();

        if (dragCanvas == null)
        {
            dragCanvas = GameObject.Find("DragCanvas").GetComponent<Canvas>();
        }
    }

    public void InitializeItem(cItem newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RemoveItem()
    {
        if (count > 0)
        {
            count--;
            RefreshCount();
            if (count <= 0)
            {
                Destroy(gameObject);
                cInventory.Instance.UpdateInventorySlots(item);
            }
        }
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;

        if (dragCanvas != null)
        {
            transform.SetParent(dragCanvas.transform, true);
        }

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }

        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        canvasGroup.blocksRaycasts = true;
    }
}
