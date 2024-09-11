using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cFurnaceUI : cMachineUI
{
    public cFurnaceSlot fuelSlot;
    public cFurnaceSlot smeltableSlot;
    public cFurnaceSlot outputSlot;
    public cFurnace furnaceData;
    public cRecipeRegistry recipeRegistry;
    
    private float smeltingProgress = 0.0f;
    private bool isSmelting = false;
    private cItem[] smeltableItems;
    private cItem fuelItem;

    protected override void Start()
    {
        base.Start();
        InitializeSlots();
        machineUiPanel.SetActive(false);
    }

    private void Update()
    {
        if (isSmelting)
        {
            smeltingProgress += Time.deltaTime;

            if (smeltingProgress >= GetSmeltingTime(smeltableItems))
            {
                CompleteSmelting();
            }
        }
        else
        {
            StartSmelting();
        }
    }

    public void InitializeSlots()
    {
        cFurnaceSlot[] slots = machineUiPanel.GetComponentsInChildren<cFurnaceSlot>();
        foreach (var slot in slots)
        {
            switch (slot.slotType)
            {
                case cFurnaceSlot.SlotType.Fuel:
                    fuelSlot = slot;
                    break;
                case cFurnaceSlot.SlotType.Smeltable:
                    smeltableSlot = slot;
                    break;
                case cFurnaceSlot.SlotType.Output:
                    outputSlot = slot;
                    break;
            }
        }
    }

    public override void TogglePanel()
    {
        base.TogglePanel();
    }

    private void StartSmelting()
    {
        List<cInventoryItem> smeltableItemsInSlot = new List<cInventoryItem>(smeltableSlot.GetComponentsInChildren<cInventoryItem>());
        cInventoryItem fuelItemInSlot = fuelSlot.GetComponentInChildren<cInventoryItem>();
        
        Debug.Log("Fuel item in slot: " + (fuelItemInSlot != null ? fuelItemInSlot.item.name : "null"));

        if (smeltableItemsInSlot.Count > 0 && fuelItemInSlot != null)
        {
            smeltableItems = new cItem[smeltableItemsInSlot.Count];
            for (int i = 0; i < smeltableItemsInSlot.Count; i++)
            {
                smeltableItems[i] = smeltableItemsInSlot[i].item;
            }
            fuelItem = fuelItemInSlot.item;

            isSmelting = true;
            smeltingProgress = 0.0f;
            
            fuelItemInSlot.count--;
            if (fuelItemInSlot.count <= 0)
            {
                Destroy(fuelItemInSlot.gameObject);
            }
            else
            {
                fuelItemInSlot.RefreshCount();
            }
        }
    }

    private void CompleteSmelting()
    {
        cItem outputItem = GetOutputItem(smeltableItems);

        cInventoryItem outputItemInSlot = outputSlot.GetComponentInChildren<cInventoryItem>();
        if (outputItemInSlot != null && outputItemInSlot.item == outputItem && outputItemInSlot.count < outputItemInSlot.item.stackSize)
        {
            outputItemInSlot.count++;
            outputItemInSlot.RefreshCount();
        }
        else if (outputItemInSlot == null)
        {
            GameObject newItem = Instantiate(cInventory.Instance.inventoryItemPrefab, outputSlot.transform);
            cInventoryItem inventoryItem = newItem.GetComponent<cInventoryItem>();
            inventoryItem.InitializeItem(outputItem);
            inventoryItem.count = 1;
            inventoryItem.RefreshCount();

            // Fix RectTransform settings
            RectTransform rectTransform = newItem.GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.one;
            rectTransform.anchoredPosition = Vector2.zero;
        }

        List<cInventoryItem> smeltableItemsInSlot = new List<cInventoryItem>(smeltableSlot.GetComponentsInChildren<cInventoryItem>());
        for (int i = 0; i < smeltableItems.Length; i++)
        {
            smeltableItemsInSlot[i].count--;
            if (smeltableItemsInSlot[i].count <= 0)
            {
                Destroy(smeltableItemsInSlot[i].gameObject);
            }
            else
            {
                smeltableItemsInSlot[i].RefreshCount();
            }
        }

        isSmelting = false;
    }

    private cItem GetOutputItem(cItem[] smeltableItems)
    {
        foreach (var recipe in recipeRegistry.recipes)
        {
            bool match = true;
            for (int i = 0; i < smeltableItems.Length; i++)
            {
                if (recipe.inputItems.Length <= i || recipe.inputItems[i] != smeltableItems[i])
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                return recipe.outputItem;
            }
        }
        return null;
    }

    private float GetSmeltingTime(cItem[] smeltableItems)
    {
        foreach (var recipe in recipeRegistry.recipes)
        {
            bool match = true;
            for (int i = 0; i < smeltableItems.Length; i++)
            {
                if (recipe.inputItems.Length <= i || recipe.inputItems[i] != smeltableItems[i])
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                return recipe.smeltingTime;
            }
        }
        return 0f;
    }
}
