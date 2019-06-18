using System;

public static class Enums
{
    public enum Density
    {
        None = 0,
        Low = 15,
        Medium = 30,
        High = 45
    }
    public enum Count
    {
        None = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4
    }
    public enum SquareType
    {
        Blank,
        BlackHole,
        OnePoint,
        TwoPoint,
        Multiplier2X,
        Multiplier3X
    }

    public static object GetRandomValue(int begin, Array someEnum)
    {
        return someEnum.GetValue(UnityEngine.Random.Range(begin, someEnum.Length));
    }
}