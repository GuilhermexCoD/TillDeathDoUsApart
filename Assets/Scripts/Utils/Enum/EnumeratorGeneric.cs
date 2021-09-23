using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumeratorGeneric<EType> : Enumerator
{
    public override string[] GetNames()
    {
        return GetNamesEnum<EType>();
    }

    public override Array GetValues()
    {
        return GetValuesEnum<EType>();
    }
}
