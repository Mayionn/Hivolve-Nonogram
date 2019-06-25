using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Enums;

public class Menu_EndlessMode : MonoBehaviour
{
    public GameObject UI;
    public GameObject Multiplier2XGameObject;
    public GameObject Multiplier3XGameObject;
    public int Size;
    public Text IndexText;
    public Text RewardAmount;
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
        SmallStarDensity.RefreshShownValue();

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

        //----- Indexes
        indexNumber = 3;
        IndexText.text = indexNumber.ToString();

        //----- Creating Listeners
        SmallStarDensity.onValueChanged.AddListener(delegate 
        {
            SetCustomPropertiesValues();
            UpdateRewardAmount();
        });
        BigStarDensity.onValueChanged.AddListener(delegate 
        {
            SetCustomPropertiesValues();
            UpdateRewardAmount();
        });
        BlackHoleDensity.onValueChanged.AddListener(delegate
        {
            SetCustomPropertiesValues();
            UpdateRewardAmount();
        });
        MultiplierCount_2X.onValueChanged.AddListener(delegate
        {
            SetCustomPropertiesValues();
            UpdateRewardAmount();
        });
        MultiplierCount_3X.onValueChanged.AddListener(delegate
        {
            SetCustomPropertiesValues();
            UpdateRewardAmount();
        });

        SetCustomPropertiesValues();
        UpdateRewardAmount();
        UpdateDropdowns();
    }

    public void PlusOne()  //BUTTON
    {
        if (indexNumber + 1 <= maxSize)
        {
            indexNumber += 1;
            IndexText.text = indexNumber.ToString();
            SetCustomPropertiesValues();
            UpdateRewardAmount();
            UpdateDropdowns();
        }
    }
    public void MinusOne() //BUTTON
    {
        if (indexNumber - 1 >= minSize)
        {
            indexNumber -= 1;
            IndexText.text = indexNumber.ToString();
            SetCustomPropertiesValues();
            UpdateRewardAmount();
            UpdateDropdowns();
        }
    }

    public void ExitEndlessMode()  //BUTTON
    {
        UI.SetActive(false);
        UIManager.Instance.OpenMenuUI();
    }
    public void StartEndlessMode() //BUTTON
    {
        SetCustomPropertiesValues();

        PropertiesManager.Instance.GameMode = GameMode.Endless;
        SceneManager.LoadScene("Game");
    }

    private void SetCustomPropertiesValues()
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
    }
    private void UpdateRewardAmount()
    {
        RewardAmount.text = PropertiesManager.Instance.GetCustomReward().ToString();
    }
    private void UpdateDropdowns()
    {
        //----- Thresholds for dropdown control
        int tier1 = 3;
        int tier2 = 5;
        int tier3 = 7;

        string[] count = Enum.GetNames(typeof(Count));
        List<string> countNames = new List<string>(count);

        MultiplierCount_3X.value = 0;
        MultiplierCount_2X.value = 0;
        Multiplier2XGameObject.SetActive(true);
        Multiplier3XGameObject.SetActive(true);

        //----- Control the amout of choices on dropdown multipliers
        if (indexNumber == tier1)
        {
            Multiplier2XGameObject.SetActive(false);
            Multiplier3XGameObject.SetActive(false);
        }
        else if (indexNumber <= tier2)
        {
            countNames.RemoveRange(2, countNames.Count - 2);
        }
        else if (indexNumber <= tier3)
        {
            countNames.RemoveRange(3, countNames.Count - 3);
        }

        //----- Update dropdowns
        MultiplierCount_2X.ClearOptions();
        MultiplierCount_2X.AddOptions(countNames);
        MultiplierCount_3X.ClearOptions();
        MultiplierCount_3X.AddOptions(countNames);
    }
}