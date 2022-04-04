using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    public int money;
    public float heat;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        money = 0;
        heat = 0;
    }
}
