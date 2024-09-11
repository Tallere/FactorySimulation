using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cRecipeSorter : MonoBehaviour
{
    public cRecipeRegistry recipeRegistry;
    public GameObject recipeButtonPrefab;
    public Transform recipeListParent;
    public cRecipeManager recipeManager;


    void Start()
    {
        recipeRegistry.Initialize();
        DisplayAvailableRecipes();
    }

    public void DisplayAvailableRecipes()
    {
        foreach (Transform child in recipeListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var recipe in recipeRegistry.recipes)
        {
            if (CanCraftRecipe(recipe))
            {
                CreateRecipeButton(recipe);
            }
        }
    }

    private bool CanCraftRecipe(cRecipe recipe)
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

    private void CreateRecipeButton(cRecipe recipe)
    {
        GameObject buttonObject = Instantiate(recipeButtonPrefab, recipeListParent);
        Button button = buttonObject.GetComponent<Button>();
        Image buttonImage = buttonObject.GetComponentInChildren<Image>();
        
        Image childImage = buttonObject.transform.Find("ChildImage").GetComponent<Image>(); 
        childImage.sprite = recipe.outputItem.image;

        button.onClick.AddListener(() => OnRecipeButtonClicked(recipe));
    }

    private void OnRecipeButtonClicked(cRecipe recipe)
    {
        bool success = recipeManager.CraftRecipe(recipe.id);
        if (success)
        {
            Debug.Log("Crafting successful!");
        }
        else
        {
            Debug.Log("Crafting failed!");
        }
        
        DisplayAvailableRecipes();
    }
}
