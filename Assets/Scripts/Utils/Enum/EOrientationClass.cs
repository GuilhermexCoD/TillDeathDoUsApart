using System;

public class EOrientationClass : Enumerator
{
    public override string[] GetNames()
    {
        return GetNamesEnum<EOrientation>();
    }
    public override Array GetValues()
    {
        return GetValuesEnum<EOrientation>();
    }
}
