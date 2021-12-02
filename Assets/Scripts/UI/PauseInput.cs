using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseInput : MonoBehaviour
{
    private PlayerControls _input;

    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject panelBlockInput;

    public bool bIsPaused;

    private void OnEnable()
    {
        _input?.Enable();
    }

    private void OnDisable()
    {
        _input?.Disable();
    }

    // Start is called before the first frame update
    void Awake()
    {
        _input = new PlayerControls();
        _input.Player.OpenMenu.performed += OpenMenu_performed;
    }

    public void SetPause(bool value)
    {
        bIsPaused = value;
        Time.timeScale = bIsPaused ? 0 : 1;

        if (pauseMenu != null)
            pauseMenu.SetActive(bIsPaused);

        if (panelBlockInput != null)
            panelBlockInput.SetActive(bIsPaused);
    }

    private void OpenMenu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        SetPause(!bIsPaused);
    }
}
