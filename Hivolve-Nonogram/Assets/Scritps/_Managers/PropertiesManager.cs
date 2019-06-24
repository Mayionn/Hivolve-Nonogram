using System;
using static Enums;
using static Structs;

public class PropertiesManager
{
    public GameProperties GetRandomGameProperties(int size)
    {
        GameProperties gp = new GameProperties
        {
            SizeX = size,
            SizeY = size,
            BlackHoles = (Density)GetRandomValue(0, Enum.GetValues(typeof(Density))),
            OnePointers = (Density)GetRandomValue(1, Enum.GetValues(typeof(Density))),
            TwoPointers = (Density)GetRandomValue(0, Enum.GetValues(typeof(Density))),
            Multipliers2X = (Count)GetRandomValue(0, Enum.GetValues(typeof(Count))),
            Multipliers3X = (Count)GetRandomValue(0, Enum.GetValues(typeof(Count)))
        };

        return gp;
    }

    public static object GetRandomValue(int begin, Array someEnum)
    {
        return someEnum.GetValue(UnityEngine.Random.Range(begin, someEnum.Length));
    }
}