using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    private void Start()
    {
        Pathfinding pathFinding = new Pathfinding(transform, 18, 10, 1, 1);
    }
}
