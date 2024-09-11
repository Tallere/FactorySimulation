using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cGrid<TGridObject>
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }
    
        private int width;
        
        private int height;
        
        private float cellSize;
        
        [SerializeField]
        private Vector3 originPosition;
        
        private TGridObject[,] gridArray;
        
        private TextMesh[,] debugTextArray;
        
    
        public cGrid(int _width, int _height, float _cellSize, Vector3 _originPosition, Func<cGrid<TGridObject>, int, int, TGridObject> _createGridObject)
        {
            this.width = _width;
            this.height = _height;
            this.cellSize = _cellSize;
            this.originPosition = _originPosition;
    
            gridArray = new TGridObject[width, height];
            debugTextArray = new TextMesh[width, height];

            gridArray = new TGridObject[_width, _height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridArray.GetLength(1); z++)
                {
                    gridArray[x, z] = _createGridObject(this, x, z);
                }   
            }
            
            bool showDebug = false;
            if (showDebug)
            {
                TextMesh[,] debugTextArray = new TextMesh[_width, _height];
                
                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    for (int z = 0; z < gridArray.GetLength(1); z++)
                    {
                        // mostly for seeing the grid in the scene view
                    
                        debugTextArray[x, z] = cUtils.CreateWorldText(gridArray[x, z]?.ToString(), null, GetWorldPosition(x, z) + new Vector3(_cellSize, _cellSize) * 0.5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                        Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
                    }
                }
                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
            }
        }
    
        
        public int GetWidth() 
        {
            return width;
        }

        public int GetHeight() 
        {
            return height;
        }

        public float GetCellSize() {
            
            return cellSize;
        }
        
        
        public Vector3 GetWorldPosition(int _x, int _z)
        {
            return new Vector3(_x, 0, _z) * cellSize;
        }
        
        public void GetXZ(Vector3 worldPosition, out int _x, out int _z)
        {
            _x = Mathf.FloorToInt((worldPosition.x - originPosition.x) / cellSize);
            _z = Mathf.FloorToInt((worldPosition.z - originPosition.z) / cellSize);
            // this needs to return the x and z values so we do a out for the x and z
        }
    
        public void SetGridObject(int _x, int _z, TGridObject _value)
        {
            if (_x >= 0 && _z >= 0 && _x < width && _z < height)
            {
                gridArray[_x, _z] = _value;
                debugTextArray[_x, _z].text = gridArray[_x, _z].ToString();
                if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = _x, z = _z });
            }
        }
        
        public void TriggerGridObjectChanged(int x, int z)
        {
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, z = z });
        }
    
        public void SetGridObject(Vector3 _worldPosition, TGridObject _value)
        {
            int x, z;
            GetXZ(_worldPosition, out x, out z);
            SetGridObject(x, z, _value);
        }

        public TGridObject GetGridObject(int x, int z)
        {
            if (x >= 0 && z >= 0 && x < width && z < height)
            {
                return gridArray[x, z];
            }
            else
            {
                return default(TGridObject);
            }
        }
    
        
        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            int x, z;
            GetXZ(worldPosition, out x, out z);
            return GetGridObject(x, z);
        }
        
        
        
}
