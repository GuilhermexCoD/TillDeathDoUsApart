using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TraditionalRoomGeneratorTest
{
    [TestCase(1, 10)]
    [TestCase(2, 5)]
    [TestCase(3, 5)]
    [TestCase(4, 2)]
    [TestCase(5, 2)]
    [TestCase(6, 2)]
    [TestCase(7, 2)]
    [TestCase(8, 2)]
    [TestCase(9, 1)]
    public void IfTraditionalGeneratorWhenSetWidthUpdateHeightThenCheckWidthHeight(int width, int height)
    {
        //Arrange
        int size = width * height;
        ETheme theme = ETheme.Castle;
        Vector2Int start = Vector2Int.zero;

        //Act
        var traditional = new TraditionalRoomGenerator(start, theme, size, width, height);

        //Assert
        Assert.AreEqual(traditional.Width, width);
        Assert.AreEqual(traditional.Height, height);

    }
}
