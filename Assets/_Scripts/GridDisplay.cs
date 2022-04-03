using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GridDisplay : Singleton<GridDisplay>
{
    public GameObject player;

    [SerializeField] public bool display = false;

    public GameObject[,] allCells;

    private void Update()
    {
        if (allCells == null)
            return;

        if (display == false)
            return;

        //do once every 120 frames
        if (Utility.RateLimiter(120))
        {
            for (int i = 0; i < allCells.GetLength(0); i++)
                for (int j = 0; j < allCells.GetLength(1); j++)
                {
                    if (Vector3.Distance(player.transform.position, allCells[i, j].transform.position) < 3.0f)
                        allCells[i, j].SetActive(true);
                    else
                        allCells[i, j].SetActive(false);
                }
        }
    }

    public void ToggleGridDisplay()
    {
        //if (display == true)
        //{
        //    for (int i = 0; i < allCells.GetLength(0); i++)
        //        for (int j = 0; j < allCells.GetLength(1); j++)
        //        {
        //            allCells[i, j].SetActive(false);
        //            display = false;
        //        }
        //}
        //else if (display == false)
        //{
        //    for (int i = 0; i < allCells.GetLength(0); i++)
        //        for (int j = 0; j < allCells.GetLength(1); j++)
        //        {
        //            allCells[i, j].SetActive(true);
        //            display = true;
        //        }
        //}
    }

    public void SetDisplayGrid(bool display)
    {
        for (int i = 0; i < allCells.GetLength(0); i++)
            for (int j = 0; j < allCells.GetLength(1); j++)
            {
                if (display)
                {
                    //allCells[i, j].SetActive(true);
                }
                else if (!display)
                    allCells[i, j].SetActive(false);
            }
    }

    public void ToggleDisplay()
    {
        if (!display)
            display = true;
        else
            display = false;

        SetDisplayGrid(display);
    }

    public void CreateCell(int x, int y, float cellSize, bool walkable)
    {
        if (allCells == null)
            allCells = new GameObject[Pathfinding.Instance.GetWidth(), Pathfinding.Instance.GetHeight()];

        allCells[x, y] = Instantiate(Resources.Load("Prefabs/Cell") as GameObject, transform);

        Transform cellT = allCells[x, y].GetComponent<Transform>();

        //Spawn depending on parent position, place in array, and with half width and height offset because the pivot is in the center instead of bottom left, spawning them incorrectly
        cellT.position = new Vector3(transform.position.x + (x * cellSize) + cellSize / 2, transform.position.y + (y * cellSize) + cellSize / 2, 0);

        TextMeshPro textMesh = allCells[x, y].GetComponentInChildren<TextMeshPro>();
        textMesh.text = "[" + x + ", " + y + "]";

        allCells[x, y].SetActive(false);
    }

    public void SetUnWalkableColor(int x, int y)
    {
        Debug.Log("X: " + x + " Y: " + y);
        SpriteRenderer spriteRenderer = allCells[x, y].GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = new Color(0.9607843f, 0.2313726f, 0.3411765f, 0.3490196f);
    }
}