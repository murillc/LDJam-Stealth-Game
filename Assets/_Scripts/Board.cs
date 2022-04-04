using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Board
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int width;
    private int height;

    private float cellSize;

    private Vector3 originPosition;

    public PathNode[,] nodeArray;

    public Board(Vector3 originPosition, int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        nodeArray = new PathNode[width, height];

        for (int x = 0; x < nodeArray.GetLength(0); x++)
            for (int y = 0; y < nodeArray.GetLength(1); y++)
            {
                nodeArray[x, y] = new PathNode(this, x, y);

                GridDisplay.instance.CreateCell(x, y, cellSize, nodeArray[x, y].GetWalkable());
            }

        OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => 
        {
            Debug.Log(eventArgs.x + eventArgs.y);
        };
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition + originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition + originPosition).y / cellSize);
    }

    public void SetGridObject(int x, int y, PathNode value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            nodeArray[x, y] = value;
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public void SetGridObject(Vector3 worldPosition, PathNode value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public PathNode GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return nodeArray[x, y];
        }
        else
        {
            Debug.Log("X: " + x + " Y: " + y);
            return default(PathNode);
        }
    }

    public PathNode GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }
}
