using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;
using static Structs;

public class TimeAttack : GameMode
{
    public int CurrentStage;
    public int CurrentGame;
    public Dificulty Dificulty;

    private InfoTimeAttack UI;
    private float time;
    private Action actionTimer;

    private int currencyEarned;
    private readonly int LEVELSPERSTAGE = 2;
    private readonly int MAXSTAGE = 8;
    private readonly int TIMEPERCOMPLETION = 8;


    public override void Init()
    {
        CurrentStage = 1;
        CurrentGame = 1;
        currencyEarned = 0;

        Properties = PropertiesManager.Instance.GetTimeAttackProperties();

        UI = UIManager.Instance.GetTimeAttackInfo();
        //UI.TextCurrency.text = "0";

        actionTimer += CheckTime;
        //----- Start Timer
        Begin(20f);
    }

    public override void SetMapProperties()
    {
        NextMap();
        Properties = PropertiesManager.Instance.GetTimeAttackProperties();
    }

    public override void LeaveGame()
    {
        Stop();
        actionTimer -= CheckTime;
        actionTimer -= Decrement;
        ProfileManager.Instance.AddCurrency(currencyEarned);
    }

    public override IEnumerator MapCompletion()
    {
        //----- Stop timer:
        Stop();
        //----- Increment time:
        AddTime(TIMEPERCOMPLETION * CurrentStage);
        //----- Play Increment time animation:
        UI.TextTimerIncrement.GetComponent<Animation>().Play("TimeIncrement");
        //----- Play Increment currency animation:
        //UI.TextCurrencyIncrement.text = "+" + PropertiesManager.Instance.GetGameReward(Properties).ToString();
        //UI.TextCurrencyIncrement.GetComponent<Animation>().Play("CurrencyIncrement");
        //Fadeout all Sqares
        GameManager.Instance.FadeOutUI();

        //---------- Wait 1.5 seconds ----------//
        yield return new WaitForSeconds(1.5f);


        //----- Add currency:
        currencyEarned += PropertiesManager.Instance.GetGameReward(Properties);
        //UI.TextCurrency.text = currencyEarned.ToString();
        //----- Resume Timer:
        Resume();
        //----- StartNextMap:
        GameManager.Instance.GenerateNextMap();
    }

    private void Update()
    {
        actionTimer?.Invoke();
    }


    public IEnumerator MapFailed()
    {
        GameManager.Instance.DisableAllButtons();

        ProfileManager.Instance.AddCurrency(currencyEarned);
        Stop();
        actionTimer -= CheckTime;

        //Play Failed Sound

        yield return new WaitForSeconds(2f);

        GameManager.Instance.Button_LeaveGame();
    }

    private void Begin(float time)
    {
        this.time = time;
        this.actionTimer += Decrement;
    }
    private void AddTime(int increment)
    {
        this.UI.TextTimerIncrement.text = string.Format(
                                     "+{0:#0} : {1:00} : {2:00}",
                                     Mathf.Floor(increment / 60),
                                     Mathf.Floor(increment) % 60,
                                     Mathf.Floor((increment * 100) % 100)
                                     );
        this.time += increment;
    }
    private void Stop()
    {
        this.actionTimer -= Decrement;
    }
    private void Resume()
    {
        this.actionTimer += Decrement;
    }
    private void Decrement()
    {
        this.time -= Time.deltaTime;
        if (time < 0) time = 0;
        UpdateTime();
    }

    private void UpdateTime()
    {
        this.UI.TextTimer.text = string.Format(
                                        "{0:#0} : {1:00} : {2:00}",
                                        Mathf.Floor(time / 60),
                                        Mathf.Floor(time) % 60,
                                        Mathf.Floor((time * 100) % 100)
                                        );
    }

    private void CheckTime()
    {
        if(time <= 0)
        {
            StartCoroutine(MapFailed());
        }
    }

    private void NextMap()
    {
        if(CurrentGame + 1 > LEVELSPERSTAGE)
        {
            if(CurrentStage + 1 > MAXSTAGE)
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
