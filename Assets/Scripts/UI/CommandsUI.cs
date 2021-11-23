using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsUI : MonoBehaviour
{

    private bool _bIsMobile { get { return SystemInfo.deviceType == DeviceType.Handheld; } }

    public GameObject mobileCommands;
    public GameObject desktopCommands;

    // Start is called before the first frame update
    void Awake()
    {
        mobileCommands.SetActive(_bIsMobile);
        desktopCommands.SetActive(!_bIsMobile);
    }
}
