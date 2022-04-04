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

    private bool walkable;
    private bool inPlayerRange;

    public PathNode previousNode;

    public PathNode(Board grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;

        walkable = true;
    }

    public void SetWalkable(bool isWalkable)
    {
        this.walkable = isWalkable;
    }

    public bool GetWalkable()
    {
        return walkable;
    }

    public void SetInPlayerRange(bool inPlayerRange)
    {
        this.inPlayerRange = inPlayerRange;
    }

    public bool GetInPlayerRange()
    {
        return inPlayerRange;
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
