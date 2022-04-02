using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private Grid<PathNode> grid;

    private List<PathNode> openList;
    private List<PathNode> closedList;

    public Pathfinding(Transform parentTransform, int width, int height, float cellSizeX = 1, float cellSizeY = 1)
    {
        grid = new Grid<PathNode>(parentTransform, width, height, cellSizeX, cellSizeY, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    //private List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    //{
    //    PathNode startNode = grid.GetGridObject(startX, startY);

    //    openList = new List<PathNode>() {  startNode };
    //    closedList = new List<PathNode>();

    //    for (int x = 0; x < grid.GetWidth();  x++)
    //        for (int y = 0; y < grid.GetHeight(); y++)
    //        {
    //            PathNode pathNode = grid.GetGridObject(x, y);
    //            pathNode.gCost = int.MaxValue;
    //            //pathNode.CalculateFCost();
    //        }
    //}
}
