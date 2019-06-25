using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class TimeAttack : Singleton<TimeAttack>
{
    public int CurrentStage;
    public int CurrentGame;
    public Dificulty Dificulty;
    public Timer timer;
    public int CurrencyEarned = 0;


    private int levelsPerStage = 2;
    private int maxStage = 8;
    private int timePerCompletion = 10;


    public void Init()
    {
        CurrentStage = 1;
        CurrentGame = 1;

        timer = GetComponent<Timer>();
        timer.Text = UIManager.Instance.GetTimer();
        timer.Begin(5f);
    }

    public void NextMap()
    {
        if(CurrentGame + 1 > levelsPerStage)
        {
            if(CurrentStage + 1 > maxStage)
            {
                Debug.Log("Won");
                //TODO: GAME WON
            }
            else
            {
                CurrentStage += 1;
                CurrentGame = 1;
            }
        }
        else
        {
            CurrentGame += 1;
        }
    }
}
