using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Enums;
using static Structs;

public class GameManager : SingletonDestroyable<GameManager>
{
    public GameGrid Game;
    public Canvas Screen;
    public GameMode GameMode;

    private float screenX;
    private float screenY;
    private GameObject[,] _buttonGrid;
    private GameObject[] _rows;
    private GameObject[] _columns;
    private GameObject[] _starLines;

    void Start()
    {
        AssetsManager.Instance.Init();

        screenX = Screen.GetComponent<RectTransform>().sizeDelta.x;
        screenY = Screen.GetComponent<RectTransform>().sizeDelta.y;

        switch (PropertiesManager.Instance.GameType)
        {
            case GameType.Singleplayer:
                break;
            case GameType.Endless:
                GameMode = PropertiesManager.Instance.gameObject.GetComponent<Endless>();
                break;
            case GameType.TimeAttack:
                GameMode = PropertiesManager.Instance.gameObject.GetComponent<TimeAttack>();
                break;
            default:
                break;
        }
        GameMode.Init();

        InitGrid();
        InstantiateGrid();
    }

    public void GenerateNextMap()
    {
        DestroyGrid();

        GameMode.SetMapProperties();

        InitGrid();
        InstantiateGrid();
    }
    public void Button_LeaveGame()
    {
        GameMode.LeaveGame();

        SceneManager.LoadScene(0);
    }
    public void Button_ResetSquares()
    {
        Game.SetGrid(SquareType.Blank);
        UpdateGridImages();
        CheckGameCompleted();
        DestroyStarLines();
    }
    public void Button_FillSquares()
    {
        Game.SetGrid(SquareType.OnePoint);
        UpdateGridImages();
        CheckGameCompleted();
        DestroyStarLines();
    }
    public void Button_Interaction(GameObject button)
    {
        //---- Since this method is called on a prefab, we call the scene's GameManager.
        GameManager.Instance.SquareInteraction(button);
    }

    private void SquareInteraction(GameObject button)
    {
        //----- Play interaction audio
        AudioManager.Instance.Play("Click");

        //----- Gets position on tile
        Vector2 pos = button.GetComponent<SquarePosition>().Position;

        //----- Interacts with the square, outcome depends on the square type
        Game.InteractSquare((int)pos.x, (int)pos.y);

        UpdateButtonImage(button, pos);
        UpdateRowImage((int)pos.y);
        UpdateColumnImage((int)pos.x);

        //----- Verify if game is complete
        CheckGameCompleted();
    }

