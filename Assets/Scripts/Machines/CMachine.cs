using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptable Objects/Machine")]
public class cMachine : cItem
{ 
    public static Dir GetNextDir(Dir dir) 
    {
        switch (dir) 
        {
            default:
            case Dir.Down:      return Dir.Left;
            case Dir.Left:      return Dir.Up;
            case Dir.Up:        return Dir.Right;
            case Dir.Right:     return Dir.Down;
        }
    }
    
    public enum Dir 
    {
        Down,
        Left,
        Up,
        Right,
    }
    
    
    [SerializeField]
    protected new string name;
    
    [SerializeField]
    protected string description;

    [SerializeField] 
    public GameObject machineModel;

    [SerializeField] 
    private int width;
    
    [SerializeField] 
    private int height;
    
    
    protected GameObject inventoryUiPanel;
    
    private Ray ray;
    
    RaycastHit hit;
    
    Vector3 mousePosition;

    public int GetHeight()
    {
        return height;
    }
    
    public int GetWidth()
    {
        return width;
    }
    

    public int GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return 0;
            case Dir.Left: return 90;
            case Dir.Up: return 180;
            case Dir.Right: return 270;
        }
    }

    public Vector2Int GetRotationOffset(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return new Vector2Int(0, 0);
            case Dir.Left: return new Vector2Int(0, width);
            case Dir.Up: return new Vector2Int(width, height);
            case Dir.Right: return new Vector2Int(height, 0);
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (dir)
        {
            default:
            case Dir.Down:
            case Dir.Up:
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
            case Dir.Left:
            case Dir.Right:
                for (int x = 0; x < height; x++)
                {
                    for (int y = 0; y < width; y++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
        }
        return gridPositionList;
    }

    public GameObject GetMachineModel
    {
        get { return machineModel; }
    }
}
