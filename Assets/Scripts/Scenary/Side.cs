using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side : MonoBehaviour
{
    private EOrientation Orientation;

    public Side(EOrientation orientation)
    {
        Orientation = orientation;
    }

    public EOrientation GetOrientation()
    {
        return Orientation;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
