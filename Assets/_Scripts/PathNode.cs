using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private Board grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;

    public PathNode previousNode;

    public PathNode(Board grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;

        isWalkable = true;
    }

    public void SetWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }

    public bool GetWalkable()
    {
        return isWalkable;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}
