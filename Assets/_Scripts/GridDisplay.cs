using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GridDisplay : Singleton<GridDisplay>
{
    [SerializeField] public bool display = true;

    List<GameObject> allCells;

    public void ToggleDisplay()
    {
        foreach (var cell in allCells)
        {
            if (display == true)
            {
                cell.gameObject.SetActive(false);
                display = false;
            }
            else
            {
                cell.gameObject.SetActive(true);
                display = true;
            }
        }
    }

    public void CreateCell(int x, int y, float cellSize)
    {
        if (allCells == null)
            allCells = new List<GameObject>();

        GameObject newCell = Instantiate(Resources.Load("Prefabs/Cell") as GameObject, transform);
        allCells.Add(newCell);

        Transform cellT = newCell.GetComponent<Transform>();
    }
}