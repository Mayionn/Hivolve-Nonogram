using System;
using UnityEngine;
using UnityEngine.UI;
using static Enums;
using static Structs;

public class GameManager : Singleton<GameManager>
{
    public Grid Game;
    public Canvas Screen;
    public Action ActionUpdate;
    public GameProperties Properties;

    private float screenX;
    private float screenY;
    private GameObject[,] _buttonGrid;
    private GameObject[] _rows;
    private GameObject[] _columns;

    void Start()
    {
        AssetsManager.Instance.Init();


        screenX = Screen.GetComponent<RectTransform>().sizeDelta.x;
        screenY = Screen.GetComponent<RectTransform>().sizeDelta.y;

        //Properties = PropertiesManager.Instance.GetRandomGameProperties(5);

        CreateGrid(Properties);
        DisplayGrid();
        SetAllLinesImage();
    }

    void Update() => ActionUpdate?.Invoke();

    public void Button_TestMapGeneration()
    {
        DestroyGrid();
        //Properties = PropertiesManager.Instance.GetRandomGameProperties(5);
        CreateGrid(Properties);
        DisplayGrid();
        SetAllLinesImage();
    }
    public void Button_ResetSquares()
    {
        GameManager.Instance.Game.SetGrid(SquareType.Blank);
        UpdateGrid();
        CheckGameCompleted();
    }
    public void Button_FillSquares()
    {
        GameManager.Instance.Game.SetGrid(SquareType.OnePoint);
        UpdateGrid();
        CheckGameCompleted();
    }
    public void InteractSquare(GameObject Button)
    {
        AudioManager.Instance.Play("Click");

        Vector2 Position = Button.GetComponent<SquarePosition>().Position;
        GameManager.Instance.Game.InteractSquare((int)Position.x, (int)Position.y);

        SetButtonImage(Button, Position);

        SetRowImage((int)Position.y);
        SetColumnImage((int)Position.x);

        CheckGameCompleted();
    }

    private void CreateGrid(GameProperties properties)
    {
        //Create Grid
        Game = new Grid(Properties);
        _buttonGrid = new GameObject[Properties.SizeX, Properties.SizeY];
        _rows = new GameObject[Properties.SizeY];
        _columns = new GameObject[Properties.SizeX];
    }
    private void DisplayGrid()
    {
        //Sets the size of the sprite based on the size of the screen
        float size = screenX / (Game.Squares.GetLength(0) + 1);
        float offSetY = screenY / 4;

        //Goes through all the squares and creates a grid of buttons
        for (int x = 0; x < Game.Squares.GetLength(0); x++)
        {
            GameObject column = AssetsManager.Instance.Line;
            _columns[x] = Instantiate(column, Vector2.zero, Quaternion.identity, Screen.transform);
            //Change Line Properties
            _columns[x].transform.name = "Columns" + x + ": " + Game.Objective_Columns[x];
            _columns[x].GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
            _columns[x].GetComponent<RectTransform>().anchoredPosition = new Vector2(size * (x+1) + (size / 2), size * Game.Squares.GetLength(1) + (size / 2) + offSetY);
            //Change Image and Number of the objective score
            _columns[x].GetComponent<Image>().sprite = AssetsManager.Instance.Skins[0].Line;
            _columns[x].transform.Find("Text").GetComponent<Text>().text = Game.Objective_Columns[x].ToString();


            for (int y = 0; y < Game.Squares.GetLength(1); y++)
            {
                //Get Button Prefab
                GameObject button = AssetsManager.Instance.Button;
                //Instantiate Square
                _buttonGrid[x, y] = Instantiate(button, Vector2.zero, Quaternion.identity, Screen.transform);
                //Change Button Properties
                _buttonGrid[x, y].transform.name = "Square" + x + y;
                _buttonGrid[x, y].GetComponent<RectTransform>().anchoredPosition = new Vector2(size * (x+1) + (size / 2), size * y + (size / 2) + offSetY);
                _buttonGrid[x, y].GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
                _buttonGrid[x, y].GetComponent<SquarePosition>().SetPosition(x, y);
                //Display correctSprite
                SetButtonImage(_buttonGrid[x, y], new Vector2(x, y));

                if(x == 0)
                {
                    GameObject row = AssetsManager.Instance.Line;
                    _rows[y] = Instantiate(row, Vector2.zero, Quaternion.identity, Screen.transform);
                    //Change Line Properties
                    _rows[y].transform.name = "Row" + y + ": " + Game.Objective_Rows[y];
                    _rows[y].GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
                    _rows[y].GetComponent<RectTransform>().anchoredPosition = new Vector2(size * x + (size / 2), size * y + (size / 2) + offSetY);
                    //Change Image and Number of the objective score
                    _rows[y].GetComponent<Image>().sprite = AssetsManager.Instance.Skins[0].Line;
                    _rows[y].transform.Find("Text").GetComponent<Text>().text = Game.Objective_Rows[y].ToString();
                }
            }
        }
    }
    private void DestroyGrid()
    {
        for (int x = 0; x < GameManager.Instance.Game.Squares.GetLength(0); x++)
        {
            Destroy(GameManager.Instance._columns[x]);
            for (int y = 0; y < GameManager.Instance.Game.Squares.GetLength(1); y++)
            {
                Destroy(GameManager.Instance._buttonGrid[x, y]);
                Destroy(GameManager.Instance._rows[y]);
            }
        }
    }
    private void SetAllLinesImage()
    {
        for (int x = 0; x < GameManager.Instance.Properties.SizeX; x++)
        {
            SetColumnImage(x);
        }
        for (int y = 0; y < GameManager.Instance.Properties.SizeY; y++)
        {
            SetRowImage(y);
        }
    }
    private void UpdateGrid()
    {
        for (int x = 0; x < _buttonGrid.GetLength(0); x++)
        {
            for (int y = 0; y < _buttonGrid.GetLength(1); y++)
            {
                SetButtonImage(_buttonGrid[x, y], new Vector2(x, y));
                SetRowImage(y);
                SetColumnImage(x);
            }
        }
    }

