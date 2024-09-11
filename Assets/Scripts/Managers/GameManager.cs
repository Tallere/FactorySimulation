using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public cItemRegistry itemRegistry;
    private cItem item;
    
    public cRecipeRegistry recipeRegistry;

    private void Awake()
    {
        itemRegistry.Initialize();
        recipeRegistry.Initialize();
        
    }
}
