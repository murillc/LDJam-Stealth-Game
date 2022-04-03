using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    [SerializeField] private EnemyMovement enemyMovement;

    private Pathfinding pathfinding;

    private void Awake()
    {
        pathfinding = new Pathfinding(Vector3.zero, 30, 30, 1);
    }

    private void Start()
    {
        enemyMovement.SetTargetPosition(new Vector3(5.0f, 5.0f, 0.0f));
        MakePath(0, 0, 8, 3);
    }

    private void Update()
    {

    }

    public void MakePath(int startX, int startY, int endX, int endY)
    {
        List<PathNode> path = pathfinding.FindPath(startX, startY, endX, endY);
        //Debug.Log(path.Count);

        if (path != null)
            foreach(PathNode node in path)
            {
                //Debug.Log(node.x + " " + node.y);
            }
    }
}
