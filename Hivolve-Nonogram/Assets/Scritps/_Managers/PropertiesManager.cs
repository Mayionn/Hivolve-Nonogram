using System;
using UnityEngine;
using static Enums;
using static Structs;

public class PropertiesManager : Singleton<PropertiesManager>
{
    public GameType GameType;

    //----- Endless Mode
    public int CustomMapReward;
    public GameProperties CustomProperties;

    //----- Time Attack
  

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

    //----- Endless mode
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

        CustomMapReward = GetGameReward(CustomProperties);
    }

    //----- Time attack
    public GameProperties GetTimeAttackProperties()
    {
        TimeAttack tm = GameManager.Instance.GameMode as TimeAttack;
        int size = tm.CurrentStage + 2;
        Density onePointers = new Density();
        Density twoPointers = new Density();
        Density blackHoles = new Density();
        Count multipliers2X = new Count();
        Count multipliers3X = new Count();

        switch (tm.Dificulty)
        {
            case Dificulty.VeryEasy:
                #region VeryEasy
                blackHoles = Density.High;
                onePointers = Density.Low;
                twoPointers = Density.None;

                if (size == 3)
                {
                    multipliers2X = Count.None;
                    multipliers3X = Count.None;
                }
                else if (size < 5)
                {
                    multipliers2X = Count.None;
                    multipliers3X = Count.None;
                }
                else if (size < 7)
                {
                    multipliers2X = Count.None;
                    multipliers3X = Count.None;
                }
                else
                {
                    multipliers2X = Count.None;
                    multipliers3X = Count.None;
                }
                #endregion
                break;
            case Dificulty.Easy:
                #region Easy
                blackHoles = Density.High;
                onePointers = Density.Medium;
                twoPointers = Density.Low;

                if (size == 3)
                {
                    multipliers2X = Count.None;
                    multipliers3X = Count.None;
                }
                else if (size < 5)
                {
                    multipliers2X = Count.None;
                    multipliers3X = Count.None;
                }
                else if (size < 7)
                {
                    multipliers2X = Count.None;
                    multipliers3X = Count.None;
                }
                else
                {
                    multipliers2X = Count.One;
                    multipliers3X = Count.None;
                }
                #endregion
                break;
            case Dificulty.Medium:
                #region Medium
                blackHoles = Density.Medium;
                onePointers = Density.Medium;
                twoPointers = Density.Medium;

                if (size == 3)
                {
                    multipliers2X = Count.None;
                    multipliers3X = Count.None;
                }
                else if (size < 5)
                {
                    multipliers2X = Count.None;
                    multipliers3X = Count.None;
                }
                else if (size < 7)
                {
                    multipliers2X = Count.One;
                    multipliers3X = Count.None;
                }
                else
                {
                    multipliers2X = Count.One;
                    multipliers3X = Count.One;
                }
                #endregion
                break;
            case Dificulty.Hard:
                #region Hard
                blackHoles = Density.Low;
                onePointers = Density.High;
                twoPointers = Density.Medium;

                if (size == 3)
                {
                    multipliers2X = Count.None;
                    multipliers3X = Count.None;
                }
                else if (size < 5)
                {
                    multipliers2X = Count.One;
                    multipliers3X = Count.One;
                }
                else if (size < 7)
                {
                    multipliers2X = Count.Two;
                    multipliers3X = Count.Two;
                }
                else
                {
                    multipliers2X = Count.Two;
                    multipliers3X = Count.Two;
                }
                #endregion
                break;
            case Dificulty.VeryHard:
                #region VeryHard
                blackHoles = Density.None;
                onePointers = Density.High;
                twoPointers = Density.High;

                if (size == 3)
                {
                    multipliers2X = Count.One;
                    multipliers3X = Count.None;
                }
                else if( size < 5)
                {
                    multipliers2X = Count.Two;
                    multipliers3X = Count.Two;
                }
                else if(size < 7)
                {
                    multipliers2X = Count.Three;
                    multipliers3X = Count.Three;
                }
                else
                {
                    multipliers2X = Count.Four;
                    multipliers3X = Count.Four;
                }
                #endregion
                break;
            default:
                break;
        }

        GameProperties go = new GameProperties
        {
            SizeX = size,
            SizeY = size,
            OnePointers = onePointers,
            TwoPointers = twoPointers,
            BlackHoles = blackHoles,
            Multipliers2X = multipliers2X,
            Multipliers3X = multipliers3X
        };

        return go;
    }

    public int GetGameReward(GameProperties prop)
    {
        float rewardAmount = 0;
        int value;

        int smallStarReward = 1;
        int bigStarReward = 1;
        int blackHolesReward = 1;
        int multiplier2XReward = 2;
        int multiplier3XReward = 2;

        value = Array.IndexOf(Enum.GetValues(prop.OnePointers.GetType()), prop.OnePointers);
        rewardAmount += value * smallStarReward;
        value = Array.IndexOf(Enum.GetValues(prop.TwoPointers.GetType()), prop.TwoPointers);
        rewardAmount += value * bigStarReward;
        value = Array.IndexOf(Enum.GetValues(prop.BlackHoles.GetType()), prop.BlackHoles);
        rewardAmount += value * blackHolesReward;
        value = Array.IndexOf(Enum.GetValues(prop.Multipliers2X.GetType()), prop.Multipliers2X);
        rewardAmount += value * multiplier2XReward;
        value = Array.IndexOf(Enum.GetValues(prop.Multipliers3X.GetType()), prop.Multipliers3X);
        rewardAmount += value * multiplier3XReward;

        rewardAmount *= ((prop.SizeX / 2f) + 1);

        return (int)rewardAmount;
    }

    private static object GetRandomValue(int begin, Array someEnum)
    {
        return someEnum.GetValue(UnityEngine.Random.Range(begin, someEnum.Length));
    }
}