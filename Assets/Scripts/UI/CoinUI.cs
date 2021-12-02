using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        GameManager.current.onCoinChanged += OnCoinChanged;
        UpdateText(GameManager.GetCoin());
    }

    public void UpdateText(int coinValue)
    {
        if (coinText != null)
            coinText.text = coinValue.ToString();
    }

    private void OnCoinChanged(int value)
    {
        UpdateText(value);
    }
}
