using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI finalMoneyText;

    private void Start()
    {
        finalMoneyText.text = PlayerStats.instance.money.ToString();
    }
}
