using System.Collections;
using System.Collections.Generic;

public class Weight
{
    private int value;

    public static bool operator ==(Weight a, Weight b) => (a.value == b.value);
    public static bool operator !=(Weight a, Weight b) => (a.value != b.value);

    public override bool Equals(object obj)
    {
        if (obj != null || !(obj is Weight))
        {
            return false;
        }
        else
        {
            return this.value == ((Weight)obj).value;
        }
    }

    public override int GetHashCode()
    {
        return value.GetHashCode();
    }
}
