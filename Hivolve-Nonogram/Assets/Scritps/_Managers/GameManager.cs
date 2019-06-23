using System;
using System.Collections;
using System.Collections.Generic;
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
        Properties = PropertiesManager.Instance.GetRandomGameProperties(5);
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
        float offSetY = screenY / 5;

        GameObject column = AssetsManager.Instance.Line;
        GameObject button = AssetsManager.Instance.Button;
        GameObject row = AssetsManager.Instance.Line;

        Transform columns = Screen.transform.Find("__Columns");
        Transform buttons = Screen.transform.Find("__Buttons");
        Transform rows = Screen.transform.Find("__Rows");

        //Goes through all the squares and creates a grid of buttons
        for (int x = 0; x < Game.Squares.GetLength(0); x++)
        {
            //----- ALL COLUMNS
            _columns[x] = Instantiate(column, Vector2.zero, Quaternion.identity, Screen.transform);
            //Change Line Properties
            _columns[x].transform.name = "Columns" + x + ": " + Game.Objective_Columns[x];
            _columns[x].transform.SetParent(columns);
            _columns[x].GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
            _columns[x].GetComponent<RectTransform>().anchoredPosition = new Vector2(size * (x + 1) + (size / 2), size * Game.Squares.GetLength(1) + (size / 2) + offSetY);
            //Change Image and Number of the objective score
            _columns[x].GetComponent<Image>().sprite = AssetsManager.Instance.ActiveSkin.Line;
            _columns[x].transform.Find("Text").GetComponent<Text>().text = Game.Objective_Columns[x].ToString();

            for (int y = 0; y < Game.Squares.GetLength(1); y++)
            {
                //----- ALL SQUARES
                _buttonGrid[x, y] = Instantiate(button, Vector2.zero, Quaternion.identity, Screen.transform);
                //Change Button Properties
                _buttonGrid[x, y].transform.name = "Square: " + x + y;
                _buttonGrid[x, y].transform.SetParent(buttons);
                _buttonGrid[x, y].GetComponent<RectTransform>().anchoredPosition = new Vector2(size * (x + 1) + (size / 2), size * y + (size / 2) + offSetY);
                _buttonGrid[x, y].GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
                _buttonGrid[x, y].GetComponent<SquarePosition>().SetPosition(x, y);
                //Display correctSprite
                SetButtonImage(_buttonGrid[x, y], new Vector2(x, y));

                if (x == 0)
                {
                    //----- ALL ROWS
                    _rows[y] = Instantiate(row, Vector2.zero, Quaternion.identity, Screen.transform);
                    //Change Line Properties
                    _rows[y].transform.name = "Row" + y + ": " + Game.Objective_Rows[y];
                    _rows[y].transform.SetParent(rows);
                    _rows[y].GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
                    _rows[y].GetComponent<RectTransform>().anchoredPosition = new Vector2(size * x + (size / 2), size * y + (size / 2) + offSetY);
                    //Change Image and Number of the objective score
                    _rows[y].GetComponent<Image>().sprite = AssetsManager.Instance.ActiveSkin.Line;
                    _rows[y].transform.Find("Text").GetComponent<Text>().text = Game.Objective_Rows[y].ToString();
                }
            }
        }

        Screen.transform.Find("Background").GetComponent<Image>().sprite = AssetsManager.Instance.ActiveSkin.Background;
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
    private void CheckGameCompleted()
    {
        if (GameManager.Instance.Game.IsGameComplete())
        {
            AudioManager.Instance.Play("GameComplete");
            Debug.Log("Won");

            //---- GET ALL POINT LOCATIONS
            List<Star> positions = GameManager.Instance.Game.GetPointsLocation();
            GameObject[] starLines = new GameObject[positions.Count];

            Transform parent = GameManager.Instance.Screen.transform.Find("__StarLines");

            //---- CREATE LINE RENDERERS
            for (int i = 0; i < positions.Count; i++)
            {
                //----- Get example and Instantiate
                starLines[i] = Instantiate(AssetsManager.Instance.StarLine);
                starLines[i].transform.SetParent(parent);

                //----- Get Positions ---- REWORK
                Vector2 position1 = GameManager.Instance._buttonGrid[(int)positions[0].Position.x, (int)positions[0].Position.y].GetComponent<RectTransform>().anchoredPosition;
                Vector2 position2 = GameManager.Instance._buttonGrid[(int)positions[1].Position.x, (int)positions[1].Position.y].GetComponent<RectTransform>().anchoredPosition;
                Debug.Log(GameManager.Instance._buttonGrid[(int)positions[0].Position.x, (int)positions[0].Position.y].transform.position);
                Debug.Log(GameManager.Instance._buttonGrid[(int)positions[1].Position.x, (int)positions[1].Position.y].transform.position);

                //----- Set line in the middle
                starLines[i].GetComponent<RectTransform>().anchoredPosition = position1 + (position2 - position1) / 2;

                //----- Set line size
                starLines[i].GetComponent<RectTransform>().sizeDelta = new Vector2(5, Vector2.Distance(position1, position2));
            }

        }
    }
    private static void SetRowImage(int y)
    {
        if (GameManager.Instance.Game.CheckRows(y))
        {
            GameManager.Instance._rows[y].GetComponent<Image>().sprite = AssetsManager.Instance.ActiveSkin.LineCompleted;
        }
        else
        {
            GameManager.Instance._rows[y].GetComponent<Image>().sprite = AssetsManager.Instance.ActiveSkin.Line;
        }
    }
    private static void SetColumnImage(int x)
    {
        if (GameManager.Instance.Game.CheckColumns(x))
        {
            GameManager.Instance._columns[x].GetComponent<Image>().sprite = AssetsManager.Instance.ActiveSkin.LineCompleted;
        }
        else
        {
            GameManager.Instance._columns[x].GetComponent<Image>().sprite = AssetsManager.Instance.ActiveSkin.Line;
        }
    }
    private static void SetButtonImage(GameObject Button, Vector2 position)
    {
        switch (GameManager.Instance.Game.Squares[(int)position.x, (int)position.y].Type)
        {
            case SquareType.Blank:
                Button.GetComponent<Image>().sprite = AssetsManager.Instance.ActiveSkin.Blank;
                CheckMultiplier(Button, position);
                break;
            case SquareType.BlackHole:
                Button.GetComponent<Image>().sprite = AssetsManager.Instance.ActiveSkin.BlackHole;
                SetNoMultiplier(Button);
                break;
            case SquareType.OnePoint:
                Button.GetComponent<Image>().sprite = AssetsManager.Instance.ActiveSkin.OnePoint;
                CheckMultiplier(Button, position);
                break;
            case SquareType.TwoPoint:
                Button.GetComponent<Image>().sprite = AssetsManager.Instance.ActiveSkin.TwoPoint;
                CheckMultiplier(Button, position);
                break;
            case SquareType.Multiplier:
                Button.GetComponent<Image>().sprite = AssetsManager.Instance.ActiveSkin.Multiplier;
                Button.transform.Find("MultiplierText").GetComponent<Text>().text = GameManager.Instance.Game.Squares[(int)position.x, (int)position.y].BaseMultiplier + "X";
                SetNoMultiplier(Button);
                break;
            default:
                break;
        }
    }
    private static void CheckMultiplier(GameObject Button, Vector2 position)
    {
        if (GameManager.Instance.Game.Squares[(int)position.x, (int)position.y].Multiplier > 1)
        {
            Button.transform.Find("Image").GetComponent<Image>().sprite = AssetsManager.Instance.Skins[0].MultiplierOverlay;
            Button.transform.Find("Image").GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
            Button.transform.Find("Text").GetComponent<Text>().text = GameManager.Instance.Game.Squares[(int)position.x, (int)position.y].Multiplier + "X";
        }
        else
        {
            SetNoMultiplier(Button);
        }
    }
    private static void SetNoMultiplier(GameObject Button)
    {
        Button.transform.Find("Image").GetComponent<Image>().sprite = null;
        Button.transform.Find("Image").GetComponent<Image>().color = Color.clear;
        Button.transform.Find("Text").GetComponent<Text>().text = "";
    }
}
