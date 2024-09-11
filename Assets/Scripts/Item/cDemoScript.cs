using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class cDemoScript : MonoBehaviour
{
    public cInventory inventory;
    public cItemRegistry itemRegistry;

    [SerializeField] cRecipeManager recipeManager;
    

    public void PickupItem(int id)
    {
        cItem itemToPickup = itemRegistry.GetItemById(id);
        if (itemToPickup != null)
        {
            bool result = inventory.AddItem(itemToPickup, 1);
            if (result)
            {
                Debug.Log("Item added to inventory");
            }
            else
            {
                Debug.Log("Item not added to inventory");
            }
        }
        else
        {
            Debug.LogError("Item ID not found in the registry");
        }
    }
    
    public void OnCraftButtonClicked(int recipeId)
    {
        bool success = recipeManager.CraftRecipe(recipeId);
        if (success)
        {
            Debug.Log("Crafting successful!");
        }
        else
        {
            Debug.Log("Crafting failed!");
        }
    }
    
    
}
