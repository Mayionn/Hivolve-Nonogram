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

    public Sprite unfilled;         //Sprite changes to unfilled if not selected
    public Sprite filled;           //Sprite changes to filled if selected

    private int selectedButton;
    private Dificulty selectedDificulty;

    public void Init()
    {
        UI.SetActive(true);

        selectedDificulty = Dificulty.VeryEasy;
        VeryEasy.image.sprite = filled;
        selectedButton = 0;
    }

    public void ButtonPressed(int index)
    {
        if(index != selectedButton)
        {
            switch (selectedButton)
            {
                case 0:
                    VeryEasy.image.sprite = unfilled;
                    break;
                case 1:
                    Easy.image.sprite = unfilled;
                    break;
                case 2:
                    Medium.image.sprite = unfilled;
                    break;
                case 3:
                    Hard.image.sprite = unfilled;
                    break;
                case 4:
                    VeryHard.image.sprite = unfilled;
                    break;
                default:
                    break;
            }
            switch (index)
            {
                case 0:
                    selectedButton = 0;
                    VeryEasy.image.sprite = filled;
                    selectedDificulty = Dificulty.VeryEasy;
                    break;
                case 1:
                    selectedButton = 1;
                    Easy.image.sprite = filled;
                    selectedDificulty = Dificulty.Easy;
                    break;
                case 2:
                    selectedButton = 2;
                    Medium.image.sprite = filled;
                    selectedDificulty = Dificulty.Medium;
                    break;
                case 3:
                    selectedButton = 3;
                    Hard.image.sprite = filled;
                    selectedDificulty = Dificulty.Hard;
                    break;
                case 4:
                    selectedButton = 4;
                    VeryHard.image.sprite = filled;
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
        PropertiesManager.Instance.gameObject.GetComponent<TimeAttack>().Dificulty = selectedDificulty;
        PropertiesManager.Instance.GameType = GameType.TimeAttack;
        SceneManager.LoadScene("Game");
    }

}
