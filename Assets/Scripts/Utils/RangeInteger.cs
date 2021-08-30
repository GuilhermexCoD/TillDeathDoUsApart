using System;

[Serializable]
public class RangeInteger
{
    public int Min;
    public int Max;
    public int Value; 
    public RangeInteger(int min, int max)
    {
        Min = min;
        Max = max;
        GenerateValue();
    }

    public void GenerateValue()
    {
        Value = new Random().Next(Min, Max);
    }

    public override string ToString()
    {
        return $"min: {Min} max: {Max} value: {Value}";
    }
}