    private static void CheckGameCompleted()
    {
        if (GameManager.Instance.Game.IsGameComplete())
        {
            AudioManager.Instance.Play("GameComplete");
            Debug.Log("Won");
        }
    }
    private static void CheckMultiplier(GameObject Button, Vector2 position)
    {
        if (GameManager.Instance.Game.Squares[(int)position.x, (int)position.y].Multiplier > 1)
        {
            Button.transform.Find("Image").GetComponent<Image>().sprite = AssetsManager.Instance.Skins[0].MultiplierOverlay;
            Button.transform.Find("Image").GetComponent<Image>().color = new Vector4(1, 1, 1, 0.5f);
            Button.transform.Find("Text").GetComponent<Text>().text = GameManager.Instance.Game.Squares[(int)position.x, (int)position.y].Multiplier + "X";
        }
        else
        {
            SetNoMultiplier(Button);
        }
    }
    private static void SetRowImage(int y)
    {
        if (GameManager.Instance.Game.CheckRows(y))
        {
            GameManager.Instance._rows[y].GetComponent<Image>().sprite = AssetsManager.Instance.Skins[0].LineCompleted;
        }
        else
        {
            GameManager.Instance._rows[y].GetComponent<Image>().sprite = AssetsManager.Instance.Skins[0].Line;
        }
    }
    private static void SetColumnImage(int x)
    {
        if (GameManager.Instance.Game.CheckColumns(x))
        {
            GameManager.Instance._columns[x].GetComponent<Image>().sprite = AssetsManager.Instance.Skins[0].LineCompleted;
        }
        else
        {
            GameManager.Instance._columns[x].GetComponent<Image>().sprite = AssetsManager.Instance.Skins[0].Line;
        }
    }
    private static void SetButtonImage(GameObject Button, Vector2 position)
    {
        switch (GameManager.Instance.Game.Squares[(int)position.x, (int)position.y].Type)
        {
            case SquareType.Blank:
                Button.GetComponent<Image>().sprite = AssetsManager.Instance.Skins[0].Blank;
                CheckMultiplier(Button, position);
                break;
            case SquareType.BlackHole:
                Button.GetComponent<Image>().sprite = AssetsManager.Instance.Skins[0].BlackHole;
                SetNoMultiplier(Button);
                break;
            case SquareType.OnePoint:
                Button.GetComponent<Image>().sprite = AssetsManager.Instance.Skins[0].OnePoint;
                CheckMultiplier(Button, position);
                break;
            case SquareType.TwoPoint:
                Button.GetComponent<Image>().sprite = AssetsManager.Instance.Skins[0].TwoPoint;
                CheckMultiplier(Button, position);
                break;
            case SquareType.Multiplier2X:
                Button.GetComponent<Image>().sprite = AssetsManager.Instance.Skins[0].Multiplier2X;
                SetNoMultiplier(Button);
                break;
            case SquareType.Multiplier3X:
                Button.GetComponent<Image>().sprite = AssetsManager.Instance.Skins[0].Multiplier3X;
                SetNoMultiplier(Button);
                break;
            default:
                break;
        }
    }
    private static void SetNoMultiplier(GameObject Button)
    {
        Button.transform.Find("Image").GetComponent<Image>().sprite = null;
        Button.transform.Find("Image").GetComponent<Image>().color = Color.clear;
        Button.transform.Find("Text").GetComponent<Text>().text = "";
    }
}