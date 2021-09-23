using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialMenuUI : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private Transform canvasTransform;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < canvasTransform.childCount; i++)
        {
            var childTransform = canvasTransform.GetChild(i);
            if (childTransform.gameObject != mainMenu)
                childTransform.gameObject.SetActive(false);
        }

        mainMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
