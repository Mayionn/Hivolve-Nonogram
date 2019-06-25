using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Enums;

public class Menu_TimeAttack : MonoBehaviour
{
    public GameObject UI;

    public Button VeryEasy;
    public Button Easy;
    public Button Medium;
    public Button Hard;
    public Button VeryHard;

    private int selectedButton;
    private Dificulty selectedDificulty;

    public void Init()
    {
        UI.SetActive(true);

        selectedDificulty = Dificulty.VeryEasy;
        VeryEasy.image.color = new Color(1, 0, 0);
        selectedButton = 0;

    }

    public void ButtonPressed(int index)
    {
        if(index != selectedButton)
        {
            switch (selectedButton)
            {
                case 0:
                    VeryEasy.image.color = new Color(1, 1, 1);
                    break;
                case 1:
                    Easy.image.color = new Color(1, 1, 1);
                    break;
                case 2:
                    Medium.image.color = new Color(1, 1, 1);
                    break;
                case 3:
                    Hard.image.color = new Color(1, 1, 1);
                    break;
                case 4:
                    VeryHard.image.color = new Color(1, 1, 1);
                    break;
                default:
                    break;
            }
            switch (index)
            {
                case 0:
                    selectedButton = 0;
                    VeryEasy.image.color = new Color(1, 0, 0);
                    selectedDificulty = Dificulty.VeryEasy;
                    break;
                case 1:
                    selectedButton = 1;
                    Easy.image.color = new Color(1, 0, 0);
                    selectedDificulty = Dificulty.Easy;
                    break;
                case 2:
                    selectedButton = 2;
                    Medium.image.color = new Color(1, 0, 0);
                    selectedDificulty = Dificulty.Medium;
                    break;
                case 3:
                    selectedButton = 3;
                    Hard.image.color = new Color(1, 0, 0);
                    selectedDificulty = Dificulty.Hard;
                    break;
                case 4:
                    selectedButton = 4;
                    VeryHard.image.color = new Color(1, 0, 0);
                    selectedDificulty = Dificulty.VeryHard;
                    break;
                default:
                    break;
            }
        }
    }

    public void ExitTimeAttackMode()  //BUTTON
    {
        UI.SetActive(false);
        UIManager.Instance.OpenMenuUI();
    }
    public void StartTimeAttackMode() //BUTTON
    {
        TimeAttack.Instance.Dificulty = selectedDificulty;
        PropertiesManager.Instance.GameMode = GameMode.TimeAttack;
        SceneManager.LoadScene("Game");
    }

}
