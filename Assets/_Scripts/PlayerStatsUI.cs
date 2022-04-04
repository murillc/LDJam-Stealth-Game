using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

    void FixedUpdate()
    {
        moneyText.text = PlayerStats.instance.money.ToString();
    }
}
