using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommandsUI : MonoBehaviour
{

    private bool _bIsMobile { get { return SystemInfo.deviceType == DeviceType.Handheld; } }

    public GameObject mobileCommands;
    public GameObject desktopCommands;
    public TextMeshProUGUI textDebug;

    // Start is called before the first frame update
    void Awake()
    {
        mobileCommands.SetActive(_bIsMobile);
        desktopCommands.SetActive(!_bIsMobile);

        textDebug.text = $"Debug: Running On {SystemInfo.deviceType} | {_bIsMobile}";
    }
}
