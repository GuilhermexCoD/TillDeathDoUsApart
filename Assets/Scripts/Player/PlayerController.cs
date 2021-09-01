using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [InspectorName("Dash")]
    public float DashCameraShakeMultiplier = 2;
    public float DashCameraShakeTime = 0.2f;
    private DashComponent dash;

    // Start is called before the first frame update
    void Awake()
    {
        if (!dash)
        {
            dash = GetComponent<DashComponent>();
        }

        dash.Subscribe(OnPlayerDashed);
    }

    public void OnPlayerDashed(Vector2 direction)
    {
        Debug.Log("Player Dashed");
        CinemachineShake.Current.ShakeCamera(direction.magnitude * DashCameraShakeMultiplier, 0.2f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
