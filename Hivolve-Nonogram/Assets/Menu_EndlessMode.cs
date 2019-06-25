using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Enums;

public class Menu_EndlessMode : MonoBehaviour
{
    public GameObject UI;

    public int Size;
    public Text IndexText;
    public Dropdown SmallStarDensity;
    public Dropdown BigStarDensity;
    public Dropdown BlackHoleDensity;
    public Dropdown MultiplierCount_2X;
    public Dropdown MultiplierCount_3X;

    private int indexNumber;
    private readonly int minSize = 3;
    private readonly int maxSize = 10;

    public void Init()
    {
        UI.SetActive(true);

        string[] density = Enum.GetNames(typeof(Density));
        string[] count = Enum.GetNames(typeof(Count));
        List<string> densityNames = new List<string>(density);
        List<string> countNames = new List<string>(count);

        //----- Small Stars
        SmallStarDensity.ClearOptions();
        SmallStarDensity.AddOptions(densityNames);
        //prevent 0 star games
        SmallStarDensity.options.RemoveAt(0);

        //----- Big Stars
        BigStarDensity.ClearOptions();
        BigStarDensity.AddOptions(densityNames);

        //----- Black Holes
        BlackHoleDensity.ClearOptions();
        BlackHoleDensity.AddOptions(densityNames);
        //----- Multiplier 2X
        MultiplierCount_2X.ClearOptions();
        MultiplierCount_2X.AddOptions(countNames);

        //----- Multiplier 3X
        MultiplierCount_3X.ClearOptions();
        MultiplierCount_3X.AddOptions(countNames);

        indexNumber = 3;
        IndexText.text = indexNumber.ToString();
    }

    public void PlusOne()
    {
        if(indexNumber + 1 <= maxSize)
        {
            indexNumber += 1;
            IndexText.text = indexNumber.ToString();
        }
    }
    public void MinusOne()
    {
        if (indexNumber - 1 >= minSize)
        {
            indexNumber -= 1;
            IndexText.text = indexNumber.ToString();
        }
    }

    public void ExitEndlessMode() //BUTTON
    {
        UI.SetActive(false);
        MenuManager.Instance.OpenMenuUI();
    }

    public void StartEndlessMode() //BUTTON
    {
        Density smallStar  = (Density)System.Enum.Parse(typeof(Density), SmallStarDensity.options[SmallStarDensity.value].text);
        Density bigStar    = (Density)System.Enum.Parse(typeof(Density), BigStarDensity.options[BigStarDensity.value].text);
        Density blackHoles = (Density)System.Enum.Parse(typeof(Density), BlackHoleDensity.options[BlackHoleDensity.value].text);
        Count multiplier2X = (Count)System.Enum.Parse(typeof(Count), MultiplierCount_3X.options[MultiplierCount_2X.value].text);
        Count multiplier3X = (Count)System.Enum.Parse(typeof(Count), MultiplierCount_3X.options[MultiplierCount_3X.value].text);

        PropertiesManager.Instance.SetCustomProperties(
            indexNumber,
            smallStar,
            bigStar,
            blackHoles,
            multiplier2X,
            multiplier3X
            );


        PropertiesManager.Instance.GameMode = GameMode.Endless;
        SceneManager.LoadScene("Game");
    }
}
