using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Inventory/Recipe")]
public class cRecipe : ScriptableObject
{

    public string recipeName;
    public cItem[] inputItems;
    public int[] inputAmounts;
    public cItem outputItem;
    public int outputAmount;
    public float smeltingTime;
    public float craftingTime;
    
    public int id;
}
