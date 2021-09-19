using System;
using UnityEngine;

public class Enumerator : MonoBehaviour , IEnumerator
{
    public virtual string[] GetNames()
    {
        return null;
    }

    protected string[] GetNamesEnum<T>()
    {
        return Enum.GetNames(typeof(T));
    }

    protected Array GetValuesEnum<T>()
    {
        return Enum.GetValues(typeof(T));
    }

    public virtual Array GetValues()
    {
        return null;
    }
}
