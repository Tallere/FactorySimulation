using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cFlexibleGridLayout : LayoutGroup
{
    enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns
    }
    
    
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private Vector2 cellSize;
    [SerializeField] private Vector2 spacing;
    [SerializeField] private FitType fitType;
    
    [SerializeField] private bool fitX;
    [SerializeField] private bool fitY;
    
    
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {
            float squareRoot = Mathf.Sqrt(transform.childCount);
            rows = columns = Mathf.CeilToInt(squareRoot);
            switch (fitType)
            {
                case FitType.Width:
                    fitX = true;
                    fitY = false;
                    break;
                case FitType.Height:
                    fitX = false;
                    fitY = true;
                    break;
                case FitType.Uniform:
                    fitX = fitY = true;
                    break;
            }
        }

        if (fitType == FitType.Width || fitType == FitType.FixedColumns)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);
        }
        if (fitType == FitType.Height || fitType == FitType.FixedRows)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }
        

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = parentWidth / (float)columns - ((spacing.x / (float)columns) * (columns - 1))
            - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cellHeight = parentHeight / (float)rows - ((spacing.y / (float)rows) * (rows - 1))
            - (padding.top / (float)rows) - (padding.bottom / (float)rows); ;

        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);

        }
    }

    public override void CalculateLayoutInputVertical()
    {
        /*throw new System.NotImplementedException();*/
    }

    public override void SetLayoutHorizontal()
    {
        /*throw new System.NotImplementedException();*/
    }

    public override void SetLayoutVertical()
    {
        /*throw new System.NotImplementedException();*/
    }
}