    /// <summary>
    /// Initializes all grid variables
    /// </summary>
    private void InitGrid()
    {
        Game = new GameGrid(GameMode.Properties);
        _buttonGrid = new GameObject[GameMode.Properties.SizeX, GameMode.Properties.SizeY];
        _rows = new GameObject[GameMode.Properties.SizeY];
        _columns = new GameObject[GameMode.Properties.SizeX];
    }
    /// <summary>
    /// Instantiates grid to screen
    /// </summary>
    private void InstantiateGrid()
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
            _columns[x].GetComponent<Image>().sprite = ProfileManager.Instance.ActiveSkin.Line;
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
                UpdateButtonImage(_buttonGrid[x, y], new Vector2(x, y));

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
                    _rows[y].GetComponent<Image>().sprite = ProfileManager.Instance.ActiveSkin.Line;
                    _rows[y].transform.Find("Text").GetComponent<Text>().text = Game.Objective_Rows[y].ToString();
                }
            }
        }

        Screen.transform.Find("Background").GetComponent<Image>().sprite = ProfileManager.Instance.ActiveSkin.Background;

        UpdateLinesImage();
    }
    /// <summary>
    /// Destroy's whole grid
    /// </summary>
    private void DestroyGrid()
    {
        for (int x = 0; x < Game.Squares.GetLength(0); x++)
        {
            Destroy(_columns[x]);
            for (int y = 0; y < Game.Squares.GetLength(1); y++)
            {
                Destroy(_buttonGrid[x, y]);
                Destroy(_rows[y]);
            }
        }

        DestroyStarLines();
    }
    /// <summary>
    /// Destroy star lines if any on screen
    /// </summary>
    private void DestroyStarLines()
    {
        if (_starLines != null)
        {
            for (int i = 0; i < _starLines.Length; i++)
            {
                Destroy(_starLines[i]);
            }
        }
    }
    /// <summary>
    /// Verifies if all objectives have been achieved, and executes win animations
    /// </summary>
    private void CheckGameCompleted()
    {
        if (Game.IsGameComplete())
        {
            AudioManager.Instance.Play("GameComplete");

            //---- GET ALL POINT LOCATIONS
            List<Vector2> stars = Game.GetPointsLocation();
            _starLines = new GameObject[stars.Count];

            Transform parent = Screen.transform.Find("__StarLines");

            //---- Create lines between all points
            for (int i = 0; i < stars.Count; i++)
            {
                if (i + 1 != stars.Count)
                {
                    //----- Get example and Instantiate with "__StarLines" as parent
                    _starLines[i] = Instantiate(AssetsManager.Instance.StarLine);
                    _starLines[i].transform.SetParent(parent);

                    Vector2 position1 = Vector2.zero;
                    Vector2 position2 = Vector2.zero;

                    //----- Get positions -- REWORK
                    int random = UnityEngine.Random.Range(0, stars.Count);
                    position1 = _buttonGrid[(int)stars[random].x, (int)stars[random].y].GetComponent<RectTransform>().anchoredPosition;
                    do
                    {
                        random = UnityEngine.Random.Range(0, stars.Count);
                        position2 = _buttonGrid[(int)stars[random].x, (int)stars[random].y].GetComponent<RectTransform>().anchoredPosition;
                    }
                    while (position2 == position1);

                    //----- Set line in the middle
                    _starLines[i].GetComponent<RectTransform>().anchoredPosition = position1 + (position2 - position1) / 2f;

                    //----- Rotate line
                    float xDiff = position2.x - position1.x;
                    float yDiff = position1.y - position2.y;
                    double angle = Math.Atan2(xDiff, yDiff) * 180f / Math.PI;
                    _starLines[i].GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, (float)angle);

                    //----- Set line size
                    float lineThickness = 10;
                    float distance = Vector2.Distance(position1, position2);
                    _starLines[i].GetComponent<RectTransform>().sizeDelta = new Vector2(lineThickness, distance + distance / 3);
                }
            }

            DisableAllButtons();

            StartCoroutine(GameMode.MapCompletion());
        }
    }

    public void FadeOutUI()
    {
        for (int x = 0; x < GameMode.Properties.SizeX; x++)
        {
            for (int y = 0; y < GameMode.Properties.SizeY; y++)
            {
                _buttonGrid[x, y].GetComponent<Animation>().Play("ButtonFadeOut");
            }
        }

        for (int i = 0; i < _rows.Length; i++)
        {
            _rows[i].GetComponent<Animation>().Play("LineSquareFadeOut");
        }

        for (int i = 0; i < _columns.Length; i++)
        {
            _columns[i].GetComponent<Animation>().Play("LineSquareFadeOut");
        }
    }
    /// <summary>
    /// Disables all buttons interaction
    /// </summary>
    public void DisableAllButtons()
    {
        for (int x = 0; x < GameMode.Properties.SizeX; x++)
        {
            for (int y = 0; y < GameMode.Properties.SizeY; y++)
            {
                _buttonGrid[x, y].GetComponent<Button>().enabled = false;
            }
        }
    }
    /// <summary>
    /// Updates whole grid's button images, row images and column images
    /// </summary>
    private void UpdateGridImages()
    {
        for (int x = 0; x < _buttonGrid.GetLength(0); x++)
        {
            for (int y = 0; y < _buttonGrid.GetLength(1); y++)
            {
                UpdateButtonImage(_buttonGrid[x, y], new Vector2(x, y));
                UpdateRowImage(y);
                UpdateColumnImage(x);
            }
        }
    }
    /// <summary>
    /// Updates Button image, depending on InteractSquare outcome
    /// </summary>
    /// <param name="Button"></param>
    /// <param name="position"></param>
    private void UpdateButtonImage(GameObject Button, Vector2 position)
    {
        switch (Game.Squares[(int)position.x, (int)position.y].Type)
        {
            case SquareType.Blank:
                Button.GetComponent<Image>().sprite = ProfileManager.Instance.ActiveSkin.Blank;
                CheckMultiplier(Button, position);
                break;
            case SquareType.BlackHole:
                Button.GetComponent<Image>().sprite = ProfileManager.Instance.ActiveSkin.BlackHole;
                SetNoMultiplier(Button);
                break;
            case SquareType.OnePoint:
                Button.GetComponent<Image>().sprite = ProfileManager.Instance.ActiveSkin.OnePoint;
                CheckMultiplier(Button, position);
                break;
            case SquareType.TwoPoint:
                Button.GetComponent<Image>().sprite = ProfileManager.Instance.ActiveSkin.TwoPoint;
                CheckMultiplier(Button, position);
                break;
            case SquareType.Multiplier:
                Button.GetComponent<Image>().sprite = ProfileManager.Instance.ActiveSkin.Multiplier;
                Button.transform.Find("MultiplierText").GetComponent<Text>().text = Game.Squares[(int)position.x, (int)position.y].BaseMultiplier + "X";
                SetNoMultiplier(Button);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Updates row line image, if objective is complete
    /// </summary>
    /// <param name="y"></param>
    private void UpdateRowImage(int y)
    {
        if (Game.CheckRows(y))
        {
           _rows[y].GetComponent<Image>().sprite = ProfileManager.Instance.ActiveSkin.LineCompleted;
        }
        else
        {
            _rows[y].GetComponent<Image>().sprite = ProfileManager.Instance.ActiveSkin.Line;
        }
    }
    /// <summary>
    /// Updates column line image, if objective is compelte
    /// </summary>
    /// <param name="x"></param>
    private void UpdateColumnImage(int x)
    {
        if (Game.CheckColumns(x))
        {
            _columns[x].GetComponent<Image>().sprite = ProfileManager.Instance.ActiveSkin.LineCompleted;
        }
        else
        {
            _columns[x].GetComponent<Image>().sprite = ProfileManager.Instance.ActiveSkin.Line;
        }
    }
    /// <summary>
    /// Changes all line's sprite if the objective as been meet
    /// </summary>
    private void UpdateLinesImage()
    {
        for (int x = 0; x < GameMode.Properties.SizeX; x++)
        {
            UpdateColumnImage(x);
        }
        for (int y = 0; y < GameMode.Properties.SizeY; y++)
        {
            UpdateRowImage(y);
        }
    }

    private void CheckMultiplier(GameObject Button, Vector2 position)
    {
        if (Game.Squares[(int)position.x, (int)position.y].Multiplier > 1)
        {
            Button.transform.Find("Image").GetComponent<Image>().sprite = ProfileManager.Instance.ActiveSkin.MultiplierOverlay;
            Button.transform.Find("Image").GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
            Button.transform.Find("Text").GetComponent<Text>().text = Game.Squares[(int)position.x, (int)position.y].Multiplier + "X";
        }
        else
        {
            SetNoMultiplier(Button);
        }
    }
    private void SetNoMultiplier(GameObject Button)
    {
        //Button.transform.Find("Image").GetComponent<Image>().sprite = null;
        Button.transform.Find("Image").GetComponent<Image>().color = Color.clear;
        Button.transform.Find("Text").GetComponent<Text>().text = "";
    }
}
