using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallByteTypes
{
    private static HashSet<int> wallTop = new HashSet<int>
    {
        0b1111,
        0b0110,
        0b0011,
        0b0010,
        0b1010,
        0b1100,
        0b1110,
        0b1011,
        0b0111
    };

    private static HashSet<int> wallSideLeft = new HashSet<int>
    {
        0b0100
    };

    private static HashSet<int> wallSideRight = new HashSet<int>
    {
        0b0001
    };

    private static HashSet<int> wallBottom = new HashSet<int>
    {
        0b1000
    };

    private static HashSet<int> wallInnerCornerDownLeft = new HashSet<int>
    {
        0b11110001,
        0b11100000,
        0b11110000,
        0b11100001,
        0b10100000,
        0b01010001,
        0b11010001,
        0b01100001,
        0b11010000,
        0b01110001,
        0b00010001,
        0b10110001,
        0b10100001,
        0b10010000,
        0b00110001,
        0b10110000,
        0b00100001,
        0b10010001
    };

    private static HashSet<int> wallInnerCornerDownRight = new HashSet<int>
    {
        0b11000111,
        0b11000011,
        0b10000011,
        0b10000111,
        0b10000010,
        0b01000101,
        0b11000101,
        0b01000011,
        0b10000101,
        0b01000111,
        0b01000100,
        0b11000110,
        0b11000010,
        0b10000100,
        0b01000110,
        0b10000110,
        0b11000100,
        0b01000010

    };

    private static HashSet<int> wallDiagonalCornerDownLeft = new HashSet<int>
    {
        0b01000000
    };

    private static HashSet<int> wallDiagonalCornerDownRight = new HashSet<int>
    {
        0b00000001
    };

    private static HashSet<int> wallDiagonalCornerUpLeft = new HashSet<int>
    {
        0b00010000,
        0b01010000,
    };

    private static HashSet<int> wallDiagonalCornerUpRight = new HashSet<int>
    {
        0b00000100,
        0b00000101
    };

    private static HashSet<int> wallFull = new HashSet<int>
    {
        0b1101,
        0b0101,
        0b1101,
        0b1001

    };

    private static HashSet<int> wallFullEightDirections = new HashSet<int>
    {
        0b00010100,
        0b11100100,
        0b10010011,
        0b01110100,
        0b00010111,
        0b00010110,
        0b00110100,
        0b00010101,
        0b01010100,
        0b00010010,
        0b00100100,
        0b00010011,
        0b01100100,
        0b10010111,
        0b11110100,
        0b10010110,
        0b10110100,
        0b11100101,
        0b11010011,
        0b11110101,
        0b11010111,
        0b11010111,
        0b11110101,
        0b01110101,
        0b01010111,
        0b01100101,
        0b01010011,
        0b01010010,
        0b00100101,
        0b00110101,
        0b01010110,
        0b11010101,
        0b11010100,
        0b10010101,
        0b00110111,
        0b01110111,
        0b01110011,
        0b01100111,
        0b01110110,
        0b00110010,
        0b00110110,
        0b00110011,
        0b01110010,
        0b00100111,
        0b01100110,
        0b01100011,
        0b00100011,
        0b00100110,
    };

    private static HashSet<int> wallBottmEightDirections = new HashSet<int>
    {
        0b01000001
    };

    public static Dictionary<EWallTileType, HashSet<int>> WallTypesCardinal = new Dictionary<EWallTileType, HashSet<int>>
    {
        { EWallTileType.Up, wallTop },
        { EWallTileType.Left, wallSideLeft },
        { EWallTileType.Right, wallSideRight },
        { EWallTileType.Down, wallBottom },

        //{ EWallTileType.InnerCornerDownRight, wallInnerCornerDownRight },
        //{ EWallTileType.InnerCornerDownLeft, wallInnerCornerDownLeft },

        //{ EWallTileType.DiagonalCornerDownRight, wallDiagonalCornerDownRight },
        //{ EWallTileType.DiagonalCornerDownLeft, wallDiagonalCornerDownLeft },

        //{ EWallTileType.DiagonalCornerUpRight, wallDiagonalCornerUpRight },
        //{ EWallTileType.DiagonalCornerUpLeft, wallDiagonalCornerUpLeft },

        { EWallTileType.Full, wallFull },
        //{ EWallTileType.FullEightDirections, wallFullEightDirections },
        //{ EWallTileType.BottomEightDirections, wallBottmEightDirections }
    };

    public static Dictionary<EWallTileType, HashSet<int>> WallTypesDiagonals = new Dictionary<EWallTileType, HashSet<int>>
    {
        { EWallTileType.InnerCornerDownRight, wallInnerCornerDownRight },
        { EWallTileType.InnerCornerDownLeft, wallInnerCornerDownLeft },

        { EWallTileType.DiagonalCornerDownRight, wallDiagonalCornerDownRight },
        { EWallTileType.DiagonalCornerDownLeft, wallDiagonalCornerDownLeft },

        { EWallTileType.DiagonalCornerUpRight, wallDiagonalCornerUpRight },
        { EWallTileType.DiagonalCornerUpLeft, wallDiagonalCornerUpLeft },

        { EWallTileType.Full, wallFull },
        { EWallTileType.FullEightDirections, wallFullEightDirections },
        { EWallTileType.BottomEightDirections, wallBottmEightDirections }
    };

}