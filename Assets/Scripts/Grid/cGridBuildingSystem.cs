using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class cGridBuildingSystem : MonoBehaviour
{
   [SerializeField] private List<cMachine> placedMachineList;
   
   private cMachine placedMachine;
   
   public static cGridBuildingSystem Instance { get; private set; }
   
   public event EventHandler OnSelectedChanged;
   public event EventHandler OnObjectPlaced;
   
   public cGrid<GridObject> grid;
   
   [SerializeField]
   Vector3 gridStartPosition;

   public cMachine.Dir dir = cMachine.Dir.Down; // defaults to down
   
   private void Awake()
    {
        Instance = this;

        int gridWidth = 20;
        int gridHeight = 20;
        float cellSize = 10f;

        grid = new cGrid<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, (cGrid<GridObject> g, int x, int z) => new GridObject(g, x, z));

        placedMachine = placedMachineList[0];
    }

    public bool CanBuildAtPosition(Vector3 position)
    {
        Vector2Int gridPosition = GetGridPosition(position);
        GridObject gridObject = grid.GetGridObject(gridPosition.x, gridPosition.y);
        return gridObject.CanBuild();
    }

    public class GridObject
    {
        private cGrid<GridObject> grid;
        private int x;
        private int z;
        private cPlacedObject placedObject;

        public GridObject(cGrid<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public override string ToString()
        {
            return x + ", " + z + "\n" + placedObject;
        }

        public void SetPlacedObject(cPlacedObject placedObject)
        {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, z);
        }

        public cPlacedObject GetPlacedObject()
        {
            return placedObject;
        }

        public void ClearPlacedObject()
        {
            placedObject = null;
            grid.TriggerGridObjectChanged(x, z);
        }

        public bool CanBuild()
        {
            return placedObject == null;
        }

        public void SetPosition(Vector3 worldPosition)
        {
            grid.GetXZ(worldPosition, out int x, out int z);
            this.x = x;
            this.z = z;
        }

        public Vector3 GetPosition()
        {
            return grid.GetWorldPosition(x, z);
        }
    }

    private void Update()
    {
        CanBuildCheck();

        if (Input.GetMouseButtonDown(2))
        {
            GridObject gridObject = grid.GetGridObject(cMouse3D.GetMouseWorldPosition());
            cPlacedObject placedObject = gridObject.GetPlacedObject();
            if (placedObject != null)
            {
                placedObject.DestroySelf();
                List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = cMachine.GetNextDir(dir);
            cUtils.CreateWorldTextPopup("" + dir, cMouse3D.GetMouseWorldPosition());
        }
    
        if (cInventory.Instance.isBuildMode)
        {
            var selectedItem = cInventory.Instance.GetSelectedItem();
            if (selectedItem != null && selectedItem.item.actionType == Actiontype.Placeable && selectedItem.item is cMachine)
            {
                placedMachine = selectedItem.item as cMachine;
            }
            else
            {
                placedMachine = null;
            }
            RefreshSelectedObjectType();
        }
        else
        {
            placedMachine = null;
            RefreshSelectedObjectType();
        }
    }

private void CanBuildCheck()
{
    if (cInventory.Instance.isBuildMode)
    {
        if (Input.GetMouseButtonDown(0))
        {
            var selectedItem = cInventory.Instance.GetSelectedItem();
            if (selectedItem == null || selectedItem.count <= 0 || !(selectedItem.item is cMachine))
            {
                return;
            }

            grid.GetXZ(cMouse3D.GetMouseWorldPosition(), out int x, out int z);
            List<Vector2Int> gridPositionList = placedMachine.GetGridPositionList(new Vector2Int(x, z), dir);
            bool canBuild = true;
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild)
            {
                Vector2Int rotationOffset = placedMachine.GetRotationOffset(dir);
                Vector3 placedMachineWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
                cPlacedObject placedObject = cPlacedObject.Create(placedMachineWorldPosition, new Vector2Int(x, z), dir, placedMachine);
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                }
                
                selectedItem.count--;
                if (selectedItem.count <= 0)
                {
                    Destroy(selectedItem.gameObject);
                }
                else
                {
                    selectedItem.RefreshCount();
                }
                
                cBuildingGhost.Instance.RefreshVisual();
            }
            else
            {
                cUtils.CreateWorldTextPopup("Cannot Build Here", cMouse3D.GetMouseWorldPosition());
            }
        }
    }
}
    public void RefreshSelectedObjectType()
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        grid.GetXZ(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public Vector3 GetMouseWorldSnappedPosition()
    {
        Vector3 mousePosition = cMouse3D.GetMouseWorldPosition();

        grid.GetXZ(mousePosition, out int x, out int z);

        if (placedMachine != null)
        {
            Vector2Int rotationOffset = placedMachine.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            return placedObjectWorldPosition;
        }
        else
        {
            return mousePosition;
        }
    }

    public Quaternion GetPlacedObjectRotation()
    {
        if (placedMachine != null)
        {
            return Quaternion.Euler(0, placedMachine.GetRotationAngle(dir), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }

    public cMachine GetPlacedMachine()
    {
        return placedMachine;
    }
   
}
