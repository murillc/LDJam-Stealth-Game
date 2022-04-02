using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    private Pathfinding pathfinding;

    private void Start()
    {
        pathfinding = new Pathfinding(transform, 18, 10, 1, 1);

        MakePath(0, 0, 8, 3);
    }

    private void Update()
    {
        //MakePath(0, 0, 5, 5);
    }

    public void MakePath(int startX, int startY, int endX, int endY)
    {
        List<PathNode> path = pathfinding.FindPath(startX, startY, endX, endY);
        //Debug.Log(path.Count);

        if (path != null)
            foreach(PathNode node in path)
            {
                Debug.Log(node.x + " " + node.y);
            }

        //for (int i = 0; i < path.Count - 1; i++)
        //{
        //    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 5.0f, true);
        //}
    }
}
