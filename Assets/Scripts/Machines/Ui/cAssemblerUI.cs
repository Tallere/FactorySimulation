using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAssemblerUI : cMachineUI
{
    public cRecipe[] recipes;
    public cAssemblerSlot[] inputSlots;
    public cAssemblerSlot outputSlot;
    public float craftingProgress = 0f;
    private bool isCrafting = false;
    private cRecipe currentRecipe;

    void Update()
    {
        if (!isCrafting)
        {
            CheckForCrafting();
        }

        if (isCrafting)
        {
            craftingProgress += Time.deltaTime;

            if (craftingProgress >= currentRecipe.craftingTime)
            {
                CompleteCrafting();
            }
        }
    }

    void CheckForCrafting()
    {
        foreach (var recipe in recipes)
        {
            if (CanCraft(recipe))
            {
                StartCrafting(recipe);
                break;
            }
        }
    }

    void StartCrafting(cRecipe recipe)
    {
        currentRecipe = recipe;
        isCrafting = true;
        craftingProgress = 0f;
        ConsumeIngredients(recipe);
    }

    void CompleteCrafting()
    {
        int outputAmount = currentRecipe.outputAmount;
        if (outputSlot.IsEmpty() || (outputSlot.item == currentRecipe.outputItem && outputSlot.count + outputAmount <= outputSlot.item.stackSize))
        {
            outputSlot.AddItem(currentRecipe.outputItem, outputAmount);
        }
        else
        {
            Debug.LogWarning("Not enough space in output slot to complete crafting.");
        }
        isCrafting = false;
    }

    bool CanCraft(cRecipe recipe)
    {
        for (int i = 0; i < recipe.inputItems.Length; i++)
        {
            cItem inputItem = recipe.inputItems[i];
            int requiredAmount = recipe.inputAmounts[i];

            if (!HasEnoughItems(inputItem, requiredAmount))
            {
                return false;
            }
        }
        return true;
    }

    bool HasEnoughItems(cItem item, int requiredAmount)
    {
        int totalAmount = 0;
        foreach (var slot in inputSlots)
        {
            if (slot.item == item)
            {
                totalAmount += slot.count;
            }
        }
        return totalAmount >= requiredAmount;
    }

    void ConsumeIngredients(cRecipe recipe)
    {
        for (int i = 0; i < recipe.inputItems.Length; i++)
        {
            cItem inputItem = recipe.inputItems[i];
            int requiredAmount = recipe.inputAmounts[i];

            foreach (var slot in inputSlots)
            {
                if (slot.item == inputItem)
                {
                    int amountToConsume = Mathf.Min(requiredAmount, slot.count);
                    slot.RemoveItem(amountToConsume);
                    requiredAmount -= amountToConsume;
                    if (requiredAmount <= 0)
                    {
                        break;
                    }
                }
            }
        }
    }

    public void TogglePanel()
    {
        machineUiPanel.SetActive(!machineUiPanel.activeSelf);
    }
}
