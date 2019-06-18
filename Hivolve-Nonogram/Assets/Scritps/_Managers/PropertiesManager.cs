using System;
using static Enums;
using static Structs;

public class PropertiesManager : Singleton<PropertiesManager>
{
    public GameProperties GetRandomGameProperties(int size)
    {
        GameProperties gp = new GameProperties
        {
            SizeX = size,
            SizeY = size,
            BlackHoles = (Density)GetRandomValue(0, Enum.GetValues(typeof(Density))),
            OnePointers = (Density)GetRandomValue(0, Enum.GetValues(typeof(Density))),
            TwoPointers = (Density)GetRandomValue(0, Enum.GetValues(typeof(Density))),
            Multipliers2X = (Count)GetRandomValue(0, Enum.GetValues(typeof(Density))),
            Multipliers3X = (Count)GetRandomValue(0, Enum.GetValues(typeof(Density)))
        };

        return gp;
    }
}