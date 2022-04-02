using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Board<T>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int width;
    private int height;

    private float cellSizeX;
    private float cellSizeY;

    public GameObject cell;

    //private Vector3 originPosition;

    private T[,] gridArray;

    private Transform parentTransform;

    //public GameObject[,] cells;

    public Board(Transform parentTransform, int width, int height, float cellSizeX, float cellSizeY, /*Vector3 originPosition,*/ Func<Board<T>, int, int, T> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSizeX = cellSizeX;
        this.cellSizeY = cellSizeY;
        this.parentTransform = parentTransform;

        gridArray = new T[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                //GameObject newCell = Instantiate(Resources.Load("Prefabs/Cell") as GameObject, parentTransform);
                ////cells[x, y] = newCell;

                //Transform cellT = newCell.GetComponent<Transform>();

                ////Spawn depending on parent position, place in array, and with half width and height offset because the pivot is in the center instead of bottom left, spawning them incorrectly
                //cellT.position = new Vector3(parentTransform.position.x + (x * cellSizeX) + cellSizeX / 2, parentTransform.position.y + (y * cellSizeY) + cellSizeY / 2, 0);

                //TextMeshPro textMesh = newCell.GetComponentInChildren<TextMeshPro>();
                //textMesh.text = "[" + x + ", " + y + "]";

                gridArray[x, y] = createGridObject(this, x, y);
            }

        OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => 
        {
            Debug.Log(eventArgs.x + eventArgs.y);
        };
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public void SetGridObject(int x, int y, T value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public T GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(T);
        }
    }

    //public Vector3 GetWorldPosition(int x, int y)
    //{
    //    return new Vector3(x, y) * cellSize + originPosition;
    //}

    //private void GetXY(Vector3 worldPosition, out int x, out int y)
    //{
    //    x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
    //    y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    //}

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
}
