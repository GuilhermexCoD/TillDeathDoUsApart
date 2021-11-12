using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightLabel : Weight
{
    public string label;

    public WeightLabel(string label, int value) : base(value)
    {
        this.label = label;
    }
}
