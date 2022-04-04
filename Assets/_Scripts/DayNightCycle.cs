using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public enum Cycle
    {
        DAY,
        NIGHT
    }

    public Cycle currentCycle;

    private void Awake()
    {
        currentCycle = Cycle.NIGHT;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
