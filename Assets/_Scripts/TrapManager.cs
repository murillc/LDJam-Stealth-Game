using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    public GameObject trapPrefab;

    public void SpawnTrap(int posX, int posY)
    {
        if (Pathfinding.Instance.GetGrid().nodeArray[posX, posY].GetWalkable())
        {

        }

        GameObject trap = Instantiate(trapPrefab, transform);

        trap.transform.position = new Vector3(posX, posY, 0);
    }
}
