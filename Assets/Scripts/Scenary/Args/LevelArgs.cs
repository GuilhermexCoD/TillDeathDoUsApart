using System;
public class LevelArgs : EventArgs
{
    public int levelCount { get; set; }
    public LevelData data { get; set; }
}
