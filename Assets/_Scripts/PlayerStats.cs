using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void AddHeat(int heat)
    {
        this.heat += heat;

        EnemySpawner.instance.spawnRate = -0.17f * this.heat + 20;
        Debug.Log(EnemySpawner.instance.spawnRate);

        if (this.heat > 100)
        {
            PlayerStatsKeeper.money = money;
            SceneManager.LoadScene("Exit");
        } 
    }

    public void CalculateHeatPenalty(float value)
    {
        heatToGet = (int)(value * 0.5f);
        moneyToGet = (int)(value * heatToGet);
    }
}
