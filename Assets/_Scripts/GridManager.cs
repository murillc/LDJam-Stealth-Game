using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : Singleton<GridManager>
{
    private Board<PathNode> grid;

    public int width;
    public int height;

    public float cellSize;

    void Start()
    {
        grid = new Board<PathNode>(Vector3.zero, width, height, cellSize, (Board<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public void CreateCell(int x, int y, float cellSize)
    {
        GameObject newCell = Instantiate(Resources.Load("Prefabs/Cell") as GameObject, transform);

        Transform cellT = newCell.GetComponent<Transform>();

        //Spawn depending on parent position, place in array, and with half width and height offset because the pivot is in the center instead of bottom left, spawning them incorrectly
        cellT.position = new Vector3(transform.position.x + (x * cellSize) + cellSize / 2, transform.position.y + (y * cellSize) + cellSize / 2, 0);

        TextMeshPro textMesh = newCell.GetComponentInChildren<TextMeshPro>();
        textMesh.text = "[" + x + ", " + y + "]";
    }
}
