using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DayNightCycle : Singleton<DayNightCycle>
{
    public enum CycleEnum
    {
        DAY,
        NIGHT
    }

    public CycleEnum currentCycle;
    private CycleEnum oldCycle;

    [SerializeField] private Slider currentCycleTimeSlider;

    [SerializeField] private float timeToSwapCycles;
    [SerializeField] private float currentCycleTime;

    [SerializeField] private GameObject DocumentManager;
    private DocumentManager documentManager;

    public GameObject dayObjects;
    public GameObject nightObjects;
    public GameObject playerLight;
    public GameObject dayCanvasObjects;
    public GameObject dayIcon;
    public GameObject nightIcon;

    private void Awake()
    {
        //currentCycle = CycleEnum.NIGHT;
        oldCycle = currentCycle;
    }

    void Start()
    {
        currentCycleTime = timeToSwapCycles;
        documentManager = DocumentManager.GetComponent<DocumentManager>();
    }

    void Update()
    {
        if (currentCycleTime > 0)
        {
            currentCycleTime -= Time.deltaTime;
        }
        else
        {
            SwitchCycle();
            currentCycleTime = timeToSwapCycles;
        }

        currentCycleTimeSlider.value = currentCycleTime;

        if (currentCycle != oldCycle)
        {
            switch (currentCycle)
            {
                case CycleEnum.DAY:
                    EnemySpawner.instance.StopSpawningEnemies();
                    playerLight.SetActive(false);
                    nightObjects.SetActive(false);
                    nightIcon.SetActive(false);

                    dayIcon.SetActive(true);
                    dayObjects.SetActive(true);
                    dayCanvasObjects.SetActive(true);
                    break;

                case CycleEnum.NIGHT:
                    EnemySpawner.instance.StartSpawningEnemies();

                    playerLight.SetActive(true);
                    nightObjects.SetActive(true);
                    nightIcon.SetActive(true);

                    dayIcon.SetActive(false);
                    dayObjects.SetActive(false);
                    dayCanvasObjects.SetActive(false);

                    documentManager.SpawnDocumentRandom();
                    break;

                default:
                    Debug.Log("DEFAULT, NO CYCLE??");
                    break;
            }

            oldCycle = currentCycle;
        }
    }

    public void SwitchCycle()
    {
        if (currentCycle == CycleEnum.DAY)
        {
            PlayerStats.instance.AddHeat(PlayerStats.instance.heatToGet);
            currentCycle = CycleEnum.NIGHT;
        }
        else
        {
            foreach (Transform child in EnemySpawner.instance.transform)
            {
                Debug.Log("DESTROYED ENEMY");
                GameObject.Destroy(child.gameObject);
            }

            currentCycle = CycleEnum.DAY;
        }

        PlayerStats.instance.money += PlayerStats.instance.moneyToGet;

        //PlayerStats.instance.heatToGet = 0;
        PlayerStats.instance.moneyToGet = 0;
    }

    public void SetCycle(CycleEnum cycle)
    {
        this.currentCycle = cycle;
    }
}
