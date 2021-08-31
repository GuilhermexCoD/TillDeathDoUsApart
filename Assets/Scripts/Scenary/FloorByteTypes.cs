using System.Collections.Generic;

public static class FloorByteTypes
{
    private static HashSet<int> Top = new HashSet<int>
    {
        0b0111,
    };

    private static HashSet<int> SideLeft = new HashSet<int>
    {
        0b1110
    };

    private static HashSet<int> SideRight = new HashSet<int>
    {
        0b1011
    };

    private static HashSet<int> Bottom = new HashSet<int>
    {
        0b1101
    };

    private static HashSet<int> DiagonalCornerDownLeft = new HashSet<int>
    {
        0b1100,
    };

    private static HashSet<int> DiagonalCornerDownRight = new HashSet<int>
    {
        0b1001,
    };

    private static HashSet<int> DiagonalCornerUpLeft = new HashSet<int>
    {
        0b0110,
    };

    private static HashSet<int> DiagonalCornerUpRight = new HashSet<int>
    {
        0b0011,
    };

    private static HashSet<int> Full = new HashSet<int>
    {
        0b1111,
    };

    private static HashSet<int> UpDown = new HashSet<int>
    {
        0b0101, 
    };

    private static HashSet<int> RightLeft = new HashSet<int>
    {
        0b1010
    };

    private static HashSet<int> UUp = new HashSet<int>
    {
        0b0010
    };

    private static HashSet<int> UDown = new HashSet<int>
    {
        0b1000
    };

    private static HashSet<int> URight = new HashSet<int>
    {
        0b0001
    };

    private static HashSet<int> ULeft = new HashSet<int>
    {
        0b0100
    };

    public static Dictionary<EWallTileType, HashSet<int>> FloorCardinalDirections = new Dictionary<EWallTileType, HashSet<int>>
    {
        { EWallTileType.Up, Top },
        { EWallTileType.Left, SideLeft },
        { EWallTileType.Right, SideRight },
        { EWallTileType.Down, Bottom },

        { EWallTileType.DiagonalCornerDownRight, DiagonalCornerDownRight },
        { EWallTileType.DiagonalCornerDownLeft, DiagonalCornerDownLeft },

        { EWallTileType.DiagonalCornerUpRight, DiagonalCornerUpRight },
        { EWallTileType.DiagonalCornerUpLeft, DiagonalCornerUpLeft },

        { EWallTileType.Full, Full },
        { EWallTileType.RightLeft, RightLeft },
        { EWallTileType.UpDown, UpDown },

        { EWallTileType.UUp, UUp },
        { EWallTileType.UDown, UDown },
        { EWallTileType.URight, URight },
        { EWallTileType.ULeft, ULeft }

    };

    public static Dictionary<EWallTileType, HashSet<int>> FloorAllDirections = new Dictionary<EWallTileType, HashSet<int>>
    {
        { EWallTileType.Up, Top },
        { EWallTileType.Left, SideLeft },
        { EWallTileType.Right, SideRight },
        { EWallTileType.Down, Bottom },

        { EWallTileType.DiagonalCornerDownRight, DiagonalCornerDownRight },
        { EWallTileType.DiagonalCornerDownLeft, DiagonalCornerDownLeft },

        { EWallTileType.DiagonalCornerUpRight, DiagonalCornerUpRight },
        { EWallTileType.DiagonalCornerUpLeft, DiagonalCornerUpLeft },

        { EWallTileType.Full, Full },
    };

}
