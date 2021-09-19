using System;

public class EDifficultyClass : Enumerator
{
    public override string[] GetNames()
    {
        return GetNamesEnum<EDifficulty>();
    }
    public override Array GetValues()
    {
        return GetValuesEnum<EDifficulty>();
    }
}
