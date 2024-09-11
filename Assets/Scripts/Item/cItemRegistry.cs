using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Registry", menuName = "Inventory/ItemRegistry")]
public class cItemRegistry : ScriptableObject
{
    public cItem[] items;

    private Dictionary<int, cItem> itemDictionary;
    
    
    public void Initialize()
    {
        itemDictionary = new Dictionary<int, cItem>();
        foreach (cItem item in items)
        {
            itemDictionary[item.id] = item;
        }
    }

    public cItem GetItemById(int id)
    {
        itemDictionary.TryGetValue(id, out cItem item);
        return item;
    }
    
    public cItem GetItemByName(string itemName)
    {
        foreach (var item in items)
        {
            if (item.name == itemName)
            {
                return item;
            }
        }
        return null;
    }
}
