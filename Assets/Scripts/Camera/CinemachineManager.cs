using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineManager : MonoBehaviour
{
    public static CinemachineManager Current;
    public CinemachineVirtualCamera Camera;

    // Start is called before the first frame update
    void Awake()
    {
        
        Current = Singleton<CinemachineManager>.Instance;

        Camera = GetComponent<CinemachineVirtualCamera>();

    }

    public void SetTarget(GameObject target)
    {
        if (Camera != null)
        {
            Camera.Follow = target.transform;
        }
    }
}
