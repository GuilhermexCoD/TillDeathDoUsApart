using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Current;

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    private float Intensity;
    private float ShakeTimerTotal;
    private float ShakeTimer;
    private bool Smooth;

    private void Awake()
    {
        Current = Singleton<CinemachineShake>.Instance;

        if (!cinemachineVirtualCamera)
        {
            cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        if (!cinemachineBasicMultiChannelPerlin)
        {
            cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    public void ShakeCamera(float intensity, float time, bool smooth = false)
    {
        if (cinemachineBasicMultiChannelPerlin)
        {
            Intensity = intensity;
            Smooth = smooth;
            ShakeTimerTotal = time;
            ShakeTimer = ShakeTimerTotal;

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Intensity;
        }
    }

    private void Update()
    {
        if (ShakeTimer > 0)
        {
            ShakeTimer -= Time.deltaTime;

            if (Smooth)
            {
                float lerp = ShakeTimer / ShakeTimerTotal;
                float intensity = Mathf.Lerp(0, Intensity, lerp);
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            }

            if (ShakeTimer <= 0)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
    }
}
