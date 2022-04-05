using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Slider heatSlider;

    [SerializeField] private TextMeshProUGUI moneyToGetText;
    [SerializeField] private TextMeshProUGUI heatToGetText;

    void FixedUpdate()
    {
        moneyText.text = PlayerStats.instance.money.ToString();
        heatSlider.value = PlayerStats.instance.heat;

        moneyToGetText.text = "$ - " + PlayerStats.instance.moneyToGet.ToString();
        heatToGetText.text = "H - " + PlayerStats.instance.heatToGet.ToString();
    }
}
