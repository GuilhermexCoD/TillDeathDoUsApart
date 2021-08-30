using UnityEngine;

public class TraditionalRoomGenerator : GeneratorRule
{
    public int Width;
    public int Height;
    public TraditionalRoomGenerator(Vector2Int start, ETheme theme, int size, params object[] args) : base(start, theme, size)
    {
        this.Width = (int)args[0];
        this.Height = (int)args[1];
    }

    public TraditionalRoomGenerator(Vector2Int start, ETheme theme, int size, int width, int height) : base(start, theme, size)
    {
        this.Width = width;
        this.Height = height;
    }

    public override void Generate()
    {
        for (int w = 0; w < Width; w++)
        {
            for (int h = 0; h < Height; h++)
            {
                var coord = new Vector2Int((w + start.x), h + start.y);

                CoordinateGenerated(coord);

            }
        }
    }
}
