using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldCounter : MonoBehaviour
{
    private static int _count = 1;

    [SerializeField]
    private TextMeshProUGUI _tmpText;
    private void Awake()
    {
        if (_tmpText == null)
            _tmpText = this.GetComponent<TextMeshProUGUI>();

        UpdateText(_count.ToString());
        _count++;
    }

    public static void ResetCounter()
    {
        _count = 1;
    }
    
    public void UpdateText(string value)
    {
        _tmpText.text = value;
    }
}
