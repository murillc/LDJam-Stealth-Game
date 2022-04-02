using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    Grid grid;

    void Start()
    {
        grid = new Grid(20, 20, 16f);
    }
}
