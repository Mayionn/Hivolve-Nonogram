using System;
using System.Linq;
using UnityEngine;
using static Enums;
using static Structs;

public class Grid
{
    public Square[,] Squares;
    public int[] Objective_Rows;
    public int[] Objective_Columns;

    private int _sizeX;
    private int _sizeY;
    private Vector2[] _2XmultipliersPosition;
    private Vector2[] _3XmultipliersPosition;

    public Grid(GameProperties properties)
    {
        //Creates Rows and Columns
        _sizeX = properties.SizeX;
        _sizeY = properties.SizeY;
        Squares = new Square[_sizeX, _sizeY];
        Objective_Rows = new int[_sizeY];
        Objective_Columns = new int[_sizeX];
        //Initializes Squares Blank
        SetGrid(SquareType.Blank);
        //Generates Random Map
        GeneretateMap(properties);
        //Set Multipliers
        SetMultipliers(_2XmultipliersPosition, 2);
        SetMultipliers(_3XmultipliersPosition, 3);
        //Calculate Objectives
        Objective_Columns = CalculateColumns();
        Objective_Rows = CalculateRows();
        //Sets Values back to normal
        SetGrid(SquareType.Blank);
    }

    /// <summary>
    /// Checks if the game is over, counting all the point in each Line and comparing it with the objective
    /// </summary>
    /// <returns></returns>
    public bool IsGameComplete()
    {
        int[] total = CalculateColumns();
        if (!Enumerable.SequenceEqual(total, Objective_Columns)) return false;
        total = CalculateRows();
        if (!Enumerable.SequenceEqual(total, Objective_Rows)) return false;

        return true;
    }
    /// <summary>
    /// Checks if the chosen column is complete
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public bool CheckColumns(int x)
    {
        int[] columns = CalculateColumns();

        if (columns[x] == Objective_Columns[x])
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Checks if the chosen row is complete
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool CheckRows(int y)
    {
        int[] rows = CalculateRows();

        if (rows[y] == Objective_Rows[y])
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Generates a map based on the properties
    /// </summary>
    /// <param name="properties"></param>
    public void GeneretateMap(GameProperties properties)
    {
        float maxSquares = properties.SizeY * properties.SizeX;

        GenerateSquares(properties, SquareType.BlackHole, GetAmount(maxSquares, (int)properties.BlackHoles));
        GenerateSquares(properties, SquareType.OnePoint, GetAmount(maxSquares, (int)properties.OnePointers));
        GenerateSquares(properties, SquareType.TwoPoint, GetAmount(maxSquares, (int)properties.TwoPointers));

        if ((int)properties.Multipliers2X > 0)
        {
            _2XmultipliersPosition = new Vector2[(int)properties.Multipliers2X];
            GenerateSquares(properties, SquareType.Multiplier2X, (int)properties.Multipliers2X);
        }
        if ((int)properties.Multipliers3X > 0)
        {
            _3XmultipliersPosition = new Vector2[(int)properties.Multipliers3X];
            GenerateSquares(properties, SquareType.Multiplier3X, (int)properties.Multipliers3X);
        }
    }
    /// <summary>
    /// Interacts with the Square, the outcome depends on the actual Type;
    /// </summary>
    /// <param name="positionX"></param>
    /// <param name="positionY"></param>
    public void InteractSquare(int positionX, int positionY)
    {
        switch (Squares[positionX, positionY].Type)
        {
            case SquareType.Blank:
                Squares[positionX, positionY].SetType(SquareType.OnePoint);
                break;
            case SquareType.BlackHole:
                break;
            case SquareType.OnePoint:
                Squares[positionX, positionY].SetType(SquareType.TwoPoint);
                break;
            case SquareType.TwoPoint:
                Squares[positionX, positionY].SetType(SquareType.Blank);
                break;
            case SquareType.Multiplier2X:
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Sets all squares on board to Grid
    /// </summary>
    public void SetGrid(SquareType type)
    {
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                if (Squares[x, y] == null)
                {
                    Squares[x, y] = new Square(type);
                }
                else
                {
                    switch (Squares[x, y].Type)
                    {
                        case SquareType.Blank:
                            SetSquareType(x, y, type);
                            break;
                        case SquareType.BlackHole:
                            break;
                        case SquareType.OnePoint:
                            SetSquareType(x, y, type);
                            break;
                        case SquareType.TwoPoint:
                            SetSquareType(x, y, type);
                            break;
                        default:
                            break;
                    }
                }

            }
        }
    }
    
    /// <summary>
    /// Gets the amount of squares per density
    /// </summary>
    /// <param name="maxSquares"></param>
    /// <param name="density"></param>
    /// <returns></returns>
    private int GetAmount(float maxSquares, int density)
    {
        return Convert.ToInt32((maxSquares / 100) * density);
    }
    /// <summary>
    /// Returns the value of the square
    /// </summary>
    /// <param name="y"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    private int CalculateSquare(int x, int y)
    {
        Square square = Squares[x, y];
        switch (square.Type)
        {
            case SquareType.Blank:
                return 0 * square.Multiplier;
            case SquareType.BlackHole:
                return 0 * square.Multiplier;
            case SquareType.OnePoint:
                return 1 * square.Multiplier;
            case SquareType.TwoPoint:
                return 2 * square.Multiplier;
            case SquareType.Multiplier2X:
                return 0 * square.Multiplier;
            case SquareType.Multiplier3X:
                return 0 * square.Multiplier;
            default:
                Debug.Log("No square type selected");
                return 0;
        }
    }
    /// <summary>
    /// Calculate Columns Total
    /// </summary>
    /// <param name="size1"></param>
    /// <param name="size2"></param>
    /// <returns></returns>
    private int[] CalculateColumns()
    {
        int[] total = new int[_sizeX];

        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                total[x] += CalculateSquare(x, y);
            }
        }

        return total;
    }
    /// <summary>
    /// Calculate Row's Total
    /// </summary>
    /// <returns></returns>
    private int[] CalculateRows()
    {
        //TODO: DIVIDE IN 2 FUNCITONS, ONE FOR ROWS ANOTHER FOR COLUMNS, UNEQUAL NUMBERs CRASH THE METHOD
        int[] total = new int[_sizeY];

        for (int y = 0; y < _sizeY; y++)
        {
            for (int x = 0; x < _sizeX; x++)
            {
                total[y] += CalculateSquare(x, y);
            }
        }

        return total;
    }
    /// <summary>
    /// Goes through multiplier locations and sets Square0s Multipliers
    /// </summary>
    private void SetMultipliers(Vector2[] positions, int multiplier)
    {
        if (positions != null)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                for (int x = 0; x < _sizeX; x++)
                {
                    if (Squares[x, (int)positions[i].y].Multiplier > 1)
                    {
                        int currentMultiplier = Squares[x, (int)positions[i].y].Multiplier;
                        Squares[x, (int)positions[i].y].SetMultiplier(currentMultiplier * multiplier);
                    }
                    else
                    {
                        Squares[x, (int)positions[i].y].SetMultiplier(multiplier);
                    }
                }
                for (int y = 0; y < _sizeY; y++)
                {
                    if (Squares[(int)positions[i].x, y].Multiplier > 1)
                    {
                        int currentMultiplier = Squares[(int)positions[i].x, y].Multiplier;
                        Squares[(int)positions[i].x, y].SetMultiplier(currentMultiplier * multiplier);
                    }
                    else
                    {
                        Squares[(int)positions[i].x, y].SetMultiplier(multiplier);
                    }
                }
            }
        }
    }
    /// <summary>
    /// Generates Squares of a certain tye
    /// </summary>
    /// <param name="properties"></param>
    /// <param name="type"></param>
    /// <param name="totalAmount"></param>
    private void GenerateSquares(GameProperties properties, SquareType type, int totalAmount)
    {
        for (int i = 0; i < totalAmount; i++)
        {
            int x = UnityEngine.Random.Range(0, properties.SizeX);
            int y = UnityEngine.Random.Range(0, properties.SizeY);

            switch (type)
            {
                case SquareType.BlackHole:
                    switch (Squares[x, y].Type)
                    {
                        //Just needs this 2 because blackhole generation always goes first
                        case SquareType.Blank:
                            SetSquareType(x, y, type);
                            break;
                        //Decrementes the cycle so it generates another location
                        case SquareType.BlackHole:
                            i--;
                            break;
                        default:
                            break;
                    }
                    break;
                case SquareType.OnePoint:
                    switch (Squares[x, y].Type)
                    {
                        case SquareType.Blank:
                            SetSquareType(x, y, type);
                            break;
                        case SquareType.BlackHole:
                            i--;
                            break;
                        default:
                            break;
                    }
                    break;
                case SquareType.TwoPoint:
                    switch (Squares[x, y].Type)
                    {
                        case SquareType.Blank:
                            SetSquareType(x, y, type);
                            break;
                        case SquareType.BlackHole:
                            i--;
                            break;
                        case SquareType.OnePoint:
                            SetSquareType(x, y, type);
                            break;
                        default:
                            break;
                    }
                    break;
                case SquareType.Multiplier2X:
                    switch (Squares[x, y].Type)
                    {
                        case SquareType.BlackHole:
                            i--;
                            break;
                        case SquareType.Multiplier2X:
                            i--;
                            break;
                        default:
                            SetSquareType(x, y, type);
                            _2XmultipliersPosition[i] = new Vector2(x, y);
                            break;
                    }
                    break;
                case SquareType.Multiplier3X:
                    switch (Squares[x, y].Type)
                    {
                        case SquareType.BlackHole:
                            i--;
                            break;
                        case SquareType.Multiplier2X:
                            i--;
                            break;
                        case SquareType.Multiplier3X:
                            i--;
                            break;
                        default:
                            SetSquareType(x, y, type);
                            _3XmultipliersPosition[i] = new Vector2(x, y);
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// Sets a Square to a certain Type
    /// </summary>
    /// <param name="positionX"></param>
    /// <param name="positionY"></param>
    /// <param name="type"></param>
    private void SetSquareType(int positionX, int positionY, SquareType type)
    {
        Squares[positionX, positionY].SetType(type);
    }
    /// <summary>
    /// Sets square's multiplier
    /// </summary>
    /// <param name="positionX"></param>
    /// <param name="positionY"></param>
    /// <param name="multiplier"></param>
    private void SetSquareMultiplier(int positionX, int positionY, int multiplier)
    {
        Squares[positionX, positionY].SetMultiplier(multiplier);
    }
}
