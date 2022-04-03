using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindingManager : MonoBehaviour
{
    public Tilemap unWalkableTilemap;

    private Pathfinding pathfinding;

    private void Awake()
    {
        pathfinding = new Pathfinding(transform.position, 50, 30, 1);
    }

    private void Start()
    {
        //foreach (var position in unWalkableTilemap.cellBounds.allPositionsWithin)
        //{
        //    // Do stuff per position
        //}

        for (int i = unWalkableTilemap.cellBounds.xMin; i < unWalkableTilemap.cellBounds.xMax; i++)
        {
            if (i > pathfinding.GetGrid().nodeArray.GetLength(0))
                break;

            for (int j = unWalkableTilemap.cellBounds.yMin; j < unWalkableTilemap.cellBounds.yMax; j++)
            {
                if (j > pathfinding.GetGrid().nodeArray.GetLength(1))
                    break;

                Vector3Int localPos = new Vector3Int(i, j, (int)unWalkableTilemap.transform.position.z);
                Vector3 worldPos = unWalkableTilemap.CellToWorld(localPos);

                if (unWalkableTilemap.GetSprite(localPos) != null)
                {
                    Debug.Log("MADE TILE UNWALKABLE: " + worldPos);
                    pathfinding.GetGrid().nodeArray[(int)worldPos.x, (int)worldPos.y].SetWalkable(false);
                }
            }
        }

    }

    private void Update()
    {

    }
}
