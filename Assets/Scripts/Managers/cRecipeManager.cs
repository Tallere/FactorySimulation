using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class cRecipeManager : MonoBehaviour
{
    public cRecipeRegistry recipeRegistry;

    public bool CraftRecipe(int recipeId)
    {
        cRecipe recipe = recipeRegistry.GetRecipeById(recipeId);
        if (recipe == null)
        {
            Debug.LogWarning("Recipe not found!");
            return false;
        }
        
        if (HasRequiredItems(recipe))
        {
            RemoveItemsFromInventory(recipe);
            cInventory.Instance.AddItem(recipe.outputItem, recipe.outputAmount);
            return true;
        }

        return false;
    }

    private bool HasRequiredItems(cRecipe recipe)
    {
        foreach (var item in recipe.inputItems)
        {
            int count = cInventory.Instance.GetItemCount(item);
            if (count < recipe.inputAmounts[Array.IndexOf(recipe.inputItems, item)])
            {
                return false;
            }
        }
        return true;
    }

    private void RemoveItemsFromInventory(cRecipe recipe)
    {
        for (int i = 0; i < recipe.inputItems.Length; i++)
        {
            cInventory.Instance.RemoveItem(recipe.inputItems[i], recipe.inputAmounts[i]);
        }
    }
}
