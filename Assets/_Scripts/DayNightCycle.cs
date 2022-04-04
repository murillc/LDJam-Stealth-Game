using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public enum CycleEnum
    {
        DAY,
        NIGHT
    }

    public CycleEnum currentCycle;
    private CycleEnum oldCycle;

    public GameObject dayObjects;
    public GameObject nightObjects;
    public GameObject playerLight;

    private void Awake()
    {
        currentCycle = CycleEnum.NIGHT;
        oldCycle = currentCycle;
    }

    void Start()
    {

    }

    void Update()
    {
        if (currentCycle != oldCycle)
        {
            switch (currentCycle)
            {
                case CycleEnum.DAY:
                    nightObjects.SetActive(false);
                    playerLight.SetActive(false);
                    dayObjects.SetActive(true);
                    break;

                case CycleEnum.NIGHT:
                    nightObjects.SetActive(true);
                    playerLight.SetActive(true);
                    dayObjects.SetActive(false);
                    break;

                default:
                    Debug.Log("DEFAULT, NO CYCLE??");
                    break;
            }

            oldCycle = currentCycle;
        }
    }
}
