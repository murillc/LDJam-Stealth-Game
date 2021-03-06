using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : Singleton<TrapManager>
{
    public int trapCost;
    public GameObject trapPrefab;

    public void SpawnTrap(int posX, int posY)
    {
        //if the grid display is off you cant spawn traps
        if (GridDisplay.instance.display == false)
            return;

        //if the trap location is out of range of the grid array then return
        if (Pathfinding.Instance.GetGrid().nodeArray.GetLength(0) < posX || Pathfinding.Instance.GetGrid().nodeArray.GetLength(1) < posY
            || 0 > Pathfinding.Instance.GetGrid().nodeArray.GetLength(0) || 0 > Pathfinding.Instance.GetGrid().nodeArray.GetLength(1))
        {
            Debug.Log("Trap spawn location out of range");
            return;
        }

        //if the grid node already has a trap on it, return
        if (Pathfinding.Instance.GetGrid().nodeArray[posX, posY].GetHasTrap())
            return;

        //don't allow to be placed on a unwalkable zone
        if (!Pathfinding.Instance.GetGrid().nodeArray[posX, posY].GetWalkable())
            return;

        if (PlayerStats.instance.money < trapCost)
        {
            Debug.Log("NO MONEY FOR TRAP");
            return;
        }

        //if the grid node is in range of the player you can spawn a trap on it
        if (Pathfinding.Instance.GetGrid().nodeArray[posX, posY].GetInPlayerRange())
        {

            Pathfinding.Instance.GetGrid().nodeArray[posX, posY].SetHasTrap(true);
            GameObject trap = Instantiate(trapPrefab, transform);
            trap.transform.position = new Vector3(posX + 0.5f, posY + 0.5f, 0);
            PlayerStats.instance.money -= trapCost;
            Debug.Log("x: " + posX + " y: " + posY);
        }
    }
}
