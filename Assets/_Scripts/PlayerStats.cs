using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    public int money;
    public float heat;
    public float heatPenalty;
    public int heatToGet = 0;
    public int moneyToGet = 0;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Start()
    {
    }

    public void CalculateHeatPenalty(float value)
    {
        heatToGet = (int)(value * 0.5f);
        moneyToGet = (int)(value * heatToGet);

        Debug.Log(heatToGet);
        Debug.Log(moneyToGet);
    }
}
