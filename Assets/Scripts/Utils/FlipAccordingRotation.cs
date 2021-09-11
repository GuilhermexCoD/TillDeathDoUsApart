using UnityEngine;

public class FlipAccordingRotation : MonoBehaviour
{
    Vector3 scale = Vector3.one;

    // Update is called once per frame
    void Update()
    {
        float angleOffset = Util.ModLoop(transform.localEulerAngles.z - 90f, 360);

        bool isRight = angleOffset > 180 && angleOffset < 360;

        if (isRight)
        {
            scale.y = -1f;
        }
        else
        {
            scale.y = 1f;
        }

        transform.localScale = scale;
    }
}
