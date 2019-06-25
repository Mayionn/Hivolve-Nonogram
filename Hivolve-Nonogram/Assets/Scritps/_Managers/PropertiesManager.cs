using System;
using static Enums;
using static Structs;

public class PropertiesManager : Singleton<PropertiesManager>
{
    public GameMode GameMode;

    public GameProperties CustomProperties;


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

    public void SetCustomProperties(int size, Density smallStars, Density bigStars, Density blackHoles, Count multipliers2X, Count multipliers3X)
    {
        CustomProperties = new GameProperties
        {
            SizeX = size,
            SizeY = size,
            OnePointers = smallStars,
            TwoPointers = bigStars,
            BlackHoles = blackHoles,
            Multipliers2X = multipliers2X,
            Multipliers3X = multipliers3X
        };
    }


    public static object GetRandomValue(int begin, Array someEnum)
    {
        return someEnum.GetValue(UnityEngine.Random.Range(begin, someEnum.Length));
    }
}