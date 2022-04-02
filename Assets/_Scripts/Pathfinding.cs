using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private Board<PathNode> grid;

    private List<PathNode> openList;
    private List<PathNode> closedList;

    public Pathfinding(Transform parentTransform, int width, int height, float cellSizeX = 1, float cellSizeY = 1)
    {
        grid = new Board<PathNode>(parentTransform, width, height, cellSizeX, cellSizeY, (Board<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public Board<PathNode> GetGrid()
    {
        return grid;
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        if (startNode == null || endNode == null)
        {
            // Invalid Path
            return null;
        }

        openList = new List<PathNode>() { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth();  x++)
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.previousNode = null;
            }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            PathNode currentNode = GetCheapestFCostPathNode(openList);

            if (currentNode == endNode)
                return CalculatePath(endNode);

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbour in GetNeighbours(currentNode))
            {
                if (closedList.Contains(neighbour))
                    continue;

                int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, neighbour);

                if (tentativeGCost < neighbour.gCost)
                {
                    neighbour.previousNode = currentNode;
                    neighbour.gCost = tentativeGCost;
                    neighbour.hCost = CalculateDistance(neighbour, endNode);
                    neighbour.CalculateFCost();

                    if (!openList.Contains(neighbour))
                        openList.Add(neighbour);
                }
            }
        }

        return null;
    }

    private List<PathNode> GetNeighbours(PathNode node)
    {
        List<PathNode> neighbours = new List<PathNode>();

        //Left
        if (node.x - 1 >= 0)
        {
            neighbours.Add(GetNode(node.x - 1, node.y));

            //Left down
            if (node.y - 1 >= 0)
                neighbours.Add(GetNode(node.x - 1, node.y - 1));

            //Left up
            if (node.y + 1 < grid.GetHeight())
                neighbours.Add(GetNode(node.x - 1, node.y + 1));
        }

        //Right
        if (node.x + 1 < grid.GetWidth())
        {
            neighbours.Add(GetNode(node.x + 1, node.y));

            //Right down
            if (node.y - 1 >= 0) 
                neighbours.Add(GetNode(node.x + 1, node.y - 1));

            //Right up
            if (node.y + 1 < grid.GetHeight())
                neighbours.Add(GetNode(node.x + 1, node.y + 1));
        }

        //Down
        if (node.y - 1 >= 0)
            neighbours.Add(GetNode(node.x, node.y - 1));

        //Up
        if (node.y + 1 < grid.GetHeight())
            neighbours.Add(GetNode(node.x, node.y + 1));

        return neighbours;
    }

    private PathNode GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;

        while(currentNode.previousNode != null)
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistance(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);

        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetCheapestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode cheapestFCostPathNode = pathNodeList[0];

        for (int i = 1; i < pathNodeList.Count; i++)
            if (pathNodeList[i].fCost < cheapestFCostPathNode.fCost)
                cheapestFCostPathNode = pathNodeList[i];

        return cheapestFCostPathNode;
    }
}
