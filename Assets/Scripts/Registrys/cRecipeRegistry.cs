using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeRegistry", menuName = "Inventory/RecipeRegistry")]
public class cRecipeRegistry : ScriptableObject
{
    public cRecipe[] recipes;

    private Dictionary<int, cRecipe> recipeDictionary;

    public void Initialize()
    {
        recipeDictionary = new Dictionary<int, cRecipe>();
        foreach (cRecipe recipe in recipes)
        {
            recipeDictionary[recipe.id] = recipe;
        }
    }

    public cRecipe GetRecipeById(int id)
    {
        recipeDictionary.TryGetValue(id, out cRecipe recipe);
        return recipe;
    }
}