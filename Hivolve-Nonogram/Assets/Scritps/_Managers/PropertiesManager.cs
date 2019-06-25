using System;
using UnityEngine;
using static Enums;
using static Structs;

public class PropertiesManager : Singleton<PropertiesManager>
{
    public GameMode GameMode;

    public GameProperties CustomProperties;
    public int CustomMapReward;

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

        CustomMapReward = GetCustomReward();
    }
    public int GetCustomReward()
    {
        float rewardAmount = 0;
        int value;

        int smallStarReward = 1;
        int bigStarReward = 1;
        int blackHolesReward = 1;
        int multiplier2XReward = 2;
        int multiplier3XReward = 2;

        value = Array.IndexOf(Enum.GetValues(CustomProperties.OnePointers.GetType()), CustomProperties.OnePointers);
        rewardAmount += value * smallStarReward;
        value = Array.IndexOf(Enum.GetValues(CustomProperties.TwoPointers.GetType()), CustomProperties.TwoPointers);
        rewardAmount += value * bigStarReward;
        value = Array.IndexOf(Enum.GetValues(CustomProperties.BlackHoles.GetType()), CustomProperties.BlackHoles);
        rewardAmount += value * blackHolesReward;
        value = Array.IndexOf(Enum.GetValues(CustomProperties.Multipliers2X.GetType()), CustomProperties.Multipliers2X);
        rewardAmount += value * multiplier2XReward;
        value = Array.IndexOf(Enum.GetValues(CustomProperties.Multipliers3X.GetType()), CustomProperties.Multipliers3X);
        rewardAmount += value * multiplier3XReward;

        rewardAmount *= ((CustomProperties.SizeX / 2f) + 1);

        return (int)rewardAmount;
    }


    public static object GetRandomValue(int begin, Array someEnum)
    {
        return someEnum.GetValue(UnityEngine.Random.Range(begin, someEnum.Length));
    }
}