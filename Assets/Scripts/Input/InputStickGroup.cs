using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

public class InputStickGroup : MonoBehaviour
{
    [SerializeField]
    private OnScreenStick _stick;

    [SerializeField]
    private RectTransform _outSideArea;

    public void UpdateOutSideAreaSize()
    {
        if (_stick != null)
        {
            float insideRadius = _stick.GetComponent<RectTransform>().sizeDelta.x/2f;
            float outsideRadius = insideRadius + _stick.movementRange;
            _outSideArea.sizeDelta = Vector2.one * outsideRadius * 2;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateOutSideAreaSize();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
