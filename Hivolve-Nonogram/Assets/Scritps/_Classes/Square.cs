using static Enums;

public class Square
{
    public SquareType Type;
    public int Multiplier;
    public int BaseMultiplier;

    public Square(SquareType type)
    {
        this.Type = type;
        this.Multiplier = 1;
    }

    public void SetType(SquareType type)
    {
        this.Type = type;
    }
    public void SetMultiplier(int multiplier)
    {
        this.Multiplier = multiplier;
    }
    public void SetBaseMultiplier(int baseMultiplier)
    {
        this.BaseMultiplier = baseMultiplier;
    }
}
