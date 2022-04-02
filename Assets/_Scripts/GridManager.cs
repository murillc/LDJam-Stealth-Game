using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    private Grid<PathNode> grid;

    public GameObject cell;

    public int width;
    public int height;

    public float cellSizeX;
    public float cellSizeY;

    void Start()
    {
        grid = new Grid<PathNode>(this, width, height, cellSizeX, cellSizeY, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    //Grid MakeGrid(int width, int height, float cellSizeX, float cellSizeY, Func<Grid<PathNode>, int, int, PathNode> createGridObject)
    //{
    //    return new Grid<PathNode>(this, width, height, 9, 5, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    //}
}
