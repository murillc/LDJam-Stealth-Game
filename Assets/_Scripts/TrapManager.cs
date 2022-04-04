using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : Singleton<TrapManager>
{
    public GameObject trapPrefab;

    public void SpawnTrap(int posX, int posY)
    {
        if (Pathfinding.Instance.GetGrid().nodeArray[posX, posY].GetInPlayerRange())
        {
            Debug.Log("SPAWNED TRAP");
            GameObject trap = Instantiate(trapPrefab, transform);
            trap.transform.position = new Vector3(posX + 0.5f, posY + 0.5f, 0);
            Debug.Log("x: " + posX + " y: " + posY);
        }
    }
}
