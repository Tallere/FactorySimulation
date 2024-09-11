using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public class cItem : ScriptableObject
{
    public Sprite image;
    
    public ItemType type;
    
    public int stackSize;
    
    public Actiontype actionType;

    public bool stackable = true;

    public int id;
    
    public bool smeltable = false;

}

public enum ItemType
{
    Tool,
    Material,
    Machine,
    Fuel,
    
}

public enum Actiontype
{
    Use,
    Equip,
    Placeable
}
