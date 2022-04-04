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
    private bool hasTrap;

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

    public void SetHasTrap(bool hasTrap)
    {
        this.hasTrap = hasTrap;
    }

    public bool GetHasTrap()
    {
        return hasTrap;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(x, y, 0);
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}
