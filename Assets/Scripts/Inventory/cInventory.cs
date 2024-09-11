using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class cInventory : MonoBehaviour
{
    public cInventorySlot[] inventorySlots;

    public GameObject inventoryItemPrefab;

    private cItem item;

    [SerializeField]
    private InputActionAsset playerControls;

    [SerializeField]
    private string inventoryActionMap = "Inventory";

    private InputAction openInventoryAction;

    [SerializeField] int maxItemSize = 999;

    public static cInventory Instance { get; private set; }

    [SerializeField]
    private GameObject uiPanel;

    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private cItemRegistry itemRegistry;

    [SerializeField] 
    private cRecipeSorter recipeSorter;

    [SerializeField] 
    private GameObject recipePanel;
    
    public int selectedSlot { get; private set; }

    public bool isBuildMode = false;
    
    public void ChangeSelectedSlot(int slot)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }

        if (slot >= 0 && slot < inventorySlots.Length)
        {
            inventorySlots[slot].Select();
            selectedSlot = slot;
            UpdateSelectedItem();
        }
        else
        {
            selectedSlot = -1;
        }
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number <= 7)
            {
                ChangeSelectedSlot(number - 1);
                var selectedItem = GetSelectedItem();
                if (selectedItem != null && selectedItem.item.actionType == Actiontype.Placeable)
                {
                    isBuildMode = true;
                }
                else
                {
                    isBuildMode = false;
                }
            }
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        openInventoryAction = playerControls.FindAction(inventoryActionMap);
        RegisterInputActions();

        openInventoryAction.Enable();
    }

    private void Start()
    {
        ChangeSelectedSlot(-1);
        uiPanel.SetActive(false);
        recipePanel.SetActive(false);
    }

    public bool AddItem(cItem item, int amount)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            cInventorySlot slot = inventorySlots[i];
            cInventoryItem itemInSlot = slot.GetComponentInChildren<cInventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxItemSize && itemInSlot.item.stackable)
            {
                itemInSlot.count += amount;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            cInventorySlot slot = inventorySlots[i];
            cInventoryItem itemInSlot = slot.GetComponentInChildren<cInventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot, amount);
                return true;
            }
        }

        return false;
    }

    void SpawnNewItem(cItem item, cInventorySlot inventorySlot, int amount)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, inventorySlot.transform);
        cInventoryItem inventoryItem = newItem.GetComponent<cInventoryItem>();
        inventoryItem.InitializeItem(item);
        inventoryItem.count = amount;
        inventoryItem.RefreshCount();
    }

    private void RegisterInputActions()
    {
        openInventoryAction.performed += context => OpenInventory();
    }

    void OpenInventory()
    {
        uiPanel.SetActive(!uiPanel.activeSelf);
        recipePanel.SetActive(!recipePanel.activeSelf);
        
        if (uiPanel.activeSelf && recipePanel.activeSelf)
        {
            recipeSorter.DisplayAvailableRecipes();
        }
    }

    public cInventoryItem GetSelectedItem()
    {
        if (selectedSlot >= 0 && selectedSlot < inventorySlots.Length)
        {
            return inventorySlots[selectedSlot].GetComponentInChildren<cInventoryItem>();
        }
        return null;
    }

    public void RemoveItem(cItem item, int amount)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            cInventorySlot slot = inventorySlots[i];
            cInventoryItem itemInSlot = slot.GetComponentInChildren<cInventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item)
            {
                itemInSlot.count -= amount;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
                return;
            }
        }
    }

    public int GetItemCount(cItem item)
    {
        int count = 0;
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            cInventorySlot slot = inventorySlots[i];
            cInventoryItem itemInSlot = slot.GetComponentInChildren<cInventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item)
            {
                count += itemInSlot.count;
            }
        }
        return count;
    }

    public void UpdateInventorySlots(cItem item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            cInventorySlot slot = inventorySlots[i];
            cInventoryItem itemInSlot = slot.GetComponentInChildren<cInventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item)
            {
                itemInSlot.RemoveItem();
                return;
            }
        }
    }

    public void UpdateSelectedItem()
    {
        var selectedItem = GetSelectedItem();
        if (selectedItem != null)
        {
            isBuildMode = selectedItem.item.actionType == Actiontype.Placeable;
        }
        else
        {
            isBuildMode = false;
        }

        cGridBuildingSystem.Instance.RefreshSelectedObjectType();
    }
}

