using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    private Grid<PathNode> grid;

    public int width;
    public int height;

    public float cellSizeX;
    public float cellSizeY;

    void Start()
    {
        grid = new Grid<PathNode>(transform, width, height, cellSizeX = 1, cellSizeY = 1, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }
}
